using System.Runtime.Serialization;

namespace MGI.Biz.Partner.Data.Transactions
{
	[DataContract]
	public class Cash : Transaction
	{
        public string TransactionType { get; set; }
	}
}
