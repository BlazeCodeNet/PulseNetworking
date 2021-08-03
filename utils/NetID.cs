using System;
using System.Collections.Generic;
using System.Text;

namespace PulseNet.utils
{
    public class NetID
    {
        public override string ToString()
        {
            return rawNetID;
        }

        public bool Equals( NetID input )
        {
            return (input.ToString().Equals(this.ToString()));
        }

        public NetID()
        {
            rawNetID = GenerateString( SIZE );
        }
        public NetID(string presetID)
        {
            if(presetID.Length == SIZE )
            {
                rawNetID = presetID;
            }
            else
            {
                rawNetID = GenerateString( SIZE );
            }
        }

        string rawNetID;

        private string GenerateString( int size )
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789#";
            char[] stringChars = new char[ size ];

            Random rand = Protocol.rand;

            for ( int i = 0; i < stringChars.Length; i++ )
            {
                stringChars[ i ] = chars[ rand.Next( chars.Length ) ];
            }

            return new String( stringChars );
        }

        public const int SIZE = 16;
    }
}
