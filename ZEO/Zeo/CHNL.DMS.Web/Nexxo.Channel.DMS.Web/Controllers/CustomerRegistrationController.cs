using TCF.Channel.Zeo.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCF.Channel.Zeo.Web.Common;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
using TCF.Zeo.Common.Util;
#endregion

namespace TCF.Channel.Zeo.Web.Controllers
{
    public class CustomerRegistrationController : BaseController
    {
        #region PersonalInfo

        public ActionResult NewCustomer()
        {
            Session["SearchCriteria"] = null;
            Session["Customer"] = null;
            Session["CustomerAO"] = null;
            Session["EditProspect"] = false;
            return RedirectToAction("PersonalInformation", new { IsAutoSearchRequired = true } );
        }

        [CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.CustomerSearch")]
        public ActionResult PersonalInformation(bool IsException = false, string ExceptionMessage = "", bool IsAutoSearchRequired = false)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                long agentSessionId = GetAgentSessionId();
                Session["activeButton"] = "newcustomer";
                PersonalInformation personalInfo = new PersonalInformation();
                ZeoClient.ZeoContext context = GetZeoContext();
                ZeoClient.ChannelPartner channelPartner = Session["ChannelPartner"] as ZeoClient.ChannelPartner;
                List<SelectListItem> selectList = new List<SelectListItem> { DefaultListItem() };

                CustomerSearch customerSearch = Session["SearchCriteria"] as CustomerSearch;
                if (channelPartner != null)
                {
                    ZeoClient.Response response = alloyServiceClient.GetPartnerGroups(channelPartner.Id, context);
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                    List<ZeoClient.KeyValuePair> groups = response.Result as List<ZeoClient.KeyValuePair>;
                    selectList.AddRange(groups.Select(x => new SelectListItem { Value = Convert.ToString(x.Value), Text = Convert.ToString(x.Value) }).ToList());

                }
                Customer customer = GetCustomerAO();

                if(customer.PersonalInformation.IsAutoSearchRequired != true)
                    customer.PersonalInformation.IsAutoSearchRequired = IsAutoSearchRequired;

                if (IsCustomerClosed(customer))
                {
                    return RedirectToAction("ProfileSummary", "CustomerRegistration");
                }
                ViewBag.IsProspectCreated = (customer.AlloyID != 0);

                if (customer.IsNewCustomer)
                {
                    ViewBag.profileSyncIn = true;
                }
                else
                {
                    ViewBag.profileSyncIn = false;
                    if (customer.PersonalInformation.ClientProfileStatus == Helper.ProfileStatus.Inactive)
                        customer.PersonalInformation.CustomerProfileStatus = customer.PersonalInformation.ClientProfileStatus;
                }

                personalInfo = customer.PersonalInformation;
                ZeoClient.Response USStatesResponse = alloyServiceClient.GetUSStates(context);

                if (WebHelper.VerifyException(USStatesResponse)) throw new ZeoWebException(USStatesResponse.Error.Details);
                personalInfo.LStates = ExtensionHelper.GetSelectListItems(USStatesResponse.Result) as List<SelectListItem>;

                ZeoClient.Response phoneTypeResponse = alloyServiceClient.GetPhoneTypes(context);
                if (WebHelper.VerifyException(phoneTypeResponse)) throw new ZeoWebException(phoneTypeResponse.Error.Details);
                personalInfo.LPrimaryPhonetype = ExtensionHelper.GetSelectListItems(phoneTypeResponse.Result) as List<SelectListItem>;

                ZeoClient.Response phoneProviderResponse = alloyServiceClient.GetPhoneProviders(context);
                if (WebHelper.VerifyException(phoneProviderResponse)) throw new ZeoWebException(phoneProviderResponse.Error.Details);
                personalInfo.LPrimaryPhoneProvider = ExtensionHelper.GetSelectListItems(phoneProviderResponse.Result) as List<SelectListItem>;

                personalInfo.LAlternatePhonetype = ExtensionHelper.GetSelectListItems(phoneTypeResponse.Result) as List<SelectListItem>;

                personalInfo.LAlternatePhoneProvider = ExtensionHelper.GetSelectListItems(phoneProviderResponse.Result) as List<SelectListItem>;

                personalInfo.LReceiptLanguage = GetReceiptLanguages();
                personalInfo.LGroup1 = selectList;
                personalInfo.LGroup2 = selectList;

                personalInfo.Notes = customer.PersonalInformation.Notes;

                personalInfo.LProfileStatus = GetProfileStatus();

                if ((int)Session["UserRoleId"] == (int)Helper.UserRoles.Teller)
                {
                    personalInfo.LProfileStatus = personalInfo.LProfileStatus.Where(x => x.Text != Helper.ProfileStatus.Closed.ToString());
                }

                if (customer.IsNewCustomer && personalInfo != null && string.IsNullOrEmpty(personalInfo.SSN))
                    personalInfo.SSN = Convert.ToString(customerSearch?.SSN);

                if (string.IsNullOrEmpty(personalInfo.SSN))
                {
                    personalInfo.SSN = personalInfo.ActualSSN;
                }
                if (customer.IsNewCustomer && string.IsNullOrEmpty(personalInfo.DateOfBirth))
                {
                    personalInfo.DateOfBirth = customerSearch?.TCFCheckDateOfBirth ?? customerSearch?.DateOfBirth;
                }
                if (customer.IsNewCustomer && string.IsNullOrEmpty(personalInfo.LastName))
                {
                    personalInfo.LastName = customerSearch?.LastName;
                }
                personalInfo.CustomerMinimumAgeMessage = string.Format(TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.DateOfBirthConfigureMessage.ToString(), personalInfo.channelPartner.CustomerMinimumAge);

                ViewBag.IsException = IsException;
                ViewBag.ExceptionMessage = ExceptionMessage;

                ViewBag.IsNotesEnable = personalInfo.channelPartner.IsNotesEnable;
                //Developed by: Sunil Shetty || Date: 09\06\2015
                //AL-533 : Mailing Address will not be visible in Customer registration on the base of IsMailingAddressEnable value			
                ViewBag.IsMailingAddressEnable = personalInfo.channelPartner.IsMailingAddressEnable;
                return View("PersonalInformation", personalInfo);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PersonalInformation(PersonalInformation personalInfo)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                //long agentSessionId = GetAgentSessionId();
                ZeoClient.ZeoContext context = GetZeoContext();

                Customer customer = GetCustomerAO();
                ViewBag.IsProspectCreated = (customer.AlloyID != 0);

                customer.PersonalInformation = personalInfo;
                Session["Customer"] = customer;

                if (ModelState.IsValid)
                {
                    if (personalInfo.CustomerProfileStatus != Helper.ProfileStatus.Closed)
                        return RedirectToAction("IdentificationInformation", "CustomerRegistration");
                    else
                        return RedirectToAction("ProfileSummary", "CustomerRegistration");
                }

                personalInfo = customer.PersonalInformation;
                ZeoClient.Response USStatesResponse = alloyServiceClient.GetUSStates(context);

                if (WebHelper.VerifyException(USStatesResponse)) throw new ZeoWebException(USStatesResponse.Error.Details);
                personalInfo.LStates = ExtensionHelper.GetSelectListItems(USStatesResponse.Result) as List<SelectListItem>;

                ZeoClient.Response phoneTypeResponse = alloyServiceClient.GetPhoneTypes(context);
                if (WebHelper.VerifyException(phoneTypeResponse)) throw new ZeoWebException(phoneTypeResponse.Error.Details);
                personalInfo.LPrimaryPhonetype = ExtensionHelper.GetSelectListItems(phoneTypeResponse.Result) as List<SelectListItem>;

                ZeoClient.Response phoneProviderResponse = alloyServiceClient.GetPhoneProviders(context);
                if (WebHelper.VerifyException(phoneProviderResponse)) throw new ZeoWebException(phoneProviderResponse.Error.Details);
                personalInfo.LPrimaryPhoneProvider = ExtensionHelper.GetSelectListItems(phoneProviderResponse.Result) as List<SelectListItem>;

                personalInfo.LAlternatePhonetype = ExtensionHelper.GetSelectListItems(phoneTypeResponse.Result) as List<SelectListItem>;

                personalInfo.LAlternatePhoneProvider = ExtensionHelper.GetSelectListItems(phoneProviderResponse.Result) as List<SelectListItem>;

                personalInfo.LReceiptLanguage = GetReceiptLanguages();

                personalInfo.LProfileStatus = GetProfileStatus();

                if ((int)Session["UserRoleId"] == (int)Helper.UserRoles.Teller)
                {
                    personalInfo.LProfileStatus = personalInfo.LProfileStatus.Where(x => x.Text != Helper.ProfileStatus.Closed.ToString());
                }
                
                return View("PersonalInformation", personalInfo);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string GetGoldCardNumber(string GoldCardNumber)
        {
            if (GoldCardNumber != null && GoldCardNumber.Trim() != "0")
                return GoldCardNumber;
            else
                return string.Empty;
        }

        public ActionResult ConfirmCustomerInactiveStatus()
        {
            return PartialView("_CustomerInactiveConfirm");
        }

        public JsonResult ValidateSSN(string SSN)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                context.SSN = SSN;
                //if (Session["Customer"] != null)
                //{
                //    Customer customerProspect = (Customer)Session["Customer"];
                //    if (customerProspect.AlloyID != 0)
                //        alloyContext.AlloyId = customerProspect.AlloyID;
                //}
                //else
                //    alloyContext.AlloyId = 0;

                ZeoClient.Response response = new ZeoClient.Response();

                response = alloyServiceClient.ValidateSSN(context);
                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                bool isSSNValid = Convert.ToBoolean(response.Result);

                string messageKey = "1001.100.8605";
                string ssnmsg = string.Empty;

                if (!isSSNValid)
                {
                    ssnmsg = GetErrorMessage(messageKey, context);
                }
                var jsonData = new
                {
                    msg = ssnmsg,
                    success = true
                };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// AL-231
        /// </summary>
        /// <param name="profileStatus"></param>
        /// <returns></returns>
        public ActionResult CanChangeProfileStatus(string profileStatus)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                var htSessions = (Hashtable)Session["HTSessions"];
                var agentSession = ((ZeoClient.AgentSession)(htSessions["TempSessionAgent"]));

                ZeoClient.ZeoContext context = GetZeoContext();
                string shoppingCartStatus = "empty";
                ZeoClient.CustomerSession customerSession = (ZeoClient.CustomerSession)Session["CustomerSession"];
                if (profileStatus == "Closed")
                {
                    if (customerSession != null)
                    {
                        ZeoClient.Response response = alloyServiceClient.IsShoppingCartEmpty(context);
                        if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                        bool isShoppingCartNotEmpty = (bool)response.Result;
                        if (isShoppingCartNotEmpty)
                            shoppingCartStatus = "nonempty";
                    }
                }
                ZeoClient.Response profileResponse = alloyServiceClient.CanChangeProfileStatus(profileStatus, context);
                if (WebHelper.VerifyException(profileResponse)) throw new ZeoWebException(profileResponse.Error.Details);
                bool canChange = Convert.ToBoolean(profileResponse.Result);

                var jsonData = new
                {
                    cartStatus = shoppingCartStatus,
                    success = canChange//AL-375
                };
                return Json(jsonData);
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
        //AL-375 changes
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetShoppingCartEmptyWhenClosedPopUp(string shoppingCartStatus)
        {
            return PartialView("_ShoppingCartEmptyWhenClosedConfirm");
        }
        //AL-375 end

        //To be Removed.
        private ZeoClient.CustomerProfile GetCustomerProfile(Customer customer)
        {
            ZeoClient.CustomerProfile customerProfile = new ZeoClient.CustomerProfile()
            {
                Phone1 = new ZeoClient.Phone(),
                Phone2 = new ZeoClient.Phone(),
                Address = new ZeoClient.Address(),
                MailingAddress = new ZeoClient.Address()
            };

            customerProfile.CustomerId = Convert.ToString(customer.AlloyID);
            if (customer.PersonalInformation != null)
            {
                var personalinformation = customer.PersonalInformation;
                customerProfile.FirstName = AlloyUtil.TrimString(personalinformation.FirstName);
                customerProfile.MiddleName = AlloyUtil.TrimString(personalinformation.MiddleName);
                customerProfile.LastName = AlloyUtil.TrimString(personalinformation.LastName);
                customerProfile.LastName2 = AlloyUtil.TrimString(personalinformation.SecondLastName);
                customerProfile.SSN = personalinformation.ActualSSN;
                if (personalinformation.Gender.ToLower() == "male")
                    customerProfile.Gender = ZeoClient.HelperGender.MALE;
                else if (personalinformation.Gender.ToLower() == "female")
                    customerProfile.Gender = ZeoClient.HelperGender.FEMALE;

                customerProfile.ProfileStatus = (ZeoClient.HelperProfileStatus)personalinformation.CustomerProfileStatus;
                customerProfile.Phone1.Number = personalinformation.PrimaryPhone.Replace("-", string.Empty);
                customerProfile.Phone1.Provider = personalinformation.PrimaryPhoneProvider;
                customerProfile.Phone1.Type = personalinformation.PrimaryPhoneType;
                customerProfile.Phone2.Number = !string.IsNullOrWhiteSpace(personalinformation.AlternativePhone) ? personalinformation.AlternativePhone.Replace("-", string.Empty) : string.Empty;
                customerProfile.Phone2.Provider = personalinformation.AlternativePhoneProvider;
                customerProfile.Phone2.Type = personalinformation.AlternativePhoneType;
                customerProfile.Email = personalinformation.Email;
                customerProfile.Address.Address1 = AlloyUtil.TrimString(personalinformation.Address1);
                customerProfile.Address.Address2 = AlloyUtil.TrimString(personalinformation.Address2);
                customerProfile.Address.City = AlloyUtil.TrimString(personalinformation.City);
                customerProfile.Address.State = AlloyUtil.TrimString(personalinformation.State);
                customerProfile.Address.ZipCode = personalinformation.ZipCode;
                customerProfile.ReferralCode = personalinformation.ReferralNumber;
                customerProfile.SMSEnabled = personalinformation.ReceiveTextMessage;
                customerProfile.IsAccountHolder = personalinformation.WoodForestAccountHolder;
                customerProfile.DoNotCall = personalinformation.DoNotCall;
                customerProfile.Group1 = personalinformation.Group1;
                customerProfile.Group2 = personalinformation.Group2;
                customerProfile.Notes = AlloyUtil.TrimString(personalinformation.Notes);
                customerProfile.MailingAddressDifferent = personalinformation.MailingAddressDifferent;
                customerProfile.MailingAddress.Address1 = AlloyUtil.TrimString(personalinformation.MailingAddress1);
                customerProfile.MailingAddress.Address2 = AlloyUtil.TrimString(personalinformation.MailingAddress2);
                customerProfile.MailingAddress.City = AlloyUtil.TrimString(personalinformation.MailingCity);
                customerProfile.MailingAddress.State = AlloyUtil.TrimString(personalinformation.MailingState);
                customerProfile.MailingAddress.ZipCode = personalinformation.MailingZipCode;
                if (!string.IsNullOrEmpty(personalinformation.DateOfBirth))
                {
                    customerProfile.DateOfBirth = Convert.ToDateTime(personalinformation.DateOfBirth);
                }
                else
                {
                    customerProfile.DateOfBirth = null;
                }
                // TODO : Assign the proper values to the below.

                //customerProfile.ChannelPartnerid = personalinformation.channelPartner.Id;
                // customerProfile.ProfileStatus = personalinformation.CustomerProfileStatus;
            }

            if (customer.IdentificationInformation != null)
            {
                var identificationInformation = customer.IdentificationInformation;
                customerProfile.MothersMaidenName = AlloyUtil.TrimString(identificationInformation.MotherMaidenName);
                customerProfile.ClientCustomerId = AlloyUtil.TrimString(identificationInformation.ClientID);
                customerProfile.LegalCode = AlloyUtil.TrimString(identificationInformation.LegalCode);
                customerProfile.PrimaryCountryCitizenShip = identificationInformation.PrimaryCountryCitizenShip;
                customerProfile.SecondaryCountryCitizenShip = identificationInformation.SecondaryCountryCitizenShip;
                customerProfile.IdIssuingCountry = identificationInformation.Country; //TODO: Double Check the mapping.
                customerProfile.CountryOfBirth = identificationInformation.CountryOfBirth;
                customerProfile.IdType = identificationInformation.GovtIDType;
                customerProfile.IdIssuingState = string.IsNullOrEmpty(identificationInformation.GovtIdIssueState) ? string.Empty : identificationInformation.GovtIdIssueState;
                //customerProfile.IDTypeName = identificationInformation.GovernmentId;
                customerProfile.IdNumber = identificationInformation.GovernmentId;

                if (string.IsNullOrEmpty(identificationInformation.IDIssuedDate))
                {
                    customerProfile.IdIssueDate = null;
                }
                else
                {
                    customerProfile.IdIssueDate = Convert.ToDateTime(identificationInformation.IDIssuedDate);
                }

                if (string.IsNullOrEmpty(identificationInformation.IDExpireDate))
                {
                    customerProfile.IdExpirationDate = null;
                }
                else
                {
                    customerProfile.IdExpirationDate = Convert.ToDateTime(identificationInformation.IDExpireDate);
                }
            }

            if (customer.EmploymentDetails != null)
            {
                var empDetails = customer.EmploymentDetails;
                customerProfile.Occupation = AlloyUtil.TrimString(empDetails.Profession);
                customerProfile.OccupationDescription = AlloyUtil.TrimString(empDetails.OccupationDescription);
                customerProfile.EmployerName = AlloyUtil.TrimString(empDetails.EmployerName);
                customerProfile.EmployerPhone = string.IsNullOrEmpty(empDetails.EmployerPhoneNumber) ? string.Empty : empDetails.EmployerPhoneNumber.Replace("-", string.Empty);
            }

            if (customer.PinDetails != null)
            {
                customerProfile.PIN = AlloyUtil.TrimString(customer.PinDetails.Pin);
            }

            customerProfile.CardNumber = customer.CardNumber;

            return customerProfile;
        }

        #endregion

        #region IdentificationInformation

        private string idRegex { get; set; }

        private bool HasExpirationDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CountryId"></param>
        /// <param name="IdType"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetIdRequirements(string CountryId, string IdType, string State)
        {
            try
            {
                ZeoClient.IdType idrequirement = null;

                string idtype, state;

                if (IdType == null || IdType != null && IdType.Trim(' ').ToLower() == "select")
                { idtype = string.Empty; }
                else { idtype = IdType; }

                if (State == null || State != null && State.Trim(' ').ToLower() == "select")
                { state = string.Empty; }
                else { state = State; }

                if (CountryId == null || CountryId != null && CountryId.Trim(' ').ToLower() == "select")
                { CountryId = string.Empty; }

                idrequirement = GetIdentificationRequirements(CountryId, idtype, state);

                if (idrequirement != null)
                { idRegex = idrequirement.Mask; HasExpirationDate = idrequirement.HasExpirationDate; }

                return Json(idrequirement, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GovtIdType(string CountryId, string legalCode)
        {
            try
            {
                ZeoClient.ChannelPartner channelPartner = Session["ChannelPartner"] as ZeoClient.ChannelPartner;

                ZeoClient.ZeoContext context = GetZeoContext();
                List<SelectListItem> idTypeSelectList = GetIdTypesSelectlistItems(CountryId, legalCode, context);

                return Json(idTypeSelectList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        public List<SelectListItem> GetIdTypesSelectlistItems(string countryId, string legalCode, ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

            List<SelectListItem> GovtIdTypeIds = new List<SelectListItem>();
            ZeoClient.Response response = alloyServiceClient.GetIdTypes(countryId, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            List<string> idTypes = response.Result as List<string>;

            if (!string.IsNullOrWhiteSpace(legalCode) && legalCode == "U")
                idTypes.Remove("PERMANENT RESIDENT CARD");

            List<SelectListItem> idTypeSelectList = new List<SelectListItem>();

            if (idTypes != null)
            {
                idTypeSelectList = GetSelectListItems(idTypes).ToList();
            }

            return idTypeSelectList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CountryId"></param>
        /// <param name="GovtIdType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStates(string CountryId, string GovtIdType)
        {
            try
            {
                ZeoClient.ChannelPartner channelPartner = Session["ChannelPartner"] as ZeoClient.ChannelPartner;
                ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();
                ZeoClient.Response response = new ZeoClient.Response();
                ZeoClient.ZeoContext context = GetZeoContext();

                List<SelectListItem> states = new List<SelectListItem>();
                //alloyContext.Country = CountryId;
                response = client.GetIdStates(GovtIdType, CountryId, context);

                if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

                states = GetSelectListItems(response.Result as List<string>);
                bool isNullItemExist = false;
                //if (states.RemoveAll(c => c.Value == null) >= 0)
                //{
                //    isNullItemExist = true;
                //}
                if (CountryId == "UNITED STATES" && (GovtIdType == "NEW YORK BENEFITS ID" || GovtIdType == "NEW YORK CITY ID"))
                {
                    states.FirstOrDefault(x => x.Text == "Select").Selected = false;
                    states.FirstOrDefault(x => x.Text == "NEW YORK").Selected = true;
                }
                return Json(new { states = states, isNullItemExist = isNullItemExist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ViewName = "IdentificationInformation", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.IdentificationInformation")]
        public ActionResult IdentificationInformation(bool isExpiring = false)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                var ssnrequired = TempData["DisplaySSNPopup"];
                long agentSessionId = GetAgentSessionId();
                Customer customerProspect = GetCustomerAO();

                ZeoClient.ZeoContext context = GetZeoContext();

                if (IsCustomerClosed(customerProspect))
                {
                    return RedirectToAction("ProfileSummary", "CustomerRegistration");
                }
                IdentificationInformation identificationInfo = customerProspect.IdentificationInformation;
                identificationInfo.IsCompanionSearch = customerProspect.PersonalInformation.IsCompanionSearch;
                identificationInfo.IsSSNPresent = !string.IsNullOrWhiteSpace(customerProspect.PersonalInformation.ActualSSN);

                ZeoClient.Response countriesResponse = alloyServiceClient.GetCountries(context);
                if (WebHelper.VerifyException(countriesResponse)) throw new ZeoWebException(countriesResponse.Error.Details);
                identificationInfo.LCountry = ExtensionHelper.GetSelectListItems(countriesResponse.Result);

                context.ChannelPartnerId = identificationInfo.channelPartner.Id;
                //alloyContext.Country = identificationInfo.Country;

                ZeoClient.Response statesResponse = alloyServiceClient.GetIdStates(identificationInfo.GovtIDType, identificationInfo.Country, context);
                if (WebHelper.VerifyException(statesResponse)) throw new ZeoWebException(statesResponse.Error.Details);
                identificationInfo.LStates = ExtensionHelper.GetSelectListItems(statesResponse.Result);

                //New method is added to get the state name based on the state code. Because in SyncIn and Core customer search we are getting Id StateCode.
                //But in the above code we are getting only state names so had to do a new DB call to get the state name from code.
                ZeoClient.Response statenameResponse = alloyServiceClient.GetStateNameByCode(identificationInfo.GovtIdIssueState, identificationInfo.Country, context);
                if (WebHelper.VerifyException(statenameResponse)) throw new ZeoWebException(statenameResponse.Error.Details);
                identificationInfo.GovtIdIssueState = statenameResponse.Result as string;

                identificationInfo.LLegalCodes = GetLegalCodes(context);

                //Defect - When Secondary Country of Citizenship is not selected (value - null). Primary Country Citizenship selected value was displayed in Secondary Country of Citizenship dropdown.
                //Reason - We were using same SelectListItems - "GetMasterCountries(context)" for all of the below dropdownlists. So If the selected item is null in the dropdown, the previous dropdown value is displayed. 
                //Starts here 
                identificationInfo.LCountryOfBirth = GetMasterCountries(context);
                identificationInfo.LPrimaryCountryCitizenship = GetMasterCountries(context);
                identificationInfo.LSecondaryCountryCitizenship = GetMasterCountries(context);
                //Ends here 

                identificationInfo.LGovtIDType = GetIdTypesSelectlistItems(identificationInfo.Country, identificationInfo.LegalCode, context);

                identificationInfo.MGIAlloyID = customerProspect.AlloyID == 0 ? string.Empty : customerProspect.AlloyID.ToString();

                if (Convert.ToBoolean(TempData["IsExpired"]))
                {
                    ModelState.AddModelError("IDExpireDate", "ID Expiration date can not be in the past.");
                    @ViewBag.ExpiredErrorMessage = true;
                }
                if (isExpiring)
                {
                    ViewBag.ErrorMessage = TCF.Channel.Zeo.Web.App_GlobalResources.Nexxo.IDExpirationMessage;
                }

                ViewBag.DOB = customerProspect.PersonalInformation.DateOfBirth;

                return View("IdentificationInformation", identificationInfo);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="identificationInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "IdentificationInformation", MasterName = "_Menu")]
        public ActionResult IdentificationInformation(IdentificationInformation identificationInfo)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

            try
            {
                long agentSessionId = GetAgentSessionId();
                Customer customerProspect = GetCustomerAO();
                customerProspect.IdentificationInformation = identificationInfo;
                //customerProspect.CustomerScreen = CustomerScreen.Identification;
                Session["Customer"] = customerProspect;

                //if the Customer belongs to "US" then SSN is mandatory.
                if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsSSNCheckRequired"])
                        && string.IsNullOrWhiteSpace(customerProspect?.PersonalInformation?.ActualSSN)
                        && (identificationInfo.CountryOfBirth?.ToLower() == "us" 
                            || identificationInfo.SecondaryCountryCitizenShip?.ToLower() == "us" 
                            || identificationInfo.PrimaryCountryCitizenShip?.ToLower() == "us"))
                {
                    TempData["DisplaySSNPopup"] = "true";
                    return RedirectToAction("IdentificationInformation");
                }

                ZeoClient.ZeoContext context = GetZeoContext();

                identificationInfo.GovtIDType = HttpUtility.UrlDecode(identificationInfo.GovtIDType);
                ZeoClient.Response countriesResponse = alloyServiceClient.GetCountries(context);
                if (WebHelper.VerifyException(countriesResponse)) throw new ZeoWebException(countriesResponse.Error.Details);
                identificationInfo.LCountry = ExtensionHelper.GetSelectListItems(countriesResponse.Result);

                ZeoClient.Response govtIdTypeResponse = alloyServiceClient.GetIdTypes(identificationInfo.Country, context);
                if (WebHelper.VerifyException(govtIdTypeResponse)) throw new ZeoWebException(govtIdTypeResponse.Error.Details);
                identificationInfo.LGovtIDType = ExtensionHelper.GetSelectListItems(govtIdTypeResponse.Result);

                ZeoClient.Response statesResponse = alloyServiceClient.GetIdStates(identificationInfo.GovtIDType, identificationInfo.Country, context);
                if (WebHelper.VerifyException(statesResponse)) throw new ZeoWebException(statesResponse.Error.Details);
                identificationInfo.LStates = ExtensionHelper.GetSelectListItems(statesResponse.Result);

                identificationInfo.LLegalCodes = GetLegalCodes(context);

                //Defect - When Secondary Country of Citizenship is not selected (value - null). Primary Country Citizenship selected value was displayed in Secondary Country of Citizenship dropdown.
                //Reason - We were using same SelectListItems - "GetMasterCountries(context)" for all of the below dropdownlists. So If the selected item is null in the dropdown, the previous dropdown value is displayed. 
                //Starts here 
                identificationInfo.LCountryOfBirth = GetMasterCountries(context);
                identificationInfo.LPrimaryCountryCitizenship = GetMasterCountries(context);
                identificationInfo.LSecondaryCountryCitizenship = GetMasterCountries(context);
                //Ends here 

                GetIdRequirements(identificationInfo.Country, identificationInfo.GovtIDType, identificationInfo.GovtIdIssueState);

                if (identificationInfo.GovernmentId != null)
                    if (!System.Text.RegularExpressions.Regex.IsMatch(identificationInfo.GovernmentId, this.idRegex))
                        ModelState.AddModelError("GovernmentId", "Please enter a valid ID Number in appropriate format.");


                if (identificationInfo.GovernmentId != null)
                    if (this.HasExpirationDate == true && (string.IsNullOrEmpty(identificationInfo.IDExpireDate) || identificationInfo.IDExpireDate.Trim() == DateTime.MinValue.ToShortDateString()))
                    {
                        if (!((identificationInfo.GovtIDType == "U.S. STATE IDENTITY CARD" && identificationInfo.GovtIdIssueState == "ARIZONA") ||
                            (identificationInfo.GovtIDType == "U.S. STATE IDENTITY CARD" && identificationInfo.GovtIdIssueState == "TEXAS") ||
                            (identificationInfo.GovtIDType == "U.S. STATE IDENTITY CARD" && identificationInfo.GovtIdIssueState == "TENNESSEE") ||
                            (identificationInfo.GovtIDType == "U.S. STATE IDENTITY CARD" && identificationInfo.GovtIdIssueState == "WYOMING")))
                            ModelState.AddModelError("IDExpireDate", "Please enter a valid expiration date in MM/DD/YYYY format.");
                    }

                if (ModelState.IsValid)
                {
                    //sharedData.Prospect prospect = GetProspect(customerProspect);
                    //Response customerprofileResponse = client.SaveCustomerProfile(((Hashtable)Session["HTSessions"])["AgentSessionId"].ToString(), customerProspect.AlloyID, prospect, mgiContext, !customerProspect.IsNewCustomer);

                    // if (WebHelper.VerifyException(customerprofileResponse)) throw new AlloyWebException(customerprofileResponse.Error.Details);
                    return RedirectToAction("EmploymentDetails", "CustomerRegistration");
                }

                return View("IdentificationInformation", identificationInfo);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// Displaying the Pop up for SSN validation.
        /// </summary>
        /// <param name="btnText"></param>
        /// <returns></returns>
        public ActionResult DisplayMessageInPopUpWithContinue(string btnText = "Continue", string msg = "")
        {
            try
            {
                string[] str = splitstring(msg);
                ViewBag.BtnText = btnText;

                SystemMessage sysmsg = new SystemMessage()
                {
                    Type = str[0],
                    Number = str[1],
                    Message = str[2],
                    AddlDetails = str[3],
                    ErrorType = str.Count() == 5 ? str[4] : Helper.ErrorType.ERROR.ToString()
                };

                return PartialView("_SSNContinue", sysmsg);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        private List<SelectListItem> GetMasterCountries(ZeoClient.ZeoContext context)
        {
            List<SelectListItem> mastercountrySelectList = new List<SelectListItem>();
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

            ZeoClient.Response masterCountryResponse = alloyServiceClient.GetMasterCountries(context);
            if (WebHelper.VerifyException(masterCountryResponse)) throw new ZeoWebException(masterCountryResponse.Error.Details);

            List<ZeoClient.MasterCountry> masterCountries = masterCountryResponse.Result as List<ZeoClient.MasterCountry>;
            if (masterCountries != null)
            {
                mastercountrySelectList = masterCountries.Select(d => new SelectListItem() { Text = d.Name, Value = d.Abbr2 }).ToList();
            }
            mastercountrySelectList.Insert(0, DefaultListItem());
            return mastercountrySelectList;
        }

        private List<SelectListItem> GetLegalCodes(ZeoClient.ZeoContext context)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

            ZeoClient.Response legalCodeResponse = alloyServiceClient.GetLegalCodes(context);
            if (WebHelper.VerifyException(legalCodeResponse)) throw new ZeoWebException(legalCodeResponse.Error.Details);

            List<ZeoClient.LegalCode> legalCodes = legalCodeResponse.Result as List<ZeoClient.LegalCode>;
            if (legalCodes != null)
            {
                selectListItems = legalCodes.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code }).ToList();
            }
            selectListItems.Insert(0, DefaultListItem());
            return selectListItems;
        }

        private SelectListItem DefaultListItem()
        {
            return new SelectListItem() { Value = string.Empty, Text = "Select", Selected = true };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="idType"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private ZeoClient.IdType GetIdentificationRequirements(string country, string idType, string state)
        {
            ZeoClient.ChannelPartner channelPartner = Session["ChannelPartner"] as ZeoClient.ChannelPartner;
            ZeoClient.Response response = new ZeoClient.Response();
            ZeoClient.ZeoContext context = GetZeoContext();
            ZeoClient.IdType entity;
            //context.Country = country;
            //context.StateName = state;

            ZeoClient.ZeoServiceClient client = new ZeoClient.ZeoServiceClient();
            response = client.GetIdType(idType, country, state, context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
            entity = response.Result as ZeoClient.IdType;
            return entity;
        }

        #endregion

        #region EmploymentDetails

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ViewName = "EmploymentDetails", MasterName = "_Menu", ResultType = "prepare", ModelType = "TCF.Channel.Zeo.Web.Models.EmploymentDetails")]
        public ActionResult EmploymentDetails()
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                List<SelectListItem> occupations = GetOccupations(context);

                Customer customerProspect = GetCustomerAO();
                EmploymentDetails employmentDetail = customerProspect.EmploymentDetails;
                employmentDetail.IsCompanionSearch = customerProspect.PersonalInformation.IsCompanionSearch;
                if (IsCustomerClosed(customerProspect))
                {
                    return RedirectToAction("ProfileSummary", "CustomerRegistration");
                }
                ViewBag.ChannelPartner = GetChannelPartnerName();

                employmentDetail.Occupations = occupations;
                return View("EmploymentDetails", employmentDetail);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="employmentDetail"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "EmploymentDetails", MasterName = "_Menu")]
        public ActionResult EmploymentDetails(EmploymentDetails employmentDetail, string pindetails, string profilesummary)
        {
            try
            {
                Customer customerProspect = GetCustomerAO();

                employmentDetail.EmployerPhoneNumber = string.IsNullOrEmpty(employmentDetail.EmployerPhoneNumber) ? string.Empty : employmentDetail.EmployerPhoneNumber.Replace("-", string.Empty);

                if (ModelState.IsValid)
                {
                    customerProspect.EmploymentDetails = employmentDetail;
                    Session["Customer"] = customerProspect;

                    if (!string.IsNullOrEmpty(profilesummary))
                    {
                        return RedirectToAction("ProfileSummary", "CustomerRegistration");
                    }
                    else
                    {
                        return RedirectToAction("PinDetails", "CustomerRegistration");
                    }
                }

                return View("EmploymentDetails", employmentDetail);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        #endregion

        #region PinDetails

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PinDetails()
        {
            Customer customerProspect = GetCustomerAO();
            if (IsCustomerClosed(customerProspect))
            {
                return RedirectToAction("ProfileSummary", "CustomerRegistration");
            }
            PinDetails pinDetail = customerProspect.PinDetails;
            pinDetail.PhoneNumber = customerProspect.PersonalInformation.PrimaryPhone;
            return View("PinDetails", pinDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pinDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "PinDetails", MasterName = "_Menu")]
        public ActionResult PinDetails(PinDetails pinDetails)
        {
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                if (ModelState.IsValid)
                {
                    Customer customerProspect = GetCustomerAO();
                    customerProspect.PinDetails = pinDetails;
                    pinDetails.PhoneNumber = customerProspect.PersonalInformation.PrimaryPhone;
                    Session["Customer"] = customerProspect;
                    return RedirectToAction("ProfileSummary", "CustomerRegistration");
                }
                return View("PinDetails", pinDetails);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        #endregion

        #region ProfileSummary

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProfileSummary(bool IsException = false, string ExceptionMessage = "")
        {
            try
            {
                ZeoClient.ZeoContext context = GetZeoContext();
                long agentSessionId = GetAgentSessionId();
                ZeoClient.ChannelPartner channelPartner = (ZeoClient.ChannelPartner)Session["ChannelPartner"];

                List<SelectListItem> occupations = GetOccupations(context);

                List<SelectListItem> masterCountries = GetMasterCountries(context);
                List<SelectListItem> legalCodes = GetLegalCodes(context);


                ProfileSummary profileSummary = GetProfileSummary();

                ViewBag.IsException = IsException;
                //This logic is added for the Status Code Error from TCF.
                // When the Status Code is returned from TCF then it will be appended with the error codes. 
                // So error codes will have 4 parts - "ProductCode/ProviderCode/ErrorCode/StatusCode so in pop up we should not show the Status Code field.Hence this logic is written.
                if (!string.IsNullOrEmpty(ExceptionMessage))
                {
                    string[] errMsg = ExceptionMessage.Split('|');

                    if (errMsg.Length > 1)
                    {
                        string[] errCode = errMsg[1].Split('.');

                        if (errCode.Length > 3)
                        {
                            string provider = ExceptionMessage.Substring(0, ExceptionMessage.IndexOf('|', 1));
                            string newErrCode = ReplaceLastOccurrence(errMsg[1], errCode[3], string.Empty);
                            ExceptionMessage = provider + ExceptionMessage.Substring(ExceptionMessage.IndexOf('|', 1)).Replace(errMsg[1], newErrCode);
                        }
                    }
                }

                ViewBag.ExceptionMessage = ExceptionMessage;

                var legalCode = legalCodes.SingleOrDefault(a => a.Value == profileSummary.LegalCode);

                if (legalCode != null)
                {
                    if (!string.IsNullOrWhiteSpace(legalCode.Value))
                        profileSummary.LegalCode = legalCode.Text;
                }

                var occupation = occupations.SingleOrDefault(a => a.Value == profileSummary.Profession);

                if (occupation != null)
                {
                    if (!string.IsNullOrWhiteSpace(occupation.Value))
                        profileSummary.Profession = occupation.Text;
                }

                var primaryCitizen = masterCountries.SingleOrDefault(a => a.Value == profileSummary.PrimaryCountryCitizenShip);

                if (primaryCitizen != null)
                {
                    if (!string.IsNullOrWhiteSpace(primaryCitizen.Value))
                        profileSummary.PrimaryCountryCitizenShip = primaryCitizen.Text;
                }

                var secondaryCitizen = masterCountries.SingleOrDefault(a => a.Value == profileSummary.SecondaryCountryCitizenShip);

                if (secondaryCitizen != null)
                {
                    if (!string.IsNullOrWhiteSpace(secondaryCitizen.Value))
                        profileSummary.SecondaryCountryCitizenShip = secondaryCitizen.Text;
                }

                var countryOfBirth = masterCountries.SingleOrDefault(a => a.Value == profileSummary.CountryOfBirth);
                if (countryOfBirth != null)
                {
                    if (!string.IsNullOrWhiteSpace(countryOfBirth.Value))
                        profileSummary.CountryOfBirth = countryOfBirth.Text;
                }

                return View("ProfileSummary", profileSummary);
            }
            catch (Exception ex)
            {
                VerifyException(ex); return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileSummary"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProfileSummary(ProfileSummary profileSummary)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

            Customer customer = GetCustomerAO();
            ZeoClient.Response response = new ZeoClient.Response();
            bool Isprofilective = false;

            if (ModelState.IsValid)
            {
                ZeoClient.CustomerProfile customerProfile = GetCustomerProfile(customer);
                bool editMode = !customer.IsNewCustomer;

                try
                {
                    ZeoClient.ZeoContext context = GetZeoContext();
                    context.Context = new Dictionary<string, object>();
                    context.SSOAttributes = GetSSOAttributes("SSO_AGENT_SESSION");
                    context.Context.Add("StatusCode", Session["StatusCode"]);
                    Session["StatusCode"] = string.Empty;

                    long alloyId;
                    Isprofilective = Activate(customerProfile, context, out alloyId);

                    customer.AlloyID = context.CustomerId = alloyId;
                    Session["Customer"] = customer;

                    if (!editMode)
                    {
                        response = alloyServiceClient.RegisterToClient(context);
                    }
                    else
                    {
                        response = alloyServiceClient.UpdateCustomerToClient(context);
                    }
                    if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);
                }
                catch (Exception ex)
                {
                    VerifyException(ex);
                    string[] errMsg = ex.Message.Split('|');

                    if (ex.Message.Contains("1111"))
                    {
                        Session["ShouldRedirect"] = true;
                        Session["controllerName"] = "CustomerSearch";
                        Session["ActionName"] = "CustomerSearch";
                    }

                    if (errMsg[1].Length >= 2 && !errMsg[1].Contains("1001.602.2"))
                    {
                        string[] errCode = errMsg[1].Split('.');
                        customer.IsNewCustomer = false;
                        Session["Customer"] = customer;

                        if (errCode.Length > 3)
                        {
                            Session["StatusCode"] = errCode.Length > 3 ? errCode[3] : string.Empty;
                        }
                        return RedirectToAction("ProfileSummary", "CustomerRegistration", new { IsException = true, ExceptionMessage = System.Web.HttpUtility.JavaScriptStringEncode(ViewBag.ExceptionMessage) });
                    }

                    return RedirectToAction("CustomerSearch", "CustomerSearch", new { IsException = true, ExceptionMessage = System.Web.HttpUtility.JavaScriptStringEncode(ViewBag.ExceptionMessage) });
                }

                if (Isprofilective)
                {
                    Session["NewUser"] = true;
                    Session["Customer"] = null;
                    Session["ChannelPartner"] = null;

                    Session.Remove("CurrentProspect");
                    if (!editMode)
                    {
                        Session["CustomerSession"] = null;
                    }
                    TempData["profileActive"] = System.Configuration.ConfigurationManager.AppSettings.Get("LANDING_ACTIVATIONUSER_MESSAGE");

                    return RedirectToAction("ValidateCustomerStatusAndId", "CustomerSearch", new { id = customer.AlloyID, Common.CardSearchType.Other, calledFrom = "CustomerSummaryPage" });
                }
            }
            ModelState.AddModelError(string.Empty, "Please fill all required fields in customer registration flow.");
            return View("ProfileSummary", profileSummary);
        }

        #endregion

        #region Private methods
        private ProfileSummary GetProfileSummary()
        {
            ProfileSummary profileSummary = new ProfileSummary();

            Customer customer = GetCustomerAO();

            //MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            //Desktop desktop = new Desktop();
            //sharedData.Prospect prospect = GetProspect(customerProspect);

            if (customer.PersonalInformation != null)
            {
                profileSummary.Name = customer.PersonalInformation.FirstName + " " + customer.PersonalInformation.MiddleName + " " + customer.PersonalInformation.LastName;
                profileSummary.Gender = customer.PersonalInformation.Gender;
                profileSummary.PrimaryPhone = customer.PersonalInformation.PrimaryPhone;
                profileSummary.Email = customer.PersonalInformation.Email;
                profileSummary.Address = string.Format("{0}, {1}, {2}, {3},{4}", customer.PersonalInformation.Address1, customer.PersonalInformation.Address2, customer.PersonalInformation.City, customer.PersonalInformation.State, customer.PersonalInformation.ZipCode);
                profileSummary.MailingAddress = string.Format("{0}, {1}, {2}, {3},{4}", customer.PersonalInformation.MailingAddress1, customer.PersonalInformation.MailingAddress2,
                    customer.PersonalInformation.MailingCity, customer.PersonalInformation.MailingState, customer.PersonalInformation.MailingZipCode);
                profileSummary.Notes = customer.PersonalInformation.Notes;//AL-184
                profileSummary.CustomerProfileStatus = customer.PersonalInformation.CustomerProfileStatus;
                profileSummary.Group1 = customer.PersonalInformation.Group1;
                profileSummary.Group2 = customer.PersonalInformation.Group2;
                profileSummary.SSN = GetSSNNumber(customer.PersonalInformation.SSN);
                profileSummary.DateOfBirth = customer.PersonalInformation.DateOfBirth;
            }

            if (customer.IdentificationInformation != null)
            {
                profileSummary.MotherMaidenName = customer.IdentificationInformation.MotherMaidenName;
                profileSummary.Country = customer.IdentificationInformation.Country;
                profileSummary.GovtIdIssueState = customer.IdentificationInformation.GovtIdIssueState;
                profileSummary.GovtIDType = customer.IdentificationInformation.GovtIDType;
                profileSummary.IDExpirationDate = customer.IdentificationInformation.IDExpireDate;
                profileSummary.IDIssueDate = customer.IdentificationInformation.IDIssuedDate;
                profileSummary.GovernmentId = customer.IdentificationInformation.GovernmentId;
                profileSummary.CountryOfBirth = customer.IdentificationInformation.CountryOfBirth;
                profileSummary.ClientID = customer.IdentificationInformation.ClientID;
                profileSummary.LegalCode = customer.IdentificationInformation.LegalCode;
                profileSummary.PrimaryCountryCitizenShip = customer.IdentificationInformation.PrimaryCountryCitizenShip;
                profileSummary.SecondaryCountryCitizenShip = customer.IdentificationInformation.SecondaryCountryCitizenShip;
                profileSummary.MGIAlloyID = customer.IdentificationInformation.MGIAlloyID;
            }

            if (customer.EmploymentDetails != null)
            {
                profileSummary.Profession = AlloyUtil.TrimString(customer.EmploymentDetails.Profession);
                profileSummary.OccupationDescription = AlloyUtil.TrimString(customer.EmploymentDetails.OccupationDescription);
                profileSummary.EmployerName = AlloyUtil.TrimString(customer.EmploymentDetails.EmployerName);
                profileSummary.EmployerPhoneNumber = string.IsNullOrEmpty(customer.EmploymentDetails.EmployerPhoneNumber) ? string.Empty :
                    customer.EmploymentDetails.EmployerPhoneNumber.Replace("-", string.Empty);
            }

            if (customer.PinDetails != null)
            {
                profileSummary.Pin = customer.PinDetails.Pin;
            }
            profileSummary.IsNewCustomer = customer.IsNewCustomer;

            return profileSummary;
        }

        private Customer GetCustomerAO()
        {
            if (Session["Customer"] != null)
            {
                return (Customer)Session["Customer"];
            }
            return new Customer()
            {
                IsNewCustomer = true,
                PersonalInformation = new PersonalInformation() { CustomerProfileStatus = Helper.ProfileStatus.Inactive },
                IdentificationInformation = new IdentificationInformation(),
                EmploymentDetails = new EmploymentDetails(),
                PinDetails = new PinDetails()
            };
        }

        private string GetSSNNumber(string SSN)
        {
            var maskedValue = "***-**-";
            string result = string.Empty;
            var subResult = string.Empty;

            if (!string.IsNullOrWhiteSpace(SSN))
            {
                subResult = SSN.Substring(SSN.Length - Math.Min(4, SSN.Length));
                result = maskedValue + subResult;
            }
            return result;
        }

        //To be Removed.
        private bool IsCustomerClosed(Customer customerProspect)
        {
            bool status = false;
            if ((customerProspect.PersonalInformation.CustomerProfileStatus) == Helper.ProfileStatus.Closed && !customerProspect.IsNewCustomer)
            {
                status = true;
            }
            return status;
        }

        private bool Activate(ZeoClient.CustomerProfile customerProfile, ZeoClient.ZeoContext context, out long alloyId)
        {
            ZeoClient.ZeoServiceClient ZeoClient = new ZeoClient.ZeoServiceClient();

            if (!string.IsNullOrWhiteSpace(customerProfile.CustomerId) && long.Parse(customerProfile.CustomerId) == 0 )
            {
                ZeoClient.Response activationResponse = ZeoClient.InsertCustomer(customerProfile, context);
                if (WebHelper.VerifyException(activationResponse)) throw new ZeoWebException(activationResponse.Error.Details);
                alloyId = Convert.ToInt64(activationResponse.Result);
                customerProfile.CustomerId = Convert.ToString(alloyId);
                Session["CustomerAO"] = customerProfile;
            }
            else
            {
                ZeoClient.Response activationResponse = ZeoClient.UpdateCustomer(customerProfile, context);
                if (WebHelper.VerifyException(activationResponse)) throw new ZeoWebException(activationResponse.Error.Details);
                ZeoClient.CustomerSession customerSession = Session["CustomerSession"] as ZeoClient.CustomerSession;
                if (customerSession != null)
                {
                    customerSession.Customer = customerProfile;
                    Session["CustomerAO"] = customerProfile;
                    Session["CustomerSession"] = customerSession;
                }
                alloyId = Convert.ToInt64(customerProfile.CustomerId);
            }

            return true;

        }

        private string ReplaceLastOccurrence(string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);

            if (place == -1)
                return source;

            string result = source.Remove(place - 1, find.Length + 1).Insert(place - 1, replace);
            return result;
        }

        private List<SelectListItem> GetProfileStatus()
        {
            List<SelectListItem> ProfileStatuses = new List<SelectListItem>();

            Array itemNames = Enum.GetNames(typeof(Helper.ProfileStatus));

            for (int i = 0; i < itemNames.Length; i++)
            {
                ProfileStatuses.Add(new SelectListItem() { Text = itemNames.GetValue(i).ToString(), Value = itemNames.GetValue(i).ToString() });
            }

            return ProfileStatuses;
        }

        private List<SelectListItem> GetOccupations(ZeoClient.ZeoContext context)
        {
            ZeoClient.ZeoServiceClient alloyServiceClient = new ZeoClient.ZeoServiceClient();

            ZeoClient.Response response = alloyServiceClient.GetOccupations(context);
            if (WebHelper.VerifyException(response)) throw new ZeoWebException(response.Error.Details);

            List<ZeoClient.Occupation> occupations = response.Result as List<ZeoClient.Occupation>;
            List<SelectListItem> listitems = new List<SelectListItem>();
            if (occupations != null)
            {
                listitems = occupations.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code }).ToList();
            }
            listitems.Insert(0, DefaultListItem());

            return listitems;
        }

        private string[] splitstring(string message)
        {
            string[] strmessage = null;

            if (message.Contains("|"))
                strmessage = message.Split(new string[] { "|" }, StringSplitOptions.None);
            else
            {
                string errorCode = "1000.100.9999";
                ZeoClient.ZeoContext context = GetZeoContext();
                string errorMessage = GetErrorMessage(errorCode, context);
                strmessage = errorMessage.Split(new string[] { "|" }, StringSplitOptions.None);
                strmessage[2] = message;
            }

            return strmessage;
        }
        #endregion
    }
}
