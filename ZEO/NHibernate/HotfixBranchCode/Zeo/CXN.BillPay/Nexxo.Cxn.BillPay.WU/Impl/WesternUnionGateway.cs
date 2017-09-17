using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Cxn.BillPay.Contract;
using MGI.Cxn.BillPay.Data;
using MGI.Cxn.BillPay.WU.Data;
using MGI.Cxn.WU.Common.Contract;
using WUCommonData = MGI.Cxn.WU.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MGI.Common.Util;
using MGI.Common.DataProtection.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.BillPay.WU.Impl
{
    public class WesternUnionGateway : IBillPayProcessor
    {
        #region Private Members
        WUCommonData.WUBaseRequestResponse _response = null;

        #endregion

        #region Dependencies
        public IWUCommonIO WesternUnionIO { private get; set; }
        public IIO WUBillPayIO { private get; set; }
        public ProcessorDAL ProcessorDAL { private get; set; }
        public IDataProtectionService BPDataProtectionSvc { private get; set; }
        public bool IsHardCodedCounterId { get; set; }
        public NLoggerCommon NLogger = new NLoggerCommon();
        internal Dictionary<string, string> _deliveryCodeMapping = new Dictionary<string, string>();
        public TLoggerCommon MongoDBLogger { private get; set; }
        #endregion

        public WesternUnionGateway()
        {
            PopulateDeliveryMethods();
            Mapper.CreateMap<BillPayTransaction, WesternUnionTrx>()
                .ForMember(d => d.WesternUnionAccount, s => s.MapFrom(c => c.Account)); ;

            Mapper.CreateMap<WesternUnionTrx, BillPayTransaction>()
                .ForMember(d => d.Id, s => s.MapFrom(c => c.Id))
                .ForMember(d => d.BillerName, s => s.MapFrom(c => c.BillerName))
                .AfterMap((s, d) =>
                {
                    d.AccountNumber = Decrypt(s.CustomerAccountNumber);
                })
                .ForMember(d => d.Amount, s => s.MapFrom(c => c.FinancialsOriginatorsPrincipalAmount))
                .ForMember(d => d.Fee, s => s.MapFrom(c => c.FinancialsFee))
                .ForMember(d => d.Account, s => s.MapFrom(c => c.WesternUnionAccount))
                .ForMember(d => d.DTTerminalCreate, s => s.MapFrom(c => c.DTTerminalCreate))
                .ForMember(d => d.ConfirmationNumber, s => s.MapFrom(c => c.Mtcn))
                .AfterMap((s, d) =>
                              {
                                  d.MetaData = new Dictionary<string, object>()
                                                   {
                                                       {"MTCN", s.Mtcn},
                                                       {"WuCardTotalPointsEarned", s.WuCardTotalPointsEarned},
                                                       {"UnDiscountedFee", s.FinancialsUndiscountedCharges},
                                                       {"DiscountedFee", s.FinancialsTotalDiscount},
                                                       {"WesternUnionCardNumber", s.WesternUnionCardNumber},
                                                       {"FillingDate",s.FillingDate},
                                                       {"FillingTime",s.FillingTime},
                                                       {"PromoCoupon",s.PromotionsSenderPromoCode},
                                                       {
                                                           "DeliveryService",
                                                           string.IsNullOrWhiteSpace(s.DeliveryCode)
                                                               ? string.Empty
                                                               :_deliveryCodeMapping[s.DeliveryCode]
                                                           },
														   {"MessageArea",s.MessageArea}
                                                   };
                              });

            Mapper.CreateMap<BillPayAccount, WesternUnionAccount>();
            Mapper.CreateMap<WesternUnionAccount, BillPayAccount>();

            Mapper.CreateMap<WUCommonData.CardInfo, CardInfo>();
            Mapper.CreateMap<CardInfo, WUCommonData.CardInfo>();

            //Mapping from CXN-Receivers to the CXN-Biller Format.
            Mapper.CreateMap<WUCommonData.Receiver, Biller>()
                .ForMember(d => d.Name, s => s.MapFrom(c => c.BusinessName))
                .ForMember(d => d.AccountNumber, s => s.MapFrom(c => c.Attention))
                .ForMember(d => d.IndexNumber, s => s.MapFrom(c => c.ReceiverIndexNumber));
            Mapper.CreateMap<BillPayAccount, WesternUnionAccount>();


        }

        public long Validate(long cxnAccountID, BillPayRequest request, MGIContext mgiContext)
        {
            try
            {
                CheckCounterId(mgiContext);
                WesternUnionTrx trx = ProcessorDAL.GetTrxById(mgiContext.TrxId);

                BillPaymentRequest billPaymentRequest = BuildBillPayRequest(request);

                PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

                if (!mgiContext.Context.ContainsKey("BaseWUObject") || mgiContext.Context["BaseWUObject"] == null)
                {
                    mgiContext.Context.Add("BaseWUObject", _response);
                }
                long trxId = WUBillPayIO.ValidatePayment(billPaymentRequest, trx, mgiContext);
                return trxId;
            }
            catch (WUCommonException ex)
            {
                //		 Begin AL-471 Changes
                //       User Story Number: AL-471 | Web |   Developed by: Sunil Shetty     Date: 25.06.2015
                //       Purpose: This method takes only ssn exception message. We have found with ssn we have only 3 exception and below are the one
                string[] _minorCode = new string[] { "5050", "7490", "6008", "0425", "0415" };

                //AL-1014 Transactional Log User Story
                MongoDBLogger.Error<BillPayRequest>(request, "Validate", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in Validate - MGI.Cxn.BillPay.WU.Impl.WesternUnionGateway", ex.Message, ex.StackTrace);

                if (_minorCode.Contains(ex.MinorCode.ToString("D4")))
                    throw new WUCommonException(GetExceptionMessage(ex.MinorCode.ToString("D4")), ex.Message, ex);
                //		 END AL-471 Changes
                else
                    throw new WUCommonException(ex.MinorCode, ex.Message, ex.InnerException);
            }

        }

        public long Commit(long transactionID, MGIContext mgiContext)
        {
            CheckCounterId(mgiContext);
            WesternUnionTrx trx = ProcessorDAL.GetTrxById(transactionID);

            //long channelPartnerId = GetChannelPartnerId(mgiContext);
            PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

            if (!mgiContext.Context.ContainsKey("BaseWUObject") || mgiContext.Context["BaseWUObject"] == null)
            {
                mgiContext.Context.Add("BaseWUObject", _response);
            }
            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = trx.ProviderId;
            }
            long trxId = WUBillPayIO.MakePayment(trx, mgiContext);

            return trxId;
        }

        /// <summary>
        /// AL-491 While printing receipts updating Gold card points
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="totalPointsEarned"></param>
        /// <param name="mgiContext"></param>
        public void UpdateGoldCardPoints(long transactionId, string totalPointsEarned, MGIContext mgiContext)
        {

            if (string.IsNullOrEmpty(mgiContext.TimeZone))
            {
                throw new Exception("TimeZone not found in the context");
            }

            WesternUnionTrx transaction = ProcessorDAL.GetTrxById(transactionId);
            transaction.DTServerLastModified = DateTime.Now;
            transaction.WuCardTotalPointsEarned = totalPointsEarned;
            transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
            ProcessorDAL.WesternUnionTrxRepo.UpdateWithFlush(transaction);
        }

        public long AddBillPayAccount(BillPay.Data.BillPayRequest request, string timeZone)
        {
            WesternUnionAccount account = new WesternUnionAccount()
            {
                CardNumber = request.CardNumber,
                FirstName = request.CustomerFirstName,
                LastName = request.CustomerLastName,
                Address1 = request.CustomerAddress1,
                Address2 = request.CustomerAddress2,
                City = request.CustomerCity,
                State = request.CustomerState,
                PostalCode = request.CustomerZip,
                Street = request.CustomerStreet,
                //DateOfBirth = request.CustomerDateOfBirth.Equals(DateTime.MinValue) ? request.CustomerDateOfBirth : null,
                Email = request.CustomerEmail,
                ContactPhone = request.CustomerPhoneNumber,
                //timestamp changes
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timeZone),
                DTServerCreate = DateTime.Now,
                MobilePhone = request.CustomerMobileNumber
            };
            if (request.CustomerDateOfBirth == DateTime.MinValue)
            {
                account.DateOfBirth = null;
            }
            else
            {
                account.DateOfBirth = request.CustomerDateOfBirth;
            }
            return ProcessorDAL.AddAccount(account);
        }

        public BillPayAccount GetBillPayAccount(long cxnAccountID)
        {
            WesternUnionAccount account = ProcessorDAL.GetAccountById(cxnAccountID);
            return new BillPayAccount()
            {
                FirstName = account.FirstName,
                LastName = account.LastName,
                City = account.City,
                ContactPhone = account.ContactPhone,
                State = account.State,
                PostalCode = account.PostalCode,
                Address1 = account.Address1,
                Address2 = account.Address2,
                CardNumber = account.CardNumber,
                DateOfBirth = account.DateOfBirth,
                Email = account.Email,
                MobilePhone = account.MobilePhone,
                Street = account.Street,
                SmsNotificationFlag = account.SmsNotificationFlag,
                Id = account.Id,
                rowguid = account.rowguid,
                DTTerminalCreate = account.DTTerminalCreate,
                DTTerminalLastModified = account.DTTerminalLastModified,
                DTServerCreate = account.DTServerCreate,
                DTServerLastModified = account.DTServerLastModified
            };
        }

        public BillPayTransaction GetTransaction(long transactionID)
        {
            WesternUnionTrx trx = ProcessorDAL.GetTrxById(transactionID);
            BillPayTransaction billPayTrx = Mapper.Map<BillPayTransaction>(trx);
            return billPayTrx;
        }

        public List<Location> GetLocations(string billerName, string accountNumber, decimal amount, MGIContext mgiContext)
        {
            CheckCounterId(mgiContext);
            List<Location> locations = null;
            PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

            WesternUnionAccount account = null;
            BillPaymentRequest billPaymentRequest = null;

            if (!mgiContext.Context.ContainsKey("BaseWUObject") || mgiContext.Context["BaseWUObject"] == null)
            {
                mgiContext.Context.Add("BaseWUObject", _response);
            }

            if (mgiContext.CxnAccountId != 0)
            {
                account = ProcessorDAL.GetAccountById(mgiContext.CxnAccountId);
            }
            if (mgiContext.Context.ContainsKey("BillPayRequest"))
            {
                BillPayRequest request = mgiContext.Context["BillPayRequest"] as BillPayRequest;
                billPaymentRequest = BuildBillPayRequest(request);
                billPaymentRequest.AccountNumber = accountNumber;
                if (account != null) //US2150 - This can be avoided if UpdateCardAccount implemented
                    account.Address2 = request.CustomerAddress2;
            }

            locations = WUBillPayIO.GetLocations(billerName, accountNumber, amount, account, billPaymentRequest, mgiContext);

            return locations;
        }

        public Fee GetFee(string billerName, string accountNumber, decimal amount, Location location, MGIContext mgiContext)
        {
            CheckCounterId(mgiContext);
            PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);
            WesternUnionAccount account = null;
            BillPaymentRequest billPaymentRequest = null;

            if (mgiContext.CxnAccountId != 0)
            {
                account = ProcessorDAL.GetAccountById(mgiContext.CxnAccountId);
            }

            if (mgiContext.Context.ContainsKey("BillPayRequest"))
            {
                BillPayRequest request = mgiContext.Context["BillPayRequest"] as BillPayRequest;
                billPaymentRequest = BuildBillPayRequest(request);
                billPaymentRequest.AccountNumber = accountNumber;
            }

            if (!mgiContext.Context.ContainsKey("BaseWUObject") || mgiContext.Context["BaseWUObject"] == null)
            {
                mgiContext.Context.Add("BaseWUObject", _response);
            }

            Fee fee = WUBillPayIO.GetDeliveryMethods(billerName, accountNumber, amount, location, account, billPaymentRequest, mgiContext);
            return fee;
        }

        public BillerInfo GetBillerInfo(string billerName, MGIContext mgiContext)
        {
            CheckCounterId(mgiContext);
            PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

            if (!mgiContext.Context.ContainsKey("BaseWUObject") || mgiContext.Context["BaseWUObject"] == null)
            {
                mgiContext.Context.Add("BaseWUObject", _response);
            }

            return new BillerInfo() { Message = WUBillPayIO.GetBillerMessage(billerName, mgiContext) };
        }

        public List<Field> GetProviderAttributes(string billerName, string locationName, MGIContext mgiContext)
        {
            CheckCounterId(mgiContext);
            List<Field> fields = new List<Field>();
            PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

            if (!mgiContext.Context.ContainsKey("BaseWUObject") || mgiContext.Context["BaseWUObject"] == null)
            {
                mgiContext.Context.Add("BaseWUObject", _response);
            }
            fields = WUBillPayIO.GetBillerFields(billerName, locationName, mgiContext);
            return fields;
        }

        public long UpdateCardDetails(long cxnAccountId, string cardNumber, MGIContext mgiContext, string timezone)
        {
            WesternUnionAccount billPayAccount = ProcessorDAL.GetAccountById(cxnAccountId);
            billPayAccount.CardNumber = cardNumber;
            billPayAccount.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            billPayAccount.DTServerLastModified = DateTime.Now;
            ProcessorDAL.WesternUnionAccountRepo.Update(billPayAccount);

            return billPayAccount.Id;
        }

        public CardInfo GetCardInfo(string cardNumber, MGIContext mgiContext)
        {
            CheckCounterId(mgiContext);
            WUCommonData.CardLookUpRequest cardRequest = new WUCommonData.CardLookUpRequest()
            {
                sender = new WUCommonData.Sender()
                {
                    PreferredCustomerAccountNumber = cardNumber
                }
            };

            WUCommonData.CardInfo wuCardInfo = WesternUnionIO.GetCardInfo(cardRequest, mgiContext);

            CardInfo cardInfo = Mapper.Map<CardInfo>(wuCardInfo);

            return cardInfo;
        }

        public BillPayTransaction GetBillerLastTransaction(string billerName, long cxnAccountID, MGIContext mgiContext)
        {
            throw new NotImplementedException();
        }

        #region Method added for User Story # US1646 - Past Billers Data from WU Service.

        /// <summary>
        /// Method in the CXN Bill Pay Gateway to add to get "Past Billers" for User Story # US1646.
        /// </summary>
        /// <param name="customerSessionId">Session ID</param>
        /// <param name="cardNumber">Card Number</param>
        /// <param name="timeZone">Time Zone</param>
        /// <param name="mgiContext">Context</param>
        /// <returns></returns>
        public List<Biller> GetPastBillers(long customerSessionId, string cardNumber, MGIContext mgiContext)
        {
            CheckCounterId(mgiContext);
            List<Biller> billers = null;
            int errorCode = BillPayException.PROVIDER_IMPORT_FAILED;
            string errorMessage = "Error while importing billers from Western Union: {0}";
            try
            {
                MGI.Cxn.WU.Common.Data.CardLookupDetails cardlookupdetails = new MGI.Cxn.WU.Common.Data.CardLookupDetails();
                MGI.Cxn.WU.Common.Data.CardLookUpRequest cxncardlookupreq = new MGI.Cxn.WU.Common.Data.CardLookUpRequest()
                {
                    sender = new MGI.Cxn.WU.Common.Data.Sender()
                    {
                        PreferredCustomerAccountNumber = cardNumber,
                        //PreferredCustomerAccountNumber = "100741741", // To be commented for US1646.
                    }
                };

                // cardlookupdetails collection will contain combination of Past Biller and Receivers Collection. User Story # US1645 and # US1646.
                List<WUCommonData.Receiver> receivers = WesternUnionIO.WUPastBillersReceivers(customerSessionId, cxncardlookupreq, mgiContext);

                if (receivers != null)
                {
                    // In receivers filter only if the Name Type = "C" and Country only "US".
                    receivers = receivers.FindAll(r => r.NameType == WUCommonData.WUEnums.name_type.C && r.BusinessName != string.Empty && r.Attention != string.Empty && r.Address.item.country_code == "US");
                    billers = Mapper.Map<List<Biller>>(receivers);
                    WesternUnionAccount account = null;
                    if (mgiContext.CxnAccountId != 0)
                    {
                        account = ProcessorDAL.GetAccountById(mgiContext.CxnAccountId);
                    }

                    long agentSessionId = 0;
                    if (mgiContext.AgentSessionId != 0)
                    {
                        agentSessionId = mgiContext.AgentSessionId;
                    }
                    ImportedBiller importBiller;
                    foreach (var biller in billers)
                    {
                        importBiller = new ImportedBiller();
                        importBiller.AccountNumber = biller.AccountNumber;
                        importBiller.BillerName = biller.Name;
                        importBiller.CardNumber = cardNumber;
                        importBiller.CustomerSessionId = customerSessionId;
                        importBiller.DTServerCreate = DateTime.Now;
                        importBiller.WUIndex = biller.IndexNumber;
                        importBiller.WUAccount = account;
                        importBiller.AgentSessionId = agentSessionId;
                        importBiller.DTServerLastModified = null;

                        ImportedBiller pastBiller = ProcessorDAL.GetExistingBiller(importBiller);

                        if (pastBiller != null)
                        {
                            pastBiller.AccountNumber = biller.AccountNumber;
                            pastBiller.DTServerLastModified = DateTime.Now;
                            ProcessorDAL.UpdateImportedBiller(pastBiller);
                        }
                        else
                            ProcessorDAL.AddImportedBiller(importBiller);
                    }
                }

                return billers;
            }
            catch (WUCommonException ex)
            {
                errorMessage = string.Format(errorMessage, ex.Message);

                //AL-1014 Transactional Log User Story
                List<string> details = new List<string>();
                details.Add("CustomerSession Id:" + Convert.ToString(customerSessionId));
                details.Add("Card Number:" + cardNumber);
                MongoDBLogger.ListError<string>(details, "GetPastBillers", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in GetPastBillers -MGI.Cxn.BillPay.WU.Impl.WesternUnionGateway", errorMessage, ex.StackTrace);

                throw new BillPayException(errorCode, errorMessage, ex);
            }
            catch (Exception ex)
            {
                errorMessage = string.Format(errorMessage, ex.Message);
                //AL-1014 Transactional Log User Story
                List<string> details = new List<string>();
                details.Add("CustomerSession Id:" + Convert.ToString(customerSessionId));
                details.Add("Card Number:" + cardNumber);
                MongoDBLogger.ListError<string>(details, "GetPastBillers", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in GetPastBillers -MGI.Cxn.BillPay.WU.Impl.WesternUnionGateway", errorMessage, ex.StackTrace);

                throw new BillPayException(errorCode, errorMessage, ex);
            }
        }

        public void UpdateAccount(BillPayAccount account, MGIContext mgiContext)
        {
            WesternUnionAccount wuAccount = ProcessorDAL.GetAccountById(account.Id);
            AutoMapper.Mapper.Map<BillPayAccount, WesternUnionAccount>(account, wuAccount);
            account.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
            account.DTServerLastModified = DateTime.Now;
            ProcessorDAL.UpdateAccount(wuAccount);
        }
        #endregion

        #region Private Methods
        private void PopulateWUObjects(long channelPartnerId, MGIContext mgiContext)
        {
            _response = WesternUnionIO.CreateRequest(channelPartnerId, mgiContext);
        }

        private BillPaymentRequest BuildBillPayRequest(BillPayRequest request)
        {
            return new BillPaymentRequest()
            {
                //Todo:Commented By Sakhatech 
                //CxnId = request.CxnId,//from biz we are not at all passing this parameter so why still its ther?
                BillerName = request.ProductName,
                AccountNumber = request.AccountNumber,
                Amount = request.Amount,
                Fee = request.Fee,
                PromoCode = request.PromoCode,
                CouponCode = request.CouponCode,
                PrimaryIdType = request.PrimaryIdType,
                PrimaryIdNumber = request.PrimaryIdNumber,
                PrimaryIdPlaceOfIssue = request.PrimaryIdPlaceOfIssueCode,
                PrimaryIdCountryOfIssue = request.PrimaryIdCountryOfIssueCode,
                SecondIdType = request.SecondIdType,
                SecondIdNumber = request.SecondIdNumber,
                SecondIdCountryOfIssue = request.SecondIdCountryOfIssue,
                Occupation = request.Occupation,
                CountryOfBirth = request.CountryOfBirth,

                //WU specific
                Location = Convert.ToString(GetDictionaryValue(request.MetaData, "Location")),
                SessionCookie = Convert.ToString(GetDictionaryValue(request.MetaData, "SessionCookie")),
                AailableBalance = Convert.ToString(GetDictionaryValue(request.MetaData, "AailableBalance")),
                AccountHolder = Convert.ToString(GetDictionaryValue(request.MetaData, "AccountHolder")),
                Attention = Convert.ToString(GetDictionaryValue(request.MetaData, "Attention")),
                Reference = Convert.ToString(GetDictionaryValue(request.MetaData, "Reference")),
                DateOfBirth = Convert.ToString(GetDictionaryValue(request.MetaData, "DateOfBirth")),
                DeliveryCode = Convert.ToString(GetDictionaryValue(request.MetaData, "DeliveryCode")),

                //Todo:Commented By Sakhatech 
                //Location = request.Location,
                //SessionCookie = request.SessionCookie,
                //AailableBalance = request.AailableBalance,
                //AccountHolder = request.AccountHolder,
                //Attention = request.Attention,
                //Reference = request.Reference,
                //DateOfBirth = request.DateOfBirth,
                //DeliveryCode = request.DeliveryCode,
            };
        }

        public static object GetDictionaryValue(Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key) == false)
                throw new Exception(String.Format("{0} not provided in dictionary", key));
            return dictionary[key];
        }
        /// <summary>
        /// AL-90
        /// </summary>
        /// <param name="mgiContext"></param>
        private void CheckCounterId(MGIContext mgiContext)
        {
            if (!IsHardCodedCounterId)
            {
                if (string.IsNullOrEmpty(mgiContext.WUCounterId))
                    throw new BillPayException(BillPayException.COUNTERID_NOT_FOUND, "Western Union counter Id is not available or has not been correctly setup");
            }
        }

        private string Encrypt(string plainString)
        {
            if (!string.IsNullOrWhiteSpace(plainString) && plainString.IsCreditCardNumber())
            {
                return BPDataProtectionSvc.Encrypt(plainString, 0);
            }
            return plainString;
        }

        private string Decrypt(string encryptedString)
        {
            string decryptString = encryptedString;
            if (!string.IsNullOrWhiteSpace(encryptedString))
            {
                try
                {
                    decryptString = BPDataProtectionSvc.Decrypt(encryptedString, 0);
                }
                catch (Exception ex)
                {
                    decryptString = encryptedString;
                    string stakeTrace = !string.IsNullOrWhiteSpace(ex.StackTrace) ? " Stack Trace: " + ex.StackTrace : "No stack trace available";
                    NLogger.Error(string.Format("Error in Decrypting card number: {0} \n {1}", ex.Message, stakeTrace));
                }
            }
            return decryptString;
        }
        //		 Begin AL-471 Changes
        //       User Story Number: AL-471 | Web |   Developed by: Sunil Shetty     Date: 25.06.2015
        //       Purpose: This method takes only ssn exception message. We have found with ssn we have only 3 exception and below arethe one
        private int GetExceptionMessage(string MinorCode)
        {
            switch (MinorCode)
            {
                case "5050":
                    return BillPayException.MISSING_SSN_ITIN;
                case "7490":
                    return BillPayException.REQUIRES_SSN_ITIN;
                case "6008":
                    return BillPayException.INVALID_SOCIAL_SECURITY_NUMBER;
                case "0425":
                    return BillPayException.TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION; //AL-2967
                case "0415":
                    return BillPayException.DO_TRANSACTION_REQUIRES_ADDITIONAL_CUSTOMER_INFORMATION;
                default:
                    return 0;
            }
        }
        //		 End AL-471 Changes

        private void PopulateDeliveryMethods()
        {
            _deliveryCodeMapping.Add("000", "Urgent");
            _deliveryCodeMapping.Add("100", "2nd Business Day");
            _deliveryCodeMapping.Add("200", "3rd Business Day");
            _deliveryCodeMapping.Add("300", "Next Business Day");
        }
        #endregion
    }
}
