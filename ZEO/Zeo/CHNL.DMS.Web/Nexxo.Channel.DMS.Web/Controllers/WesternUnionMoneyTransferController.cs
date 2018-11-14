using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class WesternUnionMoneyTransferController : SendMoneyBaseController
    {
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ProductInfo")]
        public ActionResult MoneyTransfer(bool IsException = false, string ExceptionMessage = "")
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;

                if (Session["SendMoneyRecord"] != null)
                    Session.Remove("SendMoneyRecord");

                Session["isCashierAgree"] = "true";
                Session["activeButton"] = "moneytransfer";
                if (Session["SendMoneyModel"] != null)
                {
                    Session.Remove("SendMoneyModel");
                }
                ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];
                long customerSessionId = GetCustomerSessionId();
                SendMoney sendMoney = new SendMoney();
                ViewBag.Navigation = Resources.NexxoSiteMap.SendMoney;
                ZeoClient.ZeoContext context = GetZeoContext();
                if (string.IsNullOrWhiteSpace(context.WUCounterId))
                {
                    GetCustomerSessionCounterId((int)Helper.ProviderId.WesternUnion, context);
                }
                string viewName = "";
                string masterName = "_Common";
                object model = new object();

                if (!string.IsNullOrWhiteSpace(context.WUCardNumber))
                    Session["CustomerHasGoldCard"] = "true";
                else
                    Session["CustomerHasGoldCard"] = "false";

                if (!string.IsNullOrWhiteSpace(context.WUCardNumber) || TempData["SkipGoldCard"] != null)
                {
                    if (string.IsNullOrWhiteSpace(customerSession.TipsAndOffers) && (!string.IsNullOrWhiteSpace(context.WUCardNumber) || (TempData["SkipGoldCard"] != null && !Convert.ToBoolean(TempData["SkipGoldCard"].ToString()))))
                    {
                        ///Added product type to know in WU from where the method get call.
                        context.ProductType = "sendmoney";
                        PopulatTipsAndOffersMessage(alloyServiceClient, customerSession, context);
                    }
                    string channelPartnerName = GetChannelPartnerName();

                    ZeoClient.Response response = alloyServiceClient.GetXfrCountries(context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    sendMoney.LCountry = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                    //response = alloyServiceClient.GetXfrStates(string.Empty, alloyContext);
                    //response = desktop.GetXfrStates(customerSessionId, string.Empty, mgiContext);
                    //if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
                    sendMoney.LStates = DefaultSelectList(); //GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                    sendMoney.LDelivertyMethods = DefaultSelectList();

                    if (channelPartnerName == "MGI")
                    {
                        sendMoney.LCities = DefaultSelectList();

                    }
                    else
                    {
                        sendMoney.LCities = DefaultSelectList();
                        //                  response = alloyServiceClient.GetXfrCities(string.Empty, alloyContext);
                        ////response = desktop.GetXfrCities(customerSessionId, string.Empty, mgiContext);
                        //                  if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
                        //sendMoney.LCities = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);
                    }
                    sendMoney.LDeliveryOptions = DefaultSelectList();
                    sendMoney.LActOnMyBehalf = GetActBeHalfList();
                    response = alloyServiceClient.GetFrequentReceivers(context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    sendMoney.FrequentReceivers = response.Result as List<ZeoClient.Receiver>;
                    sendMoney.WUCardNumber = context.WUCardNumber;
                    viewName = "SendMoney";
                    model = sendMoney;
                }
                else
                {
                    Session["lookupGoldCard"] = null;
                    WesternUnionDetails wuDetailsModel = new WesternUnionDetails();
                    wuDetailsModel.EditGoldCardFrom = "sendmoney";
                    viewName = "EnrollWesternUnionGoldCard";
                    model = wuDetailsModel;
                }

                if (sendMoney.FrequentReceivers != null)
                {
                    if (sendMoney.FrequentReceivers.Count > 0)
                        Session["AreThereReceivers"] = "true";
                    else
                        Session["AreThereReceivers"] = "False";
                }
                return View(viewName, masterName, model);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }
    }
}
