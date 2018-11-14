using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using static TCF.Zeo.Common.Util.Helper;

namespace MGI.Integration.Test
{
    [TestFixture]
    public partial class AlloyIntegrationTestFixture
    {

        [TestCase("TCF")]
        public void MoneyOrder(string channelPartnerName)
        {
            var result = DoMoneyOrder(channelPartnerName);
            Assert.That(result, Is.Not.Null);
        }

        [TestCase("TCF")]
        public void MoneyOrderPark(string channelPartnerName)
        {
            bool isMoneyOrderParked = ParkMoneyOrder(channelPartnerName);
            Assert.That(isMoneyOrderParked, Is.True);
        }

        [TestCase("TCF")]
        public void MoneyOrderUnPark(string channelPartnerName)
        {
            var result = UnParkMoneyOrder(channelPartnerName);
            Assert.That(result, Is.Not.Null);
        }

        [TestCase("TCF")]
        public void MoneyOrderRemove(string channelPartnerName)
        {
            bool isMoneyOrderRemoved = RemoveMoneyOrder(channelPartnerName);
            Assert.That(isMoneyOrderRemoved, Is.True);
        }


        private HelperShoppingCartCheckoutStatus DoMoneyOrder(string channelPartnerName)
        {
            Response response;

            CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);

            response = client.GetZeoContextForCustomer(customerSession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            PerformMoneyOrder(zeoContext);

            response = client.ShoppingCartCheckout(0, HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);

            HelperShoppingCartCheckoutStatus status = (HelperShoppingCartCheckoutStatus)response.Result;

            if (status == HelperShoppingCartCheckoutStatus.MOPrinting)
            {
                response = client.ShoppingCartCheckout(0, HelperShoppingCartCheckoutStatus.MOPrinting, zeoContext);
                status = (HelperShoppingCartCheckoutStatus)response.Result;
            }

            client.UpdateCounterId(zeoContext);

            return status;
        }

        private void PerformMoneyOrder(ZeoContext context)
        {
            decimal amount = GetRandomAmount();

            Response feeResponse = client.GetMoneyOrderFee(amount, context);

            TransactionFee fee = feeResponse.Result as TransactionFee;

            MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase { Amount = amount, Fee = fee.NetFee };

            Response purchaseMOResponse = client.PurchaseMoneyOrder(moneyOrderPurchase, context);
        }

        private bool ParkMoneyOrder(string channelPartnerName)
        {
            Response response;

            bool isMoneyOrderParked = false;

            CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);

            response = client.GetZeoContextForCustomer(customerSession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            PerformMoneyOrder(zeoContext);

            response = client.GetShoppingCart(customerSession.CustomerSessionId, zeoContext);
            ShoppingCart shoppingCart = response.Result as ShoppingCart;

            MoneyOrder moneyorder = shoppingCart.MoneyOrders.FirstOrDefault(x => x.State == 2);

            response = client.ParkShoppingCartTransaction(moneyorder.Id, (int)ProductType.MoneyOrder, zeoContext);

            isMoneyOrderParked = (bool)response.Result;

            client.UpdateCounterId(zeoContext);

            return isMoneyOrderParked;
        }

        private HelperShoppingCartCheckoutStatus UnParkMoneyOrder(string channelPartnerName)
        {
            Response response;

            CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);

            response = client.GetZeoContextForCustomer(customerSession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            response = client.ShoppingCartCheckout(0, HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);

            HelperShoppingCartCheckoutStatus status = (HelperShoppingCartCheckoutStatus)response.Result;

            if (status == HelperShoppingCartCheckoutStatus.MOPrinting)
            {
                response = client.ShoppingCartCheckout(0, HelperShoppingCartCheckoutStatus.MOPrinting, zeoContext);
                status = (HelperShoppingCartCheckoutStatus)response.Result;
            }

            client.UpdateCounterId(zeoContext);

            return status;
        }

        private bool RemoveMoneyOrder(string channelPartnerName)
        {
            Response response;

            bool isMoneyOrderRemoved = false;

            CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);

            response = client.GetZeoContextForCustomer(customerSession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            PerformMoneyOrder(zeoContext);

            response = client.GetShoppingCart(customerSession.CustomerSessionId, zeoContext);
            ShoppingCart shoppingCart = response.Result as ShoppingCart;

            MoneyOrder moneyorder = shoppingCart.MoneyOrders.FirstOrDefault(x => x.State == 2);

            response = client.RemoveMoneyOrder(moneyorder.Id, zeoContext);

            isMoneyOrderRemoved = (bool)response.Result;

            client.UpdateCounterId(zeoContext);

            return isMoneyOrderRemoved;
        }
    }
}
