using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.CXN.MG.Common.Contract;
using MGI.CXN.MG.Common.AgentConnectService;
using System.ServiceModel;
using MGI.CXN.MG.Common.Data;
using MGI.Common.Util;

namespace MGI.CXN.MG.Common.Impl
{
	public class SimulatorIO : IMGCommonIO
	{
		public TLoggerCommon MongoDBLogger { private get; set; }
		#region Public Methods

		public FeeLookupResponse GetFee(FeeLookupRequest feeLookupRequest, MGIContext mgiContext)
		{
			FeeLookupResponse response = new FeeLookupResponse();
			switch (feeLookupRequest.receiveCountry)
			{
				case "MEX":
					response = MockfeeLookup_For_Mexico(feeLookupRequest);
					break;
				case "CAD":
					response = MockfeeLookup_For_Canada(feeLookupRequest);
					break;
				case "AFG":
					response = MockfeeLookup_For_AFG(feeLookupRequest);
					break;
				default:
					{
						throw new Exception("Mock Fee look up is not available for this country");
					}
			}
			response = MockfeeLookup_For_Mexico(feeLookupRequest);
			return response;
		}

		public CommitTransactionResponse CommitTransaction(CommitTransactionRequest commitRequest, MGIContext mgiContext)
		{
			CommitTransactionResponse commitResponse = new CommitTransactionResponse();

			commitResponse.referenceNumber = GenerateMTCN(3);
			commitResponse.flags = 17;
			commitResponse.timeStamp = DateTime.Now;
			commitResponse.transactionDateTime = DateTime.Now;
			commitResponse.tollFreePhoneNumber = "18889255346";
			commitResponse.doCheckIn = false;
			commitResponse.expectedDateOfDelivery = DateTime.Now;
			commitResponse.expectedDateOfDeliverySpecified = false;
			commitResponse.freePhoneCallPIN = "6458283300";

			return commitResponse;
		}

		public StateRegulatorResponse GetDoddFrankStateRegulatorInfo(StateRegulatorRequest request, MGIContext mgiContext)
		{
			StateRegulatorResponse regulatorResponse = new StateRegulatorResponse();

			regulatorResponse.TimeStamp = DateTime.Now;
			regulatorResponse.Version = "1.0";
			regulatorResponse.StateRegulators = new List<StateRegulator>()
			{
				   new StateRegulator()
				   {
					   DFJurisdiction="DG",
					   LanguageCode="ENG",
					   StateRegulatorPhone="202.727.8000",
					   StateRegulatorURL="http://disb.dc.gov",
					   Translation="District of Columbia Department of insurance, securities and banking"
				   },
				   new StateRegulator()
				   {
					   DFJurisdiction="AL",
					   LanguageCode="ENG",
					   StateRegulatorPhone="1.800.222.1253",
					   StateRegulatorURL="http://asc.state.al.us/Complaint.htm",
					   Translation="Alabama Securities Commission"
				   },
				   new StateRegulator()
				   {
					   DFJurisdiction="WA",
					   LanguageCode="ENG",
					   StateRegulatorPhone="360-902-8703",
					   StateRegulatorURL="http://www.dfi.wa.gov/cs/default.htm",
					   Translation="Washington Department of Financial Institutions"
				   },
				   new StateRegulator()
				   {
					   DFJurisdiction="OK",
					   LanguageCode="ENG",
					   StateRegulatorPhone="405-521-2782",
					   StateRegulatorURL="http://www.ok.gov/banking/",
					   Translation="Oklahoma State Banking Department"
				   },
				   new StateRegulator()
				   {
					   DFJurisdiction="MI",
					   LanguageCode="ENG",
					   StateRegulatorPhone="877-999-6442",
					   StateRegulatorURL="http://www.michigan.gov/lara/",
					   Translation="Michigan Department of Insurance and Financial Affairs"
				   }			
			};

			return regulatorResponse;
		}

		public GetFieldsForProductResponse GetFieldsForProduct(GetFieldsForProductRequest request, MGIContext mgiContext)
		{
			GetFieldsForProductResponse fieldProductResponse = new GetFieldsForProductResponse();
			fieldProductResponse.doCheckIn = false;
			fieldProductResponse.timeStamp = DateTime.Now;
			fieldProductResponse.flags = 17;
			fieldProductResponse.fqdoInfo = new FQDOInfo()
			 {
				 deliveryOption = request.deliveryOption,
				 receiveCountry = request.receiveCountry,
				 receiveCurrency = request.receiveCurrency,
				 deliveryOptionDisplayName = GetDeliveryOptionName(request.deliveryOption)
			 };
			fieldProductResponse.productFieldInfo = new ProductFieldInfo[] 
			 {
			    new ProductFieldInfo()
			     {
					 xmlTag="operatorName",
					 visibility=ProductFieldInfoVisibility.SUP_OPT,
					 fieldLabel="operatorName",
					 displayOrder=1,
					 fieldCategory="Operational",
					 dynamic=false,
					 fieldMax=7,
					 fieldMin=0,
					 dataType=dataTypeCode.@string,
					 enumerated=false,
					 arrayLength=-1
			     },
				  new ProductFieldInfo()
			     {
					 xmlTag="pcTerminalNumber",
					 visibility=ProductFieldInfoVisibility.SUP_OPT,
					 fieldLabel="pcTerminalNumber",
					 displayOrder=2,
					 fieldCategory="Operational",
					 dynamic=false,
					 fieldMax=10,
					 fieldMin=0,
					 dataType=dataTypeCode.@string,
					 enumerated=false,
					 arrayLength=-1
			     },
				 new ProductFieldInfo()
			     {
					 xmlTag="consumerId",
					 visibility=ProductFieldInfoVisibility.REQ,
					 fieldLabel="consumerId",
					 displayOrder=3,
					 fieldCategory="Operational",
					 dynamic=false,
					 fieldMax=20,
					 fieldMin=0,
					 dataType=dataTypeCode.@string,
					 enumerated=false,
					 arrayLength=-1
			     },
				 new ProductFieldInfo()
			     {
					 xmlTag="mgiTransactionSessionID",
					 visibility=ProductFieldInfoVisibility.REQ,
					 fieldLabel="mgiTransactionSessionID",
					 displayOrder=7,
					 fieldCategory="Operational",
					 dynamic=false,
					 fieldMax=32,
					 fieldMin=0,
					 dataType=dataTypeCode.@string,
					 enumerated=false,
					 arrayLength=-1
			     }
			 };

			return fieldProductResponse;
		}

		public CTResponse GetMetaData(BaseRequest request, MGIContext mgiContext)
		{
			CTResponse metaDataResponse = new CTResponse();

			metaDataResponse.Version = "1.0";
			metaDataResponse.TimeStamp = DateTime.Now;
			metaDataResponse.Countries = new List<Country>()
			{
				  new Country()
				  {
					  CountryCode="AND",
					  CountryLegacyCode="AD",
					  CountryName="ANDORRA"
				  },
				  new Country()
				  {
					  CountryCode="AIA",
					  CountryLegacyCode="AI",
					  CountryName="ANGULLA"
				  },
				  new Country()
				  {
					  CountryCode="AZZ",
					  CountryLegacyCode="AZ",
					  CountryName="AZERBAIJAN"
				  },
				  new Country()
				  {
					  CountryCode="BVT",
					  CountryLegacyCode="BV",
					  CountryName="BOUVET ISLAND"
				  },
				  new Country()
				  {
					  CountryCode="IOT",
					  CountryLegacyCode="IO",
					  CountryName="BRITISH INDIAN OCEAN TERRITORY"
				  },
			};
			metaDataResponse.CountryCurrencies = new List<CountryCurrency>() 
			{
				  new CountryCurrency()
				  {
				   BaseCurrency="USD",
				   CountryCode="ABW",
				   DeliveryOption="WILL_CALL",
				   IndicativeRateAvailable="True",
				   LocalCurrency="AWG",
				   ReceiveCurrency="USD"
				  },
				  new CountryCurrency()
				  {
				   BaseCurrency="USD",
				   CountryCode="AFG",
				   DeliveryOption="OVERNIGHT2ANY",
				   IndicativeRateAvailable="True",
				   LocalCurrency="AFN",
				   ReceiveCurrency="AFN"
				  }
			};
			metaDataResponse.Currencies = new List<Currency>()
			{
				  new Currency()
				  {
				    CurrencyCode="LVL",
					CurrencyName="Latvian Lats",
					CurrencyPrecision="2"
				  },
				  new Currency()
				  {
				    CurrencyCode="SCR",
					CurrencyName="Seychellas Lats",
					CurrencyPrecision="2"
				  },
				  new Currency()
				  {
				    CurrencyCode="LVL",
					CurrencyName="Latvian Lats",
					CurrencyPrecision="2"
				  }
			};
			metaDataResponse.DeliveryOptions = new List<DeliveryOption>()
			{
			      new DeliveryOption()
				  {
					  Delivery_Option="WILL CALL",
					  DeliveryOptionID="0",
					  DeliveryOptionName="10 Minutes service"
				  },
				  new DeliveryOption()
				  {
					  Delivery_Option="BANCOMER",
					  DeliveryOptionID="4",
					  DeliveryOptionName="Bancomer"
				  },
				  new DeliveryOption()
				  {
					  Delivery_Option="HDS_USD",
					  DeliveryOptionID="6",
					  DeliveryOptionName="Home Delivery DOP"
				  }			
			};
			metaDataResponse.StateProvinces = new List<StateProvince>()
			{
				  new StateProvince()
				  {
					  CountryCode="CAN",
					  StateProvinceCode="SK",
					  StateProvinceName="SASKATCHEWAN"
				  },
				  new StateProvince()
				  {
					  CountryCode="CAN",
					  StateProvinceCode="YT",
					  StateProvinceName="YUKON"
				  },
				  new StateProvince()
				  {
					  CountryCode="CAN",
					  StateProvinceCode="QC",
					  StateProvinceName="QUEBEC"
				  },
				  new StateProvince()
				  {
					  CountryCode="CAN",
					  StateProvinceCode="PC",
					  StateProvinceName="PRINCE EDWARD ISLAND"
				  },
				  new StateProvince()
				  {
					  CountryCode="CAN",
					  StateProvinceCode="ON",
					  StateProvinceName="ONTARIO"
				  }
			};

			return metaDataResponse;
		}

		public PhotoIDType GetPhotoIdType(string idType)
		{
			var photoIdType = PhotoIDType.GOV;

			switch (idType)
			{
				case "DRIVER'S LICENSE":
					photoIdType = PhotoIDType.DRV;
					break;
				case "EMPLOYMENT AUTHORIZATION CARD (EAD)":
					photoIdType = PhotoIDType.GOV;
					break;
				case "GREEN CARD / PERMANENT RESIDENT CARD":
					photoIdType = PhotoIDType.ALN;
					break;
				case "MILITARY ID":
					photoIdType = PhotoIDType.GOV;
					break;
				case "PASSPORT":
					photoIdType = PhotoIDType.PAS;
					break;
				case "U.S. STATE IDENTITY CARD":
					photoIdType = PhotoIDType.STA;
					break;
				case "INSTITUTO FEDERAL ELECTORAL":
					photoIdType = PhotoIDType.GOV;
					break;
				case "LICENCIA DE CONDUCIR":
					photoIdType = PhotoIDType.DRV;
					break;
				case "MATRICULA CONSULAR":
					photoIdType = PhotoIDType.GOV;
					break;
			}

			return photoIdType;
		}

		public Data.TranslationsResponse GetTransalations(Data.TranslationsRequest request, MGIContext mgiContext)
		{
			// AL-1205: This method is not used in alloy system. 
			throw new NotImplementedException();
		}

		#endregion

		#region Private Methods

		private FeeLookupResponse MockfeeLookup_For_Mexico(FeeLookupRequest feeLookupRequest)
		{
			FeeLookupResponse feeResponse = new FeeLookupResponse();
			feeResponse.doCheckIn = false;
			feeResponse.flags = 17;
			feeResponse.timeStamp = DateTime.Now;
			feeResponse.feeInfo = new FeeInfo[]
			{
			   new FeeInfo()
			   {
			      validReceiveAmount=1292.18m,
			      validReceiveCurrency="MXN",
				  validExchangeRate=16.1522m,
				  estimatedReceiveAmount=0m,
				  estimatedExchangeRate=0.0000m,
				  totalAmount=89.0m,
				  receiveCountry=feeLookupRequest.receiveCountry,
				  deliveryOption="WILL_CALL",
				  receiveAmountAltered=false,
				  revisedInformationalFee=false,
				  deliveryOptId="0",
				  deliveryOptDisplayName="10 Minute Service",
				  mgiTransactionSessionID="9736251E1440393767094NN",
				  sendAmountAltered=false,
				  sendAmounts=new SendAmountInfo()
				  {
				  sendAmount=80.00m,
				  sendCurrency="USD",
				  totalSendFees=9.99m,
				  totalDiscountAmount=0.00m,
				  totalSendTaxes=0.00m,
				  totalAmountToCollect=89.99m,
				  detailSendAmounts = new AmountInfo[] 
				   {
					  new AmountInfo()
					  {
						  amountType="nonMgiSendTax", amount=0.00m, amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiSendFee", amount=0.00m, amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiNonDiscountedSendFee", amount=9.99m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",amount=9.99m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="discountedMgiSendFee",amount=9.99m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiSendTax",amount=0.00m,amountCurrency="USD"
					  },
				  		  new AmountInfo()
					  {
						  amountType="totalMgiCollectedFeesAndTaxes",amount=9.99m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalAmountToMgi",amount=89.99m,amountCurrency="USD"
					  },

					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",amount=9.99m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonMgiSendFeesAndTaxes",amount=0.00m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalSendFeesAndTaxes",amount=9.99m,amountCurrency="USD"
					  }
				  }
				 },
				 receiveAmounts = new EstimatedReceiveAmountInfo()
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
					detailEstimatedReceiveAmounts = new AmountInfo[] 
					{
					 new AmountInfo()
					  {
						  amountType="nonMgiReceiveTax", amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiReceiveFee", amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveFee",amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveTax", amount=0.00m,amountCurrency="MXN"
					  }
					}
				  }
				},
				 new FeeInfo()
			   {
			      validReceiveAmount=1283.04m,
			      validReceiveCurrency="MXN",
				  validExchangeRate=16.1522m,
				  estimatedReceiveAmount=0m,
				  estimatedExchangeRate=0.0000m,
				  totalAmount=89.0m,
				  receiveCountry=feeLookupRequest.receiveCountry,
				  deliveryOption="BANK_DEPOSIT",
				  receiveAmountAltered=false,
				  revisedInformationalFee=false,
				  deliveryOptId="10",
				  deliveryOptDisplayName="Account Deposit",
				  mgiTransactionSessionID="9736251E1440393767094NN",
				  speedOfDeliveryText="Same/Next Day",
				  receiveAgentID="43797820",
				  receiveAgentName="BANCOMER TRANSFER SERVICES INC",
				  receiveAgentAbbreviation="BANCOMER",
				  mgManaged="required",
				  sendAmountAltered=false,
				  sendAmounts=new SendAmountInfo()
				  {
					  sendAmount=80.00m,
					  sendCurrency="USD",
					  totalSendFees=9.99m,
					  totalDiscountAmount=0.00m,
					  totalSendTaxes=0.00m,
					  totalAmountToCollect=89.99m,
					  detailSendAmounts = new AmountInfo[] 
					   {
						  new AmountInfo()
						  {
							  amountType="nonMgiSendTax", amount=0.00m, amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="nonMgiSendFee", amount=0.00m, amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiNonDiscountedSendFee", amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalNonDiscountedFees",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="discountedMgiSendFee",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiSendTax",amount=0.00m,amountCurrency="USD"
						  },
				  		  new AmountInfo()
						  {
							  amountType="totalMgiCollectedFeesAndTaxes",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalAmountToMgi",amount=89.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalNonDiscountedFees",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalNonMgiSendFeesAndTaxes",amount=0.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalSendFeesAndTaxes",amount=9.99m,amountCurrency="USD"
						  }
					  }
				 },
				 receiveAmounts = new EstimatedReceiveAmountInfo()
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
					detailEstimatedReceiveAmounts = new AmountInfo[] 
					{
					 new AmountInfo()
					  {
						  amountType="nonMgiReceiveTax", amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiReceiveFee", amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveFee",amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveTax", amount=0.00m,amountCurrency="MXN"
					  }
					}
				  }
				},
				new FeeInfo()
				{
			      validReceiveAmount=1283.04m,
			      validReceiveCurrency="MXN",
				  validExchangeRate=16.1522m,
				  estimatedReceiveAmount=0m,
				  estimatedExchangeRate=0.0000m,
				  totalAmount=89.0m,
				  receiveCountry=feeLookupRequest.receiveCountry,
				  deliveryOption="RECEIVE_AT",
				  receiveAmountAltered=false,
				  revisedInformationalFee=false,
				  deliveryOptId="11",
				  deliveryOptDisplayName="Bancomer Transfer Services, Inc.",
				  mgiTransactionSessionID="9736251E1440393767094NN",
				  speedOfDeliveryText="Same/Next Day",
				  receiveAgentID="43744527",
				  receiveAgentName="TELECOMM",
				  receiveAgentAbbreviation="TELECOMM",
				  mgManaged="not allowed",
				  sendAmountAltered=false,
				  sendAmounts=new SendAmountInfo()
				  {
				  sendAmount=80.00m,
				  sendCurrency="USD",
				  totalSendFees=9.99m,
				  totalDiscountAmount=0.00m,
				  totalSendTaxes=0.00m,
				  totalAmountToCollect=89.99m,
				  detailSendAmounts = new AmountInfo[] 
				   {
					  new AmountInfo()
					  {
						  amountType="nonMgiSendTax", amount=0.00m, amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiSendFee", amount=0.00m, amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiNonDiscountedSendFee", amount=9.99m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",amount=9.99m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="discountedMgiSendFee",amount=9.99m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiSendTax",amount=0.00m,amountCurrency="USD"
					  },
				  		  new AmountInfo()
					  {
						  amountType="totalMgiCollectedFeesAndTaxes",amount=9.99m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalAmountToMgi",amount=89.99m,amountCurrency="USD"
					  },

					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",amount=9.99m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonMgiSendFeesAndTaxes",amount=0.00m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalSendFeesAndTaxes",amount=9.99m,amountCurrency="USD"
					  }
				  }
				 },
				 receiveAmounts = new EstimatedReceiveAmountInfo()
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
					detailEstimatedReceiveAmounts = new AmountInfo[] 
					{
					 new AmountInfo()
					  {
						  amountType="nonMgiReceiveTax", amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiReceiveFee", amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveFee",amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveTax", amount=0.00m,amountCurrency="MXN"
					  }
					}
				  }
				}
		     };

			return feeResponse;
		}

		private FeeLookupResponse MockfeeLookup_For_Canada(FeeLookupRequest feeLookupRequest)
		{
			FeeLookupResponse feeResponse = new FeeLookupResponse();
			feeResponse.doCheckIn = false;
			feeResponse.flags = 17;
			feeResponse.timeStamp = DateTime.Now;
			feeResponse.feeInfo = new FeeInfo[]
			{
			   new FeeInfo()
			   {
			      validReceiveAmount=103.74m,
			      validReceiveCurrency="CAD",
				  validExchangeRate=1.2967m,
				  estimatedReceiveAmount=0m,
				  estimatedExchangeRate=0.0000m,
				  totalAmount=92.50m,
				  receiveCountry=feeLookupRequest.receiveCountry,
				  deliveryOption="WILL_CALL",
				  receiveAmountAltered=false,
				  revisedInformationalFee=false,
				  deliveryOptId="0",
				  deliveryOptDisplayName="10 Minute Service",
				  mgiTransactionSessionID="9736251E1440659495977NN",
				  sendAmountAltered=false,
				  sendAmounts=new SendAmountInfo()
				  {
				  sendAmount=80.00m,
				  sendCurrency="USD",
				  totalSendFees=12.50m,
				  totalDiscountAmount=0.00m,
				  totalSendTaxes=0.00m,
				  totalAmountToCollect=92.50m,
				  detailSendAmounts = new AmountInfo[] 
				   {
					  new AmountInfo()
					  {
						  amountType="nonMgiSendTax", amount=0.00m, amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiSendFee", amount=0.00m, amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiNonDiscountedSendFee", amount=12.50m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",amount=12.50m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="discountedMgiSendFee",amount=92.50m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiSendTax",amount=0.00m,amountCurrency="USD"
					  },
				  		  new AmountInfo()
					  {
						  amountType="totalMgiCollectedFeesAndTaxes",amount=12.50m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalAmountToMgi",amount=92.50m,amountCurrency="USD"
					  },

					  new AmountInfo()
					  {
						  amountType="totalNonDiscountedFees",amount=92.50m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalNonMgiSendFeesAndTaxes",amount=0.00m,amountCurrency="USD"
					  },
					  new AmountInfo()
					  {
						  amountType="totalSendFeesAndTaxes",amount=92.50m,amountCurrency="USD"
					  }
				  }
				 },
				 receiveAmounts = new EstimatedReceiveAmountInfo()
			     {
					receiveAmount = 103.74m,
					receiveCurrency = "CAD",
					validCurrencyIndicator = true,
					payoutCurrency = "CAD",
					totalReceiveFees = 8.88m,
					totalReceiveTaxes = 5.00m,
					totalReceiveAmount = 89.86m,
					receiveFeesAreEstimated = false,
					receiveTaxesAreEstimated = true,
					detailEstimatedReceiveAmounts = new AmountInfo[] 
					{
					 new AmountInfo()
					  {
						  amountType="nonMgiReceiveTax", amount=5.00m,amountCurrency="CAD"
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiReceiveFee", amount=8.88m,amountCurrency="CAD"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveFee",amount=0.00m,amountCurrency="CAD"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveTax", amount=0.00m,amountCurrency="CAD"
					  }
					}
				  }
				},
				 new FeeInfo()
			   {
			      validReceiveAmount=1283.04m,
			      validReceiveCurrency="MXN",
				  validExchangeRate=16.1522m,
				  estimatedReceiveAmount=0m,
				  estimatedExchangeRate=0.0000m,
				  totalAmount=89.0m,
				  receiveCountry=feeLookupRequest.receiveCountry,
				  deliveryOption="BANK_DEPOSIT",
				  receiveAmountAltered=false,
				  revisedInformationalFee=false,
				  deliveryOptId="10",
				  deliveryOptDisplayName="Account Deposit",
				  mgiTransactionSessionID="9736251E1440393767094NN",
				  speedOfDeliveryText="Same/Next Day",
				  receiveAgentID="43797820",
				  receiveAgentName="BANCOMER TRANSFER SERVICES INC",
				  receiveAgentAbbreviation="BANCOMER",
				  mgManaged="required",
				  sendAmountAltered=false,
				  sendAmounts=new SendAmountInfo()
				  {
					  sendAmount=80.00m,
					  sendCurrency="USD",
					  totalSendFees=9.99m,
					  totalDiscountAmount=0.00m,
					  totalSendTaxes=0.00m,
					  totalAmountToCollect=89.99m,
					  detailSendAmounts = new AmountInfo[] 
					   {
						  new AmountInfo()
						  {
							  amountType="nonMgiSendTax", amount=0.00m, amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="nonMgiSendFee", amount=0.00m, amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiNonDiscountedSendFee", amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalNonDiscountedFees",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="discountedMgiSendFee",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiSendTax",amount=0.00m,amountCurrency="USD"
						  },
				  		  new AmountInfo()
						  {
							  amountType="totalMgiCollectedFeesAndTaxes",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalAmountToMgi",amount=89.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalNonDiscountedFees",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalNonMgiSendFeesAndTaxes",amount=0.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalSendFeesAndTaxes",amount=9.99m,amountCurrency="USD"
						  }
					  }
				 },
				 receiveAmounts = new EstimatedReceiveAmountInfo()
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
					detailEstimatedReceiveAmounts = new AmountInfo[] 
					{
					 new AmountInfo()
					  {
						  amountType="nonMgiReceiveTax", amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="nonMgiReceiveFee", amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveFee",amount=0.00m,amountCurrency="MXN"
					  },
					  new AmountInfo()
					  {
						  amountType="mgiReceiveTax", amount=0.00m,amountCurrency="MXN"
					  }
					}
				  }
				}
		     };

			return feeResponse;
		}

		private FeeLookupResponse MockfeeLookup_For_AFG(FeeLookupRequest feeLookupRequest)
		{
			FeeLookupResponse feeResponse = new FeeLookupResponse();
			feeResponse.doCheckIn = false;
			feeResponse.flags = 17;
			feeResponse.timeStamp = DateTime.Now;
			feeResponse.feeInfo = new FeeInfo[]
			{
				new FeeInfo()
			    {
			      validReceiveAmount=80.00m,
			      validReceiveCurrency="USD",
				  validExchangeRate=1.00002m,
				  estimatedReceiveAmount=0m,
				  estimatedExchangeRate=0.0000m,
				  totalAmount=91.0m,
				  receiveCountry=feeLookupRequest.receiveCountry,
				  deliveryOption="WILL_CALL",
				  receiveAmountAltered=false,
				  revisedInformationalFee=false,
				  deliveryOptId="10",
				  deliveryOptDisplayName="10 Minute Service",
				  mgiTransactionSessionID="9736251E1440659959699NN",				  
				  sendAmountAltered=false,
				  sendAmounts=new SendAmountInfo()
				  {
					  sendAmount=80.00m,
					  sendCurrency="USD",
					  totalSendFees=11.00m,
					  totalDiscountAmount=0.00m,
					  totalSendTaxes=0.00m,
					  totalAmountToCollect=91.99m,
					  detailSendAmounts = new AmountInfo[] 
					   {
						  new AmountInfo()
						  {
							  amountType="nonMgiSendTax", amount=0.00m, amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="nonMgiSendFee", amount=0.00m, amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiNonDiscountedSendFee", amount=11.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalNonDiscountedFees",amount=11.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="discountedMgiSendFee",amount=11.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiSendTax",amount=0.00m,amountCurrency="USD"
						  },
				  		  new AmountInfo()
						  {
							  amountType="totalMgiCollectedFeesAndTaxes",amount=11.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalAmountToMgi",amount=91.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalNonDiscountedFees",amount=11.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalNonMgiSendFeesAndTaxes",amount=0.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalSendFeesAndTaxes",amount=11.00m,amountCurrency="USD"
						  }
					  }
				 },
				 receiveAmounts = new EstimatedReceiveAmountInfo()
			     {
					receiveAmount = 1292.18m,
					receiveCurrency = "USD",
					validCurrencyIndicator = true,
					payoutCurrency = "USD",
					totalReceiveFees = 0.00m,
					totalReceiveTaxes = 0.00m,
					totalReceiveAmount = 80.00m,
					receiveFeesAreEstimated = false,
					receiveTaxesAreEstimated = false,
					detailEstimatedReceiveAmounts = new AmountInfo[] 
					{
						  new AmountInfo()
						  {
							  amountType="nonMgiReceiveTax", amount=0.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="nonMgiReceiveFee", amount=0.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiReceiveFee",amount=0.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiReceiveTax", amount=0.00m,amountCurrency="USD"
						  }
					}
				  }
				},
			   new FeeInfo()
			   {
			      validReceiveAmount=5086.22m,
			      validReceiveCurrency="AFN",
				  validExchangeRate=63.5778m,
				  estimatedReceiveAmount=0m,
				  estimatedExchangeRate=0.0000m,
				  totalAmount=89.99m,
				  receiveCountry=feeLookupRequest.receiveCountry,
				  deliveryOption="WILL_CALL",
				  receiveAmountAltered=false,
				  revisedInformationalFee=false,
				  deliveryOptId="0",
				  deliveryOptDisplayName="10 Minute Service",
				  mgiTransactionSessionID="9736251E1440659959699NN",
				  sendAmountAltered=false,
				  sendAmounts=new SendAmountInfo()
					  {
					  sendAmount=80.00m,
					  sendCurrency="USD",
					  totalSendFees=9.99m,
					  totalDiscountAmount=0.00m,
					  totalSendTaxes=0.00m,
					  totalAmountToCollect=89.99m,
					  detailSendAmounts = new AmountInfo[] 
					   {
						  new AmountInfo()
						  {
							  amountType="nonMgiSendTax", amount=0.00m, amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="nonMgiSendFee", amount=0.00m, amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiNonDiscountedSendFee", amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalNonDiscountedFees",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="discountedMgiSendFee",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiSendTax",amount=0.00m,amountCurrency="USD"
						  },
				  			  new AmountInfo()
						  {
							  amountType="totalMgiCollectedFeesAndTaxes",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalAmountToMgi",amount=89.99m,amountCurrency="USD"
						  },

						  new AmountInfo()
						  {
							  amountType="totalNonDiscountedFees",amount=9.99m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalNonMgiSendFeesAndTaxes",amount=0.00m,amountCurrency="USD"
						  },
						  new AmountInfo()
						  {
							  amountType="totalSendFeesAndTaxes",amount=9.99m,amountCurrency="USD"
						  }
					  }
					 },
				 receiveAmounts = new EstimatedReceiveAmountInfo()
			     {
					receiveAmount = 5086.22m,
					receiveCurrency = "AFN",
					validCurrencyIndicator = true,
					payoutCurrency = "AFN",
					totalReceiveFees = 0.00m,
					totalReceiveTaxes = 0.00m,
					totalReceiveAmount = 5086.22m,
					receiveFeesAreEstimated = false,
					receiveTaxesAreEstimated = false,
					detailEstimatedReceiveAmounts = new AmountInfo[] 
					 {
						 new AmountInfo()
						  {
							  amountType="nonMgiReceiveTax", amount=0.00m,amountCurrency="AFN"
						  },
						  new AmountInfo()
						  {
							  amountType="nonMgiReceiveFee", amount=0.00m,amountCurrency="AFN"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiReceiveFee",amount=0.00m,amountCurrency="AFN"
						  },
						  new AmountInfo()
						  {
							  amountType="mgiReceiveTax", amount=0.00m,amountCurrency="AFN"
						  }
					 }
				  }
				}
		     };

			return feeResponse;
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

		private string GetDeliveryOptionName(string deliveryOption)
		{
			string deliveryOptionName = string.Empty;

			switch (deliveryOption)
			{
				case "WILL_CALL":
					deliveryOptionName = "10 Minute Service";
					break;
				case "BANK_DEPOSIT":
					deliveryOptionName = "Account Deposit";
					break;
				case "RECEIVE_AT":
					deliveryOptionName = "Bancomer Transfer Services, Inc.";
					break;
			}

			return deliveryOptionName;
		}

		#endregion

	}
}
