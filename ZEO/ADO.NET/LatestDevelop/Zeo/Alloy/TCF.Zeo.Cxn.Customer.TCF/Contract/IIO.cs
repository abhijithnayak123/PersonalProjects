using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Customer.Data;
using TCF.Zeo.Cxn.Customer.TCF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.TCF.Contract
{
    public interface IIO
    {
        #region Customer
        List<CustomerProfile> SearchCoreCustomers(CustomerSearchCriteria criteria, RCIFCredential credential, ZeoContext context);

        string CreateCustomer(CustomerProfile customer, RCIFCredential credential, ZeoContext context);
        #endregion

        #region Pre/Post Flush
        bool PreFlush(CustomerTransactionDetails cart, RCIFCredential credential, ZeoContext context);

        void PostFlush(CustomerTransactionDetails cart, RCIFCredential credential, ZeoContext context);
        #endregion
    }
}
