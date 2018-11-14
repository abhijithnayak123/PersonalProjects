using TCF.Channel.Zeo.Web.Models;
using System;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Common;
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class ReceiveMoneyController : BaseController
    {
        //
        // GET: /ReceiveMoney/
        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "ReceiveMoney", MasterName = "_Common")]
        public ActionResult ReceiverMoneyDetails(ReceiveMoney receivemoney)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                GetCustomerSessionCounterId((int)Helper.ProviderId.WesternUnion, context);
                ZeoClient.ReceiveMoneyRequest request = new ZeoClient.ReceiveMoneyRequest()
                {
                    ConfirmationNumber = receivemoney.WesternUnionMTCN.Replace("-", "")
                };

                ZeoClient.Response response = alloyServiceClient.ReceiveMoneySearch(request, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                ZeoClient.MoneyTransferTransaction transaction = response.Result as ZeoClient.MoneyTransferTransaction;

                //Response cartResponse = desktop.ShoppingCart(customerSession.CustomerSessionId);
                //            if (WebHelper.VerifyException(cartResponse)) throw new AlloyWebException(cartResponse.Error.Details);
                //            ShoppingCart cart = cartResponse.Result as ShoppingCart;

                //MoneyTransfer mTransfer = null;

                //if (cart != null && cart.MoneyTransfers != null)
                //	mTransfer = cart.MoneyTransfers.FirstOrDefault(c => c.ConfirmationNumber == transaction.ConfirmationNumber);

                //if (mTransfer != null)
                //{
                //	string messageKey = "1005.100.8303";
                //	throw new AlloyWebException(GetErrorMessage(GetAgentSessionId(), messageKey, mgiContext));
                //}

                receivemoney.ReceiverName = Helper.GetDictionaryValueIfExists(transaction.MetaData, "ReceiverName");
                receivemoney.SenderName = transaction.SenderName;
                receivemoney.TestAnswer = transaction.TestAnswer;
                receivemoney.TestQuestion = transaction.TestQuestion;
                receivemoney.TransferAmount = transaction.DestinationPrincipalAmount.ToString();
                receivemoney.TransferAmountWithCurrency = transaction.DestinationPrincipalAmount.ToString() + " " + transaction.DestinationCurrencyCode;
                receivemoney.TransactionId = transaction.TransactionId;
                receivemoney.SenderStateCode = Helper.GetDictionaryValueIfExists(transaction.MetaData, "SenderStateCode");

                TempData["receivemoney"] = receivemoney;

                return RedirectToAction("ReceiverMoneyDetails", "ReceiveMoney");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }



        [HttpGet]
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ProductInfo")]
        public ActionResult ReceiverMoneyDetails()
        {
            try
            {

                ReceiveMoney receivemoney = (ReceiveMoney)TempData["receivemoney"];
                return View("ReceiverMoneyDetails", receivemoney);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "ReceiverMoneyDetails", MasterName = "_Common")]
        public ActionResult ReceiveMoneyValidate(ReceiveMoney receivemoney)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.ValidateRequest validateRequest = new ZeoClient.ValidateRequest()
                {
                    TransactionId = receivemoney.TransactionId,
                    TransferType = ZeoClient.HelperMoneyTransferType.Receive,
                    Amount = decimal.Parse(receivemoney.TransferAmount)
                };

                ZeoClient.Response serviceResponse = alloyServiceClient.Validate(validateRequest, context);
                if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                //ZeoClient.ValidateResponse response = serviceResponse.Result as ZeoClient.ValidateResponse;

                //string transactionId = Convert.ToString(response.TransactionId);

                ViewBag.Navigation = Resources.NexxoSiteMap.TransactionSummary;
                return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }


    }
}
