using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTransport
{
    public interface IServer
    {
        void Stop();
        void BroadcastBytes(byte[] bytes);
    }
}
