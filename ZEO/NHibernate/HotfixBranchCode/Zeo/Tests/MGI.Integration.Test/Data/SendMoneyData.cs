using MGI.Channel.Shared.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Channel.DMS.Server.Data;

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
				case "SYNOVUS":
				case "TCF":
				case "CARVER":
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
				case "MGI":
				case "REDSTONE":
					{
						receiver.DeliveryMethod = "WILL_CALL";
						receiver.FirstName = "ReceiverFName";
						receiver.LastName = "LName";
						receiver.PickupCountry = "USA";
						receiver.PickupState_Province = "AZ";
						receiver.SecurityQuestion = "Test Question";
						receiver.SecurityAnswer = "Test Answer";
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
