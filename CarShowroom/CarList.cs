using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom
{
    [Serializable]
    public abstract class CarList : ICarNumerable
    {
        private List<Car> list;

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
            return new ;
        }


    }
}
