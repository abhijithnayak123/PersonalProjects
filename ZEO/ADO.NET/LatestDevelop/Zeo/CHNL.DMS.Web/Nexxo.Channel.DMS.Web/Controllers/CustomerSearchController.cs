using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using System.Linq.Expressions;

using TCF.Channel.Zeo.Web.Common;
using TCF.Zeo.Security.Voltage;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Logging.Impl;
using TCF.Zeo.Common.Util;
using static TCF.Zeo.Common.Util.Helper;
using System.Globalization;
#endregion


namespace TCF.Channel.Zeo.Web.Controllers
{
    public class CustomerSearchController : BaseController
    {
        public NLoggerCommon NLogger = new NLoggerCommon();

        #region Customer search methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CustomerSearch")]
        public ActionResult CustomerSearch(bool IsException = false, string ExceptionMessage = "", bool CounterIdAssigned = false)
        {
            try
            {
                NLogHelper.Info("CustomerSearch:");
                long agentSessionId = GetAgentSessionId();

                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

                if (CounterIdAssigned)
                {
                    UpdateCounterId(CounterIdAssigned);
                    UpdateCashInStatus();

                }

                //Updating the AlloyContext after closing the customer session as application is writing the logs in the same customer session even after it is closed.
                ZeoClient.Response response = alloyServiceClient.GetZeoContextForAgent(agentSessionId, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                context = response.Result as ZeoClient.ZeoContext;
                context.SSOAttributes = GetSSOAttributes("SSO_AGENT_SESSION");
                Session["ZeoContext"] = context;

                ViewBag.CriteriaTag = false;

                Session["activeButton"] = "home";
                Session["CustomerSession"] = null;
                Session["Customer"] = null;
                Session["ClosedCustomerProfile"] = null;
                Session["isCashierAgree"] = "false";
                Session["CardBalance"] = null;//AL-324
                TempData["FetchedFromCustomerLookUp"] = null;
                Session["ExistingGPRCardInfo"] = null;
                Session["SearchCriteria"] = null;

                CustomerSearch customerSearch = new CustomerSearch();

                if (Session["IsTerminalSetup"] == null)
                {
                    TerminalIdentifier.IdentifyTerminal(agentSessionId, context);
                    TempData["IsChooseLocation"] = TerminalIdentifier.IsTerminalAvailableForHostName(agentSessionId, context) && !IsException;
                }
                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;

                NLogHelper.Info(string.Format("CustomerSearch : IsException {0} ExceptionMessage {1}", IsException, ExceptionMessage));
                return View("CustomerSearch", customerSearch);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="searchField"></param>
        /// <param name="searchOper"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu")]
        public ActionResult CustomerSearch(CustomerSearch customerCriteria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Session["CustomerAO"] = null;

                    if (!string.IsNullOrEmpty(customerCriteria.CardNumber))
                    {
                        SecureData secure = new SecureData();

                        NLogHelper.Debug("CustomerSearch - Before decrypting :" + customerCriteria.CardNumber);
                        NLogHelper.Debug("CustomerSearch - CVV:" + customerCriteria.CVV);

                        string decryptedCardNumber = secure.Decrypt(customerCriteria.CardNumber, customerCriteria.CVV);
                        NLogHelper.Debug("CustomerSearch - After decrypting: " + decryptedCardNumber.MaskLeft(4));
                        if (!string.IsNullOrEmpty(decryptedCardNumber))
                        {
                            customerCriteria.CardNumber = decryptedCardNumber;
                        }
                        else
                        {
                            string messageKey = "1001.100.8602";
                            throw new ZeoWebException(GetErrorMessage(messageKey, GetZeoContext()));
                        }
                    }


                    ViewBag.CriteriaTag = true;

                    Session["SearchCriteria"] = customerCriteria;

                    return View("CustomerSearch", customerCriteria);
                }
                ViewBag.CriteriaTag = false;
                return View("CustomerSearch", customerCriteria);
            }
            catch (Exception ex)
            {
                NLogHelper.Debug(ex.ToString());

                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CustomerSearchGrid(string sidx, string sord, int page = 1, int rows = 5)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.CustomerSearchCriteria searchCriteria = new ZeoClient.CustomerSearchCriteria();

                CustomerSearch customerSearch = Session["SearchCriteria"] as CustomerSearch;

                if (customerSearch != null)
                {
                    if (!string.IsNullOrWhiteSpace(customerSearch.CardNumber))
                    {
                        searchCriteria.CardNumber = customerSearch.CardNumber == string.Empty ? default(string) : Helper.SafeSQLString(customerSearch.CardNumber, true);
                        searchCriteria.DateOfBirth = customerSearch.TCFCheckDateOfBirth != null && !customerSearch.IsZeoCard? Convert.ToDateTime(customerSearch.TCFCheckDateOfBirth) : default(DateTime);
                        searchCriteria.CardType = customerSearch.IsZeoCard ? ZeoClient.HelperCardType.ZEO : ZeoClient.HelperCardType.TCF;
                    }
                    else
                    {
                        searchCriteria.LastName = customerSearch.LastName == null ? default(string) : customerSearch.LastName;
                        searchCriteria.DateOfBirth = customerSearch.DateOfBirth == null ? default(DateTime) : Convert.ToDateTime(customerSearch.DateOfBirth);
                        searchCriteria.SSN = customerSearch.SSN == null ? default(string) : Helper.SafeSQLString(customerSearch.SSN.Replace("-", ""), true);
                        searchCriteria.AccountNumber = customerSearch.AccountNumber == null ? default(string) : customerSearch.AccountNumber;
                    }
                }

                context.AgentSessionId = GetAgentSessionId();

                response = alloyServiceClient.SearchCustomers(searchCriteria, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                var customers = ((List<ZeoClient.CustomerProfile>)response.Result);

                TempData["CustomerLookUp"] = customers;

                var totalRecords = customers.Count();
                var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

                var data = (from s in customers
                            select new
                            {
                                id = s.CustomerType == ZeoClient.HelperCustomerType.ZEO ? s.CustomerId : s.ClientCustomerId,
                                cell = new object[] { s.FirstName + " " + s.LastName, s.DateOfBirth != null ? s.DateOfBirth.Value.ToString("MM/dd/yyyy") : "", s.CardNumber ?? string.Empty, s.Address.Address1 ?? string.Empty }
                            }).ToArray();

                var jsonData = new
                {
                    display = true,
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = data.Skip((page - 1) * rows).Take(rows)
                };
                return Json(jsonData, "text/html", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        } 

        public ActionResult CustomerConformation(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                List<ZeoClient.CustomerProfile> customers = TempData["CustomerLookUp"] as List<ZeoClient.CustomerProfile>;
                TempData.Keep("CustomerLookUp");
                ZeoClient.CustomerProfile customer = customers.FirstOrDefault(x => x.CustomerId == id || x.ClientCustomerId == id);

                TempData["CustomerId"] = id;

                if (customer != null)
                {
                    CustomerDetails custDetails = new CustomerDetails()
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        DateOfBirth = customer.DateOfBirth != null ? customer.DateOfBirth.Value.ToString("MM/dd/yyyy") : "NA",
                        Address = string.IsNullOrWhiteSpace(customer.Address?.Address1) ? "NA" : customer.Address.Address1,
                        GovermentId = string.IsNullOrWhiteSpace(customer.IdNumber) ? "NA" : customer.IdNumber,
                        PhoneNumber = string.IsNullOrWhiteSpace(customer.Phone1?.Number) ? "NA" : customer.Phone1.Number ,
                        SSN = string.IsNullOrWhiteSpace(customer.SSN) ? "NA" : string.Format("***-**-{0}", customer.SSN.Substring(customer.SSN.Length - 4)),
                        CardNumber = string.IsNullOrWhiteSpace(customer.CardNumber) ? "NA" : string.Format("****-****-****-{0}", customer.CardNumber.Substring(customer.CardNumber.Length - 4)),
                        IsRcifCustomer = customer.CustomerType == ZeoClient.HelperCustomerType.ZEO ? false : true,
                        ActualSSN = customer.SSN,
                        CustomerId = id
                    };
                    return PartialView("_CustomerDetailsConfirmation", custDetails);
                }
            }
            var jsonData = new
            {
                data = WebHelper.GetAppSettingValue("UnhandledExceptionMessage"),
                success = false
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisplayNoCustomerPopup(string data = "")
        {
            if (string.IsNullOrEmpty(data))
                data = "Unhandled Error";
            string[] str = SplitString(data);

            SystemMessage sysmsg = new SystemMessage()
            {
                Type = str[0],
                Number = str[1],
                Message = str[2],
                AddlDetails = str[3],
                ErrorType = str.Count() == 5 ? str[4] : Helper.ErrorType.ERROR.ToString()
            };

            ViewBag.IsException = false;
            ViewBag.ExceptionMessage = null;

            return PartialView("_NoCustomerPopup", sysmsg);
        }

        public ActionResult ClearCustomerSearchSession()
        {
            Session["SearchCriteria"] = null;

            var jsonData = new
            {
                success = true
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private string[] SplitString(string message)
        {
            string[] strmessage = null;

            if (!string.IsNullOrEmpty(message) && message.Contains("|"))
                strmessage = message.Split(new string[] { "|" }, StringSplitOptions.None);
            else
            {
                string errorCode = "1000.100.9999";
                ZeoClient.ZeoContext context = GetZeoContext();
                string errorMessage = GetErrorMessage(errorCode, context);
                strmessage = errorMessage.Split(new string[] { "|" }, StringSplitOptions.None);
                strmessage[2] = string.IsNullOrEmpty(message) ? strmessage[2] : message;
            }

            return strmessage;
        }
        #endregion

        public ActionResult CheckShoppingCartStatus()
        {
            try
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];
                string shoppingCartStatus = string.Empty;
                bool success = false;

                if (customerSession != null)
                {
                    ZeoClient.Response response = alloyServiceClient.CanCloseCustomerSession(context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    shoppingCartStatus = (bool)response.Result ? "nonempty" : "empty";
                    success = true;
                    NLogHelper.Debug("CheckShoppingCartStatus: {0} ", shoppingCartStatus);
                }
                else
                {
                    shoppingCartStatus = "empty";
                    NLogHelper.Info("CheckShoppingCartStatus: Empty ");
                }
                var jsonData = new
                {
                    data = shoppingCartStatus,
                    success = success
                };

                return Json(jsonData);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult GetShoppingCartStatusPopUp(string shoppingCartStatus)
        {
            if (shoppingCartStatus == "empty")
                return PartialView("_ShoppingCartEmptyConfirm");
            else
                return PartialView("_ShoppingCartNonEmptyConfirm");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="fieldName"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        private IQueryable<T> SortIQueryable<T>(IQueryable<T> data, string fieldName, string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(fieldName)) return data;
            if (string.IsNullOrWhiteSpace(sortOrder)) return data;

            var param = Expression.Parameter(typeof(T), "i");
            Expression conversion = Expression.Convert(Expression.Property(param, fieldName), typeof(object));
            var mySortExpression = Expression.Lambda<Func<T, object>>(conversion, param);

            return (sortOrder == "desc") ? data.OrderByDescending(mySortExpression)
                : data.OrderBy(mySortExpression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CustomerSearch")]
        public ActionResult InitiateCustomerSession(string id = null, Common.CardSearchType searchType = Common.CardSearchType.Other, string calledFrom = "")
        {
            ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
            try
            {

                int cardPresentedType = (int)searchType;

                ZeoClient.ZeoContext context = GetZeoContext();

                Customer customer = GetCustomerDetails();

                if (customer.IsNewCustomer)
                {
                    context.CustomerId = long.Parse(Convert.ToString(TempData["CustomerId"]));
                }

                if (Convert.ToBoolean(Session["IsShoppingCartExists"]))
                    Session.Remove("IsShoppingCartExists");

                if (!string.IsNullOrEmpty((string)Session["CardNo"]))
                    Session.Remove("CardNo");

                if (Session["SearchCriteria"] != null && !string.IsNullOrEmpty(((CustomerSearch)Session["SearchCriteria"]).CardNumber))
                {
                    cardPresentedType = 2;
                    Session["SearchCriteria"] = null;
                }

                //Get SSO attributes from Cookie
                context.Context = new Dictionary<string, object>();
                context.SSOAttributes = GetSSOAttributes("SSO_AGENT_SESSION");

                //context.EditMode = true;

                if (calledFrom != "CustomerSummaryPage")
                {
                    ZeoClient.Response syncInResponse = alloyClient.CustomerSyncInFromClient(context);
                    if (WebHelper.VerifyException(syncInResponse)) throw new ZeoWebException(syncInResponse.Error.Details);
                }

                ZeoClient.Response customerProfileResponse = alloyClient.GetCustomer(context);
                if (WebHelper.VerifyException(customerProfileResponse)) throw new ZeoWebException(customerProfileResponse.Error.Details);

                ZeoClient.CustomerProfile custProfile = (ZeoClient.CustomerProfile)customerProfileResponse.Result;

                long agentSessionId = GetAgentSessionId();
                customer = GetCustomer(agentSessionId, custProfile);
                Session["Customer"] = customer;
                Session["CustomerAO"] = custProfile;

                //US1458 - start
                //Starts Here

                ZeoClient.CustomerSession customerSession = GetCustomerSession(); // why we even call this here? 
                if (customerSession == null)
                {
                    ZeoClient.Response initiatecustomerResponse = alloyClient.InitiateCustomerSession(cardPresentedType, context);
                    if (WebHelper.VerifyException(initiatecustomerResponse)) throw new ZeoWebException(initiatecustomerResponse.Error.Details);
                    customerSession = (ZeoClient.CustomerSession)initiatecustomerResponse.Result;
                }

                Session["IsGPRCard"] = customerSession.IsGPRCustomer;
                //Ends Here

                Session["CustomerSession"] = customerSession;

                //AL-384
                if (customer != null && customer.PersonalInformation != null)
                {
                    Session["ClosedCustomerProfile"] = ((ZeoClient.HelperProfileStatus)customer.PersonalInformation.CustomerProfileStatus == ZeoClient.HelperProfileStatus.Closed) ? "true" : string.Empty;
                }

                //Update the AlloyContext from the customer details.
                ZeoClient.Response alloContextResponse = alloyClient.GetZeoContextForCustomer(Convert.ToInt64(customerSession.CustomerSessionId), context);
                if (WebHelper.VerifyException(alloContextResponse)) throw new ZeoWebException(alloContextResponse.Error.Details);
                context = (ZeoClient.ZeoContext)alloContextResponse.Result;

                //Set the AlloyContext in Session when the customer is initiated and use it across the modules.
                Session["ZeoContext"] = context;

                return RedirectToAction("ProductInformation", "Product");
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public ActionResult ValidateCustomerStatusAndId(string id, string cardPresentedType, string calledFrom = "")
        {
            ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();

            try
            {
                long alloyId = Convert.ToInt64(id);
                long agentSessionId = GetAgentSessionId();

                ZeoClient.ZeoContext context = GetZeoContext();
                context.CustomerId = Convert.ToInt64(id);

                ZeoClient.Response customerProfileResponse = alloyClient.GetCustomer(context);
                if (WebHelper.VerifyException(customerProfileResponse)) throw new ZeoWebException(customerProfileResponse.Error.Details);

                ZeoClient.CustomerProfile custProfile = (ZeoClient.CustomerProfile)customerProfileResponse.Result;

                Customer customer = GetCustomer(agentSessionId, custProfile);
                customer.AlloyID = Convert.ToInt64(id);
                customer.IsNewCustomer = false;
                Session["Customer"] = customer;

                Session["ClosedCustomerProfile"] = (custProfile.ProfileStatus == ZeoClient.HelperProfileStatus.Closed) ? "true" : string.Empty;
                UrlHelper url = new UrlHelper(this.ControllerContext.RequestContext);
                string returnUrl = string.Empty;
                try
                {
                    //This logic moved from Biz layer as no need to go to Biz layer to check this condition as we already have
                    // customer details available in Web layer.
                    if (custProfile.ProfileStatus == ZeoClient.HelperProfileStatus.Inactive)
                    {
                        string messageKey = "1001.100.8606";
                        throw new ZeoWebException(GetErrorMessage(messageKey, GetZeoContext()));
                    }
                }
                catch (Exception ex)
                {
                    VerifyException(ex);
                    if (calledFrom == "CustomerSummaryPage")
                    {
                        NLogHelper.Error("CustomerSummaryPage", ex.Message);
                        return RedirectToAction("CustomerSearch", "CustomerSearch", new { IsException = true, ExceptionMessage = System.Web.HttpUtility.JavaScriptStringEncode(ViewBag.ExceptionMessage) });
                    }
                    else
                    {
                        ViewBag.IsExceptionRaised = null; // It should not throw the pop in this case. In verifyException method all flags are made true. 
                        NLogHelper.Error("CustomerRegistration", ex.Message);
                        returnUrl = url.Action("ProfileSummary", "CustomerRegistration", new { IsException = true, ExceptionMessage = System.Web.HttpUtility.JavaScriptStringEncode(ViewBag.ExceptionMessage) });
                        return Json(returnUrl, JsonRequestBehavior.AllowGet);
                    }
                }

                if (calledFrom == "CustomerSummaryPage")
                {
                    if (IsCustomerIdExpiring(custProfile) && custProfile.ProfileStatus != ZeoClient.HelperProfileStatus.Closed && (int)Session["UserRoleId"] != (int)UserRoles.Tech)
                    {
                        Session["activeButton"] = "newcustomer";
                        return RedirectToAction("IdentificationInformation", "CustomerRegistration", new { isExpiring = true });
                    }
                    return RedirectToAction("InitiateCustomerSession", "CustomerSearch", new { calledFrom = "CustomerSummaryPage" });
                }
                if (IsCustomerIdExpired(custProfile) && custProfile.ProfileStatus != ZeoClient.HelperProfileStatus.Closed && (int)Session["UserRoleId"] != (int)UserRoles.Tech)
                {
                    Session["activeButton"] = "newcustomer";
                    TempData["IsExpired"] = true;
                    returnUrl = url.Action("IdentificationInformation", "CustomerRegistration");
                    return Json(returnUrl, JsonRequestBehavior.AllowGet);
                }
                if (IsCustomerIdExpiring(custProfile) && custProfile.ProfileStatus != ZeoClient.HelperProfileStatus.Closed && (int)Session["UserRoleId"] != (int)UserRoles.Tech)
                {
                    Session["activeButton"] = "newcustomer";
                    returnUrl = url.Action("IdentificationInformation", "CustomerRegistration", new { isExpiring = true });
                    return Json(returnUrl, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //CustomerSearch customerSearch = new CustomerSearch();
                    //customerSearch.showIdConfirmedPopUp = "true";
                    //customerSearch.FirstName = Session["FirstName"] == null ? default(string) : Helper.SafeSQLString(Session["FirstName"].ToString(), true);
                    //customerSearch.LastName = Session["LastName"] == null ? default(string) : Helper.SafeSQLString(Session["LastName"].ToString(), true);
                    //customerSearch.PhoneNumber = Session["PhoneNumber"] == null ? default(string) : Helper.SafeSQLString(Session["PhoneNumber"].ToString(), true);
                    //customerSearch.DateOfBirth = Session["DateOfBirth"] == null ? string.Empty : Session["DateOfBirth"].ToString();
                    //customerSearch.CardNumber = string.Empty;
                    //customerSearch.SSN = Session["SSN"] == null ? default(string) : Helper.SafeSQLString(Session["SSN"].ToString(), true);

                    returnUrl = string.Empty;
                    return Json(returnUrl, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return RedirectToAction("CustomerSearch", "CustomerSearch", new { IsException = true, ExceptionMessage = System.Web.HttpUtility.JavaScriptStringEncode(ViewBag.ExceptionMessage) });

            }
        }

        [CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CustomerSearch")]
        public ActionResult SearchCustomerFromSwipe(string CardNumber, string CVV)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.ZeoContext context = GetZeoContext();

            try
            {
                Session["activeButton"] = "swipecard";
                ZeoClient.CustomerSearchCriteria searchCriteria = new ZeoClient.CustomerSearchCriteria();
                searchCriteria.CardNumber = CardNumber == null ? CardNumber : CardNumber.Replace(" ", string.Empty);
                NLogHelper.Debug("SearchCustomerFromSwipe - Before decrypting :" + CardNumber);
                NLogHelper.Debug("SearchCustomerFromSwipe - CVV:" + CVV);

                if (!string.IsNullOrEmpty(CardNumber) && !string.IsNullOrEmpty(CVV))
                {
                    SecureData secure = new SecureData();
                    searchCriteria.CardNumber = secure.Decrypt(CardNumber, CVV);
                    Session["CardNumber"] = searchCriteria.CardNumber;
                    NLogHelper.Debug("SearchCustomerFromSwipe: After decrypting", searchCriteria.CardNumber.MaskLeft(4));
                }

                context.AgentSessionId = Convert.ToInt64(Session["sessionId"]);

                ZeoClient.Response customerSearchResultsResponse = alloyServiceClient.SearchCardCustomer(searchCriteria, context);

                if (WebHelper.VerifyException(customerSearchResultsResponse)) throw new ZeoWebException(customerSearchResultsResponse.Error.Details);
                var customerProfile = ((List<ZeoClient.CustomerProfile>)customerSearchResultsResponse.Result).FirstOrDefault();


                if (customerProfile != null)
                {
                    var jsonData = new
                    {
                        success = true,
                        customerId = customerProfile.CustomerId
                    };
                    return (Json(jsonData));
                }
                else
                {
                    CustomerSearch customerSearch = new CustomerSearch();
                    ViewBag.ErrorMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.InvalidCardErrorMessage;
                    return View("CustomerSearch", customerSearch);
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Debug(ex.ToString());

                VerifyException(ex); return null;
            }
        }

        private bool IsCustomerIdExpiring(ZeoClient.CustomerProfile custProfile)
        {
            // the ID expiration date is within 14 days or the Govt Id is empty
            if (custProfile.IdExpirationDate != DateTime.MinValue && custProfile.IdExpirationDate <= DateTime.Now.AddDays(14))
                return true;
            else  // if the ID is valid
                return false;
        }

        private bool IsCustomerIdExpired(ZeoClient.CustomerProfile custProfile)
        {
            // the ID is Expired or the Govt Id is empty
            if (custProfile.IdExpirationDate != DateTime.MinValue && custProfile.IdExpirationDate < DateTime.Now.Date)
                return true;
            else  // if the ID is valid
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ModifyCustomer(string id)
        {
            return RedirectToAction("PersonalInformation", "CustomerRegistration");
        }

        private Models.Customer GetCustomer(long agentSessionId, ZeoClient.CustomerProfile customerProfile)
        {
            Models.Customer customer = new Models.Customer()
            {
                EmploymentDetails = new EmploymentDetails(),
                ProfileSummary = new ProfileSummary(),
                PinDetails = new PinDetails(),
                IdentificationInformation = new IdentificationInformation(),
                PersonalInformation = new Models.PersonalInformation()
            };
            customer.IsNewCustomer = false;
            customer.AlloyID = Convert.ToInt64(customerProfile.CustomerId);
            customer.IdentificationInformation.ClientID = customerProfile.ClientCustomerId;
            customer.PersonalInformation.ActualSSN = customerProfile.SSN;
            customer.PersonalInformation.SSN = WebHelper.MaskSSNNumber(customerProfile.SSN);
            customer.PersonalInformation.FirstName = customerProfile.FirstName;
            customer.PersonalInformation.Gender = customerProfile.Gender.ToString();
            customer.PersonalInformation.LastName = customerProfile.LastName;
            customer.PersonalInformation.MiddleName = customerProfile.MiddleName;
            customer.PersonalInformation.SecondLastName = customerProfile.LastName2;
            customer.PersonalInformation.Address1 = customerProfile.Address.Address1;
            customer.PersonalInformation.Address2 = customerProfile.Address.Address2;
            customer.PersonalInformation.AlternativePhone = customerProfile.Phone2.Number;
            customer.PersonalInformation.AlternativePhoneProvider = customerProfile.Phone2.Provider;
            customer.PersonalInformation.AlternativePhoneType = customerProfile.Phone2.Type;
            customer.PersonalInformation.City = customerProfile.Address.City;
            customer.PersonalInformation.Email = customerProfile.Email;
            customer.PersonalInformation.MailingAddress1 = customerProfile.MailingAddress.Address1;
            customer.PersonalInformation.MailingAddress2 = customerProfile.MailingAddress.Address2;
            customer.PersonalInformation.MailingAddressDifferent = customerProfile.MailingAddressDifferent;
            customer.PersonalInformation.MailingCity = customerProfile.MailingAddress.City;
            customer.PersonalInformation.MailingState = customerProfile.MailingAddress.State;
            customer.PersonalInformation.MailingZipCode = customerProfile.MailingAddress.ZipCode;
            customer.PersonalInformation.PrimaryPhone = customerProfile.Phone1.Number;
            customer.PersonalInformation.PrimaryPhoneProvider = customerProfile.Phone1.Provider;
            customer.PersonalInformation.PrimaryPhoneType = customerProfile.Phone1.Type;
            customer.PersonalInformation.State = customerProfile.Address.State;
            customer.PersonalInformation.ZipCode = customerProfile.Address.ZipCode;
            customer.PersonalInformation.ClientProfileStatus = (Helper.ProfileStatus)customerProfile.ClientProfileStatus;
            customer.PersonalInformation.DoNotCall = customerProfile.DoNotCall;
            customer.PersonalInformation.ReceiveTextMessage = customerProfile.SMSEnabled;
            customer.PersonalInformation.ReceiptLanguage = customerProfile.ReceiptLanguage;
            customer.PersonalInformation.CustomerProfileStatus = (Helper.ProfileStatus)customerProfile.ProfileStatus;
            customer.PersonalInformation.Group1 = customerProfile.Group1;
            customer.PersonalInformation.Group2 = customerProfile.Group2;
            customer.PersonalInformation.Notes = customerProfile.Notes;
            customer.PersonalInformation.ReferralNumber = customerProfile.ReferralCode;
            customer.PersonalInformation.WoodForestAccountHolder = customerProfile.IsAccountHolder;//this field is related to partnerCustomer IsAccountHolder
            customer.PersonalInformation.DateOfBirth = customerProfile.DateOfBirth == null || customerProfile.DateOfBirth == DateTime.MinValue ? string.Empty : (Convert.ToDateTime(customerProfile.DateOfBirth)).ToString("MM/dd/yyyy");

            customer.IdentificationInformation.Country = customerProfile.IdIssuingCountry;
            customer.IdentificationInformation.CountryOfBirth = customerProfile.CountryOfBirth;
            customer.IdentificationInformation.GovtIdIssueState = customerProfile.IdIssuingState;
            customer.IdentificationInformation.GovtIDType = customerProfile.IdType;
            customer.IdentificationInformation.IDExpireDate = customerProfile.IdExpirationDate == null || customerProfile.IdExpirationDate == DateTime.MinValue ? string.Empty : (Convert.ToDateTime(customerProfile.IdExpirationDate)).ToString("MM/dd/yyyy");
            customer.IdentificationInformation.IDIssuedDate = customerProfile.IdIssueDate == null || customerProfile.IdIssueDate == DateTime.MinValue ? string.Empty : (Convert.ToDateTime(customerProfile.IdIssueDate)).ToString("MM/dd/yyyy");
            //customer.IdentificationInformation.GovernmentId = customerProfile.IDTypeName;
            customer.IdentificationInformation.GovernmentId = customerProfile.IdNumber;
            customer.IdentificationInformation.MotherMaidenName = customerProfile.MothersMaidenName;

            customer.IdentificationInformation.LegalCode = customerProfile.LegalCode;
            customer.IdentificationInformation.PrimaryCountryCitizenShip = customerProfile.PrimaryCountryCitizenShip;
            customer.IdentificationInformation.SecondaryCountryCitizenShip = customerProfile.SecondaryCountryCitizenShip;
            customer.IdentificationInformation.MGIAlloyID = customerProfile.CustomerId;
            customer.EmploymentDetails.EmployerName = customerProfile.EmployerName;
            customer.EmploymentDetails.EmployerPhoneNumber = customerProfile.EmployerPhone;
            customer.EmploymentDetails.Profession = customerProfile.Occupation;
            customer.EmploymentDetails.OccupationDescription = customerProfile.OccupationDescription;
            customer.PinDetails.PhoneNumber = customerProfile.Phone1.Number;
            customer.PinDetails.Pin = customerProfile.PIN;
            customer.PinDetails.ReEnter = customerProfile.PIN;
            customer.CardNumber = customerProfile.CardNumber;

            return customer;
        }

        /// <summary>
        /// We are not using this method
        /// </summary>
        /// <returns></returns>
        //public ActionResult CustomerProfile(string customerPAN)
        //{
        //    try
        //    {
        //        Desktop client = new Desktop();
        //        ProfileSummary profile = new ProfileSummary();
        //        CustomerSession customerSession = new CustomerSession();
        //        Response response = new DMS.Server.Data.Response();

        // Nlogger.SetContext( HttpContext.Session.SessionID, null);
        //        MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
        //        if (Session["CardNo"] != null)
        //            Session.Remove("CardNo");

        //        response = client.InitiateCustomerSession(Session["sessionId"].ToString(), Convert.ToInt64(customerPAN), (int)CardSearchType.Other, mgiContext);
        //        if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
        //        customerSession = (CustomerSession)response.Result;
        //        string state = string.Empty;
        //        if (!string.IsNullOrWhiteSpace(customerSession.Customer.ID.State))
        //        {
        //            state = customerSession.Customer.ID.State;
        //            state = string.Compare(state, "select", true) == 0 ? string.Empty : state;
        //        }

        //        profile.Name = customerSession.Customer.PersonalInformation.FName + " " + customerSession.Customer.PersonalInformation.MName + " " + customerSession.Customer.PersonalInformation.LName;
        //        profile.Address = string.Format("{0}, {1}, {2}, {3}, {4}", customerSession.Customer.Address.Address1, customerSession.Customer.Address.Address2, customerSession.Customer.Address.City, customerSession.Customer.Address.State, customerSession.Customer.Address.PostalCode);
        //        profile.MailingAddress = string.Format("{0}, {1}, {2}, {3}, {4}", customerSession.Customer.MailingAddress.Address1, customerSession.Customer.MailingAddress.Address2, customerSession.Customer.MailingAddress.City, customerSession.Customer.MailingAddress.State, customerSession.Customer.MailingAddress.PostalCode);
        //        profile.DateOfBirth = customerSession.Customer.PersonalInformation.DateOfBirth.ToShortDateString();
        //        profile.PrimaryPhone = customerSession.Customer.Phone1.Number;
        //        profile.Email = customerSession.Customer.Email;
        //        profile.Profession = customerSession.Customer.Employment.Occupation;
        //        profile.EmployerName = customerSession.Customer.Employment.Employer;
        //        profile.EmployerPhoneNumber = customerSession.Customer.Employment.EmployerPhone;
        //        profile.Gender = customerSession.Customer.PersonalInformation.Gender;
        //        profile.GovtIdIssueState = state;
        //        profile.IDIssueDate = (customerSession.Customer.ID.IssueDate != null && customerSession.Customer.ID.IssueDate != DateTime.MinValue) ? customerSession.Customer.ID.IssueDate.Value.ToShortDateString() : string.Empty;
        //        profile.IDExpirationDate = customerSession.Customer.ID.ExpirationDate.ToString() == DateTime.MinValue.ToString() ? string.Empty : customerSession.Customer.ID.ExpirationDate.ToString(); // Namit
        //        profile.GovernmentId = customerSession.Customer.ID.GovernmentId;
        //        profile.MotherMaidenName = customerSession.Customer.PersonalInformation.MothersMaidenName;
        //        profile.GovtIDType = customerSession.Customer.ID.IDType;
        //        profile.Country = customerSession.Customer.ID.Country;
        //profile.CustomerSession = customerSession;

        //        NLogHelper.Debug("CustomerProfile:profileName {0}", profile.Name);
        //        return PartialView("_ProfileSummaryPopUp", profile);
        //    }
        //    catch (Exception ex)
        //    {
        //        VerifyException(ex); return null;
        //    }
        //}

        public ActionResult ShowSwipeMessage()
        {
            return PartialView("_SwipeCard");
        }

        public ActionResult ShowIDConfirmationMessage()
        {
            return PartialView("_SwipeCardIDConfirm");
        }

        private ZeoClient.CustomerSession GetCustomerSession()
        {
            return Session["CustomerSession"] as ZeoClient.CustomerSession;
        }

        public ActionResult ClearCustomerSession()
        {
            Session["Customer"] = null;

            var jsonData = new
            {
                success = true
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [SkipNoDirectAccess]
        public ActionResult UpdateCounterId(bool counterIdAssigned)
        {
            if (counterIdAssigned)
            {
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();

                //alloyContext.IsAvailable = true;

                if (Session["CustomerSession"] != null)
                {
                    alloyServiceClient.UpdateCounterId(context);
                }

            }
            return null;
        }

        //AL-2729 Changes - On Home Click - End Customer Session - Remove Cash In Transaction
        private void UpdateCashInStatus()
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            if (Session["CustomerSession"] != null)
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                try
                {
                    ZeoClient.Response response = alloyServiceClient.RemoveCashIn(context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                }
                catch { }
            }
        }

        private Models.Customer GetCustomerDetails()
        {
            if (Session["Customer"] != null)
            {
                return (Models.Customer)Session["Customer"];
            }
            return new Models.Customer()
            {
                IsNewCustomer = true
            };
        }

        #region Card Search

        public JsonResult GetCardTypeByBIN(string cardNumber)
        {
            Dictionary<string, object> dictionary = Session["Dictionary"] as Dictionary<string, object>;

            List<ZeoClient.KeyValuePair> cardBINs = (List<ZeoClient.KeyValuePair>)dictionary.GetValue("cardBINs");

            var jsonData = new
            {
                IsZeoCard = cardBINs.Any(i => i.Key == cardNumber.Substring(0, 4))? (cardBINs.First(i => i.Key == cardNumber.Substring(0, 4)).Value.ToString() == "ZEO"): false
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

    }

        #endregion
    }
}
