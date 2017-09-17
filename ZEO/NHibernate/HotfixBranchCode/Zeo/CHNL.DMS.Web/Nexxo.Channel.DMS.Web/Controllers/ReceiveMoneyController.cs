using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using System;
using System.Web.Mvc;
using CustomerSession = MGI.Channel.Shared.Server.Data.CustomerSession;
using MGI.Common.Util;
using MGI.Channel.DMS.Web.Common;
using System.Linq;
using System.Collections.Generic;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class ReceiveMoneyController : BaseController
    {
		//
        // GET: /ReceiveMoney/
        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "ReceiveMoney", MasterName = "_Common")]
        public ActionResult ReceiverMoneyDetails(ReceiveMoney receivemoney)
        {
			Desktop desktop = new Desktop();
            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            long customerSesionId = long.Parse(customerSession.CustomerSessionId);
            
			ReceiveMoneyRequest request = new ReceiveMoneyRequest()
			{
				ConfirmationNumber = receivemoney.WesternUnionMTCN.Replace("-", "")
			};

            MoneyTransferTransaction transaction = desktop.GetReceiveTransaction(customerSesionId, request, mgiContext);

			ShoppingCart cart = GetShoppingCartDetails(customerSession);

			MoneyTransfer mTransfer = null;

			if (cart != null && cart.MoneyTransfers != null)
				mTransfer = cart.MoneyTransfers.FirstOrDefault(c => c.ConfirmationNumber == transaction.ConfirmationNumber);

			if (mTransfer != null)
				throw new Exception(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.DuplicateReceiveMoney);

			receivemoney.ReceiverName = NexxoUtil.GetDictionaryValueIfExists(transaction.MetaData, "ReceiverName");
			receivemoney.SenderName = transaction.SenderName;
            receivemoney.TestAnswer = transaction.TestAnswer;
            receivemoney.TestQuestion = transaction.TestQuestion;
            receivemoney.TransferAmount = transaction.DestinationPrincipalAmount.ToString();
			receivemoney.TransferAmountWithCurrency = transaction.DestinationPrincipalAmount.ToString() + " " + transaction.DestinationCurrencyCode;
			receivemoney.TransactionId = long.Parse(transaction.TransactionID);
			receivemoney.SenderStateCode = NexxoUtil.GetDictionaryValueIfExists(transaction.MetaData, "SenderStateCode");

            TempData["receivemoney"] = receivemoney;

            return RedirectToAction("ReceiverMoneyDetails", "ReceiveMoney");
        }

		

        [HttpGet]
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
        public ActionResult ReceiverMoneyDetails()
        {
            ReceiveMoney receivemoney = (ReceiveMoney)TempData["receivemoney"];
            return View("ReceiverMoneyDetails", receivemoney);
        }

        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "ReceiverMoneyDetails", MasterName = "_Common")]
        public ActionResult ReceiveMoneyValidate(ReceiveMoney receivemoney)
        {
            Desktop desktop = new Desktop();
            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
            long customerSesionId = long.Parse(customerSession.CustomerSessionId);
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			ValidateRequest validateRequest = new ValidateRequest()
			{
				TransactionId = receivemoney.TransactionId,
				TransferType = TransferType.RecieveMoney,
				Amount = decimal.Parse(receivemoney.TransferAmount)
			};

			ValidateResponse response = desktop.ValidateTransfer(customerSesionId, validateRequest, mgiContext);

			string transactionId = Convert.ToString(response.TransactionId);
          
           
            
            ViewBag.Navigation = Resources.NexxoSiteMap.TransactionSummary;
            return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
        }

		private ShoppingCart GetShoppingCartDetails(CustomerSession customerSession)
		{
			Desktop desktop = new Desktop();
			ShoppingCartDetail shoppingCartDetail = new ShoppingCartDetail();

			ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);

			return shoppingCart;
		}
		
    }
}
