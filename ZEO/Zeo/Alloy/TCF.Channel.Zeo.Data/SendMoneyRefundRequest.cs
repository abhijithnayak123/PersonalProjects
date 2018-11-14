using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class SendMoneyRefundRequest
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
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public long CancelTransactionId { get; set; }
        [DataMember]
        public long RefundTransactionId { get; set; }
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
            sb.AppendLine(string.Format(" CancelTransactionId:{0}", CancelTransactionId));
            sb.AppendLine(string.Format(" RefundTransactionId:{0}", RefundTransactionId));
            return sb.ToString();
        }

    }
}
