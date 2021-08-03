using System.Text;
using System;
using PulseNet.utils;
using System.Collections.Generic;

namespace PulseNet.packet
{
    public class PulsePacket
    {
        public PulsePacket( int fullBufferLength = -1 )
        {
            if ( fullBufferLength > 0 )
            {
                // Un-full buffer given.
                maxSize = -1;
            }
            else
            {
                // Complete buffer given.
                maxSize = fullBufferLength;
            }

            buffer = new List<byte>( );
        }

        public void Write( byte[ ] msg )
        {
            if ( msg.Length + buffer.Count > maxSize && maxSize > 0 )
            {
                throw new Exception( "PulseNet.PacketOverfillWriteException!" );
            }

            buffer.AddRange( msg );
        }
        public void Write( List<byte> msg )
        {
            if ( msg.Count + buffer.Count > maxSize && maxSize > 0 )
            {
                throw new Exception( "PulseNet.PacketOverfillWriteException!" );
            }

            buffer.AddRange( msg );
        }
        public void Write( bool msg )
        {
            Write( BitConverter.GetBytes( msg ) );
        }
        public void Write( short msg )
        {
            Write( BitConverter.GetBytes( msg ) );
        }
        public void Write( ushort msg )
        {
            Write( BitConverter.GetBytes( msg ) );
        }
        public void Write( int msg )
        {
            Write( BitConverter.GetBytes( msg ) );
        }
        public void Write( uint msg )
        {
            Write( BitConverter.GetBytes( msg ) );
        }
        public void Write( long msg )
        {
            Write( BitConverter.GetBytes( msg ) );
        }
        public void Write( ulong msg )
        {
            Write( BitConverter.GetBytes( msg ) );
        }
        public void Write( float msg )
        {
            Write( BitConverter.GetBytes( msg ) );
        }
        public void Write( string msg )
        {
            byte[ ] stringBuff = Encoding.Unicode.GetBytes( msg );
            byte[ ] msgLenBuff = BitConverter.GetBytes( stringBuff.Length );
            byte[ ] totalBuff = new byte[ stringBuff.Length + msgLenBuff.Length ];

            Array.Copy( msgLenBuff, 0, totalBuff, 0, msgLenBuff.Length );
            Array.Copy( stringBuff, 0, totalBuff, msgLenBuff.Length, stringBuff.Length );

            Write( totalBuff );
        }
        public void Write( Vec3f msg )
        {
            if ( msg == null )
                return;

            byte[ ] xBuff = BitConverter.GetBytes( msg.x );
            byte[ ] yBuff = BitConverter.GetBytes( msg.y );
            byte[ ] zBuff = BitConverter.GetBytes( msg.z );

            byte[ ] totalBuff = new byte[ xBuff.Length + yBuff.Length + zBuff.Length ];

            Array.Copy( xBuff, 0, totalBuff, 0, xBuff.Length );
            Array.Copy( yBuff, 0, totalBuff, xBuff.Length, yBuff.Length );
            Array.Copy( zBuff, 0, totalBuff, xBuff.Length + zBuff.Length, zBuff.Length );

            Write( totalBuff );
        }
        public void Write( Vec3i msg )
        {
            if ( msg == null )
                return;

            byte[ ] xBuff = BitConverter.GetBytes( msg.x );
            byte[ ] yBuff = BitConverter.GetBytes( msg.y );
            byte[ ] zBuff = BitConverter.GetBytes( msg.z );

            byte[ ] totalBuff = new byte[ xBuff.Length + yBuff.Length + zBuff.Length ];

            Array.Copy( xBuff, 0, totalBuff, 0, xBuff.Length );
            Array.Copy( yBuff, 0, totalBuff, xBuff.Length, yBuff.Length );
            Array.Copy( zBuff, 0, totalBuff, xBuff.Length + zBuff.Length, zBuff.Length );

            Write( totalBuff );
        }

        public bool ReadBool( )
        {
            if ( curReadPos + (int)Protocol.Sizes.Byte > buffer.Count )
            {
                throw new Exception( "PulseNet.PacketOverfillReadException!" );
            }

            byte[ ] readBuff = buffer.GetRange( curReadPos, (int)Protocol.Sizes.Byte ).ToArray( );

            bool ret = BitConverter.ToBoolean( readBuff, 0 );

            curReadPos += (int)Protocol.Sizes.Byte;

            return ret;
        }
        public ushort ReadUShort( )
        {
            if ( curReadPos + (int)Protocol.Sizes.Short > buffer.Count )
            {
                throw new Exception( "PulseNet.PacketOverfillReadException!" );
            }

            byte[ ] readBuff = buffer.GetRange( curReadPos, (int)Protocol.Sizes.Short ).ToArray( );

            ushort ret = BitConverter.ToUInt16( readBuff, 0 );

            curReadPos += (int)Protocol.Sizes.Short;

            return ret;
        }
        public short ReadShort( )
        {
            if ( curReadPos + (int)Protocol.Sizes.Short > buffer.Count )
            {
                throw new Exception( "PulseNet.PacketOverfillReadException!" );
            }

            byte[ ] readBuff = buffer.GetRange( curReadPos, (int)Protocol.Sizes.Short ).ToArray( );

            short ret = BitConverter.ToInt16( readBuff, 0 );

            curReadPos += (int)Protocol.Sizes.Short;

            return ret;
        }
        public uint ReadUInt( )
        {
            if ( curReadPos + (int)Protocol.Sizes.Normal > buffer.Count )
            {
                throw new Exception( "PulseNet.PacketOverfillReadException!" );
            }

            byte[ ] readBuff = buffer.GetRange( curReadPos, (int)Protocol.Sizes.Normal ).ToArray( );

            uint ret = BitConverter.ToUInt32( readBuff, 0 );

            curReadPos += (int)Protocol.Sizes.Normal;

            return ret;
        }
        public int ReadInt( )
        {
            if ( curReadPos + (int)Protocol.Sizes.Normal > buffer.Count )
            {
                throw new Exception( "PulseNet.PacketOverfillReadException!" );
            }

            byte[ ] readBuff = buffer.GetRange( curReadPos, (int)Protocol.Sizes.Normal ).ToArray( );

            int ret = BitConverter.ToInt32( readBuff, 0 );

            curReadPos += (int)Protocol.Sizes.Normal;

            return ret;
        }
        public ulong ReadULong( )
        {
            if ( curReadPos + (int)Protocol.Sizes.Long > buffer.Count )
            {
                throw new Exception( "PulseNet.PacketOverfillReadException!" );
            }

            byte[ ] readBuff = buffer.GetRange( curReadPos, (int)Protocol.Sizes.Long ).ToArray( );

            ulong ret = BitConverter.ToUInt64( readBuff, 0 );

            curReadPos += (int)Protocol.Sizes.Long;

            return ret;
        }
        public long ReadLong( )
        {
            if ( curReadPos + (int)Protocol.Sizes.Long > buffer.Count )
            {
                throw new Exception( "PulseNet.PacketOverfillReadException!" );
            }

            byte[ ] readBuff = buffer.GetRange( curReadPos, (int)Protocol.Sizes.Long ).ToArray( );

            long ret = BitConverter.ToInt64( readBuff, 0 );

            curReadPos += (int)Protocol.Sizes.Long;

            return ret;
        }
        public float ReadFloat( )
        {
            if ( curReadPos + (int)Protocol.Sizes.Normal > buffer.Count )
            {
                throw new Exception( "PulseNet.PacketOverfillReadException!" );
            }

            byte[ ] readBuff = buffer.GetRange( curReadPos, (int)Protocol.Sizes.Normal ).ToArray( );

            float ret = BitConverter.ToSingle( readBuff, 0 );

            curReadPos += (int)Protocol.Sizes.Normal;

            return ret;
        }
        public string ReadString( )
        {
            if ( curReadPos + (int)Protocol.Sizes.Normal > buffer.Count )
            {
                throw new Exception( "PulseNet.PacketOverfillReadException!" );
            }

            byte[ ] readBuff = buffer.GetRange( curReadPos, (int)Protocol.Sizes.Normal ).ToArray( );

            curReadPos += (int)Protocol.Sizes.Normal;

            int readLength = BitConverter.ToInt32( readBuff, 0 );

            readBuff = buffer.GetRange( curReadPos, readLength ).ToArray( );
            string ret = Encoding.Unicode.GetString( readBuff, 0, readLength );

            curReadPos += readLength;

            return ret;
        }
        public Vec3f ReadVec3f( )
        {
            if ( curReadPos + ( (int)Protocol.Sizes.Normal * 3 ) > buffer.Count )
            {
                throw new Exception( "PulseNet.PacketOverfillReadException!" );
            }

            byte[ ] readBuff = buffer.GetRange( curReadPos, (int)Protocol.Sizes.Normal * 3 ).ToArray( );
            curReadPos += ( (int)Protocol.Sizes.Normal * 3 );

            int curPos = 0;

            float xRet = BitConverter.ToSingle( readBuff, curPos );
            curPos += (int)Protocol.Sizes.Normal;

            float yRet = BitConverter.ToSingle( readBuff, curPos );
            curPos += (int)Protocol.Sizes.Normal;

            float zRet = BitConverter.ToSingle( readBuff, curPos );

            return new Vec3f( xRet, yRet, zRet );
        }
        public Vec3i ReadVec3i( )
        {
            if ( curReadPos + ( (int)Protocol.Sizes.Normal * 3 ) > buffer.Count )
            {
                throw new Exception( "PulseNet.PacketOverfillReadException!" );
            }

            byte[ ] readBuff = buffer.GetRange( curReadPos, (int)Protocol.Sizes.Normal * 3 ).ToArray( );
            curReadPos += ( (int)Protocol.Sizes.Normal * 3 );

            int curPos = 0;

            int xRet = BitConverter.ToInt32( readBuff, curPos );
            curPos += (int)Protocol.Sizes.Normal;

            int yRet = BitConverter.ToInt32( readBuff, curPos );
            curPos += (int)Protocol.Sizes.Normal;

            int zRet = BitConverter.ToInt32( readBuff, curPos );

            return new Vec3i( xRet, yRet, zRet );
        }

        /// <summary>
        /// Grabs the direct reference to the buffer used by this Packet. WARNING: Dangerous! Dont use!!
        /// </summary>
        /// <returns></returns>
        public List<byte> GetDirectBuffer( )
        {
            return buffer;
        }


        public void ResetReadPosition( )
        {
            curReadPos = 0;
        }

        public int GetMaxSize( )
        {
            return maxSize;
        }
        public int GetCurrentSize( )
        {
            return buffer.Count;
        }

        private List<byte> buffer;
        private int maxSize = 0;
        private int curReadPos = 0;

        public virtual ushort? packetID
        {
            get
            {
                return null;
            }
        }
    }
}