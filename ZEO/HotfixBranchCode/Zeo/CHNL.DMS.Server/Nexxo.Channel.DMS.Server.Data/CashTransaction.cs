using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class CashTransaction
	{
		public CashTransaction() { }

		[DataMember]
		public Purse Purse;
		[DataMember]
		public string TransactionId;
		[DataMember]
		public decimal Amount;
        [DataMember]
        public string TransactionType { get; set; }
        [DataMember]
        public string TransactionStatus { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine( "Cash Transaction:" );
			sb.AppendLine( string.Format( "	TransactionId: {0}", TransactionId ) );
			sb.AppendLine( string.Format( "	Amount: {0:C}", Amount ) );
            sb.AppendLine( string.Format("TransactionType : {0}", TransactionType) );
            sb.AppendLine( string.Format("TransactionStatus : {0}", TransactionStatus) );
            sb.Append(Purse == null ? string.Empty : Purse.ToString());
			return sb.ToString();
		}
	}
}
