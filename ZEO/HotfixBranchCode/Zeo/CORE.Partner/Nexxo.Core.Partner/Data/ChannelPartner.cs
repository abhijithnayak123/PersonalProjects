using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class ChannelPartner : NexxoModel
    {
        public virtual string Name { get; set; }

        public virtual bool FeesFollowCustomer { get; set; }

        public virtual string CashFeeDescriptionEN { get; set; }

        public virtual string CashFeeDescriptionES { get; set; }

        public virtual string DebitFeeDescriptionEN { get; set; }

        public virtual string DebitFeeDescriptionES { get; set; }

        public virtual decimal ConvenienceFeeCash { get; set; }

        public virtual decimal ConvenienceFeeDebit { get; set; }

        public virtual string ConvenienceFeeDescriptionEN { get; set; }

        public virtual string ConvenienceFeeDescriptionES { get; set; }

        public virtual bool CanCashCheckWOGovtId { get; set; }

        public virtual string LogoFileName { get; set; }

        public virtual bool IsEFSPartner { get; set; }

        public virtual int EFSClientId { get; set; }

        public virtual bool UsePINForNonGPR { get; set; }

        public virtual bool IsCUPartner { get; set; }

        public virtual bool HasNonGPRCard { get; set; }

        public virtual bool ManagesCash { get; set; }

        public virtual bool AllowPhoneNumberAuthentication { get; set; }

        public string AuthenticationType { get; set; }

        public string PhoneNumber { get; set; }

		public virtual short TIM { get; set; }

		public virtual string ComplianceProgramName { get; set; }

		//TA3520 - Start
		public int CardPresenceVerificationConfig { get; set; }
		//TA3520 - End


		public virtual ChannelPartnerConfig ChannelPartnerConfig { get; set; }

        public virtual ICollection<ChannelPartnerProductProvider> Providers { get; set; }

		public virtual ICollection<Prospect> Prospects { get; set; }		

		public virtual ICollection<Customer> Customers { get; set; }
    }

}
