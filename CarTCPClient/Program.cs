using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarTCPClient
{
    internal class Program
    {
        static void Main(string[] args)
        {

            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Выберите пункт: ");
            Console.WriteLine("1. Вывести все машины.");
            Console.WriteLine("2. Вывести по номеру.");
            var message = Console.ReadLine();

            var data = Encoding.UTF8.GetBytes(message);
            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);

            byte[] buffer = new byte[256];
            int dataSize = 0;
            var answer = new StringBuilder();

            do
            {
                dataSize = tcpSocket.Receive(buffer);
                answer.Append(Encoding.UTF8.GetString(buffer,0,dataSize));
            }
            while (tcpSocket.Available > 0);

            Console.WriteLine(answer.ToString());
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();

            Console.ReadLine();

        }
    }
}
