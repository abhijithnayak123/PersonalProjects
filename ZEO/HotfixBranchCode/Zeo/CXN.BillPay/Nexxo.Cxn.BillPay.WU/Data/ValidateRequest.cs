using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGI.Cxn.WU.Common.Data;

namespace MGI.Cxn.BillPay.WU.Data
{
    public class ValidateRequest
    {
        public PaymentTransaction PaymentTransaction { get; set; }
        public Channel Channel { get; set; }
        public ForeignRemoteSystem ForeignRemoteSystem { get; set; }
    }
}
