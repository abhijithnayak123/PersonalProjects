using System.Runtime.Serialization;
using System.Text;
using MGI.Common.Util;
namespace MGI.Channel.Shared.Server.Data
{
    public class Fund
    {
        public Fund() { }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
        [DataMember]
        public decimal CardBalance { get; set; }
        [DataMember]
        public bool IsGPRCard { get; set; }


        override public string ToString()
        {
            string str = string.Empty;
			string maskedAccountNumber = string.IsNullOrWhiteSpace(AccountNumber) || AccountNumber.Length < 4 ? null : AccountNumber.Substring(AccountNumber.Length - 4, 4);
			str = string.Concat(str, "AccountNumber = XXXX-XX- ", maskedAccountNumber, "\r\n");
            str = string.Concat(str, "CardNumber = ", NexxoUtil.cardLastFour(CardNumber), "\r\n");
            str = string.Concat(str, "CardBalance = ", CardBalance, "\r\n");
            str = string.Concat(str, "IsGPRCard = ", IsGPRCard, "\r\n");
            return str;
        }
       
       
    }
}
