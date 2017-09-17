using System;
using System.Collections.Generic;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Data
{
	public class ChannelPartner
	{
		public Guid rowguid { get; set; }

		public long Id { get; set; }

		public string Name { get; set; }

		public bool FeesFollowCustomer { get; set; }

		public string CashFeeDescriptionEN { get; set; }

		public string CashFeeDescriptionES { get; set; }

		public string DebitFeeDescriptionEN { get; set; }

		public string DebitFeeDescriptionES { get; set; }

		public decimal ConvenienceFeeCash { get; set; }

		public decimal ConvenienceFeeDebit { get; set; }

		public string ConvenienceFeeDescriptionEN { get; set; }

		public string ConvenienceFeeDescriptionES { get; set; }

		public bool CanCashCheckWOGovtId { get; set; }

		public string LogoFileName { get; set; }

		public bool IsEFSPartner { get; set; }

		public int EFSClientId { get; set; }

		public bool UsePINForNonGPR { get; set; }

		public bool IsCUPartner { get; set; }

		public bool HasNonGPRCard { get; set; }

		public bool ManagesCash { get; set; }

		public bool AllowPhoneNumberAuthentication { get; set; }

		public string AuthenticationType { get; set; }

		public string PhoneNumber { get; set; }

		public short TIM { get; set; }

        public bool DisableWithdrawCNP { get; set; }

        public bool CashOverCounter { get; set; }

		//US1421 Changes
		public string FrankData { get; set; }		
		public bool IsCheckFrank { get; set; }
		public bool IsNotesEnable { get; set; }

        public List<ChannelPartnerProductProvider> Providers { get; set; }
		
		public bool IsReferralSectionEnable { get; set; }

		public bool IsMGIAlloyLogoEnable { get; set; }

		public string MasterSSN { get; set; }
		
		//Developed by: Sunil Shetty || Date: 09\06\2015
		//AL-533 : Mailing Address will not be visible in Customer registration on the base of IsMailingAddress value
		public bool IsMailingAddressEnable { get; set; }

		//AL-586
		public virtual bool CanEnableProfileStatus { get; set; }

		//AL-1626
		public int CustomerMinimumAge { get; set; }
	}
}
