using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CarShowroom;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Reflection;

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
            string fileAdress = @"C:\CarShowroom.json";
            CarList list = GetCarList(fileAdress);

            tcpSocket.Listen(1);
            while (true)
            {
                var listener = tcpSocket.Accept();
                byte[] buffer = new byte[256];
                int dataSize = 0;
                var data = new StringBuilder();
                do
                {
                    dataSize = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, dataSize));
                }
                while (listener.Available > 0);
                string answer = data.ToString();
                if (answer.Equals(1))
                {
                    listener.Send(ConvertCarListToByte(0, list));
                }
                

                listener.Send(Encoding.UTF8.GetBytes("Успех"));
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }

        public static byte[] ConvertCarListToByte(int numRecords, CarList list)
        {

            byte[] data = new byte[256];
      

            if (numRecords == 0)
            {
                foreach (Car car in list.list)
                {
                    data = CarToByte(car);
                }
            }
            else
            {
                data = CarToByte(list[numRecords]);
            }
            return data;
        }

        public static byte[] CarToByte(Car car)
        {
            byte[] data = new byte[256];
            if (car.brand != null)
            {
                data.Append((byte)0x02);
                data.Append((byte)0x09);
                byte[] brandBytes = Encoding.ASCII.GetBytes(car.brand);
                data.Concat(BitConverter.GetBytes((int)brandBytes.Length));
                data.Concat(brandBytes);
                if (car.year != 0)
                {
                    data.Append((byte)0x12);
                    data.Concat(BitConverter.GetBytes(car.year));
                }

                if (car.engineVolume != 0)
                {
                    data.Append((byte)0x12);
                    data.Concat(BitConverter.GetBytes(car.engineVolume));
                }
                if (car.countDoors != 0)
                {
                    data.Append((byte)0x12);
                    data.Concat(BitConverter.GetBytes(car.countDoors));
                }
            }
            return data;

        }

        public static CarList GetCarList(string fileAdress)
        {

            string jsonString = File.ReadAllText(fileAdress);
            CarList list = JsonConvert.DeserializeObject<CarList>(jsonString);

            return list;
        }
    }
}
