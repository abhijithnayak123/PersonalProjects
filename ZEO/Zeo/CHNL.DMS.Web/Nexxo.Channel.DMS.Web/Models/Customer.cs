using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Web.Models
{
    public class Customer
    {
		public long AlloyID { get; set; }

        public bool IsNewCustomer { get; set; }

        public PersonalInformation PersonalInformation { get; set; }

        public IdentificationInformation IdentificationInformation { get; set; }

        public EmploymentDetails EmploymentDetails { get; set; }

        public PinDetails PinDetails { get; set; }

        public ProfileSummary ProfileSummary { get; set; }

		public Helper.CustomerScreen CustomerScreen { get; set; }

        public string CardNumber { get; set; }
    }
}