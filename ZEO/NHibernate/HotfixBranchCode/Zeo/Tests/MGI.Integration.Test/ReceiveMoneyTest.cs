
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Models;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Integration.Test
{
	[TestFixture]
	class ReceiveMoneyTest : BaseFixture
	{
		[SetUp]
		public void Setup()
		{
			Client = new Desktop();
		}

		[Test]
		public void Can_Perform_WU_ReceiveMoney()
		{
			//GetChannelPartnerDataCarver();
			GetChannelPartnerDataSynovus();

            AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "ADNEY", new DateTime(1985, 02, 02));

            CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

            MgiContext.AlloyId = AlloyId;
            MgiContext.WUCounterId = "13139925";
            MgiContext.RequestType = RequestType.HOLD.ToString();
            MgiContext.IsReferral = false;
            MgiContext.ChannelPartnerName = ChannelPartnerName;
            MgiContext.IsAvailable = true;

            List<Receiver> receivers = Client.GetFrequentReceivers(long.Parse(CustomerSession.CustomerSessionId), MgiContext);
            if (receivers.Count == 0)
            {
                CreateReceiver_WU(long.Parse(CustomerSession.CustomerSessionId));
                receivers = Client.GetFrequentReceivers(long.Parse(CustomerSession.CustomerSessionId), MgiContext);
            }
            Receiver receiver = null;

			if (receivers.Count > 0)
			{
				receiver = receivers.FirstOrDefault();

				if (receiver != null)
				{
					string currencyCode = Client.GetCurrencyCode(Convert.ToInt64(CustomerSession.CustomerSessionId), receiver.PickupCountry, MgiContext);

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

                    FeeResponse feeResponse = Client.GetMoneyTransferFee(long.Parse(CustomerSession.CustomerSessionId), feeRequest, MgiContext);
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

						ValidateResponse response = Client.ValidateTransfer(long.Parse(CustomerSession.CustomerSessionId), validateRequest, MgiContext);
						ShoppingCart shoppingCartDetail = Client.ShoppingCart(CustomerSession.CustomerSessionId);
						ShoppingCartCheckoutStatus shoppingCartCheckoutStatus = Client.Checkout(CustomerSession.CustomerSessionId, 200, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);

						ShoppingCart cart = new ShoppingCart();
						MoneyTransferTransaction trans = new MoneyTransferTransaction();
						if (shoppingCartCheckoutStatus == ShoppingCartCheckoutStatus.Completed)
						{
							cart = Client.ShoppingCart(shoppingCartDetail.CustomerSessionId.ToString());
							Client.CloseShoppingCart(long.Parse(shoppingCartDetail.CustomerSessionId.ToString()), MgiContext);

							string MTCN = cart.MoneyTransfers[0].ConfirmationNumber;
							trans = Stage_ReceiveMoney_WU(MTCN);
						}

						cart = GetShoppingCartDetails(CustomerSession);

						MoneyTransfer mTransfer = null;
						if (cart != null && cart.MoneyTransfers != null)
							mTransfer = cart.MoneyTransfers.FirstOrDefault(c => c.ConfirmationNumber == trans.ConfirmationNumber);

						var Receivestatus = Client.Checkout(CustomerSession.CustomerSessionId, 200, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);

						UpdateCounterId();

						Assert.AreEqual(ShoppingCartCheckoutStatus.Completed, Receivestatus);
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

		private MoneyTransferTransaction Stage_ReceiveMoney_WU(string mtcn)
		{
			ReceiveMoneyRequest request = new ReceiveMoneyRequest()
			{
				ConfirmationNumber = mtcn
			};

			long customerSesionId = long.Parse(CustomerSession.CustomerSessionId);

			MoneyTransferTransaction transaction = Client.GetReceiveTransaction(customerSesionId, request, MgiContext);

			ValidateRequest validateRequest = new ValidateRequest()
			{
				TransactionId = long.Parse(transaction.TransactionID),
				TransferType = TransferType.RecieveMoney,
				Amount = decimal.Parse(transaction.DestinationPrincipalAmount.ToString())
			};
			ValidateResponse response = Client.ValidateTransfer(customerSesionId, validateRequest, MgiContext);
			return transaction;
		}

		private ShoppingCart GetShoppingCartDetails(CustomerSession customerSession)
		{
			ShoppingCart shoppingCart = Client.ShoppingCart(customerSession.CustomerSessionId);
			return shoppingCart;
		}

		[Test]
		public void Can_Perform_MGI_ReceiveMoney()
		{
			GetChannelPartnerDataMGI();

			AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "John", new DateTime(1987, 02, 09));
			CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

			MgiContext.AlloyId = AlloyId;
			MgiContext.WUCounterId = "13139925";
			MgiContext.RequestType = RequestType.HOLD.ToString();
			MgiContext.IsReferral = false;
			MgiContext.ChannelPartnerName = ChannelPartnerName;
			MgiContext.IsAvailable = true;

			List<Receiver> receivers = Client.GetFrequentReceivers(long.Parse(CustomerSession.CustomerSessionId), MgiContext);
			if (receivers.Count == 0)
			{
				CreateReceiver_MGI(long.Parse(CustomerSession.CustomerSessionId));
				receivers = Client.GetFrequentReceivers(long.Parse(CustomerSession.CustomerSessionId), MgiContext);
			}

			Receiver receiver = null;

			if (receivers.Count > 0)
			{
				receiver = receivers.FirstOrDefault();
				string currencyCode = Client.GetCurrencyCode(Convert.ToInt64(CustomerSession.CustomerSessionId), receiver.PickupCountry, MgiContext);

				if (receiver != null)
				{
					FeeRequest feeRequest = new FeeRequest()
					{
						Amount = 150,
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

					FeeResponse feeResponse = Client.GetMoneyTransferFee(long.Parse(CustomerSession.CustomerSessionId), feeRequest, MgiContext);
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
						ValidateResponse response = Client.ValidateTransfer(long.Parse(CustomerSession.CustomerSessionId), validateRequest, MgiContext);
						ShoppingCart shoppingCartDetail = Client.ShoppingCart(CustomerSession.CustomerSessionId);
						ShoppingCartCheckoutStatus shoppingCartCheckoutStatus = Client.Checkout(CustomerSession.CustomerSessionId, 200, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);

						ShoppingCart cart = new ShoppingCart();
						MoneyTransferTransaction trans = new MoneyTransferTransaction();
						if (shoppingCartCheckoutStatus == ShoppingCartCheckoutStatus.Completed)
						{
							cart = Client.ShoppingCart(shoppingCartDetail.CustomerSessionId.ToString());
							Client.CloseShoppingCart(long.Parse(shoppingCartDetail.CustomerSessionId.ToString()), MgiContext);

							string mtcn = cart.MoneyTransfers[0].ConfirmationNumber;
							trans = Stage_ReceiveMoney_MGI(mtcn);
						}

						cart = GetShoppingCartDetails(CustomerSession);

						MoneyTransfer mTransfer = null;
						if (cart != null && cart.MoneyTransfers != null)
							mTransfer = cart.MoneyTransfers.FirstOrDefault(c => c.ConfirmationNumber == trans.ConfirmationNumber);


						var Receivestatus = Client.Checkout(CustomerSession.CustomerSessionId, 200, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
						Assert.AreEqual(ShoppingCartCheckoutStatus.Completed, Receivestatus);
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


		private MoneyTransferTransaction Stage_ReceiveMoney_MGI(string mtcn)
		{
			ReceiveMoneyRequest request = new ReceiveMoneyRequest()
			{
				ConfirmationNumber = mtcn
			};

			long customerSesionId = long.Parse(CustomerSession.CustomerSessionId);

			MoneyTransferTransaction transaction = Client.GetReceiveTransaction(customerSesionId, request, MgiContext);

			var validateRequest = new ValidateRequest()
			{

				Amount = Convert.ToDecimal(transaction.AmountToReceiver),
				TransactionId = long.Parse(transaction.TransactionID),
				TransferType = TransferType.RecieveMoney,
				ReferenceNumber = transaction.ConfirmationNumber,
				ReceiveCurrency = transaction.DestinationCurrencyCode,
				MetaData = new Dictionary<string, object>()
			};

			ValidateResponse response = Client.ValidateTransfer(customerSesionId, validateRequest, MgiContext);
			return transaction;
		}

	}
}
