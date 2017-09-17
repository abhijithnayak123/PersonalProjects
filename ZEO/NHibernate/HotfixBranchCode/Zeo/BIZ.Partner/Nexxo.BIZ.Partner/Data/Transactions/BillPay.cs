using System.Runtime.Serialization;

namespace MGI.Biz.Partner.Data.Transactions
{
	[DataContract]
	public class BillPay : Transaction
	{
	    [DataMember] 
        public long ProductId;
	}
}
