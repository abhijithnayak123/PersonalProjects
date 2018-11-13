using System;
using System.Collections.Generic;
using NUnit.Framework;
using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using MGI.Unit.Test;
using MGI.Common.Util;
using MGI.Biz.Partner.Data.Transactions;

namespace MGI.Biz.Partner.Test
{
    [TestFixture]
    public class TransactionHistoryTest : BaseClass_Fixture
    {
        public ITransactionHistoryService BIZPartnerTransactionHistoryService { private get; set; }

		[Test]
		public void Can_Get_Transaction_History()
		{
			long customerSessionId = 1000000005;
			long customerId = 1000000000000000;
			string transactionType = "Check";
			string location ="Test";
			MGIContext mgiContext = new MGIContext() { };

			List<TransactionHistory> transactionHistory = BIZPartnerTransactionHistoryService.GetTransactionHistory(customerSessionId, customerId, transactionType, location, DateTime.Today, mgiContext);

			Assert.AreNotEqual(transactionHistory.Count, 0);
		}

		[Test]
		public void Can_Get_Transaction_History_Null_TransactionType()
		{
			long customerSessionId = 1000000005;
			long customerId = 1000000000000000;
			string transactionType = null;
			string location = "Test";
			MGIContext mgiContext = new MGIContext() { };

			List<TransactionHistory> transactionHistory = BIZPartnerTransactionHistoryService.GetTransactionHistory(customerSessionId, customerId, transactionType, location, DateTime.Today, mgiContext);

			Assert.AreNotEqual(transactionHistory.Count, 0);
		}

		[Test]
		public void Can_Get_Transaction_History_Null_Location()
		{
			long customerSessionId = 1000000005;
			long customerId = 1000000000000000;
			string transactionType = "Check";
			string location = null;
			MGIContext mgiContext = new MGIContext() { };

			List<TransactionHistory> transactionHistory = BIZPartnerTransactionHistoryService.GetTransactionHistory(customerSessionId, customerId, transactionType, location, DateTime.Today, mgiContext);

			Assert.AreNotEqual(transactionHistory.Count, 0);
		}

		[Test]
		public void Can_Get_Transaction_History_BY_Agent()
		{
			long agentSessionId = 1000000000;
			long agentId = 0; 
			string transactionType = "Check"; 
			string location = "Test";
			bool showAll = true;
			long transactionId = 1000000001;
			int duration = 15;
			MGIContext mgiContext = new MGIContext() { };

			List<TransactionHistory> transactionHistory = BIZPartnerTransactionHistoryService.GetTransactionHistory(agentSessionId, agentId, transactionType, location, showAll, transactionId, duration, mgiContext);

			Assert.AreNotEqual(transactionHistory.Count, 0);
		}

		[Test]
		public void Can_Get_Transaction_History_Without_TranId()
		{
			long agentSessionId = 1000000000;
			long agentId = 0;
			string transactionType = "Check";
			string location = "Test";
			bool showAll = true;
			long transactionId = 0;
			int duration = 15;
			MGIContext mgiContext = new MGIContext() { };

			List<TransactionHistory> transactionHistory = BIZPartnerTransactionHistoryService.GetTransactionHistory(agentSessionId, agentId, transactionType, location, showAll, transactionId, duration, mgiContext);

			Assert.AreNotEqual(transactionHistory.Count, 0);
		}

		[Test]
		public void Can_Get_Transaction_History_Null_AgentId()
		{
			long agentSessionId = 1000000000;
			string transactionType = "Check";
			string location = "Test";
			bool showAll = true
;
			long transactionId = 0;
			int duration = 15;
			MGIContext mgiContext = new MGIContext() { };

			List<TransactionHistory> transactionHistory = BIZPartnerTransactionHistoryService.GetTransactionHistory(agentSessionId, null, transactionType, location, showAll, transactionId, duration, mgiContext);

			Assert.AreNotEqual(transactionHistory.Count, 0);
		}

		[Test]
		public void Can_Get_Transaction_History_Null_Type()
		{
			long agentSessionId = 1000000000;
			string transactionType = null;
			long agentId = 0;
			string location = "Test";
			bool showAll = true;
			long transactionId = 0;
			int duration = 15;
			MGIContext mgiContext = new MGIContext() { };

			List<TransactionHistory> transactionHistory = BIZPartnerTransactionHistoryService.GetTransactionHistory(agentSessionId, agentId, transactionType, location, showAll, transactionId, duration, mgiContext);

			Assert.AreNotEqual(transactionHistory.Count, 0);
		}

		[Test]
		public void Can_Get_Fund_Transaction()
		{
			long agentSessionId = 1000000000; 
			long customerSessionId = 1000000005; 
			long transactionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			FundTransaction fundTransaction = BIZPartnerTransactionHistoryService.GetFundTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);

			Assert.IsNotNull(fundTransaction);
		}

		[Test]
		public void Can_Get_Check_Processing_Transaction()
		{
			long agentSessionId = 1000000000;
			long customerSessionId = 1000000005;
			long transactionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			CheckTransaction checkTransaction = BIZPartnerTransactionHistoryService.GetCheckTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);

			Assert.IsNotNull(checkTransaction);
		}
		[Test]
		public void Can_Get_MoneyOrder_Transaction()
		{
			long agentSessionId = 1000000000;
			long customerSessionId = 1000000005;
			long transactionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			MoneyOrderTransaction moneyOrderTransaction = BIZPartnerTransactionHistoryService.GetMoneyOrderTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);

			Assert.IsNotNull(moneyOrderTransaction);
		}

		[Test]
		public void Can_Get_MoneyTransfer_Transaction()
		{
			long agentSessionId = 1000000000;
			long customerSessionId = 1000000005;
			long transactionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			MoneyTransferTransaction moneyTransferTransaction = BIZPartnerTransactionHistoryService.GetMoneyTransferTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);

			Assert.IsNotNull(moneyTransferTransaction);
		}

		[Test]
		public void Can_Get_BillPay_Transaction()
		{
			long agentSessionId = 1000000000;
			long customerSessionId = 1000000005;
			long transactionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			BillPayTransaction billPayTransaction = BIZPartnerTransactionHistoryService.GetBillPayTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);

			Assert.IsNotNull(billPayTransaction);
		}

		[Test]
		public void Can_Get_Cash_Transaction()
		{
			long agentSessionId = 1000000000;
			long customerSessionId = 1000000005;
			long transactionId = 1000000000;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			CashTransaction cashTransaction = BIZPartnerTransactionHistoryService.GetCashTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);

			Assert.IsNotNull(cashTransaction);
		}

		[Test]
		public void Can_Get_Past_Transaction()
		{
			long agentSessionId = 1000000000;
			long customerSessionId = 1000000005;
			long customerId = 1000000000000000;
			string transactionType = "Check"; 
			MGIContext mgiContext = new MGIContext() { };

			List<PastTransaction> transactionHistory = BIZPartnerTransactionHistoryService.GetPastTransactions(agentSessionId, customerSessionId, customerId, transactionType, mgiContext);
			
			Assert.AreNotEqual(transactionHistory.Count, 0);
		}
    }
}
