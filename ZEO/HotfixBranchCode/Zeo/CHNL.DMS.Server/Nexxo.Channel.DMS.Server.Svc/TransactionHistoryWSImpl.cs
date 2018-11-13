using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using System;
using System.Collections.Generic;
using SharedData = MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : ITransactionHistoryService
	{
        public List<TransactionHistory> GetTransactionHistory(long customerSessionId, long customerId, string transactionType, string location, DateTime dateRange, MGIContext mgiContext)
		{
			return DesktopEngine.GetTransactionHistory(customerSessionId, customerId, transactionType, location, dateRange, mgiContext);
		}

        public List<TransactionHistory> GetAgentTransactionHistory(long agentSessionId, long? agentId, string transactionType, string location, bool showAll, long transactionId, int duration, MGIContext mgiContext)
		{
			return DesktopEngine.GetAgentTransactionHistory(agentSessionId, agentId, transactionType, location, showAll, transactionId, duration, mgiContext);
		}

        public FundTransaction GetFundTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetFundTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
		}

        public CheckTransaction GetCheckTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCheckTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
		}

        public SharedData.MoneyTransferTransaction GetMoneyTransferTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetMoneyTransferTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
		}

        public MoneyOrderTransaction GetMoneyOrderTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetMoneyOrderTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
		}

        public BillPayTransaction GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetBillPayTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
		}

        public CashTransaction GetCashTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCashTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
		}
	}
}
