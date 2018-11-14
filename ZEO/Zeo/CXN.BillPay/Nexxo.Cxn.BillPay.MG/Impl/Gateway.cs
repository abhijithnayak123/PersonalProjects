using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using MGI.Cxn.BillPay.Contract;
using MGI.Cxn.BillPay.Data;
using MGI.Cxn.BillPay.MG.AgentConnectService;
using MGI.Cxn.BillPay.MG.Data;
using MGI.CXN.MG.Common.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BillerInfo = MGI.Cxn.BillPay.Data.BillerInfo;
using MGCommonService = MGI.CXN.MG.Common.AgentConnectService;
using MGCommonData = MGI.CXN.MG.Common.Data;
using MGI.Cxn.BillPay.MG.Contract;
using MGI.CXN.MG.Common.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Cxn.BillPay.MG.Impl
{
    public class Gateway : IBillPayProcessor
    {
        public IMGCommonIO MoneyGramCommonIO { private get; set; }
        public IRepository<Transaction> MgTransactionRepo { private get; set; }
        public IRepository<Account> MgAccountRepo { get; set; }
		public IIO MoneyGramIO { private get; set; }
        public ProcessorDAL ProcessorDAL { private get; set; }
        public IRepository<Country> CountryRepo { private get; set; }
        public IRepository<State> StateRepo { private get; set; }
        public TLoggerCommon MongoDBLogger { get; set; }
        private string _errorMessage;

        public Gateway()
        {
            Mapper.CreateMap<MGCommonService.ProductFieldInfo, Field>()
                .ForMember(x => x.Label, s => s.MapFrom(c => c.fieldLabel))
                .ForMember(x => x.DataType, s => s.MapFrom(c => c.dataType))
                .ForMember(x => x.MaxLength, s => s.MapFrom(c => c.fieldMax))
                .ForMember(x => x.ValidationMessage, s => s.MapFrom(c => c.validationRegEx))
                .ForMember(x => x.Mask, s => s.MapFrom(c => c.validationRegEx));

            Mapper.CreateMap<MGCommonService.DynamicFieldInfo, Field>()
                .ForMember(x => x.Label, s => s.MapFrom(c => c.fieldLabel))
                .ForMember(x => x.DataType, s => s.MapFrom(c => c.dataType))
                .ForMember(x => x.MaxLength, s => s.MapFrom(c => c.fieldMax))
                .ForMember(x => x.ValidationMessage, s => s.MapFrom(c => c.validationRegEx))
                .ForMember(x => x.Mask, s => s.MapFrom(c => c.validationRegEx));

            Mapper.CreateMap<MGCommonService.FeeLookupRequest, Transaction>()
                .ForMember(x => x.SendAmount, s => s.MapFrom(c => c.Item))
                .ForMember(x => x.PromoCodeValuesPromoCode, s => s.MapFrom(c => c.promoCodeValues[0]))
                .ForMember(x => x.ProductVariant, s => s.MapFrom(c => (int)c.productVariant));

            Mapper.CreateMap<MGCommonService.FeeLookupResponse, Transaction>();

            Mapper.CreateMap<MGCommonService.FeeInfo, Transaction>()
               .ForMember(x => x.TotalAmountToCollect, s => s.MapFrom(c => c.sendAmounts.totalAmountToCollect))
               .ForMember(x => x.TotalDiscountAmount, s => s.MapFrom(c => c.sendAmounts.totalDiscountAmount))
               .ForMember(x => x.TotalSendFees, s => s.MapFrom(c => c.sendAmounts.totalSendFees))
               .ForMember(x => x.TotalSendTaxes, s => s.MapFrom(c => c.sendAmounts.totalSendTaxes))
               .ForMember(x => x.ReceiveAmount, s => s.MapFrom(c => c.receiveAmounts.receiveAmount))
               .ForMember(x => x.ReceiveCurrency, s => s.MapFrom(c => c.receiveAmounts.receiveCurrency))
               .ForMember(x => x.ReceiveFeesAreEstimated, s => s.MapFrom(c => c.receiveAmounts.receiveFeesAreEstimated))
               .ForMember(x => x.ReceiveTaxesAreEstimated,
                          s => s.MapFrom(c => c.receiveAmounts.receiveTaxesAreEstimated))
               .ForMember(x => x.TotalReceiveAmount, s => s.MapFrom(c => c.receiveAmounts.totalReceiveAmount))
               .ForMember(x => x.TotalReceiveFees, s => s.MapFrom(c => c.receiveAmounts.totalReceiveFees))
               .ForMember(x => x.TotalReceiveTaxes, s => s.MapFrom(c => c.receiveAmounts.totalReceiveTaxes));

            Mapper.CreateMap<Transaction, MGI.Cxn.BillPay.Data.BillPayTransaction>()
            .ForMember(x => x.Account, s => s.MapFrom(c => c.Account))
            .ForMember(x => x.AccountNumber, s => s.MapFrom(c => c.AccountNumber))
            .ForMember(x => x.Amount, s => s.MapFrom(c => c.SendAmount))
            .ForMember(x => x.BillerName, s => s.MapFrom(c => c.BillerName))
            .ForMember(x => x.ConfirmationNumber, s => s.MapFrom(c => c.ReferenceNumber))
            .ForMember(x => x.Fee, s => s.MapFrom(c => c.TotalSendFees))
            .AfterMap((s, d) =>
                          {
                              d.MetaData = new Dictionary<string, object>()
                                                   {
                                                       {"ProductVariant", GetProductVariantName((productVariant) s.ProductVariant) },
                                                       {"SenderFirstName", s.SenderFirstName },
                                                       {"SenderMiddleName", s.SenderMiddleName},
                                                       {"SenderLastName", s.SenderLastName},
                                                       {"SenderLastName2", s.SenderLastName2},
                                                       {"SenderAddress", s.SenderAddress},
                                                       {"SenderCity", s.SenderCity},
                                                       {"SenderState",s.SenderState},
                                                       {"SenderZipCode",s.SenderZipCode},
                                                       {"SenderHomePhone",s.SenderHomePhone},
                                                       {"Attention",s.MessageField1},
                                                       {"Message",s.MessageField2},
                                                       {"TextTranslation",s.TextTranslation},
                                                       {"SendCurrency",s.SendCurrency},
                                                       {"TotalDiscountAmount",s.TotalDiscountAmount},
                                                       {"TotalSendTaxes",s.TotalSendTaxes},
                                                       {"TotalAmountToCollect",s.TotalAmountToCollect},
                                                       {"BillerCity",s.BillerCity},
                                                       {"BillerState",s.BillerState},
                                                       {"ReferenceNumber",s.ReferenceNumber},
                                                       {"ModifiedDate",String.Format("{0:MMM dd, yyyy HH:mm tt}", s.DTTerminalLastModified) },
                                                       {"BillerCode",s.ReceiveCode},
                                                       {"SenderEmail",s.Account.Email},
                                                       {"BillerWebsite", s.BillerWebsite},
                                                       {"BillerPhone", s.BillerPhone},
                                                       {"BillerCutoffTime", s.BillerCutoffTime},
                                                       {"MTCN", s.ReferenceNumber},
                                                       {"CustomerTipTextTranslation", s.CustomerTipTextTranslation},
                                                       {"ExpectedPostingTimeFrame", s.ExpectedPostingTimeFrame},
                                                       {"ExpectedPostingTimeFrameSecondary", s.ExpectedPostingTimeFrameSecondary},
                                                   };
                          });

            Mapper.CreateMap<MGI.Cxn.BillPay.MG.Data.Account, MGI.Cxn.BillPay.Data.BillPayAccount>();

            Mapper.CreateMap<BpValidationRequest, Transaction>()
                .ForMember(x => x.ProductVariant, s => s.MapFrom(c => (int)c.productVariant));
        }

        #region Public Methods
		public long Validate(long cxnAccountID, BillPayRequest request, MGIContext mgiContext)
        {
            try
            {
                _errorMessage = "Error while validate";

                Transaction transaction = ProcessorDAL.GetTransactionxById(mgiContext.TrxId);

				BpValidationRequest bpValidationRequest = BuildValidationRequest(request, transaction, mgiContext);

				BpValidationResponse response = MoneyGramIO.BpValidation(bpValidationRequest, mgiContext);

                if (!response.readyForCommit)
                {
                    throw new Exception("Additional fields to be collected for Billpay Validate");
                }

				UpdateValidationResponse(response, mgiContext);

                return mgiContext.TrxId;
            }
            catch (MGramProviderException ex)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<BillPayRequest>(request, "Validate", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in Validate -MGI.Cxn.BillPay.MG.Impl.Gateway", ex.Message, ex.StackTrace);
				throw new BillPayException(BillPayException.PROVIDER_ERROR, ex);
            }
            catch (TimeoutException timeoutEx)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<BillPayRequest>(request, "Validate", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in Validate -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, timeoutEx.StackTrace);
				throw new Exception(_errorMessage, timeoutEx);
            }
            catch (WebException webEx)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<BillPayRequest>(request, "Validate", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in Validate -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, webEx.StackTrace);
				throw new Exception(_errorMessage, webEx);
            }
            catch (Exception ex)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<BillPayRequest>(request, "Validate", AlloyLayerName.CXN, ModuleName.BillPayment,
						"Error in Validate -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, ex.StackTrace);
				throw new Exception(_errorMessage, ex);
            }
        }

		public long Commit(long transactionID, MGIContext mgiContext)
        {
			if (mgiContext.RequestType == "HOLD")
                return transactionID;

            try
            {
				MGCommonService.CommitTransactionRequest commitRequest = BuildCommitRequest(transactionID, mgiContext);

                _errorMessage = "Error while commit";

				MGCommonService.CommitTransactionResponse commitResponse = MoneyGramCommonIO.CommitTransaction(commitRequest, mgiContext);

				UpdateCommitTransactionResponse(transactionID, commitResponse, mgiContext);

                return transactionID;
            }
            catch (MGCommonData.MGramProviderException ex)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionID), "Commit", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in Commit -MGI.Cxn.BillPay.MG.Impl.Gateway", ex.Message, ex.StackTrace);
				throw new BillPayException(BillPayException.PROVIDER_ERROR, ex);
            }
            catch (TimeoutException timeoutEx)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionID), "Commit", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in Commit -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, timeoutEx.StackTrace);
				throw new Exception(_errorMessage, timeoutEx);
            }
            catch (WebException webEx)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionID), "Commit", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in Commit -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, webEx.StackTrace);
				throw new Exception(_errorMessage, webEx);
            }
            catch (Exception ex)
            {
				_errorMessage = string.Format("{0}. {1}", _errorMessage, ex.Message);
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(transactionID), "Commit", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in Commit -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, ex.StackTrace);
			                
                throw new Exception(_errorMessage, ex);
            }
        }

        public long AddBillPayAccount(BillPayRequest request, string timezone)
        {
            Account account = new Account()
            {
                FirstName = request.CustomerFirstName,
                LastName = request.CustomerLastName,
                Address1 = request.CustomerAddress1,
                Address2 = request.CustomerAddress2,
                City = request.CustomerCity,
                State = request.CustomerState,
                PostalCode = request.CustomerZip,
                Street = request.CustomerStreet,
                DateOfBirth = request.CustomerDateOfBirth,
                Email = request.CustomerEmail,
                ContactPhone = request.CustomerPhoneNumber,
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
				MobilePhone = request.CustomerMobileNumber,
                DTServerCreate = DateTime.Now
            };

            MgAccountRepo.AddWithFlush(account);
            return account.Id;
        }

        public BillPayAccount GetBillPayAccount(long cxnAccountID)
        {
            Account account = MgAccountRepo.FindBy(a => a.Id == cxnAccountID);

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
                DateOfBirth = account.DateOfBirth,
                Email = account.Email,
                MobilePhone = account.MobilePhone,
                Street = account.Street,
                SmsNotificationFlag = account.SmsNotificationFlag
            };
        }

        public BillPayTransaction GetTransaction(long transactionID)
        {
            Transaction transaction = ProcessorDAL.GetTransactionxById(transactionID);

            BillPayTransaction billPayTrx = Mapper.Map<BillPayTransaction>(transaction);

            return billPayTrx;
        }

		public List<Location> GetLocations(string billerName, string accountNumber, decimal amount, MGIContext mgiContext)
        {
            throw new NotImplementedException();
        }

		public Fee GetFee(string billerCode, string accountNumber, decimal amount, Location location, MGIContext mgiContext)
        {
            try
            {
                long transactionId = 0;

                string complianceMsg = CheckCompliance(billerCode, amount);

                if (complianceMsg != "")
                    throw new BillPayException(BillPayException.PROVIDER_ERROR);

				MGCommonService.FeeLookupRequest feeLookupRequest = BuildFeeRequest(billerCode, accountNumber, amount, mgiContext, out transactionId);

                _errorMessage = string.Format("Error while get fee for {0}", amount);

				MGCommonService.FeeLookupResponse commonResponse = MoneyGramCommonIO.GetFee(feeLookupRequest, mgiContext);

				UpdateTransaction(transactionId, commonResponse, mgiContext);
                Fee fee = FeeMapper(commonResponse);
                fee.TransactionId = transactionId;
                return fee;
            }
            catch (MGCommonData.MGramProviderException ex)
            {
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Biller Code:" + billerCode);
				details.Add("Biller Code:" + accountNumber);
				details.Add("Biller Code:" + Convert.ToString(amount));
				MongoDBLogger.ListError<string>(details, "GetFee", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetFee -MGI.Cxn.BillPay.MG.Impl.Gateway", ex.Message, ex.StackTrace);
				throw new BillPayException(BillPayException.PROVIDER_ERROR, ex);
            }
            catch (MGramProviderException ex)
            {
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Biller Code:" + billerCode);
				details.Add("Biller Code:" + accountNumber);
				details.Add("Biller Code:" + Convert.ToString(amount));
				MongoDBLogger.ListError<string>(details, "GetFee", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetFee -MGI.Cxn.BillPay.MG.Impl.Gateway", ex.Message, ex.StackTrace);
				throw new BillPayException(BillPayException.PROVIDER_ERROR, ex);
            }
            catch (BillPayException ex)
            {
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Biller Code:" + billerCode);
				details.Add("Biller Code:" + accountNumber);
				details.Add("Biller Code:" + Convert.ToString(amount));
				MongoDBLogger.ListError<string>(details, "GetFee", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetFee -MGI.Cxn.BillPay.MG.Impl.Gateway", ex.Message, ex.StackTrace);
				throw ex;
            }
            catch (TimeoutException timeoutEx)
            {
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Biller Code:" + billerCode);
				details.Add("Biller Code:" + accountNumber);
				details.Add("Biller Code:" + Convert.ToString(amount));
				MongoDBLogger.ListError<string>(details, "GetFee", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetFee -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, timeoutEx.StackTrace);
				throw new Exception(_errorMessage, timeoutEx);
            }
            catch (WebException webEx)
            {
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Biller Code:" + billerCode);
				details.Add("Biller Code:" + accountNumber);
				details.Add("Biller Code:" + Convert.ToString(amount));
				MongoDBLogger.ListError<string>(details, "GetFee", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetFee -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, webEx.StackTrace);
				throw new Exception(_errorMessage, webEx);
            }
            catch (Exception ex)
            {
                 _errorMessage = string.Format("{0}. {1}", _errorMessage, ex.Message);
				 //AL-1014 Transactional Log User Story
				 List<string> details = new List<string>();
				 details.Add("Biller Code:" + billerCode);
				 details.Add("Biller Code:" + accountNumber);
				 details.Add("Biller Code:" + Convert.ToString(amount));
				 MongoDBLogger.ListError<string>(details, "GetFee", AlloyLayerName.CXN, ModuleName.BillPayment,
					 "Error in GetFee -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, ex.StackTrace);				
                throw new Exception(_errorMessage, ex);
            }
        }

		public BillerInfo GetBillerInfo(string billerCode, MGIContext mgiContext)
        {
            BillerInfo billerInfo = new BillerInfo();
            try
            {
                var baseRequest = PopulateBaseRequest();

                var request = new BillerSearchRequest()
                {
                    agentID = baseRequest.AgentID,
                    agentSequence = baseRequest.AgentSequence,
                    token = baseRequest.Token,
                    apiVersion = baseRequest.ApiVersion,
                    clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
                    timeStamp = baseRequest.TimeStamp,
                };


                request.receiveCode = billerCode;
                request.searchType = searchType.CODE;

                _errorMessage = string.Format("Error while getting biller message for {0}", billerCode);

                MGI.Cxn.BillPay.MG.AgentConnectService.BillerSearchResponse billerSearchResponse = MoneyGramIO.BillerSearch(request);

                if (billerSearchResponse.billerInfo != null)
                {
                    if (billerSearchResponse.billerInfo[0].billerNotes != null)
                        billerInfo.DeliveryOption = billerSearchResponse.billerInfo[0].billerNotes;

                    if (billerSearchResponse.billerInfo[0].expectedPostingTimeFrame != null)
                        billerInfo.Message = billerSearchResponse.billerInfo[0].expectedPostingTimeFrame;

                    if (billerSearchResponse.billerInfo[0].billerState != null)
                        billerInfo.BillerState = billerSearchResponse.billerInfo[0].billerState;

                    if (billerSearchResponse.billerInfo[0].billerCity != null)
                        billerInfo.BillerCity = billerSearchResponse.billerInfo[0].billerCity;
                }
            }
            catch (MGramProviderException ex)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(billerCode, "GetBillerInfo", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetBillerInfo -MGI.Cxn.BillPay.MG.Impl.Gateway", ex.Message, ex.StackTrace);
				throw new BillPayException(BillPayException.PROVIDER_ERROR, ex);
            }
            catch (TimeoutException timeoutEx)
            {
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(billerCode, "GetBillerInfo", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetBillerInfo -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, timeoutEx.StackTrace);
                throw new Exception(_errorMessage, timeoutEx);
            }
            catch (WebException webEx)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(billerCode, "GetBillerInfo", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetBillerInfo -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, webEx.StackTrace);
				throw new Exception(_errorMessage, webEx);
            }
            catch (Exception ex)
            {
				_errorMessage = string.Format("{0}. {1}", _errorMessage, ex.Message);
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(billerCode, "GetBillerInfo", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetBillerInfo -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, ex.StackTrace);
				
                throw new Exception(_errorMessage, ex);
            }

            BillerLimit billerLimit = ProcessorDAL.GetBillerLimit(billerCode);

            if (billerLimit != null && billerLimit.Denominations != null)
            {
                billerInfo.Denominations = new List<decimal>();
                foreach (var denomination in billerLimit.Denominations)
                {
                    billerInfo.Denominations.Add(denomination.DenominationAmount);
                }
                billerInfo.Denominations.Sort();
            }

            return billerInfo;
        }

		public List<Field> GetProviderAttributes(string billerName, string locationName, MGIContext mgiContext)
        {
            try
            {
                List<string> standardBPFields = new List<string>
			    {
				    "operatorname", "productvariant",
                    "amount","destinationcountry","receivecode","receiveagentid","billeraccountnumber","senderfirstname","sendermiddlename","senderlastname","senderlastname2",
                    "formfreestaging","senderaddress","sendercity","senderstate","senderzipcode","sendercountry","senderhomephone","feeamount","accountnumberretrycount",
                    "sendcurrency","receivecurrency","mgitransactionsessionid","formfeestaging","senderphotoidtype","senderphotoidnumber","senderphotoidstate","senderphotoidcountry",
                    "senderlegalidtype","senderlegalidnumber","senderdob","senderoccupation","cardswiped","validateaccountnumber"
			    };

                var fields = new List<Field>();
                Field field;

                Transaction transaction = MgTransactionRepo.FindBy(x => x.Id == mgiContext.TrxId);

                transaction.IsValidateAccNumberRequired = false;

                _errorMessage = string.Format("Error while get fields for product {0}", billerName);

				MGCommonService.GetFieldsForProductRequest GFFPRequest = BuildGFFPRequest(mgiContext, transaction);
				MGCommonService.GetFieldsForProductResponse resp = MoneyGramCommonIO.GetFieldsForProduct(GFFPRequest, mgiContext);

                foreach (MGCommonService.ProductFieldInfo fieldInfo in resp.productFieldInfo)
                {
                    if (fieldInfo.visibility == MGCommonService.ProductFieldInfoVisibility.REQ
                        || fieldInfo.visibility == MGCommonService.ProductFieldInfoVisibility.OPT
                        )
                    {
                        if (!standardBPFields.Contains(fieldInfo.xmlTag.ToLower()))
                        {
                            field = new Field
                                        {
                                            DataType = fieldInfo.enumerated ? "Dropdown" : "TextBox",
                                            Label = fieldInfo.fieldLabel
                                        };

                            field.MaxLength = (int)fieldInfo.fieldMax;

                            field.IsMandatory = fieldInfo.visibility == MGCommonService.ProductFieldInfoVisibility.REQ;

                            field.TagName = fieldInfo.xmlTag;

                            field.Values = fieldInfo.enumerated ? fieldInfo.enumeratedValues.ToDictionary(e => e.label, e => e.value) : null;

                            fields.Add(field);
                        }
                        else if (fieldInfo.xmlTag.ToLower() == "validateaccountnumber") //ValidateAccountNumber is a disallowed field for some billers, but mandatory for some. Hence this should be passed only if the value is true in the transaction table. 
                        {
                            transaction.IsValidateAccNumberRequired = true;
                        }
                    }
                }

                if (resp.dynamicFieldInfo != null)
                {
                    foreach (MGCommonService.DynamicFieldInfo fieldInfo in resp.dynamicFieldInfo)
                    {
                        if (fieldInfo.visibility == MGCommonService.ProductFieldInfoVisibility.REQ
                            || fieldInfo.visibility == MGCommonService.ProductFieldInfoVisibility.OPT)
                        {
                            if (!standardBPFields.Contains(fieldInfo.xmlTag.ToLower()))
                            {
                                field = new Field
                                            {
                                                DataType = fieldInfo.enumerated ? "Dropdown" : "TextBox",
                                                Label = fieldInfo.fieldLabel
                                            };

                                if (fieldInfo.fieldMaxSpecified)
                                {
                                    field.MaxLength = (int)fieldInfo.fieldMax;
                                }
                                field.IsMandatory = fieldInfo.visibility == MGCommonService.ProductFieldInfoVisibility.REQ;

                                field.TagName = fieldInfo.xmlTag;

                                field.Values = fieldInfo.enumerated ? fieldInfo.enumeratedValues.ToDictionary(e => e.value, e => e.label) : null;

                                fields.Add(field);
                            }
                        }
                    }
                }

                try
                {
                    MgTransactionRepo.UpdateWithFlush(transaction);
                }
                catch (Exception ex)
                {
					//AL-1014 Transactional Log User Story
					List<string> details = new List<string>();
					details.Add("Biller Name:" + billerName);
					details.Add("Location Name:" + locationName);
					MongoDBLogger.ListError<string>(details, "GetProviderAttributes", AlloyLayerName.CXN, ModuleName.BillPayment,
						"Error in GetProviderAttributes -MGI.Cxn.BillPay.MG.Impl.Gateway", ex.Message, ex.StackTrace);
					throw new Exception("Error in MGI Transaction update", ex.InnerException);
                }

                return fields;
            }
            catch (MGCommonData.MGramProviderException ex)
            {
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Biller Name:" + billerName);
				details.Add("Location Name:" + locationName);
				MongoDBLogger.ListError<string>(details, "GetProviderAttributes", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetProviderAttributes -MGI.Cxn.BillPay.MG.Impl.Gateway", ex.Message, ex.StackTrace);
				throw new BillPayException(BillPayException.PROVIDER_ERROR, ex);
            }
            catch (TimeoutException timeoutEx)
            {
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Biller Name:" + billerName);
				details.Add("Location Name:" + locationName);
				MongoDBLogger.ListError<string>(details, "GetProviderAttributes", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetProviderAttributes -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, timeoutEx.StackTrace);
				throw new Exception(_errorMessage, timeoutEx);
            }
            catch (WebException webEx)
            {
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Biller Name:" + billerName);
				details.Add("Location Name:" + locationName);
				MongoDBLogger.ListError<string>(details, "GetProviderAttributes", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetProviderAttributes -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, webEx.StackTrace);
				throw new Exception(_errorMessage, webEx);
            }
            catch (Exception ex)
            {                
                _errorMessage = string.Format("{0}. {1}", _errorMessage, ex.Message);
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Biller Name:" + billerName);
				details.Add("Location Name:" + locationName);
				MongoDBLogger.ListError<string>(details, "GetProviderAttributes", AlloyLayerName.CXN, ModuleName.BillPayment,
					"Error in GetProviderAttributes -MGI.Cxn.BillPay.MG.Impl.Gateway", _errorMessage, ex.StackTrace);			
                throw new Exception(_errorMessage, ex);
            }
        }

		public long UpdateCardDetails(long cxnAccountId, string cardNumber, MGIContext mgiContext, string timezone)
        {
            throw new NotImplementedException();
        }

		public CardInfo GetCardInfo(string cardNumber, MGIContext mgiContext)
        {
            throw new NotImplementedException();
        }

		public List<Biller> GetPastBillers(long customerSessionId, string cardNumber, MGIContext mgiContext)
        {
            throw new NotImplementedException();
        }

		public BillPayTransaction GetBillerLastTransaction(string billerCode, long cxnAccountId, MGIContext mgiContext)
        {
            Transaction transaction = ProcessorDAL.GetBillerLastTransaction(billerCode, cxnAccountId);

            var billPayTransaction = Mapper.Map<BillPayTransaction>(transaction);

            if (transaction != null)
                billPayTransaction.MetaData.Add("LastTransactionDate", transaction.DTTerminalLastModified);

            return billPayTransaction;
        }

        #endregion

        #region Private Methods

        private Account GetAccount(long cxnAccountID)
        {
            return MgAccountRepo.FindBy(a => a.Id == cxnAccountID);
        }

        private MGCommonService.FeeLookupRequest BuildFeeRequest(string billerCode, string accountNumber, decimal amount, MGIContext mgiContext, out long transactionId)
        {
            transactionId = 0;

            var baseRequest = PopulateBaseRequest();

            var request = new MGCommonService.FeeLookupRequest()
            {
                agentID = baseRequest.AgentID,
                agentSequence = baseRequest.AgentSequence,
                token = baseRequest.Token,
                apiVersion = baseRequest.ApiVersion,
                clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
                timeStamp = baseRequest.TimeStamp,
                allOptions = false
            };

            request.productType = MGCommonService.productType.BP;
            BillerSearchResponse billerSearchResponse;
            request.productVariant = GetProductVariant(billerCode, out billerSearchResponse);

            request.productVariantSpecified = true;

            request.receiveCountry = "USA";
            request.receiveCurrency = "USD";

            request.Item = amount;
            request.ItemElementName = MGCommonService.ItemChoiceType.amountExcludingFee;

            request.receiveCode = billerCode;

            Account account = null;
            if (mgiContext.CxnAccountId != 0)
            {
                account = GetAccount(mgiContext.CxnAccountId);
            }
            try
            {

                Transaction transaction;

                if (mgiContext.TrxId != 0)
                {
                    transaction = ProcessorDAL.GetTransactionxById(mgiContext.CxnTransactionId);

                    Mapper.Map(request, transaction);
                }
                else
                {
                    transaction = Mapper.Map<Transaction>(request);
                }
                transaction.Account = account;
                transaction.AccountNumber = accountNumber;
                transaction.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
                transaction.DTServerCreate = DateTime.Now;
                transaction.RequestResponseType = (int)RequestResponseType.FeeRequest;

                if (mgiContext.TrxId == 0)
                {
                    MgTransactionRepo.AddWithFlush(transaction);
                }
                else
                {
                    MgTransactionRepo.UpdateWithFlush(transaction);
                }
                transactionId = transaction.Id;
                UpdateBillerSearchResponse(transactionId, billerSearchResponse, mgiContext);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in MGI Transaction update", ex.InnerException);
            }
            return request;
        }

        private MGCommonService.productVariant GetProductVariant(string billerCode, out BillerSearchResponse billerSearchResponse)
        {
            billerSearchResponse = null;

            var baseRequest = PopulateBaseRequest();

            var request = new BillerSearchRequest()
            {
                agentID = baseRequest.AgentID,
                agentSequence = baseRequest.AgentSequence,
                token = baseRequest.Token,
                apiVersion = baseRequest.ApiVersion,
                clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
                timeStamp = baseRequest.TimeStamp,
            };

            request.receiveCode = billerCode;
            request.searchType = searchType.CODE;
			try
			{
				billerSearchResponse = MoneyGramIO.BillerSearch(request);
			}
			catch (MGramProviderException ex)
			{
				throw new BillPayException(BillPayException.PROVIDER_ERROR, ex);
			}
            productVariant productVariant = new productVariant();

            if (billerSearchResponse.billerInfo != null)
            {
                productVariant = billerSearchResponse.billerInfo[0].productVariant;
            }
            return (MGCommonService.productVariant)productVariant;
        }

        private MGCommonService.GetFieldsForProductRequest BuildGFFPRequest(MGIContext context, Transaction transaction)
        {
            var baseRequest = PopulateBaseRequest();

            var request = new MGCommonService.GetFieldsForProductRequest()
            {
                agentID = baseRequest.AgentID,
                agentSequence = baseRequest.AgentSequence,
                token = baseRequest.Token,
                apiVersion = baseRequest.ApiVersion,
                clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
                timeStamp = baseRequest.TimeStamp,
            };

            request.productType = MGCommonService.productType.BP;
            request.productVariant = (MGCommonService.productVariant)transaction.ProductVariant;

            request.receiveCountry = "USA";
            request.receiveCurrency = "USD";
            request.productVariantSpecified = true;
            request.consumerId = "0";

            request.thirdPartyType = MGCommonService.thirdPartyType.NONE;
            request.receiveAgentID = transaction.ReceiveAgentID;
            request.amount = transaction.SendAmount;

            //request.billerAccountNumber = Convert.ToString(context["billerAccountNumber"]);
            request.billerAccountNumber = transaction.AccountNumber;
            request.cardSwiped = false;
            request.cardSwipedSpecified = true;

            return request;
        }

        private Fee FeeMapper(MGCommonService.FeeLookupResponse feeResponse)
        {
            var fee = new Fee();
            if (feeResponse.feeInfo.Any())
            {
                fee.DeliveryMethods = new List<DeliveryMethod>()
				{
					new DeliveryMethod()
					{
						Code = feeResponse.feeInfo[0].deliveryOptId,
						FeeAmount = feeResponse.feeInfo[0].sendAmounts.totalSendFees,
                        Tax=feeResponse.feeInfo[0].sendAmounts.totalSendTaxes,
						Text = feeResponse.feeInfo[0].deliveryOptDisplayName
					}
				};

            }
            return fee;
        }

        private void UpdateTransaction(long transactionId, MGCommonService.FeeLookupResponse feeResponse, MGIContext mgiContext)
        {
            Transaction transaction = MgTransactionRepo.FindBy(x => x.Id == transactionId);

            Mapper.Map(feeResponse, transaction);
            Mapper.Map(feeResponse.feeInfo[0], transaction);
            transaction.RequestResponseType = (int)RequestResponseType.FeeResponse;
            transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
            transaction.DTServerLastModified = DateTime.Now;

            MgTransactionRepo.UpdateWithFlush(transaction);
        }

        private void UpdateValidationResponse(BpValidationResponse response, MGIContext mgiContext)
        {
            Transaction transaction = MgTransactionRepo.FindBy(x => x.Id == mgiContext.TrxId);
            transaction.ServiceOfferingID = response.serviceOfferingID;//

            transaction.PrintMGICustomerServiceNumber = response.printMGICustomerServiceNumber;
            transaction.AgentTransactionId = response.agentTransactionId;
            transaction.ReadyForCommit = response.readyForCommit;
            transaction.ProcessingFee = response.processingFee;
            transaction.InfoFeeIndicator = response.infoFeeIndicator;
            transaction.ExchangeRateApplied = response.exchangeRateApplied;
            transaction.SendAmount = response.sendAmounts.sendAmount;
            transaction.SendCurrency = response.sendAmounts.sendCurrency;
            transaction.TotalSendFees = response.sendAmounts.totalSendFees;
            transaction.TotalDiscountAmount = response.sendAmounts.totalDiscountAmount;
            transaction.TotalSendTaxes = response.sendAmounts.totalSendTaxes;
            transaction.TotalAmountToCollect = response.sendAmounts.totalAmountToCollect;
            transaction.RequestResponseType = (int)RequestResponseType.ValidationResponse;
            if(response.customerTips != null)
                transaction. CustomerTipTextTranslation = response.customerTips[0].textTranslation;
            transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
            transaction.DTServerCreate = DateTime.Now;
            MgTransactionRepo.UpdateWithFlush(transaction);
        }

		private BpValidationRequest BuildValidationRequest(BillPayRequest request, Transaction transaction, MGIContext mgiContext)
        {
            var baseRequest = PopulateBaseRequest();

            var req = new BpValidationRequest()
            {
                agentID = baseRequest.AgentID,
                agentSequence = baseRequest.AgentSequence,
                token = baseRequest.Token,
                apiVersion = baseRequest.ApiVersion,
                clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
                timeStamp = baseRequest.TimeStamp,
                agentConsumerID = "0",
            };

            req.amount = request.Amount;
            req.billerAccountNumber = request.AccountNumber;

            //ValidateAccountNumber is a disallowed field for some billers. Hence this should be passed only if the value is true in the transaction table. 
            if (transaction.IsValidateAccNumberRequired)
                req.validateAccountNumber = request.AccountNumber;

            req.receiveCurrency = "USD";
            req.productVariant = (productVariant)transaction.ProductVariant;
            req.feeAmount = transaction.ReceiveAmount;
            req.destinationCountry = "USA";
            req.receiveCode = transaction.ReceiveCode;
            req.receiveAgentID = transaction.ReceiveAgentID;

            req.cardSwiped = false;
            req.cardSwipedSpecified = true;

            req.senderFirstName = NexxoUtil.TrimString(request.CustomerFirstName, 14);

            if (!string.IsNullOrWhiteSpace(request.CustomerMiddleName))
                req.senderMiddleName = NexxoUtil.TrimString(request.CustomerMiddleName, 1);

            if (!string.IsNullOrWhiteSpace(request.CustomerLastName2))
                req.senderLastName2 = NexxoUtil.TrimString(request.CustomerLastName2, 20);

            req.senderLastName = NexxoUtil.TrimString(request.CustomerLastName, 20);
            req.senderAddress = NexxoUtil.TrimString(request.CustomerAddress1, 30);

            //senderAddress2 is disallowed for some billers
            //if (!(string.IsNullOrEmpty(request.CustomerAddress2)))
            //{
            //    req.senderAddress2 = NexxoUtil.TrimString(request.CustomerAddress2, 30);
            //}

            req.senderDOB = request.CustomerDateOfBirth;

            if (!string.IsNullOrWhiteSpace(request.Occupation))
                req.senderOccupation = request.Occupation;

            req.senderCity = NexxoUtil.TrimString(request.CustomerCity, 20);
            req.senderState = request.CustomerState;
            req.senderZipCode = request.CustomerZip;
            req.senderCountry = "USA";
            req.senderHomePhone = request.CustomerPhoneNumber;

            req.sendCurrency = "USD";
            req.mgiTransactionSessionID = transaction.MgiTransactionSessionID;
            req.formFreeStaging = false;

            if (!string.IsNullOrWhiteSpace(request.SecondIdNumber))
            {
                // Compliance Information
                req.senderLegalIdType = legalIdType.SSN;
                req.senderLegalIdTypeSpecified = true;
                req.senderLegalIdNumber = request.SecondIdNumber;
            }

            req.senderPhotoIdType = GetPhotoIDType(request.PrimaryIdType);
            req.senderPhotoIdTypeSpecified = true;
            req.senderPhotoIdCountry = GetCountryCode(request.PrimaryIdCountryOfIssue);
            req.senderPhotoIdState = GetStateCode(request.PrimaryIdPlaceOfIssue, req.senderPhotoIdCountry);
            req.senderPhotoIdNumber = NexxoUtil.TrimString(request.PrimaryIdNumber, 20);

            if (mgiContext.AccountNumberRetryCount == "2")
            {
                req.accountNumberRetryCount = (string)mgiContext.AccountNumberRetryCount=="2" ? "2" : "1";
            }
            else
            {
                req.accountNumberRetryCount = "1";
            } 
            if (request.MetaData != null)
            {
                foreach (var o in request.MetaData)
                {
                    if (req.GetType().GetProperty(o.Key) != null)
                    {
                        System.Reflection.PropertyInfo propertyInfo = req.GetType().GetProperty(o.Key);
                        Type propertyType = propertyInfo.PropertyType;

                        if (!(propertyInfo.PropertyType.Name == "String" && string.IsNullOrEmpty(Convert.ToString(o.Value))))
                        {
                            var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;
                            propertyInfo.SetValue(req, Convert.ChangeType((Convert.ChangeType(o.Value, targetType)), propertyType), null);
                        }
                    }
                }
            }

			UpdateValidationRequest(req, mgiContext);
            return req;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

		private void UpdateValidationRequest(BpValidationRequest request, MGIContext mgiContext)
        {
            Transaction transaction = MgTransactionRepo.FindBy(x => x.Id == mgiContext.TrxId);

            Mapper.Map(request, transaction);

            //do a biller search to save expectedpostingtimeframe and expectedpostingtimeframesecondary
            try
            {
                var baseRequest = PopulateBaseRequest();

                var searchRequest = new BillerSearchRequest()
                {
                    agentID = baseRequest.AgentID,
                    agentSequence = baseRequest.AgentSequence,
                    token = baseRequest.Token,
                    apiVersion = baseRequest.ApiVersion,
                    clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
                    timeStamp = baseRequest.TimeStamp,
                };

                searchRequest.receiveCode = transaction.ReceiveCode;
                searchRequest.searchType = searchType.CODE;

                MGI.Cxn.BillPay.MG.AgentConnectService.BillerSearchResponse billerSearchResponse = MoneyGramIO.BillerSearch(searchRequest);

                if (billerSearchResponse.billerInfo != null)
                {
                    transaction.ExpectedPostingTimeFrame = billerSearchResponse.billerInfo[0].expectedPostingTimeFrame;
                    transaction.ExpectedPostingTimeFrameSecondary = billerSearchResponse.billerInfo[0].expectedPostingTimeFrameSecondary;
                }
            }
            catch (MGramProviderException ex)
            {
                throw new BillPayException(BillPayException.PROVIDER_ERROR, ex);
            }
             
            transaction.RequestResponseType = (int)RequestResponseType.ValidationRequest;
            transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
            transaction.DTServerCreate = DateTime.Now;
            MgTransactionRepo.UpdateWithFlush(transaction);
        }

		private void UpdateCommitTransactionResponse(long transactionID, MGCommonService.CommitTransactionResponse commitResponse, MGIContext mgiContext)
        {
            Transaction transaction = MgTransactionRepo.FindBy(x => x.Id == transactionID);
            transaction.ReferenceNumber = commitResponse.referenceNumber;
            transaction.PartnerConfirmationNumber = commitResponse.partnerConfirmationNumber;
            transaction.PartnerName = commitResponse.partnerName;
            transaction.FreePhoneCallPin = commitResponse.freePhoneCallPIN;
            transaction.TollFreePhoneNumber = commitResponse.referenceNumber;
            //transaction.ExpectedDateOfDelivery = commitResponse.expectedDateOfDelivery;
            // transaction.TransactionDateTime = commitResponse.transactionDateTime;
            transaction.RequestResponseType = (int)RequestResponseType.CommitResponse;
            transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
            transaction.DTServerLastModified = DateTime.Now;
            if (commitResponse.receiptTextInfo != null)
                transaction.TextTranslation = commitResponse.receiptTextInfo[0].textTranslation;
            MgTransactionRepo.UpdateWithFlush(transaction);
        }

		private MGCommonService.CommitTransactionRequest BuildCommitRequest(long transactionID, MGIContext mgiContext)
        {

            Transaction transaction = MgTransactionRepo.FindBy(x => x.Id == transactionID);

            var baseRequest = PopulateBaseRequest();

            MGCommonService.CommitTransactionRequest request = new MGCommonService.CommitTransactionRequest()
            {
                agentID = baseRequest.AgentID,
                agentSequence = baseRequest.AgentSequence,
                token = baseRequest.Token,
                apiVersion = baseRequest.ApiVersion,
                clientSoftwareVersion = baseRequest.ClientSoftwareVersion,
                timeStamp = baseRequest.TimeStamp,
                mgiTransactionSessionID = transaction.MgiTransactionSessionID,
            };

            try
            {
                transaction.MgiTransactionSessionID = request.mgiTransactionSessionID;
                transaction.RequestResponseType = (int)RequestResponseType.CommitRequest;
                MgTransactionRepo.UpdateWithFlush(transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in MGI Transaction update", ex.InnerException);
            }

            return request;

        }

        private string CheckCompliance(string billerCode, decimal amount)
        {
            //Record is there
            //If denomination is there, amount should be one of that throw denomination
            //if no denomination, then check for minimum or maximum

            BillerLimit bLimit = ProcessorDAL.GetBillerLimit(billerCode);
            string errorMessage = "";

            if (bLimit != null)
            {
                if (bLimit.Denominations.Count > 0)
                {
                    //amount should be one of the denominations
                    BillerDenomination bDenom = bLimit.Denominations.FirstOrDefault(x => x.DenominationAmount == amount);

                    if (bDenom == null)
                        errorMessage = bLimit.MessageForAmtNotInList;
                }
                else
                {
                    if (bLimit.MinimumAmount != 0 && amount < bLimit.MinimumAmount) //amount should not be less than minimum
                        errorMessage = bLimit.MinimumAmountMessage;
                    else if (bLimit.MaximumAmount != 0 && amount > bLimit.MaximumAmount) //amount should not be greater than maximum
                        errorMessage = bLimit.MaximumAmountMessage;
                }
            }

            return errorMessage;
        }

        private photoIdType GetPhotoIDType(string idType)
        {
            return Mapper.Map<photoIdType>(MoneyGramCommonIO.GetPhotoIdType(idType));
        }

        private string GetCountryCode(string countyName)
        {
            string countryCode = string.Empty;

            if (!string.IsNullOrWhiteSpace(countyName))
            {
                var country = CountryRepo.FindBy(c => c.Name == countyName);
                if (country != null)
                {
                    countryCode = country.Code;
                }
            }

            return countryCode;
        }

        private string GetStateCode(string stateName, string countryCode)
        {
            string stateCode = string.Empty;

            if (!string.IsNullOrWhiteSpace(stateName))
            {
                var state = StateRepo.FindBy(c => c.Name == stateName);
                if (state != null)
                {
                    stateCode = state.Code;
                }
            }
            else
            {
                var states = StateRepo.FilterBy(c => c.Countrycode == countryCode);
                if (states != null && states.Any())
                {
                    stateCode = states.FirstOrDefault().Code;
                }
            }
            return stateCode;
        }

        private string GetProductVariantName(productVariant variant)
        {
            string details = string.Empty;
            switch (variant)
            {
                case productVariant.EP:
                    details = "ExpressPayment";
                    break;
                case productVariant.UBP:
                    details = "Utility Bill Payment";
                    break;
            }
            return details;
        }

		private void UpdateBillerSearchResponse(long transactionId, BillerSearchResponse response, MGIContext mgiContext)
        {
            Transaction transaction = MgTransactionRepo.FindBy(x => x.Id == transactionId);

            if (response.billerInfo != null && response.billerInfo.Count() > 0)
            {
                transaction.BillerName = response.billerInfo[0].billerName;
                transaction.BillerWebsite = response.billerInfo[0].billerWebsite;
                transaction.BillerPhone = response.billerInfo[0].billerPhoneNumber;
                transaction.BillerCutoffTime = response.billerInfo[0].billerCutoffTime;
                transaction.BillerAddress = response.billerInfo[0].address1;
                transaction.BillerAddress2 = response.billerInfo[0].address2;
                transaction.BillerAddress3 = response.billerInfo[0].address3;
                transaction.BillerCity = response.billerInfo[0].billerCity;
                transaction.BillerState = response.billerInfo[0].billerState;
            }
            transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
            transaction.DTServerLastModified = DateTime.Now;
            MgTransactionRepo.UpdateWithFlush(transaction);
        }

        private MGCommonData.BaseRequest PopulateBaseRequest()
        {
            MGCommonData.BaseRequest baserequest = new MGCommonData.BaseRequest();

            baserequest.AgentID = "30042575";
            baserequest.AgentSequence = "1";
            baserequest.Token = "TEST";
            baserequest.ApiVersion = "1305";
            baserequest.ClientSoftwareVersion = "10.2";
            baserequest.TimeStamp = DateTime.Now;

            return baserequest;
        }

		public void UpdateGoldCardPoints(long transactionId, string totalPointsEarned, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		public void UpdateAccount(BillPayAccount billPayAccount, MGIContext mgiContext)
		{
			Account account = MgAccountRepo.FindBy(x => x.Id == billPayAccount.Id);
			if(account != null)
			{
				account.FirstName = billPayAccount.FirstName;
				account.LastName = billPayAccount.LastName;
				account.Address1 = billPayAccount.Address1;
				account.Address2 = billPayAccount.Address2;
				account.City  = billPayAccount.City;
				account.State = billPayAccount.State;
				account.PostalCode = billPayAccount.PostalCode;
				account.Street = billPayAccount.Street;
				account.DateOfBirth=billPayAccount.DateOfBirth;
				account.Email=billPayAccount.Email;
				account.ContactPhone=billPayAccount.ContactPhone;
				account.SmsNotificationFlag = billPayAccount.SmsNotificationFlag;
				account.DTServerLastModified = DateTime.Now;
				account.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
				MgAccountRepo.Update(account);
			}
			
		}
        #endregion

    }
}
