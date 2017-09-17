using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using System;

namespace MGI.Integration.Test.Data
{
    public partial class IntegrationTestData
    {

        public static CustomerSearchCriteria GetSearchCriteria(ChannelPartner channelpartner, bool IsGprCard = false)
        {
            CustomerSearchCriteria criteria;

            if (!IsGprCard)
            {
                criteria = new CustomerSearchCriteria();
                {
                    criteria.Lastname = channelpartner.Name;
                    criteria.DateOfBirth = new DateTime(1950, 10, 10);
                };
            }
            else
            {
                criteria = new CustomerSearchCriteria();
                {
                    criteria.Lastname = "Depp";
                    criteria.DateOfBirth = new DateTime(1950, 10, 10);
                };
            }
            return criteria;
        }

        public static Location LocationData(ChannelPartner channelPartner)
        {
            Location location = new Location()
            {
                IsActive = true,
                LocationIdentifier = RandomNumber(9),
                ChannelPartnerId = Convert.ToInt32(channelPartner.Id),
                LocationName = "IT_" + channelPartner.Name,
                Address1 = "Basavanagudi",
                Address2 = "Bangalore",
                BankID = "300",
                BranchID = "120",
                City = "Bangalore",
                NoOfCounterIDs = 3,
                State = "KA",
                TimezoneID = "Central Standard Time"
            };
            return location;
        }

        public static LocationProcessorCredentials LocationProcessorData(string channelPartnerName)
        {
            LocationProcessorCredentials processorCredential = new LocationProcessorCredentials();
            switch (channelPartnerName)
            {
                case "TCF":
                    processorCredential.UserName = "9900004";
                    processorCredential.Password = "9900004";
                    processorCredential.Identifier = "9900004";
                    break;
                default:
                    processorCredential.UserName = "13139925";
                    processorCredential.Password = "13139925";
                    processorCredential.Identifier = "13139925";
                    break;
            }

            return processorCredential;
        }

    }
}
