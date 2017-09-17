using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;
using TCF.Zeo.Security.Voltage;
using System;
using System.Web.Mvc;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Logging.Impl;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class ProductCredentialController : BaseController
    {
        NLoggerCommon NLogger = new NLoggerCommon();
        ZeoClient.Response response = new ZeoClient.Response();

        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "ProductCredential", MasterName = "_Common")]
        public ActionResult ProductCredential(ProductCredentialViewModel productCredential)
        {
            ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];
            ZeoClient.ZeoContext context = GetZeoContext();

            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();

            ZeoClient.Response cardRegisterResponse = new ZeoClient.Response();
            ZeoClient.Response activateResponse = new ZeoClient.Response();
            try
            {
                if (productCredential.HasGPRCard)
                {
                    return RedirectToAction("ProductInformation", "Product");
                }
                else
                {
                    string cardId = Guid.NewGuid().ToString();
                    ZeoClient.Funds funds = null;

                    string cardNumber = productCredential.CardNumber == null ? string.Empty : productCredential.CardNumber.Replace(" ", "");

                    if (!string.IsNullOrWhiteSpace(cardNumber))
                    {
                        SecureData secure = new SecureData();
                        cardNumber = secure.Decrypt(cardNumber, productCredential.CVV);

                        productCredential.CardNumber = cardNumber;
                        Session.Add("CardNumber", !string.IsNullOrEmpty(cardNumber) ? cardNumber.Substring(cardNumber.Length - 4) : string.Empty);
                        customerSession.CardNumber = cardNumber;
                        customerSession.IsGPRCustomer = false;
                        Session["CustomerSession"] = customerSession;
                    }

                    cardRegisterResponse = RegisterAccount(productCredential, context);
                    if (WebHelper.VerifyException(cardRegisterResponse)) throw new ZeoWebException(cardRegisterResponse.Error.Details);

                    funds = new ZeoClient.Funds()
                    {
                        Amount = productCredential.InitialLoad,
                        Fee = productCredential.ActivationFee,
                        PromoCode = productCredential.PromoCode,
                        TransactionId = productCredential.TransactionId
                    };

                    activateResponse = client.ActivateGPRCard(funds, context);
                    if (WebHelper.VerifyException(activateResponse)) throw new ZeoWebException(activateResponse.Error.Details);

                    return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        #region Private Methods

        private ZeoClient.Response RegisterAccount(ProductCredentialViewModel productCredential, ZeoClient.ZeoContext context)
        {

            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();

            ZeoClient.FundsAccount fundAccount = new ZeoClient.FundsAccount()
            {
                CardNumber = productCredential.CardNumber,
                // AccountNumber = productCredential.AccountNumber,
                FraudScore = Convert.ToInt32(productCredential.FraudScore),
                Resolution = productCredential.Resolution,

                ProxyId = productCredential.ProxyId,
                PseudoDDA = productCredential.PseudoDDA,
                ExpirationDate = productCredential.ExpirationDate
            };

            response = client.AddFundsAccount(fundAccount, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            return response;
        }

        private string TruncateCardNumber(string accountIdentifier)
        {
            return accountIdentifier.Length >= 4 ? accountIdentifier.Substring(accountIdentifier.Length - 4) : accountIdentifier;
        }

        #endregion
    }
}
