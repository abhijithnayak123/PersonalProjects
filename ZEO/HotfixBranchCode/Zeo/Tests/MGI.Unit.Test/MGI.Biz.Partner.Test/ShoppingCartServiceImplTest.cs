using NUnit.Framework;
using MGI.Common.Util;
using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using PNTRContract = MGI.Core.Partner.Contract;
using PNTRData = MGI.Core.Partner.Data;
using System.Linq;
using Moq;


namespace MGI.Unit.Test.MGI.Biz.Partner.Test
{
    [TestFixture]
    public class ShoppingCartServiceImplTest : BaseClass_Fixture
    {
		public IShoppingCartService BIZPartnerShoppingCartService { get; set; }
		public PNTRContract.ICustomerSessionService CustomerSessionService { private get; set; }

		[Test]
		public void Can_Add_Check_Transaction_To_ShoppingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long checkId = 1000000001;

			BIZPartnerShoppingCartService.AddCheck(customerSessionId, checkId, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.AreEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.CartItemStatus == PNTRData.ShoppingCartItemStatus.Added && a.Transaction.Type == 2), 1);
		}

		[Test]
		public void Can_Add_Fund_To_ShoppingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long fundId = 1000000001;

			BIZPartnerShoppingCartService.AddFunds(customerSessionId, fundId, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.AreEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.CartItemStatus == PNTRData.ShoppingCartItemStatus.Added && a.Transaction.Type == 3), 1);
		}

		[Test]
		public void Can_Add_Bill_Pay_To_ShoppingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long billPayId = 1000000001;

			BIZPartnerShoppingCartService.AddBillPay(customerSessionId, billPayId, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.AreEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.CartItemStatus == PNTRData.ShoppingCartItemStatus.Added && a.Transaction.Type == 4), 1);
		}

		[Test]
		public void Can_Add_MoneyOrder_To_ShopingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long moneyOrderId = 1000000001;

			BIZPartnerShoppingCartService.AddMoneyOrder(customerSessionId, moneyOrderId, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.AreEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.CartItemStatus == PNTRData.ShoppingCartItemStatus.Added && a.Transaction.Type == 5), 1);
		}

		[Test]
		public void Can_Add_MoneyTransfer_To_ShoppingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long billPayId = 1000000001;

			BIZPartnerShoppingCartService.AddMoneyTransfer(customerSessionId, billPayId, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.AreEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.CartItemStatus == PNTRData.ShoppingCartItemStatus.Added && a.Transaction.Type == 6), 1);
		}

		[Test]
		public void Can_Add_Cash_To_ShoppingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long billPayId = 1000000000;

			BIZPartnerShoppingCartService.AddCash(customerSessionId, billPayId, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.AreEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.CartItemStatus == PNTRData.ShoppingCartItemStatus.Added && a.Transaction.Type == 1), 1);
		}

		[Test]
		public void Can_Remove_Check_From_ShoppingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long checkId = 1000000001;

			BIZPartnerShoppingCartService.AddCheck(customerSessionId, checkId, mgiContext);
			BIZPartnerShoppingCartService.RemoveCheck(customerSessionId, checkId, false, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.GreaterOrEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.CartItemStatus == PNTRData.ShoppingCartItemStatus.Removed && a.Transaction.Type == 2), 1);
		}

		[Test]
		public void Can_Remove_BillPay_From_ShoppingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long billPayId = 1000000001;

			BIZPartnerShoppingCartService.AddBillPay(customerSessionId, billPayId, mgiContext);
			BIZPartnerShoppingCartService.RemoveBillPay(customerSessionId, billPayId, false, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.GreaterOrEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.CartItemStatus == PNTRData.ShoppingCartItemStatus.Removed && a.Transaction.Type == 4), 1);
		}

		[Test]
		public void Can_Remove_Fund_From_ShoppingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long fundId = 1000000001;

			BIZPartnerShoppingCartService.AddFunds(customerSessionId, fundId, mgiContext);
			BIZPartnerShoppingCartService.RemoveFunds(customerSessionId, fundId, false, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.GreaterOrEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.CartItemStatus == PNTRData.ShoppingCartItemStatus.Removed && a.Transaction.Type == 3), 1);
		}

		[Test]
		public void Can_Remove_MoneyOrder_From_ShoppingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long moneyOrderId = 1000000001;

			BIZPartnerShoppingCartService.AddMoneyOrder(customerSessionId, moneyOrderId, mgiContext);
			BIZPartnerShoppingCartService.RemoveMoneyOrder(customerSessionId, moneyOrderId, false, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.GreaterOrEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.CartItemStatus == PNTRData.ShoppingCartItemStatus.Removed && a.Transaction.Type == 5), 1);
		}

		[Test]
		public void Can_Remove_MoneyTransfer_From_ShoppingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long moneyTransferId = 1000000001;

			BIZPartnerShoppingCartService.AddMoneyTransfer(customerSessionId, moneyTransferId, mgiContext);
			BIZPartnerShoppingCartService.RemoveMoneyTransfer(customerSessionId, moneyTransferId, false, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.GreaterOrEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.CartItemStatus == PNTRData.ShoppingCartItemStatus.Removed && a.Transaction.Type == 6), 1);
		}

		[Test]
		public void Can_Park_Check_Transaction()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long checkId = 1000000001;

			BIZPartnerShoppingCartService.AddCheck(customerSessionId, checkId, mgiContext);
			BIZPartnerShoppingCartService.ParkCheck(customerSessionId, checkId, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.AreEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.ShoppingCart.IsParked == true && a.Transaction.Type == 2), 2);
		}

		[Test]
		public void Can_Park_Fund_Transaction()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long fundId = 1000000001;

			BIZPartnerShoppingCartService.AddFunds(customerSessionId, fundId, mgiContext);
			BIZPartnerShoppingCartService.ParkFunds(customerSessionId, fundId, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.AreEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.ShoppingCart.IsParked == true && a.Transaction.Type == 3), 2);
		}

		[Test]
		public void Can_Park_MoneyOrder_Transaction()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long moneyOrderId = 1000000001;

			BIZPartnerShoppingCartService.AddMoneyOrder(customerSessionId, moneyOrderId, mgiContext);
			BIZPartnerShoppingCartService.ParkMoneyOrder(customerSessionId, moneyOrderId, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.AreEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.ShoppingCart.IsParked == true && a.Transaction.Type == 5), 2);
		}

		[Test]
		public void Can_Park_MoneyTransfer_Transaction()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long moneyTransferId = 1000000001;

			BIZPartnerShoppingCartService.AddMoneyTransfer(customerSessionId, moneyTransferId, mgiContext);
			BIZPartnerShoppingCartService.ParkMoneyTransfer(customerSessionId, moneyTransferId, mgiContext);
			PNTRData.CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

			Assert.AreEqual(session.ActiveShoppingCart.ShoppingCartTransactions.Count(a => a.ShoppingCart.IsParked == true && a.Transaction.Type == 6), 2);
		}

		[Test]
		public void Can_Get_ShoppingCart()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long moneyTransferId = 1000000001;

			BIZPartnerShoppingCartService.AddMoneyTransfer(customerSessionId, moneyTransferId, mgiContext);
			ShoppingCart shoppingCart = BIZPartnerShoppingCartService.Get(customerSessionId, mgiContext);

			Assert.IsNotNull(shoppingCart);
		}

		[Test]
		public void Can_Get_ShoppingCart_By_Id()
		{
			long customerSessionId = 1000000005;
			MGIContext mgiContext = new MGIContext() { };
			long shoppingCartId = 1000000001;

			ShoppingCart shoppingCart = BIZPartnerShoppingCartService.GetCartById(customerSessionId, shoppingCartId, mgiContext);

			Assert.IsNotNull(shoppingCart);
		}

		[Test]
		public void Can_CheckOut_Transaction()
		{
			long customerSessionId = 1000000011;
			decimal cashToCustomer = 100;
			string cardNumber = "";
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			long moneyTransferId = 1000000001;

			BIZPartnerShoppingCartService.AddMoneyTransfer(customerSessionId, moneyTransferId, mgiContext);
			ShoppingCartCheckoutStatus status = BIZPartnerShoppingCartService.Checkout(customerSessionId, cashToCustomer, cardNumber, ShoppingCartCheckoutStatus.Completed, mgiContext);

			Assert.AreEqual(status, ShoppingCartCheckoutStatus.Completed);
		}

		[Test]
		public void Can_Get_ShoppingCart_PostFlush_MoneyTransfer()
		{
			long customerSessionId = 1000000007;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			BIZPartnerShoppingCartService.PostFlush(customerSessionId, mgiContext);
		}

		[Test]
		public void Can_Get_ShoppingCart_IsReferralApplicable_ReferralSectionEnable()
		{
			long customerSessionId = 1000000007;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			bool isRes = BIZPartnerShoppingCartService.IsReferralApplicable(customerSessionId, mgiContext);

			Assert.True(isRes);
		}

		[Test]
		public void Can_CheckOut_Transaction_Cash()
		{
			long customerSessionId = 1000000012;
			decimal cashToCustomer = 100;
			string cardNumber = "";
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			long moneyTransferId = 1000000001;

			BIZPartnerShoppingCartService.AddMoneyTransfer(customerSessionId, moneyTransferId, mgiContext);
			ShoppingCartCheckoutStatus status = BIZPartnerShoppingCartService.Checkout(customerSessionId, cashToCustomer, cardNumber, ShoppingCartCheckoutStatus.Completed, mgiContext);
			Assert.AreEqual(status, ShoppingCartCheckoutStatus.Completed);
		}
		
		 [Test]
        public void Can_Get_Parked_Transactions()
        {
            var s = BIZPartnerShoppingCartService.GetAllParkedShoppingCartTransactions();
            Assert.AreNotEqual(s, 0);
        }
    }
}
