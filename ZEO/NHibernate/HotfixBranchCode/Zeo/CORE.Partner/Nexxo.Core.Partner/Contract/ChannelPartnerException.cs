using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Core.Partner.Contract
{
	public partial class ChannelPartnerException : NexxoException
	{
		const int CHANNEL_PARTNER_EXCEPTION_MAJOR_CODE = 1010;

		public ChannelPartnerException(int MinorCode, string Message)
			: this(MinorCode, Message, null)
		{
		}

		public ChannelPartnerException(int MinorCode)
			: this(MinorCode, string.Empty)
		{
		}

		public ChannelPartnerException(int MinorCode, Exception innerException)
			: this(MinorCode, string.Empty, innerException)
		{
		}

		public ChannelPartnerException(int MinorCode, string Message, Exception innerException)
			: base(CHANNEL_PARTNER_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
		{
		}
		//Partner Exceptions common From 3000 - 3099
		//Agent   Exceptions        From 3100 - 3199
		//Location   Exceptions     From 3200 - 3299
		//Terminal   Exceptions     From 3300 - 3399
		//User   Exceptions         From 3400 - 3499
		//DataStructure Exceptions  From 3500 - 3599
		//Shopping Cart Exceptions  From 3600 - 3699

		static public int CHANNEL_PARTNER_NOT_FOUND = 3000;
		static public int CHANNEL_PARTNER_CHECKTYPES_NOT_FOUND = 3001;
		static public int CHANNEL_PARTNER_CHECKTYPE_NOT_FOUND = 3002;
		static public int CHANNEL_PARTNER_CHECK_FEE_NOT_FOUND = 3003;
		static public int CHANNEL_PARTNER_FUND_FEE_NOT_FOUND = 3004;
		static public int CHANNEL_PARTNER_LEDGER_CREATE_FAILED = 3005;
		static public int EMAIL_SETTINGS_SAVE_FAILED = 3006;
		static public int CHANNEL_PARTNER_EMAIL_SETTINGS_NOT_FOUND = 3007;
		static public int LOCATIONS_NOT_FOUND_FOR_CHANNEL_PARTNER = 3008;
		static public int CHANNEL_PARTNER_MONEYORDER_FEE_NOT_FOUND = 3009;
		static public int GROUP_NOT_FOUND = 3010;
		static public int GROUP_INSERT_FAILED = 3011;
		static public int GROUP_UPDATE_FAILED = 3012;
		static public int CHANNEL_PARTNER_BILLPAY_FEE_NOT_FOUND = 3010;
		static public int AGENT_MESSAGE_CREATE_FAILED = 3013;
		static public int AGENT_MESSAGE_UPDATE_FAILED = 3014;
		static public int AGENT_MESSAGE_DELETE_FAILED = 3015;
		static public int CUSTOMERSESSION_COUNTERID_CREATE_FAILED = 3016;
		static public int PROMOCODE_INVALID = 3017; //US1799 Targeted promotions for check cashing and money order
		static public int DUPLICATE_LOCATIONID = 3018;
		static public int CHANNEL_PARTNER_CERTIFICATE_INFO_NOT_FOUND = 3019;
	}
}
