using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SoundTransport
{
    public class ClientTcp : IClient
    {
        public ClientTcp(String host, Int32 port)
        {
            IPAddress ipAddr = IPAddress.Parse(host);

            tcpClient = new TcpClient();
            tcpClient.NoDelay = true;
            tcpClient.ReceiveBufferSize = 64 * 1024;
            tcpClient.SendBufferSize = 64 * 1024;
            tcpClient.NoDelay = true;
            tcpClient.Connect(new IPEndPoint(ipAddr, port));

            tcpThread = new Thread(tcpThreadFn);
            tcpThread.Start();
        }


        public event EventHandler<ByteEventArgs> OnBytes = delegate { };
        public event EventHandler<EventArgs> OnDisconnect = delegate { };

        private TcpClient tcpClient;
        private Thread tcpThread;


        public void Stop()
        {
            try
            {
                tcpClient.Close();
            }
            catch (Exception e)
            {
            }

            try
            {
                tcpThread.Interrupt();
            }
            catch (Exception e)
            {
            }

            this.OnDisconnect(this, new EventArgs());
        }

        private void tcpThreadFn()
        {
            NetworkStream stream = tcpClient.GetStream();
            
            byte[] buf = new byte[1024 * 1024];
            try
            {
                while (true)
                {
                    int total = stream.Read(buf, 0, buf.Length);
                    if (total > 0)
                    {
                        byte[] bytes = new byte[total];
                        Array.Copy(buf, bytes, total);
                        this.OnBytes(this, new ByteEventArgs(bytes));
                    }
                    Thread.Sleep(0);
                }
            }
            catch (Exception e)
            {
                Stop();
            }
        }
    }
}
