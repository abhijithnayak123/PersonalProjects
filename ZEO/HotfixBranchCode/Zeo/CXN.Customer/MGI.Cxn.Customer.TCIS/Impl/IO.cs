using AutoMapper;
using MGI.Cxn.Customer.TCIS.Data;
using MGI.Cxn.Customer.TCIS.RCIFService;
using MGI.Cxn.Customer.Contract;
using MGI.Cxn.Customer.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Globalization;
using MGI.Cxn.Customer.TCIS.Contract;

namespace MGI.Cxn.Customer.TCIS.Impl
{
	public class IO
	{
		public NLoggerCommon NLogger { get; set; }
		private const string GenericErrorMessage = "Unknown error, report issue to System Administrator";

		public IO()
		{
			//Create Customer
			Mapper.CreateMap<Account, PersonalInfo>()
				.ForMember(x => x.FName, o => o.MapFrom(s => s.FirstName))

				.ForMember(x => x.LName, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.LastName2) ? s.LastName : string.Format("{0} {1}", s.LastName, s.LastName2)))
				.ForMember(x => x.Addr1, o => o.MapFrom(s => s.Address1))
				.ForMember(x => x.Addr2, o => o.MapFrom(s => s.Address2))
				.ForMember(x => x.City, o => o.MapFrom(s => s.City))
				.ForMember(x => x.State, o => o.MapFrom(s => s.State))
				.ForMember(x => x.zip, o => o.MapFrom(s => s.ZipCode))
				.ForMember(x => x.Ph1, o => o.MapFrom(s => s.Phone1))
				//Do Not pass phone providers
				.ForMember(x => x.Ph1Type1, o => o.MapFrom(s => s.Phone1Type != null ? MapToClientPhoneTypes(s.Phone1Type) : null))
				.ForMember(x => x.Ph2, o => o.MapFrom(s => s.Phone2))
				.ForMember(x => x.Ph2Type2, o => o.MapFrom(s => s.Phone2Type != null ? MapToClientPhoneTypes(s.Phone2Type) : null))
				.ForMember(x => x.ssn, o => o.MapFrom(s => s.SSN))
				.ForMember(x => x.ClientCustId, o => o.MapFrom(s => s.ClientID))
				.ForMember(x => x.gender, o => o.MapFrom(s => s.Gender != null ? s.Gender.Substring(0, 1) : null))
				.ForMember(x => x.TaxCd, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.SSN) && (s.SSN.Substring(0, 1) == "9") ? TaxCdITIN : !string.IsNullOrWhiteSpace(s.SSN) ? TaxCdSSN : null))
				.ForMember(x => x.tcfCustind, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.ClientID) && s.ClientID != "0" ? "Y" : "N"))
				.AfterMap((s, d) =>
					{
						d.PRODCODE = PRODCODE;
						d.APPL_CD = APPL_CD;
						if (!string.IsNullOrWhiteSpace(s.MiddleName))
						{
							d.MName = s.MiddleName;
						}
					});

			Mapper.CreateMap<Account, Identification>()
				.ForMember(x => x.dob, o => o.MapFrom(s => s.DateOfBirth != null ? s.DateOfBirth.Value.ToString("yyyyMMdd") : null))
				.ForMember(x => x.maiden, o => o.MapFrom(s => s.MothersMaidenName))
				.ForMember(x => x.idType, o => o.MapFrom(s => s.GovernmentIDType != null ? MapToClientIdType(s.GovernmentIDType) : null))
				.ForMember(x => x.idIssuer, o => o.MapFrom(s => s.IDIssuingState))
				.ForMember(x => x.idIssuerCountry, o => o.MapFrom(s => s.IDIssuingCountry!=null?s.IDIssuingCountry.Trim():null))//AL-2169
				.ForMember(x => x.idNbr, o => o.MapFrom(s => s.GovernmentId))
				.ForMember(x => x.idIssueDate, o => o.MapFrom(s => s.IDIssueDate != null ? s.IDIssueDate.Value.ToString("yyyyMMdd") : null))
				.ForMember(x => x.idExpDate, o => o.MapFrom(s => s.IDExpirationDate != null && s.IDExpirationDate != DateTime.MinValue ? s.IDExpirationDate.Value.ToString("yyyyMMdd") : null))
				.ForMember(x => x.legalCode, o => o.MapFrom(s => s.LegalCode))
				.ForMember(x => x.citizenshipCountry1, o => o.MapFrom(s => s.PrimaryCountryCitizenShip))
				.ForMember(x => x.citizenshipCountry2, o => o.MapFrom(s => s.SecondaryCountryCitizenShip));

			Mapper.CreateMap<Account, EmploymentInfo>()
				.ForMember(x => x.Occupation, o => o.MapFrom(s => s.Occupation))
				.ForMember(x => x.OccDesc, o => o.MapFrom(s => s.OccupationDescription))
				.ForMember(x => x.EmployerName, o => o.MapFrom(s => s.EmployerName))
				.ForMember(x => x.EmployerPhoneNum, o => o.MapFrom(s => s.EmployerPhone));

			//Find Customer
			Mapper.CreateMap<CustInfo, Account>()
				.ForMember(x => x.IDIssueDate, opt => opt.Ignore())
				.ForMember(x => x.IDExpirationDate, opt => opt.Ignore())
				.AfterMap((s, d) =>
					{
						if (s.PersonalInfo != null)
						{
							d.FirstName = s.PersonalInfo.FName;
							d.MiddleName = s.PersonalInfo.MName;
							d.LastName = s.PersonalInfo.LName;
							d.Address1 = s.PersonalInfo.Addr1;
							d.Address2 = s.PersonalInfo.Addr2;
							d.City = s.PersonalInfo.City;
							d.State = s.PersonalInfo.State;
							d.ZipCode = s.PersonalInfo.zip;
							//Do Not pass phone providers
							d.Phone1 = s.PersonalInfo.Ph1;
							d.Phone1Type = s.PersonalInfo.Ph1Type1 != null ? MapToAlloyPhoneTypes(s.PersonalInfo.Ph1Type1) : null;
							d.Phone2 = s.PersonalInfo.Ph2;
							d.Phone2Type = s.PersonalInfo.Ph2Type2 != null ? MapToAlloyPhoneTypes(s.PersonalInfo.Ph2Type2) : null;
							d.Email = s.PersonalInfo.email;
                            d.IDCode = NexxoUtil.GetIDCode(s.PersonalInfo.ssn);
							d.SSN = s.PersonalInfo.ssn;
							d.ClientID = s.PersonalInfo.ClientCustId;
							d.Gender = string.IsNullOrWhiteSpace(s.PersonalInfo.gender) ? null : s.PersonalInfo.gender == "M" ? "MALE" : "FEMALE";
						}
						if (s.Identification != null)
						{
							d.MothersMaidenName = s.Identification.maiden;
							d.GovernmentIDType = MapToAlloyIdType(s.Identification.idType);
							if (s.Identification.idType == "D" || s.Identification.idType == "S")
							{
								d.IDIssuingState = s.Identification.idIssuer;
							}
							else
							{
								d.IDIssuingState = null;
							}
							d.IDIssuingCountry = s.Identification.idIssuerCountry!=null ? s.Identification.idIssuerCountry.Trim():null;
							d.GovernmentId = s.Identification.idNbr;
							d.LegalCode = s.Identification.legalCode;
							d.PrimaryCountryCitizenShip = s.Identification.citizenshipCountry1;
							d.SecondaryCountryCitizenShip = s.Identification.citizenshipCountry2;
							if (s.Identification.dob != null && Convert.ToInt32(s.Identification.dob) > 9999999)
							{
								d.DateOfBirth = DateTime.ParseExact(s.Identification.dob, "yyyyMMdd", CultureInfo.InvariantCulture);
							}
						}
						if (s.EmploymentInfo != null)
						{
							d.Occupation = !string.IsNullOrEmpty(s.EmploymentInfo.Occupation) ? s.EmploymentInfo.Occupation: string.Empty;
							d.OccupationDescription = s.EmploymentInfo.OccDesc;
							d.EmployerName = s.EmploymentInfo.EmployerName;
							d.EmployerPhone = s.EmploymentInfo.EmployerPhoneNum;
						}

					});
		}
		#region Contrant Declaration

		private const string PRODCODE = "ZEOSVC";
		private const string APPL_CD = "SVC";
		private const string TaxCdSSN = "S";
		private const string TaxCdITIN = "I";

		private const string SoftStop = "1";
		private const string HardStop = "2";

		#endregion
		/// <summary>
		/// FetchAll Customers by CustomerLookupCriteria
		/// </summary>
		/// <param name="context"></param>
		/// <param name="customerLookUpCriteria"></param>
		/// <returns></returns>
		public List<Account> FetchAll(System.Collections.Generic.Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext)
		{
			try
			{
				long channelPartnerId = cxnContext.ChannelPartnerId;

				ZeoFindCustomerReq request = new ZeoFindCustomerReq();

				Svc[] baserequest = new Svc[1];

				baserequest[0] = new Svc();

				baserequest[0].Security = new Security()
				{
					BasicAuth = GetBasicAuth(cxnContext)
				};

				baserequest[0].SvcParms = GetsvcParams();

				DateTime dateOfBirth = Convert.ToDateTime(NexxoUtil.GetDictionaryValueIfExists(customerLookUpCriteria, "DateOfBirth"));

				MsgData msgData = new MsgData()
				{
					ssn = NexxoUtil.GetDictionaryValueIfExists(customerLookUpCriteria, "SSN"),
					cardNum = NexxoUtil.GetDictionaryValueIfExists(customerLookUpCriteria, "CardNumber"),
					AcctNum = NexxoUtil.GetDictionaryValueIfExists(customerLookUpCriteria, "AccountNumber"),
					AcctType = string.IsNullOrWhiteSpace(NexxoUtil.GetDictionaryValueIfExists(customerLookUpCriteria, "AccountNumber")) ? null : NexxoUtil.GetDictionaryValueIfExists(customerLookUpCriteria, "AccountType"),
					dob = string.IsNullOrWhiteSpace(NexxoUtil.GetDictionaryValueIfExists(customerLookUpCriteria, "DateOfBirth")) ? null : dateOfBirth.ToString("yyyyMMdd"),
					ClientCustId = NexxoUtil.GetDictionaryValueIfExists(customerLookUpCriteria, "ClientPAN"),
				};

				if (msgData.AcctNum != null)
				{
					msgData.AcctNum = msgData.AcctNum.Length < 18 ? msgData.AcctNum.PadLeft(18, '0') : msgData.AcctNum;
				}

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

				NLogger.Info(string.Format("Connecting to financial connect service - ZeoFindCustomer - SearchCustomerBySSN,AccountNumber,CardNumber - {0} : {1} - Start", this.GetType().Name, DateTime.Now.ToString()));

				ZeoCustomerService_MsgSetPortTypeClient client = new ZeoCustomerService_MsgSetPortTypeClient();

				response = client.FindCustomer(request);

				NLogger.Info(string.Format("Retrieved the customer details - ZeoFindCustomer - {0} : {1} ", this.GetType().Name, DateTime.Now.ToString()));

				CustInfo customerInfoList = new CustInfo();

				if (response != null)
				{
					svcResponse[0].MsgData1 = response.Svc1 != null ? response.Svc1[0].MsgData1 : null;

					if (!string.IsNullOrEmpty(response.ErrCde) && response.ErrCde != "0")
					{
						string errorMessage = response.ErrMsg != null ? response.ErrMsg : "";
                        if (response.ErrCde == "2")//AL-4348
                        {
                            throw new TCIFProviderException(response.ErrCde, errorMessage);
                        }
                        else
                        {

                            throw new TCIFProviderException(response.ErrCde, "Customer sync failed: " + errorMessage);
                        }
					}
				}


				List<Account> tcisAccounts = new List<Account>();

				if (svcResponse[0].MsgData1 != null)
				{
					tcisAccounts = FetchAllResponseMapper(svcResponse[0].MsgData1.ToList(), channelPartnerId);
				}
				else
				{
					throw new ClientCustomerException(ClientCustomerException.TCIS_CUSTOMERDATA_NOTFOUND, "Customer data not found.");
				}
				return tcisAccounts;
			}
			catch (TCIFProviderException ex)
			{
                if (ex.Code == "2")//AL-4348
                {
                    throw new ClientCustomerException(ClientCustomerException.TCIS_FIND_CUSTOMER_HARDSTOP, ex);
                }
                else
                {
                    throw new ClientCustomerException(ClientCustomerException.TCIS_FIND_CUSTOMER_FAILED, ex);
                }
			}

		}


		public string CreateCustomer(Account customer, MGIContext mgiContext)
		{

			ZeoCustomerRegReq request = new ZeoCustomerRegReq();
			string clintCustId = string.Empty;

			Svc2[] baserequest = new Svc2[1];
			baserequest[0] = new Svc2();

			Security[] security = new Security[1];

			security[0] = new Security();

			security[0].BasicAuth = GetBasicAuth(mgiContext);

			baserequest[0].Security = security;

			baserequest[0].SvcParms = GetsvcParams();

			CustInfo[] custInfo = new CustInfo[1];

			custInfo[0] = new CustInfo()
			{
				PersonalInfo = Mapper.Map<PersonalInfo>(customer),
				Identification = Mapper.Map<Identification>(customer),
				EmploymentInfo = Mapper.Map<EmploymentInfo>(customer)
			};

			custInfo[0].PersonalInfo.NexxoPan = mgiContext.CXECustomerId.ToString();
			custInfo[0].PersonalInfo.tcfPAN = NexxoUtil.GetDictionaryValueIfExists(mgiContext.Context, "StatusCode");

			baserequest[0].MsgData = custInfo;

			request.Svc = baserequest[0];

			string ErrMsg = string.Empty;
			string mtvnSvcVer = string.Empty;
			string msgUUID = string.Empty;

			ZeoCustomerResp response = new ZeoCustomerResp();

			try
			{
				NLogger.Info(string.Format("Connecting to financial connect service - ZeoCustomerReg -  {0} : {1} - Start", this.GetType().Name, DateTime.Now.ToString()));

				ZeoCustomerService_MsgSetPortTypeClient client = new ZeoCustomerService_MsgSetPortTypeClient();
				response = client.CustomerRegistration(request);
				NLogger.Info(string.Format("Register the customer details - ZeoCustomerReg - {0} : {1} ", this.GetType().Name, DateTime.Now.ToString()));

				string errorMessage = string.IsNullOrWhiteSpace(response.ErrMsg) ? GenericErrorMessage : response.ErrMsg;

				if (response == null)
				{
					throw new ClientCustomerException(ClientCustomerException.TCIS_REGISTRATION_SOFTSTOP, "TimeOut:  Error in RCIF service call");
				}
				else if (response != null && response.ErrCde == SoftStop)
				{
					if (!string.IsNullOrWhiteSpace(response.Status))
					{
						throw new TCIFProviderException(response.Status, errorMessage);
					}
					throw new ClientCustomerException(ClientCustomerException.TCIS_REGISTRATION_SOFTSTOP, errorMessage);
				}
				else if (response != null && response.ErrCde == HardStop)
				{
					throw new ClientCustomerException(ClientCustomerException.TCIS_REGISTRATION_HARDSTOP, errorMessage);
				}

				if (response.Svc1 != null && response.Svc1[0] != null && response.Svc1[0].MsgData1 != null && response.Svc1[0].MsgData1[0] != null)
				{
					clintCustId = response.Svc1[0].MsgData1[0].PersonalInfo != null ? response.Svc1[0].MsgData1[0].PersonalInfo.ClientCustId : string.Empty;
				}

			}
			catch (TCIFProviderException proExc)
			{
				throw new ClientCustomerException(ClientCustomerException.TCIS_REGISTRATION_SOFTSTOP, proExc);
			}
			return clintCustId;
		}

		#region Private Methods
		/// <summary>
		///  Map the response from RCIF API call to a collection of customer profiles.
		/// </summary>
		/// <param name="responseData"></param>
		/// <param name="channelPartnerId"></param>
		/// <returns></returns>
		private BasicAuth GetBasicAuth(MGIContext cxnContext)
		{
			Dictionary<string, object> ssoAttributes = new Dictionary<string, object>();

			if (cxnContext.Context.ContainsKey("SSOAttributes") && cxnContext.Context["SSOAttributes"] != null)
			{
				ssoAttributes = (Dictionary<string, object>)cxnContext.Context["SSOAttributes"];
			}

			BasicAuth basicAuth = new BasicAuth()
			{
				UsrID = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "UserID"),
				tellerNbr = Convert.ToInt32(NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "TellerNum")),
				tellerNbrSpecified = true,
				branchNbr = Convert.ToInt32(NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "BranchNum")),
				branchNbrSpecified = true,
				bankNbr = Convert.ToInt32(NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "BankNum")),
				bankNbrSpecified = true,
				lawsonID = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "LawsonID"),
				lu = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "LU"),
				cashDrawer = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "CashDrawer"),
				amPmInd = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "AmPmInd"),
				BusinessDate = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "BusinessDate"),
				HostName = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "MachineName"),
				subClientNodeID = NexxoUtil.GetDictionaryValueIfExists(ssoAttributes, "DPSBranchID")
			};

			return basicAuth;
		}

		/// <summary>
		///  Map the response from RCIF API call to a collection of customer profiles.
		/// </summary>
		/// <param name="responseData"></param>
		/// <param name="channelPartnerId"></param>
		/// <returns></returns>
		private List<Account> FetchAllResponseMapper(List<CustInfo> customerInfoList, long channelPartnerId)
		{
			List<Account> customerprofiles = new List<Account>();
			foreach (CustInfo custInfo in customerInfoList)
			{
				Account account = new Account();

				account = Mapper.Map<Account>(custInfo);


				customerprofiles.Add(account);
			}

			return customerprofiles;
		}


		private string MapToAlloyPhoneTypes(string mapPhoneType)
		{
			string idType = string.Empty;
			Dictionary<string, string> phoneTypes = new Dictionary<string, string>()
			{
				{"H", "Home"},
				{"W", "Work"},
				{"M", "Cell"},
				{"O", "Other"},
			};

			if (phoneTypes.ContainsKey(mapPhoneType))
			{
				idType = phoneTypes[mapPhoneType];
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



		private string MapToAlloyIdType(string clientIdType)
		{
			string idType = string.Empty;
			Dictionary<string, string> idMappying = new Dictionary<string, string>()
			{
				{"D", "DRIVER'S LICENSE"},
				{"U", "MILITARY ID"},
				{"P", "PASSPORT"},
				{"S", "U.S. STATE IDENTITY CARD"},
				{"M", "MATRICULA CONSULAR"}
			};

			if (idMappying.ContainsKey(clientIdType))
			{
				idType = idMappying[clientIdType];
			}
			return idType;
		}

		private string MapToClientIdType(string AlloyIdType)
		{
			string idType = string.Empty;

			Dictionary<string, string> idMappying = new Dictionary<string, string>()
			{
				{"DRIVER'S LICENSE"						,"D"		},
				{"MILITARY ID"							,"U"		},
				{"PASSPORT"								,"P"		},
				{"U.S. STATE IDENTITY CARD"				,"S"		},
				{"MATRICULA CONSULAR"					,"M"		}
			};

			if (idMappying.ContainsKey(AlloyIdType))
			{
				idType = idMappying[AlloyIdType];
			}
			return idType;
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

		#endregion
	}

}
