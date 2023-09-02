using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShowroom
{

        interface ICarNumerable {
        ICarIterator CreateNumerator();
        int Count { get; }
        Car this[int index] { get; }
        }
        
 
}
