using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Common
{
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

    public enum ProductType
    {
        BillPay,
        Checks,
        SendMoney,
        GPRLoad,
        GPRWithdraw,
        GPRActivation,
        CashIn,
        CashOut,
        None,
        MoneyOrder,
        ReceiveMoney,
        Refund,
        AddOnCard
    }

    public enum TransactionStatus
    {
        Pending = 1,
        Authorized = 2,
        Authorization_Failed = 3,
        Committed = 4,
        Failed = 5,
        Cancelled = 6,
        Expired = 7,
        Declined = 8,
        Processing = 12
    }

    public enum CardSearchType
    {
        Swipe = 1,
        Enter = 2,
        Other = 3
    }
}