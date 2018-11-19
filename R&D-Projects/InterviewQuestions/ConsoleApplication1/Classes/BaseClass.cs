using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Classes
{
    public class BaseClass
    {
        public BaseClass()
        {
            Console.WriteLine("Base Class Constr");
        }

        static BaseClass()
        {
            Console.WriteLine("Base Class Static Constr");
        }

        public void BaseMethod()
        {

        }
    }
}
