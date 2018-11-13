using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Cxn.Fund.Contract
{
    public class FundException : NexxoException
    {
        const int FUND_EXCEPTION_MAJOR_CODE = 1003;

        public FundException(int MinorCode, string Message, Exception innerException)
            : base(FUND_EXCEPTION_MAJOR_CODE, MinorCode, Message, innerException)
        {
        }

        public FundException(int MinorCode, Exception innerException)
            : base(FUND_EXCEPTION_MAJOR_CODE, MinorCode, innerException)
        {
        }

        public FundException(int MinorCode, string Message)
            : base(FUND_EXCEPTION_MAJOR_CODE, MinorCode, Message)
        {
        }

        public static int FIRSTVIEW_CREDENTIALS_NOTFOUND = 2000;
        public static int ACCOUNT_NOT_FOUND = 2001;
        public static int TRANSACTION_NOT_FOUND = 2003;
        public static int CONTEXT_NOT_FOUND = 2004;
        public static int INVALID_CONTEXT_FOUND = 2005;
        public static int RETREIVE_CARD_BALANCE_ERROR = 2006;
        public static int POST_DEBIT_TRANSACTION_ERROR = 2007;
        public static int POST_CREDIT_TRANSACTION_ERROR = 2008;
        public static int INVALID_CARD_NUMBER = 2009;
        public static int CARD_ACTIVATION_FAILED = 2010;
        public static int INVALID_CUSTOMER_DETAILS = 2011;
        public static int INVALID_MAILINGADDRESS = 2012;
        public static int CARD_ALREADY_REGISTERED = 2013;
        public static int INVALID_IDISSUE_DATE = 2014;
        public static int GOVT_IDTYPE_NOT_FOUND = 2015;
        public static int INVALID_IDEXPIRY_DATE = 2016;
        public static int INVALID_GOVTID_DETAILS = 2017;
        public static int POST_TRANSACTION_FAILED = 2018;
        public static int CARD_NOT_ACTIVATED = 2019;
        public static int PROVIDER_ERROR = 2020;
       
        public static int ACCESS_DENIED = 2021;
        public static int INVALID_ACCOUNT_NUMBER = 2022;
        public static int INVALID_ADMIN_NUMBER = 2023;
        public static int INVALID_TRANSACTION_AMOUNT = 2024;
        public static int INVALID_TRANSACTION_TYPE = 2025;
        public static int INVALID_MSG_TYPE_IDENTIFIER = 2026;
        public static int TRAN_CODE_IS_NOT_PART_OF_MSG_TYPE_IDENTIFIER = 2027;
        public static int INVALID_TRANSACTION_CURRENCY_CODE = 2028;
        public static int MISMATCH_TRANSACTION_CURRENCY_CODE = 2029;
        public static int DISALLOW_DUPLICATE_TRANSACTION = 2030;
        public static int INVALID_EFFECTIVE_DATE = 2031;
        public static int FUTURE_DATE_TRANSACTIONS_NOT_ALLOWED = 2032;
        public static int INVALID_DEFERMENT = 2033;
        public static int REVERSAL_TARGET_NOT_FOUND = 2034;
        public static int PAYMENT_REVERSAL_TARGET_NOT_FOUND = 2035;
        public static int POSTING_FAILED_FOR_CARDHOLDER = 2036;
        public static int BATCH_ACCOUNT_POSTING_FAILED = 2037;
        public static int PLAN_TYPE_DOES_NOT_MATCH = 2038;
        public static int ERROR_OCCURRED_DURING_ACCOUNT_LOAD = 2039;
        public static int TRANSACTION_REQUEST_IS_ACCEPTED = 2040;
        public static int ERROR_OCCURRED_WHILE_SAVING = 2041;
        public static int LOAD_AMT_GRT_LSR_MIN_MAX_LOAD_AMT = 2042;
        public static int EXCEED_LIMIT_OF_DAILY_LOADS = 2043;
        public static int EXCEED_LIMTIT_WEEKLY_LOADS = 2044;
        public static int EXCEED_LIMTIT_MONTHLY_LOADS = 2045;
        public static int EXCEED_LIMTIT_YEARLY_LOADS = 2046;
        public static int EXCEED_LIMTIT_LIFETIME_LOADS = 2047;
        public static int MAXIMUM_RELOADS_MUST_BE_DEFINE = 2048;
        public static int RELOAD_NOT_ALLOWED = 2049;
        public static int PENDING_LOAD_REQUEST_INTO_SYSTEM = 2050;
        public static int INVALID_CARD_OR_DDA_NUMBER = 2051;
        public static int INVALID_COUNTRY_CODE = 2052;
        public static int INVALID_STATE_DIFF_THAN_GIVEN_COUTNRY = 2053;
        public static int CAN_NOT_UPDATE_CARD = 2054;
        public static int INVALID_USERFIELD1_CURRENCY = 2055;
        public static int INVALID_USERFIELD2_CURRENCY = 2056;
        public static int INVALID_USERFIELD3_CURRENCY = 2057;
        public static int INVALID_USERFIELD4_CURRENCY = 2058;
        public static int INVALID_USERFIELD5_CURRENCY = 2059;
        public static int INVALID_DATE_OF_BIRTH = 2060;
        public static int DOB_MUST_BE_EQUAL_OR_LESS_THAN_PRESENT_DATE = 2061;
        public static int INVALID_EMAIL_ID = 2062;
        public static int INVALID_EXPIRATION_DATE = 2063;
        public static int ID_EXPIRATION_MUST_HAVE_FUTURE_DATE = 2064;
        public static int INVALID_GOVT_ID = 2065;
        public static int INVALID_ISSUE_DATE = 2066;
        public static int ISSUE_DATE_LESSTHAN_GREATER_THAN_DOB = 2067;
        public static int ID_STATE_COUNTRY_ISSUE_DATE_CANNOT_BE_LEFT_BLANK = 2068;
        public static int ID_NUMBER_COUNTRY_CANNOT_BE_LEFT_BLANK = 2069;
        public static int GENERATE_NEW_CARDS_TAPE_VALUE = 2070;
        public static int NO_RECORDS_FOUND_FOR_USER = 2071;
        public static int SHIP_ADDRESS1_STATE_CITY_ZIP_CANNOT_NULL = 2072;
        public static int INVALID_REISSUE_FLAG_VALUE = 2073;
        public static int CARD_MAY_NOT_BE_REISSUE_RECENTLY_REISUUED = 2074;
        public static int CARD_MAY_NOT_BE_REISSUE_CARD_IS_NOT_ACTIVE = 2075;
        public static int INVALID_STATE = 2076;
        public static int INVALID_SHIP_STATE = 2078;
        public static int STATE_CANNOT_BE_NULL = 2079;
        public static int INVALID_USERFIELD1_DATE = 2080;
        public static int INVALID_USERFIELD2_DATE = 2081;
        public static int INVALID_USERFIELD3_DATE = 2082;
        public static int INVALID_USERFIELD4_DATE = 2083;
        public static int INVALID_USERFIELD5_DATE = 2084;
        public static int SSN_SHOULD_NEVER_START_WITH_799 = 2085;
        public static int INVALID_SSN_NUMBER = 2086;
        public static int INVALID_ID_NUMBER = 2087;
        public static int CARDNUMBER_CANNOT_BE_BLANK = 2088;
		public static int PAN_NUMBER_MISMATCH = 2090;
		public static int ACCOUNT_FLAGGED_AS_FRAUDULENT = 2098;
		public static int CARD_NOT_ASSOCIATED_WITH_THIS_ACCOUNT = 2099;
		public static int USER_NOT_OPEN = 2100;
		public static int NO_ACCOUNT_ASSOCIATED_TO_USER = 2101;
		public static int FEE_CHARGE_FAILURE = 2102;
		public static int CARD_NOT_ACTIVE = 2103;
		public static int CARD_CANNOT_BE_ACTIVATED = 2104;
		public static int NO_ACTIVE_CARD = 2105;
		public static int PSEUDO_DDA_MISMATCH = 2106;

		public static int COMMUNICATION_ERROR = 2107;
		public static int CARD_REGISTRATION_ERROR = 2108;
		public static int CARD_ALREADY_ISSUED = 2109;

		public static int CARD_ASSOCIATION_ERROR = 2110;

		public static int CARD_MAPPING_ERROR = 2111;
		public static int SSN_MISMATCH_ERROR = 2112;
	}
}
