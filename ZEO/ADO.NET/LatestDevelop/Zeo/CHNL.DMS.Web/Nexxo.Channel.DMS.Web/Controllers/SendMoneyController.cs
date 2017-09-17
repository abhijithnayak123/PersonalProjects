using TCF.Channel.Zeo.Web.Common;
using TCF.Channel.Zeo.Web.Models;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class SendMoneyController : BaseController
    {
        #region  Manage Receivers
        public ActionResult AddEditReceiver(string type)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();

                SendMoneyReceiver receiver = new SendMoneyReceiver();

                if (type == "Edit" || type == "add")
                {
                    receiver = (SendMoneyReceiver)Session["Receiver"];
                }
                else
                {
                    Session["Receiver"] = null;
                    receiver.AddEdit = "Add";
                }

                InitializeDropdownlistValues(receiver, alloyServiceClient, context);

                return View("SendMoneyReceiver", receiver);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        public ActionResult AddEditReceiver(SendMoneyReceiver sendMoneyReceiver)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Receiver clientReceiver = new ZeoClient.Receiver();
                long responseReceiverID = 0;
                long customerSessionId = GetCustomerSessionId();

                InitializeDropdownlistValues(sendMoneyReceiver, alloyServiceClient, context);

                if (sendMoneyReceiver.ReceiverId == 0)
                {
                    clientReceiver = SetReceivers(sendMoneyReceiver);
                    response = alloyServiceClient.AddReceiver(clientReceiver, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    responseReceiverID = long.Parse(response.Result.ToString());
                }
                else
                {
                    clientReceiver = SetReceivers(sendMoneyReceiver);
                    response = alloyServiceClient.EditReceiver(clientReceiver, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    responseReceiverID = long.Parse(response.Result.ToString());
                }

                TempData["ReceiverId"] = responseReceiverID;
                return RedirectToAction("SendMoney", "SendMoney");
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return View("SendMoneyReceiver", sendMoneyReceiver);
            }
        }

        private void InitializeDropdownlistValues(SendMoneyReceiver receiver, ZeoClient.ZeoServiceClient alloyServiceClient, ZeoClient.ZeoContext context)
        {
            long customerSessionId = GetCustomerSessionId();

            ZeoClient.Response response = new ZeoClient.Response();

            response = alloyServiceClient.GetXfrCountries(context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            receiver.LPickUpCountry = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

            if (Session["Receiver"] != null)
            {
                //Edit
                if (string.IsNullOrWhiteSpace(receiver.PickUpCountry))
                {
                    receiver.LPickUpState = DefaultSelectList();
                    receiver.LPickUpCity = DefaultSelectList();
                }
                else
                {
                    response = alloyServiceClient.GetXfrStates(receiver.PickUpCountry, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    receiver.LPickUpState = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                    if (string.IsNullOrWhiteSpace(receiver.PickUpState))
                    {
                        receiver.LPickUpCity = DefaultSelectList();
                    }
                    else
                    {
                        response = alloyServiceClient.GetXfrCities(receiver.PickUpState, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        receiver.LPickUpCity = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);
                    }
                }
                //Get the CurrencyCode
                string currencyCode = string.Empty;
                if (!string.IsNullOrEmpty(receiver.PickUpCountry))
                {
                    response = alloyServiceClient.GetCurrencyCode(receiver.PickUpCountry, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    currencyCode = response.Result as string;
                }
                //WUStates
                string stateName = "";
                string cityName = "";
                string stateCode = "";

                if (receiver.PickUpState != null && receiver.LPickUpState.Count() > 1)
                {
                    SelectListItem selectedState = receiver.LPickUpState.Where(st => st.Value == receiver.PickUpState).FirstOrDefault();
                    if (selectedState != null)
                        stateName = selectedState.Text;
                }

                if (receiver.PickUpCity != null && receiver.LPickUpCity.Count() > 1)
                {
                    SelectListItem selectedCity = receiver.LPickUpCity.Where(ct => ct.Value == receiver.PickUpCity).FirstOrDefault();
                    if (selectedCity != null)
                        cityName = selectedCity.Text;
                }

                stateCode = string.IsNullOrEmpty(receiver.PickUpState) ? string.Empty : receiver.PickUpState;

                if (!string.IsNullOrWhiteSpace(receiver.PickUpCountry))
                {
                    receiver.LDeliveryMethods = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Method, receiver.PickUpCountry, currencyCode, stateName, stateCode, context, cityName);

                    string svcCode = "";
                    if (receiver.DeliveryMethod != null && receiver.LDeliveryMethods.Count() > 1)
                    {
                        SelectListItem selectedDeliveryMethod = receiver.LDeliveryMethods.Where(dm => dm.Value == receiver.DeliveryMethod).FirstOrDefault();
                        if (selectedDeliveryMethod != null)
                            svcCode = selectedDeliveryMethod.Value;
                    }

                    //mgiContext.SVCCode = svcCode;
                    receiver.LDeliveryOptions = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Option, receiver.PickUpCountry, currencyCode, stateName, stateCode, context, "", svcCode);
                }
                else
                {
                    receiver.LDeliveryMethods = DefaultSelectList();
                    receiver.LDeliveryOptions = DefaultSelectList();
                }
            }
            else
            {
                //Add
                //            response = alloyServiceClient.GetXfrStates(customerSessionId, "", alloyContext);
                //            if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
                //receiver.LPickUpState = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                //            response = alloyServiceClient.GetXfrCities(customerSessionId, "", alloyContext);
                //            if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
                //receiver.LPickUpCity = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);
                receiver.LPickUpState = DefaultSelectList();
                receiver.LPickUpCity = DefaultSelectList();
                receiver.LDeliveryMethods = DefaultSelectList();
                receiver.LDeliveryOptions = DefaultSelectList();
            }
        }

        //We are not using
        public ActionResult EditReceiver()
        {
            Receivers receivers = new Receivers();
            return View("EditReceiver", "_Common", receivers);
        }

        #endregion

        // This method displays the Receivers grid
        [HttpPost]
        public ActionResult SearchReceiver(Receivers receivers)
        {
            //SendMoney sendmoney = new SendMoney();
            return View("EditReceiver", receivers);
        }

        public ActionResult ReceiverSearch(string LastName)
        {
            return View("EditReceiver");
        }

        //need to cheat the cache as popup shows up with old data   , string cheatDate 
        public ActionResult GetReceiverForEdit(string ReceiverId)
        {
            ZeoClient.Response response = new ZeoClient.Response();
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();
            //.Server.Data.Response response = new DMS.Server.Data.Response();
            try
            {
                if (ReceiverId != "0")
                {
                    response = alloyServiceClient.GetReceiver(long.Parse(ReceiverId), context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    ZeoClient.Receiver editReceiverDetails = response.Result as ZeoClient.Receiver;
                    SendMoneyReceiver receiver = new SendMoneyReceiver();
                    receiver.AddEdit = "Edit";
                    receiver.ReceiverId = long.Parse(ReceiverId);
                    receiver.FirstName = editReceiverDetails.FirstName;
                    receiver.LastName = editReceiverDetails.LastName;
                    receiver.SecondLastName = editReceiverDetails.SecondLastName;
                    receiver.StateProvince = editReceiverDetails.State_Province;
                    receiver.Address = editReceiverDetails.Address;
                    receiver.ZipCode = editReceiverDetails.ZipCode;
                    receiver.City = editReceiverDetails.City;
                    receiver.Phone = editReceiverDetails.PhoneNumber;
                    receiver.DeliveryMethod = Convert.ToString(editReceiverDetails.DeliveryMethod);
                    receiver.DeliveryOptions = Convert.ToString(editReceiverDetails.DeliveryOption);
                    receiver.PickUpCountry = editReceiverDetails.PickupCountry;
                    receiver.PickUpState = editReceiverDetails.PickupState_Province;
                    receiver.PickUpCity = editReceiverDetails.PickupCity;
                    Session["Receiver"] = receiver;
                    InitializeDropdownlistValues(receiver, new ZeoClient.ZeoServiceClient(), context);
                    return RedirectToAction("AddEditReceiver", "SendMoney", new { type = "Edit" });
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [CustomHandleErrorAttribute(ActionName = "SendMoney", ControllerName = "SendMoney", ResultType = "prepare")]
        public ActionResult SendMoney(bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;
                ZeoClient.ZeoContext context = GetZeoContext();
                if (string.IsNullOrWhiteSpace(context.WUCounterId))
                {
                    GetCustomerSessionCounterId((int)Helper.ProviderId.WesternUnion, context);
                }

                SendMoney sendMoney = new Models.SendMoney();
                if (TempData["ReceiverId"] != null)
                {
                    long receiverId = (long)TempData["ReceiverId"];
                    TempData.Keep("ReceiverId");
                    sendMoney = GetReceiverDetails(receiverId, context);
                }
                else
                {
                    InitializeDropdownsforSendMoney(sendMoney, new ZeoClient.ZeoServiceClient());
                }
                if (!string.IsNullOrWhiteSpace(context.WUCardNumber))
                {
                    sendMoney.WUCardNumber = context.WUCardNumber;
                }

                if (Session["SendMoneyRecord"] != null)
                {
                    SendMoney sendMoneyRecord = Session["SendMoneyRecord"] as SendMoney;
                    sendMoney.ActOnMyBehalf = sendMoneyRecord.ActOnMyBehalf;
                    sendMoney.SecondLastName = sendMoneyRecord.SecondLastName;
                    sendMoney.PersonalMessage = sendMoneyRecord.PersonalMessage;
                    sendMoney.AmountWithCurrency = sendMoneyRecord.AmountWithCurrency;
                    sendMoney.DestinationAmount = sendMoneyRecord.DestinationAmount;
                    sendMoney.Amount = sendMoneyRecord.Amount;
                    sendMoney.DestinationAmountWithCurrency = sendMoneyRecord.DestinationAmountWithCurrency;
                    sendMoney.CouponPromoCode = sendMoneyRecord.CouponPromoCode;
                }


                ViewBag.Navigation = NexxoSiteMap.SendMoney;

                return View("SendMoney", "_Common", sendMoney);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        //public JsonResult ReceiverByFullName(string fullName)
        //{
        //    try
        //    {
        //        //REVIEW: Still its using Desktop client.
        //        Desktop deskTop = new Desktop();
        //        MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
        //        CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

        //        DMS.Server.Data.Response response = deskTop.GetReceiverByFullName(Convert.ToInt64(customerSession.CustomerSessionId), fullName, mgiContext);
        //        if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
        //        Receiver receiver = response.Result as Receiver;

        //        long receiverId = receiver == null ? 0 : receiver.Id;
        //        return Json(receiverId, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        VerifyException(ex); return null;
        //    }
        //}

        /// <summary>
        /// This method auto-polulates the receiver details on click of 
        /// any of the Frequent Receivers links.
        /// </summary>
        /// <param name="ReceiverID"></param>
        /// <returns>ActionResult</returns>

        public ActionResult SelectReceiver(long ReceiverID)
        {
            ZeoClient.ZeoContext context = GetZeoContext();
            ViewBag.Navigation = NexxoSiteMap.SendMoney;
            return View("SendMoney", "_Common", GetReceiverDetails(ReceiverID, context));
        }


        /// <summary>
        /// This method will display confirmation pop-up while deleting a specific receiver from the favorite receiver list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DisplayDeleteFavReceiver(string id)
        {
            ViewBag.Id = id;
            return PartialView("_partialDeleteFrequentReceiver");
        }

        /// <summary>
        ///  This method will delete the specific receiver from favorite receiver list
        /// </summary>
        /// <param name="receiverId"></param>
        /// <returns></returns>
        public ActionResult DeleteFavoriteReceiver(string receiverId)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response response = new ZeoClient.Response();

                context.ProductType = "sendmoney";
                context.AgentId = (long)Session["agentId"];

                long customerSessionId = GetCustomerSessionId();

                response = alloyServiceClient.DeleteFavoriteReceiver(Convert.ToInt64(receiverId), context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                SendMoney sendMoney = new SendMoney();

                response = alloyServiceClient.GetFrequentReceivers(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                sendMoney.FrequentReceivers = response.Result as List<ZeoClient.Receiver>;

                return PartialView("_FrequentReceivers", sendMoney);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// Checks the notification details.
        /// </summary>
        /// <param name="receiverId">The receiver identifier.</param>
        /// <returns></returns>
        public JsonResult CheckNotificationDetails(long receiverId)
        {

            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Receiver receiver = new ZeoClient.Receiver();

                ZeoClient.Response response = alloyServiceClient.GetReceiver(receiverId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                receiver = response.Result as ZeoClient.Receiver;

                int isReceiverContactInfoRequired = 0;

                if (string.IsNullOrEmpty(receiver.Address) || string.IsNullOrEmpty(receiver.City) || string.IsNullOrEmpty(receiver.ZipCode)
                    || string.IsNullOrEmpty(receiver.State_Province) || string.IsNullOrEmpty(receiver.PhoneNumber))
                {
                    isReceiverContactInfoRequired = 1;
                }


                return Json(isReceiverContactInfoRequired, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// This method does the amount limit validations on the 
        /// model object & displays the confirm pick-up options screen.
        /// </summary>
        /// <param name="ReceiverID"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [CustomHandleError(ControllerName = "SendMoney", ActionName = "SendMoney", ResultType = "redirect")]
        public ActionResult Validate(SendMoney sendMoney)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response response = new ZeoClient.Response();

                Session["SendMoneyRecord"] = sendMoney;

                TempData["ReceiverId"] = null;
                if (sendMoney.ReceiverID > 0)
                {
                    TempData["ReceiverId"] = sendMoney.ReceiverID;
                }
                else if (sendMoney.ReceiverID == 0)
                {
                    ModelState.Remove("DestinationAmountWithCurrency");

                    SendMoneyReceiver receiver = new SendMoneyReceiver();
                    receiver.AddEdit = "Add";
                    receiver.FirstName = sendMoney.FirstName;
                    receiver.LastName = sendMoney.LastName;
                    receiver.SecondLastName = sendMoney.SecondLastName;

                    receiver.PickUpCountry = sendMoney.Country;
                    receiver.PickUpState = sendMoney.StateProvince;
                    receiver.PickUpCity = sendMoney.City;
                    receiver.DeliveryMethod = sendMoney.DeliveryMethod;
                    receiver.DeliveryOptions = sendMoney.DeliveryOptions;

                    Session["Receiver"] = receiver;
                    return RedirectToAction("AddEditReceiver", "SendMoney", new { type = "add" });
                }

                response = alloyServiceClient.GetXfrCountries(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                sendMoney.LCountry = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                if (!string.IsNullOrWhiteSpace(sendMoney.Country))
                {
                    response = alloyServiceClient.GetXfrStates(sendMoney.Country, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    sendMoney.LStates = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);
                }
                else
                {
                    sendMoney.LStates = DefaultSelectList();
                }

                if (!string.IsNullOrWhiteSpace(sendMoney.StateProvinceCode))
                {
                    response = alloyServiceClient.GetXfrCities(sendMoney.StateProvinceCode ?? string.Empty, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    sendMoney.LCities = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);
                }
                else
                {
                    sendMoney.LCities = DefaultSelectList();
                }

                //Get the CurrencyCode
                string currencyCode = string.Empty;
                if (!string.IsNullOrEmpty(sendMoney.Country))
                {
                    response = alloyServiceClient.GetCurrencyCode(sendMoney.Country, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    currencyCode = response.Result as string;
                }
                //WUStates
                string stateName = "";
                string cityName = "";
                string stateCode = "";

                if (sendMoney.StateProvince != null && sendMoney.LStates.Count() != 1)
                {
                    SelectListItem selectedState = sendMoney.LStates.Where(st => st.Value == sendMoney.StateProvince).FirstOrDefault();
                    if (selectedState != null)
                        stateName = selectedState.Text;
                    sendMoney.StateName = stateName;
                }
                if (sendMoney.City != null && sendMoney.LCities.Count() != 1)
                {
                    SelectListItem selectedCity = sendMoney.LCities.Where(ct => ct.Value == sendMoney.City).FirstOrDefault();
                    if (selectedCity != null)
                        cityName = selectedCity.Text;
                }

                sendMoney.LActOnMyBehalf = GetActBeHalfList();

                stateCode = string.IsNullOrEmpty(sendMoney.StateProvince) ? string.Empty : sendMoney.StateProvince;

                ModelState.Remove("TotalAmount");
                ModelState.Remove("PickUpMethodId");
                ModelState.Remove("DestinationAmountFromFeeEnquiry");
                ModelState.Remove("TransferAmount");
                ModelState.Remove("TotalToRecipient");
                ModelState.Remove("PickUpOptionsId");
                sendMoney.enableEditContinue = true;

                if (ModelState.IsValid)
                {
                    #region Commented Code
                    //context.Add("OriginatingCountryCode", "US");
                    //context.Add("OriginatingCurrencyCode", "USD");
                    //context.Add("DestinationCountryCode", sendMoney.Country);
                    //context.Add("DestinationCurrencyCode", currencyCode);

                    //context.Add("StateName", stateName);
                    //context.Add("StateCode", stateCode);
                    //context.Add("CityName", cityName);

                    //context.Add("SVCCode", sendMoney.DeliveryMethod); 
                    #endregion
                    context.StateCode = stateCode;

                    sendMoney.LDelivertyMethods = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Method, sendMoney.Country, currencyCode, stateName, stateCode, context, cityName);
                    sendMoney.LDeliveryOptions = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Option, sendMoney.Country, currencyCode, stateName, stateCode, context, cityName, sendMoney.DeliveryMethod);
                    sendMoney.LCountryCurrencies = GetCurrencyCodeList(GetCustomerSessionId(), sendMoney.Country, context);
                    sendMoney.enableEditContinue = true;

                    //Fixed_On_Send
                    if (sendMoney.Amount != null || sendMoney.Amount > 0)
                    {
                        sendMoney.IsFixedOnSend = true;
                    }
                    else
                    {
                        sendMoney.IsFixedOnSend = false;
                    }

                    ViewBag.Navigation = NexxoSiteMap.SendMoney;
                    if (sendMoney.CountryCode.ToLower() == "us" || sendMoney.CountryCode.ToLower() == "usa" || sendMoney.CountryCode.ToLower() == "united states")
                        sendMoney.isDomesticTransfer = true;

                    sendMoney.TransferAmount = Convert.ToDecimal(sendMoney.Amount);

                    sendMoney = GetFee(sendMoney, context);

                    Session.Remove("SendMoneyRecord");

                    //clear the view data
                    ViewData = new ViewDataDictionary();

                    return View("SendMoneyDetails", "_Common", sendMoney);
                }
                else
                {
                    string messages = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    if (!string.IsNullOrEmpty(messages))
                    {
                        ViewBag.IsException = true;
                        ViewBag.ExceptionMessage = HttpUtility.JavaScriptStringEncode(messages);
                    }
                    if (!string.IsNullOrWhiteSpace(context.WUCardNumber))
                    {
                        sendMoney.WUCardNumber = context.WUCardNumber;
                    }
                    return View("SendMoney", "_Common", sendMoney);
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        [CustomHandleError(ViewName = "SendMoneyDetails", MasterName = "_Common")]
        public ActionResult SendMoneyDetails(SendMoney sendMoney, string updateTrans, string next, string lpmtcontinue)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response serviceResponse = new ZeoClient.Response();
                ModelState.Remove("Amount");
                string stateName = "";
                string cityName = "";
                string stateCode = "";
                string countryName = "";
                long customerSessionId = GetCustomerSessionId();
                //Checking if Terms and condition pop-up is enabled based on provider and Product for a ChannelPartner
                bool isTncRequired = false;
                var product = sendMoney.Providers.FirstOrDefault(x => x.ProcessorName == "WesternUnion" && x.Name == "MoneyTransfer");
                if (product != null)
                {
                    isTncRequired = product.IsTnCForcePrintRequired;
                }
                ZeoClient.ZeoContext context = GetZeoContext();

                if (Session["SendMoneyModel"] != null && lpmtcontinue != null)
                {
                    sendMoney = Session["SendMoneyModel"] as SendMoney;
                    if (sendMoney.FirstName != null)
                        ModelState.Remove("FirstName");
                    if (sendMoney.LastName != null)
                        ModelState.Remove("LastName");
                    if (sendMoney.Country != null)
                        ModelState.Remove("Country");
                    if (sendMoney.DeliveryMethod != null)
                        ModelState.Remove("DeliveryMethod");
                    if (sendMoney.TransferAmount != 0)
                        ModelState.Remove("TransferAmount");
                    next = Session["next"] != null ? "Next" : null;
                    sendMoney.ProceedWithLPMTError = true;
                    stateName = sendMoney.PickupState;
                    cityName = sendMoney.CityName;
                    countryName = sendMoney.Country;
                    stateCode = sendMoney.StateProvinceCode;
                }
                else
                {
                    serviceResponse = alloyServiceClient.GetXfrCountries(context);
                    if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                    sendMoney.LCountry = GetSelectListItem(serviceResponse.Result as List<ZeoClient.MasterData>);

                    serviceResponse = alloyServiceClient.GetXfrStates(sendMoney.CountryCode, context);
                    if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                    sendMoney.LStates = GetSelectListItem(serviceResponse.Result as List<ZeoClient.MasterData>);

                    if (!string.IsNullOrWhiteSpace(sendMoney.StateProvinceCode))
                    {
                        serviceResponse = alloyServiceClient.GetXfrCities(sendMoney.StateProvinceCode ?? string.Empty, context);
                        if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                        sendMoney.LCities = GetSelectListItem(serviceResponse.Result as List<ZeoClient.MasterData>);
                    }
                    else
                    {
                        sendMoney.LCities = DefaultSelectList();
                    }
                    //Get the CurrencyCode
                    serviceResponse = alloyServiceClient.GetCurrencyCode(sendMoney.CountryCode, context);
                    if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                    string currencyCode = serviceResponse.Result as string;

                    if (sendMoney.StateProvince != null && sendMoney.LStates.Count() != 1)
                    {
                        SelectListItem selectedState = sendMoney.LStates.Where(st => st.Value == sendMoney.StateProvince).FirstOrDefault();
                        if (selectedState != null)
                            stateName = selectedState.Text;
                    }

                    if (sendMoney.City != null && sendMoney.LCities.Count() != 1)
                    {
                        SelectListItem selectedCity = sendMoney.LCities.Where(ct => ct.Value == sendMoney.City).FirstOrDefault();
                        if (selectedCity != null)
                            cityName = selectedCity.Text;
                    }

                    if (sendMoney.Country != null && sendMoney.LCountry.Count() != 1)
                    {
                        SelectListItem selectedCountry = sendMoney.LCountry.Where(ct => ct.Value == sendMoney.Country).FirstOrDefault();
                        if (selectedCountry != null)
                            countryName = selectedCountry.Text;
                    }

                    stateCode = string.IsNullOrEmpty(sendMoney.StateProvince) ? string.Empty : sendMoney.StateProvince;


                    sendMoney.LCountryCurrencies = GetCurrencyCodeList(GetCustomerSessionId(), sendMoney.CountryCode, context);

                    sendMoney.LDelivertyMethods = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Method, sendMoney.Country, currencyCode, stateName, stateCode, context, cityName);
                    sendMoney.LDeliveryOptions = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Option, sendMoney.Country, currencyCode, stateName, stateCode, context, cityName, sendMoney.DeliveryMethod);
                }
                serviceResponse = alloyServiceClient.GetFrequentReceivers(context);
                if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                sendMoney.FrequentReceivers = serviceResponse.Result as List<ZeoClient.Receiver>;
                sendMoney.enableEditContinue = true;
                ViewBag.Navigation = NexxoSiteMap.SendMoney;

                if (sendMoney.CountryCode.ToLower() == "us" || sendMoney.CountryCode.ToLower() == "usa" || sendMoney.CountryCode.ToLower() == "united states")
                {
                    sendMoney.isDomesticTransfer = true;
                    sendMoney.isDomesticTransferVal = "true";
                }
                else
                    sendMoney.isDomesticTransferVal = "false";

                ModelState.Remove("PickUpMethodId");
                ModelState.Remove("PickUpOptionsId");
                ViewBag.isAmountValid = ModelState.IsValidField("TransferAmount");
                ModelState.Remove("DestinationAmount");

                if (sendMoney.TestQuestionOption != null && sendMoney.TransferAmount < 300)
                {
                    if (sendMoney.TestQuestionOption != null)
                    {
                        if (sendMoney.TestQuestionOption.ToLower() == "p" || sendMoney.TestQuestionOption.ToLower() == "n")
                        {
                            ModelState.Remove("TestQuestion");
                            ModelState.Remove("TestAnswer");
                        }
                    }
                }
                else
                {
                    ModelState.Remove("TestQuestion");
                    ModelState.Remove("TestAnswer");
                }

                if (next != null && ModelState.IsValid)
                {
                    sendMoney.DeliveryMethodDesc = sendMoney.LDelivertyMethods.First(x => x.Value == sendMoney.DeliveryMethod).Text;
                    if (!string.IsNullOrEmpty(sendMoney.DeliveryOptions))
                        sendMoney.DeliveryOptionDesc = sendMoney.LDeliveryOptions.First(x => x.Value == sendMoney.DeliveryOptions).Text;
                    if (sendMoney.LDeliveryOptions.FirstOrDefault().Text == "Not Applicable")
                        sendMoney.DeliveryOptionDesc = "Not Applicable";

                    serviceResponse = alloyServiceClient.GetReceiver(sendMoney.ReceiverID, context);
                    if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                    ZeoClient.Receiver receiver = serviceResponse.Result as ZeoClient.Receiver;

                    sendMoney.PickUpLocation = receiver.Address;
                    if (sendMoney.LCountry.FirstOrDefault(x => x.Value == sendMoney.Country) != null)
                        sendMoney.Country = sendMoney.LCountry.FirstOrDefault(x => x.Value == sendMoney.Country).Text;

                    serviceResponse = alloyServiceClient.GetXfrStates(sendMoney.CountryCode, context);
                    if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                    sendMoney.LStates = GetSelectListItem(serviceResponse.Result as List<ZeoClient.MasterData>);

                    if (sendMoney.LStates.Count() > 1 && !string.IsNullOrWhiteSpace(sendMoney.StateProvinceCode))
                        sendMoney.StateProvince = sendMoney.LStates.FirstOrDefault(x => x.Value == sendMoney.StateProvinceCode).Text;
                    //DE2590
                    sendMoney.ReceiverCityName = receiver.City;
                    sendMoney.StateProvince = receiver.State_Province;
                    //US1896
                    sendMoney.PickupCity = cityName;
                    sendMoney.PickupState = stateName;
                    sendMoney.PickupCountry = countryName;

                    sendMoney.OriginalFee = Math.Round(Convert.ToDecimal((sendMoney.TransferFee + sendMoney.PromoDiscount) + sendMoney.OtherFees), 2);

                    ZeoClient.ValidateRequest validateRequest = new ZeoClient.ValidateRequest()
                    {
                        TransferType = ZeoClient.HelperMoneyTransferType.Send,
                        TransactionId = sendMoney.TransactionId,
                        ReceiverId = sendMoney.ReceiverID,

                        Amount = sendMoney.TransferAmount,
                        Fee = sendMoney.TransferFee,
                        Tax = sendMoney.TransferTax,
                        OtherFee = sendMoney.OtherFees,
                        MessageFee = sendMoney.MessageFee,

                        PersonalMessage = sendMoney.PersonalMessage,
                        PromoCode = sendMoney.CouponPromoCode,
                        IdentificationQuestion = sendMoney.TestQuestion,
                        IdentificationAnswer = sendMoney.TestAnswer,
                        DeliveryService = sendMoney.DeliveryOptions != null ? sendMoney.DeliveryOptions : sendMoney.DeliveryMethod,
                        State = sendMoney.PickupState,

                        ReceiverFirstName = sendMoney.FirstName,
                        ReceiverLastName = sendMoney.LastName,
                        ReceiverSecondLastName = string.IsNullOrWhiteSpace(sendMoney.SecondLastName) ? string.Empty : sendMoney.SecondLastName,

                        MetaData = new Dictionary<string, object>()
                    {
                        {"ExpectedPayoutCity", sendMoney.CityName},
                        {"ExpectedPayoutStateCode", sendMoney.StateProvinceCode},
                        {"ProceedWithLPMTError", sendMoney.ProceedWithLPMTError},
                        {"ReceiveAgentAbbr", sendMoney.ReceiveAgent}
                    }
                    };

                    serviceResponse = alloyServiceClient.Validate(validateRequest, context);
                    if (WebHelper.VerifyException(serviceResponse)) throw new ZeoWebException(serviceResponse.Error.Details);
                    ZeoClient.ValidateResponse response = serviceResponse.Result as ZeoClient.ValidateResponse;

                    if (response.HasLPMTError)
                    {
                        sendMoney.Amount = sendMoney.TransferAmount;
                        sendMoney.HasLPMTError = "true";
                        if (Session["SendMoneyModel"] != null)
                        {
                            Session.Remove("SendMoneyModel");
                            Session.Remove("next");
                        }
                        Session.Add("SendMoneyModel", sendMoney);
                        Session.Add("next", "Next");

                        ViewData = new ViewDataDictionary();
                        return View("SendMoneyDetails", sendMoney);
                    }
                    else
                    {
                        sendMoney.HasLPMTError = "false";
                        sendMoney.TransactionId = response.TransactionId;
                    }

                    //clear the view data
                    ViewData = new ViewDataDictionary();
                    sendMoney.TransferFee = sendMoney.TransferFee + sendMoney.OtherFees;

                    ViewBag.isTnCForcePrintRequired = isTncRequired;
                    if (Session["SendMoneyModel"] != null)
                    {
                        Session.Remove("SendMoneyModel");
                        Session.Remove("next");
                    }
                    return View("SendMoneyConfirm", sendMoney);
                }
                else
                {
                    sendMoney.Amount = sendMoney.TransferAmount;
                    sendMoney.HasLPMTError = "false";
                    ModelState.Remove("TransferAmount");
                    if (Session["SendMoneyModel"] != null)
                    {
                        Session.Remove("SendMoneyModel");
                        Session.Remove("next");
                    }

                    if (sendMoney.StateProvince != null && sendMoney.LStates.Count() != 1)
                    {
                        SelectListItem selectedState = sendMoney.LStates.Where(st => st.Value == sendMoney.StateProvince).FirstOrDefault();
                        if (selectedState != null)
                            sendMoney.StateName = selectedState.Text;
                    }
                    sendMoney = GetFee(sendMoney, context);

                }

                //clear the view data
                ViewData = new ViewDataDictionary();

                return View("SendMoneyDetails", sendMoney);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }


        [HttpPost]
        [CustomHandleError(ViewName = "SendMoneyConfirm", MasterName = "_Common")]
        public ActionResult SendMoneyConfirm(SendMoney sendMoney)
        {
            try
            {
                bool isTncRequired = false;
                var product = sendMoney.Providers.FirstOrDefault(x => x.ProcessorName == "WesternUnion" && x.Name == "MoneyTransfer");
                if (product != null)
                {
                    isTncRequired = product.IsTnCForcePrintRequired;
                }

                if (sendMoney.ProvidedTermsandConditonsMessage && sendMoney.ConsumerProtectionMessage && (sendMoney.isDomesticTransferVal == "true" || (sendMoney.isDomesticTransferVal == "false" && sendMoney.DoddFrankDisclosure)))
                {
                    if (TempData["sendMoneyModifyIds"] != null)
                    {
                        ZeoClient.ModifyResponse sendMoneyModifyIds = (ZeoClient.ModifyResponse)TempData["sendMoneyModifyIds"];

                        ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                        ZeoClient.ZeoContext context = GetZeoContext();

                        ZeoClient.ModifyRequest request = new ZeoClient.ModifyRequest() { CancelTransactionId = sendMoneyModifyIds.CancelTransactionId, ModifyTransactionId = sendMoneyModifyIds.ModifyTransactionId };

                        ZeoClient.Response response = alloyServiceClient.AuthorizeModifySendMoney(request, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                        TempData["sendMoneyModifyIds"] = null;

                        return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
                    }

                    return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
                }
                else
                {
                    ViewBag.isTnCForcePrintRequired = isTncRequired;
                    ViewBag.Navigation = NexxoSiteMap.TermsAndConditions;
                    return View("SendMoneyConfirm", sendMoney);
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult PickupOptions()
        {
            SendMoney selectreceiver = new SendMoney();
            selectreceiver = TempData["SelectReceiver"] as SendMoney;
            TempData.Keep("SelectReceiver");
            selectreceiver.PickUpLocation = string.Empty;
            ViewBag.Navigation = NexxoSiteMap.PickupOptions;
            return View("PickupOptions", "_Common", selectreceiver);
        }

        /// <summary>
        /// Action method for AutoCompleteReceiver
        /// </summary>
        /// <param name="term">The Parameter type of String for term</param>
        /// <returns>JsonResult</returns>
        //public JsonResult AutoCompleteReceiver(string term)
        //{
        //    //REVIEW: If we are not using this method please remove this code.
        //    MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
        //    Desktop deskTop = new Desktop();
        //    List<Receiver> ReceiverList = new List<Receiver>();
        //    List<string> namesList = new List<string>();

        //    try
        //    {
        //        CheckAmount getCustomerSessionId = new CheckAmount();

        //        DMS.Server.Data.Response response = deskTop.GetReceivers(getCustomerSessionId.CustomerSession.CustomerSessionId, term, mgiContext);
        //        if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
        //        ReceiverList = response.Result as List<Receiver>;
        //        ReceiverList = ReceiverList.FindAll(c => c.Status == "Active");
        //        // In the auto-populate list, show firstnames for "Active" receivers that contain the searchterm.
        //        foreach (var receiver in ReceiverList)
        //        {
        //            namesList.Add(receiver.FirstName + " " + receiver.LastName);
        //        }
        //        return Json(namesList, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ViewBag.ErrorMessage = ex.Message;
        //        //return Json(namesList, JsonRequestBehavior.AllowGet);
        //        VerifyException(ex); return null;
        //    }
        //}

        public JsonResult PopulateReceiverDetails(long ReceiverId)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            SendMoney sendMoney = new SendMoney();
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                long customerSessionId = GetCustomerSessionId();
               
                if (ReceiverId != 0)
                {
                    sendMoney = GetReceiverDetails(ReceiverId, context);
                }
                return Json(sendMoney, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult WUStates(string countryCode)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                long customerSessionId = GetCustomerSessionId();
                List<SelectListItem> states = new List<SelectListItem>();

                ZeoClient.Response response = alloyServiceClient.GetXfrStates(countryCode, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                states = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                return Json(states, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult WUCountryCurrencyCode(string countryCode)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.Response response = alloyServiceClient.GetCurrencyCode(countryCode, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                string currenyCode = response.Result as string;

                return Json(currenyCode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult WUCities(string stateCode)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                long customerSessionId = GetCustomerSessionId();
                List<SelectListItem> cities = new List<SelectListItem>();

                ZeoClient.Response response = alloyServiceClient.GetXfrCities(stateCode, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                cities = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);
                return Json(cities, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult WUDeliveryMethods(string countryCode, string state, string stateCode, string city)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                IEnumerable<SelectListItem> deliveryMethods = new List<SelectListItem>();

                ZeoClient.ZeoContext context = GetZeoContext();
                //Get the CurrencyCode
                ZeoClient.Response response = alloyServiceClient.GetCurrencyCode(countryCode, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                string currencyCode = response.Result as string;


                context.StateCode = stateCode;


                if (!countryCode.ToLower().Equals("select"))
                {
                    deliveryMethods = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Method, countryCode, currencyCode, state, stateCode, context, city);
                }

                return Json(deliveryMethods, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult WUDeliveryOptions(string countryCode, string state, string city, string svcCode)
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                IEnumerable<SelectListItem> deliveryOptions = new List<SelectListItem>();

                ZeoClient.ZeoContext context = GetZeoContext();
                //Get the Country CurrencyCode
                ZeoClient.Response response = alloyServiceClient.GetCurrencyCode(countryCode, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                string currencyCode = response.Result as string;



                if (!countryCode.ToLower().Equals("select"))
                {
                    deliveryOptions = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Option, countryCode, currencyCode, state, string.Empty, context, city, svcCode);
                }

                return Json(deliveryOptions, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult SaveReceiverConfirmation()
        {
            return PartialView("_SaveReceiverConfirmationMessage");
        }

        public ActionResult CreateReceiverAddlInfo()
        {
            Receivers receivers = new Receivers();
            //InitializeDropdownsforSendMoney(receivers, client);
            return View("AddEditReceiverContinue", "_Common", receivers);

        }

        public void InitializeDropdownsforSendMoney(SendMoney sendMoney, ZeoClient.ZeoServiceClient alloyServiceClient)
        {
            try
            {
                long customerSessionId = GetCustomerSessionId();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response response = new ZeoClient.Response();

                response = alloyServiceClient.GetFrequentReceivers(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                sendMoney.FrequentReceivers = response.Result as List<ZeoClient.Receiver>;

                response = alloyServiceClient.GetXfrCountries(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                sendMoney.LCountry = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                //Get the CurrencyCode
                response = alloyServiceClient.GetCurrencyCode(sendMoney.Country, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                string currencyCode = response.Result as string;

                //WUStates
                string stateName = "";
                string cityName = "";
                string stateCode = "";

                if (sendMoney.ReceiverID != 0)
                {
                    response = alloyServiceClient.GetXfrStates(sendMoney.Country, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    sendMoney.LStates = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                    if (!string.IsNullOrWhiteSpace(sendMoney.StateProvinceCode))
                    {
                        response = alloyServiceClient.GetXfrCities(sendMoney.StateProvinceCode, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        sendMoney.LCities = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);
                    }


                }
                else
                {
                    sendMoney.LStates = DefaultSelectList();
                    sendMoney.LCities = DefaultSelectList();
                }

                if (sendMoney.StateProvince != null && sendMoney.LStates.Count() > 1)
                {
                    SelectListItem selectedState = sendMoney.LStates.Where(st => st.Value == sendMoney.StateProvince).FirstOrDefault();
                    if (selectedState != null)
                        stateName = selectedState.Text;
                }

                if (sendMoney.City != null && sendMoney.LCities.Count() > 1)
                {
                    SelectListItem selectedCity = sendMoney.LCities.Where(ct => ct.Value == sendMoney.City).FirstOrDefault();
                    if (selectedCity != null)
                        cityName = selectedCity.Text;
                }

                stateCode = string.IsNullOrEmpty(sendMoney.StateProvince) ? string.Empty : sendMoney.StateProvince;



                sendMoney.LDelivertyMethods = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Method, sendMoney.Country, currencyCode, stateName, stateCode, context, cityName);
                sendMoney.LActOnMyBehalf = GetActBeHalfList();


                sendMoney.LDeliveryOptions = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Option, sendMoney.Country, currencyCode, stateName, stateCode, context, cityName, sendMoney.DeliveryMethod);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return;
            }
        }

        public ActionResult ShowTermsConditonDialog()
        {
            return PartialView("_partialWUTermsAndConditions");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setReceiver"></param>
        private ZeoClient.Receiver SetReceivers(SendMoneyReceiver setReceiver)
        {
            ZeoClient.Receiver newReceiver = new ZeoClient.Receiver();
            //Setting the receiver values
            if (setReceiver.ReceiverId == 0)
            {
                //newReceiver.rowguid = Guid.NewGuid();
            }
            else
            {
                newReceiver.Id = setReceiver.ReceiverId;
            }

            newReceiver.FirstName = setReceiver.FirstName;
            newReceiver.LastName = setReceiver.LastName;
            newReceiver.SecondLastName = setReceiver.SecondLastName;
            newReceiver.Status = "Active";
            newReceiver.Address = setReceiver.Address;
            newReceiver.City = setReceiver.City;
            newReceiver.State_Province = setReceiver.StateProvince;
            newReceiver.ZipCode = setReceiver.ZipCode;
            newReceiver.PhoneNumber = setReceiver.Phone;
            newReceiver.PickupCountry = setReceiver.PickUpCountry;
            newReceiver.PickupState_Province = setReceiver.PickUpState;
            newReceiver.PickupCity = setReceiver.PickUpCity;
            newReceiver.DeliveryMethod = setReceiver.DeliveryMethod == "Select" ? "" : setReceiver.DeliveryMethod;
            newReceiver.DeliveryOption = setReceiver.DeliveryOptions == "Select" ? "-1" : setReceiver.DeliveryOptions;
            return newReceiver;
        }

        private SendMoney GetFee(SendMoney sendMoney, ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

            ZeoClient.FeeRequest feeRequest = new ZeoClient.FeeRequest()
            {
                Amount = sendMoney.TransferAmount <= 0m ? 0.00m : Convert.ToDecimal(sendMoney.TransferAmount),
                ReceiveAmount = sendMoney.DestinationAmount == null ? 0.00m : Convert.ToDecimal(sendMoney.DestinationAmount),
                ReceiveCountryCode = sendMoney.CountryCode,
                ReceiveCountryCurrency = sendMoney.CurrencyType,
                TransactionId = sendMoney.TransactionId,
                IsDomesticTransfer = sendMoney.isDomesticTransfer,
                PromoCode = sendMoney.CouponPromoCode,
                ReceiverFirstName = sendMoney.FirstName,
                ReceiverLastName = sendMoney.LastName,
                ReceiverSecondLastName = sendMoney.SecondLastName,
                ReceiverId = sendMoney.ReceiverID,
                DeliveryService = new ZeoClient.DeliveryService()
                {
                    Code = sendMoney.DeliveryOptions != null ? sendMoney.DeliveryOptions : sendMoney.DeliveryMethod,
                    Name = sendMoney.DeliveryOptions != null ? sendMoney.DeliveryOptions : sendMoney.DeliveryMethod
                },
                PersonalMessage = sendMoney.PersonalMessage,
                MetaData = new Dictionary<string, object>()
            {
                {"CityCode", sendMoney.CityID},
                {"CityName", sendMoney.CityName},
                {"StateCode", sendMoney.StateProvinceCode},
                {"StateName", sendMoney.StateName},
                {"TestQuestionOption", sendMoney.TestQuestionOption},
                {"TestQuestion", sendMoney.TestQuestion},
                {"TestAnswer", sendMoney.TestAnswer},
                {"IsFixedOnSend", sendMoney.IsFixedOnSend}
            },
                ReferenceNo = sendMoney.ReferenceNo

            };

            ZeoClient.Response response = alloyServiceClient.GetFeeMoneyTransfer(feeRequest, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            ZeoClient.FeeResponse feeResponse = response.Result as ZeoClient.FeeResponse;

            ZeoClient.FeeInformation feeInformation = feeResponse.FeeInformations.FirstOrDefault();

            if (feeInformation != null)
            {

                decimal additionalCharge = feeInformation.MessageFee;

                if (feeInformation.MetaData != null)
                    additionalCharge = additionalCharge + Convert.ToDecimal(feeInformation.MetaData["PlusCharges"]);
                feeInformation.Tax = Convert.ToDecimal(feeInformation.MetaData["TransferTax"]); //AL-75
                sendMoney.TotalAmount = Math.Round(Convert.ToDecimal(feeInformation.Amount + feeInformation.Fee + feeInformation.Tax +
                        feeInformation.OtherFee + additionalCharge), 2);
                sendMoney.Amount = sendMoney.TotalAmount;
                sendMoney.TransferFee = feeInformation.Fee;

                //Author : Abhijith
                //Adding the additional charges for the Fee Component.
                //In Summary Receipt Additional Charges have not been added so added this code to add "additional charges". 
                //Starts Here
                sendMoney.TransferFee = sendMoney.TransferFee + additionalCharge + Convert.ToDecimal(feeInformation.Tax);
                //Ends Here

                //sendMoney.TransferFeeWithAllFees = Math.Round(Convert.ToDecimal(feeInformation.Fee + feeInformation.OtherFee + additionalCharge), 2);
                //Fix for AL-1307
                decimal plusCharges = 0.00m;
                if (feeInformation.MetaData != null)
                {
                    plusCharges = feeInformation.MetaData.ContainsKey("PlusCharges") ? Convert.ToDecimal(feeInformation.MetaData["PlusCharges"]) : 0;
                }
                sendMoney.TransferFeeWithAllFees = Math.Round(Convert.ToDecimal(feeInformation.Fee + feeInformation.OtherFee + plusCharges), 2);
                sendMoney.TransferTax = Convert.ToDecimal(feeInformation.Tax);
                sendMoney.PromoDiscount = feeInformation.Discount;
                sendMoney.TransferAmount = Convert.ToDecimal(feeInformation.Amount);
                sendMoney.ExchangeRate = Convert.ToDecimal(feeInformation.ExchangeRate);
                sendMoney.PromoName = feeRequest.PromoCode;
                sendMoney.ReferenceNo = feeInformation.ReferenceNumber;
                sendMoney.AmountWithCurrency = string.Format("$ {0} USD", sendMoney.Amount);
                sendMoney.OriginalFee = Math.Round(Convert.ToDecimal((sendMoney.TransferFee + sendMoney.PromoDiscount) + sendMoney.OtherFees), 2);

                sendMoney.TransferFeeWithCurrency = string.Format("$ {0} USD", sendMoney.TransferFeeWithAllFees + sendMoney.PromoDiscount);
                sendMoney.PromoDiscountWithCurrency = (sendMoney.PromoDiscount != 0.00m) ? string.Format("$ -{0} USD", sendMoney.PromoDiscount) : string.Format("$ {0} USD", sendMoney.PromoDiscount);
                sendMoney.TransferAmountWithCurrency = string.Format("$ {0} USD", sendMoney.TransferAmount);
                sendMoney.TransferTaxWithCurrency = string.Format("$ {0} USD", sendMoney.TransferTax);
                sendMoney.DestinationAmountFromFeeEnquiry = feeInformation.ReceiveAmount;
                if (sendMoney.isDomesticTransfer)
                {
                    sendMoney.ExchangeRateConversion = "Not Applicable";
                    sendMoney.DestinationAmount = 0;
                    sendMoney.DestinationAmountWithCurrency = "Not Applicable";
                    sendMoney.TotalToRecipientWithCurrency = "Not Applicable";// make one string variable to hold this
                    sendMoney.DestinationAmountWithCurrency1 = "Not Applicable";
                }
                else
                {
                    sendMoney.ExchangeRateConversion = string.Format("1.00 USD = {0} {1}", sendMoney.ExchangeRate.ToString(), sendMoney.CurrencyType);
                    sendMoney.DestinationAmount = feeInformation.ReceiveAmount;
                    sendMoney.TotalToRecipient = Math.Round(sendMoney.DestinationAmount ?? 0 - (sendMoney.OtherFees * sendMoney.ExchangeRate), 2);
                    sendMoney.TotalToRecipientWithCurrency = string.Format("{0} {1}", sendMoney.TotalToRecipient.ToString(), sendMoney.CurrencyType);
                    sendMoney.DestinationAmountWithCurrency = string.Format("{0} {1}", sendMoney.DestinationAmount, sendMoney.CurrencyType);
                    sendMoney.DestinationAmountWithCurrency1 = sendMoney.DestinationAmount + " " + sendMoney.CurrencyType;
                }

                sendMoney.OtherFees = Convert.ToDecimal(feeInformation.OtherFee);
                sendMoney.OtherFeesWithCurrency = string.Format("{0} {1}", 0, sendMoney.CurrencyType);//as this reflecting in transfertax as USD, not displaying here. 27thfeb2014 -supreetha
                sendMoney.TestQuestionOption = Convert.ToString(feeResponse.MetaData["TestQuestionOption"]);
                sendMoney.TransactionId = feeResponse.TransactionId;
                sendMoney.IsFixedOnSend = sendMoney.IsFixedOnSend;

                // Message Fee Charges is added to the Send Money Message Charge for User Story # US1684
                sendMoney.MessageFee = feeInformation.MessageFee;
            }

            return sendMoney;
        }

        private SendMoney GetReceiverDetails(long receiverId, ZeoClient.ZeoContext context)
        {
            ZeoClient.Response response = new ZeoClient.Response();
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.Receiver receiver = new ZeoClient.Receiver();
            SendMoney sendMoney = new SendMoney();

            response = alloyServiceClient.GetFrequentReceivers(context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            sendMoney.FrequentReceivers = response.Result as List<ZeoClient.Receiver>;

            response = alloyServiceClient.GetReceiver(receiverId, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            receiver = response.Result as ZeoClient.Receiver;

            response = alloyServiceClient.GetXfrCountries(context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            sendMoney.LCountry = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

            string currencyCode = string.Empty;
            if (receiver != null)
            {
                sendMoney.FirstName = receiver.FirstName;
                sendMoney.LastName = receiver.LastName;
                sendMoney.SecondLastName = receiver.SecondLastName; // has to change to second lastname
                sendMoney.ReceiverName = receiver.FirstName + " " + receiver.LastName;
                sendMoney.ReceiverID = receiver.Id;

                sendMoney.CountryCode = receiver.PickupCountry;
                sendMoney.Country = receiver.PickupCountry;

                response = alloyServiceClient.GetCurrencyCode(sendMoney.CountryCode, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                currencyCode = response.Result as string;
                sendMoney.CurrencyType = currencyCode;

                if (!string.IsNullOrEmpty(receiver.PickupCountry) && (receiver.PickupCountry == "US" || receiver.PickupCountry == "MX" || receiver.PickupCountry == "CA"))
                {
                    response = alloyServiceClient.GetXfrStates(receiver.PickupCountry, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    sendMoney.LStates = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);

                    if (!string.IsNullOrWhiteSpace(receiver.PickupState_Province))
                    {
                        response = alloyServiceClient.GetXfrCities(receiver.PickupState_Province ?? string.Empty, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        sendMoney.LCities = GetSelectListItem(response.Result as List<ZeoClient.MasterData>);
                    }
                    else
                    {
                        sendMoney.LCities = DefaultSelectList();
                    }
                }
                else
                {
                    sendMoney.LStates = DefaultSelectList();
                    sendMoney.LCities = DefaultSelectList();
                }

                if (receiver.PickupCountry == "US" || receiver.PickupCountry == "MX" || receiver.PickupCountry == "CA")
                {
                    sendMoney.StateProvinceCode = receiver.PickupState_Province;
                    if (!string.IsNullOrEmpty(receiver.PickupState_Province))
                    {
                        var stateProvince = sendMoney.LStates.ToList<SelectListItem>().Find(c => c.Value == receiver.PickupState_Province);

                        if (stateProvince == null)
                        {
                            sendMoney.StateProvince = "";
                        }
                        else
                        {
                            sendMoney.StateProvince = stateProvince.Value;
                        }
                    }

                    if (receiver.PickupCountry == "MX")
                    {
                        if (!string.IsNullOrEmpty(receiver.PickupCity) && receiver.PickupCity != "Select")
                        {
                            sendMoney.CityID = sendMoney.LCities.ToList<SelectListItem>().Find(c => c.Text == receiver.PickupCity).Value;
                            sendMoney.City = sendMoney.LCities.ToList<SelectListItem>().Find(c => c.Text == receiver.PickupCity).Text;
                            sendMoney.CityName = sendMoney.LCities.ToList<SelectListItem>().Find(c => c.Text == receiver.PickupCity).Text;
                        }
                    }
                    else
                    {
                        sendMoney.City = "Not Applicable";
                    }
                }

                //WUStates
                string stateName = "";
                string cityName = "";
                string stateCode = "";
                if (receiver.PickupState_Province != null && sendMoney.LStates.Count() != 1)
                {
                    SelectListItem selectedState = sendMoney.LStates.Where(st => st.Value == receiver.PickupState_Province).FirstOrDefault();
                    if (selectedState != null)
                        stateName = selectedState.Text;
                }

                if (receiver.PickupCity != null && sendMoney.LCities.Count() != 1)
                {
                    SelectListItem selectedCity = sendMoney.LCities.Where(ct => ct.Value == receiver.PickupCity).FirstOrDefault();
                    if (selectedCity != null)
                        cityName = selectedCity.Text;
                }

                stateCode = string.IsNullOrEmpty(receiver.PickupState_Province) ? string.Empty : receiver.PickupState_Province;

                sendMoney.LDelivertyMethods = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Method, sendMoney.Country, currencyCode, stateName, stateCode, context, cityName);

                var deliveryMethods = sendMoney.LDelivertyMethods;
                var deliveryMethod = deliveryMethods.Where(c => c.Value == receiver.DeliveryMethod).FirstOrDefault();
                if (deliveryMethod != null)
                {
                    sendMoney.PickUpMethodId = receiver.DeliveryMethod;
                    sendMoney.DeliveryMethod = deliveryMethods.Where(c => c.Value == receiver.DeliveryMethod).FirstOrDefault().Value;
                }

                sendMoney.LDeliveryOptions = PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType.Option, sendMoney.Country, currencyCode, stateName, stateCode, context, cityName, sendMoney.DeliveryMethod);

                var deliveryOptions = sendMoney.LDeliveryOptions;
                var deliveryOption = receiver.DeliveryOption != null ? deliveryOptions.Where(c => c.Value == receiver.DeliveryOption.ToString()).FirstOrDefault() : null;
                if (deliveryOption != null)
                {
                    sendMoney.PickUpOptionsId = receiver.DeliveryOption.ToString();
                    sendMoney.DeliveryOptions = deliveryOption.Value;
                }

                sendMoney.enableEditContinue = true;

                sendMoney.LActOnMyBehalf = GetActBeHalfList();
            }

            return sendMoney;
        }

        public JsonResult CancelSendMoneyDetails(long Id, string ScreenName)
        {
            try
            {
                if (Id != 0)
                {
                    ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                    ZeoClient.ZeoContext context = GetZeoContext();
                    ZeoClient.Response response = new ZeoClient.Response();

                    ZeoClient.ModifyResponse sendMoneyModifyIds = (ZeoClient.ModifyResponse)TempData["sendMoneyModifyIds"];

                    if (sendMoneyModifyIds != null)
                    {
                        response = alloyServiceClient.CancelSendMoneyModify(sendMoneyModifyIds.ModifyTransactionId, sendMoneyModifyIds.CancelTransactionId, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    }
                    else if (ScreenName.ToLower() == "sendmoneyconfirm")
                    {
                        response = alloyServiceClient.RemoveMoneyTransfer(Id, (int)Helper.ProductType.SendMoney, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    }
                    else
                    {
                        response = alloyServiceClient.CancelXfer(Id, context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    }
                }

                ViewBag.Navigation = NexxoSiteMap.ProductInformation;
                var jsonData = new { success = true };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// This ImportPast Button will enable DMS to Import Past Receivers for a particular customer from WU and add/modify tWUnion_Receiver Table.
        /// </summary>
        /// <param name="productName">Product Name</param>
        /// <returns></returns>
        public ActionResult ImportPastReceiver(string productName)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                #region Added this code block for User Story # US1645 for Past Receivers

                ZeoClient.ZeoContext context = (ZeoClient.ZeoContext)Session["ZeoContext"];
                context.ProductType = "sendmoney";
                ZeoClient.Response response = new ZeoClient.Response();

                context.AgentId = (long)Session["agentId"];

                if (!string.IsNullOrWhiteSpace(context.WUCardNumber))
                {
                    response = alloyServiceClient.AddPastReceivers(context.WUCardNumber, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                }

                #endregion

                // This will re-create partial view for FrequentReceivers for Send Money Screen.
                SendMoney sendMoney = new SendMoney();
                response = alloyServiceClient.GetFrequentReceivers(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                sendMoney.FrequentReceivers = response.Result as List<ZeoClient.Receiver>;
                return PartialView("_FrequentReceivers", sendMoney);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult ShowLPMTDialog()
        {
            return PartialView("_LPMTPopup");
        }

        /// <summary>
        /// Author: Abhijith
        /// Bug : AL-2014
        /// Description : Added a method to display the pop up - "Retry" and "Cancel Transaction".
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult DisplayDoddFrankMessage(long dt, string msg = "")
        {
            if (string.IsNullOrEmpty(msg))
                msg = "Unhandled Error";
            string[] str = splitstring(msg);

            SystemMessage sysmsg = new SystemMessage()
            {
                Type = str[0],
                Number = str[1],
                Message = str[2],
                AddlDetails = str[3]
            };

            ViewBag.IsException = false;
            ViewBag.ExceptionMessage = null;

            return PartialView("_SendMoneyPDSPrinterWarning", sysmsg);
        }

        /// <summary>
        /// Author: Abhijith
        /// Bug : AL-2014
        /// Description : Added a method to display the pop up - "Retry" and "Cancel Transaction".
        /// TODO: Move this to common place as this is used in CancelTransactionController also.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string[] splitstring(string message)
        {
            string[] strmessage = null;

            if (message.Contains("|"))
                strmessage = message.Split(new string[] { "|" }, StringSplitOptions.None);
            else
            {
                string errorCode = "1000.100.9999";
                string errorMessage = GetErrorMessage(errorCode, GetZeoContext());
                strmessage = errorMessage.Split(new string[] { "|" }, StringSplitOptions.None);
                strmessage[2] = message;
            }

            return strmessage;
        }

        private IEnumerable<SelectListItem> PopulateDeliveryServices(ZeoClient.HelperDeliveryServiceType type, string countryCode,
            string currencyCode, string state, string stateCode, ZeoClient.ZeoContext context, string cityName = "", string deliveryMethod = "")
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.DeliveryServiceRequest request = new ZeoClient.DeliveryServiceRequest()
            {
                Type = type,
                CountryCode = countryCode,
                CountryCurrency = currencyCode,
                MetaData = new Dictionary<string, object>()
                {
                    {"State", state},
                    {"StateCode", stateCode},
                    {"City", cityName},
                    {"DeliveryService", deliveryMethod}
                }
            };

            ZeoClient.Response response = alloyServiceClient.GetDeliveryServices(request, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            List<ZeoClient.DeliveryService> services = response.Result as List<ZeoClient.DeliveryService>;

            services.Insert(0, new ZeoClient.DeliveryService() { Name = "Select", Code = string.Empty });
            if (ZeoClient.HelperDeliveryServiceType.Option == type && services.Count <= 1)
            {
                ZeoClient.DeliveryService _deliveryService = new ZeoClient.DeliveryService
                {
                    Code = "",
                    Name = "Not Applicable"
                };
                services.Clear();
                services.Insert(0, _deliveryService);
            }

            return services.Select(d => new SelectListItem() { Text = d.Name, Value = d.Code });
        }

        public ActionResult ShowCancelReceiverPopUp()
        {
            return PartialView("_SendMoneyReceiverCancelPopUp");
        }

    }
}
