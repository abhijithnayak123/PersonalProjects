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


		[TestCase("Synovus")]
		[TestCase("Carver")]
		public void CashIn(string channelPartnerName)
		{
			var tranHistory = CashInTransaction(channelPartnerName);
			Assert.That(tranHistory, Is.Not.Null);
		}

		[TestCase("Synovus")]
		[TestCase("Carver")]
		public void CashInUpdate(string channelPartnerName)
		{
			bool isUpdated = UpdateCashIn(channelPartnerName);
			Assert.That(isUpdated, Is.True);
		}

		[TestCase("Synovus")]
		[TestCase("Carver")]
		public void CashInRemove(string channelPartnerName)
		{
			bool isRemoved = RemoveCashIn(channelPartnerName);
			Assert.That(isRemoved, Is.True);
		}

		
		/// <summary>
		/// CashInTransaction
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		private List<TransactionHistory> CashInTransaction(string channelPartnerName)
        {

			Desktop client = new Desktop();
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);

			PerformCashIn(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);
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
		/// PerformCashIn
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <param name="customerSessionId"></param>
		/// <param name="context"></param>
		private void PerformCashIn(string channelPartnerName, long customerSessionId, MGI.Channel.DMS.Server.Data.MGIContext context)
		{
			
			CartCash cartCash = new CartCash { Amount = GetRandomAmount() };

			CashTransaction cashtransaction = new CashTransaction { Amount = cartCash.Amount, TransactionType = "" };

			client.CashIn(customerSessionId, cashtransaction.Amount, context);
		}

		/// <summary>
		/// Remove CashIn
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		private bool RemoveCashIn(string channelPartnerName)
		{
			Desktop client = new Desktop();
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);

			PerformCashIn(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);

			bool isCashInRemoved = false;
			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);

			client.RemoveCashIn(Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(cart.Cash.FirstOrDefault().Id));
			cart = client.ShoppingCart(customerSession.CustomerSessionId);

			if (cart.Cash.Count == 0)
			{
				isCashInRemoved = true;
			}
			
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return isCashInRemoved;
		}

		private bool UpdateCashIn(string channelPartnerName)
		{
			Desktop client = new Desktop();
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
			PerformCashIn(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);
			bool isCashInUpdated = false;
			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);
			client.UpdateCash(Convert.ToInt64(customerSession.CustomerSessionId), Convert.ToInt64(cart.Cash.FirstOrDefault().Id), 100, context);
			cart = client.ShoppingCart(customerSession.CustomerSessionId);
			if(cart.CashInTotal == 100)
			{
				isCashInUpdated = true;
			}			 
			return isCashInUpdated;
		}

	}
}
