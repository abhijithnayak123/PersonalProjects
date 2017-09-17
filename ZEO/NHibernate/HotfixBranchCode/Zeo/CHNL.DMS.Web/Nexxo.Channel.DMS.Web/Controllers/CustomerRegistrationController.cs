using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sharedData = MGI.Channel.Shared.Server.Data;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Common;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class CustomerRegistrationController : BaseController
    {
        #region PersonalInfo

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CustomerSearch")]
        public ActionResult PersonalInformation(bool IsException = false, string ExceptionMsg = "")
        {
            long agentSessionId = GetAgentSessionId();
            Session["activeButton"] = "newcustomer";
            PersonalInformation personalInfo = null;
            Desktop client = new Desktop();
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            string channelPartner = Session["ChannelPartnerName"].ToString();
            var listGroups = client.GetGroups(channelPartner, mgiContext);
            CustomerProspect customerProspect = GetCustomerProspectSession();
            if ( IsCustomerClosed(customerProspect) )
            {
                return RedirectToAction("ProfileSummary", "CustomerRegistration");
            }
            ViewBag.IsProspectCreated = ( customerProspect.AlloyID != 0 );

            if ( customerProspect.IsNewCustomer )
            {
                ViewBag.profileSyncIn = true;
            }
            else
            {
                ViewBag.profileSyncIn = false;
                if ( customerProspect.PersonalInformation.Preference.ClientProfileStatus == ProfileStatus.Inactive )
                    customerProspect.PersonalInformation.Preference.CustomerProfileStatus = customerProspect.PersonalInformation.Preference.ClientProfileStatus;
            }
            personalInfo = customerProspect.PersonalInformation;
            personalInfo.contactDetail.LStates = client.USStates(agentSessionId, mgiContext);
            personalInfo.contactDetail.LPrimaryPhonetype = client.PhoneType(agentSessionId, mgiContext);
            personalInfo.contactDetail.LPrimaryPhoneProvider = client.PhoneProvider(agentSessionId, mgiContext);
            personalInfo.contactDetail.LAlternatePhonetype = client.PhoneType(agentSessionId, mgiContext);
            personalInfo.contactDetail.LAlternatePhoneProvider = client.PhoneProvider(agentSessionId, mgiContext);
            personalInfo.Preference.LReceiptLanguage = client.GetRecieptLanguages();
            personalInfo.LGroup1 = listGroups;
            personalInfo.LGroup2 = listGroups;
            personalInfo.Notes = customerProspect.PersonalInformation.Notes;
            personalInfo.Preference.LProfileStatus = client.GetProfilestatus(agentSessionId, mgiContext);

            if ( TempData["IsCompanionSearch"] != null )
            {
                personalInfo.IsCompanionSearch = Boolean.Parse(TempData["IsCompanionSearch"].ToString());
            }

            if ( ( int )Session["UserRoleId"] == ( int )UserRoles.Teller )
            {
                personalInfo.Preference.LProfileStatus = personalInfo.Preference.LProfileStatus.Where(x => x.Text != ProfileStatus.Closed.ToString());
            }

            if ( customerProspect.IsNewCustomer && personalInfo.personalDetail != null && string.IsNullOrEmpty(personalInfo.personalDetail.SSN) )
                personalInfo.personalDetail.SSN = Convert.ToString(TempData["ActualSSN"]);

            if ( string.IsNullOrEmpty(personalInfo.personalDetail.SSN) )
            {
                personalInfo.personalDetail.SSN = personalInfo.personalDetail.ActualSSN;
            }
            ViewBag.IsException = IsException;
            ViewBag.ExceptionMsg = ExceptionMsg;
            ChannelPartner channelpartner = client.GetChannelPartner(Session["ChannelPartnerName"].ToString(), mgiContext);
            ViewBag.IsNotesEnable = channelpartner.IsNotesEnable;
            //Developed by: Sunil Shetty || Date: 09\06\2015
            //AL-533 : Mailing Address will not be visible in Customer registration on the base of IsMailingAddressEnable value			
            ViewBag.IsMailingAddressEnable = channelpartner.IsMailingAddressEnable;
            return View("PersonalInformation", personalInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomHandleErrorAttribute(ActionName = "PersonalInformation", ControllerName = "CustomerRegistration", ResultType = "redirect")]
        public ActionResult PersonalInformation(FormCollection formInfo)
        {
            long agentSessionId = GetAgentSessionId();
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            PersonalInformation personalInfo = SetProspect(formInfo, mgiContext);
            CustomerProspect customerProspect = GetCustomerProspectSession();
            ViewBag.IsProspectCreated = ( customerProspect.AlloyID != 0 );
            customerProspect.PersonalInformation = personalInfo;
            Session["CustomerProspect"] = customerProspect;
            customerProspect.CustomerScreen = CustomerScreen.PersonalInfo;
            sharedData.Prospect prospect = GetProspect(customerProspect);
            Desktop client = new Desktop();
            if ( customerProspect.IsNewCustomer )
            {
                customerProspect.AlloyID = Convert.ToInt64(client.GeneratePAN(( ( Hashtable )Session["HTSessions"] )["AgentSessionId"].ToString(), prospect, mgiContext));
            }


            if ( ModelState.IsValid )
            {
                client.SaveCustomerProfile(( ( Hashtable )Session["HTSessions"] )["AgentSessionId"].ToString(), customerProspect.AlloyID, prospect, mgiContext, !customerProspect.IsNewCustomer);
                if ( personalInfo.Preference.CustomerProfileStatus != ProfileStatus.Closed )
                    return RedirectToAction("IdentificationInformation", "CustomerRegistration");
                else
                    return RedirectToAction("ProfileSummary", "CustomerRegistration");
            }
            personalInfo.contactDetail.LStates = client.USStates(agentSessionId, mgiContext);
            personalInfo.contactDetail.LPrimaryPhonetype = client.PhoneType(agentSessionId, mgiContext);
            personalInfo.contactDetail.LPrimaryPhoneProvider = client.PhoneProvider(agentSessionId, mgiContext);
            personalInfo.contactDetail.LAlternatePhonetype = client.PhoneType(agentSessionId, mgiContext);
            personalInfo.contactDetail.LAlternatePhoneProvider = client.PhoneProvider(agentSessionId, mgiContext);
            personalInfo.Preference.LReceiptLanguage = client.GetRecieptLanguages();
            personalInfo.Preference.LProfileStatus = client.GetProfilestatus(agentSessionId, mgiContext);
            if ( ( int )Session["UserRoleId"] == ( int )UserRoles.Teller )
            {
                personalInfo.Preference.LProfileStatus = personalInfo.Preference.LProfileStatus.Where(x => x.Text != ProfileStatus.Closed.ToString());
            }
            return View("PersonalInformation", personalInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalinformation"></param>
        /// <param name="prospect"></param>
        private PersonalInformation SetProspect(FormCollection personalinformation, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            PersonalInformation personalInfo = new PersonalInformation();
            personalInfo.personalDetail = new PersonalDetail();
            personalInfo.contactDetail = new ContactDetail();
            personalInfo.Preference = new Preference();

            personalInfo.IsCompanionSearch = Boolean.Parse(personalinformation[PersonalInfo.IsCompanionSearch]);
            personalInfo.personalDetail.FirstName = NexxoUtil.TrimString(personalinformation[PersonalInfo.FirstName]);
            personalInfo.personalDetail.MiddleName = NexxoUtil.TrimString(personalinformation[PersonalInfo.MiddleName]);
            personalInfo.personalDetail.LastName = NexxoUtil.TrimString(personalinformation[PersonalInfo.LastName]);
            personalInfo.personalDetail.SecondLastName = NexxoUtil.TrimString(personalinformation[PersonalInfo.SecondLastName]);
            personalInfo.personalDetail.Gender = personalinformation[PersonalInfo.Gender];
            personalInfo.contactDetail.PrimaryPhone = personalinformation[PersonalInfo.PrimaryPhone].Replace("-", string.Empty);
            personalInfo.contactDetail.PrimaryPhoneProvider = string.IsNullOrEmpty(personalinformation[PersonalInfo.PrimaryPhoneProvider]) ? string.Empty : personalinformation[PersonalInfo.PrimaryPhoneProvider];
            personalInfo.contactDetail.PrimaryPhoneType = string.IsNullOrEmpty(personalinformation[PersonalInfo.PrimaryPhoneType]) ? string.Empty : personalinformation[PersonalInfo.PrimaryPhoneType];
            personalInfo.contactDetail.AlternativePhone = string.IsNullOrEmpty(personalinformation[PersonalInfo.AlternativePhone]) ? string.Empty : personalinformation[PersonalInfo.AlternativePhone].Replace("-", string.Empty);
            personalInfo.contactDetail.AlternativePhoneProvider = string.IsNullOrEmpty(personalinformation[PersonalInfo.AlternativePhoneProvider]) ? string.Empty : personalinformation[PersonalInfo.AlternativePhoneProvider];
            personalInfo.contactDetail.AlternativePhoneType = string.IsNullOrEmpty(personalinformation[PersonalInfo.AlternativePhoneType]) ? string.Empty : personalinformation[PersonalInfo.AlternativePhoneType];
            personalInfo.contactDetail.Email = personalinformation[PersonalInfo.Email];
            personalInfo.contactDetail.Address1 = NexxoUtil.TrimString(personalinformation[PersonalInfo.Address1]);
            personalInfo.contactDetail.Address2 = NexxoUtil.TrimString(personalinformation[PersonalInfo.Address2]);
            personalInfo.contactDetail.City = NexxoUtil.TrimString(personalinformation[PersonalInfo.City]);
            personalInfo.contactDetail.State = NexxoUtil.TrimString(personalinformation[PersonalInfo.State]);
            personalInfo.contactDetail.ZipCode = personalinformation[PersonalInfo.ZipCode];
            personalInfo.ReferralNumber = personalinformation[PersonalInfo.ReferralNumber];
            personalInfo.Preference.ReceiveTextMessage = WebHelper.GetCheckBoxValue(personalinformation[PersonalInfo.ReceiveTextMessage]);
            personalInfo.WoodForestAccountHolder = WebHelper.GetCheckBoxValue(personalinformation[PersonalInfo.WoodForestAccountHolder]);
            personalInfo.Preference.DoNotCall = WebHelper.GetCheckBoxValue(personalinformation[PersonalInfo.DoNotCall]);
            personalInfo.Notes = NexxoUtil.TrimString(personalinformation[PersonalInfo.Notes]);
            string mailingAddressDifferent = personalinformation[PersonalInfo.MailingAddressDifferent] == null ? "false" : personalinformation[PersonalInfo.MailingAddressDifferent];
            personalInfo.contactDetail.MailingAddressDifferent = WebHelper.GetCheckBoxValue(mailingAddressDifferent);
            if ( personalInfo.contactDetail.MailingAddressDifferent )
            {
                personalInfo.contactDetail.MailingAddress1 = NexxoUtil.TrimString(personalinformation[PersonalInfo.MailingAddress1]);
                personalInfo.contactDetail.MailingAddress2 = NexxoUtil.TrimString(personalinformation[PersonalInfo.MailingAddress2]);
                personalInfo.contactDetail.MailingCity = NexxoUtil.TrimString(personalinformation[PersonalInfo.MailingCity]);
                personalInfo.contactDetail.MailingState = NexxoUtil.TrimString(personalinformation[PersonalInfo.MailingState]);
                personalInfo.contactDetail.MailingState = NexxoUtil.TrimString(string.IsNullOrEmpty(personalInfo.contactDetail.MailingState) ? string.Empty : personalInfo.contactDetail.MailingState);
                personalInfo.contactDetail.MailingZipCode = personalinformation[PersonalInfo.MailingZipCode];
            }
            else
            {
                personalInfo.contactDetail.MailingAddress1 = NexxoUtil.TrimString(personalinformation[PersonalInfo.Address1]);
                personalInfo.contactDetail.MailingAddress2 = NexxoUtil.TrimString(personalinformation[PersonalInfo.Address2]);
                personalInfo.contactDetail.MailingCity = NexxoUtil.TrimString(personalinformation[PersonalInfo.City]);
                personalInfo.contactDetail.MailingState = NexxoUtil.TrimString(personalinformation[PersonalInfo.State]);
                personalInfo.contactDetail.MailingZipCode = personalinformation[PersonalInfo.ZipCode];
            }

            personalInfo.Group1 = personalinformation[PersonalInfo.Group1];
            personalInfo.Group2 = personalinformation[PersonalInfo.Group2];
            //this code for Channel Partner Id.
            //Desktop desktopClient = new Desktop();
            //string channelPartner = string.Empty;
            //if (this.Session["ChannelPartnerName"] == null)
            //	channelPartner = System.Configuration.ConfigurationManager.AppSettings.Get("ChannelPartner");
            //else
            //	channelPartner = this.Session["ChannelPartnerName"].ToString();
            //Guid channelPartnerId = desktopClient.GetChannelPartner(channelPartner, mgiContext).rowguid;
            //personalInfo.ChannelPartnerId = channelPartnerId;

            personalInfo.personalDetail.ActualSSN = personalinformation[PersonalInfo.SSN];
            //prospect.ReceiptLanguage = personalinformation[PersonalInfo.ReceiptLanguage];

            personalInfo.Preference.CustomerProfileStatus = ( ProfileStatus )Enum.Parse(typeof(ProfileStatus), personalinformation[PersonalInfo.CustomerProfileStatus]);
            personalInfo.Preference.ClientProfileStatus = ( ProfileStatus )Enum.Parse(typeof(ProfileStatus), personalinformation[PersonalInfo.ClientProfileStatus]);
            return personalInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        private string GetGoldCardNumber(string GoldCardNumber)
        {
            if ( GoldCardNumber != null && GoldCardNumber.Trim() != "0" )
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
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            Desktop utilityServiceClient = new Desktop();

            if ( Session["CustomerProspect"] != null )
            {
                CustomerProspect customerProspect = ( CustomerProspect )Session["CustomerProspect"];
                if ( customerProspect.AlloyID != 0 )
                    mgiContext.AlloyId = customerProspect.AlloyID;
            }
            else
                mgiContext.AlloyId = 0;

            bool isSSNValid = utilityServiceClient.ValidateSSN(Session["sessionId"].ToString(), SSN, mgiContext);
            string ssnmsg = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.SSNITINExists;

            if ( isSSNValid )
            {
                ssnmsg = "";
            }
            var jsonData = new
            {
                msg = ssnmsg,
                success = true
            };
            return Json(jsonData);
        }

        /// <summary>
        /// AL-231
        /// </summary>
        /// <param name="profileStatus"></param>
        /// <returns></returns>
        public ActionResult CanChangeProfileStatus(string profileStatus)
        {
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            var htSessions = ( Hashtable )Session["HTSessions"];
            var agentSession = ( ( Server.Data.AgentSession )( htSessions["TempSessionAgent"] ) );
            string shoppingCartStatus = "empty";
            sharedData.CustomerSession customerSession = ( sharedData.CustomerSession )Session["CustomerSession"];
            //AL-375 changes start
            Desktop desktopClient = new Desktop();
            if ( profileStatus == "Closed" )
            {
                if ( customerSession != null )
                {
                    sharedData.ShoppingCart shoppingCart = desktopClient.ShoppingCart(customerSession.CustomerSessionId);
                    var shoppingCartDetail = ShoppingCartHelper.ShoppingCartDetailed(shoppingCart);

                    if ( shoppingCartDetail.Items.Count != 0 )
                    {
                        shoppingCartStatus = "nonempty";

                    }
                }
            }
            //AL-375 end
            var jsonData = new
            {
                cartStatus = shoppingCartStatus,
                success = desktopClient.CanChangeProfileStatus(GetAgentSessionId(), agentSession.Agent.Id, profileStatus, mgiContext)//AL-375
            };
            return Json(jsonData);
        }
        //AL-375 changes
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetShoppingCartEmptyWhenClosedPopUp(string shoppingCartStatus)
        {
            return PartialView("_ShoppingCartEmptyWhenClosedConfirm");
        }
        //AL-375 end
        private sharedData.Prospect GetProspect(CustomerProspect customerProspect)
        {
            sharedData.Prospect prospect = new sharedData.Prospect();
            if ( customerProspect.PersonalInformation != null )
            {
                var personalinformation = customerProspect.PersonalInformation;
                prospect.FName = NexxoUtil.TrimString(personalinformation.personalDetail.FirstName);
                prospect.MName = NexxoUtil.TrimString(personalinformation.personalDetail.MiddleName);
                prospect.LName = NexxoUtil.TrimString(personalinformation.personalDetail.LastName);
                prospect.LName2 = NexxoUtil.TrimString(personalinformation.personalDetail.SecondLastName);
                prospect.Gender = personalinformation.personalDetail.Gender;
                prospect.Phone1 = personalinformation.contactDetail.PrimaryPhone;
                prospect.Phone1Provider = personalinformation.contactDetail.PrimaryPhoneProvider;
                prospect.Phone1Type = personalinformation.contactDetail.PrimaryPhoneType;
                prospect.Phone2 = personalinformation.contactDetail.AlternativePhone;
                prospect.Phone2Provider = personalinformation.contactDetail.AlternativePhoneProvider;
                prospect.Phone2Type = personalinformation.contactDetail.AlternativePhoneType;
                prospect.Email = personalinformation.contactDetail.Email;
                prospect.Address1 = NexxoUtil.TrimString(personalinformation.contactDetail.Address1);
                prospect.Address2 = NexxoUtil.TrimString(personalinformation.contactDetail.Address2);
                prospect.City = NexxoUtil.TrimString(personalinformation.contactDetail.City);
                prospect.State = NexxoUtil.TrimString(personalinformation.contactDetail.State);
                prospect.PostalCode = personalinformation.contactDetail.ZipCode;
                prospect.ReferralCode = personalinformation.ReferralNumber;
                prospect.TextMsgOptIn = personalinformation.Preference.ReceiveTextMessage;
                prospect.IsAccountHolder = personalinformation.WoodForestAccountHolder;
                prospect.DoNotCall = personalinformation.Preference.DoNotCall;

                prospect.Groups = new List<string>();
                prospect.Notes = NexxoUtil.TrimString(personalinformation.Notes);
                if ( !string.IsNullOrEmpty(personalinformation.Group1) )
                    prospect.Groups.Add(personalinformation.Group1);
                if ( !string.IsNullOrEmpty(personalinformation.Group2) )
                    prospect.Groups.Add(personalinformation.Group2);

                prospect.MailingAddressDifferent = personalinformation.contactDetail.MailingAddressDifferent;
                prospect.MailingAddress1 = NexxoUtil.TrimString(personalinformation.contactDetail.MailingAddress1);
                prospect.MailingAddress2 = NexxoUtil.TrimString(personalinformation.contactDetail.MailingAddress2);
                prospect.MailingCity = NexxoUtil.TrimString(personalinformation.contactDetail.MailingCity);
                prospect.MailingState = NexxoUtil.TrimString(personalinformation.contactDetail.MailingState);
                prospect.MailingState = prospect.MailingState;
                prospect.MailingZipCode = personalinformation.contactDetail.MailingZipCode;

                prospect.ChannelPartnerId = personalinformation.channelPartner.rowguid;
                // Remove the code to fix the Defect  DE3633

                prospect.SSN = personalinformation.personalDetail.ActualSSN;

                prospect.ProfileStatus = personalinformation.Preference.CustomerProfileStatus;
            }
            if ( customerProspect.IdentificationInformation != null )
            {
                var identificationInformation = customerProspect.IdentificationInformation;
                prospect.MoMaName = NexxoUtil.TrimString(identificationInformation.MotherMaidenName);
                prospect.ClientID = NexxoUtil.TrimString(identificationInformation.ClientID);
                prospect.LegalCode = NexxoUtil.TrimString(identificationInformation.LegalCode);
                prospect.PrimaryCountryCitizenShip = identificationInformation.PrimaryCountryCitizenShip;
                prospect.SecondaryCountryCitizenShip = identificationInformation.SecondaryCountryCitizenShip;


                if ( !string.IsNullOrEmpty(identificationInformation.DateOfBirth) )
                {
                    prospect.DateOfBirth = Convert.ToDateTime(identificationInformation.DateOfBirth);
                }
                else
                {
                    prospect.DateOfBirth = null;
                }
                prospect.ID = new sharedData.Identification()
                {
                    Country = identificationInformation.Country,
                    CountryOfBirth = identificationInformation.CountryOfBirth,
                    IDType = identificationInformation.GovtIDType,
                    State = string.IsNullOrEmpty(identificationInformation.GovtIdIssueState) ? string.Empty : identificationInformation.GovtIdIssueState,
                    GovernmentId = identificationInformation.GovernmentId

                };
                if ( string.IsNullOrEmpty(identificationInformation.IDIssuedDate) )
                {
                    prospect.ID.IssueDate = null;
                }
                else
                {
                    prospect.ID.IssueDate = Convert.ToDateTime(identificationInformation.IDIssuedDate);
                }
                if ( string.IsNullOrEmpty(identificationInformation.IDExpireDate) )
                {
                    prospect.ID.ExpirationDate = null;
                }
                else
                {
                    prospect.ID.ExpirationDate = Convert.ToDateTime(identificationInformation.IDExpireDate);
                }
            }
            if ( customerProspect.EmploymentDetails != null )
            {
                var empDetails = customerProspect.EmploymentDetails;
                prospect.Occupation = NexxoUtil.TrimString(empDetails.Profession);
                prospect.OccupationDescription = NexxoUtil.TrimString(empDetails.OccupationDescription);
                prospect.Employer = NexxoUtil.TrimString(empDetails.EmployerName);
                prospect.EmployerPhone = string.IsNullOrEmpty(empDetails.EmployerPhoneNumber) ? string.Empty : empDetails.EmployerPhoneNumber.Replace("-", string.Empty);
            }
            if ( customerProspect.PinDetails != null )
            {
                prospect.PIN = NexxoUtil.TrimString(customerProspect.PinDetails.Pin);
            }
            prospect.CustomerScreen = customerProspect.CustomerScreen;
            return prospect;
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
            IDRequirement idrequirement = null;

            string idtype, state;

            if ( IdType == null || IdType != null && IdType.Trim(' ').ToLower() == "select" )
            { idtype = string.Empty; }
            else { idtype = IdType; }

            if ( State == null || State != null && State.Trim(' ').ToLower() == "select" )
            { state = string.Empty; }
            else { state = State; }

            if ( CountryId == null || CountryId != null && CountryId.Trim(' ').ToLower() == "select" )
            { CountryId = string.Empty; }

            idrequirement = GetIdentificationRequirements(CountryId, idtype, state);

            if ( idrequirement != null )
            { idRegex = idrequirement.Mask; HasExpirationDate = idrequirement.HasExpirationDate; }

            return Json(idrequirement, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GovtIdType(string CountryId)
        {
            Desktop uClient = new Desktop();
            ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            List<SelectListItem> GovtIdTypeIds = new List<SelectListItem>();
            GovtIdTypeIds = uClient.GetGovtIdType(GetAgentSessionId(), channelPartner.Id, mgiContext, CountryId);
            return Json(GovtIdTypeIds, JsonRequestBehavior.AllowGet);
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
            Desktop uClient = new Desktop();
            ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            List<SelectListItem> states = new List<SelectListItem>();
            states = uClient.GetStates(GetAgentSessionId(), channelPartner.Id, mgiContext, CountryId, GovtIdType);
            bool isNullItemExist = false;
            if ( states.RemoveAll(c => c.Value == null) >= 0 )
            {
                isNullItemExist = true;
            }
            if ( CountryId == "UNITED STATES" && ( GovtIdType == "NEW YORK BENEFITS ID" || GovtIdType == "NEW YORK CITY ID" ) )
            {
                states.FirstOrDefault(x => x.Text == "Select").Selected = false;
                states.FirstOrDefault(x => x.Text == "NEW YORK").Selected = true;
            }
            return Json(new { states = states, isNullItemExist = isNullItemExist }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IdentificationInformation(bool isExpiring = false)
        {
            Desktop client = new Desktop();
            long agentSessionId = GetAgentSessionId();
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            CustomerProspect customerProspect = GetCustomerProspectSession();
            if ( IsCustomerClosed(customerProspect) )
            {
                return RedirectToAction("ProfileSummary", "CustomerRegistration");
            }
            IdentificationInformation identificationInfo = customerProspect.IdentificationInformation;
            identificationInfo.LCountry = client.GetCountries(agentSessionId, identificationInfo.channelPartner.Id, mgiContext);
            identificationInfo.LGovtIDType = client.GetGovtIdType(agentSessionId, identificationInfo.channelPartner.Id, mgiContext, identificationInfo.Country);
            identificationInfo.LStates = client.GetStates(agentSessionId, identificationInfo.channelPartner.Id, mgiContext, identificationInfo.Country, identificationInfo.GovtIDType);

           // identificationInfo.LCountryOfBirth = GetMasterCountries(agentSessionId, identificationInfo.channelPartner.Id, mgiContext);
            identificationInfo.LLegalCodes = client.GetLegalCodes(agentSessionId, mgiContext);

            List<SelectListItem> masterCountires = GetMasterCountries(agentSessionId, identificationInfo.channelPartner.Id, mgiContext);

            identificationInfo.LCountryOfBirth = masterCountires;

            identificationInfo.LPrimaryCountryCitizenship = masterCountires;
            identificationInfo.LSecondaryCountryCitizenship = masterCountires;

            identificationInfo.CustomerMinimumAgeMessage = string.Format(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.DateOfBirthConfigureMessage.ToString(), identificationInfo.channelPartner.CustomerMinimumAge);

            if ( Convert.ToBoolean(TempData["IsExpired"]) )
            {
                ModelState.AddModelError("IDExpireDate", "ID Expiration date can not be in the past.");
                @ViewBag.ExpiredErrorMessage = true;
            }
            if ( isExpiring )
            {
                ViewBag.ErrorMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.IDExpirationMessage;
            }
            return View("IdentificationInformation", identificationInfo);
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
            Desktop client = new Desktop();
            long agentSessionId = GetAgentSessionId();
            CustomerProspect customerProspect = GetCustomerProspectSession();
            customerProspect.IdentificationInformation = identificationInfo;
            customerProspect.CustomerScreen = CustomerScreen.Identification;
            Session["CustomerProspect"] = customerProspect;
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();

            identificationInfo.GovtIDType = HttpUtility.UrlDecode(identificationInfo.GovtIDType);
            identificationInfo.LCountry = client.GetCountries(agentSessionId, identificationInfo.channelPartner.Id, mgiContext);
            
            identificationInfo.LGovtIDType = client.GetGovtIdType(agentSessionId, identificationInfo.channelPartner.Id, mgiContext, identificationInfo.Country);
            identificationInfo.LStates = client.GetStates(agentSessionId, identificationInfo.channelPartner.Id, mgiContext, identificationInfo.Country, identificationInfo.GovtIDType);
            identificationInfo.LLegalCodes = client.GetLegalCodes(agentSessionId, mgiContext);

            List<SelectListItem> masterCountires = GetMasterCountries(agentSessionId, identificationInfo.channelPartner.Id, mgiContext);

            identificationInfo.LCountryOfBirth = masterCountires;

            identificationInfo.LPrimaryCountryCitizenship = masterCountires;
            identificationInfo.LSecondaryCountryCitizenship = masterCountires;

            GetIdRequirements(identificationInfo.Country, identificationInfo.GovtIDType, identificationInfo.GovtIdIssueState);


            if ( identificationInfo.GovernmentId != null )
                if ( !System.Text.RegularExpressions.Regex.IsMatch(identificationInfo.GovernmentId, this.idRegex) )
                    ModelState.AddModelError("GovernmentId", "Please enter a valid ID Number in appropriate format.");


            if ( identificationInfo.GovernmentId != null )
                if ( this.HasExpirationDate == true && ( string.IsNullOrEmpty(identificationInfo.IDExpireDate) || identificationInfo.IDExpireDate.Trim() == DateTime.MinValue.ToShortDateString() ) )
                {
                    if ( !( ( identificationInfo.GovtIDType == "U.S. STATE IDENTITY CARD" && identificationInfo.GovtIdIssueState == "ARIZONA" ) ||
						( identificationInfo.GovtIDType == "U.S. STATE IDENTITY CARD" && identificationInfo.GovtIdIssueState == "TEXAS" ) ||
						( identificationInfo.GovtIDType == "U.S. STATE IDENTITY CARD" && identificationInfo.GovtIdIssueState == "TENNESSEE" ) ||
						( identificationInfo.GovtIDType == "U.S. STATE IDENTITY CARD" && identificationInfo.GovtIdIssueState == "WYOMING" ) ) )
                        ModelState.AddModelError("IDExpireDate", "Please enter a valid expiration date in MM/DD/YYYY format.");
                }

            if ( ModelState.IsValid )
            {
                sharedData.Prospect prospect = GetProspect(customerProspect);
                client.SaveCustomerProfile(( ( Hashtable )Session["HTSessions"] )["AgentSessionId"].ToString(), customerProspect.AlloyID, prospect, mgiContext, !customerProspect.IsNewCustomer);
                return RedirectToAction("EmploymentDetails", "CustomerRegistration");
            }
            return View("IdentificationInformation", identificationInfo);
        }

        private List<SelectListItem> GetMasterCountries(long agentSessionId, long channelPartnerId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Desktop client = new Desktop();
            List<SelectListItem> mastercountries = new List<SelectListItem>();
            var masterCountries = client.GetMasterCountries(agentSessionId, channelPartnerId, mgiContext);
            if ( masterCountries != null )
            {
                mastercountries = masterCountries.Select(d => new SelectListItem() { Text = d.Name, Value = d.Abbr2 }).ToList();
            }
            mastercountries.Insert(0, DefaultListItem());
            return mastercountries;
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
        private Server.Data.IDRequirement GetIdentificationRequirements(string country, string idType, string state)
        {
            Server.Data.IDRequirement entity;
            ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            Desktop uClient = new Desktop();
            entity = uClient.GetIdRequirements(GetAgentSessionId(), channelPartner.Id, country, idType, state, mgiContext);
            return entity;
        }

        #endregion

        #region EmploymentDetails

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult EmploymentDetails()
        {
            Desktop client = new Desktop();
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            List<SelectListItem> occupations = client.GetOccupations(GetAgentSessionId(), mgiContext);

            CustomerProspect customerProspect = GetCustomerProspectSession();
            EmploymentDetails employmentDetail = customerProspect.EmploymentDetails;
            if ( IsCustomerClosed(customerProspect) )
            {
                return RedirectToAction("ProfileSummary", "CustomerRegistration");
            }
            ViewBag.ChannelPartner = GetChannelPartnerName();

            employmentDetail.Occupations = occupations;
            return View("EmploymentDetails", employmentDetail);
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
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            sharedData.Prospect prospect = new sharedData.Prospect();
            Desktop client = new Desktop();
            CustomerProspect customerProspect = GetCustomerProspectSession();

            employmentDetail.EmployerPhoneNumber = string.IsNullOrEmpty(employmentDetail.EmployerPhoneNumber) ? string.Empty : employmentDetail.EmployerPhoneNumber.Replace("-", string.Empty);

            if ( ModelState.IsValid )
            {
                customerProspect.EmploymentDetails = employmentDetail;
                Session["CustomerProspect"] = customerProspect;
                customerProspect.CustomerScreen = CustomerScreen.Employment;
                prospect = GetProspect(customerProspect);
                client.SaveCustomerProfile(( ( Hashtable )Session["HTSessions"] )["AgentSessionId"].ToString(), customerProspect.AlloyID, prospect, mgiContext, !customerProspect.IsNewCustomer);

                if ( !string.IsNullOrEmpty(profilesummary) )
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

        #endregion

        #region PinDetails

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PinDetails()
        {
            CustomerProspect customerProspect = GetCustomerProspectSession();
            if ( IsCustomerClosed(customerProspect) )
            {
                return RedirectToAction("ProfileSummary", "CustomerRegistration");
            }
            PinDetails pinDetail = customerProspect.PinDetails;
            pinDetail.PhoneNumber = customerProspect.PersonalInformation.contactDetail.PrimaryPhone;
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
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            Desktop client = new Desktop();

            if ( ModelState.IsValid )
            {
                CustomerProspect customerProspect = GetCustomerProspectSession();
                customerProspect.PinDetails = pinDetails;
                pinDetails.PhoneNumber = customerProspect.PersonalInformation.contactDetail.PrimaryPhone;
                Session["CustomerProspect"] = customerProspect;
                customerProspect.CustomerScreen = CustomerScreen.PinDetails;
                sharedData.Prospect prospect = GetProspect(customerProspect);
                client.SaveCustomerProfile(( ( Hashtable )Session["HTSessions"] )["AgentSessionId"].ToString(), customerProspect.AlloyID, prospect, mgiContext, !customerProspect.IsNewCustomer);
                return RedirectToAction("ProfileSummary", "CustomerRegistration");
            }
            return View("PinDetails", pinDetails);
        }

        #endregion

        #region ProfileSummary

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProfileSummary(bool IsException = false, string ExceptionMsg = "")
        {

            Desktop client = new Desktop();
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new Server.Data.MGIContext();
            long agentSessionId = GetAgentSessionId();
            ChannelPartner channelPartner = ( ChannelPartner )System.Web.HttpContext.Current.Session["ChannelPartner"];

            List<SelectListItem> occupations = client.GetOccupations(agentSessionId, mgiContext);
            List<SelectListItem> masterCountries = GetMasterCountries(agentSessionId, channelPartner.Id, mgiContext);
            List<SelectListItem> legalCodes = client.GetLegalCodes(agentSessionId, mgiContext);

            ProfileSummary profileSummary = GetProfileSummary(agentSessionId);

            ViewBag.IsException = IsException;
            ViewBag.ExceptionMsg = ExceptionMsg;

            var legalCode = legalCodes.SingleOrDefault(a => a.Value == profileSummary.LegalCode);

            if ( legalCode != null )
            {
                if ( !string.IsNullOrWhiteSpace(legalCode.Value) )
                    profileSummary.LegalCode = legalCode.Text;
            }

            var occupation = occupations.SingleOrDefault(a => a.Value == profileSummary.Profession);

            if ( occupation != null )
            {
                if ( !string.IsNullOrWhiteSpace(occupation.Value) )
                    profileSummary.Profession = occupation.Text;
            }

            var primaryCitizen = masterCountries.SingleOrDefault(a => a.Value == profileSummary.PrimaryCountryCitizenShip);

            if ( primaryCitizen != null )
            {
                if ( !string.IsNullOrWhiteSpace(primaryCitizen.Value) )
                    profileSummary.PrimaryCountryCitizenShip = primaryCitizen.Text;
            }

            var secondaryCitizen = masterCountries.SingleOrDefault(a => a.Value == profileSummary.SecondaryCountryCitizenShip);

            if ( secondaryCitizen != null )
            {
                if ( !string.IsNullOrWhiteSpace(secondaryCitizen.Value) )
                    profileSummary.SecondaryCountryCitizenShip = secondaryCitizen.Text;
            }


            return View("ProfileSummary", profileSummary);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileSummary"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomHandleErrorAttribute(ActionName = "ProfileSummary", ControllerName = "CustomerRegistration", ResultType = "redirect")]
        public ActionResult ProfileSummary(ProfileSummary profileSummary)
        {
            CustomerProspect customerProspect = GetCustomerProspectSession();

            if ( ModelState.IsValid )
            {
                sharedData.Prospect prospect = GetProspect(customerProspect);

                bool editMode = !customerProspect.IsNewCustomer;

                bool Isprofilective = profileSummary.Activate(( ( Hashtable )Session["HTSessions"] )["AgentSessionId"].ToString(), prospect, customerProspect.AlloyID.ToString(), editMode);
                Desktop service = new Desktop();
                try
                {
                    MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext()
                         {
                             EditMode = editMode
                         };
                    //Get SSO attributes from Cookie
                    mgiContext.Context = new Dictionary<string, object>();
                    mgiContext.Context.Add("SSOAttributes", GetSSOAgentSession("SSO_AGENT_SESSION"));
                    mgiContext.Context.Add("StatusCode", TempData["StatusCode"]);
                    TempData["StatusCode"] = string.Empty;
                    if ( !editMode )
                    {
                        mgiContext.Context.Add("CustomerLookUpPartnerAccountNumber", TempData["CustomerLookUpPartnerAccountNumber"]);
                        mgiContext.Context.Add("CustomerLookUpRelationshipAccountNumber", TempData["CustomerLookUpRelationshipAccountNumber"]);
                        mgiContext.Context.Add("CustomerLookUpBankId", TempData["CustomerLookUpBankId"]);
                        mgiContext.Context.Add("CustomerLookUpBranchId", TempData["CustomerLookUpBranchId"]);
                        mgiContext.Context.Add("CustomerLookUpProgramId", TempData["CustomerLookUpProgramId"]);

                        if ( TempData["FetchedFromCustomerLookUp"] != null && ( bool )TempData["FetchedFromCustomerLookUp"] == true )
                            mgiContext.Context.Add("FetchedFromCustomerLookUp", TempData["FetchedFromCustomerLookUp"]);

                        service.ClientActivate(( ( Hashtable )Session["HTSessions"] )["AgentSessionId"].ToString(), customerProspect.AlloyID, mgiContext);
                    }
                    else
                        service.UpdateCustomerToClient(( ( Hashtable )Session["HTSessions"] )["AgentSessionId"].ToString(), customerProspect.AlloyID, mgiContext);
                }
                catch ( Exception ex )
                {
                    string[] errMsg = ex.Message.Split('|');
                    if ( errMsg[1].Length >= 2 && errMsg[1].Contains("1011.2007") )
                    {
                        string[] errCode = errMsg[1].Split('.');
                        customerProspect.IsNewCustomer = false;
                        Session["CustomerProspect"] = customerProspect;

                        if ( errCode.Length >= 3 && errCode[2] == "700" )
                        {
                            TempData["StatusCode"] = errCode.Length >= 4 ? errCode[3] : string.Empty;
                        }
                        return RedirectToAction("ProfileSummary", "CustomerRegistration", new { IsException = true, ExceptionMsg = System.Web.HttpUtility.JavaScriptStringEncode(ex.Message) });
                    }

                    return RedirectToAction("CustomerSearch", "CustomerSearch", new { IsException = true, ExceptionMsg = System.Web.HttpUtility.JavaScriptStringEncode(ex.Message) });
                }
                bool isCompanionCard = customerProspect.PersonalInformation.IsCompanionSearch;

                if ( isCompanionCard )
                {
                    TempData["AddOnAlloyId"] = customerProspect.AlloyID;
                    return RedirectToAction("OrderAddOnCard", "VISAProductCredential");
                    //redirect to shopping cart
                }

                if ( Isprofilective )
                {
                    Session["NewUser"] = true;
                    Session["CustomerProspect"] = null;
                    Session["ChannelPartner"] = null;

                    Session.Remove("CurrentProspect");
                    if ( !editMode )
                    {
                        Session["CustomerSession"] = null;
                    }
                    TempData["profileActive"] = System.Configuration.ConfigurationManager.AppSettings.Get("LANDING_ACTIVATIONUSER_MESSAGE");

                    return RedirectToAction("ValidateCustomerStatusAndId", "CustomerSearch", new { id = customerProspect.AlloyID, CardSearchType.Other, calledFrom = "CustomerSummaryPage" });
                }
            }
            ModelState.AddModelError(string.Empty, "Please fill all required fields in customer registration flow.");
            return View("ProfileSummary", profileSummary);
        }

        #endregion


        #region Private methods
        private PersonalInformation GetProfileSyncProspect(sharedData.Prospect prospect)
        {
            long agentSessionId = GetAgentSessionId();
            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            Desktop client = new Desktop();
            PersonalInformation personalInfo = new PersonalInformation
            {
                personalDetail = new PersonalDetail
                {
                    FirstName = prospect.FName,
                    MiddleName = prospect.MName,
                    LastName = prospect.LName,
                    SecondLastName = prospect.LName2,
                    Gender = prospect.Gender,
                    ActualSSN = prospect.SSN,
                    SSN = ( !string.IsNullOrWhiteSpace(prospect.SSN) ) ? WebHelper.MaskSSNNumber(prospect.SSN) : string.Empty
                },
                contactDetail = new ContactDetail
                {
                    Address1 = prospect.Address1,
                    Address2 = prospect.Address2,
                    PrimaryPhone = prospect.Phone1,
                    PrimaryPhoneType = prospect.Phone1Type,
                    PrimaryPhoneProvider = prospect.Phone1Provider,
                    AlternativePhone = prospect.Phone2,
                    AlternativePhoneType = prospect.Phone2Type,
                    AlternativePhoneProvider = prospect.Phone2Provider,
                    Email = prospect.Email,
                    City = prospect.City,
                    State = prospect.State,
                    ZipCode = prospect.PostalCode,
                    MailingAddressDifferent = prospect.MailingAddressDifferent,
                    MailingAddress1 = prospect.MailingAddress1,
                    MailingAddress2 = prospect.MailingAddress2,
                    MailingCity = prospect.MailingCity,
                    MailingState = prospect.MailingState,
                    MailingZipCode = prospect.MailingZipCode,
                    LPrimaryPhonetype = client.PhoneType(agentSessionId, mgiContext),
                    LStates = client.USStates(agentSessionId, mgiContext),
                    LPrimaryPhoneProvider = client.PhoneProvider(agentSessionId, mgiContext),
                    LAlternatePhoneProvider = client.PhoneProvider(agentSessionId, mgiContext),
                    LAlternatePhonetype = client.PhoneType(agentSessionId, mgiContext)

                },
                Preference = new Preference
                {
                    DoNotCall = prospect.DoNotCall,
                    ReceiveTextMessage = prospect.TextMsgOptIn,
                    ReceiptLanguage = prospect.ReceiptLanguage,
                    CustomerProfileStatus = prospect.ProfileStatus,
                    LReceiptLanguage = client.GetRecieptLanguages(),
                    ClientProfileStatus = prospect.ClientProfileStatus
                },
                ReferralNumber = prospect.ReferralCode,
                WoodForestAccountHolder = prospect.IsAccountHolder,
                Notes = prospect.Notes
            };
            return personalInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ProfileSummary GetProfileSummary(long agentSessionId)
        {
            ProfileSummary profileSummary = new ProfileSummary();

            CustomerProspect customerProspect = GetCustomerProspectSession();

            MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            Desktop desktop = new Desktop();
            sharedData.Prospect prospect = GetProspect(customerProspect);
            profileSummary.Name = prospect.FName + " " + prospect.MName + " " + prospect.LName;
            profileSummary.Gender = prospect.Gender;
            profileSummary.PrimaryPhone = prospect.Phone1;
            profileSummary.Email = prospect.Email;
            profileSummary.Address = string.Format("{0}, {1}, {2}, {3},{4}", prospect.Address1, prospect.Address2, prospect.City, prospect.State, prospect.PostalCode);
            profileSummary.MailingAddress = string.Format("{0}, {1}, {2}, {3},{4}", prospect.MailingAddress1, prospect.MailingAddress2, prospect.MailingCity, prospect.MailingState, prospect.MailingZipCode);
            profileSummary.Notes = prospect.Notes;//AL-184

            profileSummary.CustomerProfileStatus = prospect.ProfileStatus;
            profileSummary.Profession = prospect.Occupation;
            profileSummary.EmployerName = prospect.Employer;
            profileSummary.EmployerPhoneNumber = prospect.EmployerPhone;
            profileSummary.Group1 = prospect.Groups.FirstOrDefault();
            profileSummary.Group2 = prospect.Groups.Count > 1 ? prospect.Groups.LastOrDefault() : null;
            profileSummary.CustomerProfileStatus = prospect.ProfileStatus;

            profileSummary.SSN = GetSSNNumber(prospect.SSN);

            if ( prospect.ID != null )
            {
                profileSummary.MotherMaidenName = prospect.MoMaName;

                profileSummary.DateOfBirth = prospect.DateOfBirth != null ? ( prospect.DateOfBirth == DateTime.MinValue ? string.Empty : prospect.DateOfBirth.Value.ToShortDateString() ) : null;
                profileSummary.Country = prospect.ID.Country;
                profileSummary.GovtIdIssueState = prospect.ID.State;
                profileSummary.GovtIDType = prospect.ID.IDType;
                profileSummary.IDExpirationDate = prospect.ID.ExpirationDate == DateTime.MinValue ? string.Empty : prospect.ID.ExpirationDate.ToString();

                profileSummary.IDIssueDate = prospect.ID.IssueDate != null ? ( prospect.ID.IssueDate == DateTime.MinValue ? string.Empty : prospect.ID.IssueDate.Value.ToShortDateString() ) : null;
                profileSummary.GovernmentId = prospect.ID.GovernmentId;
                if ( !string.IsNullOrEmpty(prospect.ID.CountryOfBirth) )
                    profileSummary.CountryOfBirth = desktop.GetCountryOfBirth(agentSessionId, prospect.ID.CountryOfBirth, mgiContext);

                profileSummary.ClientID = prospect.ClientID;
                profileSummary.LegalCode = prospect.LegalCode;
                profileSummary.PrimaryCountryCitizenShip = prospect.PrimaryCountryCitizenShip;
                profileSummary.SecondaryCountryCitizenShip = prospect.SecondaryCountryCitizenShip;
            }

            if ( customerProspect.EmploymentDetails != null )
            {
                EmploymentDetails employmentDetail = customerProspect.EmploymentDetails;
                profileSummary.Profession = employmentDetail.Profession;
                profileSummary.OccupationDescription = employmentDetail.OccupationDescription;
                profileSummary.EmployerName = employmentDetail.EmployerName;
                profileSummary.EmployerPhoneNumber = employmentDetail.EmployerPhoneNumber;
            }

            if ( customerProspect.IdentificationInformation != null )
            {
                IdentificationInformation identificationInfo = customerProspect.IdentificationInformation;

                profileSummary.MotherMaidenName = identificationInfo.MotherMaidenName;
                if ( !string.IsNullOrEmpty(identificationInfo.DateOfBirth) )
                    profileSummary.DateOfBirth = Convert.ToDateTime(identificationInfo.DateOfBirth).ToShortDateString() == DateTime.MinValue.ToShortDateString() ? string.Empty : Convert.ToDateTime(identificationInfo.DateOfBirth).ToShortDateString();

                profileSummary.Country = identificationInfo.Country;
                profileSummary.GovtIDType = HttpUtility.UrlDecode(identificationInfo.GovtIDType);
                profileSummary.GovtIdIssueState = identificationInfo.GovtIdIssueState == "Select" ? String.Empty : identificationInfo.GovtIdIssueState;
                profileSummary.GovernmentId = identificationInfo.GovernmentId;
                if ( !string.IsNullOrEmpty(identificationInfo.IDIssuedDate) )
                    profileSummary.IDIssueDate = Convert.ToDateTime(identificationInfo.IDIssuedDate).ToShortDateString() == DateTime.MinValue.ToShortDateString() ? string.Empty : Convert.ToDateTime(identificationInfo.IDIssuedDate).ToShortDateString();

                if ( !string.IsNullOrEmpty(identificationInfo.IDExpireDate) )
                    profileSummary.IDExpirationDate = Convert.ToDateTime(identificationInfo.IDExpireDate).ToShortDateString() == DateTime.MinValue.ToShortDateString() ? string.Empty : Convert.ToDateTime(identificationInfo.IDExpireDate).ToShortDateString();
                profileSummary.MGIAlloyID = identificationInfo.MGIAlloyID;
            }

            if ( customerProspect.PinDetails != null )
            {
                PinDetails pin = customerProspect.PinDetails;
                profileSummary.Pin = pin.Pin;
            }

            return profileSummary;
        }

        private CustomerProspect GetCustomerProspectSession()
        {
            if ( Session["CustomerProspect"] != null )
            {
                return ( CustomerProspect )Session["CustomerProspect"];
            }
            return new CustomerProspect()
            {
                IsNewCustomer = true,
                PersonalInformation = new PersonalInformation()
                {
                    personalDetail = new PersonalDetail(),
                    contactDetail = new ContactDetail(),
                    Preference = new Preference() { CustomerProfileStatus = ProfileStatus.Active }
                },
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

            if ( !string.IsNullOrWhiteSpace(SSN) )
            {
                subResult = SSN.Substring(SSN.Length - Math.Min(4, SSN.Length));
                result = maskedValue + subResult;
            }
            return result;
        }

        private bool IsCustomerClosed(CustomerProspect customerProspect)
        {
            bool status = false;
            if ( ( customerProspect.PersonalInformation.Preference.CustomerProfileStatus ) == ProfileStatus.Closed && !customerProspect.IsNewCustomer )
            {
                status = true;
            }
            return status;
        }

        #endregion
    }
}
