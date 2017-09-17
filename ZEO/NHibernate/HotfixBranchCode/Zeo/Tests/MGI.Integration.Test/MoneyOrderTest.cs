using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;


namespace MGI.Integration.Test
{
    [TestFixture]
    class MoneyOrderTest : BaseFixture
    {
        [SetUp]
        public void Setup()
        {
            Client = new Desktop();
        }
        [Test]
        public void TestMoneyOrder()
        {
			GetChannelPartnerDataCarver();

			AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "Raj", new DateTime(1996, 04, 18));

			CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

            MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
            {
                Amount = 1000,
                Fee = (decimal)0.5
            };

			MoneyOrder moneyOrder = Client.PurchaseMoneyOrder(long.Parse(CustomerSession.CustomerSessionId), moneyOrderPurchase, MgiContext);

			MgiContext.IsReferral = false;

			ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
			
			ShoppingCartCheckoutStatus status = Client.Checkout(CustomerSession.CustomerSessionId, 101, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
            
			if (status == ShoppingCartCheckoutStatus.MOPrinting)
            {
				Client.UpdateMoneyOrderStatus(long.Parse(CustomerSession.CustomerSessionId), long.Parse(moneyOrder.Id), (int)MGI.Core.CXE.Data.TransactionStates.Processing, MgiContext);
				status = Client.Checkout(CustomerSession.CustomerSessionId, 101, "", ShoppingCartCheckoutStatus.FinalCheckout, MgiContext);
            }
			
			cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);

            Assert.AreEqual(ShoppingCartCheckoutStatus.Completed, status);
        }

    }
}
