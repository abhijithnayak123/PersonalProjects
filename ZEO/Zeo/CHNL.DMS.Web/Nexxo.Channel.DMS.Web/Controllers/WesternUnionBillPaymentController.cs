using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion


namespace TCF.Channel.Zeo.Web.Controllers
{
    public class WesternUnionBillPaymentController : BillPaymentBaseController
    {
        [HttpGet]
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ProductInfo")]
        public ActionResult BillPayment(bool IsException = false, string ExceptionMessage = "", string billerName = "")
        {
            try
            {
                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoContext context = GetZeoContext();
                if (string.IsNullOrWhiteSpace(context.WUCounterId))
                {
                    GetCustomerSessionCounterId((int)Helper.ProviderId.WesternUnion, context);
                }
                Session["isCashierAgree"] = "true";
                Session["activeButton"] = "billpayment";
                if (!IsException)
                Session["billFee"] = null;
                string viewName = "";
                string masterName = "_Common";
                object model = new object();
                ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];

                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;

                if (IsException || (!string.IsNullOrWhiteSpace(context.WUCardNumber) || TempData["SkipGoldCard"] != null))
                {
                    if (string.IsNullOrWhiteSpace(customerSession.TipsAndOffers))
                    {
                        context.ProductType = "billpay";
                        PopulatTipsAndOffersMessage(serviceClient, customerSession, context);
                    }
                    Models.WesternUnionBillPayViewModel billPayment = new Models.WesternUnionBillPayViewModel();

                    if (Session["BillPaymentRecord"] != null)
                    {
                        billPayment = (Models.WesternUnionBillPayViewModel)Session["BillPaymentRecord"];
                        Session["BillPaymentRecord"] = null;
                        ZeoClient.BillPayFee billPayFee = GetBillPayFeeFromSession();
                        if (billPayment != null)
                        {
                            response = GetLocations(billPayFee.TransactionId, billPayment.BillerName, billPayment.AccountNumber, billPayment.BillAmount, context);
                            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                            ZeoClient.BillPayLocation BillLocations = response.Result as ZeoClient.BillPayLocation;


                            List<SelectListItem> locationList = new List<SelectListItem>();
                            List<SelectListItem> deliveryMethods = new List<SelectListItem>();

                            if (BillLocations.BillerLocation != null && BillLocations.BillerLocation.Count > 0)
                                BillLocations.BillerLocation.ForEach(i => locationList.Add(new SelectListItem() { Text = i.Name, Value = i.Type, Selected = (i.Name == billPayment.BillerLocationName) }));

                            else
                                locationList.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable", Selected = true });


                            ZeoClient.BillPayFee billFee = GetBillPayFeeFromSession();

                            billFee.DeliveryMethods.ForEach(i => deliveryMethods.Add(new SelectListItem() { Value = i.Code, Text = i.Text, Selected = (i.Code == billPayment.BillerDeliveryMethod) }));

                            billPayment.LLocations = locationList;
                            billPayment.LDeliveryMethods = deliveryMethods;

                        }
                    }
                    else
                    {
                        billPayment.LLocations = billPayment.LDeliveryMethods = DefaultSelectList();
                    }

                    billPayment.ProviderName = Helper.ProviderId.WesternUnionBillPay.ToString();

                    ZeoClient.Response frequentBillerResponse = serviceClient.GetFrequentBillers(context);

                    if (WebHelper.VerifyException(frequentBillerResponse)) throw new ZeoWebException(frequentBillerResponse.Error.Details);

                    billPayment.FrequentBillPayees = GetFrequentBillers(frequentBillerResponse);

                    ViewBag.Navigation = Resources.NexxoSiteMap.BillPayment;
                    ViewBag.IsWUEnrolledCustomer = !string.IsNullOrWhiteSpace(context.WUCardNumber) ? true : false;

                    model = billPayment;

                    viewName = billPayment.ProviderName;
                }
                else
                {
                    ViewBag.IsBillPay = true;

                    Session["lookupGoldCard"] = null;

                    WesternUnionDetails wuDetailsModel = new WesternUnionDetails();

                    wuDetailsModel.EditGoldCardFrom = "billpay";

                    viewName = "EnrollWesternUnionGoldCard";

                    model = wuDetailsModel;
                }
                return View(viewName, masterName, model);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult PopulateBillPayeeLocation(string billPayeeName, string accountNumber, decimal amount)
        {
            try
            {          
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

                ZeoClient.BillPayFee billPayFee = GetBillPayFeeFromSession();                

                ZeoClient.Response response = GetLocations(billPayFee.TransactionId, billPayeeName, accountNumber, amount, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                ZeoClient.BillPayLocation BillLocations = response.Result as ZeoClient.BillPayLocation;

                billPayFee.TransactionId = BillLocations.TransactionId;

                Session["billFee"] = billPayFee;

                List<SelectListItem> locationsList = new List<SelectListItem>();

                BillLocations.BillerLocation.ForEach(i => locationsList.Add(new SelectListItem() { Value = i.Type, Text = i.Name }));

                return Json(locationsList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult PopulateBillDeliveryMethod(string billPayeeName, string accountNumber, decimal amount, string location, string locationType)
        {
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

                List<SelectListItem> deliveryMethods = new List<SelectListItem>();
                ZeoClient.BillerLocation billerLocation = null;
                if (!string.IsNullOrWhiteSpace(location.Trim()) && location != "Not Applicable")
                {
                    billerLocation = new ZeoClient.BillerLocation()
                    {
                        Name = location,
                        Type = locationType
                    };
                }

                ZeoClient.BillPayFee billPayFee = GetBillPayFeeFromSession();        

                response = alloyServiceClient.GetBillPayFee(billPayFee.TransactionId, billPayeeName, accountNumber, amount, billerLocation, context);

                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                billPayFee = response.Result as ZeoClient.BillPayFee;

                Session["billFee"] = billPayFee;

                billPayFee.DeliveryMethods.ForEach(i => deliveryMethods.Add(new SelectListItem() { Value = i.Code, Text = i.Text }));

                var jsonData = new
                {
                    SessionCookie = billPayFee.SessionCookie,
                    DeliveryMethods = deliveryMethods,
                    accountHolderName = billPayFee.AccountHolderName,
                    avaliableBalance = billPayFee.AvailableBalance,
                    TransactionID = billPayFee.TransactionId
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult PopulateBillFee(string deliveryMethod)
        {
            try
            {
                ZeoClient.BillPayFee billFee = GetBillPayFeeFromSession();

                ZeoClient.DeliveryMethod selectedDeliveryMethod = billFee.DeliveryMethods.FirstOrDefault(c => c.Code == deliveryMethod);

                decimal billPaymentFee = 0;

                if (selectedDeliveryMethod != null)
                    billPaymentFee = selectedDeliveryMethod.FeeAmount;

                return Json(billPaymentFee, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public JsonResult GetProviderAttributes(string billerName, string location)
        {
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();

                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

                location = string.Compare(location, "Not Applicable", true) == 0 ? null : location;

                ZeoClient.Response response = alloyServiceClient.GetProviderAttributes(billerName, location, context);

                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                List<ZeoClient.Field> fields = response.Result as List<ZeoClient.Field>;

                return Json(fields, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [HttpPost]
        [CustomHandleErrorAttribute(ActionName = "BillPayment", ControllerName = "WesternUnionBillPayment", ResultType = "redirect")]
        public ActionResult BillPayment(Models.WesternUnionBillPayViewModel billPayment)
        {
            try
            {
                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoContext context = GetZeoContext();
                context.PromotionId = billPayment.PromotionId;

                billPayment.LLocations = billPayment.LDeliveryMethods = DefaultSelectList();

                ZeoClient.BillPayFee billFee = GetBillPayFeeFromSession();

                long transactionId = billFee.TransactionId;

                Session["BillPaymentRecord"] = billPayment;

                ZeoClient.BillPayment billPay = Mapper(billPayment);
                billPay.CityCode = billFee.CityCode;

                response = serviceClient.ValidateBillPayment(transactionId, billPay, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                ZeoClient.BillPayValidateResponse validateResponse = response.Result as ZeoClient.BillPayValidateResponse;


                BillpaymentReview billPaymentReview = new BillpaymentReview()
                {
                    BillpayTransactionId = validateResponse.TransactionId,
                    BillerName = billPayment.BillerName,
                    BillerLocationName = billPayment.BillerLocationName,
                    BillerLocationId = billPayment.BillerLocation,
                    BillPaymentAccount = billPayment.AccountNumber.MaskAccountNumber(),
                    BillPaymentDeliveryMethodCode = billPayment.BillerDeliveryMethod,
                    BillPaymentDeliveryMethod = billPayment.SelectedDeliveryMethod,
                    BillpaymentAmount = billPayment.BillAmount,
                    BillPaymentFee = (validateResponse != null) ? validateResponse.Fee : 0.0M,
                    SenderName = validateResponse.SenderFirstName + " " + validateResponse.SenderLastname,
                    SenderAddress1 = validateResponse.SenderAddressLine1 + " " + validateResponse.SenderAddressLine2,
                    SenderAddress2 = validateResponse.SenderCity + ", " + validateResponse.SenderState + " " + validateResponse.SenderPostalCode,
                    SenderEmail = validateResponse.SenderEmail,
                    SenderPhoneNumber = validateResponse.SenderContactPhone,
                    SenderWUGoldcardNumber = string.IsNullOrWhiteSpace(context.WUCardNumber) ? "NA" : context.WUCardNumber,
                    CouponPromoCode = billPayment.CouponPromoCode,
                    DiscountedFee = validateResponse.DiscountedFee,
                    UnDiscountedFee = validateResponse.UnDiscountedFee
                };

                Session.Remove("BillPaymentRecord");

                ViewData = new ViewDataDictionary();

                return View("BillPayReview", "_Common", billPaymentReview);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.ProductInfo")]
        public ActionResult CancelBillPayDetails(long Id)
        {
            try
            {
                if (Id != 0)
                {
                    ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();

                    ZeoClient.ZeoContext context = GetZeoContext();

                    ZeoClient.Response response = serviceClient.CancelBillPayment(Id, context);
                }

                ViewBag.Navigation = Resources.NexxoSiteMap.ProductInformation;

                return RedirectToAction("ProductInformation", "Product");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        [CustomHandleErrorAttribute(ViewName = "BillPayReview", MasterName = "_Common")]
        public ActionResult BillPaySubmit(BillpaymentReview billpaymentReview)
        {
            try
            {
                if (billpaymentReview.ConsumerProtectionMessage)
                {
                    ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                    ZeoClient.ZeoContext context = GetZeoContext();

                    context.RequestType = Helper.RequestType.Hold.ToString();

                    ZeoClient.Response response = serviceClient.StageBillPayment(billpaymentReview.BillpayTransactionId, context);

                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                    return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
                }
                else
                {
                    ViewBag.ErrorMessage = "Please select the checkbox";
                    return View("BillPayReview", billpaymentReview);
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult DeleteFavoriteBiller(string billerID)
        {
            try
            {
                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response frequentBillerResponse = new ZeoClient.Response();
                ZeoClient.ZeoContext context = GetZeoContext();

                frequentBillerResponse = serviceClient.DeleteFavoriteBiller(Convert.ToInt64(billerID), context);
                if (WebHelper.VerifyException(frequentBillerResponse)) throw new ZeoWebException(frequentBillerResponse.Error.Details);

                Models.BillPaymentViewModel billPayment = new BillPaymentViewModel();
                billPayment.FrequentBillPayees = GetFrequentBillers(frequentBillerResponse);

                return PartialView("_partialFrequentPayees", billPayment);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult ImportPastBiller(string productName)
        {
            try
            {
                ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoContext context = GetZeoContext();

                if (!string.IsNullOrWhiteSpace(context.WUCardNumber))
                {
                    response = serviceClient.AddPastBillers(context.WUCardNumber, context);
                }

                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                Models.BillPaymentViewModel billPayment = new BillPaymentViewModel();

                billPayment.FrequentBillPayees = GetFrequentBillers(response);

                return PartialView("_partialFrequentPayees", billPayment);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult DisplayDeleteFavBiller(string id)
        {
            ViewBag.Id = id;
            return PartialView("_partialDeleteFavoriteBiller");
        }

        public object GetDictionaryValue(Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key) == false)
            {
                throw new Exception(String.Format("{0} not provided in dictionary", key));
            }
            return dictionary[key];
        }


        #region Private Methods

        private ZeoClient.BillPayment Mapper(Models.WesternUnionBillPayViewModel billpayment)
        {
            return new ZeoClient.BillPayment()
            {
                PaymentAmount = billpayment.BillAmount,
                Fee = billpayment.BillPaymentFee,
                BillerName = billpayment.BillerName,
                AccountNumber = billpayment.AccountNumber,
                CouponCode = billpayment.CouponPromoCode,
                BillerId = billpayment.BillerId,
                MetaData = new Dictionary<string, object>()
                        {
                            {"DeliveryCode", billpayment.BillerDeliveryMethod},
                            {"Location", billpayment.BillerLocationName},
                            {"SessionCookie", billpayment.SessionCookie},
                            {"Reference",billpayment.Reference},
                            {"AailableBalance",billpayment.AvailableBalance},
                            {"AccountHolder",billpayment.AccountHolder},
                            {"Attention",billpayment.Attention},
                            {"DateOfBirth",billpayment.DateOfBirth}
                        }
            };
        }

        private ZeoClient.BillPayFee GetBillPayFeeFromSession()
        {
            ZeoClient.BillPayFee billPayFee = new ZeoClient.BillPayFee();

            if (Session["billFee"] != null)
                billPayFee = Session["billFee"] as ZeoClient.BillPayFee;

            return billPayFee;
        }

        private List<FavouriteBiller> GetFrequentBillers(ZeoClient.Response frequentBillerResponse)
        {
            return ((List<ZeoClient.FavoriteBiller>)frequentBillerResponse.Result)
                                                       .Select(x => new FavouriteBiller()
                                                       {
                                                           AccountNumber = x.AccountNumber,
                                                           BillerCode = x.BillerCode,
                                                           BillerName = x.BillerName,
                                                           ChannelPartnerId = x.ChannelPartnerId,
                                                           ProductId = x.ProductId,
                                                           ProviderId = x.ProviderId,
                                                           ProviderName = x.ProviderName,
                                                           TenantId = x.TenantId
                                                       }).ToList();
        }

        private ZeoClient.Response GetLocations(long transactionId, string billerName, string accountNumber, decimal amount, ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient serviceClient = new ZeoClient.ZeoServiceClient();
            return serviceClient.GetLocations(transactionId, billerName, accountNumber, Convert.ToInt64(amount), context);
        }

        #endregion
    }
}
