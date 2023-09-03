using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom
{
    [Serializable]
    public class CarList : ICarNumerable
    {
        public List<Car> list { get;  set; }

        public CarList(List<Car> list)
        {
            this.list = list;
        }

        public Car this[int index] {
            get { return list[index]; }
        }

        public int Count {
            get { return list.Count; }
    }

        public void addCar(Car car)
        {
            list.Add(car);
        }

        public ICarIterator CreateNumerator()
        {
             return new CarListNumerator(this);

        }
    
        public string toString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Car car in list)
            {
                sb.Append(car.toString());
            }
            return sb.ToString();
        }


    }
}
