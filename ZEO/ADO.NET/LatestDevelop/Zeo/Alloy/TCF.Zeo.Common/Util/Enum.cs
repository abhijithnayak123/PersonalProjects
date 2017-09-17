using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TCF.Zeo.Common.Util
{
    public static partial class Helper
    {
        public enum ProfileStatus
        {
            Active = 1,
            Inactive = 0,
            Closed = 2
        };

        public enum TaxIDCode
        {
            [XmlEnum(Name = "SSN")]
            S = 1,
            [XmlEnum(Name = "ITIN")]
            I
        }

        public enum Gender
        {
            MALE = 1,
            FEMALE = 2
        }

        public enum Language
        {
            English,
            Spanish
        }

        public enum CardSearchType
        {
            None = 0,
            Swipe = 1,
            Enter = 2,
            Other = 3
        }

        public enum ProviderId
        {
            // Cash/Other 1 - 99
            Cash = 1,

            Alloy = 100,

            // Funds 101 - 199		
            FirstView = 101,
            TSys = 102,
            Visa = 103,

            // Checks 200 - 299
            Ingo = 200,
            Certegy = 201,

            // Money Transfer 300 - 399
            NexxoMoneyTransfer = 300,
            WesternUnion = 301,
            MoneyGram = 302, //Changes for MGI 

            // Bill Pay AND TopUp 400 - 499
            CheckFree = 400,
            WesternUnionBillPay = 401,
            TIO = 402,
            Movilix = 403,
            MoneyGramBillPay = 405,

            // Money Order 500 - 599
            OrderExpress = 500,
            WoodForest = 501,
            Nexxo = 502,
            Continental = 503,
            TCF = 504,
            MGIMoneyOrder = 505,

            //Customer 600 - 699
            FIS = 600,
            CCISCustomer = 601,
            TCISCustomer = 602
        }

        public enum TransactionStates
        {
            Pending = 1,
            Authorized = 2,
            AuthorizationFailed = 3,
            Committed = 4,
            Failed = 5,
            Canceled = 6,
            Expired = 7,
            Declined = 8,
            Initiated = 9,
            Hold = 10,
            Priced = 11,
            Processing = 12,
            CommittedReversed = 14,
            Staged = 15
        }

        public enum ProductCode
        {
            Alloy = 1000,
            Customer = 1001,
            CheckProcessing = 1002,
            Funds = 1003,
            BillPay = 1004,
            MoneyTransfer = 1005,
            MoneyOrder = 1006,
            Cash = 1007,
            Compliance = 1008,
            Partner = 1010,
            ClientCustomer = 1011,
            SynovusCustomer = 1013
        }

        public enum FeeAdjustmentConditions
        {
            Group = 1,
            Location = 2,
            TransactionAmount = 3,
            TransactionCount = 4,
            CheckType = 5,
            RegistrationDate = 6,
            DaysSinceRegistration = 7,
            Referral = 8, //US1800 Referral promotions – Free check cashing to referrer and referee 
            Code = 9, //US1799 Targeted promotions for check cashing and money order
            Aggregate = 10
        }

        public enum CompareTypes
        {
            Equal = 1,
            NotEqual = 2,
            In = 3,
            NotIn = 4,
            GreaterThan = 5,
            LessThan = 6,
            GreaterThanOrEqual = 7,
            LessThanOrEqual = 8,
            Between = 9
        }

        public enum PromotionType
        {
            Code,
            Referral
        }

        public enum ShoppingCartStatus
        {
            Active = 1,
            Parked = 2,
            Closed = 3
        }

        public enum TransactionType
        {
            ProcessCheck = 1,
            BillPayment = 2,
            MoneyTransfer = 3,
            ReceiveMoney = 4,
            MoneyOrder = 5,
            ProductCredential = 6,
            TransactionHistory = 7,
            Fund = 8,
            Cash = 9,
            CashWithdrawal = 10,
            LoadToGPR = 11,
            ActivateGPR = 12,
            DebitGPR = 13
        }

        public enum CashType
        {
            CashIn = 1,
            CashOut = 2
        }

        public enum TransactionBehavior
        {
            Credit = 1,
            Debit = 2
        }
        public enum CheckStatus
        {
            Unknown = 0,
            Pending = 1,
            Approved = 2,
            Declined = 10,
            Cashed = 11,
            Canceled = 12,
            Failed = 13
        }

        public enum DeliveryServiceType
        {
            Method,
            Option
        }

        public enum FeeRequestType
        {
            AmountExcludingFee,
            AmountIncludingFee,
            ReceiveAmount
        }

        public enum MoneyTransferType
        {
            Send = 1,
            Receive = 2,
            Refund = 3
        }

        public enum SearchRequestType
        {
            Modify,
            Refund,
            RefundWithStage,
            LookUp
        }

        public enum RequestType
        {
            Hold = 1,
            Release = 2,
            Cancel = 3
        }
        public enum UpdateTxType
        {
            [Description("FeeRequest")]
            FeeRequest = 0,
            [Description("ValidateRequest")]
            ValidateRequest = 1,
            [Description("SendMoneyStore")]
            SendMoneyStore = 2,
            [Description("FeeInquiry")]
            FeeInquiry = 3
        }

        public enum TransactionSubType
        {
            Cancel = 1,
            Modify = 2,
            Refund = 3
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

        public enum Product
        {
            ProcessCheck = 1,
            BillPayment = 2,
            MoneyTransfer = 3,
            ReceiveMoney = 4, //Note: we are not using 'Receive Money' Enum 
            MoneyOrder = 5,
            Fund = 6,
            Cash = 7,
        }

        public enum FundType
        {
            Debit,
            Credit,
            Activation,
            AddOnCard
        }

        public enum ShoppingCartItemStatus
        {
            Added,
            Removed
        }

        public enum ShoppingCartCheckoutStatus
        {
            InitialCheckout = 1,
            CashOverCounter = 2,
            CashCollected = 3,
            FinalCheckout = 4,
            Completed = 5,
            MOPrinting = 6,
            MOPrintingCancelled = 7
        }
        public enum RefundStatus
        {
            F,
            N
        }

        public enum SendType
        {
            Fxd,
            Estd
        }

        public enum CheckEntryTypes
        {
            ScanWithImage = 1,
            ScanWithoutImage = 2,
            Manual = 3
        }

        public enum TerminalIdentificationMechanism
        {
            YubiKey = 1,
            Cookie = 2,
            HostName = 3
        }

        public enum UserRoles
        {
            Teller = 1,
            Manager = 2,
            ComplianceManager = 3,
            SystemAdmin = 4,
            Tech = 5
        }

        public enum AuthenticationStatus
        {
            Authenticated = 1,
            TempPassword = 2,
            PasswordExpired = 3,
            Failed = 4,
            LockedOut = 5,
            Disabled = 6
        }

        public enum ErrorType
        {
            INFO,
            WARNING,
            ERROR
        }

        public enum TransactionStatus
        {
            Posted,
            Pending,
            Denied
        }

        public enum TransactionTypes
        {
            Cash = 1,
            Check = 2,
            Funds = 3,
            BillPay = 4,
            MoneyOrder = 5,
            MoneyTransfer = 6,
            CashWithdrawal = 7,
            LoadToGPR = 8,
            ActivateGPR = 9,
            DebitGPR = 10
        }

        public enum LocationErrorCode
        {
            LOCATIONNAME_ALREADY_EXIST = 1,
            BANK_BRANCH_ID_ALREADY_EXIST = 2,
            LOCATIONIDENTIFIER_ALREADY_EXIST = 3
        }

        public enum RefundType
        {
            FullAmount = 1,
            PrincipalAmount = 2
        }

        public enum CustomerScreen
        {
            PersonalInfo,
            Identification,
            Employment,
            PinDetails,
            ProfileSummary
        }

        public static class CacheKeys
        {
            public static string ChannelPartnerConfiguration = "ChannelPartnerConfiguration";
            public static string GetPartnerGroups = "GetPartnerGroups";

        }

        public enum TransactionScopeOptions
        {
            Required = 0,
            RequiresNew = 1,
            Suppress = 2
        }
        public enum CheckTypes
        {
            Cashier = 1,
            GovtUSTreasury = 2,
            GovtUSOther = 3,
            MoneyOrder = 5,
            PayrollHandwritten = 6,
            PayrollPrinted = 7,
            TwoParty = 10,
            LoanRAL = 14
        }

        public enum CardType
        {
            None,
            TCF,
            ZEO
        }

        public enum CustomerType
        {
            ZEO,
            RCIF
        }
    }
}
