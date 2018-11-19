using ConsoleApplication1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Classes
{
    public class TestClass : Interface1, Interface2
    {
        public int TestProper
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        void Interface2.Method1()
        {
            throw new NotImplementedException();
        }

        void Interface1.Method1()
        {
            throw new NotImplementedException();
        }
    }
}
