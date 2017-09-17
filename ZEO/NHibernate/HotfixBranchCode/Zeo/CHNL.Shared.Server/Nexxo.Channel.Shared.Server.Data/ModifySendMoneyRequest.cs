using System.Runtime.Serialization;
using System.Text;
using System.Collections.Generic;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class ModifySendMoneyRequest
    {
        public ModifySendMoneyRequest() { }
        [DataMember]
        public string TransactionId { get; set; }
        [DataMember]
        public string ConfirmationNumber { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
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
		public long CancelTransactionId { get; set; }
		[DataMember]
		public long ModifyTransactionId { get; set; }
		[DataMember]
		public decimal TransferTax { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MoneyTransferCancel:");
            sb.AppendLine(string.Format("   TransactionId: {0}", TransactionId));
            sb.AppendLine(string.Format("   MTCN: {0}", ConfirmationNumber));
            sb.AppendLine(string.Format("	FirstName: {0}", FirstName));
            sb.AppendLine(string.Format("	MiddleName: {0}", MiddleName));
            sb.AppendLine(string.Format("	LastName: {0}", LastName));
            sb.AppendLine(string.Format("	SecondLastName: {0}", SecondLastName));
            sb.AppendLine(string.Format("	TestQuestion: {0}", TestQuestion));
            sb.AppendLine(string.Format("	TestAnswer: {0}", TestAnswer));
            sb.AppendLine(string.Format("	TestQuestionAvailable: {0}", TestQuestionAvailable));
            return sb.ToString();
        }
    }
}
