using MGI.Channel.DMS.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Integration.Test.Data
{
	public partial class IntegrationTestData
	{

		public static CustomerSearchCriteria GetSearchCriteria(ChannelPartner channelpartner)
		{
			CustomerSearchCriteria criteria = new CustomerSearchCriteria();
			{
				criteria.LastName = channelpartner.Name;
				criteria.DateOfBirth = new DateTime(1950, 10, 10);
			};
			return criteria;
		}

		public static MGI.Channel.DMS.Server.Data.Location LocationData(ChannelPartner channelPartner)
		{
			MGI.Channel.DMS.Server.Data.Location location = new MGI.Channel.DMS.Server.Data.Location()
			{				
			    IsActive = true,
				LocationIdentifier = RandomNumber(9),   
			    ChannelPartnerId = channelPartner.Id,
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

		public static ProcessorCredential LocationProcessorData(string channelPartnerName)
		{
			ProcessorCredential processorCredential = new ProcessorCredential();
			switch(channelPartnerName)
			{
				case "TCF":
					processorCredential.UserName = "9900004";
					processorCredential.Password = "9900004";
					processorCredential.Identifier = "9900004";
					break;
				case "Synovus":
					processorCredential.UserName = "13139925";
					processorCredential.Password = "13139925";
					processorCredential.Identifier = "13139925";
					break;
				case "Carver":
					processorCredential.UserName = "13139925";
					processorCredential.Password = "13139925";
					processorCredential.Identifier = "13139925";
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
