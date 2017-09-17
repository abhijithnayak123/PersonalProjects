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
            Customer c = new Customer();

            CustomerFluent customer = new CustomerFluent();

            //method chaining.
            customer.NameOfCustomer("Abhijith")
                .Bornon("12/3/1075")
                .StaysAt("Mangalore");

            var n = customer.obj.FullName;
            var d = customer.obj.Dob;

            Console.ReadKey();
        }
    }

    public class Customer
    {
        private string _FullName;
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }
        private DateTime _Dob;
        public DateTime Dob
        {
            get { return _Dob; }
            set { _Dob = value; }
        }
        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

    }

    //Fluent Interface.
    //We can create a fluent interface in any version of .NET or any other language that is Object Oriented. 
    //All we need to do is create an object whose methods always return the object itself.
    public class CustomerFluent
    {
        public Customer obj = new Customer();
        public CustomerFluent NameOfCustomer(string Name)
        {
            obj.FullName = Name;
            return this;
        }
        public CustomerFluent Bornon(string Dob)
        {
           // throw new NotImplementedException();

            obj.Dob = Convert.ToDateTime(Dob);
            return this;
        }
        public void StaysAt(string Address)
        {
            obj.Address = Address;

        }
    }
}
