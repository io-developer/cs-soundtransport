using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SoundTransport
{
    public class ServerTcp : IServer
    {
        public ServerTcp(UInt16 port)
        {
            this.port = port;
            this.threadMain = new Thread(threadMainFn);
            this.threadMain.Start();
        }


        private UInt16 port;
        private TcpListener tcpListener;
        private List<TcpClient> tcpClients = new List<TcpClient>();
        private Thread threadMain;


        public void Stop()
        {
            tcpListener.Stop();
            threadMain.Interrupt();
        }

        public void BroadcastBytes(byte[] bytes)
        {
            int i = tcpClients.Count;
            while (i-- > 0)
            {
                sendToClient(tcpClients[i], bytes);
            }
        }

        private void sendToClient(TcpClient tcp, byte[] bytes)
        {
            try
            {
                NetworkStream stream = tcp.GetStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
            catch (Exception e)
            {
                tcpClients.Remove(tcp);
            }
        }

        private void threadMainFn()
        {
            tcpListener = new TcpListener(IPAddress.Any, this.port);
            tcpListener.Start();

            try
            {
                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    client.SendBufferSize = 64 * 1024;
                    client.ReceiveBufferSize = 64 * 1024;
                    client.NoDelay = true;
                    tcpClients.Add(client);
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
