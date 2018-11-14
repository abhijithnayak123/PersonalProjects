using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;

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
