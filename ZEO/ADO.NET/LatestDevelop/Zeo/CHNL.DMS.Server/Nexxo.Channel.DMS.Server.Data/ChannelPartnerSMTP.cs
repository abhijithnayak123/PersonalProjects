using System;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
    [DataContract]
    public class ChannelPartnerSMTP
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public int ChannelPartnerId { get; set; }
        [DataMember]
        public string SmtpHost { get; set; }
        [DataMember]
        public int SmtpPort { get; set; }
        [DataMember]
        public string SenderEmail { get; set; }
        [DataMember]
        public string SenderPassword { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string Body { get; set; }
        //[DataMember]
        //public DateTime DTCreate { get; set; }
        //[DataMember]
        //public Nullable<DateTime> DTLastMod { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "ChannelPartnerId = ", ChannelPartnerId, "\r\n");
			str = string.Concat(str, "SmtpHost = ", SmtpHost, "\r\n");
			str = string.Concat(str, "SmtpPort = ", SmtpPort, "\r\n");
			str = string.Concat(str, "SenderEmail = ", SenderEmail, "\r\n");
			str = string.Concat(str, "SenderPassword = ", SenderPassword, "\r\n");
			str = string.Concat(str, "Subject = ", Subject, "\r\n");
			str = string.Concat(str, "Body = ", Body, "\r\n");
            //str = string.Concat(str, "DTCreate = ", DTCreate, "\r\n");
            //str = string.Concat(str, "DTLastMod = ", DTLastMod, "\r\n");
			return str;
		}
	}
}
