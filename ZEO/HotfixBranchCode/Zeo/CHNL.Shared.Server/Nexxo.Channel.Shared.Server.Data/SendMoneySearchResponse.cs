using System.Runtime.Serialization;
using System.Text;
using System.Collections.Generic;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
	public class SendMoneySearchResponse
    {
        [DataMember]
        public string TransactionId { get; set; }
        [DataMember]
        public string ConfirmationNumber { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string SecondLastName { get; set; }
        [DataMember]
        public string TestQuestion { get; set; }
        [DataMember]
        public string TestAnswer { get; set; }
        [DataMember]
        public string TestQuestionAvailable { get; set; }
		[DataMember]
		public string RefundStatus { get; set; }
		[DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string TransactionStatus { get; set; }
        [DataMember]
        public string FeeRefund { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SendMoneySearchResponse:");
            sb.AppendLine(string.Format("   TransactionId: {0}", TransactionId));
            sb.AppendLine(string.Format("   MTCN: {0}", ConfirmationNumber));
            sb.AppendLine(string.Format("	FirstName: {0}", FirstName));
            sb.AppendLine(string.Format("	LastName: {0}", LastName));
            sb.AppendLine(string.Format("	SecondLastName: {0}", SecondLastName));
            sb.AppendLine(string.Format("	TestQuestion: {0}", TestQuestion));
            sb.AppendLine(string.Format("	TestAnswer: {0}", TestAnswer));
            sb.AppendLine(string.Format("	TestQuestionAvailable: {0}", TestQuestionAvailable));
            sb.AppendLine(string.Format("	RefundStatus: {0}", RefundStatus));
            sb.AppendLine(string.Format("	MiddleName: {0}", MiddleName));
            sb.AppendLine(string.Format("	TransactionStatus: {0}", TransactionStatus));
            return sb.ToString();
        }
    }
}
