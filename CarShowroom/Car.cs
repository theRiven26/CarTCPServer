using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom
{
    [Serializable]
    public class Car
    {
        public string brand { get;private set; }
        public int year { get; private set; }
        public double engineVolume { get; private set; }
        public int countDoors { get; private set; }

        public Car(string brand, int year, double engineVolume, int countDoors)
        {
            this.brand = brand;
            this.year = year;
            this.engineVolume = engineVolume;
            this.countDoors = countDoors;
        }

     

        public Car(string brand, int year, double engineVolume)
        {
            this.brand = brand;
            this.year = year;
            this.engineVolume = engineVolume;
            this.countDoors = 0;
        }

        public Car(string brand, int year )
        {
            this.brand = brand;
            this.year = year;
            this.engineVolume = 0;
            this.countDoors = 0;
        }

        public Car(string brand)
        {
            this.brand = brand;
            this.year = 0;
            this.engineVolume = 0;
            this.countDoors = 0;
        }

        public Car()
        {
            this.brand = null;
            this.year = 0;
            this.engineVolume = 0;
            this.countDoors = 0;
        }

        public string toString()
        {
            StringBuilder sb = new StringBuilder();
            if(brand != null)
            {
                sb.Append(brand);
            }
           if(year != 0)
            {
                sb.Append(year);
            }
            if(engineVolume != 0)
            {
                sb.Append(engineVolume);
            }
            if(countDoors != 0)
            {
                sb.Append(countDoors);
            }
            return sb.ToString();
        }
    }
}
