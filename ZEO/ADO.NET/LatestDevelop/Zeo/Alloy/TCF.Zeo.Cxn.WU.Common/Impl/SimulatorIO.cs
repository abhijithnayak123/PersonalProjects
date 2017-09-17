using TCF.Zeo.Cxn.WU.Common.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.WU.Common.Data;

namespace TCF.Zeo.Cxn.WU.Common.Impl
{
    public class SimulatorIO : BaseIO, IWUCommonIO
    {
        public GeneralName BuildGeneralName(ZeoContext context)
        {
            GeneralName name = null;
            name = new GeneralName()
            {
                Type = NameType.D,
                NameTypeSpecified = true,
                FirstName = context.AgentFirstName != null ? Convert.ToString(context.AgentFirstName) : string.Empty,
                LastName = context.AgentLastName != null ? Convert.ToString(context.AgentLastName) : string.Empty
            };
            return name;
        }

        public SwbFlaInfo BuildSwbFlaInfo(ZeoContext context)
        {
            SwbFlaInfo swFlaInfo = null;
            swFlaInfo = new SwbFlaInfo()
            {
                SwbOperatorId = context.AgentId != 0 ? Convert.ToString(context.AgentId) : string.Empty,
                ReadPrivacyNoticeFlagSpecified = true,
                ReadPrivacyNoticeFlag = SwbFlaInfoReadPrivacyNoticeFlag.Y,
                FlagCertificationFlagSpecified = true,
                FlagCertificationFlag = SwbFlaInfoFlaCertificationFlag.Y
            };

            return swFlaInfo;
        }

        public WUBaseRequestResponse CreateRequest(long channelPartnerId, ZeoContext context)
        {
            WUBaseRequestResponse request = new WUBaseRequestResponse()
            {
                Channel = new Channel()
                {
                    Name = "ESP",
                    Version = "9500"
                },
                ForeignRemoteSystem = new ForeignRemoteSystem()
            };
            return request;
        }

        public CardInfo GetCardInfo(CardLookUpRequest request, ZeoContext context)
        {
            CardInfo CardInfo = new CardInfo()
            {
                PromoCode = "A",
                TotalPointsEarned = "5"
            };
            return CardInfo;
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

        public List<AgentBanners> GetWUAgentBannerMsgs(ZeoContext context)
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

        public bool IsSWBState(string stateLocation)
        {
            bool IsSWBState = false;
            string[] swbStates = { "AZ", "CA", "NM", "TX" };
            if (!string.IsNullOrWhiteSpace(stateLocation))
            {
                IsSWBState = swbStates.Contains(stateLocation);
            }
            return IsSWBState;
        }

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

        public CardDetails WUCardEnrollment(WUEnrollmentRequest enrollmentReq, ZeoContext context)
        {
            CardDetails cardDetails = new CardDetails();
            cardDetails.AccountNumber = "501187574";
            cardDetails.ForiegnSystemId = "WGHH673600T";
            cardDetails.CounterId = context.WUCounterId;
            return cardDetails;
        }

        public CardLookupDetails WUCardLookup(CardLookUpRequest wucardlookupreq, ZeoContext context)
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

        public CardLookupDetails WUCardLookupForCardNumber(CardLookUpRequest wuCardLookupReq, ZeoContext context)
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

        public List<Receiver> WUPastBillersReceivers(CardLookUpRequest wucardlookupreq, ZeoContext context)
        {
            List<Receiver> receiversList = new List<Receiver>();
            Receiver receiver = new Receiver()
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
            receiver = new Receiver()
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
    }
}
