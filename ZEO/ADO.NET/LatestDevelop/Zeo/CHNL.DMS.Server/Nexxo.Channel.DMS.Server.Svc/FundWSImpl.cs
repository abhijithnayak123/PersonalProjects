using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;
using System.ServiceModel;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IFundsProcessorService
	{
		public Response Add(long customerSessionId, FundsProcessorAccount fundsAccount, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.Add(customerSessionId, fundsAccount, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response AuthenticateCard(long customerSessionId, string cardNumber, string pin, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.AuthenticateCard(customerSessionId, cardNumber, pin, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response Load(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.Load(customerSessionId, funds, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response ActivateGPRCard(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.ActivateGPRCard(customerSessionId, funds, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response Withdraw(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.Withdraw(customerSessionId, funds, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetBalance(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetBalance(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response Lookup(string accountIdentifier, string agentSessionId, string customerSessionId, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		public Response LookupForPAN(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.LookupForPAN(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetFee(long customerSessionId, decimal amount, FundType fundsType, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetFee(customerSessionId, amount, fundsType, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		// just changing this so it will build
		public Response UpdateFundAmount(long customerSessionId, long cxeFundTrxId, decimal amount, FundType fundType, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.UpdateFundAmount(customerSessionId, cxeFundTrxId, amount, fundType, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
			//return DesktopEngine.UpdateFundAmount( customerSessionId, cxeFundTrxId, amount );
		}

		public Response GetMinimumLoadAmount(long customerSessionId, bool initialLoad, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetMinimumLoadAmount(customerSessionId, initialLoad, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}


		public Response GetCardTransactionHistory(long customerSessionId, Data.TransactionHistoryRequest request, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetCardTransactionHistory(customerSessionId, request, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response CloseAccount(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.CloseAccount(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response UpdateCardStatus(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.UpdateCardStatus(customerSessionId, cardMaintenanceInfo, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response ReplaceCard(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.ReplaceCard(customerSessionId, cardMaintenanceInfo, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetShippingTypes(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetShippingTypes(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetShippingFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetShippingFee(customerSessionId, cardMaintenanceInfo, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response AssociateCard(long customerSessionId, FundsProcessorAccount fundsAccount, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.AssociateCard(customerSessionId, fundsAccount, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetFundFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetFundFee(customerSessionId, cardMaintenanceInfo, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response IssueAddOnCard(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.IssueAddOnCard(customerSessionId, funds, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}
	}
}
