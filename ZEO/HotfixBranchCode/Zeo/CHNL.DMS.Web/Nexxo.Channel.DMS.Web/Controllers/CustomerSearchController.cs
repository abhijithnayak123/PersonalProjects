using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using System.Linq.Expressions;
using System.Reflection;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using System.Configuration;

using System.Collections;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using MGI.Common.Sys;
using System.ServiceModel;
using MGI.Channel.DMS.Web.Common;
using MGI.Security.Voltage;
using System.Diagnostics;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class CustomerSearchController : BaseController
	{
		public NLoggerCommon NLogger = new NLoggerCommon();


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CustomerSearch")]
		public ActionResult CustomerSearch(string cardNumber, bool IsException = false, string ExceptionMsg = "", bool CounterIdAssigned = false)
		{
			NLogHelper.Info("CustomerSearch:");
			long agentSessionId = GetAgentSessionId();
			if (CounterIdAssigned)
			{
				UpdateCounterId(CounterIdAssigned);
				UpdateCashInStatus();

			}
			Session["activeButton"] = "home";
			Session["criteriaTag"] = false;
			Session["CustomerSession"] = null;
			Session["CustomerProspect"] = null;
			Session["ClosedCustomerProfile"] = null;
			Session["isCashierAgree"] = "false";
			Session["CardBalance"] = null;//AL-324
			//This is to ensure that tempdata is cleared once teller clicks on Cancel button 
			//from the Customer Look up screen even after displaying search results. 
			TempData["FetchedFromCustomerLookUp"] = null;
			TempData["CustomerLookUpPartnerAccountNumber"] = null;
			TempData["CustomerLookUpRelationshipAccountNumber"] = null;
			TempData["CustomerLookUpBankId"] = null;
			TempData["CustomerLookUpBranchId"] = null;

			CustomerSearch customerSearch = new CustomerSearch();
			if (cardNumber != null)
			{
				customerSearch.CardNumber = cardNumber.Substring(0, 8);
			}
			NLogger.Debug("CustomerSearch: Card Number {0}", cardNumber);

			int expDays = Convert.ToInt16(((Hashtable)Session["HTSessions"])["ExpDays"]);
			int minExpDays = Convert.ToInt16(ConfigurationManager.AppSettings["MIN_PWDCHANGE_DAYS"].ToString());
			if (minExpDays >= expDays)
				ViewBag.PasswordExpMessage = "Your Password is about to expire in next " + expDays + " days.";

			if (Session["IsTerminalSetup"] == null)
			{
				TerminalIdentifier.IdentifyTerminal(agentSessionId);
				TempData["IsChooseLocation"] = TerminalIdentifier.IsTerminalAvailableForHostName(agentSessionId);
				//throw new Exception("DMS is not setup correctly and Transactions are currently disabled. Please contact the System Administrator to help solve the problem.");
			}
			ViewBag.IsException = IsException;
			ViewBag.ExceptionMsg = ExceptionMsg;
			ViewBag.CVV = "000";
			NLogHelper.Info(string.Format("CustomerSearch : IsException {0} ExceptionMsg {1}", IsException, ExceptionMsg));
			return View("CustomerSearch", customerSearch);
		}

		public ActionResult CheckShoppingCartStatus()
		{
			Desktop desktop = new Desktop();

			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			string shoppingCartStatus = string.Empty;
			bool success = false;

			if (customerSession != null)
			{
				ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);
				ShoppingCartDetail shoppingCartDetail = ShoppingCartHelper.ShoppingCartDetailed(shoppingCart);
				//Not Considering Cash In Transactions.
				shoppingCartStatus = (shoppingCartDetail.Items.Where(x => x.Product != ProductType.CashIn.ToString()).Count() > 0) ? "nonempty" : "empty";
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
			if (ModelState.IsValid)
			{
				Session["FirstName"] = customerCriteria.FirstName;
				Session["LastName"] = customerCriteria.LastName;
				Session["PhoneNumber"] = customerCriteria.PhoneNumber == null ? customerCriteria.PhoneNumber : customerCriteria.PhoneNumber.Replace("-", "");
				Session["DateOfBirth"] = customerCriteria.DateOfBirth;
				Session["CardNumber"] = null;
				Session["IsIncludeClosed"] = customerCriteria.IsIncludeClosed;//AL-377
				if (!string.IsNullOrEmpty(customerCriteria.CardNumber))
				{
					//string maskedNumber = customerCriteria.CardNumber.Substring(customerCriteria.CardNumber.Length - 4);
					//string decryptnum = "**** **** ****" + maskedNumber;
					SecureData secure = new SecureData(NLogger.Logger);
					//string decryptmask = secure.Mask(customerCriteria.CardNumber);

					//LogHelper.WriteDebugLog("Before decrypting : " + customerCriteria.CardNumber);
					//LogHelper.WriteDebugLog("Before decrypting : " + decryptmask);

					//SecureData secure = new SecureData(Nlogger.Logger);
					string decryptedCardNumber = secure.Decrypt(customerCriteria.CardNumber, customerCriteria.CVV);
					NLogHelper.Debug("After decrypting:{0}", decryptedCardNumber);
					if (!string.IsNullOrEmpty(decryptedCardNumber))
					{
						Session["CardNumber"] = decryptedCardNumber;
					}
					else
					{
						throw new Exception("Please enter a valid card number.");
					}
				}
				//Session["CardNumber"] = customerCriteria.CardNumber == null ? customerCriteria.CardNumber : customerCriteria.CardNumber.Replace(" ", "");
				Session["GovernmentId"] = customerCriteria.GovernmentId;
				Session["SSN"] = customerCriteria.SSN == null ? customerCriteria.SSN : customerCriteria.SSN.Replace("-", "");
				Session["criteriaTag"] = true;

				return View("CustomerSearch", customerCriteria);
			}
			Session["criteriaTag"] = false;
			return View("CustomerSearch", customerCriteria);
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

		[CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CustomerSearch")]
		public ActionResult InitiateAnonymousCustomerSession()
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop client = new Desktop();
			long channelPartnerId = client.GetChannelPartner(Session["ChannelPartnerName"].ToString(), mgiContext).Id;
			long alloyId = client.GetAnonymousUserPAN(GetAgentSessionId(), channelPartnerId, mgiContext);

			NLogger.Info("InitiateAnonymousCustomerSession: channelPartnerId {0},AlloyID {1} ", channelPartnerId, alloyId);
			if (alloyId > 0)
				return RedirectToAction("InitiateCustomerSession", "CustomerSearch", new { id = alloyId.ToString(), searchType = CardSearchType.Other });
			else
				return View("CustomerSearch");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CustomerSearch")]
		public ActionResult InitiateCustomerSession(string id = null, CardSearchType searchType = CardSearchType.Other, string calledFrom = "")
		{
			int cardPresentedType = (int)searchType;
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();

			CustomerProspect customerProspect = GetCustomerProspectSession();
			Desktop client = new Desktop();
			if (Convert.ToBoolean(Session["IsShoppingCartExists"]))
				Session.Remove("IsShoppingCartExists");

			if (!string.IsNullOrEmpty((string)Session["CardNo"]))
				Session.Remove("CardNo");

			if (!string.IsNullOrEmpty((string)Session["CardNumber"]))
			{
				cardPresentedType = 2;
				Session["CardNumber"] = null;
			}

			//Get SSO attributes from Cookie
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.Context.Add("SSOAttributes", GetSSOAgentSession("SSO_AGENT_SESSION"));

			mgiContext.EditMode = true;

			if (calledFrom != "CustomerSummaryPage")
			{
				client.CustomerSyncInFromClient(Session["sessionId"].ToString(), customerProspect.AlloyID, mgiContext);
			}

			//US1458 - start
			CustomerSession customerSession = GetCustomerSession();
			if (customerSession == null)
			{
				customerSession = client.InitiateCustomerSession(Session["sessionId"].ToString(), customerProspect.AlloyID, cardPresentedType, mgiContext);
				client.RecordIdentificationConfirmation(Session["sessionId"].ToString(), customerSession.CustomerSessionId, true, mgiContext);
			}
			else
			{
				customerSession.Customer = client.GetCustomer(customerSession.CustomerSessionId, customerProspect.AlloyID, mgiContext);
			}
			Session["CustomerSession"] = customerSession;
			Session["IsGPRCard"] = customerSession.Customer.Fund.IsGPRCard;

			//AL-384
			if (customerProspect != null && customerProspect.PersonalInformation != null && customerProspect.PersonalInformation.Preference != null)
			{
				Session["ClosedCustomerProfile"] = (customerProspect.PersonalInformation.Preference.CustomerProfileStatus == ProfileStatus.Closed) ? "true" : string.Empty;
			}
			return RedirectToAction("ProductInformation", "Product");
		}

		public ActionResult ValidateCustomerStatusAndId(string id, string cardPresentedType, string calledFrom = "")
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop client = new Desktop();
			long alloyId = Convert.ToInt64(id);
			long agentSessionId = GetAgentSessionId();
			Prospect prospect = client.GetCustomerProfile(Session["sessionId"].ToString(), alloyId, mgiContext);
			if (prospect.ID == null)
				prospect.ID = new Identification();

			if (prospect.ID == null)
				prospect.ID = new Identification();
			CustomerProspect customerProspect = GetCustomerProspect(agentSessionId, prospect);
			customerProspect.AlloyID = Convert.ToInt64(id);
			customerProspect.IsNewCustomer = false;
			Session["CustomerProspect"] = customerProspect;
			//Nlogger.SetContext(HttpContext.Session.SessionID ,null);
			//AL-384
			ProfileStatus profileStatus = new ProfileStatus();
			profileStatus = prospect.ProfileStatus;
			Session["ClosedCustomerProfile"] = (profileStatus == ProfileStatus.Closed) ? "true" : string.Empty;

			try
			{
				client.ValidateCustomerStatus(Session["sessionId"].ToString(), alloyId, mgiContext);
			}
			catch (Exception ex)
			{
				if (calledFrom == "CustomerSummaryPage")
				{
					NLogHelper.Error("CustomerSummaryPage", ex.Message);
					return RedirectToAction("CustomerSearch", "CustomerSearch", new { IsException = true, ExceptionMsg = System.Web.HttpUtility.JavaScriptStringEncode(ex.Message) });
				}
				else
				{
					NLogHelper.Error("CustomerRegistration", ex.Message);
					return RedirectToAction("ProfileSummary", "CustomerRegistration", new { IsException = true, ExceptionMsg = System.Web.HttpUtility.JavaScriptStringEncode(ex.Message) });
				}
			}

			if (calledFrom == "CustomerSummaryPage")
			{
				if (IsCustomerIdExpiring(prospect) && profileStatus != ProfileStatus.Closed && (int)Session["UserRoleId"] != (int)UserRoles.Tech)
				{
					Session["activeButton"] = "newcustomer";
					return RedirectToAction("IdentificationInformation", "CustomerRegistration", new { isExpiring = true });
				}
				return RedirectToAction("InitiateCustomerSession", "CustomerSearch", new { calledFrom = "CustomerSummaryPage" });
			}
			if (IsCustomerIdExpired(prospect) && profileStatus != ProfileStatus.Closed && (int)Session["UserRoleId"] != (int)UserRoles.Tech)
			{
				Session["activeButton"] = "newcustomer";
				TempData["IsExpired"] = true;
				return RedirectToAction("IdentificationInformation", "CustomerRegistration");
			}
			if (IsCustomerIdExpiring(prospect) && profileStatus != ProfileStatus.Closed && (int)Session["UserRoleId"] != (int)UserRoles.Tech)
			{
				Session["activeButton"] = "newcustomer";
				return RedirectToAction("IdentificationInformation", "CustomerRegistration", new { isExpiring = true });
			}
			else
			{
				CustomerSearch customerSearch = new CustomerSearch();
				customerSearch.showIdConfirmedPopUp = "true";
				customerSearch.FirstName = Session["FirstName"] == null ? default(string) : NexxoUtil.SafeSQLString(Session["FirstName"].ToString(), true);
				customerSearch.LastName = Session["LastName"] == null ? default(string) : NexxoUtil.SafeSQLString(Session["LastName"].ToString(), true);
				customerSearch.PhoneNumber = Session["PhoneNumber"] == null ? default(string) : NexxoUtil.SafeSQLString(Session["PhoneNumber"].ToString(), true);
				customerSearch.DateOfBirth = Session["DateOfBirth"] == null ? string.Empty : Session["DateOfBirth"].ToString();
				customerSearch.CardNumber = string.Empty;
				customerSearch.GovernmentId = Session["GovernmentId"] == null ? default(string) : NexxoUtil.SafeSQLString(Session["GovernmentId"].ToString(), true);
				customerSearch.SSN = Session["SSN"] == null ? default(string) : NexxoUtil.SafeSQLString(Session["SSN"].ToString(), true);

				return View("CustomerSearch", customerSearch);
			}
		}

		public ActionResult SearchCustomerFromSwipe(string CardNumber, string CVV)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			CustomerSearchResult customers;
			Desktop client = new Desktop();
			try
			{
				Session["activeButton"] = "swipecard";
				CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria();
				searchCriteria.CardNumber = CardNumber == null ? CardNumber : CardNumber.Replace(" ", string.Empty);


				if (!string.IsNullOrEmpty(CardNumber) && !string.IsNullOrEmpty(CVV))
				{
					SecureData secure = new SecureData(NLogger.Logger);
					searchCriteria.CardNumber = secure.Decrypt(CardNumber, CVV);
				}
				customers = client.SearchCustomers(Session["sessionId"].ToString(), searchCriteria, mgiContext).FirstOrDefault();
				NLogHelper.Debug("SearchCustomerFromSwipe: Cardnumber {0}", CardNumber);
			}
			catch (Exception ex)
			{
				CustomerSearch customerSearch = new CustomerSearch();
				ViewBag.SwipeCardErrorMessage = ex.Message;
				NLogHelper.Error("SearchCustomerFromSwipe: Exception {0}", ex.Message);

				return View("CustomerSearch", customerSearch);
			}

			if (customers != null)
			{
				long agentSessionId = GetAgentSessionId();
				Prospect prospect = client.GetCustomerProfile(Session["sessionId"].ToString(), Convert.ToInt64(customers.AlloyID), mgiContext);
				CustomerProspect customerProspect = GetCustomerProspect(agentSessionId, prospect);
				Session["CustomerProspect"] = customerProspect;
				return RedirectToAction("InitiateCustomerSession", new { id = customers.AlloyID, searchType = CardSearchType.Swipe });
			}
			else
			{
				CustomerSearch customerSearch = new CustomerSearch();
				ViewBag.ErrorMessage = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.InvalidCardErrorMessage;
				return View("CustomerSearch", customerSearch);
			}
		}

		private bool IsCustomerIdExpiring(MGI.Channel.Shared.Server.Data.Prospect prospect)
		{
			// the ID expiration date is within 14 days or the Govt Id is empty
			if (prospect.ID.ExpirationDate != DateTime.MinValue && prospect.ID.ExpirationDate <= DateTime.Now.AddDays(14))
				return true;
			else  // if the ID is valid
				return false;
		}
		private bool IsCustomerIdExpired(MGI.Channel.Shared.Server.Data.Prospect prospect)
		{
			// the ID is Expired or the Govt Id is empty
			if (prospect.ID.ExpirationDate != DateTime.MinValue && prospect.ID.ExpirationDate < DateTime.Now.Date)

				return true;
			else  // if the ID is valid
				return false;
		}
		public ActionResult ConfirmIdentification()
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop client = new Desktop();
			CustomerProspect customerProspect = GetCustomerProspectSession();
			client.RecordIdentificationConfirmation(Session["sessionId"].ToString(), customerProspect.AlloyID.ToString(), true, mgiContext);
			ViewBag.Navigation = Resources.NexxoSiteMap.ProductInformation;
			Session["ConfirmIdentified"] = true;
			return RedirectToAction("ProductInformation", "Product");

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult ModifyCustomer(string id)
		{
			try
			{
				Desktop client = new Desktop();
				long agentSessionId = GetAgentSessionId();
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				Prospect prospect = client.GetCustomerProfile(Session["sessionId"].ToString(), Convert.ToInt64(id), mgiContext);
				CustomerProspect customerProspect = GetCustomerProspect(agentSessionId, prospect);
				customerProspect.IdentificationInformation.MGIAlloyID = id;
				Session["CustomerProspect"] = customerProspect;
				//AL-546 when user Role is Tech redirecting to profile summary
				//TODO ExceptionChannelPartners needs to rename, Ex -- ChannelParnters
				if ((int)Session["UserRoleId"] == (int)UserRoles.Tech || ((int)Session["UserRoleId"] == (int)UserRoles.ComplianceManager))
				{
					//AL-384 when customer profile status closed redirecting to profile summary				
					if (prospect.ProfileStatus == ProfileStatus.Closed)
					{
						Session["ClosedCustomerProfile"] = "true";
					}
					return RedirectToAction("ProfileSummary", "CustomerRegistration");
				}

				Session["EditProspect"] = true;
			}
			catch (Exception ex)
			{
				NLogHelper.Error("ModifyCustomer:", ex);
				ViewBag.ExceptionMessage = ex.Message;
			}

			return RedirectToAction("PersonalInformation", "CustomerRegistration");
		}

		private CustomerProspect GetCustomerProspect(long agentSessionId, Prospect prospect)
		{

			try
			{
				Desktop client = new Desktop();
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				CustomerProspect customerProspect = new CustomerProspect()
				{
					IsNewCustomer = false,
					AlloyID = Convert.ToInt64(prospect.PartnerAccountNumber),
					PersonalInformation = new Models.PersonalInformation()
					{
						personalDetail = new PersonalDetail()
						{
							ActualSSN = prospect.SSN,
							SSN = WebHelper.MaskSSNNumber(prospect.SSN),
							FirstName = prospect.FName,
							Gender = prospect.Gender,
							LastName = prospect.LName,
							MiddleName = prospect.MName,
							SecondLastName = prospect.LName2
						},
						contactDetail = new ContactDetail()
						{
							Address1 = prospect.Address1,
							Address2 = prospect.Address2,
							AlternativePhone = prospect.Phone2,
							AlternativePhoneProvider = prospect.Phone2Provider,
							AlternativePhoneType = prospect.Phone2Type,
							City = prospect.City,
							Email = prospect.Email,
							MailingAddress1 = prospect.MailingAddress1,
							MailingAddress2 = prospect.MailingAddress2,
							MailingAddressDifferent = prospect.MailingAddressDifferent,
							MailingCity = prospect.MailingCity,
							MailingState = prospect.MailingState,
							MailingZipCode = prospect.MailingZipCode,
							PrimaryPhone = prospect.Phone1,
							PrimaryPhoneProvider = prospect.Phone1Provider,
							PrimaryPhoneType = prospect.Phone1Type,
							State = prospect.State,
							ZipCode = prospect.PostalCode,
							LStates = client.USStates(agentSessionId, mgiContext),
							LPrimaryPhonetype = client.PhoneType(agentSessionId, mgiContext),
							LPrimaryPhoneProvider = client.PhoneProvider(agentSessionId, mgiContext),
							LAlternatePhonetype = client.PhoneType(agentSessionId, mgiContext),
							LAlternatePhoneProvider = client.PhoneProvider(agentSessionId, mgiContext)
						},
						Preference = new Preference()
						{
							ClientProfileStatus = prospect.ClientProfileStatus,
							DoNotCall = prospect.DoNotCall,
							ReceiptLanguage = prospect.ReceiptLanguage,
							ReceiveTextMessage = prospect.TextMsgOptIn,
							CustomerProfileStatus = prospect.ProfileStatus
						},

						Group1 = (prospect.Groups != null && prospect.Groups.Count > 0) ? prospect.Groups[0] : string.Empty,
						Group2 = (prospect.Groups != null && prospect.Groups.Count > 1) ? prospect.Groups[1] : string.Empty,
						Notes = prospect.Notes,
						ReferralNumber = prospect.ReferralCode,
						WoodForestAccountHolder = prospect.IsAccountHolder,//this field is related to partnerCustomer IsAccountHolder
					},
					IdentificationInformation = new IdentificationInformation()
					{

						Country = prospect.ID == null ? string.Empty : prospect.ID.Country,
						CountryOfBirth = prospect.ID == null ? string.Empty : prospect.ID.CountryOfBirth,
						DateOfBirth = Convert.ToString(prospect.DateOfBirth),
						GovtIdIssueState = prospect.ID == null ? string.Empty : prospect.ID.State,
						GovtIDType = prospect.ID == null ? string.Empty : prospect.ID.IDType,
						IDExpireDate = prospect.ID == null ? string.Empty : Convert.ToString(prospect.ID.ExpirationDate),
						IDIssuedDate = prospect.ID == null ? string.Empty : Convert.ToString(prospect.ID.IssueDate),
						GovernmentId = prospect.ID == null ? string.Empty : prospect.ID.GovernmentId,
						MotherMaidenName = prospect.MoMaName,
						ClientID = prospect.ClientID,
						LegalCode = prospect.LegalCode,
						PrimaryCountryCitizenShip = prospect.PrimaryCountryCitizenShip,
						SecondaryCountryCitizenShip = prospect.SecondaryCountryCitizenShip
					},
					EmploymentDetails = new EmploymentDetails()
					{
						EmployerName = prospect.Employer,
						EmployerPhoneNumber = prospect.EmployerPhone,
						Profession = prospect.Occupation,
						OccupationDescription = prospect.OccupationDescription
					},
					PinDetails = new PinDetails()
					{
						PhoneNumber = prospect.Phone1,
						Pin = prospect.PIN,
						ReEnter = prospect.PIN
					},
					ProfileSummary = new ProfileSummary()
					{
					}
				};
				return customerProspect;
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ActionResult CustomerProfile(string customerPAN)
		{
			Desktop client = new Desktop();
			ProfileSummary profile = new ProfileSummary();
			CustomerSession customerSession = new CustomerSession();
			// Nlogger.SetContext( HttpContext.Session.SessionID, null);
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			if (Session["CardNo"] != null)
				Session.Remove("CardNo");

			customerSession = client.InitiateCustomerSession(Session["sessionId"].ToString(), Convert.ToInt64(customerPAN), (int)CardSearchType.Other, mgiContext);
			string state = string.Empty;
			if (!string.IsNullOrWhiteSpace(customerSession.Customer.ID.State))
			{
				state = customerSession.Customer.ID.State;
				state = string.Compare(state, "select", true) == 0 ? string.Empty : state;
			}

			profile.Name = customerSession.Customer.PersonalInformation.FName + " " + customerSession.Customer.PersonalInformation.MName + " " + customerSession.Customer.PersonalInformation.LName;
			profile.Address = string.Format("{0}, {1}, {2}, {3}, {4}", customerSession.Customer.Address.Address1, customerSession.Customer.Address.Address2, customerSession.Customer.Address.City, customerSession.Customer.Address.State, customerSession.Customer.Address.PostalCode);
			profile.MailingAddress = string.Format("{0}, {1}, {2}, {3}, {4}", customerSession.Customer.MailingAddress.Address1, customerSession.Customer.MailingAddress.Address2, customerSession.Customer.MailingAddress.City, customerSession.Customer.MailingAddress.State, customerSession.Customer.MailingAddress.PostalCode);
			profile.DateOfBirth = customerSession.Customer.PersonalInformation.DateOfBirth.ToShortDateString();
			profile.PrimaryPhone = customerSession.Customer.Phone1.Number;
			profile.Email = customerSession.Customer.Email;
			profile.Profession = customerSession.Customer.Employment.Occupation;
			profile.EmployerName = customerSession.Customer.Employment.Employer;
			profile.EmployerPhoneNumber = customerSession.Customer.Employment.EmployerPhone;
			profile.Gender = customerSession.Customer.PersonalInformation.Gender;
			profile.GovtIdIssueState = state;
			profile.IDIssueDate = (customerSession.Customer.ID.IssueDate != null && customerSession.Customer.ID.IssueDate != DateTime.MinValue) ? customerSession.Customer.ID.IssueDate.Value.ToShortDateString() : string.Empty;
			profile.IDExpirationDate = customerSession.Customer.ID.ExpirationDate.ToString() == DateTime.MinValue.ToString() ? string.Empty : customerSession.Customer.ID.ExpirationDate.ToString(); // Namit
			profile.GovernmentId = customerSession.Customer.ID.GovernmentId;
			profile.MotherMaidenName = customerSession.Customer.PersonalInformation.MothersMaidenName;
			profile.GovtIDType = customerSession.Customer.ID.IDType;
			profile.Country = customerSession.Customer.ID.Country;
			profile.customerSession = customerSession;

			NLogHelper.Debug("CustomerProfile:profileName {0}", profile.Name);
			return PartialView("_ProfileSummaryPopUp", profile);

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
		//[CustomHandleErrorAttribute(ViewName = "CustomerSearch", MasterName = "_Menu")]
		public ActionResult CustomerSearchGrid(string sidx, string sord, int page = 1, int rows = 5)
		{
			CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			// Used NexxoUtil.SafeSQLString() to remove invalid words from Inputs for SQL Injection. US#1788.
			searchCriteria.FirstName = Session["FirstName"] == null ? default(string) : NexxoUtil.SafeSQLString(Session["FirstName"].ToString(), true);
			searchCriteria.LastName = Session["LastName"] == null ? default(string) : NexxoUtil.SafeSQLString(Session["LastName"].ToString(), true);
			searchCriteria.PhoneNumber = Session["PhoneNumber"] == null ? default(string) : NexxoUtil.SafeSQLString(Session["PhoneNumber"].ToString(), true);
			searchCriteria.DateOfBirth = Session["DateOfBirth"] == null ? DateTime.MinValue : Convert.ToDateTime(Session["DateOfBirth"].ToString());
			searchCriteria.CardNumber = Session["CardNumber"] == null ? default(string) : NexxoUtil.SafeSQLString(Session["CardNumber"].ToString(), true);
			searchCriteria.GovernmentId = Session["GovernmentId"] == null ? default(string) : NexxoUtil.SafeSQLString(Session["GovernmentId"].ToString(), true);
			searchCriteria.SSN = Session["SSN"] == null ? default(string) : NexxoUtil.SafeSQLString(Session["SSN"].ToString(), true);
			searchCriteria.IsIncludeClosed = Convert.ToBoolean(Session["IsIncludeClosed"].ToString());

			Desktop client = new Desktop();
			var customers = client.SearchCustomers(Session["sessionId"].ToString(), searchCriteria, mgiContext).AsQueryable();

			var totalRecords = customers.Count();
			var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

			var data = (from s in customers
						select new
						{
							id = s.AlloyID,
							cell = new object[] { s.FullName, s.PhoneNumber ?? string.Empty, s.DateOfBirth != null ? s.DateOfBirth.Value.ToShortDateString() : "", s.CardNumber ?? string.Empty, s.GovernmentId ?? string.Empty, s.ProfileStatus }
						}).ToArray();

			var jsonData = new
			{
				display = true,
				total = totalPages,
				page = page,
				records = totalRecords,
				rows = data.Skip((page - 1) * rows).Take(rows)
			};
			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		//TODO : Need to remove
		//public ActionResult GetCardBalance() // where this method called ?
		//{
		//	return RedirectToAction("CustomerSearch", "CustomerSearch");
		//}

		public ActionResult RecordIdentificationConfirmation(string id)
		{
			ProfileSummary profile = new ProfileSummary();
			try
			{
				CustomerSession customerSession = new CustomerSession();
				if (Session["CustomerSession"] == null)
				{
					Desktop client = new Desktop();
					// client.RecordIdentificationConfirmation(Session["sessionId"].ToString(), id, true);
				}
			}
			catch (Exception ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
			}

			return View("ProductInformation", new ProductInfo());
		}
		public ActionResult ShowSwipeMessage()
		{
			return PartialView("_SwipeCard");
		}

		public ActionResult ShowIDConfirmationMessage()
		{
			return PartialView("_SwipeCardIDConfirm");
		}

		private CustomerProspect GetCustomerProspectSession()
		{
			if (Session["CustomerProspect"] != null)
			{
				return (CustomerProspect)Session["CustomerProspect"];
			}
			return new CustomerProspect() { IsNewCustomer = true };
		}

		private CustomerSession GetCustomerSession()
		{
			return Session["CustomerSession"] as CustomerSession;
		}

		public ActionResult ClearCustomerSession()
		{
			Session["CustomerProspect"] = null;

			var jsonData = new
			{
				success = true
			};

			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult UpdateCounterId(bool counterIdAssigned)
		{
			if (counterIdAssigned)
			{
				Desktop desktop = new Desktop();
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				mgiContext.IsAvailable = true;

				if (Session["CustomerSession"] != null)
				{
					CustomerSession _customerSession = (CustomerSession)Session["CustomerSession"];
					desktop.UpdateCounterId(Convert.ToInt64(_customerSession.CustomerSessionId), mgiContext);
				}

			}
			return null;
		}

		//AL-2729 Changes - On Home Click - End Customer Session - Remove Cash In Transaction
		private void UpdateCashInStatus()
		{


			if (Session["CustomerSession"] != null)
			{
				Desktop desktop = new Desktop();

				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

				ShoppingCart shoppingCart = desktop.ShoppingCart(customerSession.CustomerSessionId);

				if (shoppingCart != null && shoppingCart.Id != 0 && shoppingCart.Cash.Count > 0)
				{
					foreach (var item in shoppingCart.Cash.Where(x => x.CashType == CashTransactionType.CashIn.ToString() && x.Status == Constants.STATUS_AUTHORIZED))
					{
						try
						{
							desktop.RemoveCashIn(long.Parse(customerSession.CustomerSessionId), long.Parse(item.Id));
						}
						catch { }
					}
				}
			}
		}
	}
}
