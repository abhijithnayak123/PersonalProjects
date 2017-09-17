using System;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
    [DataContract]
    public class UserDetails
    {
        [DataMember]
        public  Guid Rowguid { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public System.Nullable<int> ManagerId { get; set; }
        [DataMember]
        public long LocationId { get; set; }
        [DataMember]
        public int UserRoleId { get; set; }
        [DataMember]
        public int UserStatusId { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public string TempPassword { get; set; }
        [DataMember]
        public long ChannelPartnerId { get; set; }
        [DataMember]
        public string UserStatus { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public string ClientAgentIdentifier { get; set; }
		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Rowguid = ", Rowguid, "\r\n");
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "UserName = ", UserName, "\r\n");
			str = string.Concat(str, "FirstName = ", FirstName, "\r\n");
			str = string.Concat(str, "LastName = ", LastName, "\r\n");
			str = string.Concat(str, "FullName = ", FullName, "\r\n");
			str = string.Concat(str, "IsEnabled = ", IsEnabled, "\r\n");
			str = string.Concat(str, "ManagerId = ", ManagerId, "\r\n");
			str = string.Concat(str, "LocationId = ", LocationId, "\r\n");
			str = string.Concat(str, "UserRoleId = ", UserRoleId, "\r\n");
			str = string.Concat(str, "UserStatusId = ", UserStatusId, "\r\n");
			str = string.Concat(str, "PhoneNumber = ", PhoneNumber, "\r\n");
			str = string.Concat(str, "Email = ", Email, "\r\n");
			str = string.Concat(str, "Notes = ", Notes, "\r\n");
			str = string.Concat(str, "TempPassword = ", TempPassword, "\r\n");
			str = string.Concat(str, "ChannelPartnerId = ", ChannelPartnerId, "\r\n");
			str = string.Concat(str, "UserStatus = ", UserStatus, "\r\n");
            str = string.Concat(str, "LocationName = ", LocationName, "\r\n");
            str = string.Concat(str, "Client Agent Identifier = ", ClientAgentIdentifier, "\r\n");
			return str;
		}
	}

    public enum SaveMode : int
    {
        Add = 1,
        Update
    }
}
