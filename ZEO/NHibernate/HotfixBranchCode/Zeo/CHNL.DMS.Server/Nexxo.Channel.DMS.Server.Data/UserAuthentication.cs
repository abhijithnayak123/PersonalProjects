using System;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
    [DataContract]
    public class UserAuthentication
    {
        [DataMember]
        public int AgentId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int AuthenticationFailures { get; set; }
        [DataMember]
        public string passwordHash { get; set; }
        [DataMember]
        public string Salt { get; set; }
        [DataMember]
        public bool TemporaryPassword { get; set; }
        [DataMember]
        public string LastPasswordUpdateBy { get; set; }
        [DataMember]
        public DateTime DTLastPasswordUpdate { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "AgentId = ", AgentId, "\r\n");
			str = string.Concat(str, "UserName = ", UserName, "\r\n");
			str = string.Concat(str, "AuthenticationFailures = ", AuthenticationFailures, "\r\n");
			str = string.Concat(str, "passwordHash = ", passwordHash, "\r\n");
			str = string.Concat(str, "Salt = ", Salt, "\r\n");
			str = string.Concat(str, "TemporaryPassword = ", TemporaryPassword, "\r\n");
			str = string.Concat(str, "LastPasswordUpdateBy = ", LastPasswordUpdateBy, "\r\n");
			str = string.Concat(str, "DTLastPasswordUpdate = ", DTLastPasswordUpdate, "\r\n");
			return str;
		}
	}
}
