using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.WU.Common.Contract;
using MGI.Cxn.WU.Common.Data;
using System.Security.Cryptography.X509Certificates;
using MGI.Cxn.WU.Common.WUCardService;
using MGI.Cxn.WU.Common.WUCardLookupService;
using MGI.Common.Util;
using AutoMapper;
using MGI.Cxn.WU.Common.DASService;

namespace MGI.Cxn.WU.Common.Impl
{
    public class SimulatorIO : BaseIO, IWUCommonIO

    {
		public CardDetails WUCardEnrollment(Sender sender, PaymentDetails paymentDetails, MGIContext mgiContext)
        {
            CardDetails cardDetails = new CardDetails();
            cardDetails.AccountNumber = "501187574";
            cardDetails.ForiegnSystemId = "WGHH673600T";
			cardDetails.CounterId = mgiContext.WUCounterId;
            return cardDetails;
        }

		public List<AgentBanners> GetWUAgentBannerMsgs(MGIContext mgiContext)
        {
            
            List<AgentBanners> agentBanners = new List<AgentBanners>();
            agentBanners.Add(new AgentBanners() { ERR_CODE = "001", ERR_MESSAGE = "Mock Banner Message" });
            agentBanners.Add(new AgentBanners() { ERR_CODE = "002", ERR_MESSAGE = "FRAUDSTERS PRETENDING TO BE FROM WU OR YOUR CORPORATE IT/HQ ARE" });
            agentBanners.Add(new AgentBanners() { ERR_CODE = "003", ERR_MESSAGE = "CALLING AGENTS AND ASKING THEM TO ACCESS A WEBSITE WHICH" });
            agentBanners.Add(new AgentBanners() { ERR_CODE = "004", ERR_MESSAGE = "DOWNLOADS SOFTWARE THAT GIVES THEM CONTROL OF THE AGENTS'S PC." });
            agentBanners.Add(new AgentBanners() { ERR_CODE = "005", ERR_MESSAGE = "THIS WILL RESULT IN YOUR LOCATION BEING DEFRAUDED. HANG UP ON" });
            agentBanners.Add(new AgentBanners() { ERR_CODE = "006", ERR_MESSAGE = "THESE CALLERS." });


            return agentBanners;
        }

		public CardLookupDetails WUCardLookup(CardLookUpRequest wucardlookupreq, MGIContext mgiContext)
        {
            List<Sender> senderList = new List<Sender>();
            Sender sender = new Sender()
            {
                NameType = WUEnums.name_type.D,
                FirstName = "FirstName",
                LastName = "LastName",
                AddressAddrLine1 = "Wood Forest",
                AddressPostalCode = "90001",
                AddressState = "CALIFORNIA",
                AddressCity = "CA",
                PreferredCustomerAccountNumber = "501220607",
                ContactPhone = "9865232323"
            };
            senderList.Add(sender);
            CardLookupDetails cardLookupDetails = new CardLookupDetails()
            {
                Sender = new Sender[senderList.Count],
                WuCardTotalPointsEarned = "10"
            };
            cardLookupDetails.Sender = senderList.ToArray();
            return cardLookupDetails;
        }

		public CardLookupDetails WUCardLookupForCardNumber(CardLookUpRequest wucardlookupreq, MGIContext mgiContext)
        {
            List<Sender> senderList = new List<Sender>();
            Sender sender = new Sender()
            {
                NameType = WUEnums.name_type.D,
                FirstName = "FirstName",
                LastName = "LastName",
                AddressAddrLine1 = "Wood Forest",
                AddressPostalCode = "90001",
                AddressState = "CALIFORNIA",
                AddressCity = "CA",
                PreferredCustomerAccountNumber = "501220607",
                ContactPhone = "9865232323"
            };
            senderList.Add(sender);
            CardLookupDetails cardLookupDetails = new CardLookupDetails();
            cardLookupDetails.AccountNumber = "AMN138883";
            cardLookupDetails.Sender = senderList.ToArray();
            cardLookupDetails.ForiegnSystemId = "WGHH673600T";
            return cardLookupDetails;
        }

		public List<MGI.Cxn.WU.Common.Data.Receiver> WUPastBillersReceivers(long customerSessionId, CardLookUpRequest wucardlookupreq, MGIContext mgiContext)
        {
            List<MGI.Cxn.WU.Common.Data.Receiver> receiversList = new List<MGI.Cxn.WU.Common.Data.Receiver>();
            MGI.Cxn.WU.Common.Data.Receiver receiver = new MGI.Cxn.WU.Common.Data.Receiver()
            {
                NameType = WUEnums.name_type.C,
                BusinessName = "TESTING ONLY",
                Attention = "84523695",
                ReceiverIndexNumber = "005",
                Status = "true",
                CustomerId = 36481,
                Type = "Q",
                FirstName = "ANGELA",
                paternal_name = "CHEN",

                Address = new Address()
                {
                    item = new CountryCurrencyInfo()
                    {
                        country_code = "US",
                        currency_code = "USD"
                    }
                }

            };
            receiversList.Add(receiver);
            receiver = new MGI.Cxn.WU.Common.Data.Receiver()
            {
                NameType = WUEnums.name_type.D,
                BusinessName = "TESTING INTERNATIONAL",
                Attention = "x000027693557",
                ReceiverIndexNumber = "008",
                Status = "true",
                CustomerId = 36481,
                Type = "Q",
                FirstName = "ANGELA",
                paternal_name = "CHEN",
                SecondLastName = "T",
                Address = new Address()
                {
                    item = new CountryCurrencyInfo()
                    {
                        country_code = "US",
                        currency_code = "USD"
                    }
                }
            };
            receiversList.Add(receiver);

            return receiversList;
        }

		public WUBaseRequestResponse CreateRequest(long channelPartnerId, MGIContext mgiContext)
		{
			WUBaseRequestResponse request = new WUBaseRequestResponse();
			return request;
		}

		public CardInfo GetCardInfo(CardLookUpRequest wuCardLookupReq, MGIContext mgiContext)
		{
            CardInfo CardInfo = new CardInfo()
            {
                PromoCode = "A", TotalPointsEarned = "5"
            };
			return CardInfo;
		}
		
		/// <summary>
		/// Trim Occupation at 29 th Character
		/// </summary>
		/// <param name="occupation">string</param>
		/// <returns>string</returns>
		public string TrimOccupation(string occupation)
		{
			if (!string.IsNullOrEmpty(occupation) && occupation.Length > OCCUPATION_LENGTH)
			{
				return occupation.Substring(0, OCCUPATION_LENGTH);
			}
			else
			{
				return occupation;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="countryCode"></param>
		/// <returns></returns>
		public string GetCountryName(string countryCode)
		{
			string countryName = string.Empty;

			if (!string.IsNullOrWhiteSpace(countryCode))
			{
				var country = WUCountryRepo.FindBy(c => c.CountryCode == countryCode);
				if (country != null)
				{
					countryName = country.Name;
				}
			}

			return countryName;
		}


		public string GetGovtIDType(string idType)
		{
			Dictionary<string, string> govtIdTypeMapping = new Dictionary<string, string>()
			{
				{"SSN", "1"},
				{"DRIVER'S LICENSE", "1"},
				{"EMPLOYMENT AUTHORIZATION CARD (EAD)", "4"},
				{"GREEN CARD / PERMANENT RESIDENT CARD", "5"},
				{"MILITARY ID", "7"},
				{"PASSPORT", "2"},
				{"U.S. STATE IDENTITY CARD", "3"},
				{"INSTITUTO FEDERAL ELECTORAL", "8"},
				{"LICENCIA DE CONDUCIR", "6"},
				{"MATRICULA CONSULAR", "9"},
				{"NEW YORK BENEFITS ID","3"},
				{"NEW YORK CITY ID","3"}
			};

			return govtIdTypeMapping[idType];
		}

		/// <summary>
		/// US2054
		///WUSouth West Border Location States
		/// </summary>
		/// <param name="stateCode"></param>
		/// <returns></returns>
		public bool IsSWBState(string stateCode)
		{
			bool IsSWBState = false;
			string[] swbStates = { "AZ", "CA", "NM", "TX" };
			if (!string.IsNullOrWhiteSpace(stateCode))
			{
				IsSWBState = swbStates.Contains(stateCode);
			}
			return IsSWBState;
		}

		///// <summary>
		///// US2054
		///// </summary>
		///// <param name="context"></param>
		///// <returns></returns>
		public SwbFlaInfo BuildSwbFlaInfo(MGIContext mgiContext)
		{
			SwbFlaInfo swFlaInfo = null;
			swFlaInfo = new SwbFlaInfo()
			{
				SwbOperatorId = mgiContext.AgentId != 0 ? Convert.ToString(mgiContext.AgentId) : string.Empty,
				ReadPrivacyNoticeFlagSpecified = true,
				ReadPrivacyNoticeFlag = SwbFlaInfoReadPrivacyNoticeFlag.Y,
				FlagCertificationFlagSpecified = true,
				FlagCertificationFlag = SwbFlaInfoFlaCertificationFlag.Y
			};

			return swFlaInfo;
		}

		///// <summary>
		///// US2054
		///// </summary>
		///// <param name="context"></param>
		///// <returns></returns>
		public GeneralName BuildGeneralName(MGIContext mgiContext)
		{
			GeneralName name = null;
			name = new GeneralName()
			{
				Type = NameType.D,
				NameTypeSpecified = true,
				FirstName = mgiContext.AgentFirstName != null ? Convert.ToString(mgiContext.AgentFirstName) : string.Empty,
				LastName = mgiContext.AgentLastName != null ? Convert.ToString(mgiContext.AgentLastName) : string.Empty
			};
			return name;
		}
    }
}
