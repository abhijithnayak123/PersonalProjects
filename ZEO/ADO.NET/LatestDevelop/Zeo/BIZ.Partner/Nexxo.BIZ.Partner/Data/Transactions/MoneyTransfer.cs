using System.Runtime.Serialization;

namespace MGI.Biz.Partner.Data.Transactions
{
	[DataContract]
	public class MoneyTransfer : Transaction
	{
        public int TransferType { get; set; }
        public string TransactionSubType { get; set; }
        public long OriginalTransactionId { get; set; }
public string ConfirmationNumber { get; set; }
	}
}
