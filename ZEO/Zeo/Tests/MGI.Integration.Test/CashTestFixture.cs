using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;

namespace MGI.Integration.Test
{
    [TestFixture]
	public partial class AlloyIntegrationTestFixture 
    {


		[TestCase("TCF")]
		public void CashIn(string channelPartnerName)
		{
			var tranHistory = CashInTransaction(channelPartnerName);
			Assert.That(tranHistory, Is.Not.Null);
		}

		[TestCase("TCF")]
		public void CashInUpdate(string channelPartnerName)
		{
			bool isUpdated = UpdateCashIn(channelPartnerName);
			Assert.That(isUpdated, Is.True);
		}

		[TestCase("TCF")]
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

			//Desktop client = new Desktop();


			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
            Response response = client.GetZeoContextForAgent(Convert.ToInt64(customerSession.CustomerSessionId), zeoContext);
            zeoContext = response.Result as ZeoContext;

            PerformCashIn(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), zeoContext);

        
            Response cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId, zeoContext);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;


            TransactionHistoryRequest request = new TransactionHistoryRequest()
			{
				DateRange = 1
			};
            TransactionHistorySearchCriteria creteria = new TransactionHistorySearchCriteria()
            {
                DatePeriod = DateTime.Now,
                TransactionType = "BillPay",
                AgentId = zeoContext.AgentId,
                CustomerId = zeoContext.CustomerId,
                LocationName = zeoContext.LocationName,
                //TransactionId = zeoContext.id TrxId,
            };
            response = client.GetCustomerTransactions(creteria, zeoContext);
			var items = response.Result as List<TransactionHistory>;
			//context.IsAvailable = true;
			client.UpdateCounterId(zeoContext);
			return items;
        }

		/// <summary>
		/// PerformCashIn
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <param name="customerSessionId"></param>
		/// <param name="context"></param>
		private void PerformCashIn(string channelPartnerName, long customerSessionId, ZeoContext context)
		{
			
			CartCash cartCash = new CartCash { Amount = GetRandomAmount() };

			CashTransaction cashtransaction = new CashTransaction { Amount = cartCash.Amount, TransactionType = "" };
            Response response = client.GetZeoContextForAgent(Convert.ToInt64(customerSessionId), zeoContext);
            zeoContext = response.Result as ZeoContext;

            client.CashIn(cashtransaction.Amount, zeoContext);
		}

		/// <summary>
		/// Remove CashIn
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		private bool RemoveCashIn(string channelPartnerName)
		{
			//Desktop client = new Desktop();
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
            Response response = client.GetZeoContextForAgent(Convert.ToInt64(customerSession.CustomerSessionId), zeoContext);
            zeoContext = response.Result as ZeoContext;
            PerformCashIn(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), zeoContext);

			bool isCashInRemoved = false;
			Response cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId, zeoContext);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;
			client.RemoveCashIn(zeoContext);
			cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId, zeoContext);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            cart = cartResponse.Result as ShoppingCart;
			if (cart.Cash.Count == 0)
			{
				isCashInRemoved = true;
			}
			
			//context.IsAvailable = true;
			client.UpdateCounterId(zeoContext);
			return isCashInRemoved;
		}

		private bool UpdateCashIn(string channelPartnerName)
		{
			//Desktop client = new Desktop();
			CustomerSession customerSession = InitiateCustomerSession(channelPartnerName);
            Response response = client.GetZeoContextForCustomer(Convert.ToInt64(customerSession.CustomerSessionId), zeoContext);
            ZeoContext context = response.Result as ZeoContext;
            PerformCashIn(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);
			bool isCashInUpdated = false;
            Response cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId, context);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;
            client.UpdateOrCancelCashIn( 100, context);
            cartResponse = client.GetShoppingCart(customerSession.CustomerSessionId, context);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            cart = cartResponse.Result as ShoppingCart;
			if(cart.CashInTotal == 100)
			{
				isCashInUpdated = true;
			}			 
			return isCashInUpdated;
		}

	}
}
