using ConsoleApplication1.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //DerivedClass d = new DerivedClass();
            //ExceptionTesting();

            //string[] arr = new string[] { "A", "B", "C" };
            //string[] arr2 = new string[3];

            //var a =  arr.Clone();
            //arr.CopyTo(arr2, 0);

            //arr[2] = "Abhi";

            //TestClass2 t = new TestClass2();
            //TestClass2 t3 = t;

            //t.TestProp = 10;

            //TestClass4 c = new TestClass5();

            //c.Method1();

            try
            {
                var test = long.Parse("9223372036854775807109");
            }
            catch (Exception ex)
            {

            }

            Console.Read();
        }

        public static void ExceptionTesting()
        {
            string a = string.Empty;

            try
            {
                Console.WriteLine("in the try");
                int d = 0;
                int k = 10 / d;
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("in the DivideByZeroException catch");
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("in the catch");
                throw;
            }
           
          
            finally
            {
                Console.WriteLine("In the finally");
            }

            Console.WriteLine(a);
            Console.ReadKey();
        }
    }
}
