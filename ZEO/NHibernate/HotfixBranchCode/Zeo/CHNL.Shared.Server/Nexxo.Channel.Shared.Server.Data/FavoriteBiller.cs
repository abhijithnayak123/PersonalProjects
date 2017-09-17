using System;
using System.Runtime.Serialization;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class FavoriteBiller
    {
        [DataMember]
        public string BillerName { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public int ChannelPartnerId { get; set; }

        [DataMember]
        public int ProviderId { get; set; }

        [DataMember]
        public string BillerId { get; set; }

        [DataMember]
        public string ProviderName { get; set; }

        [DataMember]
        public string TenantId { get; set; }

        [DataMember]
        public string BillerCode { get; set; }

        [DataMember]
        public DateTime? LastTransactionDate { get; set; }

        [DataMember]
        public decimal? LastTransactionAmount { get; set; }


        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "BillerName = ", BillerName, "\r\n");
            str = string.Concat(str, "AccountNumber = ", AccountNumber.IsCreditCardNumber()? "*****" + NexxoUtil.getLastFour(AccountNumber) : AccountNumber, "\r\n");
            str = string.Concat(str, "ChannelPartnerId = ", ChannelPartnerId, "\r\n");
            str = string.Concat(str, "ProviderId = ", ProviderId, "\r\n");
            str = string.Concat(str, "BillerId = ", BillerId, "\r\n");
            str = string.Concat(str, "ProviderName = ", ProviderName, "\r\n");
            str = string.Concat(str, "TenantId = ", TenantId, "\r\n");
            str = string.Concat(str, "BillerCode = ", BillerCode, "\r\n");
            str = string.Concat(str, "LastTransactionDate = ", LastTransactionDate, "\r\n");
            str = string.Concat(str, "LastTransactionAmount = ", LastTransactionAmount, "\r\n");
            return str;
        }

    }
}