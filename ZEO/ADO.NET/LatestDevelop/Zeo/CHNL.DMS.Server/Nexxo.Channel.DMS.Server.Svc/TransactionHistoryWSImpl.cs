using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using MGI.Common.Sys;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using SharedData = MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : ITransactionHistoryService
	{
        public Response GetTransactionHistory(long customerSessionId, long customerId, string transactionType, string location, DateTime dateRange, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetTransactionHistory(customerSessionId, customerId, transactionType, location, dateRange, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetAgentTransactionHistory(long agentSessionId, long? agentId, string transactionType, string location, bool showAll, long transactionId, int duration, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetAgentTransactionHistory(agentSessionId, agentId, transactionType, location, showAll, transactionId, duration, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetFundTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetFundTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetCheckTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetCheckTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetMoneyTransferTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetMoneyTransferTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response GetMoneyOrderTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetMoneyOrderTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetBillPayTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetCashTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetCashTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
	}
}
