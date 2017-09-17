using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;

namespace MGI.Integration.Test.Data
{
    public partial class IntegrationTestData
    {
        public static decimal cashCollected = 200;
        public static decimal previousCashCollected = 200;

		public static Receiver GetReceiverData(string channelPartnerName)
		{
			Receiver receiver = new Receiver();
			switch (channelPartnerName.ToUpper())
			{
				
				case "TCF":
			
					{
						receiver.DeliveryMethod = "000";
						receiver.FirstName = "FName";
						receiver.LastName = "LName";
						receiver.PickupCountry = "US";
						//receiver.PickupCity = "COLIMA";
						receiver.PickupState_Province = "CA";
						receiver.Status = "Active";
						break;
					}
				default:
					break;
			}
			return receiver;
		}


	}
}
