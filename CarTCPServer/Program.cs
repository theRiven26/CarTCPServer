using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CarTCPServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);

            tcpSocket.Listen(5);
            while(true)
            {
                var listener = tcpSocket.Accept();
                byte[] buffer = new byte[256];
                int dataSize = 0;
                var data = new StringBuilder();
                 

                do
                {
                    dataSize = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer,0, dataSize));
                }
                while(listener.Available > 0);
                Console.WriteLine(data.ToString());

                listener.Send(Encoding.UTF8.GetBytes("Успех"));
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}
