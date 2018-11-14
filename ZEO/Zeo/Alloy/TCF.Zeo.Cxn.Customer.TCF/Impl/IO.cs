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
using TCF.Zeo.Cxn.Customer.TCF.TellerWorkQueueService;
using TellerWorkQueueService = TCF.Zeo.Cxn.Customer.TCF.TellerWorkQueueService;
using System.Reflection;
using TCF.Zeo.Cxn.Common;
using TCF.Zeo.Cxn.Customer.TCF.EWSScanning;
#endregion

namespace TCF.Zeo.Cxn.Customer.TCF.Impl
{
    public class IO : IIO
    {
        IMapper mapper;
        static Dictionary<string, string> errorMessages = new Dictionary<string, string>();
        internal X509Certificate2 RCIFCertificate = null;
        static int RCIFCOMMITENV = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RCIFCommitCallEnvironment"]);
        NLoggerCommon logwriter = new NLoggerCommon();

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
        private readonly bool IsHardcodedSSOAttr = Convert.ToBoolean(ConfigurationManager.AppSettings["IsHardCodeSSOAttributes"].ToString());
        private readonly string SSOAttributes = ConfigurationManager.AppSettings["SSOAttributes"].ToString();

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
                    BasicAuth = (BasicAuth)GetBasicAuth(context)
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

            security[0].BasicAuth = (BasicAuth)GetBasicAuth(context);

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
                            CustInfo = (CustInfo[])GetCustInfo(cart)
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

            request.Svc[0].Security.BasicAuth = (BasicAuth)GetBasicAuth(context);
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
                                CustInfo = (CustInfo[])GetCustInfo(cart)
                            },
                            Security = new Security(),
                            SvcParms = GetsvcParams()
                        }
                    }
                };
                request.Svc[0].Security.BasicAuth = (BasicAuth)GetBasicAuth(context);
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

        #region Final Commits

        public bool TellerMainFrameCommit(Transaction transaction, RCIFCredential credential, ZeoContext context)
        {
            TEL7770OperationRequest TEL7770Operation = new TEL7770OperationRequest();
            TellerMainFrameResponse response = new TellerMainFrameResponse();
            bool isCallFailed = false;

            try
            {
                //This flag is maintained for one time retry if there is any exception in Teller MainFrame. 
                int mainFrameRetryCount = 0;

                TEL7770Operation.TelnexxcTranInfo = GetTelNexxcTranInfoRequest(transaction, context);
                TellerMainFrameRequest request = new TellerMainFrameRequest();
                request.TEL7770Operation = TEL7770Operation;

                var certificate = (RCIFCertificate == null) ? GetRCIFCredentialCertificate(credential) : RCIFCertificate;

                //SerializeObjectAndLog<TellerMainFrameRequest>(request);
                response = TellerMainFrameCall(credential, ref mainFrameRetryCount, request, certificate);
                //SerializeObjectAndLog<TellerMainFrameResponse>(response);
                if (response?.TEL7770OperationResponse?.TelnexxTranReturn?.Telnexxcmessage?.Trim() != "SUCCESSFUL")
                    isCallFailed = true;
            }
            catch (Exception ex)
            {
                logwriter.Info("Exception occurred in method TellerMainFrameCommit");

                PopulateErrorInfo(ex, string.Empty);

                //Dont throw any error even if it fails.
                //if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                //HandleCheckException(ex);
                //throw new FlushException(FlushException.TELLER_MAINFRAME_COMMIT_FAILED, ex);
            }

            return isCallFailed;
        }

        public Tuple<long, long, long, bool> TellerMiddleTierCommit(CustomerTransactionDetails cart, Transaction transaction, ref string CIF7454TemplateType, RCIFCredential credential, ZeoContext context)
        {
            PostcartResponse response = new PostcartResponse();

            Tuple<long, long, long, bool> riskScoresWithCallSuccess = new Tuple<long, long, long, bool>(0000, 0000, 0000, true);

            try
            {
                //This flag is maintained for one time retry if there is any exception in Teller MiddleTier. 
                int middleTierRetryCount = 0;

                PostcartRequest postCartRequest = new PostcartRequest()
                {
                    SessionId = Convert.ToString(cart.Customer.CustomerSessionId),
                    BasicAuthInformation = (BasicAuthInfo)GetBasicAuth(context, true),
                    CustomerInformation = (TellerWorkQueueService.CustomerInfo)GetCustInfo(cart, true),
                    Transaction = GetZeoTransaction(transaction, context)
                };

                var certificate = (RCIFCertificate == null) ? GetRCIFCredentialCertificate(credential) : RCIFCertificate;
                response = TellerMiddleTierCall(credential, ref middleTierRetryCount, postCartRequest, certificate);
                riskScoresWithCallSuccess = new Tuple<long, long, long, bool>(Convert.ToInt64(response?.CustomerRiskScore), Convert.ToInt64(response?.GPRRiskScore), Convert.ToInt64(response?.ZeoServicesRiskScore), isTellerWorkQueueWSCallFailed(response));
                CIF7454TemplateType = response?.TemplateType;
            }
            catch (Exception ex)
            {
                logwriter.Info("Exception occurred in method TellerMiddleTierCommit");

                PopulateErrorInfo(ex, string.Empty);
                //Dont throw any error even if it fails.
                //if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                //HandleCheckException(ex);
                //throw new FlushException(FlushException.TELLER_MIDDLETIER_COMMIT_FAILED, ex);
            }

            return riskScoresWithCallSuccess;
        }

        public void RCIFFinalCommit(bool isVisaTrx, string CIF7454TemplateType, Tuple<long, long, long> riskScores, CustomerTransactionDetails cart, Transaction transaction, RCIFCredential credential, ZeoContext context)
        {
            RCIFMainFrameRequest request = new RCIFMainFrameRequest();
            RCIFMainFrameResponse response = new RCIFMainFrameResponse();

            try
            {
                //This flag is maintained for one time retry if there is any exception in RCIF. 
                int rcifFinalCommitRetryCount = 0;

                CIF7454Operation CIF7454Operation = new CIF7454Operation();

                CIF7454iRequestData cif7454irequestdata;

                if (isVisaTrx)
                    cif7454irequestdata = GetRequestDataForRCIF_Visa(cart.Customer, CIF7454TemplateType, riskScores, transaction, context);
                else
                    cif7454irequestdata = GetRequestDataForRCIF_Others(cart.Customer, CIF7454TemplateType, riskScores, transaction, context);

                CIF7454Operation.CIF7454irequestdata = cif7454irequestdata;
                //CIF7454Operation.cif7454i_filler = "";

                request.CIF7454Operation = CIF7454Operation;

                var certificate = (RCIFCertificate == null) ? GetRCIFCredentialCertificate(credential) : RCIFCertificate;

               // SerializeObjectAndLog<RCIFMainFrameRequest>(request);
                response = RCIFCall(credential, ref rcifFinalCommitRetryCount, request, certificate);

              //  SerializeObjectAndLog<RCIFMainFrameResponse>(response);
            }
            catch (Exception ex)
            {
                logwriter.Info("Exception occurred in method RCIFFinalCommit");

                PopulateErrorInfo(ex, string.Empty);
                //Dont throw any error even if it fails.
                //if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                //HandleCheckException(ex);
                //throw new FlushException(FlushException.RCIF_COMMIT_FAILED, ex);
            }
        }

        #endregion

        #region Customer Registration Without IIB

        public bool EWSScanningCustomerRegistration(CustomerProfile customer, RCIFCredential credential, ZeoContext context)
        {
            bool result = false;

            cpsCustomerRequestRecord cpsRequest = new cpsCustomerRequestRecord();

            cpsCustomerResponseRecord preScreenResponse = new cpsCustomerResponseRecord();

            try
            {
                cpsRequest = GetRequestDataForRCIFCustomerEWSScanning(customer, context);

                var certificate = (RCIFCertificate == null) ? GetRCIFCredentialCertificate(credential) : RCIFCertificate;

                preScreenResponse = EWSCall(credential, cpsRequest, certificate);

                if (preScreenResponse?.cpsSuccessIndicator == "S")
                { 
                    result = true;
                }
                else if (false) //Checking for the Outage error
                {
                    result = false;
                }
                else
                {
                    throw new CustomerException(CustomerException.CUSTOMER_REGISTRATION_EWS_SCANNING_IS_FAILED, null);
                }                    

                return result;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleCustomerException(ex);
                throw new CustomerException(CustomerException.CUSTOMER_REGISTRATION_EWS_SCANNING_IS_FAILED, ex);
            }
            
        }

        public string RCIFCustomerRegistration(CustomerProfile customer, RCIFCredential credential, ZeoContext context)
        {
            string partnerAccountNumber = string.Empty;
            RCIFCustomerRegRequest request = new RCIFCustomerRegRequest();
            RCIFCustomerRegResponse response = new RCIFCustomerRegResponse();

            try
            {
                CIF7450Operation CIF7450Operation = new CIF7450Operation();
                CIF7450iRequestData cif7450irequestdata = GetRequestDataForRCIFCustomerRegistration(customer, context);
                CIF7450Operation.CIF7450irequestdata = cif7450irequestdata;
                CIF7450Operation.CIF7450iFiller = "";
                request.CIF7450Operation = CIF7450Operation;

                var certificate = (RCIFCertificate == null) ? GetRCIFCredentialCertificate(credential) : RCIFCertificate;

                SerializeObjectAndLog<RCIFCustomerRegRequest>(request);
                response = RCIFCustomerRegistrationCall(credential, request, certificate);
                SerializeObjectAndLog<RCIFCustomerRegResponse>(response);

                //If the response from RCIF is null throw an exception.
                if (response == null || response.CIF7450OperationResponse == null)
                {
                    throw new CustomerProviderException(CustomerProviderException.PROVIDER_ERROR, string.Empty, null);
                }

                //If Checking for error codes in "cif7450r_return_messages" or "cif7450r_return_codes".
                if (!CheckPropertyValues(response.CIF7450OperationResponse.CIF7450rreturnsvcmsgs) || !CheckPropertyValues(response.CIF7450OperationResponse.CIF7450rreturncodes))
                {
                    string errorMessage = response.CIF7450OperationResponse.CIF7450rreturnsvcmsgs?.CIF7450rrtrnmsgtext1;

                    //If there are any errors in the RCIF response, we need to show that in the pop up or else just show customer cant be created.
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                        throw new CustomerProviderException(CustomerProviderException.RCIF_CUSTOMER_REG_ERROR, errorMessage, null);
                    else
                        throw new Exception();
                }
                   
                partnerAccountNumber = Convert.ToString(response?.CIF7450OperationResponse?.CIF7450rreturndata?.CIF7450rrtrnnewcustno);

                return partnerAccountNumber;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                HandleCustomerException(ex);
                throw new CustomerException(CustomerException.CREATE_CUSTOMER_FAILED, ex);
            }
        }

        #endregion

        #region Private Methods

        #region Customer
        private object GetBasicAuth(ZeoContext context, bool isMiddleTier = false)
        {
            if (context.SSOAttributes.IsNullorEmpty())
            {
                throw new CustomerException(CustomerException.SSO_ATTRIBUTES_EMPTY);
            }

            if (!isMiddleTier)
            {
                BasicAuth basicAuth = new BasicAuth();

                Dictionary<string, object> ssoAttributes = GetSSOAttributes(context);

                basicAuth.UsrID = ssoAttributes.GetStringValue("UserID");
                basicAuth.tellerNbr = ssoAttributes.GetIntValue("TellerNum");
                basicAuth.tellerNbrSpecified = true;
                basicAuth.branchNbr = ssoAttributes.GetIntValue("BranchNum");
                basicAuth.branchNbrSpecified = true;
                basicAuth.bankNbr = ssoAttributes.GetIntValue("BankNum");
                basicAuth.bankNbrSpecified = true;
                basicAuth.lawsonID = ssoAttributes.GetStringValue("LawsonID");
                basicAuth.lu = ssoAttributes.GetStringValue("LU");
                basicAuth.cashDrawer = ssoAttributes.GetStringValue("CashDrawer");
                basicAuth.amPmInd = ssoAttributes.GetStringValue("AmPmInd");
                basicAuth.BusinessDate = ssoAttributes.GetStringValue("BusinessDate");
                basicAuth.HostName = ssoAttributes.GetStringValue("MachineName");
                basicAuth.subClientNodeID = ssoAttributes.GetStringValue("DPSBranchID");

                return basicAuth;
            }
            else
            {
                 BasicAuthInfo basicAuth = new BasicAuthInfo();
                 
                 Dictionary<string, object> ssoAttributes = GetSSOAttributes(context);
                 
                 basicAuth.UserId = ssoAttributes.GetStringValue("UserID");
                 basicAuth.TellerNumber = ssoAttributes.GetStringValue("TellerNum");
                 basicAuth.BranchNumber = ssoAttributes.GetStringValue("BranchNum");
                 basicAuth.BankNumber = ssoAttributes.GetStringValue("BankNum");
                 basicAuth.LawsonId = ssoAttributes.GetStringValue("LawsonID");
                 basicAuth.LU = ssoAttributes.GetStringValue("LU");
                 basicAuth.CashDrawer = ssoAttributes.GetStringValue("CashDrawer");
                 basicAuth.AMPMIndicator = ssoAttributes.GetStringValue("AmPmInd");
                 basicAuth.BusinessDate = ssoAttributes.GetStringValue("BusinessDate");
                 basicAuth.HostName = ssoAttributes.GetStringValue("MachineName");
                 basicAuth.SubClientNodeId = ssoAttributes.GetStringValue("DPSBranchID");
               
                 return basicAuth;
            }
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

            Dictionary<string, string> idMappying = CXNHelper.GetGovtIdTypes((int)ProviderId.TCISCustomer);

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

        private TellerMainFrameResponse TellerMainFrameCall(RCIFCredential credential, ref int mainFrameRetryCount, TellerMainFrameRequest request, X509Certificate2 certificate)
        {
            try
            {
                if (mainFrameRetryCount > 1)
                {
                    logwriter.Info("Teller Mainframe Call Failed");
                    return new TellerMainFrameResponse();
                }

                TellerMainFrameResponse response = Zeo.Common.Util.AlloyUtil.RESTPostCall<TellerMainFrameResponse>(credential.TellerInquiryUrl, certificate, request);

                if (response.TEL7770OperationResponse != null && response.TEL7770OperationResponse.TelnexxTranReturn != null &&
                   response.TEL7770OperationResponse.TelnexxTranReturn.Telnexxcreject == "1")
                {
                    throw new Exception();
                }

                return response;
            }
            catch (Exception ex)
            {
                PopulateErrorInfo(ex, credential?.TellerInquiryUrl);

                mainFrameRetryCount = mainFrameRetryCount + 1;
                TellerMainFrameCall(credential, ref mainFrameRetryCount, request, certificate);
            }

            return null;
        }

        private PostcartResponse TellerMiddleTierCall(RCIFCredential credential, ref int middleTierRetryCount, PostcartRequest postCartRequest, X509Certificate2 certificate)
        {
            try
            {
                if (middleTierRetryCount > 1)
                {
                    logwriter.Info("Teller Middle Tier Call Failed");
                    return new PostcartResponse();
                }

                TellerWorkQueueWSClient tellerWorkQueueClient = new TellerWorkQueueWSClient("MBEndpoint");
                PostcartResponse response = tellerWorkQueueClient.Postcart(postCartRequest);

                if (response == null || (response != null && response.ErrorCode != 0))
                {
                    throw new Exception();
                }

                return response;
            }
            catch (Exception ex)
            {
                PopulateErrorInfo(ex, "NA");

                middleTierRetryCount = middleTierRetryCount + 1;
                TellerMiddleTierCall(credential, ref middleTierRetryCount, postCartRequest, certificate);
            }

            return null;
        }

        private RCIFMainFrameResponse RCIFCall(RCIFCredential credential, ref int rcifFinalCommitRetryCount, RCIFMainFrameRequest request, X509Certificate2 certificate)
        {
            RCIFMainFrameResponse response = null;
            try
            {
                if (rcifFinalCommitRetryCount > 1)
                {
                    logwriter.Info("RCIF Final Commit Call Failed");
                    return new RCIFMainFrameResponse();
                }

                response = Zeo.Common.Util.AlloyUtil.RESTPostCall<RCIFMainFrameResponse>(credential.RCIFFinalCommitURL, certificate, request);

                //If the response from RCIF is null throw an exception.
                if(response == null || response.CIF7454OperationResponse == null)
                    throw new Exception();

                //If Checking for error codes in "cif7454r_return_messages" or "cif7454r_return_codes".
                if (response != null && response.CIF7454OperationResponse != null &&
                   (!CheckPropertyValues(response.CIF7454OperationResponse.CIF7454rreturnsvcmsgs) || !CheckPropertyValues(response.CIF7454OperationResponse.CIF7454rreturncodes)))
                {
                    throw new Exception();
                }

            }
            catch (Exception ex)
            {
                PopulateErrorInfo(ex, credential?.RCIFFinalCommitURL);

                rcifFinalCommitRetryCount = rcifFinalCommitRetryCount + 1;
                RCIFCall(credential, ref rcifFinalCommitRetryCount, request, certificate);
            }

            return response;
        }

        private CIF7450iRequestData GetRequestDataForRCIFCustomerRegistration(CustomerProfile customer, ZeoContext context)
        {
            CIF7450iRequestData requestData = new CIF7450iRequestData();
            Dictionary<string, object> ssoAttributes = GetSSOAttributes(context);

            requestData.CIF7450ireqtype = "A";
            requestData.CIF7450ireqoperid = ssoAttributes.GetStringValue("UserID");
            requestData.CIF7450ireqcustinst = RCIFCOMMITENV;
            requestData.CIF7450ireqcustbrch = ssoAttributes.GetIntValue("BranchNum");
            requestData.CIF7450ireqcustno = !string.IsNullOrWhiteSpace(customer.PartnerAccountNumber) == true ? Convert.ToInt64(customer.PartnerAccountNumber?.PadLeft(13, '0')?.Substring(0, 13)) : 0;
            requestData.CIF7450ireqacctinst = ssoAttributes.GetIntValue("BankNum");
            requestData.CIF7450ireqprodcode = "ZEOSVC";
            requestData.CIF7450ireqapplcd = "SVC";
            requestData.CIF7450ireqacct = customer.CustomerId; 
            requestData.CIF7450ireqrel = 7; //Check
            requestData.CIF7450ireqsource = "ZEO";
            requestData.CIF7450ireqname = $"{customer?.FirstName} {customer?.MiddleName} {customer?.LastName}";
            requestData.CIF7450ireqtin = !string.IsNullOrWhiteSpace(customer.SSN) ? Convert.ToInt32(customer.SSN) : 0;
            requestData.CIF7450ireqdob = GetIntDate(Convert.ToString(customer.DateOfBirth)); //Check
            requestData.CIF7450ireqsex = GetGender(customer.Gender); 
            requestData.CIF7450ireqlegalcd = customer.LegalCode;
            requestData.CIF7450ireqctznctry = customer.PrimaryCountryCitizenShip;
            requestData.CIF7450ireqctznctry2 = customer.SecondaryCountryCitizenShip;
            requestData.CIF7450ireqoccupcd = customer.Occupation; 
            requestData.CIF7450ireqoccupdesc = customer.OccupationDescription;
            requestData.CIF7450ireqcurrempl = customer.EmployerName;
            requestData.CIF7450ireqmaidenname = customer.MothersMaidenName;
            requestData.CIF7450ireqaddrline1 = customer.Address?.Address1;
            requestData.CIF7450ireqaddrline2 = customer.Address?.Address2;
            //requestData.CIF7450ireqaddrline3 = " ";
            requestData.CIF7450ireqcity = customer.Address?.City;
            requestData.CIF7450ireqstate = customer.Address?.State;
            requestData.CIF7450ireqzip5 = Convert.ToInt32(customer.Address?.ZipCode??"0");
            requestData.CIF7450ireqphone1type = MapToClientPhoneTypes(customer.Phone1?.Type);
            requestData.CIF7450ireqphone1 = customer.Phone1 !=null && !string.IsNullOrWhiteSpace(customer.Phone1.Number) ? Convert.ToInt64(customer.Phone1.Number) : 0; //Check
            requestData.CIF7450ireqphone2type = MapToClientPhoneTypes(customer.Phone2?.Type); 
            requestData.CIF7450ireqphone2 = customer.Phone2 != null && !string.IsNullOrWhiteSpace(customer.Phone2.Number) ? Convert.ToInt64(customer.Phone2.Number) : 0; //Check
            requestData.CIF7450ireqemailaddr = customer.Email;
            requestData.CIF7450ireqidtype = (customer != null ? Convert.ToString(MapToClientIdType(customer.IdType)) : string.Empty);
            requestData.CIF7450ireqidissuerst = customer.IDIssuingStateCode;
            requestData.CIF7450ireqidissuerctry = customer.IdIssuingCountry;
            requestData.CIF7450ireqidnumber = customer.IdNumber;
            requestData.CIF7450ireqidissuedt = GetIntDate(Convert.ToString(customer.IdIssueDate));
            requestData.CIF7450ireqidexpiredt = GetIntDate(Convert.ToString(customer.IdExpirationDate));
            requestData.CIF7450ireqofacscan = "P"; //Check
            requestData.CIF7450ireqrisksource = "N";
            requestData.CIF7450ireqrisktemplate = "";
            requestData.CIF7450ireqriskscorecust = 1;
            requestData.CIF7450ireqriskscoreacct = 0;
            //requestData.CIF7450ireqfiller = string.Empty;

            return requestData;
        }

        private RCIFCustomerRegResponse RCIFCustomerRegistrationCall(RCIFCredential credential, RCIFCustomerRegRequest request, X509Certificate2 certificate)
        {
            RCIFCustomerRegResponse response = null;
           
            response = Zeo.Common.Util.AlloyUtil.RESTPostCall<RCIFCustomerRegResponse>(credential.RCIFCustomerRegURL, certificate, request);
            
            return response;
        }

        private cpsCustomerRequestRecord GetRequestDataForRCIFCustomerEWSScanning(CustomerProfile customer, ZeoContext context)
        {
            cpsCustomerRequestRecord cpsRequest = new cpsCustomerRequestRecord();

            Dictionary<string, object> ssoAttributes = GetSSOAttributes(context);

            cpsRequest.cpsBankNumber = ssoAttributes.GetStringValue("BankNum");
            cpsRequest.cpsBranchNumber = ssoAttributes.GetStringValue("BranchNum"); 
            cpsRequest.cpsTellerNumber = ssoAttributes.GetStringValue("TellerNum");
            cpsRequest.cpsLawsonID = ssoAttributes.GetStringValue("LawsonID");
            cpsRequest.cpsPersonalNonpersonal = "P"; // "P": since all ZEO customers are personal customers
            cpsRequest.cpsExistingNew = string.IsNullOrWhiteSpace(customer?.PartnerAccountNumber) ? "N" : "E"; //"N" if the customer is not present in RCIF and "E" if its present
            cpsRequest.cpsFullLegalName = $"{customer?.FirstName} {customer?.MiddleName} {customer?.LastName}";
            cpsRequest.cpsStreetAddress = customer?.Address?.Address1;
            cpsRequest.cpsAddressSupplement = customer?.Address?.Address2;
            cpsRequest.cpsCityCountryCode = string.Empty;
            cpsRequest.cpsState = customer?.Address?.State;
            cpsRequest.cpsCity = customer?.Address?.City;
            cpsRequest.cpsZipCode = customer?.Address?.ZipCode;
            cpsRequest.cpsTaxIDNumber = customer?.SSN;
            cpsRequest.cpsDate = ConvertDateTimeToMMDDCCYYFormat(Convert.ToString(customer?.DateOfBirth));
            cpsRequest.cpsGender = GetGender(customer?.Gender);
            cpsRequest.cpsPhone1 = customer?.Phone1?.Number;
            cpsRequest.cpsPhone2 = customer?.Phone2?.Number;
            cpsRequest.cpsIDNumber = customer?.IdNumber;
            cpsRequest.cpsIDType = customer?.IdType;
            cpsRequest.cpsIssueByState = customer?.IdIssuingState;
            cpsRequest.cpsIssueByCountry = customer?.IdIssuingCountry;
            cpsRequest.cpsIssueDate = ConvertDateTimeToMMDDCCYYFormat(Convert.ToString(customer?.IdIssuingState));
            cpsRequest.cpsIssueDate = ConvertDateTimeToMMDDCCYYFormat(Convert.ToString(customer?.IdExpirationDate));
            cpsRequest.cpsInquiryNumber = string.Empty;
            cpsRequest.cpsVersion = "ZEO"; //For customer registration   

            return cpsRequest;
        }

        private cpsCustomerResponseRecord EWSCall(RCIFCredential credential, cpsCustomerRequestRecord cpsCustomerRequest, X509Certificate2 certificate)
        {

            cpsCustomerResponseRecord preScreenResponse = null;

            CustomerWSSoapClient client = new CustomerWSSoapClient("CustomerWSSoap", credential?.EWSPreScanURL);

            client.ClientCredentials.ClientCertificate.Certificate = certificate;

            client.ClientCredentials.UserName.UserName = "MB_user";

            client.ClientCredentials.UserName.Password = "test"; //t17U9uRJ

            preScreenResponse = client.PreScreen(cpsCustomerRequest);
           
            return preScreenResponse;
        }

        private string GetGender(Gender? genderval)
        {
            string gender = string.Empty;

            gender = (genderval != null && genderval == Gender.MALE) ? "M" : "F";

            return gender;
        }

        private string ConvertDateTimeToMMDDCCYYFormat(string date)
        {
            DateTime dateTime;
            DateTime.TryParse(date, out dateTime);
            string datetime = string.Empty;

            if (dateTime != null && dateTime != DateTime.MinValue)
            {
                string day = dateTime.Day.ToString().PadLeft(2, '0');
                string month = dateTime.Month.ToString().PadLeft(2, '0');
                string year = (dateTime.Year % 100).ToString().PadLeft(2, '0');
                string century = (dateTime.Year % 100) == 0 ? (dateTime.Year / 100).ToString() : ((dateTime.Year / 100) + 1).ToString();

                datetime = $"{month}{day}{century}{year}";
            }

            return datetime;
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

        private object GetCustInfo(CustomerTransactionDetails cart, bool isMiddleTier = false)
        {
            if(!isMiddleTier)
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
            else
            {
                CustomerInfo custInfo = new CustomerInfo()
                {
                    FirstName = cart.Customer.FirstName,
                    MiddleName = cart.Customer.MiddleName,
                    LastName = string.IsNullOrWhiteSpace(cart.Customer.SecondLastName) ? cart.Customer.LastName : string.Format("{0} {1}", cart.Customer.LastName, cart.Customer.SecondLastName),
                    AddressLine1 = cart.Customer.Address1,
                    AddressLine2 = cart.Customer.Address2,
                    City = cart.Customer.City,
                    State = cart.Customer.State,
                    Zip = cart.Customer.Zip,
                    PhoneNumber1 = cart.Customer.Phone1,
                    PhoneNumber1Type = cart.Customer.Ph1Type1 != null ? MapToClientPhoneTypes(cart.Customer.Ph1Type1) : null,
                    PhoneNumber2 = cart.Customer.Phone2,
                    PhoneNumber2Type = cart.Customer.Ph2Type2 != null ? MapToClientPhoneTypes(cart.Customer.Ph2Type2) : null,
                    PhoneNumber2Prov = cart.Customer.Ph2Prov,
                    Email = cart.Customer.Email,
                    SSN = cart.Customer.SSN,
                    TaxCode = !string.IsNullOrWhiteSpace(cart.Customer.SSN) && (cart.Customer.SSN.Substring(0, 1) == "9") ? TaxCdITIN : !string.IsNullOrWhiteSpace(cart.Customer.SSN) ? TaxCdSSN : null,
                    NEXXOPAN = Convert.ToString(cart.Customer.AlloyID),
                    ClientCustomerId = !string.IsNullOrWhiteSpace(cart.Customer.ClientCustId) ? cart.Customer.ClientCustId : "0",
                    Gender = cart.Customer.Gender != null ? cart.Customer.Gender.Substring(0, 1) : null,
                    TCFCustomerIndicator = cart.Customer.CustInd ? "Y" : "N",
                    ProdCode = PRODCODE,
                    ApplicationCode = APPL_CD,
                    DOB = Convert.ToDateTime(cart.Customer.DateOfBirth) != null ? Convert.ToDateTime(cart.Customer.DateOfBirth).ToString("yyyyMMdd") : null,
                    Maiden = cart.Customer != null ? cart.Customer.Maiden : string.Empty,
                    IdType = cart.Customer != null ? Convert.ToString(MapToClientIdType(cart.Customer.IdType)) : string.Empty,
                    IdIssuer = cart.Customer != null ? cart.Customer.IdIssuer : string.Empty,
                    IdIssuerCountry = cart.Customer != null ? cart.Customer.IdIssuerCountry : string.Empty,
                    IdNumber = cart.Customer != null ? cart.Customer.Identification : string.Empty,
                    IdIssueDate = cart.Customer != null && cart.Customer.IssueDate != null && cart.Customer.IssueDate != DateTime.MinValue ? cart.Customer.IssueDate.Value.ToString("yyyyMMdd") : null,
                    IdExpDate = cart.Customer != null && cart.Customer.ExpirationDate != null ? cart.Customer.ExpirationDate.Value.ToString("yyyyMMdd") : null,
                    //legalCode = cart.Customer != null ? cart.Customer.LegalCode : string.Empty,
                    CitizenshipCountry1 = cart.Customer != null ? cart.Customer.PrimaryCountryCitizenship : string.Empty,
                    CitizenshipCountry2 = cart.Customer != null ? cart.Customer.SecondaryCountryCitizenship : string.Empty,
                    Occupation = cart.Customer != null ? cart.Customer.Occupation : string.Empty,
                    OccDesc = cart.Customer != null ? cart.Customer.OccupationDescription : string.Empty,
                    EmployerName = cart.Customer != null ? cart.Customer.EmployerName : string.Empty,
                    EmployerPhoneNum = cart.Customer != null ? cart.Customer.EmployerPhoneNum : string.Empty
                    //TCFExist,TCFPAN,CustomerSince,CustomerType
                };

                return custInfo;
            }
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

        //private PreFlushSummary_Transaction GetTellerFinalCommitData(Transaction tran)
        //{
        //    //PreFlushSummary_Transaction transactionSummary = new PreFlushSummary_Transaction();
        //    TELNexxcTranInfoRequest telnexxc_tran_info = new TELNexxcTranInfoRequest();

        //    string type = string.Empty;

        //    string transType = tran.Type;

            //if (transType == Convert.ToString(TransactionType.Cash))
            //{
            //    type = tran.CashType != null ? tran.CashType : tran.Type;
            //    transactionSummary = PopulateCash(tran, type);
            //}
            //else if (transType == Convert.ToString(TransactionType.MoneyTransfer))
            //{
            //    type = tran.TransferType != null ? tran.TransferType : tran.Type;
            //    transactionSummary = PopulateMoneyTransfer(tran, type);
            //}
            //else if (transType == Convert.ToString(TransactionType.BillPayment))
            //{
            //    transactionSummary = PopulateBillPay(tran);
            //}
            //else if (transType == Convert.ToString(TransactionType.MoneyOrder))
            //{
            //    transactionSummary = PopulateMoneyOrder(tran);
            //}
            //if (transType == Convert.ToString(TransactionType.ProcessCheck))
            //{
            //    telnexxc_tran_info.Telnexxctran = CheckProcessing;
            //    telnexxc_tran_info.Telnexxctype = "Commit";
            //    telnexxc_tran_info.Telnexxcacct = Convert.ToString(tran.AccountNumber)?.PadLeft(13, '0'),//"2000029932", 
            //    telnexxc_tran_info.Telnexxccheckno = tcfonusTrx.CheckNumber?.PadLeft(11, '0'),
            //    telnexxc_tran_info.Telnexxcamt = tcfonusTrx.Amount,
            //    telnexxc_tran_info.Telnexxcfee = tcfonusTrx.Fee,
            //    telnexxc_tran_info.Telnexxctcfcard = string.Empty,
            //    telnexxc_tran_info.Telnexxcappl = APPL_CD,
            //    telnexxc_tran_info.Telnexxczeoacct = Convert.ToString(00700000000000000183)?.PadLeft(20, '0'),
            //    telnexxc_tran_info.Telnexxcinitial = "A",
            //    telnexxc_tran_info.Telnexxcroutingno = tcfonusTrx.RoutingNumber.PadLeft(9, '0'),
            //    telnexxc_tran_info.Telnexxcmgitranid = Convert.ToString(tcfonusTrx.Id)
            //};

            //return transactionSummary;
       // }
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


            // Thumbprints of the IIB Server Certificates. Once the server cert issue is fixed at IIB - we should remove this code.
            // Made the implementation change to also have an option to read the thumbprint from the config file, just in case if there are multiple certs which we are not aware of.
            string iibCertHash = Convert.ToString(ConfigurationManager.AppSettings["IIBCertThumbprint"]);
            // 1st is NP, 2nd is Prod.
            List<string> iibCertHashList = new List<string>() { "421D0E3675BE934F20AC4B655F06EFE1C8A4BF41" };

            if (!string.IsNullOrWhiteSpace(iibCertHash))
            {
                iibCertHashList.AddRange(iibCertHash.ToUpper().Split('|'));
            }
            return iibCertHashList.Contains(cert.GetCertHashString());
        }
        #endregion

        #region Final Commit Private Methods

        private ZeoTransaction GetZeoTransaction(Transaction tran, ZeoContext context)
        {
            ZeoTransaction zeoTran = new ZeoTransaction();
            string type = string.Empty;

            string transType = tran.Type;

            if (transType == Convert.ToString(TransactionType.Cash))
            {
                type = tran.CashType != null ? tran.CashType : tran.Type;
                return PopulateCashForTellerWorkQueue(tran, type);
            }
            else if (transType == Convert.ToString(TransactionType.MoneyTransfer))
            {
                type = tran.TransferType != null ? tran.TransferType : tran.Type;
                return PopulateMoneyTransferForTellerWorkQueue(tran, type);
            }
            else if (transType == Convert.ToString(TransactionType.BillPayment))
            {
                return PopulateBillPayForTellerWorkQueue(tran);
            }
            else if (transType == Convert.ToString(TransactionType.MoneyOrder))
            {
                return PopulateMoneyOrderForTellerWorkQueue(tran);
            }
            else if (transType == Convert.ToString(TransactionType.ProcessCheck))
            {
                return PopulateCheckForTellerWorkQueue(tran, context);
            }
            else if (transType == Convert.ToString(TransactionType.Fund))
            {
                type = tran.TransferType != null ? tran.TransferType : tran.Type;
                return PopulateFundsForTellerWorkQueue(tran, type);
            }

            return zeoTran;
        }

        private ZeoTransaction PopulateCashForTellerWorkQueue(Transaction cashTran, string type)
        {
            ZeoTransaction zeoTran = new ZeoTransaction();

            if (cashTran != null)
            {
                zeoTran.TranId = Convert.ToString(cashTran.ID);
                zeoTran.TranType = type;
                zeoTran.AccountNumber = cashTran.AccountNumber;
                zeoTran.Amount = RoundOffDecimal(cashTran.Amount, 2);
                zeoTran.Total = RoundOffDecimal(cashTran.GrossTotalAmount, 2);
            }

            return zeoTran;
        }

        private ZeoTransaction PopulateMoneyTransferForTellerWorkQueue(Transaction tran, string type)
        {
            ZeoTransaction zeoTran = new ZeoTransaction();

            if (string.Compare(type, MoneyTransferType.Send.ToString(), true) == 0)
            {
                zeoTran = PopulateSendMoneyForTellerWorkQueue(tran, type);
            }
            else if (string.Compare(type, MoneyTransferType.Receive.ToString(), true) == 0)
            {
                zeoTran = PopulateReceiveMoneyForTellerWorkQueue(tran, type);
            }

            return zeoTran;
        }

        private ZeoTransaction PopulateSendMoneyForTellerWorkQueue(Transaction tran, string type)
        {
            ZeoTransaction zeoTran = new ZeoTransaction()
            {
                TranId = tran.ID,
                TranType = "SendMoney",
                MTCN = tran.MTCN,
                Amount = RoundOffDecimal(tran.Amount, 2),
                Fee = RoundOffDecimal(tran.Fee, 2),
                Total = RoundOffDecimal(tran.GrossTotalAmount, 2),
                ToFirstName = tran.ToFirstName,
                ToMiddleName = tran.ToMiddleName,
                ToLastName = tran.ToLastName,
                ToSecondLastName = tran.ToSecondLastName,
                ToGender = tran.ToGender != null ? tran.ToGender.Substring(0, 1) : null,
                ToCountry = tran.ToCountry,
                ToAddress = tran.ToAddress,
                ToCity = tran.ToCity,
                ToStateProvince = tran.ToState_Province,
                ToZipCode = tran.ToZipCode,
                ToPhoneNumber = tran.ToPhoneNumber,
                ToPickupCountry = tran.ToPickUpCountry,
                ToPickupStateProvince = tran.ToPickUpState_Province,
                ToPickupCity = tran.ToPickUpCity,
                ToDeliveryMethod = tran.ToDeliveryMethod,
                ToDeliveryOption = tran.ToDeliveryOption,
                ToOccupation = tran.ToOccupation,
                ToDOB = Convert.ToString(tran.ToDOB),
                ToCountryOfBirth = tran.ToCountryOfBirth
            };

            return zeoTran;
        }

        private ZeoTransaction PopulateReceiveMoneyForTellerWorkQueue(Transaction tran, string type)
        {
            ZeoTransaction zeoTran = new ZeoTransaction()
            {
                TranId = tran.ID,
                TranType = "ReceiveMoney",
                MTCN = tran.MTCN,
                FirstName = tran.ToFirstName, // Receiver's FirstName
                ToMiddleName = tran.ToMiddleName, // Receiver's MiddleName
                LastName = tran.ToLastName, // Receiver's LastName
                SecondLastName = tran.ToSecondLastName, // Receiver's Second LastName
                PickupCountry = tran.ToPickUpCountry,
                Amount = RoundOffDecimal(tran.Amount, 2),
                Fee = RoundOffDecimal(tran.Fee, 2),
                Total = RoundOffDecimal(tran.GrossTotalAmount, 2)
            };

            return zeoTran;
        }

        private ZeoTransaction PopulateBillPayForTellerWorkQueue(Transaction billPayTran)
        {
            ZeoTransaction zeoTran = new ZeoTransaction()
            {
                TranId = billPayTran.ID,
                TranType = "BillPay",
                AccountNumber = billPayTran.AccountNumber,
                Payee = billPayTran.Payee,
                MTCN = billPayTran.MTCN,
                Amount = RoundOffDecimal(billPayTran.Amount, 2),
                Fee = RoundOffDecimal(billPayTran.Fee, 2),
                Total = RoundOffDecimal(billPayTran.GrossTotalAmount, 2)
            };

            return zeoTran;
        }

        private ZeoTransaction PopulateMoneyOrderForTellerWorkQueue(Transaction moTran)
        {
            ZeoTransaction zeoTran = new ZeoTransaction()
            {
                TranId = moTran.ID,
                TranType = TransactionType.MoneyOrder.ToString(),
                CheckNumber = moTran.CheckNumber,
                Amount = RoundOffDecimal(moTran.Amount, 2),
                Fee = RoundOffDecimal(moTran.Fee, 2),
                Total = RoundOffDecimal(moTran.GrossTotalAmount, 2),
            };

            return zeoTran;
        }

        private ZeoTransaction PopulateCheckForTellerWorkQueue(Transaction cpTran, ZeoContext context)
        {
            ZeoTransaction zeoTran = new ZeoTransaction()
            {
                TranId = cpTran.ID.ToString(),
                TranType = CheckProcessing,
                CheckNumber = cpTran.CheckNumber,
                CheckType = (context.ProviderId == Convert.ToInt32(ProviderId.TCFCheck)) ? Convert.ToString((CheckTypes)context.OnUSChecktype) : cpTran.CheckType,
                ConfirmationNumber = cpTran.ConfirmationNumber,
                Amount = RoundOffDecimal(cpTran.Amount, 2),
                Fee = RoundOffDecimal(cpTran.Fee, 2),
                Total = RoundOffDecimal(cpTran.GrossTotalAmount, 2)
            };

            return zeoTran;
        }

        private ZeoTransaction PopulateFundsForTellerWorkQueue(Transaction feTran, string type)
        {
            ZeoTransaction zeoTran = new ZeoTransaction();

            if (string.Compare(type, FundType.Credit.ToString(), true) == 0)
            {
                return PopulatePrepaidLoadForTellerWorkQueue(feTran, type);
            }
            else if (string.Compare(type, FundType.Debit.ToString(), true) == 0)
            {
                return PopulatePrePaidWithdrawForTellerWorkQueue(feTran, type);
            }

            return zeoTran;
        }

        private ZeoTransaction PopulatePrepaidLoadForTellerWorkQueue(Transaction feTran, string type)
        {
            ZeoTransaction zeoTran = new ZeoTransaction()
            {
                TranId = Convert.ToString(feTran.ID),
                TranType = "PrePaidLoad",
                InitialPurchase = feTran.InitialPurchase,
                PurchaseFee = RoundOffDecimal(feTran.PurchaseFee, 2),
                NewCardBalance = RoundOffDecimal(feTran.NewCardBalance, 2),
                CardNbr = feTran.TransferSubType == (int)Helper.FundType.Activation ? feTran.CardNumber : feTran.CardNumber.Substring(feTran.CardNumber.Length - 6),
                AliasId = feTran.AliasId,
                ConfirmationNumber = feTran.ConfirmationNumber,
                LoadAmount = RoundOffDecimal(feTran.Amount, 2),
                Fee = RoundOffDecimal(feTran.Fee, 2),
                Total = RoundOffDecimal(feTran.GrossTotalAmount, 2)
            };

            return zeoTran;
        }

        private ZeoTransaction PopulatePrePaidWithdrawForTellerWorkQueue(Transaction feTran, string type)
        {
            ZeoTransaction zeoTran = new ZeoTransaction()
            {
                TranId = Convert.ToString(feTran.ID),
                TranType = "PrePaidWithdraw",
                NewCardBalance = RoundOffDecimal(feTran.NewCardBalance, 2),
                CardNbr = feTran.CardNumber.Substring(feTran.CardNumber.Length - 6),
                AliasId = feTran.AliasId,
                ConfirmationNumber = feTran.ConfirmationNumber,
                WithdrawAmount = RoundOffDecimal(feTran.Amount, 2),
                Fee = RoundOffDecimal(feTran.Fee, 2),
                Total = RoundOffDecimal(feTran.GrossTotalAmount, 2)
            };

            return zeoTran;
        }

        //Request is populated as per given in the CIF7454 mapping document.
        private CIF7454iRequestData GetRequestDataForRCIF_Visa(CustomerDetails customer, string CIF7454TemplateType, Tuple<long, long, long> riskScores, Transaction feTran, ZeoContext context)
        {
            CIF7454iRequestData requestData = new CIF7454iRequestData();
            Dictionary<string, object> ssoAttributes = GetSSOAttributes(context);

            requestData.CIF7454ireqtype = "A";
            requestData.CIF7454ireqcustinst = RCIFCOMMITENV;
            requestData.CIF7454ireqcustno = !string.IsNullOrWhiteSpace(customer.ClientCustId) == true ? Convert.ToInt64(customer.ClientCustId?.PadLeft(13, '0')?.Substring(0, 13)): 0;
            requestData.CIF7454ireqrisktemplate = !string.IsNullOrWhiteSpace(customer.ClientCustId) ? $"{ customer.ClientCustId}{ CIF7454TemplateType }" : string.Empty;
            requestData.CIF7454ireqverifytin = !string.IsNullOrWhiteSpace(customer.SSN) ? Convert.ToInt32(customer.SSN):0; 
            requestData.CIF7454ireqverifydob = GetIntDate(customer.DateOfBirth);
            requestData.CIF7454ireqlegalcd = customer.LegalCode;
            requestData.CIF7454ireqctznctry = customer.PrimaryCountryCitizenship;
            requestData.CIF7454ireqctznctry2 = customer.SecondaryCountryCitizenship;
            requestData.CIF7454ireqidtype = (customer != null ? Convert.ToString(MapToClientIdType(customer.IdType)) : string.Empty);
            requestData.CIF7454ireqidissuerst = customer.IdIssuer;
            requestData.CIF7454ireqidissuerctry = customer.IdIssuerCountry;
            requestData.CIF7454ireqidnumber = customer.Identification;
            requestData.CIF7454ireqidissuedt = GetIntDate(Convert.ToString(customer.IssueDate));
            requestData.CIF7454ireqidexpiredt = GetIntDate(Convert.ToString(customer.ExpirationDate));
            requestData.CIF7454ireqcipovopt = " ";
            requestData.CIF7454ireqprodcode1 = "ZEOACT";
            requestData.CIF7454ireqapplcd1 = "VSA";
            requestData.CIF7454ireqrel1 = "7";
            requestData.CIF7454ireqacct1 = feTran?.TransactionAccountId?.PadLeft(19, '0');
            requestData.CIF7454ireqprodcode2 = "ZEOCRD";
            requestData.CIF7454ireqapplcd2 = "VSA";
            requestData.CIF7454ireqacct2 = feTran?.CardNumber?.PadLeft(18, '0');
            requestData.CIF7454ireqacctinst2 = ssoAttributes.GetStringValue("BankNum");
            requestData.CIF7454ireqacctbrch2 = ssoAttributes.GetStringValue("BranchNum");
            requestData.CIF7454ireqrel2 = "7";
            requestData.CIF7454ireqriskscorecust = Convert.ToString(riskScores.Item1); //Customer Risk Score
            requestData.CIF7454ireqriskscorezeosvc = Convert.ToString(riskScores.Item3); //Zeo Service Risk Score
            requestData.CIF7454ireqriskscoreacct1 = Convert.ToString(riskScores.Item3); //Zeo Service Risk Score
            requestData.CIF7454ireqriskscoreacct2 = Convert.ToString(riskScores.Item2); //GPR Risk Score
            requestData.CIF7454ireqoperid = ssoAttributes.GetStringValue("UserID");
            requestData.CIF7454ireqacctinst1 = ssoAttributes.GetStringValue("BankNum");
            requestData.CIF7454ireqacctbrch1 = ssoAttributes.GetStringValue("BranchNum");
            return requestData;
        }

        private CIF7454iRequestData GetRequestDataForRCIF_Others(CustomerDetails customer, string CIF7454TemplateType, Tuple<long, long, long> riskScores, Transaction feTran, ZeoContext context)
        {
            CIF7454iRequestData requestData = new CIF7454iRequestData();
            Dictionary<string, object> ssoAttributes = GetSSOAttributes(context);

            requestData.CIF7454ireqtype = "A";
            requestData.CIF7454ireqcustinst = RCIFCOMMITENV;
            requestData.CIF7454ireqcustno = !string.IsNullOrWhiteSpace(customer.ClientCustId) == true ? Convert.ToInt64(customer.ClientCustId?.PadLeft(13, '0')?.Substring(0, 13)) : 0;
            requestData.CIF7454ireqrisktemplate = !string.IsNullOrWhiteSpace(customer.ClientCustId) ? $"{ customer.ClientCustId }{ CIF7454TemplateType }" : string.Empty;
            requestData.CIF7454ireqverifytin = !string.IsNullOrWhiteSpace(customer.SSN) ? Convert.ToInt32(customer.SSN) : 0;
            requestData.CIF7454ireqverifydob = GetIntDate(customer.DateOfBirth);
            requestData.CIF7454ireqlegalcd = customer.LegalCode;
            requestData.CIF7454ireqctznctry = customer.PrimaryCountryCitizenship;
            requestData.CIF7454ireqctznctry2 = customer.SecondaryCountryCitizenship;
            requestData.CIF7454ireqidtype = (customer != null ? Convert.ToString(MapToClientIdType(customer.IdType)) : string.Empty);
            requestData.CIF7454ireqidissuerst = customer.IdIssuer;
            requestData.CIF7454ireqidissuerctry = customer.IdIssuerCountry;
            requestData.CIF7454ireqidnumber = customer.Identification;
            requestData.CIF7454ireqidissuedt = GetIntDate(Convert.ToString(customer.IssueDate));
            requestData.CIF7454ireqidexpiredt = GetIntDate(Convert.ToString(customer.ExpirationDate));
            requestData.CIF7454ireqcipovopt = " ";
            requestData.CIF7454ireqriskscorecust = Convert.ToString(riskScores.Item1); //Customer Risk Score
            requestData.CIF7454ireqriskscorezeosvc = Convert.ToString(riskScores.Item3); //Zeo Service Risk Score
            requestData.CIF7454ireqoperid = ssoAttributes.GetStringValue("UserID");            

            return requestData;
        }


        private TELNexxcTranInfoRequest GetTelNexxcTranInfoRequest(Transaction tran, ZeoContext context)
        {
            TELNexxcTranInfoRequest telnexxc_tran_info = new TELNexxcTranInfoRequest();

            string transType = tran.Type;
            string type = GetTransactionType(tran);

            telnexxc_tran_info.Telnexxctran = type;
            telnexxc_tran_info.Telnexxctype = (context.ProviderId == Convert.ToInt32(ProviderId.TCFCheck)) ? Convert.ToString((CheckTypes)context.OnUSChecktype) : tran.CheckType;
            telnexxc_tran_info.Telnexxcacct = tran.CheckAccountNumber?.PadLeft(13, '0');
            telnexxc_tran_info.Telnexxcnexxoacct = context.CustomerId.ToString().PadLeft(18, '0');
            telnexxc_tran_info.Telnexxccheckno = tran.CheckNumber?.PadLeft(11, '0');
            telnexxc_tran_info.Telnexxcamt = tran.Amount;
            telnexxc_tran_info.Telnexxcfee = tran.Fee;
            telnexxc_tran_info.Telnexxcappl = "ZEO";
            telnexxc_tran_info.Telnexxczeoacct = !string.IsNullOrWhiteSpace(tran.CardNumber) ? tran.CardNumber.PadLeft(20, '0'): null;
            telnexxc_tran_info.Telnexxcinitial = tran.InitialPurchase;
            telnexxc_tran_info.Telnexxcroutingno = !string.IsNullOrWhiteSpace(tran.CheckRoutingNumber) ? tran.CheckRoutingNumber.PadLeft(9, '0').Substring(0, 9) : string.Empty;
            telnexxc_tran_info.Telnexxcmgitranid = Convert.ToString(tran.ID);

            Dictionary<string, object> ssoAttributes = GetSSOAttributes(context);

            telnexxc_tran_info.Telnexxctellerbk = ssoAttributes.GetStringValue("BankNum");
            telnexxc_tran_info.Telnexxctellerbr = ssoAttributes.GetStringValue("BranchNum");
            telnexxc_tran_info.Telnexxcteller = ssoAttributes.GetStringValue("TellerNum");
            telnexxc_tran_info.Telnexxcampm = ssoAttributes.GetStringValue("AmPmInd");
            telnexxc_tran_info.Telnexxcdrawer = ssoAttributes.GetStringValue("CashDrawer");
            telnexxc_tran_info.Telnexxclu = ssoAttributes.GetStringValue("LU");
            telnexxc_tran_info.Telnexxclawson = ssoAttributes.GetStringValue("LawsonID");

            return telnexxc_tran_info;
        }

        private int GetIntDate(string dateOfBirth)
        {
            int date = 0;
            if (!string.IsNullOrWhiteSpace(dateOfBirth) && DateTime.Parse(dateOfBirth) != DateTime.MinValue)
            {
                DateTime dt = DateTime.Parse(dateOfBirth);
                date = Int32.Parse(dt.ToString("yyyyMMdd"));
            }

            return date;
        }

        private string GetTransactionType(Transaction tran)
        {
            string transType = tran.Type;
            string type = string.Empty;

            if (transType == Convert.ToString(TransactionType.Cash))
            {
                type = tran.CashType != null ? tran.CashType : tran.Type;
            }
            else if (transType == Convert.ToString(TransactionType.MoneyTransfer))
            {
                switch (tran.TransferType)
                {
                    case "Send":
                        type = "SendMoney";
                        break;
                    case "Receive":
                        type = "ReceiveMoney";
                        break;
                }
            }
            else if (transType == Convert.ToString(TransactionType.BillPayment))
            {
                type = "BillPay";
            }
            else if (transType == Convert.ToString(TransactionType.MoneyOrder))
            {
                type = "MoneyOrder";
            }
            else if (transType == Convert.ToString(TransactionType.ProcessCheck))
            {
                type = CheckProcessing;
            }
            else if (transType == Convert.ToString(TransactionType.Fund))
            {
                switch (tran.TransferType)
                {
                    case "Debit":
                        type = "PrePaidWithdraw";
                        break;
                    case "Credit":
                        type = "PrePaidLoad";
                        break;
                    default:
                        type = string.Empty;
                        break;
                }
            }

            return type;
        }

        private void HandleCheckException(Exception ex)
        {
            Exception tcisProviderException = ex as PartnerException;
            if (tcisProviderException != null)
            {
                PartnerException tcisException = tcisProviderException as PartnerException;
                string errorCode = Convert.ToString(tcisException.ProviderErrorCode);
                string errorMsg = Convert.ToString(tcisException.Message);

                throw new PartnerException(errorCode, errorMsg, null);
            }
            Exception faultException = ex as FaultException;
            if (faultException != null)
            {
                throw new PartnerException(ProviderException.PROVIDER_FAULT_ERROR, string.Empty, faultException);
            }
            Exception endpointException = ex as EndpointNotFoundException;
            if (endpointException != null)
            {
                throw new PartnerException(ProviderException.PROVIDER_ENDPOINTNOTFOUND_ERROR, string.Empty, endpointException);
            }
            Exception commException = ex as CommunicationException;
            if (commException != null)
            {
                throw new PartnerException(ProviderException.PROVIDER_COMMUNICATION_ERROR, string.Empty, commException);
            }
            Exception timeOutException = ex as TimeoutException;
            if (timeOutException != null)
            {
                throw new PartnerException(ProviderException.PROVIDER_TIMEOUT_ERROR, string.Empty, timeOutException);
            }
        }

        private Dictionary<string, object> GetSSOAttributes(ZeoContext context)
        {
            Dictionary<string, object> ssoAttributes = new Dictionary<string, object>();

            if (!IsHardcodedSSOAttr)
                return context.SSOAttributes;

            List<string> attrs = new List<string>() {"BankNum","BranchNum", "TellerNum",
                    "AmPmInd", "CashDrawer", "LU", "LawsonID", "UserID", "BusinessDate", "MachineName", "DPSBranchID"};

            string[] attributes = SSOAttributes?.Split('|');

            if (attributes != null && attributes.Length > 0)
            {
                foreach (var attr in attributes)
                {
                    string[] keyvalue = attr.Split(':');
                    if (keyvalue.Length > 0 && attrs.Contains(keyvalue[0]))
                    {
                        ssoAttributes.Add(keyvalue[0], keyvalue[1]);
                    }
                }
            }

            return ssoAttributes;
        }

        private void SerializeObjectAndLog<T>(T obj)
        {
            string serializedStr = string.Empty;
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));

            using (var sww = new StringWriter())
            {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, obj);
                    serializedStr = sww.ToString();
                }
            }

            logwriter.Info(serializedStr);
        }

        private bool CheckPropertyValues<T>(T obj)
        {
            Type type = obj.GetType();

            try
            {
                foreach (PropertyInfo pi in type.GetProperties())
                {
                    string value = Convert.ToString(pi.GetValue(obj, null))?.Trim();

                    if (!string.IsNullOrEmpty(value))
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool isTellerWorkQueueWSCallFailed(PostcartResponse response)
        {
            if (response?.ErrorMessage?.Trim()?.ToUpper() == "PROCEED")
                return false;
            else
                return true;
        }

        private void PopulateErrorInfo(Exception ex, string url)
        {
            StringBuilder messageBuilder = new StringBuilder(); 

            int counter = 1;

            int exceptionDrillDownLimit = 5;

            while (ex != null && counter <= exceptionDrillDownLimit)
            {
                messageBuilder.AppendFormat("\n Level {0}: {1} \n", counter, !string.IsNullOrWhiteSpace(ex.Message) ? ex.Message : "No exception message available");
                messageBuilder.AppendFormat("\n Stack Trace Level {0}: {1} \n", counter, !string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.StackTrace : "No stack trace available");
                ex = ex.InnerException;
                counter++;
            }

            logwriter.Info("Exception Details : \n" + messageBuilder.ToString());
            logwriter.Info("URL of the service : " + (!string.IsNullOrEmpty(url) ? url : string.Empty));
        }

        #endregion

        #endregion

    }
}
