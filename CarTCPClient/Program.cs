using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;

namespace CarTCPClient
{
    internal class Program
    {
        static void Main(string[] args)
        {

            const string ip = "127.0.0.1";
            const int port = 8080;

            while (true)
            {
                var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Выберите пункт: ");
                Console.WriteLine("1. Вывести все машины.");
                Console.WriteLine("2. Вывести по номеру.");
                int menuItem = 0;
                var message = Console.ReadLine();
                if (!int.TryParse(message, out menuItem))
                {
                    Console.WriteLine("Введено не верное значение!");
                }
                if(menuItem !=1 && menuItem != 2)
                {
                    Console.WriteLine("Такого пункта нет(");
                }


                if (menuItem == 2)
                {
                    int carItem = 0;
                    Console.WriteLine("Введите номер машины: ");
                    message = Console.ReadLine();
                    if (!int.TryParse(message, out carItem))
                    {
                        Console.WriteLine("Введено не верное значение!");
                    }
                    menuItem = menuItem * 10 + carItem;
                }

                var data = BitConverter.GetBytes(menuItem);
                tcpSocket.Connect(tcpEndPoint);
                tcpSocket.Send(data);

                byte[] buffer = new byte[256];
                int dataSize = 0;

                do
                {
                    dataSize = tcpSocket.Receive(buffer);
                }
                while (tcpSocket.Available > 0);
                string answer = "";
                if (buffer[0] == 0x02) 
                {
                    answer = decodeAndSaveData(buffer);
                }
                else
                {
                    answer = Encoding.UTF8.GetString(buffer,0,dataSize);
                }
                    
                Console.WriteLine(answer);
                tcpSocket.Shutdown(SocketShutdown.Both);
                tcpSocket.Close();

                Console.ReadLine();
            }
        }
        static string decodeAndSaveData(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            int count = 0;

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootElement = xmlDoc.CreateElement("Cars");
            XmlElement carElement = xmlDoc.CreateElement("Car");
            XmlElement modelElement = xmlDoc.CreateElement("Model");
            XmlElement yearElement = xmlDoc.CreateElement("Year");
            XmlElement volumeElement = xmlDoc.CreateElement("EngineVolume");
            XmlElement countDoorElement = xmlDoc.CreateElement("CountDoor");
            xmlDoc.AppendChild(rootElement);

            while (index < data.Length)
            {
                if (data[index] == 0x02)
                {
                    if (count > 1)
                    {
                        sb.Append("\n");
                        rootElement.AppendChild(carElement);
                    }
                    carElement = xmlDoc.CreateElement("Car");
                   
                    count = 1;
                    int countElement = BitConverter.ToUInt16(data, ++index);
                    index += 2;

                }
                else if (data[index] == 0x09)
                {
                    
                    int stringLenght = BitConverter.ToUInt16(data, ++index);
                    index += 2;
                    string model = Encoding.ASCII.GetString(data, index, stringLenght);

                    sb.Append("Model: ");
                    sb.Append(model);
                    sb.Append("; ");
                    index += stringLenght;
                    count++;

                    modelElement = xmlDoc.CreateElement("Model");
                    modelElement.InnerText = model;
                    carElement.AppendChild(modelElement);
                }
                else if(data[index] == 0x12)
                {
                    int val = BitConverter.ToUInt16(data, ++index);
                    if (count == 2)
                    {
                        sb.Append("Year: ");

                        yearElement = xmlDoc.CreateElement("Year");
                        yearElement.InnerText = val.ToString();
                        carElement.AppendChild(yearElement);
                    }
                    else if (count == 4)
                    {
                        sb.Append("Count doors: ");

                        countDoorElement = xmlDoc.CreateElement("countDoor");
                        countDoorElement.InnerText = val.ToString();
                        carElement.AppendChild(countDoorElement);
                    }
                    sb.Append(val.ToString());
                    sb.Append("; ");
                    index += 2;
                    count++;
                }
                else if (data[index] == 0x13)
                {

                    double engineVolume = BitConverter.ToDouble(data, ++index);
                    sb.Append("Engine volume: ");
                    sb.Append(engineVolume.ToString());
                    sb.Append("; ");
  
                    volumeElement = xmlDoc.CreateElement("EngineVolume");
                    volumeElement.InnerText = engineVolume.ToString();
                    carElement.AppendChild(volumeElement);

                    index += 8;
                    count++;
                }
                else
                {
                    break;
                }
            }
            rootElement.AppendChild(carElement);
            xmlDoc.Save(@"C:\cars.xml");
            return sb.ToString();
        }
    }
}
