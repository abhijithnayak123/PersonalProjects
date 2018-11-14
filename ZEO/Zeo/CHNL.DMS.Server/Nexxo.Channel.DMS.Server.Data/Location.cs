using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
    [DataContract]
    public class Location
    {
        [DataMember]
        public Guid RowGuid { get; set; }
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string BankID { get; set; }
        [DataMember]
        public string BranchID { get; set; }
		[DataMember]
		public string LocationIdentifier { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        //[DataMember]
        //public DateTime DtCreate { get; set; }
        //[DataMember]
        //public DateTime DtLastMod { get; set; }
        [DataMember]
        public long ChannelPartnerId { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string TimezoneID { get; set; }
		[DataMember]
		public int NoOfCounterIDs { get; set; }
        [DataMember]
        public List<ProcessorCredential> ProcessorCredentials { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "RowGuid = ", RowGuid, "\r\n");
            str = string.Concat(str, "Id = ", Id, "\r\n");
            str = string.Concat(str, "BankID = ", BankID, "\r\n");
            str = string.Concat(str, "BranchID = ", BranchID, "\r\n");
			str = string.Concat(str, "LocationIdentifier = ", LocationIdentifier, "\r\n");
            str = string.Concat(str, "LocationName = ", LocationName, "\r\n");
            str = string.Concat(str, "IsActive = ", IsActive, "\r\n");
            str = string.Concat(str, "Address1 = ", Address1, "\r\n");
            str = string.Concat(str, "Address2 = ", Address2, "\r\n");
            str = string.Concat(str, "City = ", City, "\r\n");
            str = string.Concat(str, "State = ", State, "\r\n");
            str = string.Concat(str, "ZipCode = ", ZipCode, "\r\n");
            //str = string.Concat(str, "DtCreate = ", DtCreate, "\r\n");
            //str = string.Concat(str, "DtLastMod = ", DtLastMod, "\r\n");
            str = string.Concat(str, "ChannelPartnerId = ", ChannelPartnerId, "\r\n");
            str = string.Concat(str, "PhoneNumber = ", PhoneNumber, "\r\n");
            str = string.Concat(str, "TimezoneID = ", TimezoneID, "\r\n");
			str = string.Concat(str, "NoOfCounterIDs =", NoOfCounterIDs, "\r\n");
            str = string.Concat(str, "ProcessorCredentials = ", ProcessorCredentials == null ? string.Empty : ProcessorCredentials.ToString());
            return str;
        }
    }
}
