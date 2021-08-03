using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using PulseNet.Core.Enums;
using PulseNet.Core.Threading;
using System;

namespace PulseNet.Core
{
    public class PulseServer
    {

        public PulseServer( int port )
        {
            this.port = port;
        }

        private void AcceptClientsInit()
        {
            this.tcpSocketListener.BeginAccept( BeginAcceptTCPCallback, null );
        }

        private void BeginAcceptTCPCallback( IAsyncResult ar )
        {
            if(state <= RunState.STOPPING)
            {
                return;
            }
            Socket cl = this.tcpSocketListener.EndAccept( ar );
            
            PulseClient p = new PulseClient( cl );
            lock (clients)
            {
                clients.Add( p );
            }

            OnClientConnected?.Invoke( p );

            if ( state > RunState.STOPPING )
            {
                this.tcpSocketListener.BeginAccept( BeginAcceptTCPCallback, null );
            }
        }

        public void Start()
        {
            state = RunState.STARTED;

            this.tcpSocketListener = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
            this.tcpSocketListener.NoDelay = true;
            this.tcpSocketListener.Bind( new IPEndPoint( IPAddress.Any, port ) );
            this.tcpSocketListener.Listen( 10 );

            AcceptClientsInit( );
        }
        public void Stop( )
        {
            if ( state > RunState.STOPPING )
            {
                state = RunState.STOPPED;
                tcpSocketListener.Dispose( );
                clients.Clear( );
            }
        }

        private Socket tcpSocketListener;
        private List<PulseClient> clients = new List<PulseClient>();

        public int port { get; private set; }
        public RunState state { get; private set; } = RunState.STOPPED;

        public delegate void ClientConnectEvent( PulseClient client );
        public event ClientConnectEvent OnClientConnected;

        public delegate void ClientDisconnectEvent( PulseClient client );
        public event ClientDisconnectEvent OnClientDisconnected;

    }
}