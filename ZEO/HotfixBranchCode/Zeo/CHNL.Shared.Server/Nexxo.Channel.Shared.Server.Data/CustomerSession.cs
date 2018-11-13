using MGI.Common.Util;
using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class CustomerSession
    {
        public CustomerSession() { }

        [DataMember]
        public string CustomerSessionId { get; set; }

        [DataMember]
        public bool CardPresent { get; set; }

        [DataMember]
        public Customer Customer { get; set; }

        [DataMember]
        public string TipsAndOffers { get; set; }
        
        [DataMember]
        public string TimezoneID { get; set; }
        
        [DataMember]
        public bool isNewCustomer { get; set; }

		public ProfileStatus ProfileStatus { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "CustomerSessionId = ", CustomerSessionId, "\r\n");
            str = string.Concat(str, "CardPresent = ", CardPresent, "\r\n");
            str = string.Concat(str, "Customer = ", Customer, "\r\n");
            str = string.Concat(str, "TipsAndOffers = ", TipsAndOffers, "\r\n");
            str = string.Concat(str, "isNewCustomer = ", isNewCustomer, "\r\n");
			str = string.Concat(str, "ProfileStatus = ", ProfileStatus, "\r\n");
            return str;
        }
    }
}
