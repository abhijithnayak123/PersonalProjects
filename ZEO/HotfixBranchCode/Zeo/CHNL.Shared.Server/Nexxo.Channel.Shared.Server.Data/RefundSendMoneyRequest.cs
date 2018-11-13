using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class RefundSendMoneyRequest
    {
        [DataMember]
        public string TransactionId { get; set; }
        [DataMember]
        public string ConfirmationNumber { get; set; }
        [DataMember]
        public string CategoryCode { get; set; }
        [DataMember]
        public string CategoryDescription { get; set; }
        [DataMember]
		public string Reason { get; set; }
		[DataMember]
		public string RefundStatus { get; set; }
        [DataMember]
        public string FeeRefund { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Money Transfer Refund:");
            sb.AppendLine(string.Format("   TransactionId: {0}", TransactionId));
            sb.AppendLine(string.Format("   ConfirmationNumber: {0}", ConfirmationNumber));
            sb.AppendLine(string.Format("	CategoryCode: {0}", CategoryCode));
            sb.AppendLine(string.Format("	CategoryDescription: {0}", CategoryDescription));
			sb.AppendLine(string.Format("	Reason: {0}", Reason));
			sb.AppendLine(string.Format("	RefundStatus: {0}", RefundStatus));
            return sb.ToString();
        }
    }
}
