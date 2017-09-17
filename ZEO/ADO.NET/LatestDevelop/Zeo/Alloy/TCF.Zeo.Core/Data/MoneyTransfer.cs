using TCF.Zeo.Common.Data;

using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Data
{
    public class MoneyTransfer : ZeoModel
    {
        public decimal Amount { get; set; }
        public long WUTrxId { get; set; }
        public decimal Fee { get; set; }
        public decimal OtherFee { get; set; }
        public string Description { get; set; }
        public string ConfirmationNumber { get; set; }
        public long? RecipientId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public MoneyTransferType MoneyTransferType { get; set; }
        public TransactionSubType? TransactionSubType { get; set; }
        public long OriginalTransactionID { get; set; }
        public long CustomerSessionId { get; set; }
        public int CustomerRevisionNo { get; set; }
        public long ProviderId { get; set; }
        public long ProviderAccountId { get; set; }
        public string Destination { get; set; }
        public TransactionStates State { get; set; }
        public string DestinationCountry { get; set; }
        public decimal DestinationAmount { get; set; }
        public string DestinationCurrency { set; get; }
        public decimal MoneyTransferTotal { get; set; }
        public decimal OtherTax { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string SourceCountry { get; set; }      
        public string SourceCurrency { get; set; }
        public decimal TransferTax { get; set; }
        public string SenderFirstName { get; set; }  
        public string SenderLastName { get; set; } 
        public string SenderMiddleName { get; set; }
        public string SenderSecondLastName { get; set; }
    }
}
