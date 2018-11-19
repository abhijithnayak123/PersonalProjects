using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPISample.Models;

namespace WebAPISample.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs
    public class EmployeeController : ApiController
    {
        private List<Employee> _employees = new List<Employee>()
        {
            new Employee() { EmpId = 1, EmpName = "Emp1" },
            new Employee() { EmpId = 2, EmpName = "Emp2" },
            new Employee() { EmpId = 3, EmpName = "Emp3" },
            new Employee() { EmpId = 4, EmpName = "Emp4" },
            new Employee() { EmpId = 5, EmpName = "Emp5" },
            new Employee() { EmpId = 6, EmpName = "Emp6" },
            new Employee() { EmpId = 7, EmpName = "Emp7" },
            new Employee() { EmpId = 8, EmpName = "Emp8" },
        };

        // GET: api/Employee
        public IEnumerable<Employee> Get()
        {
            return _employees;
        }

        // GET: api/Employee/5
        public Employee Get(int id)
        {
            return _employees.Where(e => e.EmpId == id).First();
        }

        // POST: api/Employee
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Employee/5
        public IEnumerable<Employee> Put(int id, [FromBody]string value)
        {
            Employee e = _employees.Where(t => t.EmpId == id).FirstOrDefault();

            if (e != null)
                e.EmpName = "Updated";

            return _employees;
        }

        // DELETE: api/Employee/5
        public bool Delete(int id)
        {
            return true;
        }
    }
}
