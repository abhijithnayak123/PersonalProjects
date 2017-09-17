using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Core.Partner.Impl;
using MGI.Cxn.Customer.Contract;
using MGI.Cxn.Customer.TCIS.Data;
using MGI.Cxn.Customer.TCIS.RCIFService;
using MGI.Cxn.Customer.Data;
using AutoMapper;
using MGI.Common.Util;
using System.Globalization;


namespace MGI.Unit.Test.MockData
{
    class MockClientCustomerServiceTCF : IClientCustomerService
    {
        List<Cxn.Customer.Data.CustomerProfile> cstmrs = new List<Cxn.Customer.Data.CustomerProfile>();
        
        private const string PRODCODE = "ZEOSVC";
        private const string APPL_CD = "SVC";
        private const string TaxCdSSN = "S";
        private const string TaxCdITIN = "I";

        private const string SoftStop = "1";
        private const string HardStop = "2";
        public MockClientCustomerServiceTCF()
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
						if(!string.IsNullOrWhiteSpace(s.MiddleName))
						{ 
							d.MName = s.MiddleName;
						}
					});

			Mapper.CreateMap<Account, Identification>()
				.ForMember(x => x.dob, o => o.MapFrom(s => s.DateOfBirth != null ? s.DateOfBirth.Value.ToString("yyyyMMdd") : null))
				.ForMember(x => x.maiden, o => o.MapFrom(s => s.MothersMaidenName))
				.ForMember(x => x.idType, o => o.MapFrom(s => s.GovernmentIDType != null ? MapToClientIdType(s.GovernmentIDType) : null))
				.ForMember(x => x.idIssuer, o => o.MapFrom(s => s.IDIssuingState))
				.ForMember(x => x.idIssuerCountry, o => o.MapFrom(s => s.IDIssuingCountry))
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
							d.IDIssuingCountry = s.Identification.idIssuerCountry;
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
							d.Occupation = !string.IsNullOrEmpty(s.EmploymentInfo.Occupation) ? s.EmploymentInfo.Occupation.TrimStart('0') : string.Empty;
							d.OccupationDescription = s.EmploymentInfo.OccDesc;
							d.EmployerName = s.EmploymentInfo.EmployerName;
							d.EmployerPhone = s.EmploymentInfo.EmployerPhoneNum;
						}

					});
            //gateway mapping
            Mapper.CreateMap<Account, CustomerProfile>()
           .ForMember(x => x.CustInd, o => o.MapFrom(s => s.TcfCustInd));
            Mapper.CreateMap<CustomerProfile, Account>()
                  .ForMember(x => x.ProfileStatus, opt => opt.Ignore())
                                  .ForMember(x => x.TcfCustInd, o => o.MapFrom(s => s.CustInd))
                  .ForMember(x => x.PartnerAccountNumber, opt => opt.Ignore())
                  .ForMember(x => x.RelationshipAccountNumber, opt => opt.Ignore());
		
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

        public Cxn.Customer.Data.CustomerProfile Fetch(Dictionary<string, object> search)
        {
            throw new NotImplementedException();
        }

        public List<Cxn.Customer.Data.CustomerProfile> FetchAll(Dictionary<string, object> customerLookUpCriteria, Dictionary<string, object> cxnContext)
        {
              long channelPartnerId = Convert.ToInt64(cxnContext["ChannelPartnerId"]);

                CustInfo customerInfoList = new CustInfo() {

                    EmploymentInfo = new EmploymentInfo() { 
                        Occupation="ACCOUNTANT",
                        OccDesc="",
                        EmployerName="",
                        EmployerPhoneNum=""
                    },
                    PersonalInfo = new PersonalInfo() {
                        FName = "SIJO",
                        LName = "THOMAS",
                        MName="",
                        Addr1 = "111 Anza Blvd",
                        Addr2 = "suite 200",
                        City="burlingame",
                        State="California",
                        zip="94010",
                        Ph1="7048908997",
                        Ph1Type1="Home",
                        Ph1Prov="",
                        Ph2="",
                        Ph2Type2="",
                        Ph2Prov="",
                        email="",
                        CustType="",
                        ssn = "865123212",
                        TaxCd="",
                        gen="",
                        ClientCustId="",
                        gender="M",
                        tcfCustind="",
                        tcfPAN="",
                        PRODCODE=""

                    },
                    Identification = new Identification() {
                        dob="19801010",//should be yyyymmdd format
                        maiden="mom",
                        idNbr = "K3210987",
                        idType = "D",
                        idIssueDate = "20121010",//should be yyyymmdd format
                        idExpDate = "20221010",//should be yyyymmdd format
                        idIssuer="California",
                        idIssuerCountry = "UNITED STATES",
                        citizenshipCountry1="US"
                    }
                };
               
                List<Account> tcisAccounts = new List<Account>();

                List<CustInfo> custList = new List<CustInfo>();
                custList.Add(customerInfoList);

                tcisAccounts = FetchAllResponseMapper(custList, channelPartnerId);
                return Mapper.Map<List<Account>, List<CustomerProfile>>(tcisAccounts);           
        }

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


        public long Add(Cxn.Customer.Data.CustomerProfile customer, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public void Update(string id, Cxn.Customer.Data.CustomerProfile customer, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public long AddAccount(Cxn.Customer.Data.CustomerProfile account, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public void ValidateCustomerStatus(long CXNId, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public long AddCXNAccount(Cxn.Customer.Data.CustomerProfile customer, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public Common.Util.ProfileStatus GetClientProfileStatus(long cxnAccountId, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public string GetClientCustID(long cxnAccountId, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }

        public bool GetCustInd(long cxnAccountId, Dictionary<string, object> context)
        {
            throw new NotImplementedException();
        }
    }
}
