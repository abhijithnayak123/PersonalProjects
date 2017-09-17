using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

//using MGI.Biz.Partner.Impl;
using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Contract;

using MGI.Core.Partner.Data;
//using MGI.Core.Partner.Contract;

using Moq;
using Spring.Context;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Test
{
	[TestFixture]
	public class ShoppingCartTests : AbstractPartnerTest
	{
		
		IShoppingCartService ShoppingCartService { get; set; }
		private MGI.Core.Partner.Contract.ICustomerSessionService _csSvc;
		public MGI.Core.Partner.Contract.ICustomerSessionService CustomerSessionService { set { _csSvc = value; } }
		private static string IShopping_Cart_Service = "BIZPartnerShoppingCartService";
        public MGIContext mgiContext { get; set; }
		[SetUp]
		public void Setup()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			ShoppingCartService = (IShoppingCartService)ctx.GetObject(IShopping_Cart_Service);
		}

		[Test]
		public void GetCartTest()
		{

            Biz.Partner.Data.ShoppingCart cartResult = ShoppingCartService.Get(1000000438, mgiContext);

			Assert.IsTrue(cartResult.BillTotal >= 0);
			Assert.IsTrue(cartResult.MoneyTransferTotal >= 0);
		}

		[Test]
		public void GetCartById()
		{
            Partner.Data.ShoppingCart cart = ShoppingCartService.GetCartById(100000438, 262, mgiContext);
			Assert.IsTrue(cart != null);
		}


		[Test]
		public void GetParkingCartTest()
		{
            Biz.Partner.Data.ShoppingCart cartResult = ShoppingCartService.GetCartById(100000438, 29, mgiContext);

			Assert.IsTrue(cartResult != null);

			CustomerSession _customerSession = _csSvc.Lookup(1000000025);

			MGI.Core.Partner.Data.ShoppingCart _shoppingCart = _csSvc.GetParkingShoppingCart(_customerSession);

			Assert.IsTrue(_shoppingCart != null);
		}		

		[Test]
		public void Cannot_GetCartForExistingCustomerSessionId()
		{
			long customersessionId = 1000002366;

            Biz.Partner.Data.ShoppingCart shoppingCart = ShoppingCartService.Get(customersessionId, mgiContext);

			Assert.IsNull(shoppingCart);
		}

		[Test]
		public void Can_CxnTransactionDetails()
		{
			long customersessionId = 1000000022;
			decimal cashToCustomer = 23;           
			string cardNumber = "";

            mgiContext = new MGIContext();
            mgiContext.LocationName = "test";
            mgiContext.TimeZone = TimeZone.CurrentTimeZone.StandardName;
		    mgiContext.CheckUserName ="9900004";
			mgiContext.CheckPassword = "9900004";
       	    mgiContext.ChannelPartnerId = 34;
			mgiContext.IsReferral = true;

            ShoppingCartCheckoutStatus result = ShoppingCartService.Checkout(customersessionId, cashToCustomer, cardNumber, MGI.Biz.Partner.Data.ShoppingCartCheckoutStatus.Completed, mgiContext);

			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.EqualTo(ShoppingCartCheckoutStatus.Completed));
		}
	}
}
