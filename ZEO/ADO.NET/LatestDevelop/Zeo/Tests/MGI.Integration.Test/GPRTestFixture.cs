using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using static TCF.Zeo.Common.Util.Helper;

namespace MGI.Integration.Test
{
    [TestFixture]
    public partial class AlloyIntegrationTestFixture
    {

        #region GPRCard Intergration Test Cases

        [TestCase("TCF")]
        public void DoGPRLoad(string channelPartnerName)
        {
            var result = LoadCard(channelPartnerName);

            Assert.That(result, Is.Not.Null);
        }

        [TestCase("TCF")]
        public void DoGPRWithdraw(string channelPartnerName)
        {
            var checkoutStatus = Withdraw(channelPartnerName);

            Assert.That(checkoutStatus, Is.EqualTo(HelperShoppingCartCheckoutStatus.Completed));
        }

        [TestCase("TCF")]
        public void DoGPRLoadTransactions(string channelPartnerName)
        {
            var result = LoadCardTransactions(channelPartnerName);

            Assert.That(result, Is.Not.Null);
        }

        [TestCase("TCF")]
        public void DoGPRWithdrawTransactions(string channelPartnerName)
        {
            var result = WithdrawCardTransactions(channelPartnerName);

            Assert.That(result, Is.Not.Null);
        }

        #endregion

        #region Private

        private HelperShoppingCartCheckoutStatus LoadCard(string channelPartnerName)
        {
            Response response;

            CustomerSession customersession = InitiateCustomerSession(channelPartnerName, true);

            response = client.GetZeoContextForCustomer(customersession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            PerformLoadToCard(zeoContext);

            response = client.ShoppingCartCheckout(0, HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);

            return (HelperShoppingCartCheckoutStatus)response.Result;
        }

        private HelperShoppingCartCheckoutStatus Withdraw(string channelPartnerName)
        {
            Response response;

            decimal withdrawAmount = GetRandomAmount();

            CustomerSession customersession = InitiateCustomerSession(channelPartnerName, true);

            response = client.GetZeoContextForCustomer(customersession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            PerformWithdrawFromCard(withdrawAmount, zeoContext);

            response = client.ShoppingCartCheckout(withdrawAmount, HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);

            HelperShoppingCartCheckoutStatus status = (HelperShoppingCartCheckoutStatus)response.Result;

            if (status == HelperShoppingCartCheckoutStatus.CashOverCounter)
            {
                response = client.ShoppingCartCheckout(withdrawAmount, status, zeoContext);

                status = (HelperShoppingCartCheckoutStatus)response.Result;
            }
            return status;
        }

        private HelperShoppingCartCheckoutStatus LoadCardTransactions(string channelPartnerName)
        {
            Response response;

            decimal withdrawAmount = GetRandomAmount();

            CustomerSession customersession = InitiateCustomerSession(channelPartnerName, true);

            response = client.GetZeoContextForCustomer(customersession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            PerformBillPay(channelPartnerName, customersession.CustomerSessionId, zeoContext);

            PerformLoadToCard(zeoContext);

            response = client.GetShoppingCart(customersession.CustomerSessionId, zeoContext);

            ShoppingCart cart = response.Result as ShoppingCart;

            decimal amount = cart.GprCardTotal + cart.BillTotal + cart.MoneyTransfeTotal;

            client.CashIn(amount, zeoContext);

            response = client.ShoppingCartCheckout(amount, HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);

            return (HelperShoppingCartCheckoutStatus)response.Result;
        }

        private HelperShoppingCartCheckoutStatus WithdrawCardTransactions(string channelPartnerName)
        {
            Response response;

            decimal withdrawAmount = GetRandomAmount();

            decimal dueToCustomer = 0;

            CustomerSession customersession = InitiateCustomerSession(channelPartnerName, true);

            response = client.GetZeoContextForCustomer(customersession.CustomerSessionId, zeoContext);

            zeoContext = response.Result as ZeoContext;

            PerformCheckProcess(customersession.CustomerSessionId, zeoContext);

            PerformWithdrawFromCard(withdrawAmount, zeoContext);

            response = client.GetShoppingCart(customersession.CustomerSessionId, zeoContext);

            ShoppingCart cart = response.Result as ShoppingCart;

            dueToCustomer = cart.CheckTotal + cart.GprCardTotal;

            response = client.ShoppingCartCheckout(dueToCustomer, HelperShoppingCartCheckoutStatus.InitialCheckout, zeoContext);

            HelperShoppingCartCheckoutStatus status = (HelperShoppingCartCheckoutStatus)response.Result;

            if (status == HelperShoppingCartCheckoutStatus.CashOverCounter)
            {
                response = client.ShoppingCartCheckout(withdrawAmount, status, zeoContext);

                status = (HelperShoppingCartCheckoutStatus)response.Result;
            }
            return status;
        }

        private void PerformLoadToCard(ZeoContext context)
        {
            decimal loadAmount = GetRandomAmount();

            Funds fund = new Funds() { Amount = loadAmount };

            client.LoadFunds(fund, context);
        }

        private void PerformWithdrawFromCard(decimal withdrawAmount, ZeoContext context)
        {
            Funds fund = new Funds() { Amount = withdrawAmount };

            client.WithdrawFunds(fund, context);
        }

        #endregion

    }
}
