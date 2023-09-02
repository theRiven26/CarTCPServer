using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom
{
    public class Car
    {
        private string brand { get; set; }
        private int year { get; set; }
        private float engineVolume { get; set; }
        private int countDoors { get; set; }

        public Car(string brand, int year, float engineVolume, int countDoors)
        {
            this.brand = brand;
            this.year = year;
            this.engineVolume = engineVolume;
            this.countDoors = countDoors;
        }
    }
}
