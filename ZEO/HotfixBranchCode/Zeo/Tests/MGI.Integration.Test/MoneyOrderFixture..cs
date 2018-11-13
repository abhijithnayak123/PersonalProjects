using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Integration.Test;
using MGI.Integration.Test.Data;


namespace MGI.Integration.Test
{
    [TestFixture]
	public partial class AlloyIntegrationTestFixture 
    {

		[TestCase("Carver")]
		[TestCase("Synovus")]
		public void MoneyOrder(string channelPartnerName)
		{
			var tranHistory = DoMoneyOrder(channelPartnerName);
			Assert.That(tranHistory, Is.Not.Null);
		}

		[TestCase("Carver")]
		[TestCase("Synovus")]
		public void MoneyOrderPark(string channelPartnerName)
		{
			bool isMoneyOrderParked = ParkMoneyOrder(channelPartnerName);
			Assert.That(isMoneyOrderParked, Is.True);
		}

		[TestCase("Carver")]
		[TestCase("Synovus")]
		public void MoneyOrderParkUnPark(string channelPartnerName)
		{
			List<TransactionHistory> tranHistory = null;
			tranHistory = UnParkMoneyOrder(channelPartnerName);
			Assert.That(tranHistory, Is.Not.Null);
		}

		[TestCase("Carver")]
		[TestCase("Synovus")]
		public void MoneyOrderRemove(string channelPartnerName)
		{
			bool isMoneyOrderRemoved = RemoveMoneyOrder(channelPartnerName);
			Assert.That(isMoneyOrderRemoved, Is.True);
		}
		
		
		/// <summary>
		/// DoMoneyOrder
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		private List<TransactionHistory> DoMoneyOrder(string channelPartnerName)
        {

			Desktop client = new Desktop();
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);

			PerformMoneyOrder(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);
			ShoppingCartCheckoutStatus status = client.Checkout(customerSession.CustomerSessionId, 101, "", ShoppingCartCheckoutStatus.InitialCheckout, context);
            
			if (status == ShoppingCartCheckoutStatus.MOPrinting)
            {
				client.UpdateMoneyOrderStatus(long.Parse(customerSession.CustomerSessionId), long.Parse(cart.MoneyOrders.FirstOrDefault().Id), (int)MGI.Core.CXE.Data.TransactionStates.Processing, context);
				status = client.Checkout(customerSession.CustomerSessionId, 101,"", ShoppingCartCheckoutStatus.FinalCheckout, context);
            }
			TransactionHistoryRequest request = new TransactionHistoryRequest()
			{
				DateRange = 1
			};

			var items = client.GetTransactionHistory(Convert.ToInt64(customerSession.CustomerSessionId), customerSession.Customer.CIN, string.Empty, string.Empty, DateTime.Now.AddDays(-1), context);
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return items;
        }

		/// <summary>
		/// PerformMoneyOrder
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <param name="customerSessionId"></param>
		/// <param name="context"></param>
		private void PerformMoneyOrder(string channelPartnerName, long customerSessionId, MGI.Channel.DMS.Server.Data.MGIContext context)
		{
			MoneyOrderData moneyOrderData = new MoneyOrderData { Amount = GetRandomAmount() };

			TransactionFee fee = client.GetMoneyOrderFee(customerSessionId, moneyOrderData, context);

			MoneyOrderPurchase moneyOrderPurchase = new MoneyOrderPurchase { Amount = moneyOrderData.Amount, Fee = fee.NetFee };

			MoneyOrder moneyOrder = client.PurchaseMoneyOrder(customerSessionId, moneyOrderPurchase, context);			
		}

		/// <summary>
		/// ParkMoneyOrder
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		private bool ParkMoneyOrder(string channelPartnerName)
		{
			Desktop client = new Desktop();
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			PerformMoneyOrder(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);

			bool isMoneyOrderParked = false;
			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);

			client.ParkMoneyOrder(Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(cart.MoneyOrders.FirstOrDefault().Id));
			cart = client.ShoppingCart(customerSession.CustomerSessionId);

			if (cart.MoneyOrders.Count == 0)
			{
				isMoneyOrderParked = true;
			}
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return isMoneyOrderParked;
		}

		/// <summary>
		/// UnparkMoneyOrder
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		private List<TransactionHistory> UnParkMoneyOrder(string channelPartnerName)
		{
			Desktop client = new Desktop();
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);

			//ShoppingCartCheckoutStatus status = client.Checkout(customerSession.CustomerSessionId, Convert.ToDecimal(150), Convert.ToDecimal(150), 0, string.Empty, ShoppingCartCheckoutStatus.FinalCheckout, context);

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);
			ShoppingCartCheckoutStatus status = client.Checkout(customerSession.CustomerSessionId, 101,"", ShoppingCartCheckoutStatus.InitialCheckout, context);

			if (status == ShoppingCartCheckoutStatus.MOPrinting)
			{
				client.UpdateMoneyOrderStatus(long.Parse(customerSession.CustomerSessionId), long.Parse(cart.MoneyOrders.FirstOrDefault().Id), (int)MGI.Core.CXE.Data.TransactionStates.Processing, context);
				status = client.Checkout(customerSession.CustomerSessionId, 101, "", ShoppingCartCheckoutStatus.FinalCheckout, context);
			}

			TransactionHistoryRequest request = new TransactionHistoryRequest()
			{
				DateRange = 1
			};

			var items = client.GetTransactionHistory(Convert.ToInt64(customerSession.CustomerSessionId), customerSession.Customer.CIN, string.Empty, string.Empty, DateTime.Now.AddDays(-1), context);
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return items;
		}

		/// <summary>
		/// RemoveMoneyOrder
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		private bool RemoveMoneyOrder(string channelPartnerName)
		{
			Desktop client = new Desktop();
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			PerformMoneyOrder(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);

			bool isMoneyOrderRemoved = false;
			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);

			client.RemoveMoneyOrder(Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(cart.MoneyOrders.FirstOrDefault().Id));
			cart = client.ShoppingCart(customerSession.CustomerSessionId);
			if (cart.MoneyOrders.Count == 0)
			{
				isMoneyOrderRemoved = true;
			}
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return isMoneyOrderRemoved;
		}
	}
}
