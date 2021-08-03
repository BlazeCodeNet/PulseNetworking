using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using PulseNet.Core.Enums;
using PulseNet.packet;

namespace PulseNet.Core
{
    public class PulseClient
    {

        private void ProcessBufferLoop()
        {
            while(this.state != LifeState.DEAD)
            {
                int count = this.packetStreamBuff.Count;

                if(count >= 0)
                {
                    if (pendingPacketSize > 0)
                    {
                        if(count >= pendingPacketSize)
                        {
                            byte[ ] pBuff;
                            lock(this.packetStreamBuff)
                            {
                                pBuff = this.packetStreamBuff.GetRange( 0, (int)this.pendingPacketSize ).ToArray( );
                                this.packetStreamBuff.RemoveRange( 0, (int)this.pendingPacketSize );
                            }
                            PulsePacket tmp = new PulsePacket( pBuff.Length );
                            tmp.Write( pBuff );
                            ushort packCMD = tmp.ReadUShort( );
                            PulseHandler handler = null;
                            if(Protocol.debugMode)
                                Protocol.PushLog( "Handling:" + this.pendingPacketSize + ";" + pBuff.Length );
                            if(isCustomHandled)
                            {
                                OnPacketRecieved?.Invoke( tmp, this );
                            }
                            else
                            {
                                if ( handlerRegistry.TryGetValue( packCMD, out handler ) )
                                {
                                    handler.Handle( tmp, this );
                                }
                            }

                            this.pendingPacketSize = -1;
                        }
                    }
                    else
                    {
                        if ( count >= (int)Protocol.Sizes.Normal )
                        {
                            byte[ ] val = new byte[ (int)Protocol.Sizes.Normal ];
                            lock(this.packetStreamBuff)
                            {
                                val = this.packetStreamBuff.GetRange( 0, (int)Protocol.Sizes.Normal ).ToArray( );
                                this.packetStreamBuff.RemoveRange( 0, (int)Protocol.Sizes.Normal );
                            }

                            int packetContainedSize = BitConverter.ToInt32( val, 0 );
                            this.pendingPacketSize = packetContainedSize;
                            continue;
                        }
                    }
                }
                else
                {
                    Disconnect( );
                }


                Thread.Sleep( 18 );
            }
        }

        public void Connect()
        {
            if( this.tcpSocket == null )
            {
                this.state = LifeState.INACTIVE;
                try
                {
                    this.tcpSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
                    this.tcpSocket.NoDelay = true;
                    this.tcpSocket.BeginConnect( IPAddress.Parse( this.serverIP ), this.serverPort, BeginConnectCallback, null );
                }
                catch(Exception ex)
                {
                    this.state = LifeState.INACTIVE;
                    this.OnConnectionClosed?.Invoke( Protocol.DisconnectReasons.Unknown );
                    Protocol.PushLog( "unable to connect..." + Environment.NewLine + "EX=" + ex.ToString( ) );
                }
            }
        }
        public void Disconnect()
        {
            OnConnectionClosed?.Invoke( Protocol.DisconnectReasons.Unknown );
            if( tcpSocket!=null )
            {
                tcpSocket.Dispose( );
            }
        }

        public void SendPacket(PulsePacket packet)
        {
            if( state != LifeState.DEAD && packet!=null )
            {
                try
                {
                    byte[ ] buff = packet.GetDirectBuffer( ).ToArray();
                    byte[ ] totalBuff = new byte[ buff.Length + (int)Protocol.Sizes.Normal ];
                    byte[ ] sizePrefixBuff = BitConverter.GetBytes( buff.Length );

                    Array.Copy( sizePrefixBuff, 0, totalBuff, 0, sizePrefixBuff.Length );
                    Array.Copy( buff, 0, totalBuff, sizePrefixBuff.Length, buff.Length );

                    if(!BitConverter.IsLittleEndian)
                    { 
                        Array.Reverse( totalBuff );
                    }

                    tcpSocket.BeginSend( totalBuff, 0, totalBuff.Length, SocketFlags.None, BeginSendCallback, null );

                    if ( Protocol.debugMode )
                        Protocol.PushLog( "SENT '" + buff.Length + "' bytes!" );
                }
                catch(Exception ex)
                {
                    // Protocol.PushLog( "Error:" + ex.ToString( ) );
                    Disconnect( );
                }

            }
        }

        private void BeginSendCallback( IAsyncResult ar )
        {
            tcpSocket.EndSend( ar );
        }

        private void BeginConnectCallback(IAsyncResult ar)
        {
            this.tcpSocket.EndConnect( ar );

            tcpBuffer = new byte[ Protocol.DEFAULT_BUFF_SIZE ];

            OnConnectionEstablished?.Invoke( );

            this.state = LifeState.ALIVE;

            processBytesThread = new Thread( ProcessBufferLoop );
            processBytesThread.IsBackground = true;
            processBytesThread.Start( );

            BeginReadTCP( );
        }

        public void BeginReadTCP()
        {
            try
            {
                this.tcpBuffer = new byte[ Protocol.DEFAULT_BUFF_SIZE ];
                this.tcpSocket.BeginReceive( tcpBuffer, 0, tcpBuffer.Length, SocketFlags.None, ReadDataCallback, null );
            }
            catch(Exception ex)
            {
                Protocol.PushLog( "PulseClient.StartReading().BeginRead() error:" + Environment.NewLine + ex.ToString( ) );
            }
        }

        private void ReadDataCallback(IAsyncResult ar)
        {
            int bytesRead = this.tcpSocket.EndReceive(ar);

            // Protocol.PushLog( "BytesRead=" + bytesRead );

            if( bytesRead <= 0 )
            {
                Disconnect( );
                return;
            }

            byte[ ] arr = new byte[ bytesRead ];
            Array.Copy( this.tcpBuffer, 0, arr, 0, bytesRead );

            if ( !BitConverter.IsLittleEndian )
            {
                Array.Reverse( arr );
            }

            lock (this.packetStreamBuff)
            {
                this.packetStreamBuff.AddRange( arr );
            }

            BeginReadTCP( );
        }

        public void RegisterHandler(ushort handlerID, PulseHandler handlerInstance)
        {
            if( !handlerRegistry.ContainsKey( handlerID ) )
            {
                if( handlerInstance != null )
                {
                    handlerRegistry.Add( handlerID, handlerInstance );
                }
            }
            else
            {
                throw new Exception( "Tried to register a PulseHandler twice with the same ID:" + handlerID );
            }
        }

        public PulseClient( Socket connectedClient )
        {
            state = LifeState.ALIVE;

            tcpBuffer = new byte[ Protocol.DEFAULT_BUFF_SIZE ];
            tcpSocket = connectedClient;

            processBytesThread = new Thread( ProcessBufferLoop );
            processBytesThread.IsBackground = true;
            processBytesThread.Start( );

            BeginReadTCP( );
        }
        public PulseClient( string serverIP, int port )
        {
            this.serverPort = port;
            this.serverIP = serverIP;
        }

        public void EnableCustomHandled()
        {
            isCustomHandled = true;
        }

        private byte[ ] tcpBuffer;
        private List<byte> packetStreamBuff = new List<byte>();
        private int pendingPacketSize = -1;

        private Thread processBytesThread;
        private Socket tcpSocket;

        public int serverPort { get; private set; }
        public string serverIP { get; private set; }

        public bool isCustomHandled { get; private set; } = false;

        public LifeState state { get; private set; } = LifeState.NOT_SET;

        public Dictionary<ushort, PulseHandler> handlerRegistry = new Dictionary<ushort, PulseHandler>( );
        
        public delegate void ConnectionEstablishedEvent( );
        public event ConnectionEstablishedEvent OnConnectionEstablished;

        public delegate void ConnectionClosedEvent( Protocol.DisconnectReasons reason );
        public event ConnectionClosedEvent OnConnectionClosed;

        public delegate void PacketRecievedEvent( PulsePacket packet, PulseClient src );
        public event PacketRecievedEvent OnPacketRecieved;
    }
}