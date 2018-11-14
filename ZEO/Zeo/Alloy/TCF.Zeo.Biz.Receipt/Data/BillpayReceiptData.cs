using System;

namespace TCF.Zeo.Biz.Receipt.Data
{
    public class BillpayReceiptData : BaseReceiptData
    {
        public long ConfirmationId { get; set; }
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
