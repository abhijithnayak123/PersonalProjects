using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Biz.CPEngine.Contract
{
    public class BizCPEngineException : NexxoException
    {
        const int MAJOR_CODE = 1002;

        public static readonly int MINIMUM_CHECK_AMOUNT_BREACHED = 6000;
        public static readonly int LOCATION_NOT_SET = 6001;
        public static readonly int CHECKFRANK_TEMPLATE_NOT_FOUND = 6002;

        static public readonly int UNABLE_TO_SEE_CHECK_IMAGE = 4;
        static public readonly int TELLER_USED_WRONG_CUSTOMER = 38;
        static public readonly int APPROVED_AFTER_CUSTOMER_ENDROSES = 51;
        static public readonly int CUSTOMER_ID_NOT_ACCEPTABLE = 52;
        static public readonly int DUPLICATE_CHECK_A = 7;
        static public readonly int DUPLICATE_CHECK_B = 37;
        static public readonly int DUPLICATE_CHECK_UNIDOS = 54;
        static public readonly int NEED_INFO_FROM_CUSTOMER = 40;
        static public readonly int UNABLE_CONTACT_MAKER = 2;
        static public readonly int CHECK_IS_NOT_FILLED_OUT = 46;
        static public readonly int CANNOT_VERIFY_MAKERS_BUISNESS = 47;
        static public readonly int NO_MORE_CHECKS_TODAY = 25;
        static public readonly int FARDULENT_CHECK = 18;
        static public readonly int NON_SUFFICENT_FUNDS_IN_MAKER = 1;
        static public readonly int BANK_ACCOUNT_CLOSED = 31;
        static public readonly int CHECK_ALTERED = 24;
        static public readonly int RISK_TO_HIGH = 33;
        static public readonly int STOP_PAYMENT_CHECK = 30;
        static public readonly int SUSPICIOUS_ACTIVITY = 34;
        static public readonly int CHECK_FROM_OUT_OF_COUNTRY = 35;
        static public readonly int CHECK_TOO_OLD = 6;
        static public readonly int CHECK_DECLINED_BY_MANAGMENT = 19;
        static public readonly int CREDIT_CARD_CHECK = 29;
        static public readonly int STARTER_CHECK = 20;
        static public readonly int NOT_A_CHECK = 44;
        static public readonly int DO_NOT_CASH = 5;
        static public readonly int UNTILL_NORMAL_BUISNESS_HOURS = 49;
        static public readonly int AMOUNT_DO_NOT_MATCH = 13;
        static public readonly int ALL_PAYEES_NOT_PRESENT = 42;
        static public readonly int BANK_NEEDS_CANCEL = 41;
        static public readonly int CHECK_ENDORSED_INCORRECTLY = 12;
        static public readonly int CHECK_NOT_SIGNED = 11;
        static public readonly int CHECK_SUBMITTED_AFETR_HOURS = 26;
        static public readonly int CHECK_CASHING_EXCEEDED = 50;
        static public readonly int DATA_FIELD_MISSING = 14;
        static public readonly int MAKER_AND_PAYEE_SAME_PERSON = 36;
        static public readonly int MAKER_PAYEE_SAME_ADDRESS = 23;
        static public readonly int MAKER_CHECK_TOLD_NOT_TO_CASH = 27;
        static public readonly int MAKER_TEMPORARILY_UNAVALIABLE = 45;
        static public readonly int MAKER_WILL_NOT_VERIFY_CHECK = 43;
        static public readonly int MONEY_ORDER_CANNOT_BE_CASHED = 32;
        static public readonly int CUSTOMER_WITHDREW_TRANSACTION = 3;
        static public readonly int TELLER_NOT_AVILIABLE = 48;
        static public readonly int POST_DATED_CHECK = 10;
        static public readonly int SHOULD_BE_CHECKED_REGIONAL_BANK = 53;
        static public readonly int CHECK_MONEYORDER_SUSPICIOUS = 55;
        static public readonly int PAYEE_VERIFICATION_FAILED = 57;
        static public readonly int AMOUNT_EXCEEDS_CARD_CAPACITY = 58;
        static public readonly int CUSTOMER_RISK_SCORE_TOOO_HIGH = 59;
        static public readonly int LIMIT_EXCEEDED = 60;
		static public readonly int UNHANDLED_DECLINE_CODE = 0;

        public BizCPEngineException(int MinorCode, string Message)
            : this(MinorCode, Message, null)
        {
        }

        public BizCPEngineException(int MinorCode)
            : this(MinorCode, string.Empty)
        {
        }

        public BizCPEngineException(int MinorCode, Exception innerException)
            : this(MinorCode, string.Empty, innerException)
        {
        }

        public BizCPEngineException(int MinorCode, string Message, Exception innerException)
            : base(MAJOR_CODE, MinorCode, Message, innerException)
        {
        }
    }
}
