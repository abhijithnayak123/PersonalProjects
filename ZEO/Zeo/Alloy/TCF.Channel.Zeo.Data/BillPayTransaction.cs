using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class BillPayTransaction
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string PrimaryAuth { get; set; }
        [DataMember]
        public string SecondaryAuth { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusDescription { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        public long BillerId { get; set; }
        //Yashasvi:Added a flag maintained in tShoppingCartItems table to identify if the checkfee has been accepted by customer
        [DataMember]
        public bool AcceptedFee { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public decimal BillTotal { get; set; }
        [DataMember]
        public string BillerName { get; set; }
        [DataMember]
        public string BillId { get; set; }
        [DataMember]
        public string BillerZip { get; set; }
        [DataMember]
        public int AgentID { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string CustomerCity { get; set; }
        [DataMember]
        public DateTime CustomerDOB { get; set; }
        [DataMember]
        public string CustomerFirstName { get; set; }
        [DataMember]
        public string CustomerLastName { get; set; }
        [DataMember]
        public string CustomerPAN { get; set; }
        [DataMember]
        public string CustomerPhoneNumber { get; set; }
        [DataMember]
        public string CustomerState { get; set; }
        [DataMember]
        public string CustomerZip { get; set; }
        [DataMember]
        public Dictionary<string, object> MetaData { set; get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string maskedCustomerPAN = string.IsNullOrWhiteSpace(CustomerPAN) || CustomerPAN.Length < 6 ? null : CustomerPAN.Substring(0, 6) + "XXXXXX" + CustomerPAN.Substring(CustomerPAN.Length - 4, 4);
            sb.AppendLine("Bill:");
            sb.AppendLine(string.Format("   Id: {0}", Id));
            sb.AppendLine(string.Format("	Amount: {0}", Amount.ToString("c2")));
            sb.AppendLine(string.Format("	Status: {0}", Status));
            sb.AppendLine(string.Format("	Status Description: {0}", StatusDescription));
            sb.AppendLine(string.Format("	Status Message: {0}", StatusMessage));
            sb.AppendLine(string.Format("	Status BillerId: {0}", BillerId));
            sb.AppendLine(string.Format("	Status AcceptedFee: {0}", AcceptedFee.ToString()));
            sb.AppendLine(string.Format("	Status Fee: {0}", Fee.ToString()));
            sb.AppendLine(string.Format("	Status BillId: {0}", BillId));
            sb.AppendLine(string.Format("	Status BillTotal: {0}", BillTotal.ToString("c2")));
            sb.AppendLine(string.Format("	Status BillerName: {0}", BillerName));
            sb.AppendLine(string.Format("	Status BillerZip: {0}", BillerZip));
            sb.AppendLine(string.Format("	Status AgentID: {0}", AgentID.ToString()));

            sb.AppendLine(string.Format("	Status AccountNumber: {0}", AccountNumber));
            sb.AppendLine(string.Format("	Status CustomerDOB: {0}", CustomerDOB.ToString("yyyy-MM-dd")));
            sb.AppendLine(string.Format("	Status CustomerFirstName: {0}", CustomerFirstName));
            sb.AppendLine(string.Format("	Status CustomerLastName: {0}", CustomerLastName));
            sb.AppendLine(string.Format("	Status CustomerPAN: {0}", maskedCustomerPAN));
            sb.AppendLine(string.Format("	Status CustomerPhoneNumber: {0}", CustomerPhoneNumber));
            sb.AppendLine(string.Format("	Status CustomerState: {0}", CustomerState));
            sb.AppendLine(string.Format("	Status CustomerZip: {0}", CustomerZip));

            return sb.ToString();
        }
    }
}
