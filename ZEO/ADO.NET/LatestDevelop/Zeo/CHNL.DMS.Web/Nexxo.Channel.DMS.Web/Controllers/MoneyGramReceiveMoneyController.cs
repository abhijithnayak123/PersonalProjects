using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.Common;

namespace MGI.Channel.DMS.Web.Controllers
{
    public class MoneyGramReceiveMoneyController : BaseController
    {
        [HttpGet]
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
        public ActionResult ReceiveMoney()
        {
            Session["activeButton"] = "receivemoney";
            MoneyGramReceiveMoneyViewModel receiveMoney = new MoneyGramReceiveMoneyViewModel();
            return View("MoneyGramReceiveMoney", receiveMoney);
        }

        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "MoneyGramReceiveMoney", MasterName = "_Common")]
        public ActionResult ReceiveMoney(MoneyGramReceiveMoneyViewModel receivemoney)
        {
			try
			{
				Desktop desktop = new Desktop();
				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
				long customerSessionId = long.Parse(customerSession.CustomerSessionId);
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();

				ReceiveMoneyRequest request = new ReceiveMoneyRequest()
				{
					ConfirmationNumber = receivemoney.ReferenceNumber
				};

				DMS.Server.Data.Response response = desktop.GetReceiveTransaction(customerSessionId, request, mgiContext);
                if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
				MoneyTransferTransaction transaction = response.Result as MoneyTransferTransaction;

				receivemoney.ReceiverName = transaction.ReceiverFirstName + " " + transaction.ReceiverLastName + " " + transaction.ReceiverSecondLastName;
				receivemoney.SenderName = transaction.SenderName;
				receivemoney.TestAnswer = transaction.TestQuestion;
				receivemoney.TestQuestion = transaction.TestAnswer;
				receivemoney.ReceiveAmount = transaction.DestinationPrincipalAmount.ToString();
				receivemoney.ReceiveAmountWithCurrency = transaction.DestinationPrincipalAmount.ToString() + " " + transaction.DestinationCurrencyCode;
				receivemoney.TransactionId = long.Parse(transaction.TransactionID);
				receivemoney.SenderStateCode = transaction.MetaData.ContainsKey("SenderStateCode") ? transaction.MetaData["SenderStateCode"].ToString() : transaction.OriginatingCountryCode;
				receivemoney.ReceiveCurrency = transaction.DestinationCurrencyCode;

				var attributeRequest = new AttributeRequest()
				{
					Amount = transaction.DestinationPrincipalAmount,
					ReceiveCountry = transaction.DestinationCountryCode,
					ReceiveCurrencyCode = transaction.DestinationCurrencyCode,
					TransferType = TransferType.RecieveMoney,
				};

				receivemoney.DynamicFields = GetDynamicFields(attributeRequest, mgiContext);

				TempData["receivemoney"] = receivemoney;

				return RedirectToAction("ReceiveMoneyDetails", "MoneyGramReceiveMoney");
			}
			catch (Exception ex)
			{
				VerifyException(ex); return null;
			}
        }

        private List<FieldViewModel> GetDynamicFields(AttributeRequest attributeRequest, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
        {
            Desktop desktop = new Desktop();

			mgiContext.ProviderId = (int)ProviderIds.MoneyGramBillPay;

            CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
            long customerSessionId = Convert.ToInt64(customerSession.CustomerSessionId);

			DMS.Server.Data.Response response = desktop.GetXfrProviderAttributes(customerSessionId, attributeRequest, mgiContext);
            if (WebHelper.VerifyException(response)) throw new AlloyWebException(response.Error.Details);
			List<Field> fields = response.Result as List<Field>;

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

            //if (TempData["DynamicFields"] != null)
            //{
            //    var storedFields = TempData["DynamicFields"] as List<FieldViewModel>;

            //    foreach (FieldViewModel fieldViewModel in storedFields)
            //    {
            //        dynamicFields.Find(d => d.Label == fieldViewModel.Label).Value = fieldViewModel.Value;
            //    }
            //}

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


        [HttpGet]
        [CustomHandleErrorAttribute(ViewName = "ProductInformation", MasterName = "_Common", ResultType = "prepare", ModelType = "MGI.Channel.DMS.Web.Models.ProductInfo")]
        public ActionResult ReceiveMoneyDetails()
        {
            MoneyGramReceiveMoneyViewModel receivemoney = (MoneyGramReceiveMoneyViewModel)TempData["receivemoney"];
            return View("MoneyGramReceiveMoneyDetails", receivemoney);
        }

        [HttpPost]
        [CustomHandleErrorAttribute(ViewName = "MoneyGramReceiveMoneyDetails", MasterName = "_Common")]
        public ActionResult ReceiveMoneyValidate(MoneyGramReceiveMoneyViewModel receivemoney)
        {
			try
			{
				var client = new Desktop();

				CustomerSession customerSession = (CustomerSession)Session["CustomerSession"];
				MGI.Channel.DMS.Server.Data.MGIContext mgiContext = new MGI.Channel.DMS.Server.Data.MGIContext();
				ViewBag.Navigation = Resources.NexxoSiteMap.TransactionSummary;
				TempData["DynamicFields"] = receivemoney.DynamicFields;

				if (ModelState.IsValid)
				{
					var validateRequest = new ValidateRequest()
					{
						Amount = Convert.ToDecimal(receivemoney.ReceiveAmount),
						TransactionId = receivemoney.TransactionId,
						TransferType = TransferType.RecieveMoney,
						ReferenceNumber = receivemoney.ReferenceNumber,
						ReceiveCurrency = receivemoney.ReceiveCurrency,
						MetaData = new Dictionary<string, object>()
					};

					var fieldValues = new Dictionary<string, string>();

					if (receivemoney.DynamicFields != null)
					{
						foreach (var dynamicField in receivemoney.DynamicFields)
						{
							if (dynamicField.IsDynamic)
							{
								fieldValues.Add(dynamicField.TagName, dynamicField.Value);
							}
							else
							{
								validateRequest.MetaData.Add(dynamicField.TagName, dynamicField.Value);
							}
						}
					}

					if (fieldValues.Count > 0)
					{
						validateRequest.FieldValues = fieldValues;
					}
					
					DMS.Server.Data.Response serviceResponse = client.ValidateTransfer(long.Parse(customerSession.CustomerSessionId), validateRequest, mgiContext);
                    if (WebHelper.VerifyException(serviceResponse)) throw new AlloyWebException(serviceResponse.Error.Details);
					ValidateResponse response = serviceResponse.Result as ValidateResponse;

					receivemoney.TransactionId = response.TransactionId;

					//clear the view data
					ViewData = new ViewDataDictionary();
					TempData.Remove("DynamicFields");
					return RedirectToAction("ShoppingCartCheckout", "ShoppingCart");
				}
				else
				{
					return View("MoneyGramReceiveMoneyDetails", receivemoney);
				}
			}
			catch (Exception ex)
			{
				VerifyException(ex); return null;
			}
        }
    }
}
