using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class Check
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public DateTime SubmissionDate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusDescription { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        public string SelectedType { get; set; }
        [DataMember]
        public string ValidatedType { get; set; }
        [DataMember]
        public decimal BaseFee { get; set; }
        [DataMember]
        public decimal DiscountApplied { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public string DiscountName { get; set; }
        [DataMember]
        public byte[] Image { get; set; }
        [DataMember]
        public bool IsHardDeclined { get; set; }
        [DataMember]
        public string DmsStatusMessage { get; set; }
        [DataMember]
        public decimal SelectedFee { get; set; }
        [DataMember]
        public decimal ValidatedFee { get; set; }
        [DataMember]
        public int DeclineCode { get; set; }
        [DataMember]
        public string DiscountDescription { get; set; }
        [DataMember]
        public ProviderId ProductProviderCode { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "Id = ", Id, "\r\n");
            str = string.Concat(str, "SubmissionDate = ", SubmissionDate, "\r\n");
            str = string.Concat(str, "Amount = ", Amount, "\r\n");
            str = string.Concat(str, "Status = ", Status, "\r\n");
            str = string.Concat(str, "StatusDescription = ", StatusDescription, "\r\n");
            str = string.Concat(str, "StatusMessage = ", StatusMessage, "\r\n");
            str = string.Concat(str, "SelectedType = ", SelectedType, "\r\n");
            str = string.Concat(str, "ValidatedType = ", ValidatedType, "\r\n");
            str = string.Concat(str, "BaseFee = ", BaseFee, "\r\n");
            str = string.Concat(str, "DiscountApplied = ", DiscountApplied, "\r\n");
            str = string.Concat(str, "Fee = ", Fee, "\r\n");
            str = string.Concat(str, "DiscountName = ", DiscountName, "\r\n");
            str = string.Concat(str, "Image = ", Image, "\r\n");
            str = string.Concat(str, "IsHardDeclined = ", IsHardDeclined, "\r\n");
            str = string.Concat(str, "DeclineCode = ", DeclineCode.ToString(), "\r\n");
            str = string.Concat(str, "DmsStatusMessage = ", DmsStatusMessage, "\r\n");
            str = string.Concat(str, "SelectedFee = ", SelectedFee, "\r\n");
            str = string.Concat(str, "DiscountDescription = ", DiscountDescription, "\r\n");
            return str;
        }
    }
}
