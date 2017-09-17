using TCF.Zeo.Cxn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Data
{
    public class WUAccount : BaseRequest
    {
        public string NameType { get; set; }
        public string PreferredCustomerAccountNumber { get; set; }
        public string PreferredCustomerLevelCode { get; set; }
        public string SmsNotificationFlag { get; set; }
    }
}
