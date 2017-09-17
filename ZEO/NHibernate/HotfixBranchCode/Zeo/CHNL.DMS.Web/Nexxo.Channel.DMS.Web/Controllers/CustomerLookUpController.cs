using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.Common;

using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Server.Data;
using MGI.Security.Voltage;
using System.Diagnostics;
using Microsoft.Security.Application;

namespace MGI.Channel.DMS.Web.Controllers
{

	public class CustomerLookUpController : BaseController
	{
		public NLoggerCommon NLogger { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[CustomHandleErrorAttribute(ViewName = "CustomerLookUp", MasterName = "_menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CustomerLookUp")]
		public ActionResult CustomerLookUp(bool IsException = false, string ExceptionMsg = "", bool IsPrimaryCardHolder = false)
		{
			Session["activeButton"] = "newcustomer";
			Session["EditProspect"] = false;
			Session["CustomerProspect"] = null;
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

			lookUpInfo.CustomerMinimumAgeMessage = string.Format(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.DateOfBirthConfigureMessage.ToString(), lookUpInfo.channelPartner.CustomerMinimumAge);

			lookUpInfo.IsCompanionSearch = IsPrimaryCardHolder;

			ViewBag.IsException = IsException;
			ViewBag.ExceptionMsg = ExceptionMsg;
			NLogHelper.Info("CustomerLookUpController :");
			NLogHelper.Error("CustomerLookup : IsException {0} , ExceptionMsg{1}", IsException, ExceptionMsg);
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
			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();
			Dictionary<string, object> prevLookUpCriteria = new Dictionary<string, object>();
			bool lookUpCriteriaSameAsPrev = true;
			Prospect[] customers;
			NLogHelper.Info("CustomerSearchGrid With Lookup");

			if (!string.IsNullOrEmpty(lookUpCriteria.CardNumber) && !string.IsNullOrEmpty(lookUpCriteria.CVV))
			{
				SecureData secure = new SecureData();
				string decryptedCardNumber = secure.Decrypt(lookUpCriteria.CardNumber, lookUpCriteria.CVV);
				lookUpCriteria.CardNumber = decryptedCardNumber;
			}

			if (!string.IsNullOrEmpty(lookUpCriteria.SSN)) customerLookUpCriteria.Add("SSN", NexxoUtil.SafeSQLString(lookUpCriteria.SSN.Replace("-", ""), true));
			if (!string.IsNullOrEmpty(lookUpCriteria.PrimaryPhone)) customerLookUpCriteria.Add("PhoneNumber", NexxoUtil.SafeSQLString(lookUpCriteria.PrimaryPhone.Replace("-", ""), true));
			if (!string.IsNullOrEmpty(lookUpCriteria.ZipCode)) customerLookUpCriteria.Add("ZipCode", NexxoUtil.SafeSQLString(lookUpCriteria.ZipCode, true));
			if (!string.IsNullOrEmpty(lookUpCriteria.LastName)) customerLookUpCriteria.Add("LastName", NexxoUtil.SafeSQLString(lookUpCriteria.LastName, true));
			if (!string.IsNullOrEmpty(lookUpCriteria.DateOfBirth)) customerLookUpCriteria.Add("DateOfBirth", NexxoUtil.SafeSQLString(lookUpCriteria.DateOfBirth, true));
			if (!string.IsNullOrEmpty(lookUpCriteria.AccountNumber)) customerLookUpCriteria.Add("AccountNumber", NexxoUtil.SafeSQLString(lookUpCriteria.AccountNumber, true));
			if (!string.IsNullOrEmpty(lookUpCriteria.CardNumber)) customerLookUpCriteria.Add("CardNumber", NexxoUtil.SafeSQLString(lookUpCriteria.CardNumber, true));
			//if (!string.IsNullOrEmpty(lookUpCriteria.AccountType) && lookUpCriteria.AccountType != "undefined") customerLookUpCriteria.Add("AccountType", NexxoUtil.SafeSQLString(lookUpCriteria.AccountType, true));
			//Server should be contacted only if the lookup criteria has changed. For pagination, use the data from TempData
			prevLookUpCriteria = (Dictionary<string, object>)TempData["CustomerLookUpCriteria"];
			if (!string.IsNullOrEmpty(lookUpCriteria.CardNumber) && !string.IsNullOrEmpty(lookUpCriteria.CVV))
			{
				SecureData secure = new SecureData();

				string decryptedCardNumber = secure.Decrypt(lookUpCriteria.CardNumber, lookUpCriteria.CVV);
				Session["CardNumber"] = decryptedCardNumber;

				NLogHelper.Debug("CustomerSearchGrid :decrypted card number {0}", decryptedCardNumber);

			}
			if (prevLookUpCriteria != null && prevLookUpCriteria.Count.Equals(customerLookUpCriteria.Count))
			{
				foreach (var item in customerLookUpCriteria)
				{
					if (!(prevLookUpCriteria.ContainsKey(item.Key)) || item.Value.ToString() != prevLookUpCriteria[item.Key].ToString())
					{
						lookUpCriteriaSameAsPrev = false;
						break;
					}
				}
			}
			else
			{
				lookUpCriteriaSameAsPrev = false;
			}

			if (!lookUpCriteriaSameAsPrev)
			{
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				Desktop client = new Desktop();

				long channelPartnerId = client.GetChannelPartner(Session["ChannelPartnerName"].ToString(), mgiContext).Id;

				//Get SSO attributes from Cookie
				mgiContext.Context = new Dictionary<string, object>();

				if (!mgiContext.Context.ContainsKey("SSOAttributes") || mgiContext.Context["SSOAttributes"] == null)
				{
					mgiContext.Context.Add("SSOAttributes", GetSSOAgentSession("SSO_AGENT_SESSION"));
				}
				customers = client.CustomerLookUp(Session["sessionId"].ToString(), customerLookUpCriteria, mgiContext);

				//Store the details in TempData
				TempData["CustomerLookUp"] = customers;

			}
			else
			{
				customers = (Prospect[])TempData["CustomerLookUp"];

				//Store the details in TempData
				TempData["CustomerLookUp"] = customers;
			}

			TempData["CustomerLookUpCriteria"] = customerLookUpCriteria;
			int totalRecords;
			object jsonData;

			if (customers != null)
			{
				totalRecords = customers.Count();
				var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);



				//************* DO NOT CHANGE THE ALIAS NAMES *************//
				int i = 0;
				var data = (from s in customers
							select new
							{
								id = i++,
								customername = s.FName + " " + s.MName + " " + s.LName,
								DateOfBirth = s.DateOfBirth != null || s.DateOfBirth != DateTime.MinValue ? s.DateOfBirth.Value.ToShortDateString() : string.Empty,
								GovernmentId = s.ID.GovernmentId ?? string.Empty,
								address = s.Address1 ?? string.Empty,
								SSN = s.SSN ?? string.Empty
							}).ToArray();

				jsonData = new
				{
					display = true,
					repeatItems = false,
					total = totalPages,
					page = page,
					records = totalRecords,
					rows = data.Skip((page - 1) * rows).Take(rows)
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[CustomHandleErrorAttribute(ViewName = "CustomerLookUp", MasterName = "_Menu", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.CustomerLookUp")]
		public string PopulateCustomerDetails(string id, string SSN, string IsCompanionSearch)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop client = new Desktop();
			long agentSessionId = GetAgentSessionId();
			bool isSSNValid = true;
			bool isCompanionSearch = Boolean.Parse(IsCompanionSearch);
			if (!string.IsNullOrWhiteSpace(SSN))
				isSSNValid = client.ValidateSSN(Session["sessionId"].ToString(), SSN, mgiContext);

			if (!isSSNValid && isCompanionSearch)
			{
				CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria()
				{
					SSN = SSN
				};
				mgiContext.IsCompanionSearch = isCompanionSearch;
				CustomerSearchResult result = client.SearchCustomers(Convert.ToString(agentSessionId), searchCriteria, mgiContext).FirstOrDefault();

				if (!string.IsNullOrWhiteSpace(result.CardNumber))
				{
					var errMsg = MGI.Channel.DMS.Web.App_GlobalResources.Nexxo.OrderCompanionCardMessage;
					NLogHelper.Error("Companion Card Search:", errMsg);
					throw new Exception(errMsg);
				}
				else
				{
					TempData["Prospect"] = GetCustomerDetails(id);
					TempData["AddOnAlloyId"] = result.AlloyID;
					return "CompanionSearch";
				}
			}

			Prospect prospect = new Prospect();

			if (isSSNValid)
			{
				prospect = GetCustomerDetails(id);
				prospect.ProfileStatus = ProfileStatus.Active;
				CustomerProspect customerProspect = GetCustomerProspect(agentSessionId, prospect);
				customerProspect.IsNewCustomer = true;
				Session["CustomerProspect"] = customerProspect;
				TempData["CustomerLookUpPartnerAccountNumber"] = prospect.PartnerAccountNumber;
				TempData["CustomerLookUpRelationshipAccountNumber"] = prospect.RelationshipAccountNumber;
				TempData["CustomerLookUpBankId"] = prospect.BankId;
				TempData["CustomerLookUpBranchId"] = prospect.BranchId;
				TempData["CustomerLookUpProgramId"] = prospect.ProgramId;
				TempData["FetchedFromCustomerLookUp"] = true;
				//Check if we have all the required filed value
				if (client.ValidateCustomer(Session["sessionId"].ToString(), prospect, mgiContext))
				{
					customerProspect.AlloyID = Convert.ToInt64(client.GeneratePAN(((Hashtable)Session["HTSessions"])["AgentSessionId"].ToString(), prospect, mgiContext));
					NLogger.Info("PopulateCustomerDetails: customerProspect.AlloyID {0} ", customerProspect.AlloyID);
					return "True";
				}
				TempData["IsCompanionSearch"] = IsCompanionSearch;

				return "False";
			}
			else
			{
				var errMsg = "Customer with SSN " + SSN + " already exists. Cannot re-register customer in system";
				NLogHelper.Error("PopulateCustomerDetails: isSSNValid {0} ", errMsg);
				throw new Exception(errMsg);
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


		private CustomerProspect GetCustomerProspect(long agentSessionId, Prospect prospect)
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
						FirstName = prospect.FName,
						Gender = prospect.Gender,
						LastName = prospect.LName,
						MiddleName = prospect.MName,
						SecondLastName = prospect.LName2,
						SSN = (!string.IsNullOrWhiteSpace(prospect.SSN)) ? WebHelper.MaskSSNNumber(prospect.SSN) : string.Empty
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
						CustomerProfileStatus = prospect.ProfileStatus,
						LReceiptLanguage = client.GetRecieptLanguages(),
					}
				},
				IdentificationInformation = new IdentificationInformation()
				{
					Country = prospect.ID.Country,
					CountryOfBirth = prospect.ID.CountryOfBirth,
					//2080
					DateOfBirth = prospect.DateOfBirth == null || prospect.DateOfBirth == DateTime.MinValue ? string.Empty : (Convert.ToDateTime(prospect.DateOfBirth)).ToString("MM/dd/yyyy"),
					GovtIdIssueState = prospect.ID != null ? prospect.ID.State : string.Empty,


					GovtIDType = prospect.ID != null ? prospect.ID.IDType : string.Empty,
					IDExpireDate = prospect.ID != null ? (prospect.ID.ExpirationDate == DateTime.MinValue ? string.Empty : (Convert.ToDateTime(prospect.ID.ExpirationDate)).ToString("MM/dd/yyyy")) : string.Empty,
					//2080
					IDIssuedDate = prospect.ID != null ? (prospect.ID.IssueDate != null ? (prospect.ID.IssueDate == DateTime.MinValue ? string.Empty : ((DateTime)prospect.ID.IssueDate).ToString("MM/dd/yyyy")) : string.Empty) : string.Empty,
					GovernmentId = prospect.ID != null ? prospect.ID.GovernmentId : string.Empty,

					MotherMaidenName = prospect.MoMaName,
					LegalCode = prospect.LegalCode,
					PrimaryCountryCitizenShip = prospect.PrimaryCountryCitizenShip,
					SecondaryCountryCitizenShip = prospect.SecondaryCountryCitizenShip,
					ClientID = prospect.ClientID
				},
				EmploymentDetails = new EmploymentDetails()
				{
					EmployerName = prospect.Employer,
					EmployerPhoneNumber = prospect.EmployerPhone,
					Profession = prospect.Occupation
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

		/****************************Begin TA-50 Changes************************************************/
		//     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
		//     Purpose: On Vera Code Scan, This call contains a cross-site scripting (XSS) flaw
		//				We found That this method is public and is called inside another action result and never called from view or js. So we can make it private. By making private the method will not be taken as action result
		private Prospect GetCustomerDetails(string id)
		{
			/****************************Begin TA-50 Changes************************************************/
			//     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
			//     Purpose: On Vera Code Scan, This call contains a cross-site scripting (XSS) flaw
			id = Sanitizer.GetSafeHtmlFragment(id);
			Prospect[] customers = (Prospect[])TempData["CustomerLookUp"];
			Prospect selectedCustomer = new Prospect();

			for (int i = 0; i < customers.Length; i++) //(var s in customers)
			{
				if (i.ToString() == id)
				{
					selectedCustomer = customers[i];
					break;
				}
			}

			return selectedCustomer;
		}
		/****************************End TA-50 Changes************************************************/
	}
}
