using TCF.Channel.Zeo.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Common;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using System.Linq;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class ProductController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProductInformation(bool IsException = false, string ExceptionMessage = "")
        {
            Session["activeButton"] = null;
            ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];
            if (customerSession == null)
            {
                CustomerSearch customerSearch = new CustomerSearch();
                return View("CustomerSearch", customerSearch);
            }
            ViewBag.IsException = IsException;
            ViewBag.ExceptionMessage = ExceptionMessage;

            ViewBag.Navigation = Resources.NexxoSiteMap.ProductInformation;
            return View("ProductInformation", new ProductInfo());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ProductInfo")]
        public ActionResult SendMoney()
        {
            try
            {
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoContext context = GetZeoContext();
                if (string.IsNullOrWhiteSpace(context.WUCounterId))
                {
                    GetCustomerSessionCounterId((int)Helper.ProviderId.WesternUnion, context);
                }
                Session["isCashierAgree"] = "true";
                Session["activeButton"] = "moneytransfer";
                if (Session["SendMoneyModel"] != null)
                {
                    Session.Remove("SendMoneyModel");
                }

                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];

                SendMoney sendMoney = new SendMoney();
                ViewBag.Navigation = Resources.NexxoSiteMap.SendMoney;
                string viewName = "";
                string masterName = "_Common";
                object model = new object();

                if (!string.IsNullOrEmpty(context.WUCardNumber))
                    Session["CustomerHasGoldCard"] = "true";
                else
                    Session["CustomerHasGoldCard"] = "false";

                if (!string.IsNullOrEmpty(context.WUCardNumber) || TempData["SkipGoldCard"] != null)
                {
                    if (string.IsNullOrWhiteSpace(customerSession.TipsAndOffers) && (!string.IsNullOrEmpty(context.WUCardNumber) || (TempData["SkipGoldCard"] != null && !Convert.ToBoolean(TempData["SkipGoldCard"].ToString()))))
                    {
                        PopulatTipsAndOffersMessage(alloyServiceClient, customerSession, context);
                    }
                    response = alloyServiceClient.GetXfrCountries(context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    sendMoney.LCountry = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                    sendMoney.LStates = DefaultSelectList();
                    sendMoney.LCities = DefaultSelectList();
                    sendMoney.LDelivertyMethods = DefaultSelectList();

                    sendMoney.LDeliveryOptions = DefaultSelectList();
                    sendMoney.LActOnMyBehalf = GetActBeHalfList();
                    sendMoney.WUCardNumber = context.WUCardNumber;

                    response = alloyServiceClient.GetFrequentReceivers(context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    sendMoney.FrequentReceivers = response.Result as List<ZeoClient.Receiver>;
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

        /// <summary>
        /// US2054
        /// </summary>
        /// <returns></returns>
        public JsonResult CashierLocationState()
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];
                bool isStateAvailable = false;
                string agentFirstName = string.Empty;
                string agentLastName = string.Empty;
                if (Session["HTSessions"] != null)
                {
                    System.Collections.Hashtable htSessions = (System.Collections.Hashtable)Session["HTSessions"];
                    ZeoClient.AgentSession agentSession = ((ZeoClient.AgentSession)(htSessions["TempSessionAgent"]));
                    if (agentSession != null)
                    {
                        ZeoClient.Response response = alloyServiceClient.GetLocationById(Convert.ToInt32(context.LocationId), context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        List<ZeoClient.Location> locations = response.Result as List<ZeoClient.Location>;
                        ZeoClient.Location location = locations.FirstOrDefault();
                        context.StateCode = location.State;
                        if (!string.IsNullOrWhiteSpace(context.StateCode))
                        {
                            ZeoClient.Response alloyResponse = alloyServiceClient.IsSWBStateXfer(context);
                            if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                            bool isLocationState = Convert.ToBoolean(alloyResponse.Result);
                            if (isLocationState)
                            {
                                alloyResponse = alloyServiceClient.GetAgentDetails(long.Parse(agentSession.SessionId), context);
                                if (WebHelper.VerifyException(alloyResponse)) throw new ZeoWebException(alloyResponse.Error.Details);
                                ZeoClient.Agent cashierDetails = alloyResponse.Result as ZeoClient.Agent;

                                isStateAvailable = true;
                                agentFirstName = cashierDetails.AgentFirstName;
                                agentLastName = cashierDetails.AgentLastName;
                            }
                        }
                    }
                }
                var jsonData = new
                {
                    isStateAvailable,
                    agentFirstName,
                    agentLastName
                };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// US2054 
        /// </summary>
        /// <param name="agentFirstName"></param>
        /// <param name="agentLastName"></param>
        /// <returns></returns>
        public ActionResult CashierValidatePopup(string agentFirstName, string agentLastName)
        {
            AgentDetails agentDetails = new AgentDetails();
            agentDetails.AgentFirstName = agentFirstName;
            agentDetails.AgentLastName = agentLastName;
            return PartialView("_partialCashierValidaton", agentDetails);
        }

        public ActionResult ReceiveMoney()
        {
            Session["isCashierAgree"] = "true";
            Session["activeButton"] = "receivemoney";
            ReceiveMoney receive = new ReceiveMoney();
            return View("ReceiveMoney", "_Common", receive);
        }

        public ActionResult TipsAndOffers()
        {
            try
            {
                ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];

                Models.TipsAndOffersViewModel tipsAndOffers = new Models.TipsAndOffersViewModel();

                //TODO - Abhi - Check the logic for Tips and Offers. Assigning the empty message for time being.
                tipsAndOffers.Message = string.Empty;
                if (customerSession != null)
                    tipsAndOffers.Message = customerSession.TipsAndOffers;
                else
                    tipsAndOffers.Message = string.Empty;

                return PartialView("_tipsAndOffers", tipsAndOffers);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult IsSSNExists()
        {
            BaseModel model = new BaseModel();
            bool isSSNExists = string.IsNullOrWhiteSpace(model.CustomerSession.Customer.SSN);
            return Json(isSSNExists, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowSSNValidationWarning()
        {
            BaseModel baseModel = new BaseModel();
            return PartialView("_SSNValidationWarning",Convert.ToInt64(baseModel.CustomerSession.Customer.CustomerId));
        }

    }
}
