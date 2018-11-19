using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Classes
{
    public class DerivedClass : BaseClass
    {
        public DerivedClass()
        {
            Console.WriteLine("Derived Class Constr");
        }

        static DerivedClass()
        {
            Console.WriteLine("Derived Class Static Constr");
        }

        public void DerivedMethod()
        {

        }
    }
}
