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
    }
}
