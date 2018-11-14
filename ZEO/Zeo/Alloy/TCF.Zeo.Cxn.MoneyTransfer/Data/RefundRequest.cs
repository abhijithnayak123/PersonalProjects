using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
   public  class RefundRequest: BaseRequest
    {
        public string ReasonDesc { get; set; }
        public string ReasonCode { get; set; }
        public string Comments { get; set; }
        public string RefundStatus { get; set; }
        public long TransactionId { get; set; }
        public string ReferenceNumber { get; set; }
        public string FeeRefund { get; set; }
        public string ReceiverSecondLastName { get; set; }
        public string PreferredCustomerAccountNumber { get; set; }
        public string SenderComplianceDetailsComplianceDataBuffer { get; set; }
        public string DestinationCountryCode { get; set; }
        public string DestinationCurrencyCode { get; set; }
        public string ExpectedPayoutStateCode { get; set; }
        public string ExpectedPayoutCityName { get; set; }
        public string MTCN { get; set; }
        public string TempMTCN { get; set; }
        public string MoneyTransferKey { get; set; }
        public decimal Charges { get; set; }
        public decimal OriginatorsPrincipalAmount { get; set; }
        public decimal GrossTotalAmount { get; set; }
        public decimal AmountToReceiver { get; set; }
        public decimal DestinationPrincipalAmount { get; set; }
        public decimal plus_charges_amount { get; set; }
        public string ConfirmationNumber { get; set; }
        public long CancelTransactionId { get; set; }
        public long RefundTransactionId { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
    }
}
