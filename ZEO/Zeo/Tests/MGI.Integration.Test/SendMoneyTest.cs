
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using MGI.Channel.DMS.Web.Common;


namespace MGI.Integration.Test
{
	[TestFixture]
	class SendMoneyTest : BaseFixture
	{
		[SetUp]
		public void Setup()
		{
			Client = new Desktop();
		}

		[Test]
		public void Can_Perform_WU_SendMoney()
		{
			//GetChannelPartnerDataCarver();
			GetChannelPartnerDataSynovus();

			Response serviceResponse = new Response();

			AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "ADNEY", new DateTime(1985, 02, 02));

			Response customerResponse = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);
            if (VerifyException(customerResponse)) throw new AlloyWebException(customerResponse.Error.Details); 
            CustomerSession = (CustomerSession)customerResponse.Result;

			MgiContext.AlloyId = AlloyId;
			MgiContext.WUCounterId = "13139925";
			MgiContext.RequestType = RequestType.HOLD.ToString();
			MgiContext.IsReferral = false;
			MgiContext.ChannelPartnerName = ChannelPartnerName;
			MgiContext.IsAvailable = true;

			serviceResponse = Client.GetFrequentReceivers(long.Parse(CustomerSession.CustomerSessionId), MgiContext);
			List<Receiver> receivers = serviceResponse.Result as List<Receiver>;
			
			if (receivers.Count == 0)
			{
				CreateReceiver_WU(long.Parse(CustomerSession.CustomerSessionId));
				serviceResponse = Client.GetFrequentReceivers(long.Parse(CustomerSession.CustomerSessionId), MgiContext);
				receivers = serviceResponse.Result as List<Receiver>;
			}
			Receiver receiver = null;

			if (receivers.Count > 0)
			{
				receiver = receivers.FirstOrDefault();

				if (receiver != null)
				{
					serviceResponse = Client.GetCurrencyCode(Convert.ToInt64(CustomerSession.CustomerSessionId), receiver.PickupCountry, MgiContext);
					string currencyCode = serviceResponse.Result as string;

					FeeRequest feeRequest = new FeeRequest()
					{
						Amount = 60,
						ReceiveAmount = 100,
						ReceiveCountryCode = receiver.PickupCountry,
						ReceiveCountryCurrency = currencyCode,
						TransactionId = 0,
						IsDomesticTransfer = false,
						PromoCode = "",
						ReceiverFirstName = receiver.FirstName,
						ReceiverLastName = receiver.LastName,
						ReceiverSecondLastName = receiver.SecondLastName,
						ReceiverId = receiver.Id,
						DeliveryService = new DeliveryService()
						{
							Code = receiver.DeliveryOption != null ? receiver.DeliveryOption : receiver.DeliveryMethod,
							Name = receiver.DeliveryOption != null ? receiver.DeliveryOption : receiver.DeliveryMethod
						},
						PersonalMessage = "",
						MetaData = new Dictionary<string, object>()
						{
							{"CityCode",  receiver.PickupCity},
							{"CityName",  receiver.City},
							{"StateCode",  receiver.PickupState_Province},
							{"StateName",  receiver.State_Province},
							{"TestQuestionOption", "Test Question Option"},
							{"TestQuestion", "Test Question"},
							{"TestAnswer", "Test Answer"},
							{"IsFixedOnSend", "false"}		
						},
						ReferenceNo = "12323412"
					};

					serviceResponse = Client.GetMoneyTransferFee(long.Parse(CustomerSession.CustomerSessionId), feeRequest, MgiContext);
					FeeResponse feeResponse = serviceResponse.Result as FeeResponse;
					FeeInformation feeInformation = new FeeInformation();
					if (feeResponse.FeeInformations.Count > 0)
					{
						feeInformation = feeResponse.FeeInformations.FirstOrDefault();

						ValidateRequest validateRequest = new ValidateRequest()
						{
							TransferType = TransferType.SendMoney,
							TransactionId = feeResponse.TransactionId,
							ReceiverId = 0,

							Amount = feeInformation.Amount,
							Fee = feeInformation.Fee,
							Tax = feeInformation.Tax,
							OtherFee = feeInformation.OtherFee,
							MessageFee = feeInformation.OtherFee,

							PersonalMessage = "",
							PromoCode = "",
							//IdentificationQuestion = "Test Question",
							//IdentificationAnswer = "Test Answer",
							DeliveryService = DeliveryService,
							State = receiver.PickupState_Province,

							ReceiverFirstName = feeRequest.ReceiverFirstName,
							ReceiverLastName = feeRequest.ReceiverFirstName,
							ReceiverSecondLastName = feeRequest.ReceiverLastName,

							MetaData = new Dictionary<string, object>()
							{
								{"ExpectedPayoutCity",receiver.PickupCity}, 
								{"ExpectedPayoutStateCode",receiver.PickupState_Province}, 
								{"ProceedWithLPMTError","false"},
								{"ReceiveAgentAbbr", ""},
							}
						};

						serviceResponse = Client.ValidateTransfer(long.Parse(CustomerSession.CustomerSessionId), validateRequest, MgiContext);
						ValidateResponse response = serviceResponse.Result as ValidateResponse;
						Response cartResponse = Client.ShoppingCart(CustomerSession.CustomerSessionId);
                        if(VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
                        ShoppingCart cart = cartResponse.Result as ShoppingCart;
						Response statusResponse = Client.Checkout(CustomerSession.CustomerSessionId, GetRandomAmount(), "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
                        if(VerifyException(statusResponse)) throw new Exception(statusResponse.Error.Details);
                        ShoppingCartCheckoutStatus status = (ShoppingCartCheckoutStatus)statusResponse.Result;
						UpdateCounterId();
						Assert.AreEqual(ShoppingCartCheckoutStatus.Completed, status);
					}
					else
					{
						UpdateCounterId();
						throw new Exception("No Fee available for this transaction");
					}
				}
			}
			else
			{
				UpdateCounterId();
				throw new Exception("No receivers available for this customer");
			}
		}

		[Test]
		public void Can_Perform_MGI_SendMoney()
		{
			GetChannelPartnerDataMGI();

			AgentSession = GetAgentSession();
			Response serviceResponse = new Response();
			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "John", new DateTime(1987, 02, 09));
			
            Response customerResponse = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);
            if (VerifyException(customerResponse)) throw new AlloyWebException(customerResponse.Error.Details); 
            CustomerSession = (CustomerSession)customerResponse.Result;

			MgiContext.AlloyId = AlloyId;
			MgiContext.WUCounterId = "13139925";
			MgiContext.RequestType = RequestType.HOLD.ToString();
			MgiContext.IsReferral = false;
			MgiContext.ChannelPartnerName = ChannelPartnerName;
			MgiContext.IsAvailable = true;

			serviceResponse = Client.GetFrequentReceivers(long.Parse(CustomerSession.CustomerSessionId), MgiContext);
			List<Receiver> receivers = serviceResponse.Result as List<Receiver>;
			if (receivers.Count == 0)
			{
				CreateReceiver_MGI(long.Parse(CustomerSession.CustomerSessionId));
				serviceResponse = Client.GetFrequentReceivers(long.Parse(CustomerSession.CustomerSessionId), MgiContext);
				receivers = serviceResponse.Result as List<Receiver>;
			}

			Receiver receiver = null;

			if (receivers.Count > 0)
			{
				receiver = receivers.FirstOrDefault();
				serviceResponse = Client.GetCurrencyCode(Convert.ToInt64(CustomerSession.CustomerSessionId), receiver.PickupCountry, MgiContext);
				string currencyCode = serviceResponse.Result as string;
				if (receiver != null)
				{
					FeeRequest feeRequest = new FeeRequest()
					{
						Amount = 160,
						ReceiveAmount = 200,
						ReceiveCountryCode = receiver.PickupCountry,
						ReceiveCountryCurrency = currencyCode,
						TransactionId = 0,
						IsDomesticTransfer = false,
						PromoCode = "",
						ReceiverFirstName = receiver.FirstName,
						ReceiverLastName = receiver.LastName,
						ReceiverSecondLastName = receiver.SecondLastName,
						ReceiverId = receiver.Id,
						DeliveryService = new DeliveryService()
						{
							Code = receiver.DeliveryOption != null ? receiver.DeliveryOption : receiver.DeliveryMethod,
							Name = receiver.DeliveryOption != null ? receiver.DeliveryOption : receiver.DeliveryMethod
						},
						PersonalMessage = "",
						MetaData = new Dictionary<string, object>()
						{
							{"CityCode",  receiver.PickupCity},
							{"CityName",  receiver.City},
							{"StateCode",  receiver.PickupState_Province},
							{"StateName",  receiver.State_Province},
							{"TestQuestionOption", "Test Question Option"},
							{"TestQuestion", "Test Question"},
							{"TestAnswer", "Test Answer"},
							{"IsFixedOnSend", "false"}		
						},
						ReferenceNo = "12323412"
					};

					serviceResponse = Client.GetMoneyTransferFee(long.Parse(CustomerSession.CustomerSessionId), feeRequest, MgiContext);
					FeeResponse feeResponse = serviceResponse.Result as FeeResponse;
					FeeInformation feeInformation = new FeeInformation();
					if (feeResponse.FeeInformations.Count > 0)
					{
						feeInformation = feeResponse.FeeInformations.FirstOrDefault();

						ValidateRequest validateRequest = new ValidateRequest()
						{
							TransferType = TransferType.SendMoney,
							TransactionId = feeResponse.TransactionId,
							ReceiverId = 0,

							Amount = feeInformation.Amount,
							Fee = feeInformation.Fee,
							Tax = feeInformation.Tax,
							OtherFee = feeInformation.OtherFee,
							MessageFee = feeInformation.OtherFee,

							PersonalMessage = "",
							PromoCode = "",
							IdentificationQuestion = "Test Question",
							IdentificationAnswer = "Test Answer",
							DeliveryService = "WILL_CALL",
							State = receiver.PickupState_Province,
							ReceiveCurrency = currencyCode,
							ReceiverFirstName = feeRequest.ReceiverFirstName,
							ReceiverLastName = feeRequest.ReceiverFirstName,
							ReceiverSecondLastName = feeRequest.ReceiverLastName,

							MetaData = new Dictionary<string, object>()
					       {
								{"ExpectedPayoutCity",receiver.City}, 
								{"ExpectedPayoutStateCode",receiver.State_Province}, 
								{"ProceedWithLPMTError","false"},
								{"ReceiveAgentAbbr", ""},
					      }
						};

						serviceResponse = Client.ValidateTransfer(long.Parse(CustomerSession.CustomerSessionId), validateRequest, MgiContext);
						ValidateResponse response = serviceResponse.Result as ValidateResponse;
						Response cartResponse = Client.ShoppingCart(CustomerSession.CustomerSessionId);
                        if(VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
                        ShoppingCart cart = cartResponse.Result as ShoppingCart;
						var status = Client.Checkout(CustomerSession.CustomerSessionId, 200, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
						Assert.AreEqual(ShoppingCartCheckoutStatus.Completed, status);
					}
					else
					{
						throw new Exception("No Fee available for this transaction");
					}
				}
			}
			else
			{
				throw new Exception("No receivers available for this customer");
			}
		}

	}

}
