using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : ICheckCashingService
	{
		public Response SubmitCheck(long customerSessionId, CheckSubmission check, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
			
				response =  DesktopEngine.SubmitCheck(customerSessionId, check, mgiContext);

			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

        public Response CancelCheck(long customerSessionId, string check, MGIContext mgiContext)
        {
			Response response = new Response();
			try
			{
				response = DesktopEngine.CancelCheck(customerSessionId, check, mgiContext);

			}
			catch(FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
        }

		public Response GetCheckStatus(long customerSessionId, string checkId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetCheckStatus(customerSessionId, checkId, mgiContext);
			
			}
			catch(FaultException<NexxoSOAPFault> ex)
			{
				response.Error =PrepareError(ex);
			}
			return response;
		}

		public Response CanResubmit(long customerSessionId, string checkId, MGIContext mgiContext)
		{
			
				Response response = new Response();
				try
				{
					response = DesktopEngine.CanResubmit(customerSessionId, checkId, mgiContext);
				
				}
			catch(FaultException<NexxoSOAPFault> ex)
				{
					response.Error =PrepareError(ex);
				}
			return response;
					
		}

		public Response GetCheckTypes(long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response =DesktopEngine.GetCheckTypes(customerSessionId, mgiContext);
			
			}
			catch(FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
			 
		}

		public Response GetCheckFee(long customerSessionId, CheckSubmission checkSubmit, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetCheckFee(customerSessionId, checkSubmit, mgiContext);
			}
			catch(FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetCheckTranasactionDetails(long agentSessionId, long customerSessionId, string checkId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetCheckTranasactionDetails(agentSessionId, customerSessionId, checkId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetCheckFrankingData(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response =  DesktopEngine.GetCheckFrankingData(customerSessionId, transactionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetCheckProcessorInfo(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetCheckProcessorInfo(agentSessionId, mgiContext);
			}
			catch(FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response; 
		}

		public Response UpdateTransactionFranked(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.UpdateTransactionFranked(customerSessionId, transactionId, mgiContext);
			}
			 catch(FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetCheckSession(long customerSessionId, MGIContext mgiContext)
		{
			 Response response = new Response();
			 try
			 {
				response = DesktopEngine.GetCheckSession(customerSessionId, mgiContext);
			 }
			catch(FaultException<NexxoSOAPFault> ex)
			 {
				response.Error  = PrepareError(ex);
			 }
			 return response;
		}
	}
}