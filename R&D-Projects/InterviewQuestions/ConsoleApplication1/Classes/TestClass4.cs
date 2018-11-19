using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Classes
{
    public abstract class TestClass4 : Interface1
    {
        public int MyProperty { get; set; }

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

        public void Method1()
        {
            throw new NotImplementedException();
        }
    }
}
