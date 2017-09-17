using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Globalization;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
using System.Collections;
using System.ServiceModel;
#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using System.Reflection;
using TCF.Zeo.Common.Util;
#endregion
namespace TCF.Channel.Zeo.Web.Controllers
{
    [Authorize(Roles = "Teller, Manager, ComplianceManager, SystemAdmin, Tech")]
    public class BaseController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ViewName"></param>
        /// <param name="masterName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override ViewResult View(string ViewName, string masterName, object model)
        {
            return PrepareView(ViewName, masterName, model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ViewName"></param>
        /// <param name="masterName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private ViewResult PrepareView(string ViewName, string masterName, object model)
        {
            ViewResult renderview = null;
          
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            if (!(ViewName.ToLower().Contains("login")) && (!(ViewName.ToLower().Contains("manageusers"))) && (!(ViewName.ToLower().Contains("location"))) && (!(ViewName.ToLower().Contains("newuser"))))
            {
                string optionalFilter = null;

                if (ViewName.ToLower() == "prepaidcard")
                {
                    bool GPRCardExists = (bool)model.GetType().GetProperty("GPRCardExists").GetValue(model, null);
                    if (!GPRCardExists)
                        optionalFilter = "Activation";
                }
                else if (ViewName.ToLower() == "transactionsummary")
                {
                    string SummaryTitle = (string)model.GetType().GetProperty("SummaryTitle").GetValue(model, null);
                    if (SummaryTitle == "Prepaid Card")
                        optionalFilter = "Prepaid Card";
                }

                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response response = alloyServiceClient.GetTipsAndOffers(context.ChannelPartnerId, CultureInfo.CurrentUICulture.ToString(), ViewName, optionalFilter, context);
                ViewBag.TipsAndOffersMessage = response.Result as string;
                PopulateCheckProcessorInfo(context);
            }


            if (!(ViewName.ToLower().Contains("login") || ViewName.ToLower().Contains("landing")))
            {
                if (Session != null && Session["ChannelPartnerName"] != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(VirtualPathUtility.ToAbsolute("~/Views/" + Session["ChannelPartnerName"].ToString().Trim() + "/" + ViewName + ".cshtml"))))
                    {
                        renderview = base.View("~/Views/" + Session["ChannelPartnerName"].ToString().Trim() + "/" + ViewName + ".cshtml", masterName, model);
                    }
                    else if (System.IO.File.Exists(Server.MapPath(VirtualPathUtility.ToAbsolute("~/Views/Shared" + "/" + ViewName + ".cshtml"))))
                    {
                        renderview = base.View("~/Views/Shared/" + ViewName + ".cshtml", masterName, model);
                    }
                    else
                    {
                        renderview = base.View("~/Views/Nexxo/" + ViewName + ".cshtml", masterName, model);
                    }
                }
                else
                {
                    renderview = base.View("~/Views/Nexxo/" + ViewName + ".cshtml", masterName, model);
                }

                if (renderview != null)
                    return renderview;
            }

            return base.View(ViewName, masterName, model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ViewName"></param>
        /// <returns></returns>
        protected new ViewResult View(string ViewName)
        {
            return View(ViewName, null, null);
        }

        /// <summary>
        /// Something about what the <c>MySomeFunction</c> does
        /// with some of the sample like
        /// <code>
        /// Some more code statement to comment it better
        /// </code>
        /// For more information seee <see cref="http://www.me.com"/>
        /// </summary>
        /// <param name="someObj">What the input to the function is</param>
        /// <returns>What it returns</returns>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NLogHelper.Info(string.Format("Begin action call {0}/{1}.", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName));
            
            bool isPersonalInformation = filterContext.HttpContext.Request.RawUrl.Trim('/').Contains("PersonalInformation") ? true : false;
            bool IsPersonalInfoRequiredToProceed = PersonalInfoRequiredToProceed(filterContext.HttpContext.Request.RawUrl.Trim('/'));

            /*
			 * This should handle if the user is logged in, but not have created a customer profile 
			 * and trying to access other sucessive pages
			 */
            if (filterContext.HttpContext.Session["Customer"] == null && !isPersonalInformation && IsPersonalInfoRequiredToProceed)
            { filterContext.Result = RedirectToAction("PersonalInformation", "CustomerRegistration", new { newCustomer = true }); return; }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Controller.ViewBag.IsExceptionRaised != null && filterContext.Controller.ViewBag.IsExceptionRaised
                && filterContext.Controller.ViewBag.IsException != null && filterContext.Controller.ViewBag.IsException)
            {
                filterContext.ExceptionHandled = true;
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    if (filterContext.Controller.ViewBag.ViewName != null && filterContext.Controller.ViewBag.ModelType == null && filterContext.Controller.ViewBag.ResultType == null)
                    {
                        filterContext.Result = PrepareView(filterContext.Controller.ViewBag.ViewName, filterContext.Controller.ViewBag.MasterName, filterContext.Controller.ViewBag.ParamValue);
                    }
                    else if (filterContext.Controller.ViewBag.ViewName != null && filterContext.Controller.ViewBag.ResultType == "prepare")
                    {
                        if (filterContext.Controller.ViewBag.ModelType != null)
                        {
                            var model = GetInstance<object>(filterContext.Controller.ViewBag.ModelType);

                            filterContext.Result = PrepareView(filterContext.Controller.ViewBag.ViewName, filterContext.Controller.ViewBag.MasterName, model);
                        }
                        else
                            filterContext.Result = PrepareView(filterContext.Controller.ViewBag.ViewName, filterContext.Controller.ViewBag.MasterName, null);
                    }
                    else if (filterContext.Controller.ViewBag.ResultType == "redirect")
                    {
                        if (filterContext.Controller.ViewBag.ActionName != null)
                            filterContext.Result = RedirectToAction((string)filterContext.Controller.ViewBag.ActionName, (string)filterContext.Controller.ViewBag.ControllerName, new { IsException = ViewBag.IsException, ExceptionMessage = ViewBag.ExceptionMessage });
                    }
                }
                else
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new { success = false, data = System.Web.HttpUtility.JavaScriptStringEncode(filterContext.Controller.ViewBag.ExceptionMessage) },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            NLogHelper.Info(string.Format("Completed action call {0}/{1}.", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageName"></param>
        /// <returns></returns>
        private bool PersonalInfoRequiredToProceed(string pageName)
        {
            if ((pageName.Contains("IdentificationInformation") || pageName.Contains("EmploymentDetails") ||
                     pageName.Contains("PinDetails") || pageName.Contains("ProfileSummary")) && pageName.StartsWith("CustomerSearch") == false)
                return true;
            else
                return false;
        }

        private T GetInstance<T>(string type)
        {
            return (T)Activator.CreateInstance(Type.GetType(type));
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                NLogHelper.Error(filterContext);
            }
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ViewBag.IsException = true;
                ViewBag.ExceptionMessage = System.Web.HttpUtility.JavaScriptStringEncode(filterContext.Exception.Message);

                filterContext.ExceptionHandled = true;

                if (filterContext.Controller.ViewBag.ViewName != null && filterContext.Controller.ViewBag.ModelType == null && filterContext.Controller.ViewBag.ResultType == null)
                {
                    filterContext.Result = PrepareView(filterContext.Controller.ViewBag.ViewName, filterContext.Controller.ViewBag.MasterName, filterContext.Controller.ViewBag.ParamValue);
                }
                else if (filterContext.Controller.ViewBag.ViewName != null && filterContext.Controller.ViewBag.ResultType == "prepare")
                {
                    if (filterContext.Controller.ViewBag.ModelType != null)
                    {
                        var model = GetInstance<object>(filterContext.Controller.ViewBag.ModelType);

                        filterContext.Result = PrepareView(filterContext.Controller.ViewBag.ViewName, filterContext.Controller.ViewBag.MasterName, model);
                    }
                    else
                        filterContext.Result = PrepareView(filterContext.Controller.ViewBag.ViewName, filterContext.Controller.ViewBag.MasterName, null);
                }
                else if (filterContext.Controller.ViewBag.ResultType == "redirect")
                {
                    if (filterContext.Controller.ViewBag.ActionName != null)
                        filterContext.Result = RedirectToAction((string)filterContext.Controller.ViewBag.ActionName, (string)filterContext.Controller.ViewBag.ControllerName, new { IsException = ViewBag.IsException, ExceptionMessage = ViewBag.ExceptionMessage });
                }
            }
            else
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = new JsonResult
                {
                    Data = new { success = false, data = System.Web.HttpUtility.JavaScriptStringEncode(filterContext.Exception.Message) },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }

        protected bool IsPeripheralServerSetUp(ZeoClient.Terminal terminal)
        {
            bool hasSetup = false;
            if (terminal.TerminalId != 0 && terminal.PeripheralServerUrl != null)
            {
                if (!string.IsNullOrWhiteSpace(terminal.PeripheralServerUrl))
                    hasSetup = true;
            }
            return hasSetup;
        }

        protected string GetChannelPartnerName()
        {
            string channelPartnerName = string.Empty;
            if (Session["ChannelPartnerName"] != null)
            {
                channelPartnerName = Session["ChannelPartnerName"].ToString();
            }

            return channelPartnerName;
        }


        public Dictionary<string, object> GetSSOAttributes(string sessionName)
        {
            //creating dictionary to return as collection.
            Dictionary<string, object> sessionContext = new Dictionary<string, object>();
            sessionContext = (Dictionary<string, object>)Session[sessionName];
            return sessionContext;
        }

        protected long GetAgentSessionId()
        {
            long agentSessionId = 0L;
            if (Session["HTSessions"] != null)
            {
                agentSessionId = long.Parse(((Hashtable)Session["HTSessions"])["AgentSessionId"].ToString());
            }
            return agentSessionId;
        }


        //internal bool VerifyException(Response response)
        //{
        //    bool isErrorRaised = false;
        //    if (response.Error != null)
        //    {
        //        var errorDetails = response.Error.Details.Split('|');
        //        string errorType = errorDetails.Count() == 5 ? errorDetails[4].ToString() : ErrorType.ERROR.ToString();
        //        if (errorType == ErrorType.ERROR.ToString() || errorType == ErrorType.WARNING.ToString())
        //        {
        //            ViewBag.IsException = true;
        //            ViewBag.IsExceptionRaised = true;
        //            ViewBag.ExceptionMessage = response.Error.Details;
        //            isErrorRaised = true;
        //        }
        //    }
        //    return isErrorRaised;
        //}

        internal bool VerifyException(ZeoClient.Response response)
        {
            bool isErrorRaised = false;
            if (response.Error != null)
            {
                var errorDetails = response.Error.Details.Split('|');
                string errorType = errorDetails.Count() == 5 ? errorDetails[4].ToString() : Helper.ErrorType.ERROR.ToString();
                if (errorType == Helper.ErrorType.ERROR.ToString() || errorType == Helper.ErrorType.WARNING.ToString())
                {
                    ViewBag.IsException = true;
                    ViewBag.IsExceptionRaised = true;
                    ViewBag.ExceptionMessage = response.Error.Details;
                    isErrorRaised = true;
                }
            }
            return isErrorRaised;
        }

        internal bool VerifyException(Exception ex)
        {
            ViewBag.IsException = true;
            ViewBag.IsExceptionRaised = true;
            Exception faultException = ex as FaultException;
            if (faultException != null)
            {
                ViewBag.ExceptionMessage = WebHelper.GetAppSettingValue("FaultExceptionMessage");
                return true;
            }
            Exception endpointException = ex as EndpointNotFoundException;
            if (endpointException != null)
            {
                ViewBag.ExceptionMessage = WebHelper.GetAppSettingValue("EndpointNotFoundExceptionMessage");
                return true;
            }
            Exception commException = ex as CommunicationException;
            if (commException != null)
            {
                ViewBag.ExceptionMessage = WebHelper.GetAppSettingValue("CommunicationExceptionMessage");
                return true;
            }
            Exception timeOutException = ex as TimeoutException;
            if (timeOutException != null)
            {
                ViewBag.ExceptionMessage = WebHelper.GetAppSettingValue("TimeoutExceptionMessage");
                return true;
            }
            Exception alloyWebException = ex as ZeoWebException;
            if (alloyWebException != null)
            {
                ViewBag.ExceptionMessage = alloyWebException.Message;
                return true;
            }

            ViewBag.ExceptionMessage = GetErrorMessage("1000.100.9999", GetZeoContext());
            return true;
        }

        private SelectListItem DefaultListItem()
        {
            return new SelectListItem() { Value = string.Empty, Text = "Select", Selected = true };
        }

        protected List<SelectListItem> GetSelectListItems(List<KeyValuePair<string, string>> listItems)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(DefaultListItem());
            foreach (var item in listItems)
            {
                selectListItems.Add(new SelectListItem() { Text = item.Key, Value = item.Value });
            }
            return selectListItems;
        }

        protected List<SelectListItem> GetSelectListItems(List<string> listItems)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(DefaultListItem());
            foreach (var item in listItems)
            {
                selectListItems.Add(new SelectListItem() { Text = item, Value = item });
            }
            return selectListItems;
        }

        public List<SelectListItem> GetCurrencyCodeList(long customerSessionId, string countryName, ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            List<SelectListItem> currencyList = new List<SelectListItem>();
            ZeoClient.Response response = alloyServiceClient.GetCurrencyCodeList(countryName, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            List<ZeoClient.MasterData> currencies = response.Result as List<ZeoClient.MasterData>;

            if (currencies.Count > 0)
            {
                foreach (var val in currencies)
                {
                    currencyList.Add(new SelectListItem() { Text = val.Name, Value = val.Code });
                }
            }
            return currencyList;
        }

        public List<SelectListItem> GetRefundReasons(long customerSessionId, ZeoClient.ReasonRequest request, ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            List<SelectListItem> pickupOptions = new List<SelectListItem>();
            pickupOptions.Add(DefaultListItem());
            ZeoClient.Response response = alloyServiceClient.GetRefundReasons(request, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            List<ZeoClient.Reason> pickupOptionsResult = response.Result as List<ZeoClient.Reason>;
            if (pickupOptionsResult.Count > 0)
            {
                foreach (var item in pickupOptionsResult)
                {
                    pickupOptions.Add(new SelectListItem() { Value = item.Code, Text = item.Name });
                }
            }
            return pickupOptions;
        }

        public List<SelectListItem> GetSelectListItem(List<ZeoClient.MasterData> masterData)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            selectList.Add(DefaultListItem());

            foreach (var item in masterData)
            {
                selectList.Add(new SelectListItem() { Value = item.Code, Text = item.Name });
            }

            return selectList;
        }


        protected string GetErrorMessage(string messageKey, ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            ZeoClient.Response response = alloyServiceClient.GetMessage(messageKey, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            ZeoClient.Message msg = response.Result as ZeoClient.Message;
            string errorMessage = string.Join("|", new object[] { msg.Processor, msg.MessageKey, msg.Content, msg.AddlDetails, Helper.ErrorType.ERROR });
            return errorMessage;
        }

        #region AO code changes
        internal void SetBaseRequest(ZeoClient.BaseRequest request)
        {
            ZeoClient.AgentSession agentSession = (ZeoClient.AgentSession)Session["AgentSession"];

            if (!string.IsNullOrWhiteSpace(agentSession.SessionId))
            { request.AgentSessionId = Convert.ToInt64(agentSession.SessionId); }

            request.ChannelPartnerName = agentSession.ChannelPartnerName;
            request.AgentBankId = agentSession.BankID;
            request.AgentBranchId = agentSession.BranchID;
            request.SSOAttributes = GetSSOAttributes("SSO_AGENT_SESSION");
        }

        public List<SelectListItem> GetCheckTypes()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            if (HttpContext.Session["CheckTypes"] == null)
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response response = alloyServiceClient.GetCheckTypes(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                List<ZeoClient.CheckType> checkTypes = (List<ZeoClient.CheckType>)response.Result;
                Session["CheckTypes"] = checkTypes;
                selectList.Add(DefaultListItem());
                checkTypes.ForEach(i => selectList.Add(new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }));
            
            }
            else
            {
                List<ZeoClient.CheckType> checkTypes = (List<ZeoClient.CheckType>)Session["CheckTypes"];
                selectList.Add(DefaultListItem());
                checkTypes.ForEach(i => selectList.Add(new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }));
            }

            return selectList;
        }

        public ZeoClient.ZeoContext GetCheckLogin()
        {
            CashACheck cashacheck = new CashACheck();
           
            ZeoClient.ZeoContext context = GetZeoContext();

            ZeoClient.ChannelPartnerProductProvider channelPartnerProductProvider = cashacheck.ProductProvider.Find(x => x.ProcessorName.ToLower() == "ingo");

            if (channelPartnerProductProvider != null)
            {

                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.CheckLogin chexarLogin = new ZeoClient.CheckLogin();

                if (HttpContext.Session["ChexarLogin"] == null)
                {
                    ZeoClient.Response response = new ZeoClient.Response();
                    response = alloyServiceClient.GetCheckSession(context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    chexarLogin = response.Result as ZeoClient.CheckLogin;
                    HttpContext.Session["ChexarLogin"] = chexarLogin;
                    context.URL = chexarLogin.URL;
                    context.IngoBranchId = chexarLogin.BranchId;
                    context.CompanyToken = chexarLogin.CompanyToken;
                    context.EmployeeId = chexarLogin.EmployeeId;
                }
                else
                {
                    chexarLogin = (ZeoClient.CheckLogin)HttpContext.Session["ChexarLogin"];
                    context.URL = chexarLogin.URL;
                    context.IngoBranchId = chexarLogin.BranchId;
                    context.CompanyToken = chexarLogin.CompanyToken;
                    context.EmployeeId = chexarLogin.EmployeeId;
                }

            }
            return context;
        }

        private void PopulateCheckProcessorInfo(ZeoClient.ZeoContext context)
        {
            ZeoClient.Response checkInfoResponse = new ZeoClient.Response();
            if (Session["HTSessions"] != null)
            {
                //ZeoClient.CustomerSearchCriteria 
                System.Collections.Hashtable htSessions = (System.Collections.Hashtable)Session["HTSessions"];
                ZeoClient.AgentSession agentSession = ((ZeoClient.AgentSession)(htSessions["TempSessionAgent"]));
                ZeoClient.CheckProcessorInfo info = new ZeoClient.CheckProcessorInfo();

                ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
                checkInfoResponse = alloyClient.GetCheckProcessorInfo(context);
                if (WebHelper.VerifyException(checkInfoResponse)) throw new ZeoWebException(checkInfoResponse.Error.Details);
                info = checkInfoResponse.Result as ZeoClient.CheckProcessorInfo;
                if (info != null)
                {
                    ViewBag.CheckProcessorInfo = info;
                }
            }
        }

        protected ZeoClient.ZeoContext GetZeoContext()
        {
            return Session["ZeoContext"] as ZeoClient.ZeoContext;
        }

        protected long GetCustomerSessionId()
        {
            long customerSessionId = 0L;
            if (Session["CustomerSession"] != null)
            {
                customerSessionId = ((ZeoClient.CustomerSession)Session["CustomerSession"]).CustomerSessionId;
            }
            return customerSessionId;
        }

        //public ZeoClient.CustomerSession GetCustomerSessionAO()
        //{
        //    if (Session["CustomerSession"] != null)
        //        return (ZeoClient.CustomerSession)Session["CustomerSession"];

        //    else
        //        return null;
        //}

        #endregion
        public List<SelectListItem> DefaultSelectList()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            selectList.Add(DefaultListItem());
            return selectList;
        }

        public List<SelectListItem> GetActBeHalfList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Select" });
            items.Add(new SelectListItem { Value = "1", Text = "Yes" });
            items.Add(new SelectListItem { Value = "2", Text = "No" });
            return items;
        }

        public void GetCustomerSessionCounterId(int prodctProviderId, ZeoClient.ZeoContext context)
        {
            if (string.IsNullOrWhiteSpace(context.WUCounterId))
            {
                //TODO: Once Service updated, then below code has to be changed.
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
                response = alloyClient.CreateCustomerSessionCounterId(prodctProviderId, Convert.ToInt32(context.LocationId), context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                context.WUCounterId = response.Result.ToString();
                Session["ZeoContext"] = context;
            }
        }

        public void PopulatTipsAndOffersMessage(ZeoClient.ZeoServiceClient serviceClient, ZeoClient.CustomerSession customerSession, ZeoClient.ZeoContext context)
        {
            ZeoClient.Response response = new ZeoClient.Response();
            System.Text.StringBuilder tipsAndOffersBuilder = new System.Text.StringBuilder();
            if (!string.IsNullOrWhiteSpace(context.WUCardNumber))
            {
                response = serviceClient.GetCardInfo(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                ZeoClient.CardInfo cardInfo = response.Result as ZeoClient.CardInfo;
                if (cardInfo != null)
                {
                    tipsAndOffersBuilder.AppendFormat("The customer has earned {0} Gold Points", cardInfo.TotalPointsEarned);

                    if (!string.IsNullOrWhiteSpace(cardInfo.PromoCode))
                    {
                        tipsAndOffersBuilder.Append(string.Format(" and a Western Union Promo Code: {0}", cardInfo.PromoCode));
                    }
                }
            }
            customerSession.TipsAndOffers = tipsAndOffersBuilder.ToString();
            Session["CustomerSession"] = customerSession;
        }

        private StringBuilder TraceObject(string name, object o, StringBuilder sb)
        {
            if (o == null)
            {
                sb.AppendLine(string.Format("{0} : {1}", name, "NULL"));
                return sb;
            }
            else if (o.GetType() == typeof(string))
            {
                sb.AppendLine(string.Format("{0} : {1}", name, o.ToString()));
            }
            else if (o.GetType().Name.Contains("Dictionary"))
            {
                var list = (IDictionary<string, object>)o;
                foreach (var item in list)
                {
                    TraceObject(item.Key, item.Value, sb);
                }
            }
            else if (o.GetType().BaseType.Name == "Object" || o.GetType().BaseType.Name == "BaseModel")
            {
                PropertyInfo[] properties = o.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    TraceObject(prop.Name, prop.GetValue(o), sb);
                }
            }
            else
            {
                sb.AppendLine(string.Format("{0} : {1}", name, o.ToString()));
            }
            return sb;
        }
        protected List<SelectListItem> GetReceiptLanguages()
        {
            List<SelectListItem> ReceiptLanguages = new List<SelectListItem>();
            Array itemNames = Enum.GetNames(typeof(Helper.Language));
            for (int i = 0; i < itemNames.Length; i++)
            {
                ReceiptLanguages.Add(new SelectListItem() { Text = itemNames.GetValue(i).ToString(), Value = itemNames.GetValue(i).ToString() });
            }
            return ReceiptLanguages;
        }

        protected decimal GetPrepaidCardBalance()
        {
            return (Session["CardBalance"] as ZeoClient.CardBalanceInfo) != null
                                            ? (Session["CardBalance"] as ZeoClient.CardBalanceInfo).Balance : 0M;
        }
    }
}
