using commonData = TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Biz.Customer.Contract
{
    public interface IFlushService
    {
        void PostFlush(decimal cardBalance, commonData.ZeoContext context);

        void PreFlush(decimal cashToCustomer, commonData.ZeoContext context);
    }
}
