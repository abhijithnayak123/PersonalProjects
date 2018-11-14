using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;
using System.ServiceModel;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IReceiptsService
	{

		public Response GetCheckReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			Response response = new Response();

			try
			{
				response = DesktopEngine.GetCheckReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetFundsReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{

				response = DesktopEngine.GetFundsReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{

				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetMoneyTransferReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{

				response = DesktopEngine.GetMoneyTransferReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{

				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetDoddFrankReceipt(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{

				response = DesktopEngine.GetDoddFrankReceipt(agentSessionId, customerSessionId, transactionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{

				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetCheckDeclinedReceiptData(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{

				response = DesktopEngine.GetCheckDeclinedReceiptData(agentSessionId, customerSessionId, transactionId, mgiContext);

			}
			catch (FaultException<NexxoSOAPFault> ex)
			{

				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetMoneyOrderReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{

				response = DesktopEngine.GetMoneyOrderReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{

				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetBillPayReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			Response response = new Response();

			try
			{
				response = DesktopEngine.GetBillPayReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{

				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetSummaryReceipt(long customerSessionId, long cartId, MGIContext mgiContext)
		{
			Response response = new Response();

			try
			{
				response = DesktopEngine.GetSummaryReceipt(customerSessionId, cartId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{

				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetSummaryReceipt(long agentSessionId, long customerSessionId, long transactionId, string transactiontype, MGIContext mgiContext)
		{
			Response response = new Response();

			try
			{
				response = DesktopEngine.GetSummaryReceipt(agentSessionId, customerSessionId, transactionId, transactiontype, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{

				response.Error = PrepareError(ex);
			}
			return response;
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Added new method to Get CouponCode Receipt
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <returns></returns>
		public Response GetCouponCodeReceipt(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();

			try
			{
				response = DesktopEngine.GetCouponCodeReceipt(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{

				response.Error = PrepareError(ex);
			}
			return response;
		}
	}
}
