using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom
{
    internal class CarListNumerator : ICarIterator
    {
        ICarNumerable aggregate;
        int index = 0;

        public CarListNumerator(ICarNumerable aggregate)
        {
            this.aggregate = aggregate;
        }

        public bool HasNext()
        {
            return index < aggregate.Count;
        }

        public Car Next()
        {
            return aggregate[index++];
        }
    }
}
