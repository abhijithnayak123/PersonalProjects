//using MGI.Channel.DMS.Web.Common;
//using MGI.Channel.DMS.Web.Models;
//using MGI.Channel.DMS.Web.ServiceClient;
//using MGI.Channel.Shared.Server.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;

//#region AO
//using AlloyClient = MGI.Channel.Alloy.Web.ServiceClient.AlloyService;
//#endregion

//namespace MGI.Channel.DMS.Web.Controllers
//{
//	public class MoneyGramMoneyTransferController : SendMoneyBaseController
//	{
//		Desktop client;

//		[HttpGet]
//		[CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
//		public ActionResult MoneyTransfer()
//		{
//			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
//			Session["activeButton"] = "moneytransfer";
//			client = new Desktop();
//			long customerSessionId = GetCustomerSessionId();
//			if (Session["FeeReesponse"] != null)
//			{
//				Session.Remove("FeeReesponse");
//			}

//			MoneyGramSendMoney moneyGramSendMoney = null;
//			if (TempData["ReceiverId"] != null)
//			{
//				long receiverId = (long)TempData["ReceiverId"];
//				moneyGramSendMoney = GetReceiverDetails(receiverId, mgiContext);
//				moneyGramSendMoney.FrequentReceivers.SelectedReceiverId = receiverId;
//			}
//			else
//			{
//				moneyGramSendMoney = new MoneyGramSendMoney();
//				moneyGramSendMoney.FrequentReceivers = GetFrequentReceivers(mgiContext);
//				DMS.Server.Data.Response response = client.GetXfrCountries(customerSessionId, mgiContext);
//                if (WebHelper.VerifyException(response)) return null;
//				moneyGramSendMoney.PickupCountries = GetSelectListItem(response.Result as List<AlloyClient.MasterData>);
//				moneyGramSendMoney.PickupStates = client.GetDefaultList();
//			}

//			return View("MoneyGramSendMoney", moneyGramSendMoney);
//		}

//		[HttpPost]
//		[CustomHandleErrorAttribute(ViewName = "MoneyGramSendMoney", MasterName = "_Common")]
//		public ActionResult SendMoney(MoneyGramSendMoney moneyGramSendMoney)
//		{
//			client = new Desktop();

//			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new DMS.Server.Data.MGIContext();
//			long customerSessionId = GetCustomerSessionId();
//			try
//			{
//				moneyGramSendMoney.ReceiverId = moneyGramSendMoney.FrequentReceivers.SelectedReceiverId;

//				if (moneyGramSendMoney.FrequentReceivers.SelectedReceiverId == 0)
//				{
//					var receiver = new Models.MoneyGramReceiverViewModel()
//					{
//						FirstName = moneyGramSendMoney.FirstName,
//						LastName = moneyGramSendMoney.LastName,
//						MiddleName = moneyGramSendMoney.MiddleName,
//						SecondLastName = moneyGramSendMoney.SecondLastName,
//						PickUpCountry = moneyGramSendMoney.PickupCountry,
//						PickUpState = moneyGramSendMoney.PickupState
//					};

//					PopulateDropdownlist(receiver, mgiContext);

//					Session["Receiver"] = receiver;
//					return RedirectToAction("AddReceiver");
//				}

//				MoneyGramSendMoneyDetail sendMoneyDetail = GetFee(moneyGramSendMoney, client, mgiContext);

//				sendMoneyDetail.LDeliveryMethods = client.GetDefaultList();
//				sendMoneyDetail.ReceiverID = moneyGramSendMoney.ReceiverId;

//				//clear the view data
//				ViewData = new ViewDataDictionary();
//				return View("MoneyGramSendMoneyDetail", sendMoneyDetail);
//			}
//			catch (Exception ex)
//			{
//				//moneyGramSendMoney.FrequentReceivers = GetFrequentReceivers(mgiContext);
//				//moneyGramSendMoney.FrequentReceivers.SelectedReceiverId = moneyGramSendMoney.ReceiverId;
//				//moneyGramSendMoney.PickupCountries = client.GetXfrCountries(customerSessionId, mgiContext);
//				//moneyGramSendMoney.PickupStates = client.GetXfrStates(customerSessionId, moneyGramSendMoney.PickupCountry, mgiContext);

//				//throw ex;
//				VerifyException(ex); return null;
//			}
//		}

//		[HttpGet]
//		public ActionResult Validate()
//		{
//			var sendMoneyDetail = new MoneyGramSendMoneyDetail();
//			return View("MoneyGramSendMoneyDetail", sendMoneyDetail);
//		}

//		[HttpPost]
//		[CustomHandleErrorAttribute(ViewName = "MoneyGramSendMoneyDetail", MasterName = "_Common")]
//		public ActionResult SendMoneyDetails(MoneyGramSendMoneyDetail sendMoneyDetail, string updateTrans, string next)
//		{
//			try
//			{
//				client = new Desktop();
//				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
//				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
//				DMS.Server.Data.Response serviceResponse = new DMS.Server.Data.Response();
//				ViewBag.Navigation = Resources.NexxoSiteMap.SendMoney;

//				sendMoneyDetail.IsDomesticTransfer = IsDomesticTransfer(sendMoneyDetail.CountryCode);

//				TempData["ReceiveCurrency"] = sendMoneyDetail.CurrencyType;
//				TempData["DeliveryOption"] = sendMoneyDetail.DeliveryMethod;
//				TempData["ReceiveAgent"] = sendMoneyDetail.ReceiveAgent;
//				TempData["DynamicFields"] = sendMoneyDetail.DynamicFields;

//				ModelState.Remove("DestinationAmount");
//				if (next != null && ModelState.IsValid)
//				{
//					serviceResponse = client.GetReceiverDetails(long.Parse(customerSession.CustomerSessionId), sendMoneyDetail.ReceiverID, mgiContext);
//                    if (WebHelper.VerifyException(serviceResponse)) throw new AlloyWebException(serviceResponse.Error.Details);
					
//					Receiver receiver = serviceResponse.Result as Receiver;

//					sendMoneyDetail.StateProvince = receiver.State_Province;
//					sendMoneyDetail.OriginalFee = Math.Round(Convert.ToDecimal((sendMoneyDetail.TransferFee + sendMoneyDetail.PromoDiscount)), 2);
//					sendMoneyDetail.ReceiveCurrencies = PopulateReceiveCurrencies();

//					var validateRequest = new ValidateRequest()
//					{
//						Amount = sendMoneyDetail.TransferAmount,
//						Fee = sendMoneyDetail.TransferFee,
//						Tax = sendMoneyDetail.TransferTax,
//						TransactionId = sendMoneyDetail.TransactionId,
//						TransferType = TransferType.SendMoney,
//						PromoCode = sendMoneyDetail.CouponPromoCode,
//						PersonalMessage = sendMoneyDetail.PersonalMessage,
//						State = sendMoneyDetail.PickupState,
//						ReferenceNumber = sendMoneyDetail.ReferenceNo,
//						DeliveryService = sendMoneyDetail.DeliveryMethod,
//						ReceiveCurrency = sendMoneyDetail.CurrencyType,
//						MetaData = new Dictionary<string, object>()
//					};

//					if (!string.IsNullOrWhiteSpace(sendMoneyDetail.ReceiveAgent))
//					{
//						validateRequest.MetaData.Add("receiveAgentID", sendMoneyDetail.ReceiveAgent);
//					}
//					if (!string.IsNullOrWhiteSpace(sendMoneyDetail.ReceiveAgentAbbreviation))
//					{
//						validateRequest.MetaData.Add("ReceiveAgentAbbreviation", sendMoneyDetail.ReceiveAgentAbbreviation);
//					}
//					var fieldValues = new Dictionary<string, string>();

//					if (sendMoneyDetail.DynamicFields != null)
//					{
//						foreach (var dynamicField in sendMoneyDetail.DynamicFields)
//						{
//							if (dynamicField.IsDynamic)
//							{
//								fieldValues.Add(dynamicField.TagName, dynamicField.Value);
//							}
//							else
//							{
//								validateRequest.MetaData.Add(dynamicField.TagName, dynamicField.Value);
//							}
//						}
//					}

//					if (fieldValues.Count > 0)
//					{
//						validateRequest.FieldValues = fieldValues;
//					}
					
//					serviceResponse = client.ValidateTransfer(long.Parse(customerSession.CustomerSessionId), validateRequest, mgiContext);
//                    if (WebHelper.VerifyException(serviceResponse)) throw new AlloyWebException(serviceResponse.Error.Details);
//					ValidateResponse response = serviceResponse.Result as ValidateResponse;
					
//					sendMoneyDetail.TransactionId = response.TransactionId;

//					//clear the view data
//					ViewData = new ViewDataDictionary();
//					MoneyGramSendMoneyConfirm confirmModel = MapToMoneyGramConfirm(sendMoneyDetail, mgiContext);
//					Session.Remove("FeeReesponse");

//					TempData.Remove("ReceiveCurrency");
//					TempData.Remove("DeliveryOption");
//					TempData.Remove("ReceiveAgent");
//					TempData.Remove("DynamicFields");

//					return View("MoneyGramSendMoneyConfirm", confirmModel);
//				}
//				else
//				{
//					bool isReceiveAmount = sendMoneyDetail.IsReceiveAmount;

//					var moneyGramSendMoney = new MoneyGramSendMoney()
//					{
//						Amount = isReceiveAmount ? "0" : sendMoneyDetail.TransferAmount.ToString(),
//						DestinationAmount = isReceiveAmount ? sendMoneyDetail.DestinationAmount.ToString() : "0",
//						DeliveryMethod = sendMoneyDetail.DeliveryMethod,
//						PickupCountry = sendMoneyDetail.PickupCountry,
//						CouponPromoCode = string.IsNullOrEmpty(sendMoneyDetail.CouponPromoCode) ? string.Empty : sendMoneyDetail.CouponPromoCode,
//						TransactionId = sendMoneyDetail.TransactionId,
//						FirstName = sendMoneyDetail.FirstName,
//						LastName = sendMoneyDetail.LastName,
//						MiddleName = sendMoneyDetail.MiddleName,
//						SecondLastName = sendMoneyDetail.SecondLastName,
//						PickupState = sendMoneyDetail.PickupState,
//						CurrencyCode = sendMoneyDetail.CurrencyType,
//						ReceiverId = sendMoneyDetail.ReceiverID
//					};
//					MoneyGramSendMoneyDetail moneyGramSendMoneyDet = GetFee(moneyGramSendMoney, client, mgiContext);

//					//clear the view data
//					ViewData = new ViewDataDictionary();
//					moneyGramSendMoneyDet.LDeliveryMethods = client.GetDefaultList();
//					moneyGramSendMoneyDet.PersonalMessage = sendMoneyDetail.PersonalMessage;
//					return View("MoneyGramSendMoneyDetail", moneyGramSendMoneyDet);
//				}
//			}
//			catch (Exception ex)
//			{
//				VerifyException(ex); return null;
//			}
//		}


//		[HttpPost]
//		[CustomHandleErrorAttribute(ViewName = "MoneyGramSendMoneyConfirm", MasterName = "_Common")]
//		public ActionResult SendMoneyConfirm(MoneyGramSendMoneyConfirm sendMoneyConfirm)
//		{
//			return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
//		}

//		[HttpGet]
//		public ActionResult AddReceiver()
//		{
//			client = new Desktop();
//			long customerSessionId = GetCustomerSessionId();
//			var receiver = new MoneyGramReceiverViewModel();

//			if (Session["Receiver"] != null)
//			{
//				receiver = Session["Receiver"] as MoneyGramReceiverViewModel;
//			}
//			else
//			{
//				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new DMS.Server.Data.MGIContext();

//				DMS.Server.Data.Response response = client.GetXfrCountries(customerSessionId, mgiContext);
//                if (WebHelper.VerifyException(response)) return null;
//				receiver.LPickUpCountry = GetSelectListItem(response.Result as List<AlloyClient.MasterData>);
				
//				receiver.LPickUpState = client.GetDefaultList();
//				receiver.LPickUpCity = client.GetDefaultList();
//			}
//			receiver.AddEdit = "Add";
//			return View("MoneyGramReceiver", receiver);
//		}

//		[HttpPost]
//		[CustomHandleErrorAttribute(ViewName = "SendMoneyMGIReceiver", MasterName = "_Common")]
//		public ActionResult AddReceiver(MoneyGramReceiverViewModel receiver)
//		{
//			DMS.Server.Data.Response response = new DMS.Server.Data.Response();
//			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new DMS.Server.Data.MGIContext();
//			if (ModelState.IsValid)
//			{
//				client = new Desktop();
//				var clientReceiver = new Receiver();
//				long responseReceiverID = 0;

//				PopulateDropdownlist(receiver, mgiContext);

//				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

//				if (receiver.ReceiverId == 0)
//				{
//					clientReceiver = SetReceivers(receiver);
//					try
//					{
//						response = client.SaveReceiver(long.Parse(customerSession.CustomerSessionId), clientReceiver, mgiContext);
//                        if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
//						responseReceiverID = long.Parse(response.Result.ToString());
//					}
//					catch (Exception ex)
//					{
//						VerifyException(ex); return null;
//					}
//				}
//				else
//				{
//					clientReceiver = SetReceivers(receiver);

//					try
//					{
//						response = client.UpdateReceiver(Convert.ToInt64(customerSession.CustomerSessionId), clientReceiver, mgiContext);
//						if (VerifyException(response)) throw new AlloyWebException(response.Error.Details);
//						responseReceiverID = long.Parse(response.Result.ToString());
//					}
//					catch (Exception ex)
//					{
//						VerifyException(ex); return null;
//					}
//				}

//				TempData["ReceiverId"] = responseReceiverID;
//				return RedirectToAction("MoneyTransfer", "MoneyGramMoneyTransfer");
//			}

//			PopulateDropdownlist(receiver, mgiContext);

//			return View("MoneyGramReceiver", receiver);
//		}

//		[HttpGet]
//		public ActionResult EditReceiver()
//		{
//			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
//			MoneyGramReceiverViewModel receiver = null;
//			if ((MoneyGramReceiverViewModel)Session["Receiver"] != null)
//			{
//				receiver = Session["Receiver"] as MoneyGramReceiverViewModel;
//			}
//			PopulateDropdownlist(receiver, mgiContext);

//			return View("MoneyGramReceiver", receiver);
//		}

//		public JsonResult PopulateReceiverDetails(long receiverId)
//		{
//			client = new Desktop();
//			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
//			var moneyGramSendMoney = new MoneyGramSendMoney();

//			try
//			{
//				moneyGramSendMoney.FrequentReceivers = GetFrequentReceivers(mgiContext);

//				if (receiverId > 0)
//				{
//					moneyGramSendMoney = GetReceiverDetails(receiverId, mgiContext);
//				}
//				return Json(moneyGramSendMoney, JsonRequestBehavior.AllowGet);
//			}
//			catch (Exception ex)
//			{
//				ViewBag.ErrorMessage = ex.Message;
//				return Json(moneyGramSendMoney, JsonRequestBehavior.AllowGet);
//			}
//		}

//		[HttpGet]
//		public ActionResult GetReceiverForEdit(string ReceiverId)
//		{
//			client = new Desktop();

//			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
//			DMS.Server.Data.Response response = new DMS.Server.Data.Response();
//			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
//			try
//			{
//				if (ReceiverId != "0")
//				{
//					response = client.GetReceiverDetails(Convert.ToInt64(customerSession.CustomerSessionId), long.Parse(ReceiverId), mgiContext);
//                    if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
//					Receiver editReceiverDetails = response.Result as Receiver;

//					var receiver = new MoneyGramReceiverViewModel()
//					{
//						AddEdit = "Edit",
//						ReceiverId = long.Parse(ReceiverId),
//						FirstName = editReceiverDetails.FirstName,
//						MiddleName = editReceiverDetails.MiddleName,
//						LastName = editReceiverDetails.LastName,
//						SecondLastName = editReceiverDetails.SecondLastName,
//						StateProvince = editReceiverDetails.State_Province,
//						Address = editReceiverDetails.Address,
//						ZipCode = editReceiverDetails.ZipCode,
//						City = editReceiverDetails.City,
//						Phone = editReceiverDetails.PhoneNumber,
//						PickUpCountry = editReceiverDetails.PickupCountry,
//						PickUpState = editReceiverDetails.PickupState_Province,
//						PickUpCity = editReceiverDetails.PickupCity,
//						IsReceiverHasPhotoId = editReceiverDetails.IsReceiverHasPhotoId,
//						TestQuestion = editReceiverDetails.SecurityQuestion,
//						TestAnswer = editReceiverDetails.SecurityAnswer
//					};

//					PopulateDropdownlist(receiver, mgiContext);
//					Session["Receiver"] = receiver;

//					return RedirectToAction("EditReceiver", "MoneyGramMoneyTransfer");
//				}
//				else
//				{
//					return null;
//				}
//			}
//			catch (Exception ex)
//			{
//				VerifyException(ex); return null;
//			}
//		}

//		[HttpGet]
//		[CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
//		public ActionResult CancelSendMoneyDetails(long transactionId = 0, string screenName = "")
//		{
//			try
//			{
//				if (transactionId != 0)
//				{
//					client = new Desktop();
//					MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
//					CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
//					DMS.Server.Data.Response response = new DMS.Server.Data.Response();

//					long customerSessionId = long.Parse(customerSession.CustomerSessionId);

//					if (screenName.ToLower() == "sendmoneyconfirm")
//					{
//						response = client.RemoveMoneyTransfer(customerSessionId, transactionId);
//                        if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
//					}
//					else
//					{
//						response = client.CancelXfer(customerSessionId, transactionId, mgiContext);
//                        if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
//					}
//				}

//				ViewBag.Navigation = Resources.NexxoSiteMap.ProductInformation;
//				return RedirectToAction("ProductInformation", "Product");
//			}
//			catch (Exception ex)
//			{
//				VerifyException(ex); return null;
//			}
//		}

//		public JsonResult PopulateDeliveryOptions(string currencyCode)
//		{
//			var deliveryServices = new List<DeliveryService>();
//			if (Session["FeeReesponse"] != null)
//			{
//				var feeResponse = Session["FeeReesponse"] as FeeResponse;

//				if (feeResponse != null)
//				{
//					deliveryServices = feeResponse.FeeInformations.FindAll(f => f.ReceiveCurrencyCode == currencyCode)
//						.Select(feeInfo => feeInfo.DeliveryService)
//						.GroupBy(ds => ds.Code)
//						.Select(ds => ds.First())
//						.ToList();
//				}
//			}
//			return Json(deliveryServices, JsonRequestBehavior.AllowGet);
//		}

//		public JsonResult GetFeeInformation(string currencyCode, string deliveryOption, string receiveAgent)
//		{
//			FeeInformation feeInformation = null;
//			if (Session["FeeReesponse"] != null)
//			{
//				var feeResponse = Session["FeeReesponse"] as FeeResponse;

//				if (feeResponse != null)
//				{
//					feeInformation = feeResponse.FeeInformations.
//						Find(f => f.ReceiveCurrencyCode.ToLower() == currencyCode.ToLower() && f.DeliveryService.Code == deliveryOption && (String.IsNullOrEmpty(receiveAgent) ? true : f.ReceiveAgentId == receiveAgent));
//				}
//			}
//			return Json(feeInformation, JsonRequestBehavior.AllowGet);
//		}

//		public JsonResult GetReceiveAgents(string currencyCode, string deliveryOption)
//		{
//			List<FeeInformation> feeInformations = null;
//			var receiveAgents = new List<ReceiveAgent>();
//			if (Session["FeeReesponse"] != null)
//			{
//				var feeResponse = Session["FeeReesponse"] as FeeResponse;

//				if (feeResponse != null)
//				{
//					feeInformations = feeResponse.FeeInformations.
//						FindAll(f => f.ReceiveCurrencyCode.ToLower() == currencyCode.ToLower() && f.DeliveryService.Code == deliveryOption);
//				}

//				if (feeInformations != null && feeInformations.Count > 0)
//				{
//					receiveAgents.AddRange
//					(
//						from feeInformation in feeInformations
//						where feeInformation.ReceiveAgentId != null
//						select new ReceiveAgent()
//						{
//							Id = feeInformation.ReceiveAgentId,
//							Name = feeInformation.ReceiveAgentAbbreviation
//						}
//					);
//				}
//			}
//			return Json(receiveAgents, JsonRequestBehavior.AllowGet);
//		}

//		public ActionResult PopulateDynamicControls(decimal amount, string deliverOption, string countryCode, string currencyCode, string receiveAgentId)
//		{
//			try
//			{
//				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new DMS.Server.Data.MGIContext();
//				var sendMoneyDetail = new MoneyGramSendMoneyDetail
//				{
//					DynamicFields = GetDynamicFields(amount, deliverOption, countryCode, currencyCode, receiveAgentId, mgiContext)
//				};

//				return PartialView("_DynamicFields", sendMoneyDetail);
//			}
//			catch
//			{
//				var jsonData = new
//				{
//					data = "Error while retreiving dynamic fields",
//					success = false
//				};

//				return Json(jsonData);
//			}
//		}

//		public ActionResult ShowCancelReceiverPopUp()
//		{
//			return PartialView("_SendMoneyMGReceiverCancelPopUp");
//		}

//		#region Private Methods

//		private FrequentReceivers GetFrequentReceivers(MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
//		{
//			try
//			{
//				client = new Desktop();

//				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
//				DMS.Server.Data.Response response = client.GetFrequentReceivers(long.Parse(customerSession.CustomerSessionId), mgiContext);
//                if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);


//				var frequentReceivers = new FrequentReceivers()
//				{
//					Receivers = response.Result as List<Receiver>,
//					SelectedReceiverId = 0
//				};

//				return frequentReceivers;
//			}
//			catch (Exception ex)
//			{
//				VerifyException(ex); return null;
//			}
//		}

//		private void PopulateDropdownlist(MoneyGramReceiverViewModel receiver, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
//		{
//			long customerSessionId = GetCustomerSessionId();
//			client = new Desktop();
//			DMS.Server.Data.Response response = new DMS.Server.Data.Response();

//			response = client.GetXfrCountries(customerSessionId, mgiContext);
//            if (WebHelper.VerifyException(response)) return;
//			receiver.LPickUpCountry = GetSelectListItem(response.Result as List<AlloyClient.MasterData>);

//			if (receiver.ReceiverId > 0 || !string.IsNullOrWhiteSpace(receiver.PickUpCountry))
//			{
//				response = client.GetXfrStates(customerSessionId, receiver.PickUpCountry, mgiContext);
//                if (WebHelper.VerifyException(response)) return;
//				receiver.LPickUpState = GetSelectListItem(response.Result as List<AlloyClient.MasterData>);
//			}
//			else
//			{
//				response = client.GetXfrStates(customerSessionId, "", mgiContext);
//                if (WebHelper.VerifyException(response)) return;
//				receiver.LPickUpState = GetSelectListItem(response.Result as List<AlloyClient.MasterData>);
//			}
//		}

//		private Receiver SetReceivers(MoneyGramReceiverViewModel sendMoneyReceiver)
//		{
//			long receiverId = 0L;
//			if (sendMoneyReceiver.ReceiverId > 0)
//			{
//				receiverId = sendMoneyReceiver.ReceiverId;
//			}

//			var receiver = new Receiver()
//			{
//				Id = receiverId,
//				FirstName = sendMoneyReceiver.FirstName,
//				LastName = sendMoneyReceiver.LastName,
//				MiddleName = sendMoneyReceiver.MiddleName,
//				SecondLastName = sendMoneyReceiver.SecondLastName,
//				Status = "Active",
//				Address = sendMoneyReceiver.Address,
//				City = sendMoneyReceiver.City,
//				State_Province = sendMoneyReceiver.StateProvince,
//				ZipCode = sendMoneyReceiver.ZipCode,
//				PhoneNumber = sendMoneyReceiver.Phone,
//				PickupCountry = sendMoneyReceiver.PickUpCountry,
//				PickupState_Province = sendMoneyReceiver.PickUpState,
//				PickupCity = sendMoneyReceiver.PickUpCity,
//				IsReceiverHasPhotoId = sendMoneyReceiver.IsReceiverHasPhotoId,
//				SecurityQuestion = sendMoneyReceiver.TestQuestion,
//				SecurityAnswer = sendMoneyReceiver.TestAnswer
//			};

//			return receiver;
//		}

//		private MoneyGramSendMoney GetReceiverDetails(long receiverId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
//		{
//			client = new Desktop();
//			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
//			var moneyGramSendMoney = new MoneyGramSendMoney();
//			DMS.Server.Data.Response response = new Server.Data.Response();
//			try
//			{
//				moneyGramSendMoney.FrequentReceivers = GetFrequentReceivers(mgiContext);
//				response = client.GetReceiverDetails(Convert.ToInt64(customerSession.CustomerSessionId), receiverId, mgiContext);
//                if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
//				Receiver receiver = response.Result as Receiver;

//				if (receiver != null)
//				{
//					moneyGramSendMoney.FirstName = receiver.FirstName;
//					moneyGramSendMoney.LastName = receiver.LastName;
//					moneyGramSendMoney.MiddleName = receiver.MiddleName;
//					moneyGramSendMoney.SecondLastName = receiver.SecondLastName;
//					moneyGramSendMoney.FrequentReceivers.SelectedReceiverId = receiver.Id;

//					response = client.GetXfrCountries(long.Parse(customerSession.CustomerSessionId), mgiContext);
//                    if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
//					moneyGramSendMoney.PickupCountries = GetSelectListItem(response.Result as List<AlloyClient.MasterData>);

//					moneyGramSendMoney.PickupCountry = receiver.PickupCountry;
					
//					response = client.GetXfrStates(long.Parse(customerSession.CustomerSessionId), receiver.PickupCountry, mgiContext);
//                    if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
					
//					moneyGramSendMoney.PickupStates = GetSelectListItem(response.Result as List<AlloyClient.MasterData>);
//					moneyGramSendMoney.PickupState = receiver.PickupState_Province;

//					if (!string.IsNullOrEmpty(receiver.PickupState_Province))
//					{
//						moneyGramSendMoney.PickupState = moneyGramSendMoney.PickupStates.ToList().Find(c => c.Value == receiver.PickupState_Province).Value;
//					}

//					moneyGramSendMoney.DeliveryMethods = PopulateDeliveryServices(mgiContext);

//					var deliveryMethods = moneyGramSendMoney.DeliveryMethods;
//					var deliveryMethod = deliveryMethods.FirstOrDefault(c => c.Value == receiver.DeliveryMethod);
//					if (deliveryMethod != null)
//					{
//						moneyGramSendMoney.DeliveryMethod = deliveryMethods.FirstOrDefault(c => c.Value == receiver.DeliveryMethod).Value;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				VerifyException(ex); return null;
//			}

//			return moneyGramSendMoney;
//		}

//		private MoneyGramSendMoneyConfirm MapToMoneyGramConfirm(MoneyGramSendMoneyDetail sendMoneyDetail, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
//		{
//			try
//			{
//				// Populating Receiver info, CountryName, StateName
//				client = new Desktop();
//				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
//				DMS.Server.Data.Response response = new DMS.Server.Data.Response();
//				response = client.GetReceiverDetails(Convert.ToInt64(customerSession.CustomerSessionId), sendMoneyDetail.ReceiverID, mgiContext);
//				Receiver receiver = response.Result as Receiver;

//				response = client.GetXfrCountries(long.Parse(customerSession.CustomerSessionId), mgiContext);//.Find(x => x.Value == sendMoneyDetail.PickupCountry).Text;
//				List<XferMasterData> countries = response.Result as List<XferMasterData>;
//				sendMoneyDetail.Country = countries.Find(x => x.Code == sendMoneyDetail.PickupCountry).Name;
				
//				if (sendMoneyDetail.PickupState != null) { 
//					response = client.GetXfrStates(long.Parse(customerSession.CustomerSessionId), sendMoneyDetail.PickupCountry, mgiContext);//.Find(x => x.Value == sendMoneyDetail.PickupState).Text;
//                    if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
//					List<XferMasterData> states = response.Result as List<XferMasterData>;
//					sendMoneyDetail.StateName = states.Find(x => x.Code == sendMoneyDetail.PickupState).Name;
//				}

//				var moneyGramSendMoneyConfirm = new MoneyGramSendMoneyConfirm()
//				{
//					ReceiverName = string.Format("{0} {1} {2} {3}", receiver.FirstName, receiver.MiddleName, receiver.LastName, receiver.SecondLastName),
//					PickupCountry = sendMoneyDetail.Country,
//					PickupState = sendMoneyDetail.StateName,
//					DeliveryMethod = GetDeliverySerivceDisplayName(sendMoneyDetail.DeliveryMethod),
//					TransferAmount = sendMoneyDetail.TransferAmount,
//					OriginalFee = sendMoneyDetail.OriginalFee,
//					PromoDiscount = sendMoneyDetail.PromoDiscount,
//					CouponPromoCode = sendMoneyDetail.CouponPromoCode,
//					TransferFee = sendMoneyDetail.TransferFee,
//					Amount = sendMoneyDetail.TransferAmount + sendMoneyDetail.TransferFee,
//					TransactionId = sendMoneyDetail.TransactionId,
//					IsDomesticTransfer = sendMoneyDetail.IsDomesticTransfer,
//					ReceiveAgent = GetReceiveAgentDisplayName(sendMoneyDetail.ReceiveAgent),
//					ReceiverAddress = receiver.Address,
//					ReceiverCity = receiver.City,
//					ReceiverState = receiver.State_Province
//				};
//				return moneyGramSendMoneyConfirm;
//			}
//			catch (Exception ex)
//			{
//				VerifyException(ex); return null;
//			}
//		}

//		private string GetDeliverySerivceDisplayName(string deliveryService)
//		{
//			var feeResponse = Session["FeeReesponse"] as FeeResponse;
//			string displayName = string.Empty;

//			if (feeResponse != null)
//			{
//				var feeInformation = feeResponse.FeeInformations.FirstOrDefault(f => f.DeliveryService.Code == deliveryService);
//				displayName = feeInformation.DeliveryService.Name;
//			}

//			return displayName;
//		}

//		private string GetReceiveAgentDisplayName(string receiveAgentId)
//		{
//			var feeResponse = Session["FeeReesponse"] as FeeResponse;
//			string displayName = string.Empty;

//			if (feeResponse != null)
//			{
//				var feeInformation = feeResponse.FeeInformations.FirstOrDefault(f => f.ReceiveAgentId == receiveAgentId);
//				displayName = !string.IsNullOrWhiteSpace(feeInformation.ReceiveAgentName) ? feeInformation.ReceiveAgentName : "NA";
//			}

//			return displayName;
//		}

//		private decimal GetExactSendMoneyAmount(MoneyGramSendMoney sendMoney)
//		{
//			string amount = sendMoney.Amount;
//			decimal sendMoneyAmount = 0M;
//			if (!(string.IsNullOrEmpty(amount)))
//			{
//				var amounts = amount.Split(' ');
//				if (amounts.Length == 3)
//				{
//					sendMoneyAmount = Convert.ToDecimal(amounts[1]);
//				}
//				else
//					sendMoneyAmount = Convert.ToDecimal(amount);
//			}

//			return sendMoneyAmount;
//		}

//		private decimal GetExactSendMoneyDestinationAmount(MoneyGramSendMoney sendMoney)
//		{
//			string amount = sendMoney.DestinationAmount;
//			decimal sendMoneyAmount = 0M;
//			if (!(string.IsNullOrEmpty(amount)))
//			{
//				var amounts = amount.Split(' ');
//				if (amounts.Length == 2)
//				{
//					sendMoneyAmount = Convert.ToDecimal(amounts[0]);
//				}
//				else
//					sendMoneyAmount = Convert.ToDecimal(amount);
//			}

//			return sendMoneyAmount;
//		}

//		private MoneyGramSendMoneyDetail GetFee(MoneyGramSendMoney sendMoney, Desktop client, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
//		{
//			try
//			{
//				DMS.Server.Data.Response response = new DMS.Server.Data.Response();
//				decimal sendMoneyAmount = GetExactSendMoneyAmount(sendMoney);
//				decimal sendMoneyDestinationAmount = GetExactSendMoneyDestinationAmount(sendMoney);

//				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
//				long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

//				var feeRequest = new FeeRequest()
//				{
//					Amount = sendMoneyAmount,
//					ReceiveCountryCode = sendMoney.PickupCountry,
//					ReceiveCountryCurrency = sendMoney.CurrencyCode,
//					TransactionId = sendMoney.TransactionId,
//					ReceiveAmount = sendMoneyDestinationAmount,
//					IsDomesticTransfer = IsDomesticTransfer(sendMoney.PickupCountry),
//					PromoCode = sendMoney.CouponPromoCode,
//					ReceiverId = sendMoney.ReceiverId,
//					ReceiverFirstName = sendMoney.FirstName,
//					ReceiverMiddleName = sendMoney.MiddleName,
//					ReceiverLastName = sendMoney.LastName,
//					ReceiverSecondLastName = sendMoney.SecondLastName,
//					DeliveryService = new DeliveryService()
//					{
//						Code = sendMoney.DeliveryMethod,
//						Name = sendMoney.DeliveryMethod
//					}
//				};

//				if (sendMoneyAmount > 0)
//				{
//					feeRequest.FeeRequestType = FeeRequestType.AmountExcludingFee;
//				}
//				else if (sendMoneyDestinationAmount > 0)
//				{
//					feeRequest.FeeRequestType = FeeRequestType.ReceiveAmount;
//				}
				
//				response = client.GetMoneyTransferFee(customerSessionId, feeRequest, mgiContext);
//                if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
//				FeeResponse feeResponse = response.Result as FeeResponse;

//				Session.Add("FeeReesponse", feeResponse);

//				IEnumerable<SelectListItem> receiveCurrencies = PopulateReceiveCurrencies();

//				var sendMoneyDetail = new MoneyGramSendMoneyDetail
//				{
//					CouponPromoCode = feeRequest.PromoCode,
//					CountryCode = feeRequest.ReceiveCountryCode,
//					ReceiverID = sendMoney.ReceiverId,
//					FirstName = sendMoney.FirstName,
//					MiddleName = sendMoney.MiddleName,
//					LastName = sendMoney.LastName,
//					PickupState = sendMoney.PickupState,
//					PickupCountry = sendMoney.PickupCountry,
//					IsDomesticTransfer = IsDomesticTransfer(sendMoney.PickupCountry),
//					ReceiveCurrencies = receiveCurrencies
//				};

//				if (sendMoneyDetail.IsDomesticTransfer)
//				{
//					sendMoneyDetail.ExchangeRateConversion = "Not Applicable";
//					sendMoneyDetail.DestinationAmount = 0;
//					sendMoneyDetail.DestinationAmountWithCurrency = "Not Applicable";
//					sendMoneyDetail.DestinationAmountWithCurrency1 = "Not Applicable";
//				}
//				else
//				{
//					sendMoneyDetail.DestinationAmount = 0;
//				}

//				if (sendMoneyAmount > 0)
//				{
//					sendMoneyDetail.IsReceiveAmount = false;
//				}
//				else if (sendMoneyDestinationAmount > 0)
//				{
//					sendMoneyDetail.IsReceiveAmount = true;
//				}

//				sendMoneyDetail.TransactionId = feeResponse.TransactionId;
//				sendMoneyDetail.DeliveryMethod = sendMoney.DeliveryMethod;
//				return sendMoneyDetail;
//			}
//			catch (Exception ex)
//			{
//				VerifyException(ex); return null;
//			}
//		}

//		private IEnumerable<SelectListItem> PopulateReceiveCurrencies()
//		{
//			var feeResponse = Session["FeeReesponse"] as FeeResponse;
//			var currencyCodes = feeResponse.FeeInformations.Select(x => x.ReceiveCurrencyCode).Distinct();

//			var receiveCurrencies = currencyCodes
//				.Select(currencyCode => new SelectListItem() { Text = currencyCode, Value = currencyCode })
//				.ToList();

//			receiveCurrencies.Insert(0, new SelectListItem() { Value = string.Empty, Text = "Select", Selected = true });

//			return receiveCurrencies;
//		}

//		private static bool IsDomesticTransfer(string countryCode)
//		{
//			return countryCode.ToLower() == "us" || countryCode.ToLower() == "usa" || countryCode.ToLower() == "united states";
//		}

//		private IEnumerable<SelectListItem> PopulateDeliveryServices(MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
//		{
//			client = new Desktop();
//			var request = new DeliveryServiceRequest();

//			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
//			long customerSessionId = long.Parse(customerSession.CustomerSessionId);

//			DMS.Server.Data.Response response = client.GetDeliveryServices(customerSessionId, request, mgiContext);
//            if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
//			List<DeliveryService> deliveryServices = response.Result as List<DeliveryService>;
//			deliveryServices.Insert(0, new DeliveryService() { Name = "Select", Code = string.Empty });
//			IEnumerable<SelectListItem> deliveryOptions = deliveryServices.Select(d => new SelectListItem() { Text = d.Code, Value = d.Name });

//			return deliveryOptions;
//		}

//		private List<FieldViewModel> GetDynamicFields(decimal amount, string deliverOption, string countryCode, string currencyCode, string receiveAgentId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
//		{
//			client = new Desktop();

//			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
//			long customerSessionId = long.Parse(customerSession.CustomerSessionId);
//			if (Session["FeeReesponse"] != null)
//			{
//				FeeResponse feeResponse = Session["FeeReesponse"] as FeeResponse;
//				mgiContext.TrxId = feeResponse.TransactionId;
//			}


//			var attributeRequest = new AttributeRequest()
//			{
//				Amount = amount,
//				DeliveryService = new DeliveryService() { Code = deliverOption },
//				ReceiveCountry = countryCode,
//				ReceiveCurrencyCode = currencyCode,
//				ReceiveAgentId = receiveAgentId,
//				TransferType = TransferType.SendMoney,
//			};

//			DMS.Server.Data.Response response = client.GetXfrProviderAttributes(customerSessionId, attributeRequest, mgiContext);
//            if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
//			List<Field> fields = response.Result as List<Field>;

//			List<FieldViewModel> dynamicFields = fields.Select(field => new FieldViewModel()
//			{
//				Label = field.Label,
//				TagName = field.TagName,
//				DataType = field.DataType,
//				IsRequired = field.IsMandatory,
//				IsDynamic = field.IsDynamic,
//				MaxLength = Convert.ToInt32(field.MaxLength),
//				RegularExpression = field.RegularExpression,
//				Values = SelectListMapper(field.Values, field.Label)
//			}).ToList();

//			if (TempData["DynamicFields"] != null)
//			{
//				var storedFields = TempData["DynamicFields"] as List<FieldViewModel>;

//				foreach (FieldViewModel fieldViewModel in storedFields)
//				{
//					dynamicFields.Find(d => d.Label == fieldViewModel.Label).Value = fieldViewModel.Value;
//				}
//			}

//			return dynamicFields;
//		}

//		private IEnumerable<SelectListItem> SelectListMapper(Dictionary<string, string> valueDictionary, string Label)
//		{
//			var dropdownValues = new List<SelectListItem>();
//			if (valueDictionary != null)
//			{
//				if (TempData["DynamicFields"] != null)
//				{
//					var storedFields = TempData["DynamicFields"] as List<FieldViewModel>;

//					string seletedValue = storedFields.Find(d => d.Label == Label).Value;

//					dropdownValues.AddRange(valueDictionary.Select(item => new SelectListItem() { Text = item.Key, Value = item.Value, Selected = item.Value == seletedValue }));
//				}
//				else
//				{
//					dropdownValues.AddRange(valueDictionary.Select(item => new SelectListItem() { Text = item.Key, Value = item.Value }));
//				}
//			}

//			return dropdownValues;
//		}

//		#endregion
//	}

//	public class ReceiveAgent
//	{
//		public string Id { get; set; }
//		public string Name { get; set; }
//	}
//}
