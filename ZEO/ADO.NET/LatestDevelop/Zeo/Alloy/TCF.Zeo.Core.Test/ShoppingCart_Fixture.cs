using System;
using System.Collections.Generic;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using TCF.Zeo.Common.Data;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class ShoppingCart_Fixture
    {
        IShoppingCartService shoppingCartService { set; get; }
        ZeoContext alloycontext = new ZeoContext();
        [Test]
        public void AddShoppingCartTransaction_Test()
        {
            long customerSessionId = 1000000005;
            long transactionId = 1000007;
            int productId = 5;
            shoppingCartService = new ShoppingCartServiceImpl();
            bool result = shoppingCartService.AddShoppingCartTransaction(customerSessionId, transactionId, productId, DateTime.Now.ToString(), alloycontext);

            Assert.IsTrue(result);
        }

        [Test]
        public void CanCloseCustomerSession_Test()
        {
            long customerSessionId = 1000000005;

            shoppingCartService = new ShoppingCartServiceImpl();

            bool result = shoppingCartService.CanCloseCustomerSession(customerSessionId, "", alloycontext);

            Assert.IsTrue(result);
        }

        [Test]
        public void Can_GetShoppingCart()
        {
            long customerSessionId = 1000000005;

            shoppingCartService = new ShoppingCartServiceImpl();

            ShoppingCart cart = shoppingCartService.GetShoppingCart(customerSessionId, alloycontext);

            Assert.IsNotNull(cart);
        }

        [Test]
        public void CashOutAndUpdateCartStatus_Test()
        {
            bool isException = false;
            try
            {
                int cartId = 10;
                decimal amount = 50;
                long customerSessionId = 1000000005;
                shoppingCartService = new ShoppingCartServiceImpl();
                shoppingCartService.CashOutAndUpdateCartStatus(cartId, amount, customerSessionId, (int)Helper.ShoppingCartCheckoutStatus.CashOverCounter, "", alloycontext);
            }
            catch (Exception ex)
            {
                isException = true;
            }

            Assert.IsFalse(isException);
        }

        [Test]
        public void GetShoppingCartCheckOutDetails_Test()
        {
            long customerSessionId = 1000000005;
            int channelPartnerId = 34;
            shoppingCartService = new ShoppingCartServiceImpl();
            ShoppingCartCheckOut result = shoppingCartService.GetShoppingCartCheckOutDetails(customerSessionId, channelPartnerId,false, alloycontext);

            Assert.IsNotNull(result);
        }

        [Test]
        public void IsShoppingCartEmpty_Test()
        {
            long customerSessionId = 1000000005;
            shoppingCartService = new ShoppingCartServiceImpl();
            bool result = shoppingCartService.IsShoppingCartEmpty(customerSessionId, alloycontext);

            Assert.IsTrue(result);
        }

        [Test]
        public void ParkShoppingCartTransaction_Test()
        {
            long transactionId = 1000007;
            int productId = 5;
            long customerSessionId = 1000000005;
            shoppingCartService = new ShoppingCartServiceImpl();
            bool result = shoppingCartService.ParkShoppingCartTransaction(customerSessionId, transactionId, productId, "", alloycontext);

            Assert.IsTrue(result);
        }

        [Test]
        public void RemoveShoppingCartTransaction_Test()
        {
            long transactionId = 100000004;
            int productId = 5;
            shoppingCartService = new ShoppingCartServiceImpl();
            bool isRemoved = shoppingCartService.RemoveShoppingCartTransaction(transactionId, productId, alloycontext);

            Assert.IsTrue(isRemoved);
        }

        [Test]
        public void GetResubmitTransactions()
        {
            long customerSessionId = 1000000012;
            int productId = 2;
            shoppingCartService = new ShoppingCartServiceImpl();
            List<long> transactions = shoppingCartService.GetResubmitTransactions(productId, customerSessionId, alloycontext);

            Assert.IsNotEmpty(transactions);
        }
    }
}
