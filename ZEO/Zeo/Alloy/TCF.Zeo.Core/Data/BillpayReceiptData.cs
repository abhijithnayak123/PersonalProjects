using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class BillpayReceiptData : BaseReceiptData
    {
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderCity { get; set; }
        public string SenderState { get; set; }
        public string SenderZip { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string SenderMobileNumber { get; set; }
        public string ConfirmationId { get; set; }
        public long TransactionId { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TransferAmmount { get; set; }
        public DateTime TxrDate { get; set; }
        public string MessageArea { get; set; }
        public string WuCardTotalPointsEarned { get; set; }
        public string DeliveryService { get; set; }
        public string GCNumber { get; set; }
        public string MTCN { get; set; }
        public decimal PrmDiscount { get; set; }
        public decimal UnDiscountedFee { get; set; }
        public string AccountNumber { get; set; }
        public string ReceiverName { get; set; }
    }
}
