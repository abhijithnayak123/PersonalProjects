using System;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.NpsInstallerServer.Data
{
    [DataContract]
    public class Location
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Guid rowguid { get; set; }
        [DataMember]
        public virtual string BankID { get; set; }
        [DataMember]
        public virtual string BranchID { get; set; }
        [DataMember]
        public virtual string LocationIdentifier { get; set; }
        [DataMember]
        public virtual string LocationName { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual string Address1 { get; set; }
        [DataMember]
        public virtual string Address2 { get; set; }
        [DataMember]
        public virtual string City { get; set; }
        [DataMember]
        public virtual string State { get; set; }
        [DataMember]
        public virtual string ZipCode { get; set; }
        [DataMember]
        public virtual int ChannelPartnerId { get; set; }
        [DataMember]
        public virtual string PhoneNumber { get; set; }
        [DataMember]
        public virtual string TimezoneID { get; set; }
        [DataMember]
        public virtual DateTime DTCreate { get; set; }
        [DataMember]
        public virtual DateTime? DTLastMod { get; set; }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("RowGuid = " + rowguid + "\r\n");
            str.Append("Id = " + Id + "\r\n");
            str.Append("Bank Id = " + BankID + "\r\n");
            str.Append("Branch Id = " + BranchID + "\r\n");
            str.Append("Location Identifier = " + LocationIdentifier + "\r\n");
            str.Append("LocationName = " + LocationName + "\r\n");
            str.Append("IsActive = " + IsActive + "\r\n");
            str.Append("Address1 = " + Address1 + "\r\n");
            str.Append("Address2 = " + Address2 + "\r\n");
            str.Append("City = " + City + "\r\n");
            str.Append("State = " + State + "\r\n");
            str.Append("ZipCode = " + ZipCode + "\r\n");
            str.Append("DtCreate = " + DTCreate + "\r\n");
            str.Append("DtLastMod = " + DTLastMod + "\r\n");
            str.Append("ChannelPartnerId = " + ChannelPartnerId + "\r\n");
            str.Append("PhoneNumber = " + PhoneNumber + "\r\n");
            return str.ToString();
        }
	}
}
