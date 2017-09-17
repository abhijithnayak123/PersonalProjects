using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.ServiceClient.DMSService;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using NexxoSOAPFault = MGI.Common.Sys.NexxoSOAPFault;
using SharedData = MGI.Channel.Shared.Server.Data;


namespace MGI.Channel.DMS.Web.ServiceClient
{
	public partial class Desktop
	{
		public const string UNITED_STATES = "UNITED STATES";
		//DMS Backend Service
		public DesktopServiceClient DesktopService { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public Desktop()
		{
			DesktopService = new DesktopServiceClient();
		}
		public Response GetSummaryReceipt(long customerSessionId, long cartId, MGIContext mgiContext)
		{
			return DesktopService.GetSummaryReceipt(customerSessionId, cartId, mgiContext);
		}

		#region CHECK PROCESSING RELATED METHODS

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="checkId"></param>
		/// <returns></returns>
		public Response GetCheckStatus(string customerSessionId, string checkId, MGIContext mgiContext)
		{
		   Response response = DesktopService.GetCheckStatus(long.Parse(customerSessionId), checkId, mgiContext);
		   return response;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="check"></param>
		/// <returns></returns>
		public Response SubmitCheck(string customerSessionId, CheckSubmission check, MGIContext mgiContext)
		{
			Response response = DesktopService.SubmitCheck(long.Parse(customerSessionId), check, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="checkId"></param>
		/// <returns></returns>
		public Response CancelCheck(string customerSessionId, string checkId, MGIContext mgiContext)
		{
			
			Response response =	DesktopService.CancelCheck(long.Parse(customerSessionId), checkId, mgiContext);
			return response;
			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="checkId"></param>
		/// <returns></returns>
		public Response CanResubmit(long customerSessionId, string checkId, MGIContext mgiContext)
		{
			Response response = DesktopService.CanResubmit(customerSessionId, checkId, mgiContext);
			return response;
		}

		public Response GetCheckTypes(long customerSessionId, MGIContext mgiContext)
		{

				Response response = new Response();
				List<SelectListItem> list = new List<SelectListItem>();
				list.Add(DefaultListItem());

				response = DesktopService.GetCheckTypes(customerSessionId, mgiContext);
				List<string> checkTypes = response.Result as List<string>;

				foreach (var val in checkTypes)
				{
					list.Add(new SelectListItem() { Text = val, Value = val });
				}
				
				response.Result= list;
				return response;
		}

		public Response GetCheckFee(string customerSessionId, CheckSubmission checkSubmit, MGIContext mgiContext)
		{

			Response response = DesktopService.GetCheckFee(long.Parse(customerSessionId), checkSubmit, mgiContext);
			return response;
		}

		public Response GetCheckProcessorInfo(string agentSessionId, MGIContext mgiContext)
		{
			
				Response response = DesktopService.GetCheckProcessorInfo(long.Parse(agentSessionId), mgiContext);
				return response;
			
		}
		public Response GetMessageDetails(long agentSessionId, long transactionId, MGIContext mgiContext)
		{
			
				Response response = DesktopService.GetMessageDetails(agentSessionId, transactionId, mgiContext);
				return response;
		}


		public Response  RemoveCheck(long customerSessionId, long checkId, MGIContext mgiContext, bool isParkedTransaction = false)
		{
			Response response = DesktopService.RemoveCheck(customerSessionId, checkId, isParkedTransaction, mgiContext);
			return response;
		}

		#endregion

		#region BILL PAY RELATED METHODS

		/// <summary>
		/// This will return the list of billers (actually products)
		/// </summary>
		/// <param name="channelPartnerID"></param>
		/// <param name="searchTerm"></param>
		/// <param name="context">context should contain LocationRegionID which is a guid</param>
		/// <returns></returns>
		public Response GetBillers(long customerSessionId, long channelPartnerID, string searchTerm, MGIContext mgiContext)
		{
			Response response = DesktopService.GetBillers(customerSessionId, channelPartnerID, searchTerm, mgiContext);
			return response;
		}

		//@@@ are we using this method?
		/// <summary>
		/// Get biller information by ID
		/// </summary>
		/// <param name="billerID"></param>
		/// <returns></returns>
		public Response GetBiller(long customerSessionId, long billerID, MGIContext mgiContext)
		{
			Response response = DesktopService.GetBiller(customerSessionId, billerID, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="billerName"></param>
		/// <returns></returns>
		public Response GetBiller(long customerSessionId, long channelPartnerID, string billerNameOrCode, MGIContext mgiContext)
		{
			Response response = DesktopService.GetBillerByName(customerSessionId, channelPartnerID, billerNameOrCode, mgiContext);
			return response;
		}

		/// <summary>
		/// CFP implementation
		/// </summary>
		/// <param name="customerAccountNo"></param>
		/// <param name="customerSessionId"></param>
		/// <returns></returns>
		/// <param name="alloyId"></param>
		public Response GetFrequentBillers(long customerSessionId, long alloyId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetFrequentBillers(customerSessionId, alloyId, mgiContext);
			return response;
		}

		//@@@ are we using this method?
		public Response GetAllBillers(long customerSessionId, long channelPartnerID, Guid locationRegionID, MGIContext mgiContext)
		{
			Response response = DesktopService.GetAllBillers(customerSessionId, channelPartnerID, locationRegionID, mgiContext);
			return response;
		}

		public Response GetLocations(long customerSessionId, string billerName, string accountNumber, decimal amount, MGIContext mgiContext)
		{
			Response response = DesktopService.GetLocations(customerSessionId, billerName, accountNumber, amount, mgiContext);
			return response;
		}

		public Response GetFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation location, MGIContext mgiContext)
		{
			Response response = DesktopService.GetFee(customerSessionId, billerNameOrCode, accountNumber, amount, location, mgiContext);
			return response;
		}

		public Response GetBillerInfo(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
		{
			Response response = DesktopService.GetBillerInfo(customerSessionId, billerNameOrCode, mgiContext);
			return response;
		}

		public Response GetProviderAttributes(long customerSessionId, string billerNameOrCode, string location, MGIContext mgiContext)
		{
			Response response = DesktopService.GetProviderAttributes(customerSessionId, billerNameOrCode, location, mgiContext);
			return response;
		}

		public List<SelectListItem> DefaultSelectList()
		{
			List<SelectListItem> selectList = new List<SelectListItem>();
			selectList.Add(DefaultListItem());
			return selectList;
		}

		public Response GetFavoriteBiller(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
		{
			Response response = DesktopService.GetFavoriteBiller(customerSessionId, billerNameOrCode, mgiContext);
			return response;
		}

		public Response StageBillPayment(long customerSessionId, long transactionID, MGIContext mgiContext)
		{
			Response response = DesktopService.StageBillPayment(customerSessionId, transactionID, mgiContext);
			return response;
		}

		public Response ValidateBillPayment(long customerSessionId, BillPayment payment, MGIContext mgiContext)
		{
			Response response = DesktopService.ValidateBillPayment(customerSessionId, payment, mgiContext);
			return response;
		}

		//@@@ are we using this method?
		public Response CancelBillPayment(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			try
			{
				//DesktopService.CancelBillPayment(customerSessoinID, transactionId);
				return null;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		public Response GetCardInfo(long customerSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetCardInfo(customerSessionId, mgiContext);
			return response;
		}

		public Response AddPastBillers(long customerSessionId, string cardNumber, MGIContext mgiContext)
		{
			Response response = DesktopService.AddPastBillers(customerSessionId, cardNumber, mgiContext);
			return response;
		}

		//Begin TA-191 Changes
		//       User Story Number: TA-191 | Web |   Developed by: Sunil Shetty     Date: 21.04.2015
		//       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. we are sending Customer session id and biller id.
		// the customer session id will get AlloyID/Pan ID and biller ID will help us to disable biller from tCustomerPreferedProducts table
		public Response DeleteFavoriteBiller(long customerSessionId, long billerID, MGIContext mgiContext)
		{
			Response response = DesktopService.DeleteFavoriteBiller(customerSessionId, billerID, mgiContext);
			return response;
		}
		//End TA-191 Changes
		#endregion

		#region CUSTOMER RELATED METHODS

		/// <summary>
		/// 
		/// </summary>
		/// <param name="agentSessionId"></param>
		/// <param name="customerAuthentication"></param>
		/// <returns></returns>
        public Response InitiateCustomerSession(string agentSessionId, long alloyId, int cardPresentedType, MGIContext mgiContext)
        {
            //Service method shoud be modified to accept only AlloyID instead of CustomerAuthentication object as second param.
            CustomerAuthentication customerAuthentication = new CustomerAuthentication() { AlloyID = alloyId };
            mgiContext.CardPresentedType = cardPresentedType;
            Response response = DesktopService.InitiateCustomerSession(long.Parse(agentSessionId), customerAuthentication, mgiContext);

            return response;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="agentId"></param>
		/// <param name="customerSessionId"></param>
		/// <param name="IdentificationStatus"></param>
		/// <returns></returns>
        public Response RecordIdentificationConfirmation(string agentId, string customerSessionId, bool IdentificationStatus, MGIContext mgiContext)
        {
            Response response = DesktopService.RecordIdentificationConfirmation(long.Parse(customerSessionId), agentId, IdentificationStatus, mgiContext);
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="prospect"></param>
        /// <returns></returns>
        public Response GeneratePAN(string agentSessionId, Prospect prospect, MGIContext mgiContext)
        {
            Response response = DesktopService.Create(long.Parse(agentSessionId), prospect, mgiContext);
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="alloyId"></param>
        /// <param name="prospect"></param>
        public Response SaveCustomerProfile(string agentSessionId, long alloyId, Prospect prospect, MGIContext mgiContext, bool editMode = false)
        {
            Response saveResponse = DesktopService.Save(long.Parse(agentSessionId), alloyId, prospect, mgiContext);

            if (saveResponse.Error != null && saveResponse.Error.Details != null)
                return saveResponse;

            if ( editMode )
            {
                Response updateResponse = DesktopService.UpdateCustomer(long.Parse(agentSessionId), alloyId, prospect, mgiContext);

                if (updateResponse.Error != null && updateResponse.Error.Details != null)
                    return updateResponse;
            }
            
            return new Response();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="alloyId"></param>
        /// <returns></returns>
        public Response GetCustomerProfile(string agentSessionId, long alloyId, MGIContext mgiContext)
        {
            Response response = DesktopService.GetProspect(long.Parse(agentSessionId), alloyId, mgiContext);
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Response SearchCustomers(string agentSessionId, CustomerSearchCriteria searchCriteria, MGIContext mgiContext)
        {
            Response response = DesktopService.SearchCustomers(long.Parse(agentSessionId), searchCriteria, mgiContext);
            return response;
        }

		/// <summary>
		///
		/// </summary>
		/// <param name="fundPaymentId"></param>
		/// <returns></returns>
		/// <param name="sessionId"></param>
		public string[] GetReceiptData(string fundPaymentId, string sessionId, MGIContext mgiContext)
		{
			try
			{
				Dictionary<string, string> context = new Dictionary<string, string>();
				//return DesktopService.GetReceiptData(sessionId, fundPaymentId, context);
				return new string[0];
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelPartner"></param>
		/// <returns></returns>
		public Response GetChannelLocations(long agentSessionId, string channelPartner, MGIContext mgiContext)
		{
			return DesktopService.Locations(agentSessionId, channelPartner, mgiContext);
		}

		public Response AuthenticateSSO(AgentSSO ssoAgent, string channelPartner, string terminalName, MGIContext mgiContext)
		{
			return DesktopService.AuthenticateSSO(ssoAgent, channelPartner, terminalName, mgiContext);
		}

		public Response UpdateSession(long sessionId, Terminal terminal, MGIContext mgiContext)
		{
			return DesktopService.UpdateSession(sessionId, terminal, mgiContext);
		}

		public Response GetAgentMessages(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			response = DesktopService.GetAgentMessages(agentSessionId, mgiContext);
			var msgs = response.Result as List<AgentMessage>;
			List<AgentMessage> lst = new List<AgentMessage>();
			foreach (var msg in msgs)
			{
				lst.Add(
					new AgentMessage()
					{
						CustomerFirstName = msg.CustomerFirstName.Trim(),
						CustomerLastName = msg.CustomerLastName.Trim(),
						Amount = Convert.ToDecimal(msg.Amount).ToString("F"),
						TransactionState = msg.TransactionState.Trim(),
						TransactionId = msg.TransactionId,
						TicketNumber = msg.TicketNumber,
						DeclineMessage = msg.DeclineMessage
					});
			}
			response.Result = lst;
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="channelPartnerName"></param>
		/// <returns></returns>
		public Response GetChannelPartner(string channelPartnerName, MGIContext mgiContext)
		{
			return DesktopService.ChannelPartnerConfig(channelPartnerName, mgiContext);
		}

		public Response GetChannelPartnerCertificateInfo(long channelPartnerId, string issuer, MGIContext mgiContext)
		{

			return DesktopService.GetChannelPartnerCertificateInfo(channelPartnerId, issuer, mgiContext);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		public Customer GetCustomerByUserName(string userName)
		{
			try
			{
				Customer customer = new Customer();
				customer.ID = new Identification();

				return customer;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="agentSessionId"></param>
		/// <param name="alloyId"></param>
		/// <returns></returns>
		public Response NexxoActivate(string agentSessionId, long alloyId, MGIContext mgiContext)
		{
			Response response = DesktopService.NexxoActivate(long.Parse(agentSessionId), alloyId, mgiContext);
            return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="agentSessionId"></param>
		/// <param name="alloyId"></param>
		/// <returns></returns>
		public Response ClientActivate(string agentSessionId, long alloyId, MGIContext mgiContext)
		{
			Response response =	DesktopService.ClientActivate(long.Parse(agentSessionId), alloyId, mgiContext);
            return response;
		}

        public Response UpdateCustomerToClient(string agentSessionId, long alloyId, MGIContext mgiContext)
		{
            Response response = DesktopService.UpdateCustomerToClient(long.Parse(agentSessionId), alloyId, mgiContext);
            return response;
		}

        public Response CustomerSyncInFromClient(string agentSessionId, long alloyId, MGIContext mgiContext)
		{
            Response response = DesktopService.CustomerSyncInFromClient(long.Parse(agentSessionId), alloyId, mgiContext);
            return response;
		}

        public Response ValidateSSN(string agentSessionId, string SSN, MGIContext mgiContext)
        {
             Response response = DesktopService.ValidateSSN(long.Parse(agentSessionId), SSN, mgiContext);
             return response;
        }

        /// <summary>
        /// AL-231
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="purse"></param>
        /// <returns></returns>
        public Response CanChangeProfileStatus(long agentSessionId, long userId, string profileStatus, MGIContext mgiContext)
        {
             Response response = DesktopService.CanChangeProfileStatus(agentSessionId, userId, profileStatus, mgiContext);
             return response;
        }

        public Response ValidateCustomer(string agentSessionId, Prospect prospect, MGIContext mgiContext)
        {
             Response response = DesktopService.ValidateCustomer(long.Parse(agentSessionId), prospect, mgiContext);
             return response;
        }

        /// <summary>
        /// Get Customers Details By Search Parameters
        /// </summary>
        /// <param name="agentSessionId">Session Id</param>
        /// <param name="customerLookUpCriteria">Search Parameters</param>
        /// <returns></returns>
        public Response CustomerLookUp(string agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
             Response response = DesktopService.CustomerLookUp(long.Parse(agentSessionId), customerLookUpCriteria, mgiContext);
             return response;
        }

        /// <summary>
        /// Validate the Customer for InitiateCustomerSession
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <param name="alloyId"></param>
        public Response ValidateCustomerStatus(string agentSessionId, long alloyId, MGIContext mgiContext)
        {
              Response response = DesktopService.ValidateCustomerStatus(long.Parse(agentSessionId), alloyId, mgiContext);
              return response;
        }

		public Response UpdateCounterId(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopService.UpdateCounterId(customerSessionId, mgiContext);
		}
		#endregion

		#region MASTER DATA RELATED METHODS
		/// <summary>
		/// 
		/// </summary>
		/// <param name="countryId"></param>
		/// <param name="idType"></param>
		/// <param name="state"></param>
		/// <returns></returns>
		public Response GetIdRequirements(long agentSessionId, long channelPartnerId, string countryId, string idType, string state, MGIContext mgiContext)
		{
			Response response = DesktopService.IdRequirements(agentSessionId, channelPartnerId, countryId, idType, state, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="CountryId"></param>
		/// <param name="GovtIdType"></param>
		/// <returns></returns>
		public Response GetStates(long agentSessionId, long channelPartnerId, MGIContext mgiContext, string CountryId = null, string GovtIdType = null)
		{
			Response response = new Response();
			List<SelectListItem> StateList = new List<SelectListItem>();
			StateList.Add(DefaultListItem());
			if (CountryId != null && GovtIdType != null)
			{
				response = DesktopService.IdStates(agentSessionId, channelPartnerId, CountryId, GovtIdType, mgiContext);
				if (response.Error != null)
					return response;
				List<string> WebStates = response.Result as List<string>;
               
				if (WebStates != null && WebStates.Count > 0)
				{
                    WebStates.Sort();
					StateList.AddRange(WebStates.Select(s => new SelectListItem() { Value = s, Text = s }));
				}
			}
			response.Result = StateList;
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>

		public Response GetMasterCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{
			List<MasterCountry> mastercountries = new List<MasterCountry>();
			Response response = DesktopService.MasterCountries(agentSessionId, channelPartnerId, mgiContext);
			if (response.Error != null)
				return response;
			mastercountries = response.Result as List<MasterCountry>;
			var topCountries = new[] { "UNITED STATES", "CANADA", "MEXICO" };
			var orderByCountryName = mastercountries.OrderByDescending(c => topCountries.Contains(c.Name.ToUpper())).ThenBy(c => c.Name).ToList();
			var topItem = orderByCountryName.Single(x => x.Name.ToUpper() == UNITED_STATES);
			var masterCountryList = new List<MasterCountry>();
			masterCountryList.Add(topItem);
			masterCountryList = masterCountryList.Concat(orderByCountryName.Where(c => c.Name.ToUpper() != UNITED_STATES)).ToList();
			response.Result = masterCountryList;
			return response;			
		}

		public Response GetCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext)
		{
			List<SelectListItem> countries = new List<SelectListItem>();
			countries.Add(DefaultListItem());
			Response response = DesktopService.IdCountries(agentSessionId, channelPartnerId, mgiContext);
			if (response.Error != null)
				return response;
            List<string> WebCountries = response.Result as List<string>;
			var topCountries = new[] { "UNITED STATES", "CANADA", "MEXICO" };
			var orderByCountryName = WebCountries.OrderByDescending(c => topCountries.Contains(c.ToUpper())).ThenBy(x => x);
			var topItem = orderByCountryName.Single(x => x.ToUpper() == UNITED_STATES);
			var masterCountryList = new List<string>();
			masterCountryList.Add(topItem);
			masterCountryList = masterCountryList.Concat(orderByCountryName.Where(p => p.ToUpper() != UNITED_STATES)).ToList();
			if (masterCountryList != null)
			{
				foreach (var val in masterCountryList)
				{
					countries.Add(new SelectListItem() { Text = val, Value = val });
				}
			}
			response.Result = countries;
			return response;			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response GetCountryOfBirth(long agentSessionId, string countryISOCode, MGIContext mgiContext)
		{
			//Response response = new Response();
			Response response = DesktopService.GetMasterCountryByCode(agentSessionId, countryISOCode, mgiContext);
			return response;

			//string countryName = string.Empty;
			//try
			//{
			//	response = DesktopService.GetMasterCountryByCode(agentSessionId, countryISOCode, mgiContext);

			//	if (response != null)
			//	{
			//		countryName = country.Name;
			//	}
			//}

			//response.Result = countryName;
			//return response;		
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="countryId"></param>
		/// <returns></returns>
		public Response GetGovtIdType(long agentSessionId, long channelPartnerId, MGIContext mgiContext, string countryId = null)
		{
			Response response = new Response();
			List<SelectListItem> GovtIdList = new List<SelectListItem>();
			GovtIdList.Add(DefaultListItem());
			if (countryId != null)
			{
				response = DesktopService.IdTypes(agentSessionId, channelPartnerId, countryId, mgiContext);
				if (response.Error != null)
					return response;
                List<string> WebIdCountries = response.Result as List<string>;
				if (WebIdCountries != null)
				{
					foreach (var val in WebIdCountries)
					{
						GovtIdList.Add(new SelectListItem() { Text = val, Value = val });
					}
				}
			}
			response.Result = GovtIdList;
			return response;			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response GetLegalCodes(long agentSessionId, MGIContext mgiContext)
		{
			List<SelectListItem> LegalcodeList = new List<SelectListItem>();
			LegalcodeList.Add(DefaultListItem());
			Response response = DesktopService.GetLegalCodes(agentSessionId, mgiContext);
			if (response.Error != null)
				return response;
            List<LegalCode> WebIdLegalCodes = response.Result as List<LegalCode>;
			if (WebIdLegalCodes != null)
			{
				foreach (var val in WebIdLegalCodes)
				{
					LegalcodeList.Add(new SelectListItem() { Text = val.Name, Value = val.Code });
				}
			}
			response.Result = LegalcodeList;
			return response;
			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response GetOccupations(long agentSessionId, MGIContext mgiContext)
		{
			List<SelectListItem> OccupationList = new List<SelectListItem>();
			OccupationList.Add(DefaultListItem());
			Response response = DesktopService.GetOccupations(agentSessionId, mgiContext);
			if (response.Error != null)
				return response;
			List<Occupation> Occupations = response.Result as List<Occupation>;

			if (Occupations != null)
			{
				foreach (var val in Occupations)
				{
					OccupationList.Add(new SelectListItem() { Text = val.Name, Value = val.Code });
				}
			}

			response.Result = OccupationList;
			return response;

		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response PhoneType(long agentSessionId, MGIContext mgiContext)
		{
			List<SelectListItem> PhoneTypes = new List<SelectListItem>();
			PhoneTypes.Add(DefaultListItem());
			Response response = DesktopService.PhoneTypes(agentSessionId, mgiContext);
			if (response.Error != null)
				return response;
            List<string> WebPhoneTypes = response.Result as List<string>;
			if (WebPhoneTypes != null)
			{
				foreach (var val in WebPhoneTypes)
				{
					PhoneTypes.Add(new SelectListItem() { Text = val, Value = val });
				}
			}
			response.Result = PhoneTypes;
			return response;
		
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response USStates(long agentSessionId, MGIContext mgiContext)
		{
			List<SelectListItem> States = new List<SelectListItem>();
			States.Add(DefaultListItem());

			Response response = DesktopService.USStates(agentSessionId, mgiContext);
			if (response.Error != null)
				return response;
            List<string> WebStates = response.Result as List<string>;

			if (WebStates != null)
			{
                WebStates.Sort();
				foreach (var val in WebStates)
				{
					States.Add(new SelectListItem() { Text = val, Value = val });
				}
			}
			response.Result = States;
			return response;			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response PhoneProvider(long agentSessionId, MGIContext mgiContext)
		{
			List<SelectListItem> MobileProviders = new List<SelectListItem>();
			MobileProviders.Add(DefaultListItem());
			Response response = DesktopService.MobileProviders(agentSessionId, mgiContext);
			if (response.Error != null)
				return response;
            List<string> WebMobileProviders = response.Result as List<string>;

			if (WebMobileProviders != null)
			{
				foreach (var val in WebMobileProviders)
				{
					MobileProviders.Add(new SelectListItem() { Text = val, Value = val });
				}
			}
			response.Result = MobileProviders;
			return response;
			
		}
		/// <summary>
		/// Get List of Groups
		/// </summary>
		/// <returns></returns>
		public Response GetGroups(string channelPartner, MGIContext mgiContext)
		{
			Response response = new Response();
			List<SelectListItem> Groups = new List<SelectListItem>();
			Groups.Add(DefaultListItem());
			response = DesktopService.GetPartnerGroups(channelPartner, mgiContext);
			if (response.Error != null)
				return response;
			var list = response.Result as List<string>;
			if (list.Count() > 0)
			{
				foreach (var val in list)
				{
					Groups.Add(new SelectListItem() { Text = val, Value = val });
				}
			}
			response.Result = Groups;
			return response;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetRecieptLanguages()
		{
			try
			{
				List<SelectListItem> ReceiptLanguages = new List<SelectListItem>();

				Array itemNames = Enum.GetNames(typeof(Language));

				for (int i = 0; i < itemNames.Length; i++)
				{
					ReceiptLanguages.Add(new SelectListItem() { Text = itemNames.GetValue(i).ToString(), Value = itemNames.GetValue(i).ToString() });
				}

				//ReceiptLanugages.Add(new SelectListItem() { Text = "English", Value = "English", Selected = true });
				//ReceiptLanugages.Add(new SelectListItem() { Text = "Spanish", Value = "Spanish" });

				return ReceiptLanguages;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="viewName"></param>
		/// <param name="cultureInfo"></param>
		/// <param name="channelPartner"></param>
		/// <returns></returns>
		public Response GetTipsAndOffersForChannelPartner(long agentSessionId, string viewName, string cultureInfo, string channelPartner, string optionalFilter, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				string tipsandOffers = null;
				response = DesktopService.GetTipsAndOffers(agentSessionId, channelPartner, cultureInfo, viewName, mgiContext);				

				List<TipsAndOffers> TipsAndOffersList = response.Result as List<TipsAndOffers>;


				if (TipsAndOffersList.Count > 0 && optionalFilter != null)

					tipsandOffers = TipsAndOffersList.Where(x => x.OptionalFilter == optionalFilter).FirstOrDefault().TipsAndOffersValue;

				else if (TipsAndOffersList.Count > 0)

					tipsandOffers = TipsAndOffersList.Where(x => x.OptionalFilter == null || x.OptionalFilter == "").FirstOrDefault().TipsAndOffersValue;


				if (tipsandOffers != null && tipsandOffers.Trim().Length > 0)
				{
					response.Result = tipsandOffers;
					return response;
				}
			}
			catch (Exception)
			{
				response.Result = string.Empty;
				return response;
			}

			response.Result = string.Empty;
			return response;
		}

		#endregion

		#region REPORTS RELATED METHODS
		/// <summary>
		/// 
		/// </summary>
		/// <param name="agentId"></param>
		/// <returns></returns>
		public CashDrawer CashDrawerReport(long agentSessionId, int agentId, long locationId, MGIContext mgiContext)
		{
			try
			{
				return DesktopService.CashDrawerReport(agentSessionId, agentId, locationId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}
		#endregion

		#region Shopping Cart Methods

        public Response GetAllParkedTransactions()
        {
            Response response = DesktopService.GetAllParkedShoppingCartTransactions();
			return response;
        }

        public Response GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, MGIContext mgiContext)
        {
            Response response = DesktopService.GenerateReceiptsForShoppingCart(customerSessionId, shoppingCartId, mgiContext);
			return response;
        }

        public Response CloseShoppingCart(long customerSessionId, MGIContext mgiContext)
        {
           Response response = DesktopService.CloseShoppingCart(customerSessionId, mgiContext);
		   return response;            
        }

		#endregion

		#region PRIVATE METHODS

		/// </summary>
		/// <returns></returns>
		private SelectListItem DefaultListItem()
		{
			return new SelectListItem() { Value = string.Empty, Text = "Select", Selected = true };
		}
		#endregion

		#region SEND MONEY METHODS

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetStatus()
		{
			try
			{
				List<SelectListItem> status = new List<SelectListItem>();
				status.Add(DefaultListItem());

				List<string> stringList = new List<string>();
				stringList.Add("Active");
				stringList.Add("Inactive");

				foreach (var val in stringList)
				{
					status.Add(new SelectListItem() { Text = val, Value = val });
				}

				return status;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetGender()
		{
			try
			{
				List<SelectListItem> gender = new List<SelectListItem>();
				gender.Add(DefaultListItem());

				List<string> stringList = new List<string>();
				stringList.Add("Male");
				stringList.Add("Female");
				stringList.Sort();
				foreach (var val in stringList)
				{
					gender.Add(new SelectListItem() { Text = val, Value = val });
				}
				return gender;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetRelationShip()
		{
			try
			{
				List<SelectListItem> relationship = new List<SelectListItem>();
				relationship.Add(DefaultListItem());

				List<string> stringList = new List<string>();
				stringList.Add("Father");
				stringList.Add("Mother");

				foreach (var val in stringList)
				{
					relationship.Add(new SelectListItem() { Text = val, Value = val });
				}
				return relationship;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetCities()
		{
			try
			{
				List<SelectListItem> cities = new List<SelectListItem>();
				cities.Add(DefaultListItem());

				List<string> stringList = new List<string>();
				stringList.Add("Banglore");
				stringList.Add("Hyderabad");
				stringList.Add("Vijayawada");
				stringList.Add("Chennai");
				foreach (var val in stringList)
				{
					cities.Add(new SelectListItem() { Text = val, Value = val });
				}
				return cities;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetPayment()
		{
			try
			{
				List<SelectListItem> payment = new List<SelectListItem>();
				payment.Add(DefaultListItem());

				List<string> stringList = new List<string>();
				stringList.Add("Check");
				stringList.Add("Cash");
				stringList.Add("Balancetransfer");

				foreach (var val in stringList)
				{
					payment.Add(new SelectListItem() { Text = val, Value = val });
				}
				return payment;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetAmountType()
		{
			try
			{
				List<SelectListItem> amountType = new List<SelectListItem>();
				amountType.Add(DefaultListItem());

				List<string> stringList = new List<string>();
				stringList.Add("Check");
				stringList.Add("Cash");
				stringList.Add("Balancetransfer");

				foreach (var val in stringList)
				{
					amountType.Add(new SelectListItem() { Text = val, Value = val });
				}
				return amountType;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetDeliverMethod()
		{
			try
			{
				List<SelectListItem> deliveryMethod = new List<SelectListItem>();
				deliveryMethod.Add(DefaultListItem());

				List<string> stringList = new List<string>();
				stringList.Add("Cashon delivery");
				stringList.Add("Online");

				foreach (var val in stringList)
				{
					deliveryMethod.Add(new SelectListItem() { Text = val, Value = val });
				}
				return deliveryMethod;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// This methos we are not using
		public List<SelectListItem> GetDeliverOption()
		{
			try
			{
				List<SelectListItem> deliveryOption = new List<SelectListItem>();
				deliveryOption.Add(DefaultListItem());

				List<string> stringList = new List<string>();
				stringList.Add("Delivery at home");
				stringList.Add("Delivery at workplace");

				foreach (var val in stringList)
				{
					deliveryOption.Add(new SelectListItem() { Text = val, Value = val });
				}
				return deliveryOption;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="receiver"></param>
		/// <returns></returns>
		public Response SaveReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext)
		{
			Response response = DesktopService.AddReceiver(customerSessionId, receiver, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="receiver"></param>
		/// <returns></returns>
		public Response UpdateReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext)
		{
			Response response = DesktopService.EditReceiver(customerSessionId, receiver, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name=" "></param>
		public Response GetReceivers(long customerSessionId, string searchTerm, MGIContext mgiContext)
		{
			Response response = DesktopService.GetReceivers(customerSessionId, searchTerm, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name=" "></param>
		public Response GetFrequentReceivers(long customerSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetFrequentReceivers(customerSessionId, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name=" "></param>
		public Response GetReceiverDetails(long customerSessionId, long receiverId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetReceiver(customerSessionId, receiverId, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fullName"></param>
		/// <returns></returns>
		public Response GetReceiverByFullName(long customerSessionId, string fullName, MGIContext mgiContext)
		{
			Response response = DesktopService.GetReceiverByFullName(customerSessionId, fullName, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name=" "></param>
		//This method we are not using
		public Response GetReceiverDetailsForEdit(long customerSessionId, long receiverId, MGIContext mgiContext) // This method is needed ?? Has to be removed
		{
			Response response = DesktopService.GetReceiver(customerSessionId, receiverId, mgiContext);
			return response;
		}

		/// <summary>
		///  AL-3502
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="receiverId"></param>
		/// <param name="mgiContext"></param>
		public Response DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext)
		{
			Response response = DesktopService.DeleteFavoriteReceiver(customerSessionId, receiverId, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name=" "></param>
		/// This method we are not using
		public DoddFrankConfirmationClient GetDoddFrankConfirmationDetails(string receiverId)
		{
			DoddFrankConfirmationClient receiver = new DoddFrankConfirmationClient()
			{
				ReceiverName = "Ashok kumar",
				PickupLocation = "Anil",
				PickupMethod = "Active",
				PickupOptions = "Yes",
				CurrencyType = "India",
				TransferFee = 5,
				TransferTax = 5,
				ExchangeRate = 5,
				TransferAmount = 10,
				TotalAmount = 5,
				OtherFees = 5,
				OtherTaxes = 5,
				TotalToRecipient = 5
			};
			return receiver;
		}

		#endregion

		#region WUReceiver MaterData

		public Response GetXfrCountries(long customerSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetXfrCountries(customerSessionId, mgiContext);//.ToList<XferMasterData>();
			return response;
		}

		public Response GetXfrStates(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			Response response = DesktopService.GetXfrStates(customerSessionId, countryCode, mgiContext);//.ToList<XferMasterData>().OrderBy(c => c.Name);
			return response;
		}

		public Response GetXfrCities(long customerSessionId, string stateCode, MGIContext mgiContext)
		{
			Response response = DesktopService.GetXfrCities(customerSessionId, stateCode, mgiContext);//.ToList<XferMasterData>().OrderBy(c => c.Name);
			return response;
		}

		public Response GetDeliveryServices(long customerSessionId, DeliveryServiceRequest request, MGIContext mgiContext)
		{
			Response response = DesktopService.GetDeliveryServices(customerSessionId, request, mgiContext);
			return response;
		}

		public Response GetXfrProviderAttributes(long customerSessionId, AttributeRequest attributeRequest, MGIContext mgiContext)
		{
			Response response = DesktopService.GetXfrProviderAttributes(customerSessionId, attributeRequest, mgiContext);
			return response;
		}

		public List<SelectListItem> DefaultSelectListItem()
		{
			List<SelectListItem> defaultList = new List<SelectListItem>();
			defaultList.Add(DefaultListItem());

			return defaultList;
		}

		public Response GetRefundReasons(long customerSessionId, ReasonRequest request, MGIContext mgiContext)
		{
			Response response = DesktopService.GetRefundReasons(customerSessionId, request, mgiContext);//.ToList<SharedData.MoneyTransferReason>().OrderBy(c => c.Name);
			return response;
		}


		public Response WUGetAgentBannerMessage(long agentSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.WUGetAgentBannerMessage(agentSessionId, mgiContext);
			return response;
		}

		public Response UpdateCustomerProfile(long customerSessionId, string WUGoldCardNumber, MGIContext mgiContext)
		{
			Response response = DesktopService.UpdateWUAccount(customerSessionId, WUGoldCardNumber, mgiContext);
			return response;
		}
		#endregion

		#region MONEY TRANSFER

		/// <summary>
		/// Get MoneyTransfer Fee
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="feeRequest"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public Response GetMoneyTransferFee(long customerSessionId, FeeRequest feeRequest, MGIContext mgiContext)
		{
			Response response = DesktopService.GetMoneyTransferFee(customerSessionId, feeRequest, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xSender"></param>
		/// <param name="xBeneficiary"></param>
		/// <param name="xPay"></param>
		/// <returns></returns>
		public Response ValidateTransfer(long customerSessionId, ValidateRequest validateRequest, MGIContext mgiContext)
		{
			Response response = DesktopService.ValidateTransfer(customerSessionId, validateRequest, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="countryCode"></param>
		/// <returns></returns>
		public Response GetCurrencyCode(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			Response response = DesktopService.GetCurrencyCode(customerSessionId, countryCode, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="countryCode"></param>
		/// <returns></returns>
		public Response GetCurrencyCodeList(long customerSessionId, string countryCode, MGIContext mgiContext)
		{
			Response response = DesktopService.GetCurrencyCodeList(customerSessionId, countryCode, mgiContext);
			return response;
		}

		public Response GetReceiveTransaction(long customerSessionId, ReceiveMoneyRequest request, MGIContext mgiContext)
		{
			Response response = DesktopService.ReceiveMoneySearch(customerSessionId, request, mgiContext);
			return response;
		}

		public Response UpdateFundAmount(long customerSessionId, long cxeFundTrxId, decimal amount, FundType fundType, MGIContext mgiContext)
		{
			Response response = DesktopService.UpdateFundAmount(customerSessionId, cxeFundTrxId, amount, fundType, mgiContext);
			return response;
		}

		//AL-2729 user story for updating the cash-in transaction
		public void UpdateCash(long customerSessionId, long trxId, decimal amount, MGIContext mgiContext)
		{
			try
			{
				DesktopService.UpdateCash(customerSessionId, trxId, amount, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		public Response WUCardEnrollment(long customerSessionId, XferPaymentDetails paymentDetails, MGIContext mgiContext)
		{
			Response response = DesktopService.WUCardEnrollment(customerSessionId, paymentDetails, mgiContext);
			return response;
		}
		public Response WUCardLookup(long customerSessionId, CardLookupDetails wucardlookupreq, MGIContext mgiContext)
		{
			Response response = DesktopService.WUCardLookup(customerSessionId, wucardlookupreq, mgiContext);
			return response;
		}
		//This method we are not using
		public Response GetWUCardAccount(long customerSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetWUCardAccount(customerSessionId, mgiContext);
			return response;
		}

		public Response DisplayWUCardAccountInfo(long customerSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.DisplayWUCardAccountInfo(customerSessionId, mgiContext);
			return response;
		}

		public Response CancelXfer(long customerSessionId, long ptnrTransactionId, MGIContext mgiContext)
		{
			Response response = DesktopService.CancelXfer(customerSessionId, ptnrTransactionId, mgiContext);
			return response;
		}
		public Response AddPastReceivers(long customerSessionId, string cardNumber, MGIContext mgiContext)
		{
			Response response = DesktopService.AddPastReceivers(customerSessionId, cardNumber, mgiContext);
			return response;
		}

		public List<SelectListItem> GetActBeHalfList()
		{
			List<SelectListItem> items = new List<SelectListItem>();
			items.Add(new SelectListItem { Selected = true, Value = "0", Text = "Select" });
			items.Add(new SelectListItem { Value = "1", Text = "Yes" });
			items.Add(new SelectListItem { Value = "2", Text = "No" });
			return items;
		}

		#endregion

		#region  Send Money Refund

		public Response StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
		{
			Response response = DesktopService.StageRefundSendMoney(customerSessionId, moneyTransferRefund, mgiContext);
			return response;
		}

		public Response MoneyTransferRefund(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext)
		{
			Response response = DesktopService.SendMoneyRefund(customerSessionId, moneyTransferRefund, mgiContext);
			return response;
		}

		#endregion

		#region  Send Money Modify

		public Response GetStatus(long customerSessionId, string confirmationNumber, MGIContext mgiContext)
		{
			Response response = DesktopService.GetStatus(customerSessionId, confirmationNumber, mgiContext);
			return response;
		}

		public Response SendMoneySearch(long customerSessionId, SendMoneySearchRequest request, MGIContext mgiContext)
		{
			Response response = DesktopService.SendMoneySearch(customerSessionId, request, mgiContext);
			return response;
		}

		public Response StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext)
		{
			Response response = DesktopService.StageModifySendMoney(customerSessionId, moneyTransferModify, mgiContext);
			return response;
		}

		public Response GetMoneyTransferDetailsTransaction(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetMoneyTransferDetailsTransaction(customerSessionId, transactionId, mgiContext);
			return response;
		}

		public Response AuthorizeSendMoneyModify(long customerSessionId, ModifySendMoneyRequest modifySendMoneyRequest, MGIContext mgiContext)
		{
			Response response = DesktopService.AuthorizeModifySendMoney(customerSessionId, modifySendMoneyRequest, mgiContext);
			return response;
		}

		#endregion

		public Response GetCardInfoXfer(long customerSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetCardInfoXfer(customerSessionId, mgiContext);
			return response;
		}


		#region PREPAID CARD

		public Response GetMinimumLoadAmount(long customerSessionId, bool initialLoad, MGIContext mgiContext)
		{
			Response response = DesktopService.GetMinimumLoadAmount(customerSessionId, initialLoad, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="AlloyID"></param>
		/// <param name="fundAccount"></param>
		/// <param name="agentSessionId"></param>
		/// <param name="customerSessionId"></param>
		/// <returns></returns>
		public Response RegisterCard(FundsProcessorAccount fundAccount, string customerSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.AddFundsAccount(GetCustomerSessionId(customerSessionId), fundAccount, mgiContext);
			return response;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="CardNumber"></param>
		/// <returns></returns>
		public Response AuthenticateCard(string customerSessionId, string cardNumber, string pin, MGIContext mgiContext)
		{
			Response response = DesktopService.AuthenticateCard(GetCustomerSessionId(customerSessionId), cardNumber, pin, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="AlloyID"></param>
		/// <param name="CardNumber"></param>
		/// <param name="agentSessionId"></param>
		/// <param name="customerSessionId"></param>
		/// <returns></returns>
		public Response GetCardBalance(string customerSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetFundsBalance(GetCustomerSessionId(customerSessionId), mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="funds"></param>
		/// <returns></returns>
		public Response Load(string customerSessionId, Funds funds, MGIContext mgiContext)
		{
			Response response = DesktopService.LoadFunds(GetCustomerSessionId(customerSessionId), funds, mgiContext);
			return response;
		}
		/// <summary>
		/// Record the activation fee transaction
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="funds"></param>
		/// <returns></returns>
		public Response ActivateGPRCard(string customerSessionId, Funds funds, MGIContext mgiContext)
		{
			Response response = DesktopService.ActivateGPRCard(GetCustomerSessionId(customerSessionId), funds, mgiContext);
			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="funds"></param>
		/// <returns></returns>
		public Response Withdraw(string customerSessionId, Funds funds, MGIContext mgiContext)
		{
			Response response = DesktopService.WithdrawFunds(GetCustomerSessionId(customerSessionId), funds, mgiContext);
			return response;
		}


		/// <summary>
		/// Retrieves the fee for fund transactions based on load, withdraw or activate
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="fundType"></param>
		/// <returns></returns>
		public Response GetFundsFee(long customerSessionId, decimal amount, FundType fundType, MGIContext mgiContext)
		{
			Response response = DesktopService.GetFeeForFunds(customerSessionId, amount, fundType, mgiContext);
			return response;
		}

		public Response GetTransactionHistory(long customerSessionId, TransactionHistoryRequest request, MGIContext mgiContext)
		{
			Response response = DesktopService.GetCardTransactionHistory(customerSessionId, request, mgiContext);
			return response;
		}

		public Response CloseAccount(long customerSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.CloseAccount(customerSessionId, mgiContext);
			return response;
		}

		public Response UpdateCardStatus(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			Response response = DesktopService.UpdateCardStatus(customerSessionId, cardMaintenanceInfo, mgiContext);
			return response;
		}

		public Response ReplaceCard(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			Response response = DesktopService.ReplaceCard(customerSessionId, cardMaintenanceInfo, mgiContext);
			return response;
		}

		#endregion

		# region ManageUser

		public Response GetUser(long agentSessionId, int userId, MGIContext mgiContext)
		{
			return DesktopService.GetUser(agentSessionId, userId, mgiContext);

		}

		public Response GetUsers(long agentSessionId, long locationId, MGIContext mgiContext)
		{
			Response response = new Response();
			
			List<SelectListItem> userDetails = new List<SelectListItem>();
			userDetails.Add(DefaultListItem());

			response = DesktopService.GetUsers(agentSessionId, locationId, mgiContext);
			if (response.Error != null)
				return response;
			List<UserDetails> userDetailsList = response.Result as List<UserDetails>;
			foreach (var item in userDetailsList)
			{
				userDetails.Add(new SelectListItem() { Text = item.FullName, Value = item.Id.ToString() });
			}

			response.Result = userDetails;
			return response;
		}

		#endregion

		#region Location

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> GetLocationStatus()
		{
			try
			{
				List<SelectListItem> locationStatus = new List<SelectListItem>();
				locationStatus.Add(DefaultListItem());

				List<string> stringList = new List<string>();
				stringList.Add("Active");
				stringList.Add("Inactive");

				foreach (var val in stringList)
				{
					locationStatus.Add(new SelectListItem() { Text = val, Value = val });
				}
				return locationStatus;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response GetAllLocationNames()
		{
			return DesktopService.GetAll();
		}

		public Response GetAllLocationNames(long agentSessionId, MGIContext mgiContext)
		{

			return DesktopService.GetAllLocationByChannelPartnerId(agentSessionId, mgiContext);

		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		public Response SaveLocation(long agentSessionId, MGI.Channel.DMS.Server.Data.Location location, MGIContext mgiContext)
		{
			return DesktopService.CreateLocation(agentSessionId, location, mgiContext);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		public Response UpdateLocation(long agentSessionId, MGI.Channel.DMS.Server.Data.Location location, MGIContext mgiContext)
		{
			return DesktopService.UpdateLocation(agentSessionId, location, mgiContext);
		}


		public Response GetLocationProcessorCredentials(long agentSessionId, long locationId, MGIContext mgiContext)
		{
			DesktopServiceClient client = new DesktopServiceClient();
			return client.GetLocationProcessorCredentials(agentSessionId, locationId, mgiContext);
		}

		public Response SaveLocationProcessorCredentials(long agentSessionId, long locationId, ProcessorCredential processorCredentials, MGIContext mgiContext)
		{
			DesktopServiceClient client = new DesktopServiceClient();
			return client.SaveProcessorCredentials(agentSessionId, locationId, processorCredentials, mgiContext);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="locationName"></param>
		/// <returns></returns>
		public MGI.Channel.DMS.Server.Data.Response GetLocationDetailsForEdit(long agentSessionId, string locationName, MGIContext mgiContext)
		{

			return DesktopService.GetByName(agentSessionId, locationName, mgiContext);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="locationId"></param>
		/// <returns></returns>
		public MGI.Channel.DMS.Server.Data.Response GetLocationDetailsForEdit(string agentSessionId, long locationId, MGIContext mgiContext)
		{
				return DesktopService.LookupLocationById(agentSessionId, locationId, mgiContext);
			}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response GetUSStates(long agentSessionId, MGIContext mgiContext)
		{
			List<SelectListItem> States = new List<SelectListItem>();
			States.Add(DefaultListItem());

			Response response = DesktopService.USStates(agentSessionId, mgiContext);
			if (response.Error != null)
				return response;
            List<string> WebStates = response.Result as List<string>;
               
			if (WebStates != null)
			{
                WebStates.Sort();
				foreach (var val in WebStates)
				{
					States.Add(new SelectListItem() { Text = val, Value = val });
				}
			}
			response.Result = States;
			return response;			
		}

		#endregion

		#region NpsTerminal

		public Response CreateNpsTerminal(long agentSessionId, NpsTerminal npsTerminal, MGIContext mgiContext)
		{
			return DesktopService.CreateNpsTerminal(agentSessionId, npsTerminal, mgiContext);
		}

		public Response UpdateNpsTerminal(long agentSessionId, NpsTerminal npsTerminal, MGIContext mgiContext)
		{
			return DesktopService.UpdateNpsTerminal(agentSessionId, npsTerminal, mgiContext);
		}

		public Response LookupNpsTerminal(long agentSessionId, long terminalId, MGIContext mgiContext)
		{
			return DesktopService.LookupNpsTerminalById(agentSessionId, terminalId, mgiContext);
		}

		public Response LookupNpsTerminal(long agentSessionId, string ipAddress, MGIContext mgiContext)
		{
			return DesktopService.LookupNpsTerminalByIpAddress(agentSessionId, ipAddress, mgiContext);
		}

		public Response LookupNpsTerminalByLocationID(long agentSessionId, long locationId, MGIContext mgiContext)
		{
			return DesktopService.LookupNpsTerminalByLocationID(agentSessionId, locationId, mgiContext);
		}

		public Response LookupNpsTerminalByName(long agentSessionId, string Name, ChannelPartner channelPartner, MGIContext mgiContext)
		{
			return DesktopService.LookupNpsTerminalByName(agentSessionId, Name, channelPartner, mgiContext);
		}

		#endregion

		#region Terminal Setup

		public Response LookupTerminal(long Id, MGIContext mgiContext)
		{
			return DesktopService.LookupTerminalByGuid(Id, mgiContext);
		}

		public Response LookupTerminal(long agentSessionId, string terminalName, int channelPartnerId, MGIContext mgiContext)
		{
			return DesktopService.LookupTerminalByChannelPartner(agentSessionId, terminalName, channelPartnerId, mgiContext);
		}

		public Response CreateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
		{
			return DesktopService.CreateTerminal(agentSessionId, terminal, mgiContext);
		}

		public Response UpdateTerminal(long agentSessionId, Terminal terminal, MGIContext mgiContext)
		{
			return DesktopService.UpdateTerminal(agentSessionId, terminal, mgiContext);
		}
		#endregion

		#region Cash

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <returns></returns>
		public long CashIn(long customerSessionId, decimal amount, MGIContext mgiContext)
		{
			return DesktopService.CashIn(customerSessionId, amount, mgiContext);
		}
		/// <param name="cxeTxnId"></param>
		/// <returns></returns>


		/// <summary>
		/// Cancel CashIn
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="cxeTrxnId"></param>
		public void CancelCashIn(long customerSessionId, long cashInId)
		{
			DesktopService.RemoveCashIn(customerSessionId, cashInId, new MGIContext());
		}

		#endregion

		#region Transaction History

		public Response GetTransactionHistory(long customerSessionId, long customerId, string transactionType, string location, DateTime dateRange, MGIContext mgiContext)
		{
			Response response = new Response();
			response = DesktopService.GetTransactionHistory(customerSessionId, customerId, transactionType, location, dateRange, mgiContext);
			return response;
		}

		public Response GetTransactionHistory(long agentSessionId, long? agentId, string transactionType, string location, bool showAll, long transactionId, int duration, MGIContext mgiContext)
		{
			Response response = new Response();
			response = DesktopService.GetAgentTransactionHistory(agentSessionId, agentId, transactionType, location, showAll, transactionId, duration, mgiContext);
			return response;
		}

		public Response GetFundTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetFundTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
			return response;
		}

		public Response GetCheckTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response =  DesktopService.GetCheckTranasactionDetails(agentSessionId, customerSessionId, transactionId.ToString(), mgiContext);
			return response;
		}

		public Response GetMoneyOrderTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			response = DesktopService.GetMoneyOrderTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
			return response;
		}

		public Response GetMoneyTransferTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			response = DesktopService.GetMoneyTransferTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
			return response;
		}

		public Response GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			response = DesktopService.GetBillPayTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
			return response;
		}

		public Response GetCashTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			response = DesktopService.GetCashTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
			return response;
		}

		public Response GetFundReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			return DesktopService.GetFundsReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
		}

		public Response GetCheckReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			return DesktopService.GetCheckReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
		}

		public Response GetBillPayReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			return DesktopService.GetBillPayReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
		}

		public Response GetSummaryReceiptForReprint(long agentSessionId, long customerSessionId, long transactionId, string transactiontype, MGIContext mgiContext)
		{
			return DesktopService.GetSummaryReceiptForReprint(agentSessionId, customerSessionId, transactionId, transactiontype, mgiContext);
		}

		public Response GetMoneyOrderReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			return DesktopService.GetMoneyOrderReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
		}

		public Response GetMoneyTransferReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			return DesktopService.GetMoneyTransferReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
		}

		public Response GetDoddfrankReceipt(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopService.GetDoddFrankReceipt(agentSessionId, customerSessionId, transactionId, mgiContext);
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Added method to get Coupon Code Receipt 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <returns></returns>
		public Response GetCouponCodeReceipt(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopService.GetCouponCodeReceipt(customerSessionId, mgiContext);
		}

        public Response GetCustomer(string customerSessionId, long alloyId, MGIContext mgiContext)
        {
             Response response = DesktopService.Lookup(long.Parse(customerSessionId), alloyId, mgiContext);
             return response;
        }

		public Response GetCheckFrankData(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
				Response response =  DesktopService.GetCheckFrankingData(customerSessionId, transactionId, mgiContext);
				return response;
		}

		public void UpdateCheckTransactionFranked(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			try
			{


				DesktopService.UpdateTransactionFranked(customerSessionId, transactionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
			catch (Exception ex)
			{
				throw new Exception(ExceptionHandler.GetExceptionMessage(ex));
			}

		}

		#endregion

		#region Private method
		private long GetCustomerSessionId(string customerSessionId)
		{
			//need to change the customer session id to long everywhere. This will be a change in InitiateCustomerSession as well
			//currently it returns string which needs to be changed.
			long custSessionId = 0;
			long.TryParse(customerSessionId, out custSessionId);
			return custSessionId;
		}

		#endregion

		#region MONEY ORDER RELATED METHODS
		public Response GetMoneyOrderFee(long customerSessionId, MoneyOrderData moneyOrderData, MGIContext mgiContext)
		{

			Response response = DesktopService.GetMoneyOrderFee(customerSessionId, moneyOrderData, mgiContext);
			return response;
		}

		public Response PurchaseMoneyOrder(long customerSessionId, MoneyOrderPurchase moneyOrderPurchase, MGIContext mgiContext)
		{
			Response response = new Response();
			response = DesktopService.PurchaseMoneyOrder(customerSessionId, moneyOrderPurchase, mgiContext);
			return response;
		}

		public Response UpdateMoneyOrder(long customerSessionId, MoneyOrderTransaction moneyOrderTransaction, long moneyOrderId, MGIContext mgiContext)
		{

			Response response = DesktopService.UpdateMoneyOrder(customerSessionId, moneyOrderTransaction, moneyOrderId, mgiContext);
			return response;

		}

		public Response UpdateMoneyOrderStatus(long customerSessionId, long moneyOrderId, int newMoneyOrderStatus, MGIContext mgiContext)
		{
			Response response = new Response();
			DesktopService.UpdateMoneyOrderStatus(customerSessionId, moneyOrderId, newMoneyOrderStatus, mgiContext);
			return response;
		}

		public Response GenerateCheckPrintForMoneyOrder(long moneyOrderId, long customerSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			response = DesktopService.GenerateCheckPrintForMoneyOrder(moneyOrderId, customerSessionId, mgiContext);
			return response;
		}

		public Response GenerateMoneyOrderDiagnostics(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			response = DesktopService.GenerateMoneyOrderDiagnostics(agentSessionId, mgiContext);
			return response;
		}

		public Response GetMoneyOrderStage(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{

			Response response = DesktopService.GetMoneyOrderStage(customerSessionId, moneyOrderId, mgiContext);
			return response;
		}

		#endregion

        public Response GetAnonymousUserPAN(long customerSessionId, long channelPartnerId, MGIContext mgiContext)
        {
             Response response = DesktopService.GetAnonymousUserPAN(customerSessionId, channelPartnerId, mgiContext);
             return response;
        }

		public decimal GetBillPayFee(long customerSessionId, string providerName, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopService.GetBillPayFee(customerSessionId, providerName, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
			decimal result = Convert.ToDecimal(response.Result);
			return result;
		}

		public IList<SelectListItem> GetDefaultList()
		{
			IList<SelectListItem> defaultList = new List<SelectListItem>();
			defaultList.Add(DefaultListItem());
			return defaultList;
		}

		public Response IsSWBStateXfer(long customerSessionId, string locationState, MGIContext mgiContext)
		{
			Response response = DesktopService.IsSWBStateXfer(customerSessionId, locationState, mgiContext);
			return response;
		}

		public Response GetAgentXfer(long agentSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetAgentXfer(agentSessionId, mgiContext);
			return response;
		}

		public CashierDetails GetAgent(long agentSessionId, MGIContext mgiContext)
		{
			Response response = new Response();
			try
			{
				response = DesktopService.GetAgent(agentSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
			catch (Exception ex)
			{
				throw ex;
			}
			CashierDetails result = response.Result as CashierDetails;
			return result;
		}

		public void PostFlush(long customerSessionId, MGIContext mgiContext)
		{
			try
			{
				DesktopService.PostFlush(customerSessionId, mgiContext);
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				throw new Exception(ExceptionHandler.GetSOAPExceptionMessage(nexxoFault));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Response GetProfilestatus(long agentSessionId, MGIContext mgiContext)
		{
			List<SelectListItem> profileStatus = new List<SelectListItem>();

			Response response = DesktopService.ProfileStatus(agentSessionId, mgiContext);
			if (response.Error != null)
				return response;
            List<string> Status = response.Result as List<string>;
			foreach (var val in Status)
			{
				profileStatus.Add(new SelectListItem() { Text = val, Value = val });
			}
			response.Result = profileStatus;
			return response;			
		}

		public Response GetCheckLogin(long customerSessionId, MGIContext mgiContext)
		{
		
			Response response =  DesktopService.GetCheckSession(customerSessionId, mgiContext);
			return response;
			
		}

		public Response RemoveCheckFromCart(long customersessionId, long checkId, MGIContext mgiContext)
		{
			return DesktopService.RemoveCheckFromCart(customersessionId, checkId, mgiContext);
		}

		public Response GetCheckDeclinedReceiptData(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopService.GetCheckDeclinedReceiptData(agentSessionId, customerSessionId, transactionId, mgiContext);
		}

		public Response GetPrepaidActions(string cardStatus)
		{
			Response response = new Response();
			try
			{
				List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

				switch (cardStatus)
				{
					case "active":
						items.Add(new KeyValuePair<string, string>("Report Lost & Replace", "5"));
						items.Add(new KeyValuePair<string, string>("Report Stolen & Replace", "6"));
						items.Add(new KeyValuePair<string, string>("Suspend Card(Do not replace)", "3"));
						items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
						break;
					case "suspended":
						items.Add(new KeyValuePair<string, string>("Activate Card", "0"));
						items.Add(new KeyValuePair<string, string>("Report Lost & Replace", "5"));
						items.Add(new KeyValuePair<string, string>("Report Stolen & Replace", "6"));
						items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
						break;
					case "cardissued":
						items.Add(new KeyValuePair<string, string>("Report Lost & Replace", "5"));
						items.Add(new KeyValuePair<string, string>("Report Stolen & Replace", "6"));
						items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
						break;
					case "lostcard":
						items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
						break;
					case "stolencard":
						items.Add(new KeyValuePair<string, string>("Replace Card", "14"));
						break;
				}

				response.Result = items;
				return response;
			}
			catch (FaultException<NexxoSOAPFault> nexxoFault)
			{
				Error error = new Error()
				{
					MajorCode = nexxoFault.Detail.MajorCode,
					MinorCode = nexxoFault.Detail.MinorCode,
					Processor = nexxoFault.Detail.Processor,
					Details = nexxoFault.Message
				};

				return response;
			}
		}

		public Response GetShippingTypes(long customerSessionId, MGIContext mgiContext)
		{
			Response response = DesktopService.GetShippingTypes(customerSessionId, mgiContext);
			List<ShippingTypes> shippingTypes = null;

			if (response.Result != null)
				shippingTypes = response.Result as List<ShippingTypes>;
			else
				return response;

			List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

			foreach (var shippingType in shippingTypes)
			{
				items.Add(new KeyValuePair<string, string>(shippingType.Name, shippingType.Code));
			}
			response.Result = items;

			return response;
		}
		public Response GetShippingFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			Response response = DesktopService.GetShippingFee(customerSessionId, cardMaintenanceInfo, mgiContext);
			return response;
		}

		public Response AssociateCard(long customerSessionId, FundsProcessorAccount fundsProcessorAccount, MGIContext mgiContext)
		{
			Response response = DesktopService.AssociateCard(customerSessionId, fundsProcessorAccount, mgiContext);
			return response;
		}

		public Response GetFundFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			Response response = DesktopService.GetFundFee(customerSessionId, cardMaintenanceInfo, mgiContext);
			return response;
		}
		public Response RequestAddOnCard(long customerSessionId, Funds fund, MGIContext mgiContext)
		{
			Response response = DesktopService.IssueAddOnCard(customerSessionId, fund, mgiContext);
			return response;

		}

		public Response GetMessage(long agentSessionId, string messageKey, MGIContext mgiContext)
		{
			Response response = DesktopService.GetMessage(agentSessionId,messageKey, mgiContext);
			return response;
		}
	}
}


public enum UserRoles
{
	[Description("Teller")]
	Teller = 1,
	[Description("Manager")]
	Manager = 2,
	[Description("Compliance Manager")]
	ComplianceManager = 3,
	[Description("System Admin")]
	SystemAdmin = 4,
	[Description("Tech")]
	Tech = 5
}

public enum TerminalIdentificationMechanism
{
	YubiKey = 1,
	Cookie,
	HostName
}

public class MngUserSearch
{

	public MngUserSearch(string firstname, string lastname, string status, string location)
	{
		this.FirstName = firstname;
		this.LastName = lastname;
		this.Status = status;
		this.Location = location;
	}
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Status { get; set; }
	public string Location { get; set; }
}

public class SearchReceiver
{
	public string ReceiverID { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Status { get; set; }
	public string Country { get; set; }
	public string State { get; set; }
	public string City { get; set; }
}

public class DoddFrankConfirmationClient
{
	public string ReceiverId { get; set; }

	public string ReceiverName { get; set; }

	public string PickupLocation { get; set; }

	public string PickupMethod { get; set; }

	public string PickupOptions { get; set; }

	public string CurrencyType { get; set; }

	public int TransferFee { get; set; }

	public int TransferTax { get; set; }

	public int TotalAmount { get; set; }

	public int ExchangeRate { get; set; }

	public int TransferAmount { get; set; }

	public int OtherFees { get; set; }

	public int OtherTaxes { get; set; }

	public int TotalToRecipient { get; set; }
}

public class GPRCardDefaults
{
	public decimal ActivationFee { set; get; }

	public decimal LoadFee { get; set; }

	public decimal WithdrawFee { get; set; }

	public decimal MinimumLoad { get; set; }

	public decimal MaximumLoad { get; set; }

	public decimal MinimumWithdraw { get; set; }

	public decimal MaximumWithdraw { get; set; }
}

public class Department
{
	public int DepartmentId { get; set; }

	public string DepartmentName { get; set; }

	public bool IsActive { get; set; }
}

public class ManageUser
{
	public string LastName { get; set; }
	public string FirstName { get; set; }
	public string UserName { get; set; }
	public string PrimaryLocation { get; set; }
	public string Department { get; set; }
	public string Manager { get; set; }
	public string UserRole { get; set; }
	public string UserStatus { get; set; }
	public string Phone { get; set; }
	public string Email { get; set; }
	public string TempPassword { get; set; }
}

public class Location : NexxoModel
{
	// public int LocationId { get; set; }
	public string LocationType { get; set; }

	public string ParentLocation { get; set; }

	public string LocationName { get; set; }

	public string LocationStatus { get; set; }

	public string Address1 { get; set; }

	public string Address2 { get; set; }

	public string City { get; set; }

	public string State { get; set; }

	public string ZipCode { get; set; }
}

public class SetupLocationType
{
	public string LocationType { get; set; }

	public bool Enabled { get; set; }
}


