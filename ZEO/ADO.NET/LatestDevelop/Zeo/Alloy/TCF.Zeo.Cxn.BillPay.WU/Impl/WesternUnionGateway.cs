#region System References

using System.Configuration;
using System;
using System.Collections.Generic;

#endregion

#region Zeo References

using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.BillPay.Contract;
using AutoMapper;
using TCF.Zeo.Cxn.BillPay.Data;
using TCF.Zeo.Cxn.BillPay.WU.Data;
using TCF.Zeo.Cxn.BillPay.WU.Contract;
using WUCommonData = TCF.Zeo.Cxn.WU.Common.Data;
using WUCommon = TCF.Zeo.Cxn.WU.Common;
using TCF.Zeo.Cxn.WU.Common.Contract;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.BillPay.Data.Exceptions;
using TCF.Zeo.Cxn.BillPay.WU.Data.Exceptions;

#endregion

namespace TCF.Zeo.Cxn.BillPay.WU.Impl
{
    public class WesternUnionGateway : IBillPayProcessor
    {
        #region Private Members
        public IExceptionHelper ExceptionHelper = new BillpayProviderException();

        #endregion

        #region Dependencies
        public ProcessorDAL ProcessorDAL = new Impl.ProcessorDAL();
        public bool IsHardCodedCounterId { get; set; } = Convert.ToBoolean(ConfigurationManager.AppSettings["IsHardCodedCounterId"].ToString());
        public IIO WUBillPayIO = GetBillPayProcessor();
        public IWUCommonIO WUCommonIO = GetCommonProcessor();
        WUCommonData.WUBaseRequestResponse response = null;

        IMapper mapper;

        #endregion

        public WesternUnionGateway()
        {
            #region Mapping

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WUTransaction, BillPayTransaction>()
               .ForMember(d => d.AccountNumber, s => s.MapFrom(c => BaseIO.Decrypt(c.CustomerAccountNumber)))
               .ForMember(d => d.Amount, s => s.MapFrom(c => c.FinancialsOriginatorsPrincipalAmount))
               .ForMember(d => d.Fee, s => s.MapFrom(c => c.FinancialsFee))
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
                                                       {"DeliveryService", ProcessorDAL.GetDeliveryServiceByCode(s.DeliveryCode)},
                                                       {"MessageArea",s.MessageArea}
                                                  };
               });

                cfg.CreateMap<WUTransaction, BillPayValidateResponse>()
               .ForMember(d => d.TransactionId, s => s.MapFrom(c => c.Id))
               .ForMember(d => d.Amount, s => s.MapFrom(c => c.FinancialsOriginatorsPrincipalAmount))
               .ForMember(d => d.Fee, s => s.MapFrom(c => c.FinancialsFee))
               .ForMember(d => d.ConfirmationNumber, s => s.MapFrom(c => c.Mtcn))
               .ForMember(d => d.SenderWUGoldcardNumber, s => s.MapFrom(c => c.WesternUnionCardNumber))
               .ForMember(d => d.DiscountedFee, s => s.MapFrom(c => c.FinancialsTotalDiscount))
               .ForMember(d => d.UnDiscountedFee, s => s.MapFrom(c => c.FinancialsUndiscountedCharges));

                cfg.CreateMap<WUCommonData.Receiver, Biller>()
               .ForMember(d => d.Name, s => s.MapFrom(c => c.BusinessName))
               .ForMember(d => d.AccountNumber, s => s.MapFrom(c => c.Attention))
               .ForMember(d => d.IndexNumber, s => s.MapFrom(c => c.ReceiverIndexNumber));

                cfg.CreateMap<WUCommonData.CardInfo, CardInfo>()
               .ForMember(d => d.PromoCode, s => s.MapFrom(c => c.PromoCode))
               .ForMember(d => d.TotalPointsEarned, s => s.MapFrom(c => c.TotalPointsEarned));

            });
            mapper = config.CreateMapper();

            #endregion
        }


        #region IBillPayProcessor Methods


        #region Bill Pay transaction

        public Fee GetFee(long wuTrxId, string billerName, string accountNumber, decimal amount, Location location, ZeoContext context)
        {
            try
            {
                CheckCounterId(context.WUCounterId);

                PopulateWUObjects(ref context);

                BillPaymentRequest billPaymentRequest = BuildBillPayRequest(wuTrxId, billerName, accountNumber, amount, context);

                Fee fee = WUBillPayIO.GetDeliveryMethods(wuTrxId, billerName, accountNumber, amount, location, billPaymentRequest, context);

                return fee;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLPAY_GETFEE_FAILED, ex);
            }
        }

        public List<Location> GetLocations(string billerName, string accountNumber, decimal amount, ZeoContext context)
        {
            try
            {
                CheckCounterId(context.WUCounterId);

                PopulateWUObjects(ref context);

                //Sending transaction id as 0, as GetLocations is used else where.
                long wuTrxId = 0;

                BillPaymentRequest billPaymentRequest = BuildBillPayRequest(wuTrxId, billerName, accountNumber, amount, context);

                List<Location> locations = WUBillPayIO.GetLocations(billerName, accountNumber, amount, billPaymentRequest, context);

                return locations;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.LOCATION_RETRIEVAL_FAILED, ex);
            }
        }

        public BillPayValidateResponse Validate(long wuTrxId, BillPayment billPayment, ZeoContext context)
        {
            try
            {
                CheckCounterId(context.WUCounterId);

                BillPaymentRequest billPaymentRequest = BuildBillPayValidateRequest(wuTrxId, billPayment, context);

                PopulateWUObjects(ref context);

                WUTransaction wUTransaction = WUBillPayIO.ValidatePayment(wuTrxId, billPaymentRequest, context);

                return mapper.Map<BillPayValidateResponse>(wUTransaction);

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLPAY_VALIDATE_FAILED, ex);
            }
        }

        public BillPayTransaction Commit(long wuTrxId, ZeoContext context)
        {
            try
            {
                CheckCounterId(context.WUCounterId);

                PopulateWUObjects(ref context);

                ProcessorDAL = new ProcessorDAL();
                BillPayTransactionRequest trxRequest = ProcessorDAL.GetBillPayTransactionRequest(wuTrxId);

                WUTransaction wUTransaction = WUBillPayIO.MakePayment(trxRequest, context);

                return mapper.Map<BillPayTransaction>(wUTransaction);

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLPAY_COMMIT_FAILED, ex);
            }
        }

        public long GetBillPayAccountId(long customerSessionId, string timeZone, ZeoContext context)
        {
            ProcessorDAL = new ProcessorDAL();
            return ProcessorDAL.GetWUBillPayAccount(customerSessionId, timeZone);
        }

        public BillPayTransaction GetTransaction(long wuTrxId, ZeoContext context)
        {
            try
            {
                ProcessorDAL = new ProcessorDAL();
                BillPayTransaction billPayTrx = ProcessorDAL.GetTransactionById(wuTrxId);
                return billPayTrx;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.TRANSACTION_GET_FAILED, ex);
            }
        }

        #endregion


        #region Billers

        public List<Biller> GetPastBillers(string cardNumber, ZeoContext context)
        {
            CheckCounterId(context.WUCounterId);
            List<Biller> billers = null;

            try
            {
                WUCommonData.CardLookUpRequest cxncardlookupreq = new WUCommonData.CardLookUpRequest()
                {
                    sender = new WUCommonData.Sender()
                    {
                        PreferredCustomerAccountNumber = cardNumber
                    }
                };

                //WUCommonIO = GetCommonProcessor();
                List<WUCommonData.Receiver> receivers = WUCommonIO.WUPastBillersReceivers(cxncardlookupreq, context);


                if (receivers != null)
                {
                    receivers = receivers.FindAll(r => r.NameType == WUCommonData.WUEnums.name_type.C && r.BusinessName != string.Empty && r.Attention != string.Empty && r.Address.item.country_code == "US");
                    billers = mapper.Map<List<Biller>>(receivers);

                    List<ImportedBiller> importBillers = new List<ImportedBiller>();
                    ImportedBiller importBiller;

                    foreach (var biller in billers)
                    {
                        importBiller = new ImportedBiller()
                        {
                            AccountNumber = BaseIO.Encrypt(biller.AccountNumber),
                            BillerName = biller.Name,
                            CardNumber = cardNumber,
                            CustomerSessionId = context.CustomerSessionId,
                            WUIndex = biller.IndexNumber,
                            AgentSessionId = context.AgentSessionId,
                            DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone),
                            DTServerCreate = DateTime.Now
                        };

                        importBillers.Add(importBiller);
                    }

                    ProcessorDAL = new ProcessorDAL();
                    ProcessorDAL.CreateOrUpdateImportBillers(importBillers);
                }

                return billers;
            }
            catch (WUCommonData.WUCommonProviderException ex)
            {
                throw new BillpayProviderException(ex.ProviderErrorCode, ex.Message, ex);
            }
            catch (WUCommonData.WUCommonException ex)
            {
                throw new BillPayException(ex.ZeoErrorCode, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.PROVIDER_IMPORT_FAILED, ex);
            }
        }

        public List<Field> GetProviderAttributes(string billerName, string locationName, ZeoContext context)
        {
            try
            {
                CheckCounterId(context.WUCounterId);

                PopulateWUObjects(ref context);

                List<Field> fields = WUBillPayIO.GetBillerFields(billerName, locationName, context);

                return fields;

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLERFIELDS_GET_FAILED, ex);
            }
        }

        public BillerInfo GetBillerInfo(string billerName, ZeoContext context)
        {
            try
            {
                CheckCounterId(context.WUCounterId);

                PopulateWUObjects(ref context);

                return new BillerInfo() { Message = WUBillPayIO.GetBillerMessage(billerName, context) };
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.BILLERMESSAGE_GET_FAILED, ex);
            }
        }

        #endregion


        public CardInfo GetCardInfo(string cardNumber, ZeoContext context)
        {
            try
            {
                CheckCounterId(context.WUCounterId);
                WUCommonData.CardLookUpRequest cardRequest = new WUCommonData.CardLookUpRequest()
                {
                    sender = new WUCommonData.Sender()
                    {
                        PreferredCustomerAccountNumber = cardNumber
                    }
                };

                return mapper.Map<CardInfo>(WUCommonIO.GetCardInfo(cardRequest, context));
            }
            catch (WUCommonData.WUCommonProviderException ex)
            {
                throw new BillpayProviderException(ex.ProviderErrorCode, ex.Message, ex);
            }
            catch (WUCommonData.WUCommonException ex)
            {
                throw new BillPayException(ex.ZeoErrorCode, ex);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BillPayException(BillPayException.CARDINFO_GET_FAILED, ex);
            }
        }

        #endregion

        #region Private Methods

        private void PopulateWUObjects(ref ZeoContext context)
        {
            context.ProductType = "billpay";

            if (!context.Context.ContainsKey("BaseWUObject") || context.Context["BaseWUObject"] == null)
            {
                //WUCommonIO = GetCommonProcessor();
                response = WUCommonIO.CreateRequest(context.ChannelPartnerId, context);
                context.Context.Add("BaseWUObject", response);
            }

            if (WUBillPayIO == null)
                WUBillPayIO = GetBillPayProcessor();
        }

        private void CheckCounterId(string wUCounterId)
        {
            if (!IsHardCodedCounterId)
            {
                if (string.IsNullOrEmpty(wUCounterId))
                    throw new BillPayException(BillPayException.COUNTERID_NOT_FOUND);
            }
        }

        private BillPaymentRequest BuildBillPayRequest(long wuTrxId, string billerName, string accountNumber, decimal amount, ZeoContext context)
        {
            ProcessorDAL = new ProcessorDAL();

            BillPaymentRequest billPayRequest = ProcessorDAL.GetWUBillPayRequest(context.CustomerId, wuTrxId);

            billPayRequest.Amount = amount;
            billPayRequest.BillerName = billerName;
            billPayRequest.AccountNumber = accountNumber;
            billPayRequest.SecondIdType = "SSN";
            billPayRequest.SecondIdCountryOfIssue = "US";
            billPayRequest.Location = Convert.ToString(GetDictionaryValue(context.Context, "Location"));
            billPayRequest.SessionCookie = Convert.ToString(GetDictionaryValue(context.Context, "SessionCookie"));
            billPayRequest.AailableBalance = Convert.ToString(GetDictionaryValue(context.Context, "AailableBalance"));
            billPayRequest.AccountHolder = Convert.ToString(GetDictionaryValue(context.Context, "AccountHolder"));
            billPayRequest.Attention = Convert.ToString(GetDictionaryValue(context.Context, "Attention"));
            billPayRequest.Reference = Convert.ToString(GetDictionaryValue(context.Context, "Reference"));
            billPayRequest.DeliveryCode = Convert.ToString(GetDictionaryValue(context.Context, "DeliveryCode"));
            return billPayRequest;

        }

        private BillPaymentRequest BuildBillPayValidateRequest(long wuTrxId, BillPayment billPayment, ZeoContext context)
        {
            context.Context = billPayment.MetaData;

            BillPaymentRequest billPayRequest = BuildBillPayRequest(wuTrxId, billPayment.BillerName, billPayment.AccountNumber, billPayment.PaymentAmount, context);

            billPayRequest.Fee = billPayment.Fee;
            billPayRequest.CouponCode = billPayment.CouponCode;

            if (string.IsNullOrWhiteSpace(billPayRequest.Location))
            {
                if (billPayment.CityCode != null)
                    billPayRequest.Location = billPayment.CityCode;
            }

            return billPayRequest;
        }

        public static object GetDictionaryValue(Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key) == false)
                return string.Empty;
            return dictionary[key];
        }

        private static IWUCommonIO GetCommonProcessor()
        {
            string commonProcessor = ConfigurationManager.AppSettings["BillPayProcessor"].ToString();

            if (commonProcessor.ToUpper() == "IO")
                return new WUCommon.Impl.IO();
            else
                return new WUCommon.Impl.SimulatorIO();
        }

        private static IIO GetBillPayProcessor()
        {
            string commonProcessor = ConfigurationManager.AppSettings["BillPayProcessor"].ToString();

            if (commonProcessor.ToUpper() == "IO")
                return new IO();
            else
                return new SimulatorIO();

        }

        public void UpdateBillPayGoldCardPoints(long transactionId, string cardNumber, ZeoContext context)
        {
            ProcessorDAL.UpdateBillPayGoldCardPoints(transactionId, GetCardInfo(cardNumber, context).TotalPointsEarned, (int)Helper.ProductCode.BillPay);
        }

        #endregion
    }
}
