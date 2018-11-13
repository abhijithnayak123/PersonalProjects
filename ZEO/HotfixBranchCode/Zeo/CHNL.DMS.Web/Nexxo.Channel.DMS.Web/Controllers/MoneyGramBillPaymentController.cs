using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.Shared.Server.Data;
using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using ServerData = MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Data;


namespace MGI.Channel.DMS.Web.Controllers
{
	public class MoneyGramBillPaymentController : BillPaymentBaseController
	{
		[HttpGet]
		[CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
		public ActionResult BillPayment(bool IsException = false, string ExceptionMsg = "")
		{
			Session["activeButton"] = "billpayment";
			Desktop desktop = new Desktop();
			ViewBag.IsException = IsException;
			ViewBag.ExceptionMsg = ExceptionMsg;
			TempData["DynamicFields"] = null;
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Models.MoneyGramBillPayViewModel billpayment = new Models.MoneyGramBillPayViewModel();
			billpayment.ProviderName = ProviderIds.MoneyGramBillPay.ToString();
			billpayment.FrequentBillPayees = desktop.GetFrequentBillers(long.Parse(billpayment.customerSession.CustomerSessionId), billpayment.customerSession.Customer.CIN, mgiContext);

			if (billpayment.FrequentBillPayees != null)
			{
				billpayment.FrequentBillPayees.ForEach(x => x.ProductName = x.ProductName + "/" + x.BillerCode);
			}

			if (Session["billFee"] != null)
			{
				Session.Remove("billFee");
			}

			Session["AccountNumberRetryCount"] = "1";
			Session["SelectedBiller"] = "";

			return View("MoneyGramBillPay", billpayment);
		}

		public JsonResult PopulateBillFee(decimal amount, string billerCode, string accountNumber)
		{
			try
			{
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				Desktop desktop = new Desktop();
				long channelPartnerId = desktop.GetChannelPartner(Session["ChannelPartnerName"].ToString(), mgiContext).Id;
				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
				long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

				if (Session["billFee"] != null)
				{
					SharedData.BillFee billFeeVar = Session["billFee"] as SharedData.BillFee;
					mgiContext.TrxId = billFeeVar.TransactionId;
				}

				SharedData.BillFee billFee = desktop.GetFee(customerSessionId, billerCode, accountNumber, amount, null, mgiContext);

				Session["billFee"] = billFee;

				return Json(billFee, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				throw ex;
			}
		}

		public ActionResult PopulateDynamicControls(string billerName)
		{
			try
			{
				var billPay = new MoneyGramBillPayViewModel()
				{
					DynamicFields = GetDynamicFields(billerName)
				};

				return PartialView("_DynamicFieldsBillPay", billPay);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private List<FieldViewModel> GetDynamicFields(string billerName)
		{
			Desktop deskTop = new Desktop();
			ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;

			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext()
				{
					ChannelPartnerId = channelPartner.Id,
					ProviderId = (int)ProviderIds.MoneyGramBillPay
				};

			if (Session["billFee"] != null)
			{
				SharedData.BillFee billFee = Session["billFee"] as SharedData.BillFee;
				mgiContext.TrxId = billFee.TransactionId;
			}

			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
			long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

			List<Field> fields = deskTop.GetProviderAttributes(customerSessionId, billerName, "", mgiContext);

			List<FieldViewModel> dynamicFields = fields.Select(field => new FieldViewModel()
			{
				Label = string.IsNullOrEmpty(field.Label) ? (string.IsNullOrEmpty(GetLabelFromResource("MG_" + field.TagName)) ? field.TagName : GetLabelFromResource("MG_" + field.TagName)) : field.Label,
				TagName = field.TagName,
				DataType = field.DataType,
				IsRequired = field.IsMandatory,
				IsDynamic = field.IsDynamic,
				MaxLength = Convert.ToInt32(field.MaxLength),
				RegularExpression = field.RegularExpression,
				Values = SelectListMapper(field.Values)
			}).ToList();

			if (TempData["DynamicFields"] != null)
			{
				var storedFields = TempData["DynamicFields"] as List<FieldViewModel>;

				foreach (FieldViewModel fieldViewModel in storedFields)
				{
					dynamicFields.Find(d => d.Label == fieldViewModel.Label).Value = fieldViewModel.Value;
				}
			}

			return dynamicFields;
		}

		private IEnumerable<SelectListItem> SelectListMapper(Dictionary<string, string> valueDictionary)
		{
			var dropdownValues = new List<SelectListItem>();

			if (valueDictionary != null && valueDictionary.Count > 0)
			{
				dropdownValues.Add(new SelectListItem() { Selected = true, Text = "--Select--", Value = "" });
				dropdownValues.AddRange(valueDictionary.Select(item => new SelectListItem() { Text = item.Value, Value = item.Key }));
			}

			return dropdownValues;
		}

		private string GetLabelFromResource(string sResourceName)
		{
			string resourceValue = string.Empty;

			ResourceManager manager = new ResourceManager(typeof(MGI.Channel.DMS.Web.App_GlobalResources.Nexxo));
			object obj = manager.GetObject(sResourceName);
			if (obj != null)
				resourceValue = HttpUtility.JavaScriptStringEncode(obj.ToString());

			return resourceValue;
		}

		[HttpPost]
		[CustomHandleErrorAttribute(ActionName = "BillPayment", ControllerName = "MoneyGramBillPayment", ResultType = "redirect")]
		public ActionResult Validate(MoneyGramBillPayViewModel mgdetail)
		{
			MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
			Desktop desktop = new Desktop();

			CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];

			long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
			long customerId = Convert.ToInt64(customerSession.Customer.CIN);
			
			mgiContext.AlloyId = customerId;

			TempData["DynamicFields"] = mgdetail.DynamicFields;

			if (Session["billFee"] != null)
			{
				SharedData.BillFee billFee = Session["billFee"] as SharedData.BillFee;
				mgiContext.TrxId = billFee.TransactionId;
			}

			Session["SelectedBiller"] = mgdetail.BillerName;
			if (Session["AccountNumberRetryCount"] != null)
			{
				mgiContext.AccountNumberRetryCount = Convert.ToString(Session["AccountNumberRetryCount"]);
			}

			BillPayment billPayment = new BillPayment();
			billPayment.BillerId = mgdetail.BillerId;

			if (!string.IsNullOrEmpty(mgdetail.BillerDenominations))
				billPayment.PaymentAmount = mgdetail.BillAmount1;
			else
				billPayment.PaymentAmount = mgdetail.BillAmount;

			billPayment.AccountNumber = mgdetail.AccountNumber;

			var fieldValues = new Dictionary<string, string>();

			if (mgdetail.DynamicFields != null)
			{
				billPayment.MetaData = new Dictionary<string, object>();

				foreach (var dynamicField in mgdetail.DynamicFields)
				{
					billPayment.MetaData.Add(dynamicField.TagName, dynamicField.Value);
				}
			}

			long transactionID = 0L;

			try
			{
				transactionID = desktop.ValidateBillPayment(customerSessionId, billPayment, mgiContext);
			}
			catch (Exception ex)
			{
				ViewBag.IsException = true;
				ViewBag.ExceptionMsg = HttpUtility.JavaScriptStringEncode(ex.Message);
				mgdetail.FrequentBillPayees = desktop.GetFrequentBillers(customerSessionId, customerId, mgiContext);

				if (mgdetail.FrequentBillPayees != null)
				{
					mgdetail.FrequentBillPayees.ForEach(x => x.ProductName = x.ProductName + "/" + x.BillerCode);
				}

				if (ex.Message.Contains("1004.2415.405.615") || ex.Message.Contains("1004.2415.405.1010"))
				{
					Session["AccountNumberRetryCount"] = "2";
				}

				return View("MoneyGramBillPay", mgdetail);
			}
			if (Session["billFee"] != null)
			{
				Session.Remove("billFee");
			}
			ServerData.BillPayTransaction trx = desktop.GetBillPayTransaction(GetAgentSessionId(), customerSessionId, transactionID, mgiContext);

			SharedData.Customer customerProfile = customerSession.Customer;

			MoneyGramBillPayReview billPaymentReview = new MoneyGramBillPayReview()
			{
				BillpayTransactionId = transactionID,
				BillerName = mgdetail.BillerName,
				BillerLocationName = Convert.ToString(trx.MetaData["BillerCity"]) + ", " + Convert.ToString(trx.MetaData["BillerState"]),
				BillPaymentAccount = billPayment.AccountNumber,
				BillPaymentDeliveryMethod = mgdetail.BillerNotes,
				BillPaymentDeliveryOption = mgdetail.DeliveryOption,
				BillpaymentAmount = trx.Amount,
				BillPaymentFee = (trx != null) ? trx.Fee : 0.0M,
				SenderName = customerProfile.PersonalInformation.FName + " " + customerProfile.PersonalInformation.LName,
				SenderAddress1 = customerProfile.Address.Address1 + " " + customerProfile.Address.Address2,
				SenderAddress2 = customerProfile.Address.City + ", " + customerProfile.Address.State + " " + customerProfile.Address.PostalCode,
				SenderEmail = customerProfile.Email,
				SenderPhoneNumber = customerProfile.Phone1.Number,
			};

			ViewData = new ViewDataDictionary();
			return View("MoneyGramBillPayReview", "_Common", billPaymentReview);
		}

		public object GetDictionaryValue(Dictionary<string, object> dictionary, string key)
		{
			if (dictionary.ContainsKey(key) == false)
			{
				throw new Exception(String.Format("{0} not provided in dictionary", key));
			}
			return dictionary[key];
		}

		[CustomHandleErrorAttribute(ViewName = "MoneyGramBillPayReview", MasterName = "_Common")]
		public ActionResult BillPaySubmit(MoneyGramBillPayReview billpaymentReview)
		{
			if (billpaymentReview.ConsumerProtectionMessage)
			{
				Desktop desktop = new Desktop();
				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
				long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);
				ChannelPartner channelPartner = Session["ChannelPartner"] as ChannelPartner;

				MGI.Channel.DMS.Server.Data.MGIContext context = new MGI.Channel.DMS.Server.Data.MGIContext() 
                { 
                    RequestType = RequestType.HOLD.ToString() 
                };

				//Add to shopping cart and stage in cxn
				desktop.StageBillPayment(customerSessionId, billpaymentReview.BillpayTransactionId, context);

				return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
			}
			else
			{
				ViewBag.ErrorMessage = "Please select the checkbox";
				return View("MoneyGramBillPayReview", billpaymentReview);
			}
		}
	}
}
