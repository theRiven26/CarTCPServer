using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CarShowroom
{
    public class Program
    {
        public static void Main(string[] args)
        {
   
            List<Car> list = new List<Car>();
            list.Add(new Car("Nissan", 2008, 1.6));
            list.Add(new Car("Lada", 2019, 1.6, 5));
            list.Add(new Car("VW", 2021, 1.4));
            list.Add(new Car("Toyota", 2018, 2.5, 4));

            CarList carList = new CarList(list);
            string fileName = @"C:\CarShowroom.json";
            string jsonString = JsonConvert.SerializeObject(carList);
            if (!File.Exists(fileName))
            {
                try
                { File.Create(fileName);
                }catch(Exception ex) { 
                    Console.WriteLine(ex.ToString());
                }
            }
            try
            {
                File.WriteAllText(fileName, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();

            foreach(Car car in list)
            {

            }

        }
    }
}
