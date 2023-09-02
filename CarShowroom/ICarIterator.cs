using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom
{
    public interface ICarIterator
    {
        bool HasNext();
        Car Next();
    }
}
