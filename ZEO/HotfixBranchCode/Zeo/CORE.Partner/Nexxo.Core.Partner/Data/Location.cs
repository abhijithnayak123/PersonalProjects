﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data
{
    public class Location : NexxoModel
    {
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
		public virtual LocationCounterId LocationCounterId { get; set; }
        public virtual IList<LocationProcessorCredentials> LocationProcessorCredentials { get; set; }
    }
}
