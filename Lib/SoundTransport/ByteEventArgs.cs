using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTransport
{
    public class ByteEventArgs : EventArgs
    {
        public ByteEventArgs(byte[] bytes)
        {
            this.bytes = bytes;
        }


        private byte[] bytes;


        public byte[] Bytes
        {
            get { return this.bytes; }
        }
    }
}
