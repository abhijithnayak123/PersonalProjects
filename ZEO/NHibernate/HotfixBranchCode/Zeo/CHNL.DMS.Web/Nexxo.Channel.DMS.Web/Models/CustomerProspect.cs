using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class CustomerProspect
    {
		public long AlloyID { get; set; }

        public bool IsNewCustomer { get; set; }

        public PersonalInformation PersonalInformation { get; set; }

        public IdentificationInformation IdentificationInformation { get; set; }

        public EmploymentDetails EmploymentDetails { get; set; }

        public PinDetails PinDetails { get; set; }

        public ProfileSummary ProfileSummary { get; set; }

		public CustomerScreen CustomerScreen { get; set; }
    }
}