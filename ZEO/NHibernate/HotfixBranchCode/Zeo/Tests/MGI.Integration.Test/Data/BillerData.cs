using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Integration.Test.Data
{
	public partial class IntegrationTestData
	{
		public static BillPayment GetBillerInformation(ChannelPartner channelPartner)
		{
			BillPayment billpayment = new BillPayment()
			{
				BillerName = "REGIONAL ACCEPTANCE",
                AccountNumber = "1234561830",
            };
			return billpayment;
		}
	}
}
