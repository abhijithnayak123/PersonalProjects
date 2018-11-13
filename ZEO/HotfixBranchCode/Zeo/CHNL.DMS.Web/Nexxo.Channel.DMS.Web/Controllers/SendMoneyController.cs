using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MGI.Channel.DMS.Web.Controllers
{
	public class SendMoneyController : BaseController
	{
		#region  Manage Receivers

		public ActionResult AddEditReceiver(string type)
		{
			MGIContext mgiContext = new MGIContext();
			Desktop client = new Desktop();
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
			InitializeDropdownlistValues(receiver, client, mgiContext);
			return View("SendMoneyReceiver", receiver);
		}

		[HttpPost]
		public ActionResult AddEditReceiver(SendMoneyReceiver sendMoneyReceiver)
		{
			Desktop client = new Desktop();
			Receiver clientReceiver = new Receiver();
			long responseReceiverID = 0;

			MGIContext mgiContext = new MGIContext();
			InitializeDropdownlistValues(sendMoneyReceiver, client, mgiContext);

			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			if (sendMoneyReceiver.ReceiverId == 0)
			{
				clientReceiver = SetReceivers(sendMoneyReceiver);
				try
				{
                    responseReceiverID = client.SaveReceiver(long.Parse(customerSession.CustomerSessionId), clientReceiver, mgiContext);
				}
				catch (Exception ex)
				{
					ViewBag.IsException = true;
					ViewBag.ExceptionMsg = HttpUtility.JavaScriptStringEncode(ex.Message);
					sendMoneyReceiver.AddEdit = "Add";
					return View("SendMoneyReceiver", sendMoneyReceiver);
				}
			}
			else
			{
				clientReceiver = SetReceivers(sendMoneyReceiver);
				try
				{
					responseReceiverID = client.UpdateReceiver(Convert.ToInt64(customerSession.CustomerSessionId), clientReceiver, mgiContext);
				}
				catch (Exception ex)
				{
					ViewBag.IsException = true;
					ViewBag.ExceptionMsg = HttpUtility.JavaScriptStringEncode(ex.Message);
					return View("SendMoneyReceiver", sendMoneyReceiver);
				}
			}

			TempData["ReceiverId"] = responseReceiverID;
			return RedirectToAction("SendMoney", "SendMoney");
		}

		private void InitializeDropdownlistValues(SendMoneyReceiver receiver, Desktop client, MGIContext mgiContext)
		{
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

            long customerSessionId = GetCustomerSessionId();

            receiver.LPickUpCountry = client.GetXfrCountries(customerSessionId, mgiContext);


			if (Session["Receiver"] != null)
			{
				//Edit
				if (string.IsNullOrWhiteSpace(receiver.PickUpCountry))
				{
					receiver.LPickUpState = client.DefaultSelectList();
					receiver.LPickUpCity = client.DefaultSelectList();
				}
				else
				{
                    receiver.LPickUpState = client.GetXfrStates(customerSessionId, receiver.PickUpCountry, mgiContext);
					if (string.IsNullOrWhiteSpace(receiver.PickUpState))
					{
						receiver.LPickUpCity = client.DefaultSelectList();
					}
					else
					{
                        receiver.LPickUpCity = client.GetXfrCities(customerSessionId, receiver.PickUpState, mgiContext);
					}
				}
				//Get the CurrencyCode
				string currencyCode = string.Empty;
				if (!string.IsNullOrEmpty(receiver.PickUpCountry))
                    currencyCode = client.GetCurrencyCode(customerSessionId, receiver.PickUpCountry, mgiContext);

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
					receiver.LDeliveryMethods = PopulateDeliveryServices(DeliveryServiceType.Method, receiver.PickUpCountry, currencyCode, stateName, stateCode, mgiContext, cityName);

					string svcCode = "";
					if (receiver.DeliveryMethod != null && receiver.LDeliveryMethods.Count() > 1)
					{
						SelectListItem selectedDeliveryMethod = receiver.LDeliveryMethods.Where(dm => dm.Value == receiver.DeliveryMethod).FirstOrDefault();
						if (selectedDeliveryMethod != null)
							svcCode = selectedDeliveryMethod.Value;
					}

					mgiContext.SVCCode = svcCode;
					receiver.LDeliveryOptions = PopulateDeliveryServices(DeliveryServiceType.Option, receiver.PickUpCountry, currencyCode, stateName, stateCode, mgiContext, "", svcCode);
				}
				else
				{
					receiver.LDeliveryMethods = client.DefaultSelectList();
					receiver.LDeliveryOptions = client.DefaultSelectList();
				}
			}
			else
			{
				//Add
                receiver.LPickUpState = client.GetXfrStates(customerSessionId, "", mgiContext);
                receiver.LPickUpCity = client.GetXfrCities(customerSessionId, "", mgiContext);
				receiver.LDeliveryMethods = client.DefaultSelectListItem();
				receiver.LDeliveryOptions = client.DefaultSelectListItem();
			}
		}

		public ActionResult EditReceiver()
		{
			Desktop client = new Desktop();
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

		[HttpPost]
		public ActionResult SearchReceiverGrid(string searchTerm, int page = 1, int rows = 5)
		{
			//string searchTerm = "kk";
			// Display jqGrid with Receiver details
			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			Receiver receiver = new Receiver();
			SendMoney sendmoney = new SendMoney();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			if (searchTerm == null) { searchTerm = ""; };

			try
			{
				var receivers = new List<Receiver>().AsQueryable();
				List<Receiver> receiverList = desktop.GetReceivers(long.Parse(customerSession.CustomerSessionId), searchTerm, mgiContext);

				if (receiverList != null)
				{
					receivers = receiverList.AsQueryable();
				}

				IQueryable<Receiver> filteredreceivers = receivers;
				var sortedReceivers = filteredreceivers;      // SortIQueryable<Customer>(filteredCustomers, sidx, sord);
				var totalRecords = filteredreceivers.Count();

				var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

				var data = (from s in sortedReceivers
							select new
							{
								id = s.Id,
								cell = new object[] 
                                { 
                                    s.Id,
                                    s.FirstName,
                                    s.LastName, 
                                    s.Status, 
                                    s.PickupCountry, 
                                    s.PickupState_Province,
                                    s.PickupCity
                                }
							}).ToArray();

				var jsonData = new
				{
					total = totalPages,
					page = page,
					records = totalRecords,
					rows = data.Skip((page - 1) * rows).Take(rows)
				};

				return Json(jsonData);
			}
			catch (SystemException ex)
			{
				throw ex;
			}
		}

		public ActionResult ReceiverSearch(string LastName)
		{
			return View("EditReceiver");
		}

		//need to cheat the cache as popup shows up with old data   , string cheatDate 
		public ActionResult GetReceiverForEdit(string ReceiverId)
		{
			Desktop client = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			try
			{
				if (ReceiverId != "0")
				{
					Receiver editReceiverDetails = client.GetReceiverDetails(Convert.ToInt64(customerSession.CustomerSessionId), long.Parse(ReceiverId), mgiContext);

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
					InitializeDropdownlistValues(receiver, client, mgiContext);
					return RedirectToAction("AddEditReceiver", "SendMoney", new { type = "Edit" });
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
				return null;
			}
		}

		public ActionResult SendMoney()
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop client = new Desktop();

			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			long receiverId = (long)TempData["ReceiverId"];
			TempData.Keep("ReceiverId");
			SendMoney sendMoney = GetReceiverDetails(receiverId, mgiContext);

			InitializeDropdownsforSendMoney(sendMoney, client);
			ViewBag.Navigation = NexxoSiteMap.SendMoney;

			return View("SendMoney", "_Common", sendMoney);
		}

		public JsonResult ReceiverByFullName(string fullName)
		{
			Desktop deskTop = new Desktop();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			Receiver receiver = deskTop.GetReceiverByFullName(Convert.ToInt64(customerSession.CustomerSessionId), fullName, mgiContext);
			long receiverId = receiver == null ? 0 : receiver.Id;
			return Json(receiverId, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="CountryId"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult GetStates(string CountryId)
		{
			Desktop uClient = new Desktop();
			List<SelectListItem> States = new List<SelectListItem>();
			ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;
            MGIContext mgiContext = new MGIContext();
            States = uClient.GetStates(GetAgentSessionId(), channelPartner.Id, mgiContext, CountryId);
			return Json(States, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// This method auto-polulates the receiver details on click of 
		/// any of the Frequent Receivers links.
		/// </summary>
		/// <param name="ReceiverID"></param>
		/// <returns>ActionResult</returns>

		public ActionResult SelectReceiver(long ReceiverID)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			ViewBag.Navigation = NexxoSiteMap.SendMoney;
			return View("SendMoney", "_Common", GetReceiverDetails(ReceiverID, mgiContext));
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
			Desktop client = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];	
		
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			mgiContext.ProductType = "sendmoney";
			mgiContext.AgentId = (int)Session["agentId"];

			long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

			client.DeleteFavoriteReceiver(customerSessionId, Convert.ToInt64(receiverId), mgiContext);

			SendMoney sendMoney = new SendMoney();
			sendMoney.FrequentReceivers = client.GetFrequentReceivers(long.Parse(customerSession.CustomerSessionId), mgiContext);
			return PartialView("_FrequentReceivers", sendMoney);
		}

        /// <summary>
        /// Checks the notification details.
        /// </summary>
        /// <param name="receiverId">The receiver identifier.</param>
        /// <returns></returns>
        public JsonResult CheckNotificationDetails(long receiverId)
        {

            Desktop desktop = new Desktop();
            long customerSessionId = GetCustomerSessionId();
            Receiver receiver = new Receiver();
            SendMoney sendMoney = new SendMoney();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            receiver = desktop.GetReceiverDetails(Convert.ToInt64(customerSessionId), receiverId, mgiContext);
            int isReceiverContactInfoRequired = 0;
                        
                if (string.IsNullOrEmpty(receiver.Address) || string.IsNullOrEmpty(receiver.City) ||string.IsNullOrEmpty(receiver.ZipCode) 
                    ||string.IsNullOrEmpty(receiver.State_Province) ||string.IsNullOrEmpty(receiver.PhoneNumber))
                {
                    isReceiverContactInfoRequired = 1;
                }
            

            return Json(isReceiverContactInfoRequired, JsonRequestBehavior.AllowGet);
        }

		/// <summary>
		/// This method does the amount limit validations on the 
		/// model object & displays the confirm pick-up options screen.
		/// </summary>
		/// <param name="ReceiverID"></param>
		/// <returns>ActionResult</returns>
		[HttpPost]
		[CustomHandleError(ViewName = "SendMoney", MasterName = "_Common")]
		public ActionResult Validate(SendMoney sendMoney)
		{
			Desktop client = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
            long customerSessionId = GetCustomerSessionId();
            MGIContext mgiContext = new MGIContext();

            sendMoney.LCountry = client.GetXfrCountries(customerSessionId, mgiContext);
            sendMoney.LStates = client.GetXfrStates(customerSessionId, sendMoney.Country ?? string.Empty, mgiContext);
            sendMoney.LCities = client.GetXfrCities(customerSessionId, sendMoney.StateProvinceCode ?? string.Empty, mgiContext);

			//Get the CurrencyCode
			string currencyCode = string.Empty;
			if (!string.IsNullOrEmpty(sendMoney.Country))
                currencyCode = client.GetCurrencyCode(customerSessionId, sendMoney.Country, mgiContext);
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

			sendMoney.LActOnMyBehalf = client.GetActBeHalfList();

			stateCode = string.IsNullOrEmpty(sendMoney.StateProvince) ? string.Empty : sendMoney.StateProvince;

			ModelState.Remove("TotalAmount");
			ModelState.Remove("PickUpMethodId");
			ModelState.Remove("DestinationAmountFromFeeEnquiry");
			ModelState.Remove("TransferAmount");
			ModelState.Remove("TotalToRecipient");
			ModelState.Remove("PickUpOptionsId");

			sendMoney.FrequentReceivers = client.GetFrequentReceivers(long.Parse(sendMoney.customerSession.CustomerSessionId), mgiContext);
			sendMoney.enableEditContinue = true;

			if (sendMoney.ReceiverID == 0)
			{
				ModelState.Remove("DestinationAmountWithCurrency");
			}

			if (sendMoney.ReceiverID == 0)
			{
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

				InitializeDropdownlistValues(receiver, client, mgiContext);

				Session["Receiver"] = receiver;
				return RedirectToAction("AddEditReceiver", "SendMoney", new { type = "add" });
			}

			if (ModelState.IsValid)
			{
                //context.Add("OriginatingCountryCode", "US");
                //context.Add("OriginatingCurrencyCode", "USD");
                //context.Add("DestinationCountryCode", sendMoney.Country);
                //context.Add("DestinationCurrencyCode", currencyCode);

                //context.Add("StateName", stateName);
                //context.Add("StateCode", stateCode);
                //context.Add("CityName", cityName);

                //context.Add("SVCCode", sendMoney.DeliveryMethod);
                mgiContext.OriginatingCountryCode = "US";
                mgiContext.OriginatingCurrencyCode="USD";
                mgiContext.DestinationCountryCode = sendMoney.Country;
                mgiContext.DestinationCurrencyCode = currencyCode;
                mgiContext.StateName = stateName;
                mgiContext.StateCode = stateCode;
                mgiContext.CityName = cityName;

				sendMoney.LDelivertyMethods = PopulateDeliveryServices(DeliveryServiceType.Method, sendMoney.Country, currencyCode, stateName, stateCode, mgiContext, cityName);
				sendMoney.LDeliveryOptions = PopulateDeliveryServices(DeliveryServiceType.Option, sendMoney.Country, currencyCode, stateName, stateCode, mgiContext, cityName, sendMoney.DeliveryMethod);

				sendMoney.FrequentReceivers = client.GetFrequentReceivers(long.Parse(sendMoney.customerSession.CustomerSessionId), mgiContext);
                sendMoney.LCountryCurrencies = client.GetCurrencyCodeList(customerSessionId, sendMoney.Country, mgiContext);
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

				sendMoney = GetFee(sendMoney, mgiContext);

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
					ViewBag.ExceptionMsg = HttpUtility.JavaScriptStringEncode(messages);
				}

				return View("SendMoney", "_Common", sendMoney);
			}
		}

        [HttpPost]
        [CustomHandleError(ViewName = "SendMoneyDetails", MasterName = "_Common")]
        public ActionResult SendMoneyDetails(SendMoney sendMoney, string updateTrans, string next, string lpmtcontinue)
        {
            Desktop client = new Desktop();
            ModelState.Remove("Amount");
            string stateName = "";
            string cityName = "";
            string stateCode = "";
            string countryName = "";
            long customerSessionId = GetCustomerSessionId();
            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
            //Checking if Terms and condition pop-up is enabled based on provider and Product for a ChannelPartner
            bool isTncRequired = false;
            var product = sendMoney.Providers.FirstOrDefault(x => x.ProcessorName == "WesternUnion" && x.Name == "MoneyTransfer");
            if (product != null)
            {
                isTncRequired = product.IsTnCForcePrintRequired;
            }
            MGIContext mgiContext = new MGIContext();

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
                sendMoney.LCountry = client.GetXfrCountries(customerSessionId, mgiContext);
                sendMoney.LStates = client.GetXfrStates(customerSessionId, sendMoney.CountryCode, mgiContext);
                sendMoney.LCities = client.GetXfrCities(customerSessionId, sendMoney.StateProvinceCode ?? string.Empty, mgiContext);
                //Get the CurrencyCode
                string currencyCode = client.GetCurrencyCode(customerSessionId, sendMoney.CountryCode, mgiContext);

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
                mgiContext.OriginatingCountryCode = "US";
                mgiContext.OriginatingCurrencyCode = "USD";
                mgiContext.DestinationCountryCode = sendMoney.Country;
                mgiContext.DestinationCurrencyCode = currencyCode;

                sendMoney.LCountryCurrencies = client.GetCurrencyCodeList(customerSessionId, sendMoney.CountryCode, mgiContext);

                sendMoney.LDelivertyMethods = PopulateDeliveryServices(DeliveryServiceType.Method, sendMoney.Country, currencyCode, stateName, stateCode, mgiContext, cityName);
                sendMoney.LDeliveryOptions = PopulateDeliveryServices(DeliveryServiceType.Option, sendMoney.Country, currencyCode, stateName, stateCode, mgiContext, cityName, sendMoney.DeliveryMethod);
            }
            sendMoney.FrequentReceivers = client.GetFrequentReceivers(long.Parse(sendMoney.customerSession.CustomerSessionId), mgiContext);
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

                Receiver receiver = client.GetReceiverDetails(long.Parse(customerSession.CustomerSessionId), sendMoney.ReceiverID, mgiContext);
                sendMoney.PickUpLocation = receiver.Address;
                if (sendMoney.LCountry.FirstOrDefault(x => x.Value == sendMoney.Country) != null)
                    sendMoney.Country = sendMoney.LCountry.FirstOrDefault(x => x.Value == sendMoney.Country).Text;
                sendMoney.LStates = client.GetXfrStates(customerSessionId, sendMoney.CountryCode, mgiContext);
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

                ValidateRequest validateRequest = new ValidateRequest()
                {
                    TransferType = TransferType.SendMoney,
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
                ValidateResponse response = client.ValidateTransfer(long.Parse(customerSession.CustomerSessionId), validateRequest, mgiContext);

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
                try
                {
                    if (sendMoney.StateProvince != null && sendMoney.LStates.Count() != 1)
                    {
                        SelectListItem selectedState = sendMoney.LStates.Where(st => st.Value == sendMoney.StateProvince).FirstOrDefault();
                        if (selectedState != null)
                            sendMoney.StateName = selectedState.Text;
                    }
                    sendMoney = GetFee(sendMoney, mgiContext);
                }
                catch (Exception ex)
                {
                    //Below this line Send Money details page if they entered invalid promocode will go to catch block and executed viewbag values in js file.
                    if (ex.Message.Contains("3006"))
                    {
                        ViewBag.invalidPromoCode = "T3006";
                        throw ex;
                    }
                }

            }

            //clear the view data
            ViewData = new ViewDataDictionary();

            return View("SendMoneyDetails", sendMoney);
        }

	
		[HttpPost]
		[CustomHandleError(ViewName = "SendMoneyConfirm", MasterName = "_Common")]
		public ActionResult SendMoneyConfirm(SendMoney sendMoney)
		{
            //Checking if Terms and condition pop-up is enabled based on provider and Product for a ChannelPartner
            bool isTncRequired = false;

			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
            var  product = sendMoney.Providers.FirstOrDefault(x => x.ProcessorName == "WesternUnion" && x.Name == "MoneyTransfer");
             if (product != null)
             {
                 isTncRequired = product.IsTnCForcePrintRequired;
             }
           			
			if (sendMoney.ProvidedTermsandConditonsMessage && sendMoney.ConsumerProtectionMessage && (sendMoney.isDomesticTransferVal == "true" || (sendMoney.isDomesticTransferVal == "false" && sendMoney.DoddFrankDisclosure)))
			{
				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

				ShoppingCartItemModel shoppingCartItemModel = new ShoppingCartItemModel();

				if (TempData["sendMoneyModifyIds"] != null)
				{
					ModifySendMoneyResponse sendMoneyModifyIds = (ModifySendMoneyResponse)TempData["sendMoneyModifyIds"];

					Desktop desktop = new Desktop();

					MoneyTransferTransaction modifiedTrx = desktop.GetMoneyTransferDetailsTransaction(Convert.ToInt64(customerSession.CustomerSessionId), sendMoneyModifyIds.CancelTransactionId, mgiContext);

					ModifySendMoneyRequest request = new ModifySendMoneyRequest() { CancelTransactionId = sendMoneyModifyIds.CancelTransactionId, ModifyTransactionId = sendMoneyModifyIds.ModifyTransactionId };

					ModifySendMoneyResponse AuthorizedsendMoneyModifyIds = desktop.AuthorizeSendMoneyModify(Convert.ToInt64(customerSession.CustomerSessionId), request, mgiContext);
					TempData["sendMoneyModifyIds"] = null;

					return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
				}

				return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
			}
			else
			{
				//ViewBag.ErrorMessage = "Please select all the checkboxes";
				ViewBag.isTnCForcePrintRequired = isTncRequired;
				ViewBag.Navigation = NexxoSiteMap.TermsAndConditions;
				return View("SendMoneyConfirm", sendMoney);
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
		public JsonResult AutoCompleteReceiver(string term)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop deskTop = new Desktop();
			List<Receiver> ReceiverList = new List<Receiver>();
			List<string> namesList = new List<string>();

			try
			{
				CheckAmount getCustomerSessionId = new CheckAmount();

				ReceiverList = deskTop.GetReceivers(long.Parse(getCustomerSessionId.customerSession.CustomerSessionId), term, mgiContext);
				ReceiverList = ReceiverList.FindAll(c => c.Status == "Active");
				// In the auto-populate list, show firstnames for "Active" receivers that contain the searchterm.
				foreach (var receiver in ReceiverList)
				{
					namesList.Add(receiver.FirstName + " " + receiver.LastName);
				}
				return Json(namesList, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return Json(namesList, JsonRequestBehavior.AllowGet);
			}
		}
 
		public JsonResult PopulateReceiverDetails(long ReceiverId)
		{
			Desktop desktop = new Desktop();

			SendMoney sendMoney = new SendMoney();
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			try
			{
				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

				sendMoney.FrequentReceivers = desktop.GetFrequentReceivers(long.Parse(customerSession.CustomerSessionId), mgiContext);

				if (ReceiverId != 0)
				{
					try
					{
						sendMoney = GetReceiverDetails(ReceiverId, mgiContext);
                        List<SelectListItem> cities = desktop.GetXfrCities(long.Parse(customerSession.CustomerSessionId), sendMoney.StateProvinceCode, mgiContext);

						SelectListItem cityItem = cities.Where(c => c.Text == sendMoney.City).FirstOrDefault();

						if (cityItem != null)
						{
							sendMoney.CityName = cityItem.Text;
						}
					}
					catch (Exception ex)
					{	
						throw ex;
					}

				}
				return Json(sendMoney, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				throw ex;
			}
		}

		public JsonResult WUStates(string countryCode)
		{
            long customerSessionId = GetCustomerSessionId();
			List<SelectListItem> states = new List<SelectListItem>();
			Desktop client = new Desktop();
            MGIContext mgiContext = new MGIContext();
            states = client.GetXfrStates(customerSessionId, countryCode, mgiContext);
			return Json(states, JsonRequestBehavior.AllowGet);
		}

		public JsonResult WUCountryCurrencyCode(string countryCode)
		{
			Desktop client = new Desktop();
            long customerSessionId = GetCustomerSessionId();
            MGIContext mgiContext = new MGIContext();
            string currenyCode = client.GetCurrencyCode(customerSessionId, countryCode, mgiContext);
			return Json(currenyCode, JsonRequestBehavior.AllowGet);
		}

		public JsonResult WUCities(string stateCode)
		{
            long customerSessionId = GetCustomerSessionId();
			List<SelectListItem> cities = new List<SelectListItem>();
			Desktop client = new Desktop();
            MGIContext mgiContext = new MGIContext();
            cities = client.GetXfrCities(customerSessionId, stateCode, mgiContext);
			return Json(cities, JsonRequestBehavior.AllowGet);
		}

		public JsonResult WUDeliveryMethods(string countryCode, string state, string stateCode, string city)
		{
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			IEnumerable<SelectListItem> deliveryMethods = new List<SelectListItem>();
			Desktop client = new Desktop();
            MGIContext mgiContext = new MGIContext();
			//Get the CurrencyCode
            string currencyCode = client.GetCurrencyCode(long.Parse(customerSession.CustomerSessionId), countryCode, mgiContext);
           
            mgiContext.OriginatingCountryCode = "US";
            mgiContext.OriginatingCurrencyCode = "USD";
            mgiContext.DestinationCountryCode = countryCode;
            mgiContext.DestinationCurrencyCode = currencyCode;
            mgiContext.StateName = state;
            mgiContext.StateCode = stateCode;
            mgiContext.CityName = city;

			if (!countryCode.ToLower().Equals("select"))
			{
				try
				{
					deliveryMethods = PopulateDeliveryServices(DeliveryServiceType.Method, countryCode, currencyCode, state, stateCode, mgiContext, city);
				}
				catch (Exception ex)
				{
					ViewBag.ErrorMessage = ex.Message;
					throw ex;
				}
			}

			return Json(deliveryMethods, JsonRequestBehavior.AllowGet);
		}

		public JsonResult WUDeliveryOptions(string countryCode, string state, string city, string svcCode)
		{
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			IEnumerable<SelectListItem> deliveryOptions = new List<SelectListItem>();
			Desktop client = new Desktop();
            MGIContext mgiContext = new MGIContext();
			//Get the Country CurrencyCode
            string currencyCode = client.GetCurrencyCode(long.Parse(customerSession.CustomerSessionId), countryCode, mgiContext);

            mgiContext.OriginatingCountryCode = "US";
            mgiContext.OriginatingCurrencyCode = "USD";
            mgiContext.DestinationCountryCode = countryCode;
            mgiContext.DestinationCurrencyCode = currencyCode;
            mgiContext.StateName = state;
            mgiContext.SVCCode = svcCode;
            mgiContext.CityName = city;

			if (!countryCode.ToLower().Equals("select"))
			{
				deliveryOptions = PopulateDeliveryServices(DeliveryServiceType.Option, countryCode, currencyCode, state, string.Empty, mgiContext, city, svcCode);
			}

			return Json(deliveryOptions, JsonRequestBehavior.AllowGet);
		}

		public ActionResult SaveReceiverConfirmation()
		{
			return PartialView("_SaveReceiverConfirmationMessage");
		}

		public ActionResult CreateReceiverAddlInfo()
		{
			Desktop client = new Desktop();
			Receivers receivers = new Receivers();
			//InitializeDropdownsforSendMoney(receivers, client);
			return View("AddEditReceiverContinue", "_Common", receivers);

		}

		public void InitializeDropdownsforSendMoney(SendMoney sendMoney, Desktop client)
		{
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
            long customerSessionId = long.Parse(customerSession.CustomerSessionId);
            MGIContext mgiContext = new MGIContext();
            sendMoney.LCountry = client.GetXfrCountries(customerSessionId, mgiContext);

			Desktop desktop = new Desktop();

			//Get the CurrencyCode
            string currencyCode = desktop.GetCurrencyCode(customerSessionId, sendMoney.Country, mgiContext);

			//WUStates
			string stateName = "";
			string cityName = "";
			string stateCode = "";

			if (sendMoney.ReceiverID != 0)
			{
                sendMoney.LStates = client.GetXfrStates(customerSessionId, sendMoney.Country, mgiContext);
                sendMoney.LCities = client.GetXfrCities(customerSessionId, sendMoney.StateProvinceCode, mgiContext);


			}
			else
			{
                sendMoney.LStates = client.GetXfrStates(customerSessionId, "", mgiContext);
                sendMoney.LCities = client.GetXfrCities(customerSessionId, "", mgiContext);
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

            mgiContext.OriginatingCountryCode = "US";
            mgiContext.OriginatingCurrencyCode = "USD";
            mgiContext.DestinationCountryCode = sendMoney.Country;
            mgiContext.DestinationCurrencyCode = currencyCode;
            mgiContext.StateName = stateName;
            mgiContext.StateName = stateCode;
            mgiContext.CityName = cityName;

			sendMoney.LDelivertyMethods = PopulateDeliveryServices(DeliveryServiceType.Method, sendMoney.Country, currencyCode, stateName, stateCode, mgiContext, cityName);
			sendMoney.LActOnMyBehalf = client.GetActBeHalfList();

            mgiContext.SVCCode = sendMoney.DeliveryMethod;
			sendMoney.LDeliveryOptions = PopulateDeliveryServices(DeliveryServiceType.Option, sendMoney.Country, currencyCode, stateName, stateCode, mgiContext, cityName, sendMoney.DeliveryMethod);
		}

		public ActionResult ShowTermsConditonDialog()
		{
			return PartialView("_partialWUTermsAndConditions");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="setReceiver"></param>
		private Receiver SetReceivers(SendMoneyReceiver setReceiver)
		{
			Receiver newReceiver = new Receiver();
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

		private SendMoney GetFee(SendMoney sendMoney, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{

			Desktop client = new Desktop();
			var customerSession = (CustomerSession)Session["CustomerSession"];

			FeeRequest feeRequest = new FeeRequest()
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
				DeliveryService = new DeliveryService()
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

			long customerSessionId = long.Parse(customerSession.CustomerSessionId);
			FeeResponse feeResponse = client.GetMoneyTransferFee(customerSessionId, feeRequest, mgiContext);

			FeeInformation feeInformation = feeResponse.FeeInformations.FirstOrDefault();

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

		private SendMoney GetReceiverDetails(long receiverId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			Desktop desktop = new Desktop();
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
            long customerSessionId = GetCustomerSessionId();
			Receiver receiver = new Receiver();
			SendMoney sendMoney = new SendMoney();
			try
			{
				sendMoney.FrequentReceivers = desktop.GetFrequentReceivers(long.Parse(customerSession.CustomerSessionId), mgiContext);
				receiver = desktop.GetReceiverDetails(Convert.ToInt64(customerSession.CustomerSessionId), receiverId, mgiContext);

				if (receiver != null)
				{
					sendMoney.FirstName = receiver.FirstName;
					sendMoney.LastName = receiver.LastName;
					sendMoney.SecondLastName = receiver.SecondLastName; // has to change to second lastname
					sendMoney.ReceiverName = receiver.FirstName + " " + receiver.LastName;
					sendMoney.ReceiverID = receiver.Id;

					sendMoney.CountryCode = receiver.PickupCountry;
					sendMoney.Country = receiver.PickupCountry;// desktop.WUCountries().Find(c => c.Value == receiver.PickupCountry).Text;
                    sendMoney.CurrencyType = desktop.GetCurrencyCode(customerSessionId, sendMoney.CountryCode, mgiContext);
					if (!string.IsNullOrEmpty(receiver.PickupCountry))
					{
                        sendMoney.LStates = desktop.GetXfrStates(customerSessionId, receiver.PickupCountry, mgiContext);
                        sendMoney.LCities = desktop.GetXfrCities(customerSessionId, receiver.PickupState_Province ?? string.Empty, mgiContext);
					}
					else
					{
						sendMoney.LStates = desktop.GetDefaultList();
						sendMoney.LCities = desktop.GetDefaultList();
					}

					if (receiver.PickupCountry == "US" || receiver.PickupCountry == "MX" || receiver.PickupCountry == "CA")
					{
						sendMoney.StateProvinceCode = receiver.PickupState_Province;
						if (!string.IsNullOrEmpty(receiver.PickupState_Province))
						{
							sendMoney.StateProvince = sendMoney.LStates.ToList<SelectListItem>().Find(c => c.Value == receiver.PickupState_Province).Value;
						}

						if (receiver.PickupCountry == "MX")
						{
							if (!string.IsNullOrEmpty(receiver.PickupCity) && receiver.PickupCity != "Select")
							{
								sendMoney.CityID = sendMoney.LCities.ToList<SelectListItem>().Find(c => c.Value == receiver.PickupCity).Text;
								sendMoney.City = sendMoney.LCities.ToList<SelectListItem>().Find(c => c.Value == receiver.PickupCity).Text;
								sendMoney.CityName = sendMoney.LCities.ToList<SelectListItem>().Find(c => c.Value == receiver.PickupCity).Text;
							}
						}
						else
						{
							sendMoney.City = "Not Applicable";
						}
					}

					//Get the CurrencyCode
                    string currencyCode = desktop.GetCurrencyCode(customerSessionId, receiver.PickupCountry, mgiContext);

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

					sendMoney.LDelivertyMethods = PopulateDeliveryServices(DeliveryServiceType.Method, sendMoney.Country, currencyCode, stateName, stateCode, mgiContext, cityName);

					var deliveryMethods = sendMoney.LDelivertyMethods;
					var deliveryMethod = deliveryMethods.Where(c => c.Value == receiver.DeliveryMethod).FirstOrDefault();
					if (deliveryMethod != null)
					{
						sendMoney.PickUpMethodId = receiver.DeliveryMethod;
						sendMoney.DeliveryMethod = deliveryMethods.Where(c => c.Value == receiver.DeliveryMethod).FirstOrDefault().Value;
					}

					sendMoney.LDeliveryOptions = PopulateDeliveryServices(DeliveryServiceType.Option, sendMoney.Country, currencyCode, stateName, stateCode, mgiContext, cityName, sendMoney.DeliveryMethod);

					var deliveryOptions = sendMoney.LDeliveryOptions;
					var deliveryOption = receiver.DeliveryOption != null ? deliveryOptions.Where(c => c.Value == receiver.DeliveryOption.ToString()).FirstOrDefault() : null;
					if (deliveryOption != null)
					{
						sendMoney.PickUpOptionsId = receiver.DeliveryOption.ToString();
						sendMoney.DeliveryOptions = deliveryOption.Value;
					}

					sendMoney.enableEditContinue = true;
				}
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				throw ex;
			}

			return sendMoney;
		}

		public JsonResult CancelSendMoneyDetails(long Id, string ScreenName)
		{

			if (Id != 0)
			{
				Desktop desktop = new Desktop();
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

				long customerSessionId = long.Parse(customerSession.CustomerSessionId);

				ModifySendMoneyResponse sendMoneyModifyIds = (ModifySendMoneyResponse)TempData["sendMoneyModifyIds"];

				if (sendMoneyModifyIds != null)
				{
					desktop.CancelXfer(customerSessionId, sendMoneyModifyIds.ModifyTransactionId, mgiContext);
					desktop.CancelXfer(customerSessionId, sendMoneyModifyIds.CancelTransactionId, mgiContext);
				}
				else if (ScreenName.ToLower() == "sendmoneyconfirm")
					desktop.RemoveMoneyTransfer(customerSessionId, Id);
				else
					desktop.CancelXfer(customerSessionId, Id, mgiContext);
			}

			ViewBag.Navigation = NexxoSiteMap.ProductInformation;
			var jsonData = new { success = true };

			return Json(jsonData, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// This ImportPast Button will enable DMS to Import Past Receivers for a particular customer from WU and add/modify tWUnion_Receiver Table.
		/// </summary>
		/// <param name="productName">Product Name</param>
		/// <returns></returns>
		public ActionResult ImportPastReceiver(string productName)
		{
			try
			{
				#region Added this code block for User Story # US1645 for Past Receivers
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				Desktop client = new Desktop();
				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
				long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
				Account senderDetails = client.DisplayWUCardAccountInfo(customerSessionId, mgiContext);
				mgiContext.ProductType = "sendmoney";
				mgiContext.AgentId = (int)Session["agentId"];
				mgiContext.ProcessorId = 13;

				if (customerSession.Customer.IsWUGoldCard)
                    client.AddPastReceivers(customerSessionId, senderDetails.LoyaltyCardNumber, mgiContext);

				#endregion

				// This will re-create partial view for FrequentReceivers for Send Money Screen.
				SendMoney sendMoney = new SendMoney();
				sendMoney.FrequentReceivers = client.GetFrequentReceivers(long.Parse(customerSession.CustomerSessionId), mgiContext);
				return PartialView("_FrequentReceivers", sendMoney);
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return Json(string.Empty, JsonRequestBehavior.AllowGet);
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
			ViewBag.ExceptionMsg = null;

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
				strmessage = new string[4];
				strmessage[0] = "MGiAlloy";
				strmessage[1] = "0.0";
				strmessage[2] = message;
				strmessage[3] = message;
			}

			return strmessage;
		}

		private IEnumerable<SelectListItem> PopulateDeliveryServices(DeliveryServiceType type, string countryCode,
			string currencyCode, string state, string stateCode, MGIContext mgiContext, string cityName = "", string deliveryMethod = "")
		{
			Desktop desktopp = new Desktop();
			DeliveryServiceRequest request = new DeliveryServiceRequest()
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
			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			long customerSessionId = long.Parse(customerSession.CustomerSessionId);

            List<DeliveryService> services = desktopp.GetDeliveryServices(customerSessionId, request, mgiContext);
			if (DeliveryServiceType.Option == type && services.Count <= 1)
			{
				DeliveryService _deliveryService = new DeliveryService
				{
					Code = "",
					Name = "Not Applicable"
				};
				services.Clear();
				services.Insert(0, _deliveryService);
			}

			return services.Select(d => new SelectListItem() { Text = d.Name, Value = d.Code });
		}

	}
}
