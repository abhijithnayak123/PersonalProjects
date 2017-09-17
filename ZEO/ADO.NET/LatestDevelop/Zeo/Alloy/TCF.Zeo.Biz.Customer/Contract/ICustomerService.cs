using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Biz.Customer.Contract
{
    //This interface will have all the customer related methods and can be exposed to the service layer.
    public interface ICustomerService : ICustomerRepository, ICustomerCommonService
    {
    }
}
