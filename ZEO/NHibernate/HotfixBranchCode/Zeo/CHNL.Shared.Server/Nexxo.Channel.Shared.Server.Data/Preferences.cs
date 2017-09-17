using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
    public class Preferences
    {
        public Preferences() { }
        [DataMember]
        public bool DoNotCall { get; set; }
        [DataMember]
        public bool MarketingSMSEnabled { get; set; }
        [DataMember]
        public bool SMSEnabled { get; set; }
        [DataMember]
        public string ReceiptLanguage { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "DoNotCall = ", DoNotCall, "\r\n");
            str = string.Concat(str, "MarketingSMSEnabled = ", MarketingSMSEnabled, "\r\n");
            str = string.Concat(str, "SMSEnabled = ", SMSEnabled, "\r\n");
            str = string.Concat(str, "ReceiptLanguage = ", ReceiptLanguage, "\r\n");
            return str;
        }

    }
}
