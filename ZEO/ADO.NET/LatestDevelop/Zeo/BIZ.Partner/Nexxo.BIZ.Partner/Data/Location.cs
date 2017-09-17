using System;
using System.Collections.Generic;

namespace MGI.Biz.Partner.Data
{
    public class Location
    {
        public Guid RowGuid { get; set; }
        public long Id { get; set; }
        public string BankID { get; set; }
        public string BranchID { get; set; }
		public string LocationIdentifier { get; set; }
        public string LocationName { get; set; }
        public bool IsActive { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        //public DateTime DtCreate { get; set; }
        //public DateTime DtLastMod { get; set; }
        public long ChannelPartnerId { get; set; }
        public string PhoneNumber { get; set; }
        public string TimezoneID { get; set; }
		public int NoOfCounterIDs { get; set; }
        public List<ProcessorCredentials> ProcessorCredentials { get; set; }
    }
}
