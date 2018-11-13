using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.MoneyTransfer.MG.Contract;
using System.ServiceModel;
using MGI.Cxn.MoneyTransfer.MG.Data;
using MGI.Cxn.MoneyTransfer.MG.AgentConnectService;
using MGI.Common.Util;

namespace MGI.Cxn.MoneyTransfer.MG.Impl
{
	public class SimulatorIO : IIO
	{
		public TLoggerCommon MongoDBLogger { private get; set; }
		#region Public methods

		public DetailLookupResponse DetailLookupRequest(DetailLookupRequest detailLookupRequest)
		{
			DetailLookupResponse lookUpResponse = new DetailLookupResponse();
			lookUpResponse = MockDetailLookup(detailLookupRequest);
			return lookUpResponse;
		}

		public SendValidationResponse SendValidation(SendValidationRequest validationRequest, MGIContext mgiContext)
		{
			SendValidationResponse validationResponse = new SendValidationResponse();

			switch (validationRequest.receiverCountry)
			{
				case "MEX":
					validationResponse = MockSendValidation_For_Mexico(validationRequest);
					break;
				case "CAN":
					validationResponse = MockSendValidation_For_Canada(validationRequest);
					break;
				case "AFG":
					validationResponse = MockSendValidation_For_AFG(validationRequest);
					break;
				default:
					validationResponse = MockSendValidation_For_Mexico(validationRequest);
					break;
			}
			return validationResponse;
		}

		public CommitTransactionResponse SendCommit(CommitTransactionRequest commitRequest, MGIContext mgiContext)
		{
			CommitTransactionResponse commitResponse = MockSendCommit(commitRequest);
			return commitResponse;
		}

		public AmendTransactionResponse AmendTransaction(AmendTransactionRequest amendTransactionRequest)
		{
			AmendTransactionResponse amendTransactionResponse = new AmendTransactionResponse();
			amendTransactionResponse = MockAmendTransaction(amendTransactionRequest);
			return amendTransactionResponse;
		}

		public SendReversalResponse SendReversal(SendReversalRequest sendReversalRequest, MGIContext mgiContext)
		{
			SendReversalResponse sendReversalResponse = MockSendReversal(sendReversalRequest);
			return sendReversalResponse;
		}

        public ReferenceNumberResponse RequestReferenceNumber(ReferenceNumberRequest referenceNumberRequest)
        {
            ReferenceNumberResponse responseReferenceNumber = new ReferenceNumberResponse();
            responseReferenceNumber.transactionStatus = transactionStatus.AVAIL;
            responseReferenceNumber.okForAgent = true;
			responseReferenceNumber.mgiTransactionSessionID = "97087115E1440756770388NN";
			responseReferenceNumber.receiverFirstName = "RECEIVER";
			responseReferenceNumber.receiverLastName = "LNAME";
			responseReferenceNumber.deliveryOption = "WILL_CALL";
			responseReferenceNumber.transactionStatus = transactionStatus.AVAIL;
			responseReferenceNumber.receiveAmount = 80;
			responseReferenceNumber.originalSendAmount = 80;
			responseReferenceNumber.originalSendFee = 12.50m;
			responseReferenceNumber.originalExchangeRate = 1.0000m;
			responseReferenceNumber.okForPickup = true;
            responseReferenceNumber.referenceNumber = referenceNumberRequest.referenceNumber;
            responseReferenceNumber.dateTimeSent = DateTime.Now;
            responseReferenceNumber.okForPickup = true;
            responseReferenceNumber.originatingCountry = "US";
			responseReferenceNumber.notOkForPickupReasonCode = "0";
			responseReferenceNumber.minutesUntilOkForPickup = "0";
			responseReferenceNumber.okForAgent = true;
			responseReferenceNumber.agentCheckNumber = "00000000";
			responseReferenceNumber.customerCheckNumber = "00000000";
			responseReferenceNumber.senderHomePhone = "9883478347439";
			responseReferenceNumber.senderFirstName = "SimulatorFName";
			responseReferenceNumber.senderLastName="Simulator";
            return responseReferenceNumber;
        }

        public ReceiveValidationResponse ReceiveValidation(ReceiveValidationRequest receiveValidationRequest)
        {
            ReceiveValidationResponse receiveValidationResponse = new ReceiveValidationResponse();
            receiveValidationResponse.doCheckIn = false;
            receiveValidationResponse.timeStamp = DateTime.Now;
            receiveValidationResponse.flags = 19;
            receiveValidationResponse.readyForCommit = true;
            return receiveValidationResponse;
        }

		public CommitTransactionResponse ReceiveCommit(CommitTransactionRequest commitRequest, MGIContext mgiContext)
		{
			CommitTransactionResponse commitResponse = new CommitTransactionResponse();
            commitResponse.doCheckIn = false;
            commitResponse.timeStamp = DateTime.Now;
            commitResponse.flags = 17;
            commitResponse.referenceNumber = GenerateMTCN(8);
            commitResponse.transactionDateTime = DateTime.Now;
            return commitResponse;
		}

		#endregion

		#region Private methods

		private SendValidationResponse MockSendValidation_For_Mexico(SendValidationRequest validationRequest)
		{
			SendValidationResponse validationResponse = new SendValidationResponse();
			validationResponse.doCheckIn = false;
			validationResponse.timeStamp = DateTime.Now;
			validationResponse.flags = 17;
			validationResponse.mgiTransactionSessionID = "9736251E1439962670320NN";
			validationResponse.readyForCommit = true;
			validationResponse.sendAmounts = new SendAmountInfo()
			{
				sendAmount = validationRequest.amount,
				sendCurrency = validationRequest.sendCurrency,
				totalSendFees = 9.99m,
				totalDiscountAmount = 0.00m,
				totalSendTaxes = 0.00m,
				totalAmountToCollect = Convert.ToDecimal(validationRequest.amount) + 12.50m,
				detailSendAmounts = new AmountInfo[] 
				{
					  new AmountInfo()
					  {
						  amountType="nonMgiSendTax",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiSendFee",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiNonDiscountedSendFee",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="discountedMgiSendFee",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiSendTax",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
				  		  new AmountInfo()
					  {
						  amountType="totalMgiCollectedFeesAndTaxes",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalAmountToMgi",
						  amount=89.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },

					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonMgiSendFeesAndTaxes",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalSendFeesAndTaxes",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
				}
			};
			validationResponse.receiveAmounts = new ReceiveAmountInfo()
			{
				receiveAmount = 1292.18m,
				receiveCurrency = validationRequest.receiveCurrency,
				validCurrencyIndicator = true,
				payoutCurrency = validationRequest.receiveCurrency,
				totalReceiveFees = 0.00m,
				totalReceiveTaxes = 0.00m,
				totalReceiveAmount = 1292.18m,
				receiveFeesAreEstimated = false,
				receiveTaxesAreEstimated = false,
				detailReceiveAmounts = new AmountInfo[] 
				{
					 new AmountInfo()
					  {
						  amountType="nonMgiReceiveTax",
						  amount=0.00m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiReceiveFee",
						  amount=0.00m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveFee",
						  amount=0.00m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveTax",
						  amount=0.00m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
				}
			};
			validationResponse.exchangeRateApplied = 16.1522m;
			validationResponse.receiveFeeDisclosureText = false;
			validationResponse.receiveTaxDisclosureText = false;

			return validationResponse;
		}

		private SendValidationResponse MockSendValidation_For_Canada(SendValidationRequest validationRequest)
		{
			SendValidationResponse validationResponse = new SendValidationResponse();
			validationResponse.doCheckIn = false;
			validationResponse.timeStamp = DateTime.Now;
			validationResponse.flags = 17;
			validationResponse.mgiTransactionSessionID = "9736251E1440481379046NN";
			validationResponse.readyForCommit = true;
			validationResponse.promotionalMessage = new TextTranslation[]
			{
				new TextTranslation
				{
					longLanguageCode="ENG",
					textTranslation="Please visit www.moneygram.com to view your MoneyGram Rewards information."
				},
				new TextTranslation
				{
					longLanguageCode="SPA",
					textTranslation="Por favor, visite www.moneygram.com para ver su MoneyGram Rewards de la informacion."
				}
			};
			validationResponse.sendAmounts = new SendAmountInfo()
			{
				sendAmount = validationRequest.amount,
				sendCurrency = validationRequest.sendCurrency,
				totalSendFees = 12.50m,
				totalDiscountAmount = 0.00m,
				totalSendTaxes = 0.00m,
				totalAmountToCollect = Convert.ToDecimal(validationRequest.amount) + 12.50m,
				detailSendAmounts = new AmountInfo[] 
				{
					  new AmountInfo()
					  {
						  amountType="nonMgiSendTax",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiSendFee",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiNonDiscountedSendFee",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="discountedMgiSendFee",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiSendTax",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
				  		  new AmountInfo()
					  {
						  amountType="totalMgiCollectedFeesAndTaxes",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalAmountToMgi",
						  amount=89.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },

					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonMgiSendFeesAndTaxes",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalSendFeesAndTaxes",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
				}
			};
			validationResponse.receiveAmounts = new ReceiveAmountInfo()
			{
				receiveAmount = 128.25m,
				receiveCurrency = validationRequest.receiveCurrency,
				validCurrencyIndicator = true,
				payoutCurrency = validationRequest.receiveCurrency,
				totalReceiveFees = 8.88m,
				totalReceiveTaxes = 5.00m,
				totalReceiveAmount = 114.37m,
				receiveFeesAreEstimated = false,
				receiveTaxesAreEstimated = true,
				detailReceiveAmounts = new AmountInfo[] 
				{
					 new AmountInfo()
					  {
						  amountType="nonMgiReceiveTax",
						  amount=5.00m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiReceiveFee",
						  amount=8.88m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveFee",
						  amount=0.00m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveTax",
						  amount=0.00m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
				}
			};
			validationResponse.exchangeRateApplied = 1.2825m;
			validationResponse.receiveFeeDisclosureText = false;
			validationResponse.receiveTaxDisclosureText = false;

			return validationResponse;
		}

		private SendValidationResponse MockSendValidation_For_AFG(SendValidationRequest validationRequest)
		{
			SendValidationResponse validationResponse = new SendValidationResponse();
			validationResponse.doCheckIn = false;
			validationResponse.timeStamp = DateTime.Now;
			validationResponse.flags = 17;
			validationResponse.mgiTransactionSessionID = "9736251E1440481379046NN";
			validationResponse.readyForCommit = true;
			validationResponse.promotionalMessage = new TextTranslation[]
			{
				new TextTranslation
				{
					longLanguageCode="ENG",
					textTranslation="Please visit www.moneygram.com to view your MoneyGram Rewards information."
				},
				new TextTranslation
				{
					longLanguageCode="SPA",
					textTranslation="Por favor, visite www.moneygram.com para ver su MoneyGram Rewards de la informacion."
				}
			};
			validationResponse.sendAmounts = new SendAmountInfo()
			{
				sendAmount = validationRequest.amount,
				sendCurrency = validationRequest.sendCurrency,
				totalSendFees = 12.50m,
				totalDiscountAmount = 0.00m,
				totalSendTaxes = 0.00m,
				totalAmountToCollect = Convert.ToDecimal(validationRequest.amount) + 12.50m,
				detailSendAmounts = new AmountInfo[] 
				{
					  new AmountInfo()
					  {
						  amountType="nonMgiSendTax",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiSendFee",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiNonDiscountedSendFee",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="discountedMgiSendFee",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiSendTax",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
				  		  new AmountInfo()
					  {
						  amountType="totalMgiCollectedFeesAndTaxes",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalAmountToMgi",
						  amount=89.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },

					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonMgiSendFeesAndTaxes",
						  amount=0.00m,
						  amountCurrency=validationRequest.sendCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="totalSendFeesAndTaxes",
						  amount=9.99m,
						  amountCurrency=validationRequest.sendCurrency
					  },
				}
			};
			validationResponse.receiveAmounts = new ReceiveAmountInfo()
			{
				receiveAmount = 128.25m,
				receiveCurrency = validationRequest.receiveCurrency,
				validCurrencyIndicator = true,
				payoutCurrency = validationRequest.receiveCurrency,
				totalReceiveFees = 8.88m,
				totalReceiveTaxes = 5.00m,
				totalReceiveAmount = 114.37m,
				receiveFeesAreEstimated = false,
				receiveTaxesAreEstimated = true,
				detailReceiveAmounts = new AmountInfo[] 
				{
					 new AmountInfo()
					  {
						  amountType="nonMgiReceiveTax",
						  amount=5.00m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiReceiveFee",
						  amount=8.88m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveFee",
						  amount=0.00m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveTax",
						  amount=0.00m,
						  amountCurrency= validationRequest.receiveCurrency
					  },
				}
			};
			validationResponse.exchangeRateApplied = 62.9937m;
			validationResponse.receiveFeeDisclosureText = false;
			validationResponse.receiveTaxDisclosureText = false;

			return validationResponse;
		}

		private DetailLookupResponse MockDetailLookup(DetailLookupRequest detailLookupRequest)
		{
			DetailLookupResponse response = new DetailLookupResponse();
			response.doCheckIn = false;
			response.timeStamp = DateTime.Now;
			response.transactionStatus = transactionStatus.AVAIL;
			response.flags = 17;
			response.dateTimeSent = DateTime.Now;
			response.referenceNumber = GenerateMTCN(8);
			response.freqCustCardNumber = "0000000000000000";
			response.receiveCountry = "MEX";
			response.deliveryOption = "WILL_CALL";
			response.senderFirstName = "JOHN";
			response.senderLastName = "SMITH";
			response.senderAddress = "1200 STEPHENSON HIGHWAY";
			response.senderCity = "TROY";
			response.senderCountry = "USA";
			response.senderState = "MI";
			response.senderZipCode = "48083";
			response.senderHomePhone = "9738835532";
			response.receiverFirstName = "RECEIVERFNAME";
			response.receiverLastName = "LNAME";
			response.senderPhotoIdType = photoIdType.STA;
			response.senderPhotoIdNumber = "K23379233";
			response.senderPhotoIdState = "MI";
			response.senderPhotoIdCountry = "USA";
			response.senderLegalIdType = legalIdType.SSN;
			response.senderLegalIdNumber = "987894759";
			response.senderDOB = DateTime.ParseExact("1987-02-09", "yyyy-MM-dd", null);
			response.senderBirthCountry = "USA";
			response.validIndicator = true;
			response.expectedDateOfDelivery = DateTime.Now;
			response.redirectIndicator = false;
			response.feeRefundRequired = true;
			response.sendAmounts = new SendAmountInfo()
			{
				sendAmount = 90,
				sendCurrency = "USD",
				totalSendFees = 9.99M,
				totalDiscountAmount = 0.00M,
				totalSendTaxes = 0.00M,
				detailSendAmounts = new AmountInfo[]
				{
				  new AmountInfo()
				  {
					  amountType="nonMgiSendFee",
					  amount=0.00m,
					  amountCurrency="USD"
				  },
				  new AmountInfo()
				  {
					  amountType="totalMgiCollectedFeesAndTaxes",
					  amount=9.99m,
					  amountCurrency="USD"
				  },
				  new AmountInfo()
				  {
					  amountType="discountedMgiSendFee",
					  amount=9.99m,
					  amountCurrency="USD"
				  },
				  new AmountInfo()
				  {
					  amountType="nonMgiSendTax",
					  amount=0.00m,
					  amountCurrency="USD"
				  },
				  new AmountInfo()
				  {
					  amountType="mgiNonDiscountedSendFee",
					  amount=9.99m,
					  amountCurrency="USD"
				  },
				  new AmountInfo()
				  {
					  amountType="mgiSendTax",
					  amount=0.00m,
					  amountCurrency="USD"
				  },
				  new AmountInfo()
				  {
					  amountType="totalAmountToMgi",
					  amount=89.99m,
					  amountCurrency="USD"
				  },
				  new AmountInfo()
				  {
					  amountType="totalNonDiscountedFees",
					  amount=9.99m,
					  amountCurrency="USD"
				  },
				  new AmountInfo()
				  {
					  amountType="totalNonMgiSendFeesAndTaxes",
					  amount=0.00m,
					  amountCurrency="USD"
				  },
				  new AmountInfo()
				  {
					  amountType="totalSendFeesAndTaxes",
					  amount=9.99m,
					  amountCurrency="USD"
				  },
				}
			};
			response.receiveAmounts = new ReceiveAmountInfo()
			{
				receiveAmount = 1292.18m,
				receiveCurrency = "MXN",
				validCurrencyIndicator = true,
				payoutCurrency = "MXN",
				totalReceiveFees = 0.00m,
				totalReceiveTaxes = 0.00m,
				totalReceiveAmount = 1292.18m,
				receiveFeesAreEstimated = false,
				receiveTaxesAreEstimated = false,
				detailReceiveAmounts = new AmountInfo[] 
				{
					 new AmountInfo()
					  {
						  amountType="nonMgiReceiveTax",
						  amount=0.00m,
						  amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiReceiveFee",
						  amount=0.00m,
						  amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveFee",
						  amount=0.00m,
						  amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="totalSendFeesAndTaxes",
						  amount=9.99m,
						  amountCurrency="MXN"
					  },
				}
			};
			response.exchangeRateApplied = 16.152246M;
			return response;
		}

		private CommitTransactionResponse MockSendCommit(CommitTransactionRequest commitRequest)
		{
			CommitTransactionResponse commitResponse = new CommitTransactionResponse();
			commitResponse.doCheckIn = false;
			commitResponse.flags = 17;
			commitResponse.timeStamp = DateTime.Now;
			commitResponse.referenceNumber = GenerateMTCN(8);
			commitResponse.freePhoneCallPIN = "6458283300";
			commitResponse.tollFreePhoneNumber = "18889255346";
			commitResponse.expectedDateOfDelivery = DateTime.Now;
			commitResponse.transactionDateTime = DateTime.Now;			
			return commitResponse;
		}

		private AmendTransactionResponse MockAmendTransaction(AmendTransactionRequest amendTransactionRequest)
		{
			AmendTransactionResponse amedResponse = new AmendTransactionResponse();
			amedResponse.transactionSucceeded = true;
			amedResponse.timeStamp = DateTime.Now;
			amedResponse.flags = 17;
			amedResponse.doCheckIn = false;

			return amedResponse;
		}

		private SendReversalResponse MockSendReversal(SendReversalRequest sendReversalRequest)
		{
			SendReversalResponse sendReversalResponse = new SendReversalResponse();
			sendReversalResponse.doCheckIn = false;
			sendReversalResponse.refundFaceAmount = sendReversalRequest.feeAmount;
			sendReversalResponse.refundFaceAmountSpecified = false;
			sendReversalResponse.refundTotalAmount = sendReversalRequest.sendAmount;
			sendReversalResponse.refundTotalAmountSpecified = false;
			sendReversalResponse.timeStamp = DateTime.Now;
			sendReversalResponse.reversalType = sendReversalType.R;
			sendReversalResponse.totalCheckAmount = sendReversalRequest.customerCheckAmount;
			sendReversalResponse.transactionDateTime = DateTime.Now;
			sendReversalResponse.refundTotalAmountSpecified = false;
			return sendReversalResponse;
		}

		private string GenerateMTCN(int length)
		{
			RandomCryptoServiceProvider randomCryptoServiceProvider = new RandomCryptoServiceProvider();
			string mtcn = string.Empty;
			int i;
			for (i = 0; i < length; i++)
			{
				mtcn += randomCryptoServiceProvider.Next(0, 9).ToString();
			}
			return mtcn;
		}

		#endregion
	}
}
	