using System;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using System.Threading;
using TCF.Channel.Zeo.Web.Common;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    [SkipNoDirectAccess]
    public class CustomerProfileController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerProfile()
        {
            ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();

            ZeoClient.ZeoContext context = GetZeoContext();

            CustomerProfile customerProfile = new CustomerProfile();

            long customerId = 0;

            if (customerProfile.CustomerSession != null)
                customerId = customerProfile.CustomerSession.CustomerId;

            customerProfile.CardBalance = new CardBalance();
            string maskedNumber = string.Empty;

            ZeoClient.CustomerProfile custProfile = customerProfile.CustomerSession.Customer as ZeoClient.CustomerProfile;
            string cardNumber = custProfile.CardNumber;

            if (!string.IsNullOrEmpty(cardNumber) && cardNumber.Length >= 4)
            {
                cardNumber = cardNumber.Length > 4 ? cardNumber.Substring(cardNumber.Length - 4) : cardNumber;
                maskedNumber = "**** **** **** " + cardNumber;
            }
            else
            {
                maskedNumber = cardNumber;
            }
            if (customerProfile.CustomerSession.IsGPRCustomer)
            {
                ViewBag.GPRCardExists = true;

                ZeoClient.CardBalanceInfo cardInfo = GetCardBalance(context);
                customerProfile.CardBalance.Balance = cardInfo.Balance;
                Common.CardStatus cardStatus = (Common.CardStatus)cardInfo.CardStatus;
                customerProfile.CardBalance.CardStatus = cardStatus.ToString();
                if (Helper.GetDictionaryValueIfExists(cardInfo.MetaData, "IsPrimaryCardHolder") != null)
                {
                    bool cardType = Helper.GetBoolDictionaryValueIfExists(cardInfo.MetaData, "IsPrimaryCardHolder");
                    customerProfile.CardBalance.CardType = cardType == true ?
                        App_GlobalResources.Nexxo.Primarycard : App_GlobalResources.Nexxo.Companioncard;
                }
                string expMsg = Convert.ToString(TempData["GPRDown"]);
                if (expMsg != string.Empty)
                {
                    ViewBag.ExceptionMessage = expMsg;
                    ViewBag.IsException = true;
                }
            }
            else
                ViewBag.GPRCardExists = false;

            if (customerProfile.CustomerSession.IsGPRCustomer)
            {
                customerProfile.MaskedCardNumber = maskedNumber.ToString();
            }

            if (custProfile != null)
            {
                customerProfile.FirstName = custProfile.FirstName;
                customerProfile.LastName = custProfile.LastName;
                customerProfile.AlloyID = Convert.ToInt64(customerId);
            }

            if (custProfile != null && custProfile.Address != null)
            {
                customerProfile.Address1 = string.Format("{0} {1}", custProfile.Address.Address1, custProfile.Address.Address2);
                customerProfile.Address1 = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(custProfile.Address.Address1.ToLower());
                customerProfile.City = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(custProfile.Address.City.ToLower());
                customerProfile.StateZipCode = string.Format("{0}, {1}", custProfile.Address.State, custProfile.Address.ZipCode);
            }

            if (custProfile != null && custProfile.Phone1 != null)
            {
                customerProfile.PhoneNumber = string.IsNullOrEmpty(custProfile.Phone1.Number) ? string.Empty : String.Format("{0:###-###-####}", Convert.ToInt64(custProfile.Phone1.Number));
                customerProfile.PhoneType = string.IsNullOrEmpty(custProfile.Phone1.Type) ? string.Empty : custProfile.Phone1.Type;
            }

            return PartialView("_CustomerProfile", customerProfile);
        }

        private ZeoClient.CardBalanceInfo GetCardBalance(ZeoClient.ZeoContext context)
        {
            ZeoClient.CardBalanceInfo cardInfo = new ZeoClient.CardBalanceInfo();
            if (Session["CardBalance"] == null)
            {
                ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();
                try
                {
                    ZeoClient.Response response = new ZeoClient.Response();

                    response = client.GetCardBalance(context);
                    if (WebHelper.VerifyException(response))
                        throw new ZeoWebException(response.Error.Details);
                    else
                        cardInfo = response.Result as ZeoClient.CardBalanceInfo;
                }
                catch (Exception ex)
                {

                    cardInfo.Balance = decimal.MinValue;
                    cardInfo.CardStatus = 100;
                    TempData["GPRDown"] = ex.Message;
                }
                Session["CardBalance"] = cardInfo;
            }
            else
            {
                cardInfo = (ZeoClient.CardBalanceInfo)Session["CardBalance"];
            }
            return cardInfo;
        }
    }
}
