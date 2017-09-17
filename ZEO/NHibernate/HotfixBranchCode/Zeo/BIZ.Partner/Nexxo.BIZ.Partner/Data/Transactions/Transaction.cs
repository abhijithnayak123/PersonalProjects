using System;
using System.Runtime.Serialization;

namespace MGI.Biz.Partner.Data.Transactions
{
	[DataContract]
	public abstract class Transaction
	{
		[DataMember]
		public Guid rowguid;
		[DataMember]
		public long Id;
		[DataMember]
		public int CXEState;
		[DataMember]
		public int CXNState;
		[DataMember]
		public decimal Amount;
		[DataMember]
		public decimal Fee;
		[DataMember]
		public string Description;
        [DataMember]
        public long CXEId;
        [DataMember]
        public long CXNId;
        [DataMember]
        public long CustomerSessionId { get; set; }
	}
}
