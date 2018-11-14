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

			Response customerResponse = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

            CustomerSession = (CustomerSession)customerResponse.Result;

            MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase
            {
                Amount = 1000,
                Fee = (decimal)0.5
            };

			Response purchaseMOResponse = Client.PurchaseMoneyOrder(long.Parse(CustomerSession.CustomerSessionId), moneyOrderPurchase, MgiContext);
			MoneyOrder moneyOrder = purchaseMOResponse.Result as MoneyOrder;

			MgiContext.IsReferral = false;

			Response cartResponse = Client.ShoppingCart(CustomerSession.CustomerSessionId);
			if(VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;
			Response statusResponse = Client.Checkout(CustomerSession.CustomerSessionId, 101, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
            if(VerifyException(statusResponse)) throw new Exception(statusResponse.Error.Details);
            ShoppingCartCheckoutStatus status = (ShoppingCartCheckoutStatus)statusResponse.Result;
			if (status == ShoppingCartCheckoutStatus.MOPrinting)
            {
				Client.UpdateMoneyOrderStatus(long.Parse(CustomerSession.CustomerSessionId), long.Parse(moneyOrder.Id), (int)MGI.Core.CXE.Data.TransactionStates.Processing, MgiContext);
				statusResponse = Client.Checkout(CustomerSession.CustomerSessionId, 101, "", ShoppingCartCheckoutStatus.FinalCheckout, MgiContext);
                if(VerifyException(statusResponse)) throw new Exception(statusResponse.Error.Details);
                status = (ShoppingCartCheckoutStatus)statusResponse.Result;
            }
			
			cartResponse = Client.ShoppingCart(CustomerSession.CustomerSessionId);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            cart = cartResponse.Result as ShoppingCart;
            Assert.AreEqual(ShoppingCartCheckoutStatus.Completed, status);
        }

    }
}
