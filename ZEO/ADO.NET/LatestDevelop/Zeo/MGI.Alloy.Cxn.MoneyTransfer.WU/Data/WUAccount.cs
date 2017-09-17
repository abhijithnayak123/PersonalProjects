using MGI.Alloy.CXN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Alloy.Cxn.MoneyTransfer.WU.Data
{
    public class WUAccount : BaseRequest
    {
        public long RevisionNo { get; set; }
        public string NameType { get; set; }
        public string PreferredCustomerAccountNumber { get; set; }
        public string PreferredCustomerLevelCode { get; set; }
        public string SmsNotificationFlag { get; set; }
    }
}
