using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTransport
{
    public interface IClient
    {
        event EventHandler<ByteEventArgs> OnBytes;
        event EventHandler<EventArgs> OnDisconnect;
    }
}
