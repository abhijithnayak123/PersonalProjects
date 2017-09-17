using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class AttributeRequest
    {
        public string ReceiveCountry { get; set; }
        public string ReceiveCurrencyCode { get; set; }
        public string ReceiveAgentId { get; set; }
        public decimal Amount { get; set; }
        public string DeliveryServiceCode { get; set; }
        public string DeliveryServiceName { get; set; }
        public MoneyTransferType TransferType { get; set; }
    }
}
