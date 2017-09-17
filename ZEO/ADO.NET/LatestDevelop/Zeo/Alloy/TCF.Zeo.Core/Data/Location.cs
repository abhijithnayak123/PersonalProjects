using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Data
{
    public class Location : ZeoModel
    {
        public virtual long LocationId { get; set; }
        public virtual string BankID { get; set; }
        public virtual string BranchID { get; set; }
        public virtual string LocationIdentifier { get; set; }
        public virtual string LocationName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual int ChannelPartnerId { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string TimezoneID { get; set; }
        public virtual int NoOfCounterIDs { get; set; }
    }
}
