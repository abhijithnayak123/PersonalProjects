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
    public partial class DesktopWSImpl : IMoneyOrderService
    {

		public Response GetMoneyOrderFee(long customerSessionId, MoneyOrderData moneyOrder, MGIContext mgiContext)
        {
			Response response = new Response();
			try
			{
				response = DesktopEngine.GetMoneyOrderFee(customerSessionId, moneyOrder, mgiContext);

			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
        }

        public Response PurchaseMoneyOrder(long customerSessionId, MoneyOrderPurchase moneyOrderPurchase, MGIContext mgiContext)
        {
			Response response = new Response();
			try
			{
				response = DesktopEngine.PurchaseMoneyOrder(customerSessionId, moneyOrderPurchase, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
        }

        public Response CancelMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
        {
			Response response = new Response();
			try
			{
				response = DesktopEngine.CancelMoneyOrder(customerSessionId, moneyOrderId, mgiContext);

			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
        }

		public Response UpdateMoneyOrder(long customerSessionId, MoneyOrderTransaction moneyOrderTransaction, long moneyOrderId, MGIContext mgiContext)
        {
			Response response = new Response();

			try
			{
				response = DesktopEngine.UpdateMoneyOrder(customerSessionId, moneyOrderTransaction, moneyOrderId, mgiContext);

			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
        }

		public Response UpdateMoneyOrderStatus(long customerSessionId, long moneyOrderId, int newMoneyOrderStatus, MGIContext mgiContext)
        {
			Response response = new Response();
			try
			{
				response = DesktopEngine.UpdateMoneyOrderStatus(customerSessionId, moneyOrderId, newMoneyOrderStatus, mgiContext);

			}
			catch (FaultException<NexxoSOAPFault> ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
        }

		public Response GetMoneyOrderStage(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
        {

			Response response = new Response();
			try
			{
			response = DesktopEngine.GetMoneyOrderStage(customerSessionId, moneyOrderId, mgiContext);

			}
			catch(FaultException<NexxoSOAPFault>ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
        }

		public Response GenerateCheckPrintForMoneyOrder(long moneyOrderId, long customerSessionId, MGIContext mgiContext)
        {
			Response response = new Response();
		   try
		   {
			   response = DesktopEngine.GenerateCheckPrintForMoneyOrder(moneyOrderId, customerSessionId, mgiContext);

		   }
			catch(FaultException<NexxoSOAPFault>ex)
		   {
				response.Error = PrepareError(ex);
			}
			return response;
        }
	

		public Response GenerateMoneyOrderDiagnostics(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopEngine.GenerateMoneyOrderDiagnostics(agentSessionId, mgiContext);
			}
			catch(FaultException<NexxoSOAPFault>ex)
			{
				response.Error = PrepareError(ex);
			}
			return response;
			
		}
	}
}