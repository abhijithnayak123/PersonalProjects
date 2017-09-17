using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.Customer.Contract
{
    public interface IFlushProcessor : IDisposable
    {
        void PreFlush(decimal cashToCustomer, ZeoContext context);

        void PostFlush( decimal cardBalance, ZeoContext context);
    }
}
