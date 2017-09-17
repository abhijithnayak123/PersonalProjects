using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class Customer
    {

        public Customer()
        {
        }
        [DataMember]
        public PersonalInformation PersonalInformation { get; set; }
        [DataMember]
        public Address Address { get; set; }
        [DataMember]
        public Address MailingAddress { get; set; }
        [DataMember]
		public Identification ID { get; set; }
        [DataMember]
        public Employment Employment { get; set; }
        [DataMember]
        public Fund Fund { get; set; }
        [DataMember]
        public Preferences Preferences { get; set; }
        [DataMember]
        public Phone Phone1 { get; set; }
        [DataMember]
        public Phone Phone2 { get; set; }

        [DataMember]
        public bool IsAnonymous { get; set; }
        
        // shouldn't have this...
        [DataMember]
        public bool IsWUGoldCard { get; set; }

        // what is this??
        public string Resolution { get; set; }

        //what is this??
        public int FraudScore { get; set; }
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string SSN { get; set; }

        [DataMember]
        public long CIN { get; set; }

        [DataMember]
        public bool MailingAddressDifferent { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "PersonalInformation = ", PersonalInformation, "\r\n");
            str = string.Concat(str, "Address = ", Address, "\r\n");
            str = string.Concat(str, "Email = ", Email, "\r\n");
            str = string.Concat(str, "MailingAddress = ", MailingAddress, "\r\n");
			str = string.Concat(str, "GovernmentId = ", ID, "\r\n");
            str = string.Concat(str, "Employment = ", Employment, "\r\n");
            str = string.Concat(str, "IFundD = ", Fund, "\r\n");
            str = string.Concat(str, "Preferences = ", Preferences, "\r\n");
            str = string.Concat(str, "Phone1 = ", Phone1, "\r\n");
            str = string.Concat(str, "Phone2 = ", Phone2, "\r\n");

            str = string.Concat(str, "IsAnonymous = ", IsAnonymous, "\r\n");
            str = string.Concat(str, "IsWUGoldCard = ", IsWUGoldCard, "\r\n");
            str = string.Concat(str, "Resolution = ", Resolution, "\r\n");
            str = string.Concat(str, "FraudScore = ", FraudScore, "\r\n");
            str = string.Concat(str, "Email = ", Email, "\r\n");
            str = string.Concat(str, "SSN = XXXX-XX-", string.IsNullOrWhiteSpace(SSN) ? null : SSN.Substring(SSN.Length - 4, 4), "\r\n");
			str = string.Concat(str, "CIN = ", CIN.ToString().Substring(0, 6) + "XXXXXX" + CIN.ToString().Substring(CIN.ToString().Length - 4, 4), "\r\n");
            return str;
        }
    }
}

