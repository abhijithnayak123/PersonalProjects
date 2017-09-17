using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Integration.Test.Data;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Integration.Test
{
	[TestFixture]
	public partial class AlloyIntegrationTestFixture
	{
		#region Members
		private decimal LoadAmount  { get; set; }
		private decimal WithdrawAmount { get; set; }
		#endregion

		#region GPRCard Intergration Test Cases

		[TestCase("Synovus")]
		public void DoGPRLoadIT(string channelPartnerName)
		{
			var tranHistory = LoadCard(channelPartnerName);
			Assert.That(tranHistory.Count, Is.GreaterThan(0));
		}

		[TestCase("Synovus")]
		public void DoGPRWithdrawIT(string channelPartnerName)
		{
			var checkoutStatus = Withdraw(channelPartnerName);
			Assert.That(checkoutStatus, Is.EqualTo(ShoppingCartCheckoutStatus.CashOverCounter));
		}

		[TestCase("Synovus")]
		public void DoGPRLoadTransactionsIT(string channelPartnerName)
		{
			var tranHistory = LoadCardTransactions(channelPartnerName);
			Assert.That(tranHistory, Is.Not.Null);
		}

		[TestCase("Synovus")]
		public void DoGPRWithdrawTransactionsIT(string channelPartnerName)
		{
			var tranHistory = WithdrawCardTransactions(channelPartnerName);
			Assert.That(tranHistory, Is.Not.Null);
		}

		#endregion

		#region Private
		
		/// <summary>
		/// LoadCard
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		private List<TransactionHistory> LoadCard(string channelPartnerName)
		{
			var customerSession = GetGprCustomerSession(channelPartnerName);
			PerformLoadToCard(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);
			ShoppingCartCheckoutStatus status = client.Checkout(customerSession.CustomerSessionId, cart.GprCardTotal, customerSession.Customer.Fund.IsGPRCard ? customerSession.Customer.Fund.CardNumber : string.Empty, ShoppingCartCheckoutStatus.InitialCheckout, context);

			var items = client.GetTransactionHistory(Convert.ToInt64(customerSession.CustomerSessionId), customerSession.Customer.CIN, string.Empty, string.Empty, DateTime.Now.AddDays(-1), context);
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			items = items.Where(c => c.TransactionType == "Prepaid-Load").ToList();
			return items;
		}

		/// <summary>
		/// Withdraw
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		private ShoppingCartCheckoutStatus Withdraw(string channelPartnerName)
		{
			var customerSession = GetGprCustomerSession(channelPartnerName);
			PerformWithdrawFromCard(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);
			ShoppingCartCheckoutStatus status = client.Checkout(customerSession.CustomerSessionId, cart.GprCardTotal, customerSession.Customer.Fund.IsGPRCard ? customerSession.Customer.Fund.CardNumber : string.Empty, ShoppingCartCheckoutStatus.InitialCheckout, context);
			return status;
		}	
		
		/// <summary>
		/// LoadCardTractions
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		private List<TransactionHistory> LoadCardTransactions(string channelPartnerName)
		{
			var customerSession = GetGprCustomerSession(channelPartnerName);
			
			PerformBillPay(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), out context);

			PerformSendMoney(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), out context);

			PerformLoadToCard(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);	
			
			decimal amount = LoadAmount + cart.BillTotal + cart.MoneyTransfeTotal;

			client.CashIn(Convert.ToInt64(customerSession.CustomerSessionId),amount, context);
			
			ShoppingCartCheckoutStatus status = client.Checkout(customerSession.CustomerSessionId, 0, customerSession.Customer.Fund.IsGPRCard ? customerSession.Customer.Fund.CardNumber : string.Empty, ShoppingCartCheckoutStatus.InitialCheckout, context);
						
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
		/// WithdrawCardTransactions
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		private List<TransactionHistory> WithdrawCardTransactions(string channelPartnerName)
		{
			var customerSession = GetGprCustomerSession(channelPartnerName);

			PerformCheckProcess(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);

			PerformWithdrawFromCard(channelPartnerName, Convert.ToInt64(customerSession.CustomerSessionId), context);

			ShoppingCart cart = client.ShoppingCart(customerSession.CustomerSessionId);

			ShoppingCartCheckoutStatus status = client.Checkout(customerSession.CustomerSessionId, cart.GprCardTotal, customerSession.Customer.Fund.IsGPRCard ? customerSession.Customer.Fund.CardNumber : string.Empty, ShoppingCartCheckoutStatus.InitialCheckout, context);			
			
			TransactionHistoryRequest request = new TransactionHistoryRequest()
			{
				DateRange = 1
			};

			var items = client.GetTransactionHistory(Convert.ToInt64(customerSession.CustomerSessionId), customerSession.Customer.CIN, string.Empty, string.Empty, DateTime.Now.AddDays(-1), context);
			context.IsAvailable = true;
			client.UpdateCounterId(Convert.ToInt64(customerSession.CustomerSessionId), context);
			return items;
		}

		private CustomerSession GetGprCustomerSession(string channelPartnerName)
		{
			AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName);

			CustomerSearchCriteria customerSearch = new CustomerSearchCriteria() { LastName = "LEWIS", DateOfBirth = new DateTime(1980, 02, 02) };

			CustomerSearchResult[] customers = client.SearchCustomers(agentSession.SessionId, customerSearch, context);

			if (customers.Length != 0)
			{
				AlloyId = long.Parse(customers[0].AlloyID);
			}
			CustomerSession customerSession = client.InitiateCustomerSession(agentSession.SessionId, AlloyId, 3, context);

			return customerSession;
		}

		private void PerformLoadToCard(string channelPartnerName, long CustomerSessionId, MGI.Channel.DMS.Server.Data.MGIContext context)
		{
			LoadAmount = GetRandomAmount();
			decimal LoadFee = client.GetFundsFee(CustomerSessionId, LoadAmount, FundType.Credit, context).NetFee;
			Funds fund = new Funds() { Amount = LoadAmount, Fee = LoadFee };
			client.Load(Convert.ToString(CustomerSessionId), fund, context);
		}

		private void PerformWithdrawFromCard(string channelPartnerName, long CustomerSessionId, MGI.Channel.DMS.Server.Data.MGIContext context)
		{
			WithdrawAmount = GetRandomAmount();
			decimal withdrawFee = client.GetFundsFee(CustomerSessionId, WithdrawAmount, FundType.Debit, context).NetFee;
			Funds fund = new Funds() { Amount = WithdrawAmount, Fee = withdrawFee };
			client.Withdraw(Convert.ToString(CustomerSessionId), fund, context);
		}

		#endregion

	}
}
