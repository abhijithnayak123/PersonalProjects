using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Common
{
	public static class Constants
	{
		//Constants for Prepaid card transaction type
		public const string PREPAID_CARD_ACTIVATE = "None";
		public const string PREPAID_CARD_LOAD = "Credit";
		public const string PREPAID_CARD_WITHDRAW = "Debit";
		public const string PREPAID_CARD_NOT_ACTIVE = "BLOCKED/INACTIVE";
		public const string PREPAID_CARD_ADDONCARD = "AddOnCard";

		//Transaction status
		public const string STATUS_PENDING = "1";
		public const string STATUS_AUTHORIZED = "2";
		public const string STATUS_AUTHORIZATION_FAILED = "3";
		public const string STATUS_COMMITTED = "4";
		public const string STATUS_FAILED = "5";
		public const string STATUS_CANCELED = "6";
		public const string STATUS_EXPIRED = "7";
		public const string STATUS_DECLINED = "8";
		public const string STATUS_PROCESSING = "12";
	}

	public enum Resolutions
	{
		No_Discrepancy,
		Verified_DOB_with_Birth_certificate,
		Verified_with_Articles_of_Incorporation,
		Verified_with_Auto_Insurance_Card,
		Verified_with_Business_License,
		Verified_with_Court_Ordered_Name_change,
		Verified_with_Credit_Card,
		Verified_with_Employee_ID_Card,
		Verified_with_Fire_Arms_Permit_ID,
		Verified_with_Lease_Or_Mortgage,
		Verified_with_Medicaid_Card,
		Verified_with_Medical_Insurance_Card,
		Verified_with_School_ID_Card,
		Verified_with_Social_Security_Administration_Or_IRS,
		Verified_with_Utility_Bill,
		Verified_with_Voter_Registration_Card
	}

	public enum PayStatus
	{
		BLANK,
		PAID,
		CAN,
		PURG,
		OVLM,
		OVLQ,
		SECQ,
		QQC1,
		BUST,
		OFAC,
		FBST,
		FBLK,
		ACPT,
		AUTH,
		QQC2,
		ACRQ,
		CUBA,
		SWPA,
		HOLD,
		PKUP,
		PKPQ,
		UNAV,
		PHD
	}
}