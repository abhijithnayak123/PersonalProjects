using MGI.Biz.Partner.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Integration.Test
{
	[TestFixture]
	public partial class AlloyIntegrationTestFixture : BaseFixture
	{
		[TestCase("Synovus")]
		[TestCase("Carver")]
		//[TestCase("TCF")]
		public void DoReceiveMoney(string channelPartnerName)
		{			
			Channel.DMS.Server.Data.MGIContext context;
			Desktop client = new Desktop();
			CustomerSession customerSession;
			MGI.Channel.DMS.Server.Data.TransactionHistory trx;
			MGI.Channel.Shared.Server.Data.MoneyTransferTransaction moneytransferTransaction;
			SendMoneyFetchMTCN(channelPartnerName, out context, out customerSession, out trx, out moneytransferTransaction);
			
			MGI.Channel.Shared.Server.Data.ShoppingCartCheckoutStatus shoppingCartCheckoutStatus = ReceiveMoneyCheckout(context, client, customerSession, moneytransferTransaction);
			Assert.AreEqual(MGI.Channel.Shared.Server.Data.ShoppingCartCheckoutStatus.Completed, shoppingCartCheckoutStatus);
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);			
		}
	}
}
