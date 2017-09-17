using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.BillPay.Data
{
    public class BillPayValidateResponse
    {
        public long TransactionId { get; set; }
        public string BillerName { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string ConfirmationNumber { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastname { get; set; }
        public string SenderAddressLine1 { get; set; }
        public string SenderAddressLine2 { get; set; }
        public string SenderCity { get; set; }
        public string SenderState { get; set; }
        public string SenderPostalCode { get; set; }
        public string SenderEmail { get; set; }
        public string SenderContactPhone { get; set; }
        public string SenderDateOfBirth { get; set; }
        public string SenderWUGoldcardNumber { set; get; }
        public decimal UnDiscountedFee { set; get; }
        public decimal DiscountedFee { set; get; }
    }
}
