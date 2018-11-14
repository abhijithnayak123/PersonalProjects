using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using static TCF.Zeo.Common.Util.Helper;

namespace MGI.Integration.Test
{
	[TestFixture]
	public partial class AlloyIntegrationTestFixture : BaseFixture
	{
        #region ReceiveMoney Integration Test cases
        [TestCase("TCF")]
		public void DoReceiveMoney(string channelPartnerName)
		{
            ZeoContext zeoContext;
            CustomerSession customerSession;
            TransactionHistory trx;
            MoneyTransferTransaction moneytransferTransaction;
            SendMoneyFetchMTCN(channelPartnerName, out zeoContext, out customerSession, out trx, out moneytransferTransaction);
            ShoppingCartCheckoutStatus shoppingCartCheckoutStatus = ReceiveMoneyCheckout(zeoContext,customerSession, moneytransferTransaction);
            Assert.AreEqual(ShoppingCartCheckoutStatus.Completed, shoppingCartCheckoutStatus);
            client.UpdateCounterId(zeoContext);
        }
        #endregion
    }
}
