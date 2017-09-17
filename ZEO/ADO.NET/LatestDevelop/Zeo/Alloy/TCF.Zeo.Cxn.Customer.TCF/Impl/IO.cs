using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Security.Cryptography.X509Certificates;

#region Alloy References
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.Customer.Data;
using TCF.Zeo.Cxn.Customer.TCF.Data;
using TCF.Zeo.Cxn.Customer.TCF.RCIFService;
using static TCF.Zeo.Common.Util.Helper;
using System.Globalization;
using TCF.Zeo.Common.Data;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Cxn.Customer.TCF.Contract;
using TCF.Zeo.Cxn.Customer.TCF.Data.Exceptions;
using TCF.Zeo.Cxn.Customer.Data.Exceptions;
using System.ServiceModel;
using TCF.Zeo.Common.Logging.Impl;
using System.Net;
using System.Net.Security;

#endregion

namespace TCF.Zeo.Cxn.Customer.TCF.Impl
{
    public class IO : IIO
    {
        IMapper mapper;
        static Dictionary<string, string> errorMessages = new Dictionary<string, string>();
        internal X509Certificate2 RCIFCertificate = null;

        public IO()
        {
            #region Mapping
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerProfile, PersonalInfo>()
                .ForMember(x => x.FName, o => o.MapFrom(s => s.FirstName))

                .ForMember(x => x.LName, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.LastName2) ? s.LastName : string.Format("{0} {1}", s.LastName, s.LastName2)))
                .ForMember(x => x.Addr1, o => o.MapFrom(s => AlloyUtil.TrimString(s.Address.Address1, 30)))
                .ForMember(x => x.Addr2, o => o.MapFrom(s => AlloyUtil.TrimString(s.Address.Address2, 30)))
                .ForMember(x => x.City, o => o.MapFrom(s => s.Address.City))
                .ForMember(x => x.State, o => o.MapFrom(s => s.Address.State))
                .ForMember(x => x.zip, o => o.MapFrom(s => s.Address.ZipCode))
                .ForMember(x => x.Ph1, o => o.MapFrom(s => s.Phone1.Number))
                //Do Not pass phone providers
                .ForMember(x => x.Ph1Type1, o => o.MapFrom(s => s.Phone1.Type != null ? MapToClientPhoneTypes(s.Phone1.Type) : null))
                .ForMember(x => x.Ph2, o => o.MapFrom(s => s.Phone2.Number))
                .ForMember(x => x.Ph2Type2, o => o.MapFrom(s => s.Phone2.Type != null ? MapToClientPhoneTypes(s.Phone2.Type) : null))
                .ForMember(x => x.ssn, o => o.MapFrom(s => s.SSN))
                .ForMember(x => x.ClientCustId, o => o.MapFrom(s => string.IsNullOrWhiteSpace((s.ClientCustomerId.ToString())) ? "0" : s.ClientCustomerId))
                .ForMember(x => x.gender, o => o.MapFrom(s => s.Gender != null ? s.Gender.ToString().Substring(0, 1) : null))
                .ForMember(x => x.TaxCd, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.SSN) && (s.SSN.Substring(0, 1) == "9") ? TaxCdITIN : !string.IsNullOrWhiteSpace(s.SSN) ? TaxCdSSN : null))
                .ForMember(x => x.tcfCustind, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.ClientCustomerId) && s.ClientCustomerId != "0" ? "Y" : "N"))
                .AfterMap((s, d) =>
                {
                    d.PRODCODE = PRODCODE;
                    d.APPL_CD = APPL_CD;
                    if (!string.IsNullOrWhiteSpace(s.MiddleName))
                    {
                        d.MName = s.MiddleName;
                    }
                });

                cfg.CreateMap<CustomerProfile, Identification>()
                .ForMember(x => x.dob, o => o.MapFrom(s => s.DateOfBirth != null ? s.DateOfBirth.Value.ToString("yyyyMMdd") : null))
                .ForMember(x => x.maiden, o => o.MapFrom(s => s.MothersMaidenName))
                .ForMember(x => x.idType, o => o.MapFrom(s => s.IdType != null ? MapToClientIdType(s.IdType) : null))
                .ForMember(x => x.idIssuer, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.IDIssuingStateCode) ? null : s.IDIssuingStateCode))
                .ForMember(x => x.idIssuerCountry, o => o.MapFrom(s => s.IdIssuingCountry != null ? s.IdIssuingCountry.Trim() : null))//AL-2169
                .ForMember(x => x.idNbr, o => o.MapFrom(s => s.IdNumber))
                .ForMember(x => x.idIssueDate, o => o.MapFrom(s => s.IdIssueDate != null && s.IdIssueDate != DateTime.MinValue ? s.IdIssueDate.Value.ToString("yyyyMMdd") : null))
                .ForMember(x => x.idExpDate, o => o.MapFrom(s => s.IdExpirationDate != null && s.IdExpirationDate != DateTime.MinValue ? s.IdExpirationDate.Value.ToString("yyyyMMdd") : null))
                .ForMember(x => x.legalCode, o => o.MapFrom(s => s.LegalCode))
                .ForMember(x => x.citizenshipCountry1, o => o.MapFrom(s => s.PrimaryCountryCitizenShip))
                .ForMember(x => x.citizenshipCountry2, o => o.MapFrom(s => s.SecondaryCountryCitizenShip));

                cfg.CreateMap<CustomerProfile, EmploymentInfo>()
                .ForMember(x => x.Occupation, o => o.MapFrom(s => s.Occupation))
                .ForMember(x => x.OccDesc, o => o.MapFrom(s => s.OccupationDescription))
                .ForMember(x => x.EmployerName, o => o.MapFrom(s => s.EmployerName))
                .ForMember(x => x.EmployerPhoneNum, o => o.MapFrom(s => s.EmployerPhone));
            });
            mapper = config.CreateMapper();
            #endregion
        }

        // Added static constructor to initialize static variable.
        //Reference - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors
        static IO()
        {
            string providerMessage = Convert.ToString(ConfigurationManager.AppSettings["RCIFErrorMessages"]);
            if (!string.IsNullOrEmpty(providerMessage))
            {
                errorMessages = providerMessage
                             .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(x => x.Split(':')).ToList()
                             .ToDictionary(split => Convert.ToString(split[0]), split => Convert.ToString(split[1]));
            }
            SetCertificatePolicy();
        }


        private const string SoftStop = "1";
        private const string HardStop = "2";
        private const string TaxCdITIN = "I";
        private const string TaxCdSSN = "S";
        private const string PRODCODE = "ZEOSVC";
        private const string APPL_CD = "SVC";
        private const string CheckProcessing = "CheckProcessing";

        #region Customer

        public List<CustomerProfile> SearchCoreCustomers(CustomerSearchCriteria criteria, RCIFCredential credential, ZeoContext context)
        {
            List<CustomerProfile> customers = null;
            try
            {
                long channelPartnerId = context.ChannelPartnerId;

                ZeoFindCustomerReq request = new ZeoFindCustomerReq();

                Svc[] baserequest = new Svc[1];

                baserequest[0] = new Svc();

                baserequest[0].Security = new Security()
                {
                    BasicAuth = GetBasicAuth(context.SSOAttributes)
                };

                baserequest[0].SvcParms = GetsvcParams();

                MsgData msgData = new MsgData()
                {
                    ssn = criteria.SSN,
                    cardNum = criteria.Cardnumber,
                    AcctNum = string.IsNullOrWhiteSpace(criteria.Accountnumber) ? null : criteria.Accountnumber.PadLeft(18, '0'),
                    dob = criteria.DateOfBirth.HasValue ? Convert.ToDateTime(criteria.DateOfBirth).ToString("yyyyMMdd") : null,
                    ClientCustId = string.IsNullOrWhiteSpace(criteria.ClientCustId) ? null : criteria.ClientCustId,
                };
                baserequest[0].MsgData = msgData;

                request.Svc = baserequest;

                string ErrMsg = string.Empty;
                string mtvnSvcVer = string.Empty;
                string msgUUID = string.Empty;

                Svc1[] svcResponse = new Svc1[1];
                svcResponse[0] = new Svc1();

                PrcsParms[] prcsParms = new PrcsParms[1];
                prcsParms[0] = new PrcsParms()
                {
                    SrcID = ""
                };

                ZeoCustomerResp response = new ZeoCustomerResp();

                ZeoCustomerService_MsgSetPortTypeClient client = new ZeoCustomerService_MsgSetPortTypeClient("ZeoCustomer_Port", credential.ServiceUrl);
                client.ClientCredentials.ClientCertificate.Certificate = (RCIFCertificate == null) ? GetRCIFCredentialCertificate(credential) : RCIFCertificate;

                //TODO: AO - Do logging here, trace the request and response.

                NLoggerCommon logwriter = new NLoggerCommon();
                logwriter.Info("From AO - New Log Frame Work - Connecting to TCF Web Services...");

                response = client.FindCustomer(request);

                CustInfo customerInfoList = new CustInfo();

                if (response != null)
                {
                    svcResponse[0].MsgData1 = response.Svc1 != null ? response.Svc1[0].MsgData1 : null;

                    if (!string.IsNullOrEmpty(response.ErrCde) && response.ErrCde != "0")
                    {
                        //TODO:AO - Revisit this whole section of error handling.. 
                        string errorMessage = response.ErrMsg != null ? response.ErrMsg : "";

                        if (response.ErrCde == "2")//AL-4348
                        {
                            throw new CustomerProviderException(response.ErrCde, errorMessage, null);
                        }
                        else
                        {
                            throw new CustomerProviderException(response.ErrCde, errorMessage, null);
                        }
                    }
                }

                if (!svcResponse[0].MsgData1.IsNullorEmpty())
                {
                    customers = RCIFMapper.Map(svcResponse[0].MsgData1.ToList());
                }
                else
                {
                    throw new CustomerException(CustomerException.SEARCH_CUSTOMER_FAILED);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleCustomerException(ex);
                throw new CustomerException(CustomerException.SEARCH_CUSTOMER_FAILED, ex);
            }
            return customers;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string CreateCustomer(CustomerProfile customer, RCIFCredential credential, ZeoContext context)
        {
            ZeoCustomerRegReq request = new ZeoCustomerRegReq();
            string clintCustId = string.Empty;

            Svc2[] baserequest = new Svc2[1];
            baserequest[0] = new Svc2();

            Security[] security = new Security[1];

            security[0] = new Security();

            security[0].BasicAuth = GetBasicAuth(context.SSOAttributes);

            baserequest[0].Security = security;

            baserequest[0].SvcParms = GetsvcParams();

            CustInfo[] custInfo = new CustInfo[1];

            custInfo[0] = new CustInfo()
            {
                PersonalInfo = mapper.Map<PersonalInfo>(customer),
                Identification = mapper.Map<Identification>(customer),
                EmploymentInfo = mapper.Map<EmploymentInfo>(customer)
            };

            custInfo[0].PersonalInfo.NexxoPan = customer.CustomerId.ToString();
            custInfo[0].PersonalInfo.tcfPAN = Helper.GetDictionaryValueIfExists(context.Context, "StatusCode");

            baserequest[0].MsgData = custInfo;

            request.Svc = baserequest[0];

            string ErrMsg = string.Empty;
            string mtvnSvcVer = string.Empty;
            string msgUUID = string.Empty;

            ZeoCustomerResp response = new ZeoCustomerResp();

            try
            {
                ZeoCustomerService_MsgSetPortTypeClient client = new ZeoCustomerService_MsgSetPortTypeClient("ZeoCustomer_Port", credential.ServiceUrl);
                client.ClientCredentials.ClientCertificate.Certificate = (RCIFCertificate == null) ? GetRCIFCredentialCertificate(credential) : RCIFCertificate;

                response = client.CustomerRegistration(request);

                if (response == null)
                {
                    throw new CustomerProviderException(CustomerProviderException.PROVIDER_ERROR, string.Empty, null);
                }

                //string errorMessage = !string.IsNullOrWhiteSpace(response.ErrMsg) ? response.ErrMsg : string.Empty;
                //string errCode = validateCustomerException(errorMessage);
                //errCode = string.IsNullOrEmpty(errCode) ? response.ErrCde : errCode;

                string errorMessage = response.ErrMsg;
                string errCode = response.ErrCde;
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errCode = validateCustomerException(errorMessage, errCode);
                }

                // Is this not null check really required? The null check has been done previously, if the code
                // if the code control came to this stage means that the null check got passed.
                //if (response != null && response.ErrCde == SoftStop)
                if (response.ErrCde == SoftStop)
                {
                    if (!string.IsNullOrWhiteSpace(response.Status))
                    {
                        throw new CustomerProviderException(string.Concat(errCode, ".", response.Status), errorMessage, null);
                    }
                    throw new CustomerProviderException(errCode, errorMessage, null);
                }
                //same as above
                else if (response.ErrCde == HardStop)
                //else if (response != null && response.ErrCde == HardStop)
                {
                    throw new CustomerProviderException(errCode, errorMessage, null);
                }

                if (response.Svc1 != null && response.Svc1[0] != null && response.Svc1[0].MsgData1 != null && response.Svc1[0].MsgData1[0] != null)
                {
                    clintCustId = response.Svc1[0].MsgData1[0].PersonalInfo != null ? response.Svc1[0].MsgData1[0].PersonalInfo.ClientCustId : string.Empty;
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleCustomerException(ex);
                throw new CustomerException(CustomerException.CREATE_CUSTOMER_FAILED, ex);
            }
            return clintCustId;
        }
        #endregion

        #region Pre/PostFlush

        public bool PreFlush(CustomerTransactionDetails cart, RCIFCredential credential, ZeoContext context)
        {
            string ErrMsg = string.Empty;
            string mtvnSvcVer = string.Empty;
            string msgUUID = string.Empty;

            ZeoCustomerPreFlushRequest request = new ZeoCustomerPreFlushRequest()
            {
                PreFlushSummary = GetPreFlushSummary(cart),
                MsgUUID = msgUUID,
                MtvnSvcVer = mtvnSvcVer,

                Svc = new Svc[1]
                {
                    new Svc()
                    {
                        MsgData = new MsgData()
                        {
                            CustInfo = GetCustInfo(cart)
                        },
                        Security = new Security(),
                        SvcParms = GetsvcParams()
                    }
                },
                PrcsParms = new PrcsParms[1]
                {
                    new PrcsParms()
                    {
                        SrcID = ""
                    }
                }
            };

            request.Svc[0].Security.BasicAuth = GetBasicAuth(context.SSOAttributes);
            ZeoCustomerService_MsgSetPortTypeClient client = new ZeoCustomerService_MsgSetPortTypeClient("ZeoCustomer_Port", credential.ServiceUrl);
            client.ClientCredentials.ClientCertificate.Certificate = (RCIFCertificate == null) ? GetRCIFCredentialCertificate(credential) : RCIFCertificate;

            ZeoCustomerPreFlushResponse response = client.CustomerPreFlush(request);

            if (response == null)
            {
                throw new PartnerException(PartnerException.TCIS_PREFLUSH_NORESPONSE, string.Empty, null);
            }
            else if (response != null && response.ErrCde != "0")
            {
                throw new PartnerException(response.ErrCde, response.ErrMsg, null);
            }
            return true;
        }

        public void PostFlush(CustomerTransactionDetails cart, RCIFCredential credential, ZeoContext context)
        {
            int i = 0;
            StringBuilder errMsgs = new StringBuilder();
            string noResponseMessage = "Time out Error";
            string ErrMsg = string.Empty;
            string mtvnSvcVer = string.Empty;
            string msgUUID = string.Empty;

            PreFlushSummary[] preflushSummary = new PreFlushSummary[1];

            preflushSummary[0] = new PreFlushSummary();

            List<ZeoCustomerPostFlushResponse> responses = new List<ZeoCustomerPostFlushResponse>();

            Task<ZeoCustomerPostFlushResponse>[] tasks = new Task<ZeoCustomerPostFlushResponse>[cart.Transactions.Count];

            foreach (var transaction in cart.Transactions)
            {
                ZeoCustomerPostFlushRequest request = new ZeoCustomerPostFlushRequest()
                {
                    MsgUUID = msgUUID,
                    MtvnSvcVer = mtvnSvcVer,
                    PrcsParms = new PrcsParms[1]
                        {
                            new PrcsParms()
                            {
                                SrcID = ""
                            }
                        },
                    PreFlushSummary = new PreFlushSummary[1]
                    {
                        new PreFlushSummary()
                        {
                            SessionId = Convert.ToString(cart.Customer.CustomerSessionId),
                            transaction = new PreFlushSummary_Transaction[1]
                            {
                                GetPostFlushSummary(transaction)
                            },
                        }
                    },
                    Svc = new Svc[1]
                    {
                        new Svc()
                        {
                            MsgData = new MsgData()
                            {
                                CustInfo = GetCustInfo(cart)
                            },
                            Security = new Security(),
                            SvcParms = GetsvcParams()
                        }
                    }
                };
                request.Svc[0].Security.BasicAuth = GetBasicAuth(context.SSOAttributes);
                try
                {
                    ZeoCustomerService_MsgSetPortTypeClient client = new ZeoCustomerService_MsgSetPortTypeClient("ZeoCustomer_Port", credential.ServiceUrl);
                    client.ClientCredentials.ClientCertificate.Certificate = (RCIFCertificate == null) ? GetRCIFCredentialCertificate(credential) : RCIFCertificate;
                    tasks[i] = Task.Factory.StartNew(() => client.CustomerPostFlush(request));
                }
                catch (Exception ex)
                {
                    throw new PartnerException(PartnerException.PROVIDER_ERROR, noResponseMessage, ex);
                }

                i++;
            }

            Task.WaitAll(tasks);

            //Get the response of CashIn transaction from the response.
            // When Transaction Type is inserted in the response.
            //Starts Here
            Task<ZeoCustomerPostFlushResponse> cashInTask = tasks.Where(t => t.Result != null && t.Result.TranType == Convert.ToString(CashType.CashIn)).FirstOrDefault();

            if (cashInTask != null)
            {
                ZeoCustomerPostFlushResponse response = cashInTask.Result;

                if (response.ErrCde != "0")
                    throw new PartnerException(response.ErrCde, response.ErrMsg, null);
            }

            //Ends Here
        }

        #endregion

        #region Private Methods

        #region Customer
        private BasicAuth GetBasicAuth(Dictionary<string, object> ssoAttributes)
        {
            if (ssoAttributes.IsNullorEmpty())
            {
                throw new CustomerException(CustomerException.SSO_ATTRIBUTES_EMPTY);
            }

            BasicAuth basicAuth = new BasicAuth()
            {
                UsrID = ssoAttributes.GetStringValue("UserID"),
                tellerNbr = ssoAttributes.GetIntValue("TellerNum"),
                tellerNbrSpecified = true,
                branchNbr = ssoAttributes.GetIntValue("BranchNum"),
                branchNbrSpecified = true,
                bankNbr = ssoAttributes.GetIntValue("BankNum"),
                bankNbrSpecified = true,
                lawsonID = ssoAttributes.GetStringValue("LawsonID"),
                lu = ssoAttributes.GetStringValue("LU"),
                cashDrawer = ssoAttributes.GetStringValue("CashDrawer"),
                amPmInd = ssoAttributes.GetStringValue("AmPmInd"),
                BusinessDate = ssoAttributes.GetStringValue("BusinessDate"),
                HostName = ssoAttributes.GetStringValue("MachineName"),
                subClientNodeID = ssoAttributes.GetStringValue("DPSBranchID")
            };

            return basicAuth;
        }

        private SvcParms[] GetsvcParams()
        {
            SvcParms[] svcParams = new SvcParms[1];
            svcParams[0] = new SvcParms()
            {
                ApplID = "",
                RqstUUID = "",
                SvcID = "",
                SvcNme = "",
                SvcVer = ""
            };

            return svcParams;
        }

        private string MapToClientIdType(string AlloyIdType)
        {
            string idType = string.Empty;

            Dictionary<string, string> idMappying = new Dictionary<string, string>()
            {
                {"DRIVER'S LICENSE"                     ,"D"        },
                {"MILITARY ID"                          ,"U"        },
                {"PASSPORT"                             ,"P"        },
                {"U.S. STATE IDENTITY CARD"             ,"S"        },
                {"MATRICULA CONSULAR"                   ,"M"        }
            };

            if (idMappying.ContainsKey(AlloyIdType))
            {
                idType = idMappying[AlloyIdType];
            }
            return idType;
        }

        private string MapToClientPhoneTypes(string mapPhoneType)
        {
            string idType = string.Empty;
            Dictionary<string, string> phoneTypes = new Dictionary<string, string>()
            {
                {"Home", "H"},
                {"Work", "W"},
                {"Cell", "M"},
                {"Other", "O"},
            };

            if (phoneTypes.ContainsKey(mapPhoneType))
            {
                idType = phoneTypes[mapPhoneType];
            }
            return idType;
        }

        private void HandleCustomerException(Exception ex)
        {
            Exception tcisProviderException = ex as CustomerProviderException;
            if (tcisProviderException != null)
            {
                CustomerProviderException tcisException = tcisProviderException as CustomerProviderException;
                string errorCode = Convert.ToString(tcisException.ProviderErrorCode);
                string errorMsg = Convert.ToString(tcisException.Message);

                throw new CustomerProviderException(errorCode, errorMsg, null);
            }
            Exception faultException = ex as FaultException;
            if (faultException != null)
            {
                throw new CustomerProviderException(ProviderException.PROVIDER_FAULT_ERROR, string.Empty, faultException);
            }
            Exception endpointException = ex as EndpointNotFoundException;
            if (endpointException != null)
            {
                throw new CustomerProviderException(ProviderException.PROVIDER_ENDPOINTNOTFOUND_ERROR, string.Empty, endpointException);
            }
            Exception commException = ex as CommunicationException;
            if (commException != null)
            {
                throw new CustomerProviderException(ProviderException.PROVIDER_COMMUNICATION_ERROR, string.Empty, commException);
            }
            Exception timeOutException = ex as TimeoutException;
            if (timeOutException != null)
            {
                throw new CustomerProviderException(ProviderException.PROVIDER_TIMEOUT_ERROR, string.Empty, timeOutException);
            }
        }

        private string validateCustomerException(string errorMessage, string errorCode)
        {
            string errCode = string.Empty;
            if (errorMessages.Any())
            {
                errCode = errorMessages.Where(x => x.Key == errorMessage).Select(x => x.Value).FirstOrDefault();
                errorCode = errCode ?? errorCode;
            }
            return errorCode;
        }
        #endregion

        #region Pre/PostFlush

        private PreFlushSummary[] GetPreFlushSummary(CustomerTransactionDetails cart)
        {
            PreFlushSummary_Transaction transactionSummary = new PreFlushSummary_Transaction();
            PreFlushSummary[] preflushSummary = new PreFlushSummary[1];

            preflushSummary[0] = new PreFlushSummary();

            PreFlushSummary_Transaction[] pfSummartTransaction = new PreFlushSummary_Transaction[cart.Transactions.Count];

            preflushSummary[0].SessionId = Convert.ToString(cart.Customer.CustomerSessionId);
            preflushSummary[0].transaction = pfSummartTransaction;

            DateTime dateOfBirth = Convert.ToDateTime(cart.Customer.DateOfBirth);

            for (var i = 0; i <= cart.Transactions.Count - 1; i++)
            {
                string type = string.Empty;

                string transType = cart.Transactions[i].Type;

                if (transType == Convert.ToString(TransactionType.Cash))
                {
                    type = cart.Transactions[i].CashType != null ? cart.Transactions[i].CashType : cart.Transactions[i].Type;
                    transactionSummary = PopulateCash(cart.Transactions[i], type);
                }
                else if (transType == Convert.ToString(TransactionType.MoneyTransfer))
                {
                    type = cart.Transactions[i].TransferType != null ? cart.Transactions[i].TransferType : cart.Transactions[i].Type;
                    transactionSummary = PopulateMoneyTransfer(cart.Transactions[i], type);
                }
                else if (transType == Convert.ToString(TransactionType.BillPayment))
                {
                    transactionSummary = PopulateBillPay(cart.Transactions[i]);
                }
                else if (transType == Convert.ToString(TransactionType.MoneyOrder))
                {
                    transactionSummary = PopulateMoneyOrder(cart.Transactions[i]);
                }
                else if (transType == Convert.ToString(TransactionType.ProcessCheck))
                {
                    transactionSummary = PopulateCheck(cart.Transactions[i]);
                }
                else if (transType == Convert.ToString(TransactionType.Fund))
                {
                    type = cart.Transactions[i].TransferType != null ? cart.Transactions[i].TransferType : cart.Transactions[i].Type;
                    transactionSummary = PopulateFunds(cart.Transactions[i], type);
                }
                preflushSummary[0].transaction[i] = transactionSummary;

            }

            return preflushSummary;
        }


        /// <summary>
		/// Author : Kaushik
		/// Description : Populate MoneyTransfer related transactions.
		/// </summary>
		/// <param name="tran"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction PopulateMoneyTransfer(Transaction tran, string type)
        {
            PreFlushSummary_Transaction transaction = new PreFlushSummary_Transaction();

            if (string.Compare(type, MoneyTransferType.Send.ToString(), true) == 0)
            {
                transaction = PopulateSendMoney(tran, type);
            }
            else if (string.Compare(type, MoneyTransferType.Receive.ToString(), true) == 0)
            {
                transaction = PopulateReceiveMoney(tran, type);
            }

            return transaction;
        }

        /// <summary>
        /// Author : Kaushik
        /// Description : Populate Billpay related transactions.
        /// </summary>
        /// <param name="billPayTran"></param>
        /// <returns></returns>
        private PreFlushSummary_Transaction PopulateBillPay(Transaction billPayTran)
        {
            PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
            {
                tranID = billPayTran.ID,
                tranType = "BillPay",
                acctNbr = billPayTran.AccountNumber,
                payee = billPayTran.Payee,
                MTCN = billPayTran.MTCN,
                amount = RoundOffDecimal(billPayTran.Amount, 2),
                fee = RoundOffDecimal(billPayTran.Fee, 2),
                total = RoundOffDecimal(billPayTran.GrossTotalAmount, 2)
            };

            return tran;
        }

        /// <summary>
        ///  Author : Kaushik
        ///  Description : Populate MoneyOrder related transactions.
        /// </summary>
        /// <param name="moTran"></param>
        /// <returns></returns>
        private PreFlushSummary_Transaction PopulateMoneyOrder(Transaction moTran)
        {
            PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
            {
                tranID = moTran.ID,
                tranType = TransactionType.MoneyOrder.ToString(),
                CheckNbr = moTran.CheckNumber,
                amount = RoundOffDecimal(moTran.Amount, 2),
                fee = RoundOffDecimal(moTran.Fee, 2),
                total = RoundOffDecimal(moTran.GrossTotalAmount, 2),
            };

            return tran;
        }

        /// <summary>
        /// Author : Kaushik
        /// Description : Populate Check Processing related transactions.
        /// </summary>
        /// <param name="cpTran"></param>
        /// <returns></returns>
        private PreFlushSummary_Transaction PopulateCheck(Transaction cpTran)
        {
            PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
            {
                tranID = cpTran.ID.ToString(),
                tranType = CheckProcessing,
                CheckNbr = cpTran.CheckNumber,
                CheckType = cpTran.CheckType,
                ConfirmationNbr = cpTran.ConfirmationNumber,
                amount = RoundOffDecimal(cpTran.Amount, 2),
                fee = RoundOffDecimal(cpTran.Fee, 2),
                total = RoundOffDecimal(cpTran.GrossTotalAmount, 2)
            };

            return tran;
        }

        /// <summary>
        /// Author : Kaushik
        /// Description : Populate Fund related transactions.
        /// </summary>
        /// <param name="feTran"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private PreFlushSummary_Transaction PopulateFunds(Transaction feTran, string type)
        {
            PreFlushSummary_Transaction transaction = new PreFlushSummary_Transaction();

            if (string.Compare(type, FundType.Credit.ToString(), true) == 0)
            {
                transaction = PopulatePrepaidLoad(feTran, type);
            }
            else if (string.Compare(type, FundType.Debit.ToString(), true) == 0)
            {
                transaction = PopulatePrePaidWithdraw(feTran, type);
            }

            return transaction;
        }

        /// <summary>
        /// Author : Kaushik
        /// Description : Populate Fund- Prepaid Load related transactions.
        /// </summary>
        /// <param name="feTran"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private PreFlushSummary_Transaction PopulatePrepaidLoad(Transaction feTran, string type)
        {
            PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
            {
                tranID = Convert.ToString(feTran.ID),
                tranType = "PrePaidLoad",
                InitialPurchase = feTran.InitialPurchase,
                purchasefee = RoundOffDecimal(feTran.PurchaseFee, 2),
                NewCardBalance = RoundOffDecimal(feTran.NewCardBalance, 2),
                CardNbr = feTran.TransferSubType == (int)Helper.FundType.Activation ? feTran.CardNumber : feTran.CardNumber.Substring(feTran.CardNumber.Length - 6),
                AliasId = feTran.AliasId,
                ConfirmationNbr = feTran.ConfirmationNumber,
                LoadAmount = RoundOffDecimal(feTran.Amount, 2),
                fee = RoundOffDecimal(feTran.Fee, 2),
                total = RoundOffDecimal(feTran.GrossTotalAmount, 2)
            };

            return tran;
        }

        /// <summary>
        /// Author : Kaushik
        /// Description : Populate Fund- Prepaid Withdraw related transactions.
        /// </summary>
        /// <param name="feTran"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private PreFlushSummary_Transaction PopulatePrePaidWithdraw(Transaction feTran, string type)
        {
            PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
            {
                tranID = Convert.ToString(feTran.ID),
                tranType = "PrePaidWithdraw",
                NewCardBalance = RoundOffDecimal(feTran.NewCardBalance, 2),
                CardNbr = feTran.CardNumber.Substring(feTran.CardNumber.Length - 6),
                AliasId = feTran.AliasId,
                ConfirmationNbr = feTran.ConfirmationNumber,
                WithdrawAmount = RoundOffDecimal(feTran.Amount, 2),
                fee = RoundOffDecimal(feTran.Fee, 2),
                total = RoundOffDecimal(feTran.GrossTotalAmount, 2)
            };

            return tran;
        }

        /// <summary>
        /// Author : Kaushik
        /// Description : Populate Fund- Prepaid Activate related transactions.
        /// </summary>
        /// <param name="feTran"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private PreFlushSummary_Transaction PopulatePrepaidActivate(Transaction feTran, string type)
        {
            PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction()
            {
                tranID = Convert.ToString(feTran.ID),
                tranType = type,
                InitialPurchase = feTran.InitialPurchase,
                purchasefee = RoundOffDecimal(feTran.PurchaseFee, 2),
                NewCardBalance = RoundOffDecimal(feTran.NewCardBalance, 2),
                CardNbr = feTran.CardNumber,
                AliasId = feTran.AliasId,
                ConfirmationNbr = feTran.ConfirmationNumber,
                LoadAmount = RoundOffDecimal(feTran.LoadAmount, 2),
                fee = RoundOffDecimal(feTran.Fee, 2),
                total = RoundOffDecimal(feTran.GrossTotalAmount, 2)
            };

            return tran;
        }

        /// <summary>
        /// Author : Kaushik
        /// Description : Populate Cash related transactions.
        /// </summary>
        /// <param name="cashTran"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private PreFlushSummary_Transaction PopulateCash(Transaction cashTran, string type)
        {
            PreFlushSummary_Transaction tran = new PreFlushSummary_Transaction();

            if (cashTran != null)
            {
                tran.tranID = Convert.ToString(cashTran.ID);
                tran.tranType = type;
                tran.acctNbr = cashTran.AccountNumber;
                tran.amount = RoundOffDecimal(cashTran.Amount, 2);
                tran.total = RoundOffDecimal(cashTran.GrossTotalAmount, 2);
            }
            return tran;
        }

        /// <summary>
        /// Author : Kaushik
        /// Description : Populate MoneyTransfer- SendMoney related transactions.
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private PreFlushSummary_Transaction PopulateSendMoney(Transaction tran, string type)
        {
            PreFlushSummary_Transaction transactionSummary = new PreFlushSummary_Transaction()
            {
                tranID = tran.ID,
                tranType = "SendMoney",
                MTCN = tran.MTCN,
                amount = RoundOffDecimal(tran.Amount, 2),
                fee = RoundOffDecimal(tran.Fee, 2),
                total = RoundOffDecimal(tran.GrossTotalAmount, 2),
                ToFirstName = tran.ToFirstName,
                ToMiddleName = tran.ToMiddleName,
                ToLastName = tran.ToLastName,
                ToSecondLastName = tran.ToSecondLastName,
                ToGender = tran.ToGender != null ? tran.ToGender.Substring(0, 1) : null,
                ToCountry = tran.ToCountry,
                ToAddress = tran.ToAddress,
                ToCity = tran.ToCity,
                ToState_Province = tran.ToState_Province,
                ToZipCode = tran.ToZipCode,
                ToPhoneNumber = tran.ToPhoneNumber,
                ToPickupCountry = tran.ToPickUpCountry,
                ToPickupState_Province = tran.ToPickUpState_Province,
                ToPickupCity = tran.ToPickUpCity,
                ToDeliveryMethod = tran.ToDeliveryMethod,
                ToDeliveryOption = tran.ToDeliveryOption,
                ToOccupation = tran.ToOccupation,
                ToDOB = Convert.ToString(tran.ToDOB),
                ToCountryOfBirth = tran.ToCountryOfBirth
            };

            return transactionSummary;
        }

        /// <summary>
        /// Author : Kaushik
        /// Description : Populate MoneyTransfer- ReceiveMoney related transactions.
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private PreFlushSummary_Transaction PopulateReceiveMoney(Transaction tran, string type)
        {
            PreFlushSummary_Transaction transactionSummary = new PreFlushSummary_Transaction()
            {
                tranID = tran.ID,
                tranType = "ReceiveMoney",
                MTCN = tran.MTCN,
                FirstName = tran.ToFirstName, // Receiver's FirstName
                ToMiddleName = tran.ToMiddleName, // Receiver's MiddleName
                LastName = tran.ToLastName, // Receiver's LastName
                SecondLastName = tran.ToSecondLastName, // Receiver's Second LastName
                PickupCountry = tran.ToPickUpCountry,
                amount = RoundOffDecimal(tran.Amount, 2),
                fee = RoundOffDecimal(tran.Fee, 2),
                total = RoundOffDecimal(tran.GrossTotalAmount, 2)
            };

            return transactionSummary;
        }

        private CustInfo[] GetCustInfo(CustomerTransactionDetails cart)
        {
            PersonalInfo personalInfo = new PersonalInfo()
            {
                FName = cart.Customer.FirstName,
                MName = cart.Customer.MiddleName,
                LName = string.IsNullOrWhiteSpace(cart.Customer.SecondLastName) ? cart.Customer.LastName : string.Format("{0} {1}", cart.Customer.LastName, cart.Customer.SecondLastName),
                Addr1 = cart.Customer.Address1,
                Addr2 = cart.Customer.Address2,
                City = cart.Customer.City,
                State = cart.Customer.State,
                zip = cart.Customer.Zip,
                Ph1 = cart.Customer.Phone1,
                Ph1Type1 = cart.Customer.Ph1Type1 != null ? MapToClientPhoneTypes(cart.Customer.Ph1Type1) : null,
                Ph2 = cart.Customer.Phone2,
                Ph2Type2 = cart.Customer.Ph2Type2 != null ? MapToClientPhoneTypes(cart.Customer.Ph2Type2) : null,
                Ph2Prov = cart.Customer.Ph2Prov,
                email = cart.Customer.Email,
                ssn = cart.Customer.SSN,
                TaxCd = !string.IsNullOrWhiteSpace(cart.Customer.SSN) && (cart.Customer.SSN.Substring(0, 1) == "9") ? TaxCdITIN : !string.IsNullOrWhiteSpace(cart.Customer.SSN) ? TaxCdSSN : null,
                NexxoPan = Convert.ToString(cart.Customer.AlloyID),
                ClientCustId = !string.IsNullOrWhiteSpace(cart.Customer.ClientCustId) ? cart.Customer.ClientCustId : "0",
                gender = cart.Customer.Gender != null ? cart.Customer.Gender.Substring(0, 1) : null,
                tcfCustind = cart.Customer.CustInd ? "Y" : "N",
                PRODCODE = PRODCODE,
                APPL_CD = APPL_CD
            };

            DateTime dateOfBirth = Convert.ToDateTime(cart.Customer.DateOfBirth);

            Identification identification = new Identification()
            {
                dob = dateOfBirth != null ? dateOfBirth.ToString("yyyyMMdd") : null,
                maiden = cart.Customer != null ? cart.Customer.Maiden : string.Empty,
                idType = cart.Customer != null ? Convert.ToString(MapToClientIdType(cart.Customer.IdType)) : string.Empty,
                idIssuer = cart.Customer != null ? cart.Customer.IdIssuer : string.Empty,
                idIssuerCountry = cart.Customer != null ? cart.Customer.IdIssuerCountry : string.Empty,
                idNbr = cart.Customer != null ? cart.Customer.Identification : string.Empty,
                idIssueDate = cart.Customer != null && cart.Customer.IssueDate != null && cart.Customer.IssueDate != DateTime.MinValue ? cart.Customer.IssueDate.Value.ToString("yyyyMMdd") : null,
                idExpDate = cart.Customer != null && cart.Customer.ExpirationDate != null ? cart.Customer.ExpirationDate.Value.ToString("yyyyMMdd") : null,
                legalCode = cart.Customer != null ? cart.Customer.LegalCode : string.Empty,
                citizenshipCountry1 = cart.Customer != null ? cart.Customer.PrimaryCountryCitizenship : string.Empty,
                citizenshipCountry2 = cart.Customer != null ? cart.Customer.SecondaryCountryCitizenship : string.Empty,
            };

            EmploymentInfo employmentInfo = new EmploymentInfo()
            {
                Occupation = cart.Customer != null ? cart.Customer.Occupation : string.Empty,
                OccDesc = cart.Customer != null ? cart.Customer.OccupationDescription : string.Empty,
                EmployerName = cart.Customer != null ? cart.Customer.EmployerName : string.Empty,
                EmployerPhoneNum = cart.Customer != null ? cart.Customer.EmployerPhoneNum : string.Empty
            };

            CustInfo[] custInfo = new CustInfo[1];

            custInfo[0] = new CustInfo()
            {
                PersonalInfo = personalInfo,
                Identification = identification,
                EmploymentInfo = employmentInfo
            };

            return custInfo;
        }

        /// <summary>
		/// Author : Kaushik
		/// Description : Post Flush Related call
		/// Caller : Biz.Partner - ShoppingCartImpl
		/// Update : Added a new logic of passing one by one transaction in PostFlush Request.
		/// </summary>
		/// <param name="tran"></param>
		/// <returns></returns>
		private PreFlushSummary_Transaction GetPostFlushSummary(Transaction tran)
        {
            PreFlushSummary_Transaction transactionSummary = new PreFlushSummary_Transaction();

            string type = string.Empty;

            string transType = tran.Type;

            if (transType == Convert.ToString(TransactionType.Cash))
            {
                type = tran.CashType != null ? tran.CashType : tran.Type;
                transactionSummary = PopulateCash(tran, type);
            }
            else if (transType == Convert.ToString(TransactionType.MoneyTransfer))
            {
                type = tran.TransferType != null ? tran.TransferType : tran.Type;
                transactionSummary = PopulateMoneyTransfer(tran, type);
            }
            else if (transType == Convert.ToString(TransactionType.BillPayment))
            {
                transactionSummary = PopulateBillPay(tran);
            }
            else if (transType == Convert.ToString(TransactionType.MoneyOrder))
            {
                transactionSummary = PopulateMoneyOrder(tran);
            }
            else if (transType == Convert.ToString(TransactionType.ProcessCheck))
            {
                transactionSummary = PopulateCheck(tran);
            }
            else if (transType == Convert.ToString(TransactionType.Fund))
            {
                type = tran.TransferType != null ? tran.TransferType : tran.Type;
                transactionSummary = PopulateFunds(tran, type);
            }

            return transactionSummary;
        }
        #endregion

        #region Certificate

        private X509Certificate2 GetRCIFCredentialCertificate(RCIFCredential rcifCredentials)
        {
            X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            // Open the store.
            certificateStore.Open(OpenFlags.ReadWrite | OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            // Find the certificate with the specified subject.
            X509Certificate2Collection certificates = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, rcifCredentials.ThumbPrint, false);
            certificateStore.Close();

            if (certificates.Count < 1)
            {
                throw new CustomerException(CustomerException.CERIFICATE_NOTFOUND, null);
            }

            return certificates[0];
        }

        /// <summary>
        /// Sets the cert policy.
        /// </summary>
        private static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
        }

        /// <summary>
        /// Certificate validation callback 
        /// </summary>
        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            if (error == SslPolicyErrors.None)
            {
                return true;   // already determined to be valid
            }
            switch (cert.GetCertHashString())
            {
                // Thumbprints of the IIB Server Certificates. Once the server cert issue is fixed at IIB - we should remove this code.
                case "421D0E3675BE934F20AC4B655F06EFE1C8A4BF41": // This is for Non Prod
                case "0748182c22543da2d29edeb06ebddb321fa00b0e": // This is for Prod
                    return true;
                default:
                    return false;
            }
        }


        #endregion

        #endregion
    }
}
