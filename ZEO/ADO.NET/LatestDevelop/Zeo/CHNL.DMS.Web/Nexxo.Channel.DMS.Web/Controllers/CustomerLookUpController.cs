using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Models;
using TCF.Channel.Zeo.Web.Common;
using TCF.Zeo.Security.Voltage;
using Microsoft.Security.Application;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Logging.Impl;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{

    public class CustomerLookUpController : BaseController
    {
        public NLoggerCommon NLogger { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ViewName = "CustomerLookUp", MasterName = "_menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CustomerLookUp")]
        public ActionResult CustomerLookUp(bool IsException = false, string ExceptionMessage = "", bool IsPrimaryCardHolder = false)
        {
            Session["activeButton"] = "newcustomer";
            Session["Customer"] = null;
            if (!IsPrimaryCardHolder)
            {
                TempData["CustomerLookUp"] = null;
                TempData["CustomerLookUpCriteria"] = null;
                TempData["FetchedFromCustomerLookUp"] = null;
                TempData["CustomerLookUpPartnerAccountNumber"] = null;
                TempData["CustomerLookUpRelationshipAccountNumber"] = null;
                TempData["CustomerLookUpBankId"] = null;
                TempData["CustomerLookUpBranchId"] = null;
                Session["ClosedCustomerProfile"] = null;
                Session["CustomerSession"] = null;
            }
            CustomerLookUp lookUpInfo = new CustomerLookUp();

            lookUpInfo.CustomerMinimumAgeMessage = string.Format(App_GlobalResources.Nexxo.DateOfBirthConfigureMessage.ToString(), lookUpInfo.channelPartner.CustomerMinimumAge);

            lookUpInfo.IsCompanionSearch = IsPrimaryCardHolder;

            ViewBag.IsException = IsException;
            ViewBag.ExceptionMessage = ExceptionMessage;
            NLogHelper.Info("CustomerLookUpController :");
            NLogHelper.Error("CustomerLookup : IsException {0} , ExceptionMsg{1}", IsException, ExceptionMessage);
            // logger.SetContext(context);
            //Nlogger.SetContext(HttpContext.Session.SessionID, null);
            return View("CustomerLookUp", lookUpInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "CustomerLookUp", MasterName = "_Menu")]
        public ActionResult CustomerSearchGrid(CustomerLookUp lookUpCriteria, int page = 1, int rows = 5)
        {
            try
            {
                Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();
                List<ZeoClient.CustomerProfile> customerResults;
                NLogHelper.Info("CustomerSearchGrid With Lookup");

                if (!string.IsNullOrEmpty(lookUpCriteria.CardNumber) && !string.IsNullOrEmpty(lookUpCriteria.CVV))
                {
                    SecureData secure = new SecureData();
                    string decryptedCardNumber = secure.Decrypt(lookUpCriteria.CardNumber, lookUpCriteria.CVV);
                    lookUpCriteria.CardNumber = decryptedCardNumber;
                    Session["CardNumber"] = decryptedCardNumber;

                    NLogHelper.Debug("CustomerSearchGrid :decrypted card number {0}", decryptedCardNumber);
                }

                ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

                ZeoClient.CustomerSearchCriteria customersearchCriteria = new ZeoClient.CustomerSearchCriteria();

                if (!string.IsNullOrEmpty(lookUpCriteria.SSN)) customersearchCriteria.SSN = Helper.SafeSQLString(lookUpCriteria.SSN.Replace("-", ""), true);
                if (!string.IsNullOrEmpty(lookUpCriteria.LastName)) customersearchCriteria.LastName = Helper.SafeSQLString(lookUpCriteria.LastName, true);
                if (!string.IsNullOrEmpty(lookUpCriteria.DateOfBirth)) customersearchCriteria.DateOfBirth = Convert.ToDateTime(lookUpCriteria.DateOfBirth);
                if (!string.IsNullOrEmpty(lookUpCriteria.AccountNumber)) customersearchCriteria.AccountNumber = Helper.SafeSQLString(lookUpCriteria.AccountNumber, true);
                if (!string.IsNullOrEmpty(lookUpCriteria.CardNumber)) customersearchCriteria.CardNumber = Helper.SafeSQLString(lookUpCriteria.CardNumber, true);

                //This method need to be called before making any call to ZeoService.
                SetBaseRequest(customersearchCriteria);

                ZeoClient.ZeoContext context = GetZeoContext();

                //customerResults = 
                ZeoClient.Response response = alloyServiceClient.SearchCoreCustomers(customersearchCriteria, context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                customerResults = response.Result as List<ZeoClient.CustomerProfile>;
                TempData["CustomerLookUp"] = customerResults;
                int totalRecords;
                object jsonData;

                if (customerResults != null)
                {
                    totalRecords = customerResults.Count();
                    var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

                    //************* DO NOT CHANGE THE ALIAS NAMES *************//
                    int i = 0;
                    var data = (from s in customerResults
                                select new
                                {
                                    id = s.ClientCustomerId,
                                    customername = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                                    DateOfBirth = s.DateOfBirth != null || s.DateOfBirth != DateTime.MinValue ? s.DateOfBirth.Value.ToString("MM/dd/yyyy") : string.Empty,
                                    GovernmentId = s.IdNumber ?? string.Empty,
                                    address = s.Address.Address1 ?? string.Empty,
                                    SSN = s.SSN ?? string.Empty
                                }).ToArray();

                    jsonData = new
                    {
                        display = true,
                        repeatItems = false,
                        total = totalPages,
                        page = page,
                        records = totalRecords,
                        rows = data.Skip((page - 1) * rows).Take(totalRecords)
                    };
                }
                else
                {
                    totalRecords = 0;
                    var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
                    jsonData = null;
                }

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex);
                return Json(
                        new
                        {
                            success = false,
                            err = ViewBag.ExceptionMessage
                        }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ViewName = "CustomerLookUp", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CustomerLookUp")]
        public string PopulateCustomerDetails(string id, string SSN, string IsCompanionSearch)
        {
            try
            {
                string valid = "True";
                ZeoClient.ZeoServiceClient alloyClient = new ZeoClient.ZeoServiceClient();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.Response response = new ZeoClient.Response();

                ZeoClient.CustomerProfile result = new ZeoClient.CustomerProfile();
                long agentSessionId = GetAgentSessionId();

                bool isSSNValid = true;
                bool isCompanionSearch = Boolean.Parse(IsCompanionSearch);

                if (!string.IsNullOrWhiteSpace(SSN))
                {
                    context.SSN = SSN;
                    response = alloyClient.ValidateSSN(context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                    isSSNValid = Convert.ToBoolean(response.Result);
                }

                if (!isSSNValid && isCompanionSearch)
                {
                    ZeoClient.CustomerSearchCriteria searchCriteria = new ZeoClient.CustomerSearchCriteria()
                    {
                        SSN = SSN
                    };
                    //alloyContext.IsCompanionSearch = isCompanionSearch;

                    var customersResponse = alloyClient.SearchCoreCustomers(searchCriteria, context);
                    if (WebHelper.VerifyException(customersResponse)) throw new ZeoWebException(customersResponse.Error.Details);

                    var results = (List<ZeoClient.CustomerProfile>)customersResponse.Result;

                    if (results != null)
                    {
                        result = results.FirstOrDefault();
                    }

                    if (!string.IsNullOrWhiteSpace(result.CardNumber))
                    {
                        string messageKey = "1001.100.8601";
                        throw new ZeoWebException("Card Number Error");
                    }
                    else
                    {
                        TempData["Customer"] = GetCustomerDetails(id);
                        TempData["AddOnAlloyId"] = result.CustomerId;
                        return "CompanionSearch";
                    }
                }

                ZeoClient.CustomerProfile profile = new ZeoClient.CustomerProfile();

                if (isSSNValid)
                {
                    profile = GetCustomerDetails(id);
                    ZeoClient.ChannelPartner channelPartner = (ZeoClient.ChannelPartner)Session["ChannelPartner"];

                    profile.ProfileStatus = ZeoClient.HelperProfileStatus.Active;
                    profile.ChannelPartnerName = channelPartner.Name;

                    Models.Customer customerProspect = GetCustomer(agentSessionId, profile);
                    customerProspect.IsNewCustomer = true;
                    Session["Customer"] = customerProspect;

                    #region Used only for Synovus and Carver customer Registration
                    //TempData["CustomerLookUpPartnerAccountNumber"] = profile.PartnerAccountNumber;
                    //TempData["CustomerLookUpRelationshipAccountNumber"] = profile.RelationshipAccountNumber;
                    //TempData["CustomerLookUpBankId"] = profile.BankId;
                    //TempData["CustomerLookUpBranchId"] = profile.BranchId; 
                    #endregion

                    TempData["CustomerLookUpProgramId"] = profile.ProgramId;
                    //Check if we have all the required filed value
                    ZeoClient.Response validateCustomerResponse = alloyClient.ValidateCustomer(profile, context);

                    if (WebHelper.VerifyException(validateCustomerResponse)) throw new ZeoWebException(validateCustomerResponse.Error.Details);

                    bool isValidate = Convert.ToBoolean(validateCustomerResponse.Result);

                    TempData["IsCompanionSearch"] = IsCompanionSearch;
                    //Check if we have all the required filed value
                    TempData["FetchedFromCustomerLookUp"] = true;

                    if (!isValidate)
                        valid = "False";

                    return valid;
                }
                else
                {
                    string messageKey = "1001.100.8600";
                    throw new ZeoWebException(GetErrorMessage(messageKey, context));
                }
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomHandleErrorAttribute(ActionName = "NewCustomer", ControllerName = "CustomerLookUp", ResultType = "redirect")]
        public ActionResult NewCustomer(FormCollection personalInformation)
        {
            TempData["ActualSSN"] = personalInformation["ActualSSN"].Replace("-", "");
            TempData["SSN"] = personalInformation["SSN"];
            TempData["IsCompanionSearch"] = Boolean.Parse(personalInformation[PersonalInfo.IsCompanionSearch]);
            return RedirectToAction("PersonalInformation", "CustomerRegistration", new { newCustomer = true });
        }


        private Models.Customer GetCustomer(long agentSessionId, ZeoClient.CustomerProfile profile)
        {

            Models.Customer customerProspect = new Models.Customer()
            {
                IsNewCustomer = false,
                AlloyID = Convert.ToInt64(profile.CustomerId),

                PersonalInformation = new Models.PersonalInformation()
                {
                    DateOfBirth = profile.DateOfBirth == null || profile.DateOfBirth == DateTime.MinValue ? string.Empty : (Convert.ToDateTime(profile.DateOfBirth)).ToString("MM/dd/yyyy"),
                    ActualSSN = profile.SSN,
                    FirstName = profile.FirstName,
                    Gender = profile.Gender.ToString(),
                    LastName = profile.LastName,
                    MiddleName = profile.MiddleName,
                    SecondLastName = profile.LastName2,
                    SSN = (!string.IsNullOrWhiteSpace(profile.SSN)) ? WebHelper.MaskSSNNumber(profile.SSN) : string.Empty,
                    Address1 = profile.Address.Address1,
                    Address2 = profile.Address.Address2,
                    AlternativePhone = profile.Phone2.Number,
                    AlternativePhoneProvider = profile.Phone2.Provider,
                    AlternativePhoneType = profile.Phone2.Type,
                    City = profile.Address.City,
                    Email = profile.Email,
                    MailingAddressDifferent = profile.MailingAddressDifferent,
                    MailingAddress1 = profile.MailingAddress != null ? (profile.MailingAddressDifferent ? profile.MailingAddress.Address1 : profile.Address.Address1) : string.Empty,
                    MailingAddress2 = profile.MailingAddress != null ? (profile.MailingAddressDifferent ? profile.MailingAddress.Address2 : profile.Address.Address2) : string.Empty,
                    MailingCity = profile.MailingAddress != null ? (profile.MailingAddressDifferent ? profile.MailingAddress.City : profile.Address.City) : string.Empty,
                    MailingState = profile.MailingAddress != null ? (profile.MailingAddressDifferent ? profile.MailingAddress.State : profile.Address.State) : string.Empty,
                    MailingZipCode = profile.MailingAddress != null ? (profile.MailingAddressDifferent ? profile.MailingAddress.ZipCode : profile.Address.ZipCode) : string.Empty,
                    PrimaryPhone = profile.Phone1.Number,
                    PrimaryPhoneProvider = profile.Phone1.Provider,
                    PrimaryPhoneType = profile.Phone1.Type,
                    State = profile.Address.State,
                    ZipCode = profile.Address.ZipCode,
                    //ClientProfileStatus = profile.pr.ClientProfileStatus,
                    DoNotCall = profile.DoNotCall,
                    ReceiptLanguage = profile.ReceiptLanguage,
                    ReceiveTextMessage = profile.TextMsgOptIn,
                    CustomerProfileStatus = (Helper.ProfileStatus.Inactive),
                    LReceiptLanguage = GetReceiptLanguages(),
                },
                IdentificationInformation = new IdentificationInformation()
                {
                    Country = profile.IdIssuingCountry,
                    CountryOfBirth = profile.CountryOfBirth,
                    //2080
                    GovtIdIssueState = profile.IdIssuingState,
                    GovtIDType = profile.IdType,
                    IDExpireDate = ((profile.IdExpirationDate == null || profile.IdExpirationDate == DateTime.MinValue) ? string.Empty : (Convert.ToDateTime(profile.IdExpirationDate)).ToString("MM/dd/yyyy")),
                    //2080
                    IDIssuedDate = ((profile.IdIssueDate == null || profile.IdIssueDate == DateTime.MinValue) ? string.Empty : ((DateTime)profile.IdIssueDate).ToString("MM/dd/yyyy")),
                    GovernmentId = profile.IdNumber != null ? profile.IdNumber : string.Empty,
                    MotherMaidenName = profile.MothersMaidenName,
                    LegalCode = profile.LegalCode,
                    PrimaryCountryCitizenShip = profile.PrimaryCountryCitizenShip,
                    SecondaryCountryCitizenShip = profile.SecondaryCountryCitizenShip,
                    ClientID = profile.ClientCustomerId
                },
                EmploymentDetails = new EmploymentDetails()
                {
                    EmployerName = profile.EmployerName,
                    EmployerPhoneNumber = profile.EmployerPhone,
                    Profession = profile.Occupation,
                    OccupationDescription = profile.OccupationDescription
                },
                PinDetails = new PinDetails()
                {
                    PhoneNumber = profile.Phone1.Number,
                    Pin = profile.PIN,
                    ReEnter = profile.PIN
                },
                ProfileSummary = new ProfileSummary()
                {
                }
            };

            return customerProspect;
        }

        /****************************Begin TA-50 Changes************************************************/
        //     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
        //     Purpose: On Vera Code Scan, This call contains a cross-site scripting (XSS) flaw
        //				We found That this method is public and is called inside another action result and never called from view or js. So we can make it private. By making private the method will not be taken as action result
        private ZeoClient.CustomerProfile GetCustomerDetails(string id)
        {
            /****************************Begin TA-50 Changes************************************************/
            //     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
            //     Purpose: On Vera Code Scan, This call contains a cross-site scripting (XSS) flaw
            id = Sanitizer.GetSafeHtmlFragment(id);
            List<ZeoClient.CustomerProfile> customers = TempData["CustomerLookUp"] as List<ZeoClient.CustomerProfile>;
            ZeoClient.CustomerProfile selectedCustomer = new ZeoClient.CustomerProfile();

            selectedCustomer = customers.Count > 0 ? customers.FirstOrDefault(x => x.ClientCustomerId == id) : selectedCustomer;

            return selectedCustomer;
        }
        /****************************End TA-50 Changes************************************************/
    }
}
