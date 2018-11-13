using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.Util;

namespace MGI.Core.Partner.Data
{
	public class ChannelPartnerConfig
	{
		public virtual ChannelPartner ChannelPartner { get; set; }
		public virtual Guid ChannelPartnerID { get; set; }
		public virtual bool DisableWithdrawCNP { get; set; }
		public virtual bool CashOverCounter { get; set; }

		//US1421 Changes
		public virtual string FrankData { get; set; }		
		public virtual bool IsCheckFrank { get; set; }
		public virtual bool IsNotesEnable { get; set; }
  		//US1800 Referral promotions – Free check cashing to referrer and referee 
        public virtual bool IsReferralSectionEnable { get; set; }
		//AL-291 
		public virtual bool IsMGIAlloyLogoEnable { get; set; }

		public virtual string MasterSSN { get; set; }

				
		//Developed by: Sunil Shetty Date: 09/06/2015
		//AL-533 : A new boolean value is introduced to configure Mailing address option in Customer registration page
		public virtual bool IsMailingAddressEnable { get; set; }

		//AL-586
		public virtual bool CanEnableProfileStatus { get; set; }

		//AL-1626
		public virtual int CustomerMinimumAge { get; set; }
	}
}
