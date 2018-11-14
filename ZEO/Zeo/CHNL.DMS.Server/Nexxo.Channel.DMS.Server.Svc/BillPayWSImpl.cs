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

	public partial class DesktopWSImpl : IBillPayService
	{
		public Response GetBillers(long customerSessionId, long channelPartnerID, string searchTerm, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
				response = DesktopEngine.GetBillers(customerSessionId, channelPartnerID, searchTerm, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetBiller(long customerSessionId, long billerId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                 response = DesktopEngine.GetBiller(customerSessionId, billerId, mgiContext);
            }
            catch(FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response GetBillerByName(long customerSessionId, long channelPartnerID, string billerNameOrCode, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                 response = DesktopEngine.GetBillerByName(customerSessionId, channelPartnerID, billerNameOrCode, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response GetFrequentBillers(long customerSessionId, long alloyId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetFrequentBillers(customerSessionId, alloyId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response GetAllBillers(long customerSessionId, long ChannelPartnerID, Guid LocationRegionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
				response = DesktopEngine.GetAllBillers(customerSessionId, ChannelPartnerID, LocationRegionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetLocations(long customerSessionId, string billerName, string accountNumber, decimal amount, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetLocations(customerSessionId, billerName, accountNumber, amount, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response GetFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation location, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetFee(customerSessionId, billerNameOrCode, accountNumber, amount, location, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response GetBillerInfo(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetBillerInfo(customerSessionId, billerNameOrCode, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response GetProviderAttributes(long customerSessionId, string billerNameOrCode, string location, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
				response = DesktopEngine.GetProviderAttributes(customerSessionId, billerNameOrCode, location, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
		}

		public Response GetFavoriteBiller(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetFavoriteBiller(customerSessionId, billerNameOrCode, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response GetBillPayFee(long customerSessionId, string providerName, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetBillPayFee(customerSessionId, providerName, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response StageBillPayment(long customerSessionId, long transactionID, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.StageBillPayment(customerSessionId, transactionID, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response ValidateBillPayment(long customerSessionId, BillPayment billPayment, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.ValidateBillPayment(customerSessionId, billPayment, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response GetAgent(long customerSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                return DesktopEngine.GetAgent(customerSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response GetCardInfo(long customerSessionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                response = DesktopEngine.GetCardInfo(customerSessionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		/// <summary>
		/// This method will publish the methods in the DMS Service Operation for Add Past Billers - User Story # US1646.
		/// </summary>
		/// <param name="customerSessionId">Session ID</param>
		/// <param name="cardNumber">Card Number</param>
		/// <param name="mgiContext">mgiContext</param>
		public Response AddPastBillers(long customerSessionId, string cardNumber, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.AddPastBillers(customerSessionId, cardNumber, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		public Response CancelBillPayment(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.CancelBillPayment(customerSessionId, transactionId, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}

		//Begin TA-191 Changes
		//       User Story Number: TA-191 | Server |   Developed by: Sunil Shetty     Date: 21.04.2015
		//       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. we are sending Customer session id and biller id.
		// the customer session id will get AlloyID/Pan ID and biller ID will help us to disable biller from tCustomerPreferedProducts table
		public Response DeleteFavoriteBiller(long customerSessionId, long billerID, MGIContext mgiContext)
		{
            Response response = new Response();
            try
            {
                DesktopEngine.DeleteFavoriteBiller(customerSessionId, billerID, mgiContext);
            }
            catch (FaultException<NexxoSOAPFault> ex)
            {
				response.Error = PrepareError(ex);
            }
            return response;
		}
	}
}
