using System.Runtime.Serialization;

namespace MGI.Biz.Partner.Data.Transactions
{
	[DataContract]
	public class Funds : Transaction
	{
        public string TransactionType { get; set; }
		public long AddOnCustomerId { get; set; }
	}
}
