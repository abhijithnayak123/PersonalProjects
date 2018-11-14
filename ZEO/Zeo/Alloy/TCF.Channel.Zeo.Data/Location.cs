using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCF.Zeo.Common.Data;
using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class Location
    {
        [DataMember]
        public virtual long LocationID { get; set; }
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
        public virtual int NoOfCounterIDs { get; set; }
    }
}
