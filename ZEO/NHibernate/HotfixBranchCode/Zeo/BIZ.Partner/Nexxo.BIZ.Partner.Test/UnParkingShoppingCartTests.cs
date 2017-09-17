using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using MGI.Biz.Partner.Impl;
using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Contract;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;

using Moq;

namespace MGI.Biz.Partner.Test
{
    [TestFixture]
    public class UnParkingShoppingCartTests : AbstractPartnerTest 
    {
        private ShoppingCartServiceImpl svc = new ShoppingCartServiceImpl();
        private Mock<ICustomerSessionService> moqCSSvc = new Mock<ICustomerSessionService>();
        private Mock<MGI.Core.Partner.Contract.IShoppingCartService> moqSCSvc = new Mock<Core.Partner.Contract.IShoppingCartService>();

        [TestFixtureSetUp]
        public void testFixtureSetup()
        {
            svc.CustomerSessionService = moqCSSvc.Object;
            svc.ShoppingCartSvc = moqSCSvc.Object;
        }


        //[Test]
        //[ExpectedException]
        //public void Cannot_GetCartForCustomerSessionId()
        //{
        //    long customersessionId = GetRandomNumber();

        //    Biz.Partner.Data.ShoppingCart shoppingCart = svc.Get(customersessionId);

        //    Assert.IsNotNull(shoppingCart);
        //}

        //[Test]
        //public void Can_GetCartForCustomerSessionId()
        //{
        //    long customersessionId = 1000002365;

        //    CustomerSession cs = new CustomerSession();

        //    moqCSSvc.Setup(m => m.Lookup(customersessionId)).Returns(cs);

        //    Assert.IsTrue(cs.Id == customersessionId);

        //    //Biz.Partner.Data.ShoppingCart shoppingCart = cs.ActiveShoppingCart;

        //    //Assert.IsNotNull(shoppingCart);
        //}


        //[Test]
        //public void Cannot_GetCartForExistingCustomerSessionId()
        //{
        //    long customersessionId = 1000002366;

        //    Biz.Partner.Data.ShoppingCart shoppingCart = svc.Get(customersessionId);

        //    Assert.IsNull(shoppingCart);
        //}

        //private long GetRandomNumber()
        //{
        //    long min = 10000000000001;
        //    long max = 99999999999999;
        //    Random random = new Random();
        //    return min + random.Next() % (max - min); 
        //}



        [Test]
        public void GetCartTest()
        {
            Core.Partner.Data.CustomerSession cs = new CustomerSession();
            cs.AddShoppingCart();
            cs.ActiveShoppingCart.AddTransaction(new Core.Partner.Data.Transactions.BillPay(20, 1, "billpay 1", 1, 1, 1, 1, null, null));
            cs.ActiveShoppingCart.AddTransaction(new Core.Partner.Data.Transactions.MoneyTransfer(400, 1, "mt 1", 2, 1, 2, 1, null, null));
            cs.ActiveShoppingCart.AddTransaction(new Core.Partner.Data.Transactions.BillPay(20, 1, "billpay 2", 3, 1, 3, 1, null, null));

            moqCSSvc.Setup(m => m.Lookup(It.IsAny<long>())).Returns(cs);

            Biz.Partner.Data.ShoppingCart cartResult = svc.Get(1);

            Assert.IsTrue(cartResult.BillTotal == 42);
            Assert.IsTrue(cartResult.MoneyTransferTotal == 401);
        }
      
    }
}
