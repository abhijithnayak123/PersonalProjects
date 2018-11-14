using System;
using System.Runtime.Serialization;
using static TCF.Zeo.Common.Util.Helper;
namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class CheckSubmission
    {
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public string MICR { get; set; }
        [DataMember]
        public DateTime IssueDate { get; set; }
        [DataMember]
        public string CheckType { get; set; }
        [DataMember]
        public string ImageFormat { get; set; }
        [DataMember]
        public byte[] FrontImage { get; set; }
        [DataMember]
        public byte[] BackImage { get; set; }
        [DataMember]
        public byte[] FrontImageTIFF { get; set; }
        [DataMember]
        public byte[] BackImageTIFF { get; set; }
        [DataMember]
        public string PromoCode { get; set; }
        [DataMember]
        public bool IsSystemApplied { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string RoutingNumber { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public int MicrEntryType { get; set; }
        [DataMember]
        public long FeeAdjustmentId { get; set; }
        [DataMember]
        public decimal BaseFee { get; set; }
        [DataMember]
        public decimal DiscountApplied { get; set; }
        [DataMember]
        public string DiscountName { get; set; }
        [DataMember]
        public string DiscountDescription { get; set; }
        [DataMember]
        public decimal AdditionalFee { get; set; }
        [DataMember]
        public ProviderId ProductProviderCode { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "Amount = ", Amount, "\r\n");
            str = string.Concat(str, "Fee = ", Fee, "\r\n");
            str = string.Concat(str, "MICR = ", MICR, "\r\n");
            str = string.Concat(str, "IssueDate = ", IssueDate, "\r\n");
            str = string.Concat(str, "CheckType = ", CheckType, "\r\n");
            str = string.Concat(str, "ImageFormat = ", ImageFormat, "\r\n");
            str = string.Concat(str, "AccountNumber = ", AccountNumber, "\r\n");
            str = string.Concat(str, "RoutingNumber = ", RoutingNumber, "\r\n");
            str = string.Concat(str, "CheckNumber = ", CheckNumber, "\r\n");
            str = string.Concat(str, "MicrEntryType = ", MicrEntryType.ToString(), "\r\n");
            str = string.Concat(str, "ProductProviderCode = ", ((int)ProductProviderCode).ToString(), "\r\n");
            return str;
        }
    }
}
