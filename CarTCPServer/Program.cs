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
            CarList list = getCarList(fileAdress);

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
                if (answer.Equals("1"))
                {
                    listener.Send(convertCarListToByte(0, list));
                }
                
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }

        public static byte[] convertCarListToByte(int numRecords, CarList list)
        {

            byte[] data = new byte[256];
      

            if (numRecords == 0)
            {
                    data = carToByte(list);
            }
            else
            {
                List<Car> listCar = new List<Car>();
                listCar.Add(list[numRecords]);
                CarList carList = new CarList(listCar);
                data = carToByte(carList);
            }
            return data;
        }

        public static byte[] carToByte(CarList cars)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                foreach (Car car in cars.list)
                {
                    if (car.brand != null)
                    {
                        stream.WriteByte(0x02);
                        stream.Write(BitConverter.GetBytes(car.getLenght()), 0, 2);
                        stream.WriteByte(0x09);
                        byte[] brandBytes = Encoding.ASCII.GetBytes(car.brand);
                        stream.Write(BitConverter.GetBytes(brandBytes.Length), 0, 2);
                        stream.Write(brandBytes, 0, brandBytes.Length);

                        if (car.year != 0)
                        {
                            stream.WriteByte(0x12);
                            stream.Write(BitConverter.GetBytes(car.year), 0, 2);
                        }
                        if (car.engineVolume != 0)
                        {
                            stream.WriteByte(0x13);
                            stream.Write(BitConverter.GetBytes(car.engineVolume), 0, 8);
                        }
                        if (car.countDoors != 0)
                        {
                            stream.WriteByte(0x12);
                            stream.Write(BitConverter.GetBytes(car.countDoors), 0, 2);
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public static CarList getCarList(string fileAdress)
        {

            string jsonString = File.ReadAllText(fileAdress);
            CarList list = JsonConvert.DeserializeObject<CarList>(jsonString);

            return list;
        }
    }
}
