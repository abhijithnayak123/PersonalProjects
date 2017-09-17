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
using MGI.Common.Sys;
using MGI.Cxn.WU.Common.Data;

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

            Mapper.CreateMap<WUCommonData.CardInfo, MGI.Cxn.BillPay.Data.CardInfo>();
            Mapper.CreateMap<MGI.Cxn.BillPay.Data.CardInfo, WUCommonData.CardInfo>();

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
            catch (Exception ex)
            {
                MongoDBLogger.Error<BillPayRequest>(request, "Validate", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in Validate - MGI.Cxn.BillPay.WU.Impl.WesternUnionGateway", ex.Message, ex.StackTrace);
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLPAY_VALIDATE_FAILED, ex);
            }
        }

        public long Commit(long transactionID, MGIContext mgiContext)
        {
            try
            {
                CheckCounterId(mgiContext);
                WesternUnionTrx trx = ProcessorDAL.GetTrxById(transactionID);

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
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLPAY_COMMIT_FAILED, ex);
            }
        }

        /// <summary>
        /// AL-491 While printing receipts updating Gold card points
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="totalPointsEarned"></param>
        /// <param name="mgiContext"></param>
        public void UpdateGoldCardPoints(long transactionId, string totalPointsEarned, MGIContext mgiContext)
        {
            try
            {
                WesternUnionTrx transaction = ProcessorDAL.GetTrxById(transactionId);
                transaction.DTServerLastModified = DateTime.Now;
                transaction.WuCardTotalPointsEarned = totalPointsEarned;
                transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
                ProcessorDAL.WesternUnionTrxRepo.UpdateWithFlush(transaction);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.GOLD_CARD_POINTS_UPDATE_FAILED, ex);
            }
        }

        public long AddBillPayAccount(BillPay.Data.BillPayRequest request, string timeZone)
        {
            try
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
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.ACCOUNT_CREATE_FAILED, ex);
            }
        }

        public BillPayAccount GetBillPayAccount(long cxnAccountID)
        {
            try
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
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.ACCOUNT_GET_FAILED, ex);
            }
        }

        public BillPayTransaction GetTransaction(long transactionID)
        {
            try
            {
                WesternUnionTrx trx = ProcessorDAL.GetTrxById(transactionID);
                BillPayTransaction billPayTrx = Mapper.Map<BillPayTransaction>(trx);
                return billPayTrx;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.TRANSACTION_GET_FAILED, ex);
            }
        }

        public List<Location> GetLocations(string billerName, string accountNumber, decimal amount, MGIContext mgiContext)
        {
            try
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
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.LOCATION_RETRIEVAL_FAILED, ex);
            }
        }

        public Fee GetFee(string billerName, string accountNumber, decimal amount, Location location, MGIContext mgiContext)
        {
            try
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
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLPAY_GETFEE_FAILED, ex);
            }
        }

        //@@@ what is the perpose of this method, getiing only biller message or biller info.
        public BillerInfo GetBillerInfo(string billerName, MGIContext mgiContext)
        {
            try
            {
                CheckCounterId(mgiContext);
                PopulateWUObjects(mgiContext.ChannelPartnerId, mgiContext);

                if (!mgiContext.Context.ContainsKey("BaseWUObject") || mgiContext.Context["BaseWUObject"] == null)
                {
                    mgiContext.Context.Add("BaseWUObject", _response);
                }

                return new BillerInfo() { Message = WUBillPayIO.GetBillerMessage(billerName, mgiContext) };
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLERMESSAGE_GET_FAILED, ex);
            }
        }

        public List<Field> GetProviderAttributes(string billerName, string locationName, MGIContext mgiContext)
        {
            try
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
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLERFIELDS_GET_FAILED, ex);
            }
        }

        public long UpdateCardDetails(long cxnAccountId, string cardNumber, MGIContext mgiContext, string timezone)
        {
            try
            {
                WesternUnionAccount billPayAccount = ProcessorDAL.GetAccountById(cxnAccountId);
                billPayAccount.CardNumber = cardNumber;
                billPayAccount.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
                billPayAccount.DTServerLastModified = DateTime.Now;
                ProcessorDAL.WesternUnionAccountRepo.Update(billPayAccount);

                return billPayAccount.Id;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.ACCOUNT_UPDATE_FAILED, ex);
            }
        }

        public MGI.Cxn.BillPay.Data.CardInfo GetCardInfo(string cardNumber, MGIContext mgiContext)
        {
            try
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

                MGI.Cxn.BillPay.Data.CardInfo cardInfo = Mapper.Map<MGI.Cxn.BillPay.Data.CardInfo>(wuCardInfo);

                return cardInfo;
            }
            catch (WUCommonProviderException ex)
            {
                throw new BillpayProviderException(ex.ProviderErrorCode, ex.Message, ex);
            }
            catch (WUCommonException ex)
            {
                throw new BillPayException(ex.AlloyErrorCode, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.CARDINFO_GET_FAILED, ex);
            }
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
            string errorMessage = "Error while importing billers from Western Union: {0}";
            List<string> details = new List<string>();
            details.Add("CustomerSession Id:" + Convert.ToString(customerSessionId));
            details.Add("Card Number:" + cardNumber);
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
            catch (WUCommonProviderException ex)
            {
                errorMessage = string.Format(errorMessage, ex.Message);
                MongoDBLogger.ListError<string>(details, "GetPastBillers", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in GetPastBillers -MGI.Cxn.BillPay.WU.Impl.WesternUnionGateway", errorMessage, ex.StackTrace);
                throw new BillpayProviderException(ex.ProviderErrorCode, ex.Message, ex);
            }
            catch (WUCommonException ex)
            {
                errorMessage = string.Format(errorMessage, ex.Message);
                MongoDBLogger.ListError<string>(details, "GetPastBillers", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in GetPastBillers -MGI.Cxn.BillPay.WU.Impl.WesternUnionGateway", errorMessage, ex.StackTrace);
                throw new BillPayException(ex.AlloyErrorCode, ex);
            }
            catch (Exception ex)
            {
                errorMessage = string.Format(errorMessage, ex.Message);
                MongoDBLogger.ListError<string>(details, "GetPastBillers", AlloyLayerName.CXN, ModuleName.BillPayment,
                    "Error in GetPastBillers -MGI.Cxn.BillPay.WU.Impl.WesternUnionGateway", errorMessage, ex.StackTrace);
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.PROVIDER_IMPORT_FAILED, ex);
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
            mgiContext.ProductType = "billpay";
            _response = WesternUnionIO.CreateRequest(channelPartnerId, mgiContext);

        }

        private BillPaymentRequest BuildBillPayRequest(BillPayRequest request)
        {
            return new BillPaymentRequest()
            {
                BillerName = request.ProductName,
                AccountNumber = request.AccountNumber,
                Amount = request.Amount,
                Fee = request.Fee,
                PromoCode = request.PromoCode,
                CouponCode = request.CouponCode,
                PrimaryIdType = request.PrimaryIdType,
                PrimaryIdNumber = request.PrimaryIdNumber,
                PrimaryIdPlaceOfIssue = request.PrimaryIdPlaceOfIssueCode,
                PrimaryCountryOfIssue = request.PrimaryCountryOfIssue,
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
                    throw new BillPayException(BillPayException.COUNTERID_NOT_FOUND);
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
