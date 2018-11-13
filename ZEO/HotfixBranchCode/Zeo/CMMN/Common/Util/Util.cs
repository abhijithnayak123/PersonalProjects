using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;


namespace MGI.Common.Util
{
	public static class NexxoUtil
	{

		private static NLoggerCommon NLogger = new NLoggerCommon();
		public static string AppVersion
		{
			get
			{
				Version v = Assembly.GetEntryAssembly().GetName().Version;
				return v.Major + "." + v.Minor + "." + v.Build;
			}
		}

		public static string AppFullVersion
		{
			get
			{
				return Assembly.GetEntryAssembly().GetName().Version.ToString();
			}
		}

		public static string MemUsage
		{
			get
			{
				Process myProcess = Process.GetCurrentProcess();
				string WorkingSet = "Working Set: " + (System.Convert.ToDouble(myProcess.WorkingSet64) / 1000000.0).ToString(".#") + " MB\n";
				WorkingSet += "Peak Working Set: " + (System.Convert.ToDouble(myProcess.PeakWorkingSet64) / 1000000.0).ToString(".#") + " MB";
				return WorkingSet;
			}
		}

		public static string AppLoginName
		{
			get
			{
				string LoginName = System.Environment.UserName;
				if (LoginName.IndexOf('\\') >= 0)
					LoginName = LoginName.Substring(LoginName.IndexOf('\\') + 1);
				return LoginName;
			}
		}
		/****************************Begin TA-50 Changes************************************************/
		//       User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
		//       Purpose: On Vera Code Scan, the below commented methods were found having Insufficient Entropy and none of below methods are being used
		//public static int AppSessionId
		//{
		//	get // MAX 8 digits
		//	{
		//		Random RandomGenerator = new Random(unchecked((int)DateTime.Now.Ticks));
		//		return RandomGenerator.Next(10000000, 99999999);
		//	}
		//}

		//public static int RandomNumber(int length)
		//{
		//	int allOnes = 0;
		//	for (int i = 0; i < length; i++)
		//	{
		//		allOnes += (int)Math.Pow(10, i);
		//	}
		//	int allNines = allOnes * 9;
		//	Random randomGenerator = new Random(unchecked((int)DateTime.Now.Ticks));
		//	return randomGenerator.Next(0, allNines);
		//}

		//public static long NewTransactionId
		//{
		//	get
		//	{
		//		Random rnd = new Random();
		//		string Digits = ((char)rnd.Next('1', '9')).ToString();
		//		for (int i = 0; i < 9; i++)
		//			Digits += (char)rnd.Next('0', '9');
		//		return long.Parse(Digits);
		//	}
		//}
		/****************************END TA-50 Changes************************************************/

		public static void ParseECTId(string ECTTxnId, out string ConfirmationId, out string WireId)
		{
			ConfirmationId = string.Empty;
			WireId = string.Empty;
			try
			{
				if (ECTTxnId.IndexOf('@') >= 0)
				{
					ConfirmationId = ECTTxnId.Substring(0, ECTTxnId.IndexOf('@'));
					WireId = ECTTxnId.Substring(ECTTxnId.IndexOf('@') + 1);
				}
				else
					ConfirmationId = ECTTxnId;
			}
			catch (Exception ex)
			{
				NLogger.Error("Unable to parse ConfirmationId and WireId: " + ex.Message, "NexxoUtil");
			}
		}

		public static string MakePIN(DateTime DateOfBirth)
		{
			return DateOfBirth.Date.Day.ToString("00") + DateOfBirth.Date.Month.ToString("00");
		}

		public static string FormatPhoneNumber(string PhoneNumber)
		{
			return FormatPhoneNumber(PhoneNumber, CountryCode.UNITED_STATES);
		}

		public static string IntlDialingPrefix(CountryCode Country)
		{
			switch (Country)
			{
				case CountryCode.MEXICO: return "521";
				case CountryCode.GUATEMALA: return "502";
				case CountryCode.EL_SALVADOR: return "503";
				case CountryCode.HONDURAS: return "504";
				case CountryCode.NICARAGUA: return "505";
				case CountryCode.PANAMA: return "507";
				case CountryCode.UNITED_STATES: return string.Empty;
				default: return string.Empty;
			}
		}

		public static string FormatPhoneNumber(string PhoneNumber, CountryCode Country)
		{
			if (!Char.IsDigit(PhoneNumber[0]))
				return PhoneNumber;

			string _IntlDialingPrefix = IntlDialingPrefix(Country);

			PhoneNumber = RawPhoneNumber(PhoneNumber);

			if (PhoneNumber.StartsWith(_IntlDialingPrefix))
				PhoneNumber = PhoneNumber.Substring(_IntlDialingPrefix.Length);

			switch (Country)
			{
				case CountryCode.MEXICO:
					try
					{
						while (PhoneNumber.Length < 10)
							PhoneNumber = "0" + PhoneNumber;
						return string.Format("52 1 {0} {1} {2}", PhoneNumber.Substring(0, 3), PhoneNumber.Substring(3, 3), PhoneNumber.Substring(6));
					}
					catch { }
					break;
				case CountryCode.GUATEMALA:
				case CountryCode.HONDURAS:
				case CountryCode.EL_SALVADOR:
				case CountryCode.NICARAGUA:
					try
					{
						// 50x - 0000 0000
						return string.Format("{0} - {1} {2}", _IntlDialingPrefix, PhoneNumber.Substring(0, 4), PhoneNumber.Substring(4));
					}
					catch { }
					break;
				case CountryCode.PANAMA:
					try
					{
						// 507 - 000 0000
						return string.Format("{0} - {1} {2}", _IntlDialingPrefix, PhoneNumber.Substring(0, 3), PhoneNumber.Substring(3));
					}
					catch { }
					break;
				case CountryCode.UNITED_STATES:
				default:
					try
					{
						return string.Format("({0}) {1}-{2}", PhoneNumber.Substring(0, 3), PhoneNumber.Substring(3, 3), PhoneNumber.Substring(6, 4));
					}
					catch { }
					break;
			}
			return PhoneNumber;
		}

		public static string FormatNumber(string Number, string Mask)
		{
			if (Mask.Length == 0)
				return Number;
			int iSrc = 0;
			string Result = string.Empty;
			foreach (char c in Mask)
				if (c == '9' || c == '0')
				{
					if (iSrc >= Number.Length)
						return Result;
					Result += Number[iSrc++];
				}
				else
					Result += c;
			return Result;
		}

		public static string FormatZipCode(string zipCode)
		{
			if (zipCode.Length > 5 && !zipCode.Contains("-"))
				return string.Format("{0}-{1}", zipCode.Substring(0, 5), zipCode.Substring(5));
			return zipCode;
		}

		public static string RawPhoneNumber(string PhoneNumber)
		{
			string Raw = string.Empty;
			foreach (char c in PhoneNumber)
				if (char.IsDigit(c))
					Raw += c;
			return Raw;
		}

		public static string RawMICR(string MICR)
		{
			// Same process as RawPhoneNumber
			return RawPhoneNumber(MICR);
		}

		public enum CountryCode : int
		{
			COLOMBIA = 170,
			MEXICO = 484,
			EL_SALVADOR = 222,
			GUATEMALA = 320,
			ARGENTINA = 32,
			PANAMA = 591,
			ECUADOR = 218,
			PERU = 604,
			HONDURAS = 340,
			BRAZIL = 76,
			BOLIVIA = 68,
			CHILE = 152,
			COSTA_RICA = 188,
			DOMINICAN_REPUBLIC = 214,
			URUGUAY = 858,
			NICARAGUA = 558,
			UNITED_STATES = 840,
			PARAGUAY = 600,
			JAMAICA = 388
		}

		public static string CountryName(CountryCode Code)
		{
			switch (Code)
			{
				case CountryCode.UNITED_STATES: return "United States";
				case CountryCode.COLOMBIA: return "Colombia";
				case CountryCode.MEXICO: return "Mexico";
				case CountryCode.EL_SALVADOR: return "El Salvador";
				case CountryCode.GUATEMALA: return "Guatemala";
				case CountryCode.ARGENTINA: return "Argentina";
				case CountryCode.PANAMA: return "Panama";
				case CountryCode.ECUADOR: return "Ecuador";
				case CountryCode.PERU: return "Peru";
				case CountryCode.HONDURAS: return "Honduras";
				case CountryCode.BRAZIL: return "Brazil";
				case CountryCode.BOLIVIA: return "Bolivia";
				case CountryCode.CHILE: return "Chile";
				case CountryCode.COSTA_RICA: return "Costa Rica";
				case CountryCode.DOMINICAN_REPUBLIC: return "Dominican Republic";
				case CountryCode.URUGUAY: return "Uruguay";
				case CountryCode.NICARAGUA: return "Nicaragua";
				case CountryCode.PARAGUAY: return "Paraguay";
				case CountryCode.JAMAICA: return "Jamaica";
				default: return string.Empty;
			}
		}

		public static string CountryName(int Code)
		{
			return CountryName((CountryCode)Code);
		}

		public static string CurrencyCode(int countryId)
		{
			switch ((CountryCode)countryId)
			{
				case CountryCode.ARGENTINA: return "ARS";
				case CountryCode.BOLIVIA: return "BOB";
				case CountryCode.BRAZIL: return "BRL";
				case CountryCode.COLOMBIA: return "COP";
				case CountryCode.DOMINICAN_REPUBLIC: return "DOP";
				case CountryCode.GUATEMALA: return "GTQ";
				case CountryCode.HONDURAS: return "HNL";
				case CountryCode.MEXICO: return "MXN";
				case CountryCode.PARAGUAY: return "PYG";
				default: return "USD";
			}
		}

		public enum EventType : short // This enum MUST match tEventTypes
		{
			UserCardSwipe = 1,
			AgentCardSwipe = 2,
			SuperAgentCardSwipe = 3,
			SessionError_PINFailure = 4,
			PINCancel = 5,
			PINSuccess = 6,
			AddFundsSuccess = 7,
			AddFundsCancel = 8,
			TransRequest = 9,
			TransPrint = 10,				// no longer in use
			StatementRequest = 11,
			StatementPrint = 12,
			CardActivation = 13,
			BonusAward = 14,				// Bonus program name in Comment field
			PanelTimeout = 15,			// Panel name in Comment field
			SelectEnglish = 16,
			SelectSpanish = 17,
			BillCollectorEmptied = 18,
			AgentDemo = 19,
			AgentReport = 20,
			ServiceLoginSuccess_NOT_IN_USE = 21,
			ServiceLoginFailure_NOT_IN_USE = 22,
			AddFundsFailure = 23,
			ReferralCouponRequest = 24,	// Panel name in Comment field
			SessionDisconnect = 25,
			NetworkTest = 26,
			MasterCardActivation = 27,
			MasterCardActivationFailure = 28,
			CustomerServiceRequest = 29,
			KioskHeartbeat_NOT_IN_USE = 30,
			CustomerServiceInstructions = 31,
			PanelCancel = 32,
			SessionStart = 33,
			PrintTelMexReceipt = 34,
			SessionError_CardNotFound = 35,
			SessionError_CardNotActive = 36,
			SessionError_CardExpired = 37,
			SessionDisconnect_RTFEError = 38,
			SessionDisconnect_ECTError = 39,
			SessionDisconnect_eFundsError = 40,
			SessionDisconnect_DataCenterError = 41,
			SessionDisconnect_CardSetupError = 42,
			SessionDisconnect_UnknownError = 43,
			SessionCancel = 44,
			TransferSuccess = 45,
			TransferFailure = 46,
			PINBypass = 47,
			PINBypassSuccess = 48,
			PINBypassFailure = 49,
			ShowPanel = 50,
			PanelEvent = 51,
			UDPCommand = 52,
			ServiceFixedProblem = 53,
			ServiceReplacedPaper = 54,
			ServiceAddedCards = 55,
			SessionDisconnect_OFACSystemError = 56,
			AgentKioskTestFailure = 57,
			AgentKioskTestSuccess = 58,
			AgentKioskTestTimeout = 59,
			ServiceCardSwipe = 60,
			OPALFailure = 61,
			CollectionCardSwipe = 62,
			BillCollectorSwapped = 63,
			CardBypassSuccess = 64,
			CardBypassFailure = 65,
			CollectionComplete = 66,
			BatteryPower = 67,
			WallPower = 68,
			MoneyStationStart = 69,
			MoneyStationExit = 70,
			SessionError_PANNotActive = 71,
			DOBEntryIncorrect = 72,
			DOBEntryFailure = 73,
			DOBEntrySuccess = 74,
			CollectionCompleteManual = 75,
			MoneyOrderPrinterCheckReplenishment = 76,
			MoneyOrderPrinterRibbonReplacement = 77,
			CheckFrankingStampReplacement = 78
		};

		#region ISO8583WebSvc
		public enum ISO8583WebSvcErrorCodes : int
		{
			Unknown = int.MinValue,
			NoError = 0,

			IncorrectPIN = 30, // Allow customer to retry PIN

			HSMFailure = 100,

			ProcessorFailure = 200,

			ISO8583WebSvcFailure = 300
		}
		#endregion

		#region CIAS

		public enum EFSClientId : int // tEFSClients
		{
			Unknown = 0,
			Nexxo = 1,
			HRBlock = 2,
			Centris = 3,
			HRBKony = 4,
			Woodforest = 5,
			Carver = 6,
			Axcess = 7,
			CSB = 8
		}

		public enum CIASErrorCodes : int
		{
			Unknown = int.MinValue,
			NoError = 0,

			RequestDOB = 10,
			RequestPIN = 20,

			IncorrectPIN = 30, // Allow customer to retry PIN
			Track2FormatError = 31,

			CustomerNotFound = 100,
			CustomerDisabled = 101,
			CustomerPINFailure = 102,
			CustomerAccountError = 103,
			CustomerSessionStartError = 104,
			CustomerAlreadyRegistered = 105,
			CustomerPhoneAndPINNotUnique = 106,
			CustomerPhoneAndDOBNotUnique = 107,

			InvalidGender = 150,
			InvalidPhoneNumber = 151,

			ProcessorFailure = 200,

			CIASEngineFailure = 300
		}

		public enum CIASCustomerStatus : int
		{
			Unknown = 0,
			NewPendingActivation = 1,
			NewActive = 2,
			NewProfileConfirmationRequired = 3,
			IncompleteProfileGender = 4,
			IncompleteProfileSSN = 5,
			IncompleteProfilePIN = 6,
			IncompleteProfilePhone = 7,
			IncompleteProfileDOB = 8,
			ExistingActive = 10,
			UpdatedActive = 11,
			Activated = 12,
			Disabled = 20,
			ReadyForActivation = 30
		}

		public enum CIASCardType : int
		{
			Unknown = 0,
			Customer = 1,
			SalesAgent = 2,
			Manager = 3,
			NonMonetaryService = 4,
			MonetaryService = 5,
			Collection = 6
		}

		#endregion

		#region KCC

		public enum KCCErrorCodes : int
		{
			Unknown = int.MinValue,
			NoError = 0,

			GeneralError = 1
		}

		public enum WebServiceConfigs : int
		{
			UNKNOWN = 0,
			TEST = 1,
			LIVEWIRE = 2,
			STAGE = 3,
			PRODUCTION = 4,
			TRADESHOW = 5,
			TRADESHOW_DC3 = 6
		}

		public enum ServiceTypes : int
		{
			Unknown = 0,
			CIASWebSvc = 1,
			ProductCatalogWebSvc = 2,
			CPWebSvc = 3,
			XEngineWebSvc = 4,
			GiftEngineWebSvc = 5,
			KioskConfigControlWebSvc = 6,
			ImageCacheSvc = 7,
			LogFileUploadServer = 8,
			TerminalWorkingKey
		}

		public enum SkinElementTypes : int
		{
			Unknown = 0,
			Text = 1,
			ImageCacheImageName = 2,
			Color = 3
		}

		#endregion

		#region Product Catalog

		public enum ProductProcessorIds : int // Must match tProductProcessors
		{
			UNKNOWN = 0,
			Nexxo = 1,
			TIO = 2,
			AmericaVoice = 3,
			Progreso = 4,
			TelMex = 5,
			Movilix = 6,
			Lunex = 7,
			NexxoMoneyOrder = 8,
			ExoNexxoPurseProcessor = 9,
			OrderExpress = 10,
			WoodforestMO = 11,
			CheckFreePay = 13
		}

		public enum ProductTypeIds : int // Must match tProductTypes
		{
			UNKNOWN = 0,
			NexxoPromotion = 1, // i.e. DPD or other purchasable promo
			PhoneCard = 2,
			BillPay = 3,
			GiftCard = 4,
			PrePaidDebitCard = 5,
			DomesticTopUp = 6,
			IntlTopUp = 7,
			MoneyOrder = 8,
			ExoNexxoPurseDebit = 9,
			ExoNexxoPurseCredit = 10,
			CheckCashing = 11
		}

		public enum ProductBillTypeIds : int // tProductBillTypes
		{
			UNKNOWN = 0,
			Utility = 1,
			TVService = 2,
			CellPhone = 3,
			HomePhone = 4,
			RetailCard = 5,
			BankCreditCard = 6,
			GasCard = 7,
			AutoPayment = 8,
			GiftCard = 9,
			PrepaidPhoneCard = 10,
			FreePhoneCard = 11,
			TopUp_VirginMobile = 12,
			TopUp_Boost = 14,
			TopUp_Cricket = 15,
			TopUp_TMobile = 16,
			TopUp_TelCel = 17,
			TopUp_Movistar = 18,
			TopUp_Tigo = 19,
			TopUp_Claro = 20,
			TopUp_Digicel = 22,
			TopUp_ATnT = 23,
			MoneyOrder = 24,
			ExoNexxoPurseDebitCredit = 25
		}

		public enum ProductIds : int // tProducts
		{
			Unknown = 0,
			WireTransfer = 1,
			ATT_TIO = 2,
			TwoDollarPhoneCard = 3,
			DPD = 4,
			PGnE_TIO = 5,
			Verizon_TIO = 6,
			FiveDollarPhoneCard = 7,
			TransferHistory = 11,
			Sprint_TIO = 12,
			DirecTV_TIO = 13,
			ComcastCable_TIO = 14,
			DishNetwork_TIO = 15,
			TMobile_TIO = 16,
			SoCalEdison_TIO = 18,
			SoCalGasCo_TIO = 22,
			LADWP_TIO = 23,
			TimeWarnerSoCal_TIO = 24,
			Capital_One_Auto_Finance_TIO = 35,
			Chrysler_Financial_TIO = 36,
			Ford_Credit_TIO = 37,
			GMAC_Automotive_Financing_TIO = 38,
			Honda_Financial_Services_TIO = 39,
			Infiniti_Financing_Services_TIO = 40,
			Mitsubishi_Motors_Credit_TIO = 41,
			Nissan_Motor_Acceptance_TIO = 42,
			Toyota_Financial_Services_TIO = 43,
			Volkswagen_Credit_TIO = 44,
			BP_TIO = 45,
			Chevron_TIO = 46,
			CITGO_TIO = 47,
			Conoco_TIO = 48,
			ExxonMobil_TIO = 49,
			Shell_TIO = 50,
			Sunoco_TIO = 51,
			Texaco_TIO = 52,
			American_Express_TIO = 53,
			Aspire_TIO = 54,
			ATT_Universal_TIO = 55,
			Bank_of_America_TIO = 56,
			Barclaycard_TIO = 57,
			Capital_One_TIO = 58,
			Chase_TIO = 59,
			Citibank_TIO = 60,
			Direct_Merchants_Bank_TIO = 61,
			Discover_TIO = 62,
			First_PREMIER_Bank_TIO = 63,
			First_USA_TIO = 64,
			GE_Money_TIO = 65,
			HSBC_TIO = 66,
			Merrick_Bank_TIO = 67,
			Tribute_TIO = 68,
			Wachovia_TIO = 69,
			Washington_Mutual_TIO = 70,
			Wells_Fargo_TIO = 71,
			Frontier_Communications_TIO = 72,
			MCI_TIO = 75,
			Trinsic_TIO = 76,
			Verizon_Home_TIO = 77,
			ADT_Security_Services_TIO = 78,
			Best_Buy_Credit_TIO = 79,
			Bloomingdales_Credit_TIO = 80,
			Dell_Financial_Services_TIO = 81,
			Fashion_Bug_Credit_TIO = 82,
			GAP_Credit_TIO = 83,
			Home_Depot_Credit_TIO = 84,
			JCPenney_Credit_TIO = 85,
			Kohls_Credit_TIO = 86,
			Lane_Bryant_Credit_TIO = 87,
			Lowes_Credit_TIO = 88,
			Macys_TIO = 89,
			New_York_Company_Credit_TIO = 91,
			Old_Navy_Credit_TIO = 92,
			RadioShack_Credit_TIO = 93,
			Sams_Club_Credit_TIO = 94,
			Sears_TIO = 95,
			Spiegel_Credit_TIO = 97,
			Target_TIO = 98,
			Victorias_Secret_Credit_TIO = 100,
			Walmart_Credit_TIO = 101,
			Charter_Communications_TIO = 102,
			Cox_Communications_TIO = 103,
			Mediacom_TIO = 104,
			CenturyLink_TIO = 105,
			City_of_Long_Beach_Utilities_TIO = 106,
			City_of_Oceanside_Utilities_TIO = 107,
			City_of_Ontario_Utilities_TIO = 108,
			City_of_Palo_Alto_Utlilities_TIO = 109,
			City_of_Sacramento_Utilities_TIO = 110,
			City_of_San_Diego_Utilities_TIO = 111,
			Direct_Energy_TIO = 112,
			Ferrell_Propane_TIO = 113,
			Modesto_Irrigation_District_TIO = 114,
			NV_Energy_TIO = 115,
			Rocky_Mountain_Power_TIO = 116,
			Sacramento_Municipal_Utility_TIO = 117,
			San_Diego_Gas_Electric_TIO = 118,
			SF_Water_Department_TIO = 119,
			Southwest_Gas_Corporation_TIO = 120,
			TID_Water_Power_TIO = 121,
			Amerigas_TIO = 122,
			Boost_Mobile_TIO = 123,
			Cricket_TIO = 124,
			Test_Product_Nexxo = 125,
			Frontier_Communications_TIO_126 = 126,
			Charter_Communications_TIO_127 = 127,
			Applied_Bank_TIO = 128,
			Amoco_TIO = 129,
			MetroPolitan_TeleCom_TIO = 130,
			Telecom_USA_TIO = 131,
			VerizonWireless_TIO = 132,
			ComcastCable_TIO_133 = 133,
			ATT_TIO_134 = 134,
			ATT_TIO_136 = 136,
			AmericaVoiceRecharge = 137,
			AmericaVoiceRecharge_138 = 138,
			ProgresoFinanciero = 144,
			AmericaVoiceFree = 146,
			VirginMobileTopUp = 147,
			BoostMobileTopUp = 148,
			CricketTopUp = 149,
			TMobileTopUp = 150,
			TelcelTopUp = 152,
			MovistarMxTopUp = 154,
			MovistarGuatemalaTopUp = 155,
			MovistarElSalvadorTopUp = 159,
			MovistarNicaraguaTopUp = 161,
			MovistarPanamaTopUp = 163,
			ClaroGuatemalaTopUp = 164,
			ClaroElSalvadorTopUp = 165,
			ClaroHondurasTopUp = 166,
			ClaroNicaraguaTopUp = 167,
			TigoGuatemalaTopUp = 168,
			TigoElSalvadorTopUp = 169,
			DigicelElSalvadorTopUp = 170,
			AmericaVoiceTopUp = 171,
			TigoHondurasTopUp = 172,
			ATTTopUp = 173,
			DigicelHondurasTopUp = 174,
			LunexPINLessMAX = 175,
			NexxoCoupon = 176,
			NexxoMoneyOrder = 177,
			ExoNexxoPurseBalanceRefund = 178,
			ExoNexxoPurseCashDisbursement = 180,
			ExoNexxoPurseDepositFromCash = 181,
			ExoNexxoPurseDepositFromCheck = 182,
			KansasCityPowerAndLight_TIO = 183,
			AbercrombieAndFitch_TIO = 184,
			AlltelWireless_TIO = 185,
			BrightHouseTampa_TIO = 186,
			OrderExpressMoneyOrder = 187,
			CommunityAmericanCU = 188,
			NationalWhlesaleLiqCard = 189,
			ImperialIrrigationDistrict = 190,
			INGLifeInsuranceAnnuity = 191,
			PalisadesSafetyInsurance = 192,
			WachoviaSmallBusinessCard = 193,
			IrvineRanchWaterDistrict = 194,
			SierraPacificPowerNVEnergy = 195,
			AirlinePilotsAssociation = 196,
			EastValleyWaterDistrict = 197,
			HorryElectricCooperative = 198,
			AmericanEagleOutfittersCard = 199,
			PublishersClearingHouseCard = 200,
			ActionCardAndBankfirst = 201,
			BarclaysBankofDelaware = 202,
			CarrollElectricMissouri = 203,
			FirstNationalBankOmaha = 204,
			FreechoiceCommunications = 205,
			HillsboroughCountyWater = 206,
			MiddleTennesseeElectric = 207,
			PrimusTelecommunications = 208,
			WellsFargoHomeMortgage = 209,
			CitizenBankCreditCard = 210,
			EmpireDistrictElectric = 211,
			FairpointCommunications = 212,
			GlendaleWaterAndPower = 213,
			NationalWaterAndPower = 214,
			RBSCreditCardServices = 215,
			StudentLoanCorporation = 216,
			WachoviaDealerServices = 217,
			WellsFargoAutoFinance = 218,
			GenworthLongTermCareInsura = 219,
			AmericreditCorporation = 220,
			HumanaDentalInsurance = 221,
			MercedesBenzFinancial = 222,
			MissouriAmericanWater = 223,
			ProgressEnergyFlorida = 224,
			VillageofPalmSprings = 225,
			YorkCountyNaturalGas = 226,
			ExpressClubExpressCard = 227,
			ModellsSportingGoodsCard = 228,
			DirecpathMediaworks = 229,
			HargrayCommunications = 230,
			TDSTelecomMetrocom = 231,
			UnionPlusCreditCard = 232,
			BrightHouseNetworksCalifo = 233,
			MidCarolinaElectricCompany = 234,
			AlliedWasteServices = 235,
			BoardofPublicWorks = 236,
			CrossCountryVisaMC = 237,
			FloridaKeysElectric = 238,
			FloridaPowerLight = 239,
			GardenGroveSanitary = 240,
			HPHomeHomeOffice = 241,
			IndioWaterAuthority = 242,
			PleasantViewUtility = 243,
			ProgressiveInsurance = 244,
			ToyotaLeasePayments = 245,
			WellsFargoFinancial = 246,
			BroadRiverElectricCoop = 247,
			SouthCarolinaElectricity = 248,
			BestBuyRewardZoneMasterCard = 249,
			RestorationHardwareCard = 250,
			ValueCityFurnitureCard = 251,
			AetnaLifeInsurance = 252,
			CitifinancialRetail = 253,
			EdfinancialServices = 254,
			MidwestLoanService = 255,
			SprintLongDistance = 256,
			StateFarmInsurance = 257,
			TowerHillInsurance = 258,
			USBankCreditCards = 259,
			WachoviaInstallment = 260,
			RegionalAcceptanceCorporatio = 261,
			MetlifeAutoHomeInsurance = 262,
			DesignWithinReachCard = 263,
			PoloRalphLaurenCard = 264,
			ServiceMerchandiseCard = 265,
			TheChildrensPlaceCard = 266,
			ThroughtheCountryCard = 267,
			AllmericaFinancial = 268,
			BayFinanceCompany = 269,
			ChaseHeritageVisa = 270,
			LacledeGasCompany = 271,
			MazumaCreditUnion = 272,
			MissouriGasEnergy = 273,
			MyWorldMastercard = 274,
			PaypalBuyerCredit = 275,
			QwestLongDistance = 276,
			SafeAutoInsurance = 277,
			UnumLifeInsurance = 278,
			WachoviaCommercial = 279,
			ChickasawElectricCooperative = 280,
			ManufacturersLifeInsurance = 281,
			GoldenStateWaterCompany = 282,
			HomeShoppingClubCard = 283,
			LittleSwitzerlandCard = 284,
			ACNCommunications = 285,
			AspenCreditCards = 286,
			AtlanticBroadband = 287,
			ChaseAutoFinance = 288,
			ChaseHomeFinance = 289,
			CitifinancialAuto = 290,
			CommerceInsurance = 291,
			GoodyearTirePlan = 292,
			SantanderConsumer = 293,
			TurlockIrrigation = 294,
			TimeWarnerCableKansasCity = 295,
			TimeWarnerCableSouthCarolina = 296,
			PalmettoElectricCooperative = 297,
			WestValleyWaterDistrict = 298,
			CAAmericanWaterCompany = 299,
			TennesseeAmericanWater = 300,
			CrescentJewelersCard = 301,
			GordonsJewelersCard = 302,
			HelzbergDiamondsCard = 303,
			MastercraftTiresCard = 304,
			SaksFifthAvenueCard = 305,
			TheCompanyStoreCard = 306,
			EverhomeMortgage = 307,
			FarmersInsurance = 308,
			LernerMailOrder = 309,
			LincolnFinancial = 310,
			meijerMasterCard = 311,
			MitsubishiRetail = 312,
			SpartanburgWater = 313,
			TrinityInsurance = 314,
			VistaHealthPlan = 315,
			WachoviaBankCard = 316,
			ZalesCreditPlan = 317,
			PrematicServiceCorporation = 318,
			SpecializedLoanServicing = 319,
			CaliforniaWaterServices = 320,
			HarlemFurnitureCard = 321,
			HaynesFurnitureCard = 322,
			LevitzFurnitureCard = 323,
			SportsAuthorityCard = 324,
			TheSwissColonyCard = 325,
			ATTBellsouth = 326,
			GeicoMasterCard = 327,
			HealthyFamilies = 328,
			JuniperBarclays = 329,
			KemperInsurance = 330,
			MetlifeBenefits = 331,
			MitsubishiLease = 332,
			SallieMaeLoans = 333,
			WalmartDiscover = 334,
			WasteIndustries = 335,
			SanteeElectricCooperative = 336,
			AmericanFamilyInsurance = 337,
			SunAmericaLifeInsurance = 338,
			WestCoastLifeInsurance = 339,
			ChattanoogaGasCompany = 340,
			DeerParkWaterCompany = 341,
			EasleyCombinedUtility = 342,
			BananaRepublicCard = 343,
			GanderMountainCard = 344,
			LevinFurnitureCard = 345,
			MidnightVelvetCard = 346,
			MonroeandMainCard = 347,
			PacificSunwearCard = 348,
			AshleyStewart = 349,
			CommerceEnergy = 350,
			FirstInvestors = 351,
			GeicoInsurance = 352,
			HorryTelephone = 353,
			LoftMasterCard = 354,
			StateFarmBank = 355,
			SunComWireless = 356,
			TownNorthBank = 357,
			HannFinancialServices = 358,
			TampaElectricCompany = 359,
			ValenciaWaterCompany = 360,
			CrateBarrelCard = 361,
			DomesticationsCard = 362,
			LinenThingsCard = 363,
			MasonEasyPayCard = 364,
			PolandSpringsCard = 365,
			SamAshDirectCard = 366,
			SeventhAvenueCard = 367,
			AllyFinancial = 368,
			EarthlinkInc = 369,
			EmergeVisaMC = 370,
			JessicaLondon = 371,
			ToyotaFinance = 372,
			GuitarCenterRetailServices = 373,
			YorkElectricCooperative = 374,
			GenworthLifeInsurance = 375,
			FontanaWaterCompany = 376,
			FurnitureRowCard = 377,
			LordTaylorCard = 378,
			NeimanMarcusCard = 379,
			TrekBicyclesCard = 380,
			ArchWireless = 381,
			BillMeLater = 382,
			GMACMortgage = 383,
			NashvilleGas = 384,
			NationalCity = 385,
			CircuitCityCard = 386,
			HenriBendelCard = 387,
			NewportNewsCard = 388,
			PalaisRoyalCard = 389,
			PostCoachCard = 390,
			PotteryBarnCard = 391,
			ShopatHomeCard = 392,
			StageStoresCard = 393,
			AquaAmerica = 394,
			AquaFinance = 395,
			ASGSecurity = 396,
			ATTUverse = 397,
			JuniperBank = 398,
			SageTelecom = 399,
			YamahaMotor = 400,
			AnthemLifeInsurance = 401,
			BannerLifeInsurance = 402,
			BrylaneHomeCard = 403,
			EddieBauerCard = 404,
			GrandPointeCard = 405,
			LimitedTooCard = 406,
			PCRichardsCard = 407,
			SilhouettesCard = 408,
			TheLimitedCard = 409,
			WorkNGearCard = 410,
			Freedomcard = 411,
			HSBCAMEX = 412,
			Monitronics = 413,
			TargetVisa = 414,
			USCellular = 415,
			AnnTaylorCard = 416,
			DressBarnCard = 417,
			MetrostyleCard = 418,
			OfficeMaxCard = 419,
			TheBuckleCard = 420,
			ZGallerieCard = 421,
			AIGDirect = 422,
			DukePower = 423,
			GreenTree = 424,
			GulfPower = 425,
			Suddenlink = 426,
			Windstream = 427,
			GroupUSATheClothingCompany = 428,
			AmeriMarkCard = 429,
			AnniesezCard = 430,
			ChadwicksCard = 431,
			FingerhutCard = 432,
			FirestoneCard = 433,
			FortunoffCard = 434,
			NordstromCard = 435,
			SteinmartCard = 436,
			ToysRUsCard = 437,
			UndergearCard = 438,
			WoodcraftCard = 439,
			AmerenUE = 440,
			CableOne = 441,
			AllstateAutoPropertyInsur = 442,
			JCPenneyMasterCard = 443,
			WawanesaInsurance = 444,
			CompUSACard = 445,
			DillardsCard = 446,
			GordmansCard = 447,
			HavertysCard = 448,
			MarianneCard = 449,
			mauricesCard = 450,
			ShopNBCCard = 451,
			AlertPay = 452,
			KJordan = 453,
			Kawasaki = 454,
			KingSize = 455,
			MIBank = 456,
			PNCBank = 457,
			SonyCard = 458,
			BoscovsCard = 459,
			HootersCard = 460,
			ImagineCard = 461,
			MenardsCard = 462,
			PeeblesCard = 463,
			RoamansCard = 464,
			SilkiesCard = 465,
			StaplesCard = 466,
			GMCard = 467,
			AvenueCard = 468,
			BeallsCard = 469,
			BontonCard = 470,
			MandeeCard = 471,
			Brinks = 472,
			HomeQCard = 473,
			QVCQCard = 474,
			BBTVisaMC = 475,
			BelkCard = 476,
			BoseCard = 477,
			FrysCard = 478,
			IKEACard = 479,
			Fina = 480,
			AmericasServicingCompany = 481,
			BJSMastercard = 482,
			HSNCard = 483,
			NashvilleElectricService = 484,
			SamuelsJewelers = 485,
			WestBuildingMaterials = 486,
			USAMobility = 487,
			ProgressEnergySouthCarolina = 488,
			QtelHome = 489,
			SuburbanNaturalGas = 490,
			BlackRiverElecCoopInc = 491,
			SuburbanDisposalCorp = 492,
			SuburbanHeatingOil = 493,
			SuburbanPropane = 494,
			FarmersTelephoneCoopInc = 495,
			SearsMasterCard = 496,
			SipiCollectpay = 497,
			AmericanElectricPower = 498,
			MetropolitanUtilitiesDistric = 499,
			NortheastNebraskaPPD = 500,
			OmahaPublicPowerDistrict = 501,
			OmahaPublicPower = 502,
			SourceGas = 503,
			TecoPeoplesGas = 505,
			Torrid = 506,
			AAASouthernCAMembership = 507,
			Knology508 = 508,
			MetroWaterofNashville = 509,
			CharlestonPublicWorks = 518,
			Knology519 = 519,
			VerizonFIOS = 521,
			BlackHillsEnergy = 522,
			MidAmericanEnergy = 523,
			WasteConnections = 524,
			GreatPlainCommunications = 525,
			NebraskaFurnitureMart = 526,
			WoodforestMoneyOrder = 527
		}

		#endregion

		public enum PremiumServiceId : int
		{
			UNKNOWN = 0,
			MONEYNOW = 1
		}

		public enum OfferTypeIds : int
		{
			UNKNOWN = 0,
			NEXXORECOMMENDED = 1
		}

		public enum BillReaderTypes : int
		{
			UNKNOWN = 0,
			OPAL = 1,
			CASHCODE = 2,
			MEI = 3,
			DEBUG_ONLY = 4
		}

		public enum BillReaderStatuses : int
		{
			// Must match tKioskBillReaderEventTypes
			NoError = 0,
			ReaderActivity = 1,
			BillInserted = 2,
			BillJam = 3,
			HopperFull = 4,
			HopperJam = 5,
			HopperMissing = 6,
			BillCheated = 7,
			HardwareFailure = 8,
			SerialPortFailure = 9,
			TempDisabled = 10,
			TempDisableTimeout = 11,
			Disabled = 12,
			Enabled = 13,
			ServiceEnabled = 14
		}

		public enum FundStatuses : int // Must match tFundStatuses
		{
			Unknown = 0,
			PendingFunding = 1,
			Funded = 2,
			Closed = 3,
			Expired
		}

		public enum FundPaymentStatuses : int // Must match tFundPaymentStatuses
		{
			Unknown = 0,
			PendingFunding = 1,
			ReadyToPay = 2,
			PaymentSent = 10,
			Canceled = 20,
			Voided = 30
		}

		#region Check Processing

		public enum CPErrorCodes : int
		{
			Unknown = int.MinValue,
			NoError = 0,

			ProcessorDelay = 10,

			GeneralError = 20,
			AccountError = 21,
			ProcessorError = 22,
			CashOutPending = 23,
			ValidationError = 24,
			IneligibleForCheckCashing = 25,
			CheckCashLimitReached = 26,
			DuplicateCheck = 27
		}

		public enum CPSenderStatuses : int
		{
			Unknown = int.MinValue,
			Enabled = 1,
			Disabled = 2,
			NotFound = 3,
			BlackListed = 4,
			LimitReached = 5
		}

		public enum CPCheckTypes : int
		{
			Unknown = int.MinValue,
			Cashiers = 1,
			GovtUSTreasury = 2,
			GovtUSOther = 3,
			GovtState = 4,
			MoneyOrder = 5,
			PayrollHandwritten = 6,
			PayrollPrinted = 7,
			TaxRefundUSTreasury = 8,
			TaxRefundOther = 9,
			TwoParty = 10,
			TwoPartyBusiness = 11,
			InsuranceAttorney,
			PromoPrintedPayroll,
			Loan_RAL,
			Loan,
			UnknownCheckType,
			HRB_RAC,
			Woodforest
		}

		public enum CPCheckProcessingStatus : int
		{
			Unknown = int.MinValue,

			InProcess = 1,
			SubmittedToProcessor = 2, // pending status, waiting for approval/decline from processor

			ApprovedByProcessor = 10, // approved by processor but not yet accepted (Stacked) by customer
			DeclinedByProcessor = 11, // canceled with processor by monitor after decline
			ExpiredByProcessor = 12,  // canceled with processor by monitor after approval not accepted by customer within 1 hr

			Stacked = 100, // approved by the processor, accepted by the customer, and stacked
			FeeAcceptedByCustomer = 101, // approved by the processor and fee accepted by the customer (mobile)

			CanceledByCustomer = 110, // intermediate status used to indicate customer has chosen (or ackknowledged) cancellation
			CanceledByMonitor = 111, // deprecated (as of CPEngine 1.7) - intermediate status

			CompleteProcessed = 200, // final status after ApprovedByProcessor and Stacked by customer
			CompleteCanceledRejectedByCustomer = 201,  // final status after ApprovedByProcessor but rejected by customer
			CompleteCanceledExpired = 202, // final status after ExpiredByProcessor
			CompleteCanceledDeclined = 203, // final status after DeclinedByProcessor
			CompleteCancelled = 210, // deprecated (as of CPEngine 1.7) - general final status for cancellation
			CompleteProcessorError = 220 // final status after processor error in check submission
		}

		public enum PurseProcessingStatus : int
		{
			Unknown = int.MinValue,

			InProcess = 1,
			SubmittedToProcessor = 2,

			ApprovedByProcessor = 10,

			CompleteApproved = 200,
			CompleteDeclined = 210,
			CompleteCancelled = 220,

			CompleteProcessorError = 300
		}

		public static void ParseMICR(string MICR, out string CheckNumber, out string RoutingNumber, out string AccountNumber, out string AllTheRest)
		{
			// ie '<033262<:121000248:4121494595<'
			CheckNumber = RoutingNumber = AccountNumber = AllTheRest = string.Empty;
			string[] _MICRParts = MICR.Split(new char[] { '<', ':', ';', '=' });
			foreach (string _MICRPart in _MICRParts)
				if (_MICRPart.Length > 0)
					if (CheckNumber.Length == 0)
						CheckNumber = _MICRPart;
					else if (RoutingNumber.Length == 0)
						RoutingNumber = _MICRPart;
					else if (AccountNumber.Length == 0)
						AccountNumber = _MICRPart;
					else
						AllTheRest += _MICRPart + ":";
		}

		#endregion

		public enum FlaggedCardTypes : int
		{
			UNKNOWN = 0,
			VISA = 1,
			MASTERCARD = 2,
			AMERICANEXPRESS = 3
		}

		public enum KioskCDUEventTypes : int
		{
			UNKNOWN = 0,
			STARTUP = 1,
			DISPENSE = 2,
			EMPTIED = 3,
			FILLED = 4,
			SERVICED = 5,
			DOOROPENED = 6,
			DOORCLOSED = 7,
			DISPENSERALERT = 8,
			DISPENSERFAIL = 9,
			MANUALSTOP = 10
		}

		public enum Gender : ushort
		{
			Unknown = 0,
			Female = 1,
			Male = 2
		}

		public enum HandNames : int
		{
			Unknown = 0,
			Left = 1,
			Right = 2
		}

		public enum FingerNames : int
		{
			Unknown = 0,
			Thumb = 1,
			Index = 2,
			Middle = 3,
			Ring = 4,
			Pinky = 5
		}

		public enum FPScannerModels : int
		{
			Unknown = 0,
			IntegratedBiometricsUSB = 1,
			UPEKEikonTouch = 2
		}

		public static Gender MapRelationshipToGender(string Relationship)
		{
			switch (Relationship)
			{
				case "AUNT":
				case "DAUGHTER":
				case "DAUGHTER-IN-LAW":
				case "GIRLFRIEND":
				case "GRANDMOTHER":
				case "MOTHER":
				case "MOTHER-IN-LAW":
				case "NIECE":
				case "SISTER":
				case "SISTER-IN-LAW":
				case "WIFE":
					return Gender.Female;
				case "BOYFRIEND":
				case "BROTHER":
				case "BROTHER-IN-LAW":
				case "FATHER":
				case "FATHER-IN-LAW":
				case "GRANDFATHER":
				case "HUSBAND":
				case "NEPHEW":
				case "SON":
				case "SON-IN-LAW":
				case "UNCLE":
					return Gender.Male;
				default:
					return Gender.Unknown;
			}
		}

		public static string MakeNDigits(string s, int d)
		{
			for (int i = (d - 1) - s.Length; i >= 0; i--)
				s = "0" + s;
			return s;
		}

		public enum Language : int
		{
			English = 0,
			Spanish = 1
		}

		public enum FIMChangeTypes : int
		{
			NoChange = 0,
			New = 1,
			Updated = 2,
			Deleted = 3,
			SizeChanged = 4,
			ContentChanged = 5
		}

		public static string GetDescription(Enum value, Language lang)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] attributes =
					(DescriptionAttribute[])fi.GetCustomAttributes(
					typeof(DescriptionAttribute), false);
			int langCode = (int)lang;
			if (attributes.Length > 0)
			{
				string[] descriptions = attributes[0].Description.Split('|');
				if (descriptions.Length == (langCode + 1))
					return descriptions[langCode];
				else
					return descriptions[0];
			}
			else
				return value.ToString();
		}

		public static string TrimString(string s, int len)
		{
			s = s.Trim();
			if (s.Length > len)
				s = s.Substring(0, len);
			return s;
		}

		public static string Reverse(string x)
		{
			char[] charArray = new char[x.Length];
			int len = x.Length - 1;
			for (int i = 0; i <= len; i++)
				charArray[i] = x[len - i];
			return new string(charArray);
		}

		public static string ReplaceAccents(string s)
		{
			char[] convert = new char[] { 'À', 'A', 'Á', 'A', 'É', 'E', 'Í', 'I', 'Ì', 'I', 'Ó', 'O', 'Ú', 'U', 'Ü', 'U', 'Ñ', 'N', 'á', 'a', 'é', 'e', 'í', 'i', 'ó', 'o', 'ú', 'u', 'ñ', 'n', '°', 'o', 'ª', 'A', '½', ' ', '-', ' ' };

			for (int i = 0; i < convert.Length; i = i + 2)
				s = s.Replace(convert[i], convert[i + 1]);

			return s;
		}

		public static string RemoveIllegalNameCharacters(string s)
		{
			return Regex.Replace(s, @"[^A-NO-Z ]", "");
		}

		public static string RemoveIllegalAddressCharacters(string s)
		{
			return Regex.Replace(s, @"[^0-9A-NO-Z/.,/\#;:_ -]", "");
		}

		/// <summary>
		/// Added this method for SQL Injection without Illegal Name and Words 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string SafeSQLString(string s)
		{
			return s.Replace("\\", "").Replace("'", "''");
		}

		/// <summary>
		/// Added this overloaded method for SQL Injection. This allows only whitelist words which are acceptable for Inputs.
		/// Made changes for Defect # DE2484. New Implementation allows '-'. This will NOT ALLOW ('&','<','>','/','\\','"',"'",'?','+')
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string SafeSQLString(string s, bool bln)
		{
			//This is the old implementation for SQL Injection.
			//if (s.All(c => Char.IsLetterOrDigit(c) || c == '_' || c == '@' || c == '!' || c == '#' || c == '$' || c == '%' || c == '^' || c == '&' || c == '*' || c == '(' || c == ')') && bln)
			if (s.All(c => Char.IsLetterOrDigit(c) || c != '&' || c != '<' || c != '>' || c != '/' || c != '\\' || c != '"' || c != '\'' || c != '?' || c != '+') && bln)
				return s.Replace("\\", "").Replace("'", "''");
			else
				return "";
		}

		public static string MakeSafeFileName(string s)
		{
			return s.Replace("&", string.Empty).Replace(".", string.Empty).Replace("$", string.Empty).Replace("-", string.Empty).Replace("'", string.Empty);
		}

		public static string FormatClave(string Clave)
		{
			// Print claves on Money Transfer Receipt in groups of 4-5 digits: 
			// - If Clave is 10 digits: ** **** **** 
			// - If Clave is 11 digits: *** **** **** 
			// - If Clave is 12 digits: **** **** **** 
			// - If Clave is 13 digits: ***** **** **** 
			// - If Clave is 14 digits: ** **** **** **** 
			// - If Clave is 15 digits: *** **** **** **** 
			// - If Clave is 16 digits: **** **** **** **** 
			try
			{
				switch (Clave.Length)
				{
					case 10:
						// ** **** ****
						// 01 2345 6789
						return string.Format("{0} {1} {2}", Clave.Substring(0, 2), Clave.Substring(2, 4), Clave.Substring(6, 4));

					case 11:
						// *** **** ****
						// 012 3456 7890
						return string.Format("{0} {1} {2}", Clave.Substring(0, 3), Clave.Substring(3, 4), Clave.Substring(7, 4));

					case 12:
						// **** **** ****
						// 0123 4567 8901
						return string.Format("{0} {1} {2}", Clave.Substring(0, 4), Clave.Substring(4, 4), Clave.Substring(8, 4));

					case 13:
						// ***** **** ****
						// 01234 5678 9012
						return string.Format("{0} {1} {2}", Clave.Substring(0, 5), Clave.Substring(5, 4), Clave.Substring(9, 4));

					case 14:
						// ** **** **** ****
						// 01 2345 6789 0123
						return string.Format("{0} {1} {2} {3}", Clave.Substring(0, 2), Clave.Substring(2, 4), Clave.Substring(6, 4), Clave.Substring(10, 4));

					case 15:
						// *** **** **** ****
						// 012 3456 7890 1234
						return string.Format("{0} {1} {2} {3}", Clave.Substring(0, 3), Clave.Substring(3, 4), Clave.Substring(7, 4), Clave.Substring(11, 4));

					case 16:
						// **** **** **** ****
						// 0123 4567 8901 2345
						return string.Format("{0} {1} {2} {3}", Clave.Substring(0, 4), Clave.Substring(4, 4), Clave.Substring(8, 4), Clave.Substring(12, 4));

					default:
						return Clave;

				}
			}
			catch { }
			return Clave;
		}

		public static ArrayList MakeLines(string LongText, int MaxLineLen, string Format)
		{
			int Start = 0, End = 0;
			ArrayList Lines = new ArrayList(LongText.Length / MaxLineLen + 10);
			while (End < LongText.Length)
			{
				End = Start + MaxLineLen;
				if (End >= LongText.Length)
				{
					Lines.Add(Format + LongText.Substring(Start));
					Lines.TrimToSize();
					return Lines;
				}
				while (LongText[End] != ' ')
					End--;
				Lines.Add(Format + LongText.Substring(Start, End - Start));
				Start = End + 1;
			}
			Lines.TrimToSize();
			return Lines;
		}

		public static List<string> MakeLinesList(string LongText, int MaxLineLen, string Format)
		{
			string[] LongTextParts = LongText.Split('\\');
			List<string> Lines = new List<string>();
			for (int i = 0; i < LongTextParts.Length; i++)
			{
				int Start = 0, End = 0;
				while (End < LongTextParts[i].Length)
				{
					End = Start + MaxLineLen;
					if (End >= LongTextParts[i].Length)
					{
						Lines.Add(Format + LongTextParts[i].Substring(Start));
						break;
						//return Lines;
					}
					while (LongTextParts[i][End] != ' ')
						End--;
					Lines.Add(Format + LongTextParts[i].Substring(Start, End - Start));
					Start = End + 1;
				}
			}
			return Lines;
		}

		public static string EncodeSuperPAN(long superPAN, int superPANType)
		{
			return EncodeSuperId(superPAN, "P", superPANType);
		}

		public static string EncodeSuperBene(long superBene, int superBeneType)
		{
			return EncodeSuperId(superBene, "B", superBeneType);
		}

		private static string EncodeSuperId(long superId, string superType, int superPANType)
		{
			// can't use SuperPANType here since Moneystation uses this class (but not SchemaObjects.cs
			string prefix = string.Format("S{0}{1}****", superType, superPANType == 1 ? "O" : superPANType == 2 ? "A" : "I");
			return prefix + superId.ToString().Substring(8);
		}

		public static string EncodePAN(long alloyId)
		{
			return NexxoCard.EncodeCardNumber(alloyId);
		}

		public static bool GetPAN(long last8, out long alloyId)
		{
			alloyId = 1000000000000000 + last8;
			if (ISOCard.IsValidCardNumber(alloyId))
				return true;

			alloyId = long.MinValue;
			return false;
		}

		public static bool IsSuperPAN(long alloyId)
		{
			return (alloyId > 2000000000000000 && alloyId < 3000000000000000);
		}

		public static bool GetSuperPAN(long last8, out long sPan)
		{
			sPan = 2000000000000000 + last8;
			if (ISOCard.IsValidCardNumber(sPan))
				return true;

			sPan = long.MinValue;
			return false;
		}

		public static bool IsSuperBene(long alloyId)
		{
			return (alloyId > 3000000000000000 && alloyId < 4000000000000000);
		}

		public static bool GetSuperBene(long last8, out long sBene)
		{
			sBene = 3000000000000000 + last8;
			if (ISOCard.IsValidCardNumber(sBene))
				return true;

			sBene = long.MinValue;
			return false;
		}

		public static bool IsNexxoKiosk(string kioskId)
		{
			return Regex.IsMatch(kioskId.ToLower(), @"[ckw]\d{4,7}");
		}

		public static bool IsKiosk(string kioskId)
		{
			return Regex.IsMatch(kioskId, @"[A-Za-z]{1,2}\d{4,7}");
		}

		public static string CreateYearMonthDayTree()
		{
			return DateTime.Today.ToString("yyyy") + "\\" + DateTime.Today.ToString("yyyyMM") + "\\" + DateTime.Today.ToString("yyyyMMdd") + "\\";
		}

		public static bool ParseFullName(string FullName, out string FirstName, out string MiddleName, out string LastName, out string LastName2, out string MoMaName)
		{
			FirstName = string.Empty;
			MiddleName = string.Empty;
			LastName = string.Empty;
			LastName2 = string.Empty;
			MoMaName = string.Empty;
			try
			{
				FullName = FullName.Replace(" DE ", " DE_").Replace(" DEL ", " DEL_").Replace(" LA ", " LA_").Replace("_LA ", "_LA_").Replace("_LOS ", "_LOS_").Replace("@", string.Empty).Replace("#", string.Empty).Replace("&", string.Empty).Replace("%", string.Empty).Replace("!", string.Empty).Replace(" ID ", string.Empty);

				string[] NameParts = FullName.ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				switch (NameParts.Length)
				{
					case 0:
					case 1:
						return false;
					case 2: // FirstName LastName
						FirstName = NameParts[0].Replace("_", " ");
						LastName = NameParts[1].Replace("_", " ");
						return true;
					case 3: // FirstName MiddleName LastName
						FirstName = NameParts[0].Replace("_", " ");
						MiddleName = NameParts[1].Replace("_", " ");
						LastName = NameParts[2].Replace("_", " ");
						if (MiddleName.Equals(LastName))
						{
							MoMaName = MiddleName;
							MiddleName = string.Empty;
						}
						return true;
					case 4: // FirstName MiddleName LastName LastName2
						FirstName = NameParts[0].Replace("_", " ");
						MiddleName = NameParts[1].Replace("_", " ");
						LastName = NameParts[2].Replace("_", " ");
						LastName2 = NameParts[3].Replace("_", " ");
						if (LastName.Equals(LastName2))
						{
							MoMaName = LastName2;
							LastName2 = string.Empty;
						}
						return true;
					default:
						FirstName = NameParts[0].Replace("_", " ");
						MiddleName = NameParts[1].Replace("_", " ");
						LastName = NameParts[2].Replace("_", " ");
						for (int i = 3; i < NameParts.Length; i++)
							LastName2 += NameParts[i] + " ";
						LastName2 = LastName2.Replace("_", " ").Trim();
						return true;
				}
			}
			catch
			{
				return false;
			}
		}

		public static bool ContainsMaMaria(string name)
		{
			return Regex.IsMatch(name, @"(^|\s)(ma|maria)(\s|$)", RegexOptions.IgnoreCase);
		}

		public static string ToggleMaMaria(string name)
		{
			Regex ma = new Regex(@"(^|\s)(ma)(\s|$)", RegexOptions.IgnoreCase);
			if (ma.IsMatch(name))
				return ma.Replace(name, "$1MARIA$3");

			Regex maria = new Regex(@"(^|\s)(maria)(\s|$)", RegexOptions.IgnoreCase);
			if (maria.IsMatch(name))
				return maria.Replace(name, "$1MA$3");

			return name;
		}

		public static bool ValidCPF(string cpf)
		{
			bool flag = false;
			int[] v = new int[cpf.Length];

			for (int i = 0; i < cpf.Length; i++)
				v[i] = int.Parse(cpf[i].ToString());

			if (v.Length == 11)
			{
				int v1 = (10 * v[0]) + (9 * v[1]) + (8 * v[2]) + (7 * v[3]) + (6 * v[4]) + (5 * v[5]) + (4 * v[6]) + (3 * v[7]) + (2 * v[8]);
				v1 = 11 - (v1 % 11);
				if (v1 >= 10)
					v1 = 0;
				//compute 2nd verification digit.
				int v2 = (11 * v[0]) + (10 * v[1]) + (9 * v[2]) + (8 * v[3]) + (7 * v[4]) + (6 * v[5]) + (5 * v[6]) + (4 * v[7]) + (3 * v[8]);
				v2 += 2 * v1;
				v2 = 11 - (v2 % 11);
				if (v2 >= 10)
					v2 = 0;
				//True if verification digits are as expected.
				flag = (v1 == v[9] && v2 == v[10]);
			}
			return flag;
		}

		public static string TwoDecimalPlacesNoRounding(decimal p)
		{
			string m = p.ToString() + "0";
			return m.Substring(0, m.IndexOf('.') + 3);
		}

		public static byte[] CompressBytes(byte[] rawBytes)
		{
			MemoryStream compressedMemoryStream = new MemoryStream();
			// Use the newly created memory stream for the compressed data.
			DeflateStream compressedzipStream = new DeflateStream(compressedMemoryStream, CompressionMode.Compress, true);
			compressedzipStream.Write(rawBytes, 0, rawBytes.Length);
			// Close the stream.
			compressedzipStream.Close();

			NLogger.Debug("Compressed byte array from " + rawBytes.Length + " bytes to " + compressedMemoryStream.Length + " bytes.");

			return compressedMemoryStream.ToArray();
		}

		public static byte[] DecompressBytes(byte[] compressedBytes)
		{
			MemoryStream compressedMemoryStream = new MemoryStream(compressedBytes);
			// Use the newly created memory stream for the compressed data.
			DeflateStream Decompress = new DeflateStream(compressedMemoryStream, CompressionMode.Decompress);

			byte[] decompressedBytes = new byte[compressedBytes.Length * 10];
			int offset = 0;
			int bytesRead;
			int totalCount = 0;
			do
			{
				bytesRead = Decompress.Read(decompressedBytes, offset, 100);
				offset += bytesRead;
				totalCount += bytesRead;
			} while (bytesRead != 0);

			Array.Resize(ref decompressedBytes, totalCount);
			NLogger.Debug("Decompressed byte array from " + compressedBytes.Length + " bytes back to " + totalCount + " bytes.");

			return decompressedBytes;
		}

		public static double ConvertBytesToMegabytes(long bytes)
		{
			return (bytes / 1024f) / 1024f;
		}

		public static void MemberwiseCrossClone(Object Source, Object Target)
		{
			Type SourceType = Source.GetType();
			Type TargetType = Target.GetType();
			foreach (FieldInfo _SourceField in SourceType.GetFields())
			{
				FieldInfo _TargetField = TargetType.GetField(_SourceField.Name);
				if (_TargetField != null)
					try
					{
						_TargetField.SetValue(Target, _SourceField.GetValue(Source));
					}
#if DEBUG
					catch (Exception ex)
					{
						NLogger.Error("EXCEPTION in MemberwiseCrossClone() field " + _SourceField.Name + ": " + ex.Message);
					}
#else
                    catch
                    {
                    }
#endif
			}
			foreach (PropertyInfo _SourceProperty in SourceType.GetProperties())
			{
				PropertyInfo _TargetProperty = TargetType.GetProperty(_SourceProperty.Name);
				if (_TargetProperty != null)
					try
					{
						_TargetProperty.SetValue(Target, _SourceProperty.GetValue(Source, null), null);
					}
#if DEBUG
					catch (Exception ex)
					{
						NLogger.Error("EXCEPTION in MemberwiseCrossClone() property " + _SourceProperty.Name + ": " + ex.Message);
					}
#else
                    catch
                    {
                    }
#endif
			}

		}

		public static void DumpMembers(Object Source)
		{
			if (Source == null)
				return;
			foreach (FieldInfo _SourceField in Source.GetType().GetFields())
			{
				try
				{
					object _SourceValue = _SourceField.GetValue(Source);
					if (_SourceValue == null)
						continue;
					if (_SourceValue.GetType() == typeof(String) && ((String)_SourceValue).Length == 0)
						continue;
					if (_SourceValue.GetType() == typeof(int) && (int)_SourceValue == int.MinValue)
						continue;
					if (_SourceValue.GetType() == typeof(long) && (long)_SourceValue == long.MinValue)
						continue;
					if (_SourceValue.GetType() == typeof(decimal) && (decimal)_SourceValue == decimal.MinValue)
						continue;
					if (_SourceValue.GetType() == typeof(DateTime) && (DateTime)_SourceValue == DateTime.MinValue)
						continue;
					if (_SourceValue.GetType() == typeof(Guid) && ((Guid)_SourceValue).Equals(Guid.Empty))
						continue;
					if (_SourceField.Name.Equals("PIN") && (int)_SourceValue == 11111)
						continue;
					NLogger.Debug(_SourceField.Name + ": " + (_SourceField.Name.Equals("PIN") ? "****" : _SourceField.GetValue(Source)));
				}
				catch (Exception ex)
				{
					NLogger.Error("EXCEPTION dumping member " + _SourceField.Name + ": " + ex.Message);
				}
			}
		}

		public static string ConvertListToString(List<string> Receipt)
		{
			return string.Join(System.Environment.NewLine, Receipt.ToArray());
		}

		public static List<string> ConvertStringToList(string Receipt)
		{
			return new List<string>(Receipt.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None));
		}

		public static List<string> ReceiptHeader(Language SelectedLanguage,
			string KioskId,
			string LogoFileName,
			string LocationName,
			string LocationAddress,
			string LocationCity,
			string LocationState,
			string LocationZip)
		{
			List<string> _Header = new List<string>();

			if (LogoFileName.Length > 0)
				_Header.Add(LogoFileName);
			_Header.Add((SelectedLanguage == Language.English ? ".c..f11..b.Nexxo® Kiosk " : ".c..f11..b.Cajero Nexxo® ") + KioskId);
			_Header.Add(".c..f11." + LocationName);
			_Header.Add(".c..f11." + LocationAddress);
			_Header.Add(".c..f11." + LocationCity + " " + LocationState + " " + LocationZip);
			_Header.Add(string.Empty);
			_Header.Add(string.Empty);

			return _Header;
		}

		public static List<string> ReceiptFooter(Language SelectedLanguage, int SessionId)
		{
			return ReceiptFooter(SelectedLanguage, SessionId, DateTime.Now);
		}

		public static List<string> ReceiptFooter(Language SelectedLanguage, int SessionId, DateTime dtKiosk)
		{
			if (dtKiosk == DateTime.MinValue)
				dtKiosk = DateTime.Now;
			List<string> _Footer = new List<string>();
			_Footer.Add(string.Empty);
			_Footer.Add(string.Empty);
			_Footer.Add(".c..f10." + dtKiosk.ToString("G"));
			_Footer.Add(".c..f10." + (SelectedLanguage == Language.English ? "Receipt Number: " : "Número de Recibo: ") + SessionId);
			return _Footer;
		}

		public static List<string> MakeReceipt(List<string> ReceiptHeader, List<string> ReceiptBody, List<string> ReceiptFooter)
		{
			List<string> _Receipt = new List<string>(ReceiptHeader);
			_Receipt.AddRange(ReceiptBody);
			_Receipt.AddRange(ReceiptFooter);
			return _Receipt;
		}

		public static bool IsValidEmailAddress(string email)
		{
			return Regex.IsMatch(email, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$");
		}

		public static bool IsValidPhoneNumber(string p)
		{
			return (!String.IsNullOrEmpty(p) && p.Length == 10 &&
				(Regex.IsMatch(p, @"^[2-9]\d{9}$") && !Regex.IsMatch(p, "2{9}|3{9}|4{9}|5{9}|6{9}|7{9}|8{9}|9{9}")));
		}

		public static string GetPurseName(string purseName, bool isEnglish)
		{
			string purseNameEn = "Cash Available ";
			string purseNameEs = "Efectivo Disponible ";
			return string.IsNullOrEmpty(purseName) ? isEnglish ? purseNameEn : purseNameEs : purseName;
		}

		public static string ExecuteRESTFulService(string requestUri)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
			HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
			Stream stream = webResponse.GetResponseStream();
			StreamReader sr = new StreamReader(stream);
			return sr.ReadToEnd();
		}

		public static string ExecuteRESTFulServiceAsBase64(string requestUri)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
			HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
			MemoryStream ms = new MemoryStream();
			Stream stream = webResponse.GetResponseStream();
			stream.CopyTo(ms);
			return Convert.ToBase64String(ms.ToArray());


		}


		/// <summary>
		/// Round off amount Up to decimal places
		/// </summary>
		/// <param name="amount">double</param>
		/// <returns>decimal</returns>
		public static string RoundOffDecimal(decimal amount, int decimalLength)
		{
			decimal finalAmount = (decimal)Math.Round(amount, decimalLength);
			string amountUptoTwoDecimals = string.Format("{0:0.00}", finalAmount);
			return amountUptoTwoDecimals;
		}

		/// <summary>
		/// Method to convert Amount to Words
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="majorCurrency"></param>
		/// <param name="minorCurrency"></param>
		/// <returns></returns>
		public static String AmountToString(double amount, string majorCurrency, string minorCurrency)
		{
			String words = "";
			bool majorPlural = true;
			//bool minorPlural = true;

			int majorAmount = (int)amount;
			int minorAmount = (int)Math.Round((amount - (int)amount) * 100.0);

			//set plural flags
			if (majorAmount == 1)
				majorPlural = false;

			//if (minorAmount == 1)
			//    minorPlural = false;


			//--- major ---
			words = NumberToWords(majorAmount);

			words += majorCurrency;
			if (majorPlural == true)
				words += "s";
			words += " and ";


			//--- minor ---
			//words += NumberToWords(minorAmount);
			//words += minorCurrency;
			//if (minorPlural == true)
			//    words += "s";
			words += minorAmount.ToString("00") + "/100";

			return words.Substring(0, 1).ToUpper() + words.Substring(1).ToLower();
		}


		public static string NumberToWords(int number)
		{
			if (number == 0)
				return "zero ";

			if (number < 0)
				return "minus " + NumberToWords(Math.Abs(number));

			string words = "";

			if ((number / 1000000) > 0)
			{
				words += NumberToWords(number / 1000000) + "million ";
				number %= 1000000;
			}

			if ((number / 1000) > 0)
			{
				words += NumberToWords(number / 1000) + "thousand ";
				number %= 1000;
			}

			if ((number / 100) > 0)
			{
				words += NumberToWords(number / 100) + "hundred ";
				number %= 100;
			}

			if (number > 0)
			{
				if (words != "")
					words += "and ";

				var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
				var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

				if (number < 20)
					words += unitsMap[number];
				else
				{
					words += tensMap[number / 10];
					if ((number % 10) > 0)
						words += "-" + unitsMap[number % 10];
				}

				words += " ";
			}

			return words;
		}

		public static String AmountToStringForTCF(double amount)
		{
			string words = "";
			bool majorPlural = true;

			int majorAmount = (int)amount;
			int minorAmount = (int)Math.Round((amount - (int)amount) * 100.0);

			//set plural flags
			if (majorAmount == 1)
				majorPlural = false;

			//--- major ---
			words = NumberToWordsForTCF(majorAmount);

			words += "AND ";
			if (majorPlural == true)
				words += minorAmount.ToString("00") + "/100";

			return "***" + words + "***";
		}

		public static string NumberToWordsForTCF(int number)
		{
			if (number == 0)
				return "zero ";

			if (number < 0)
				return "minus " + NumberToWordsForTCF(Math.Abs(number));

			string words = "";

			if ((number / 1000000) > 0)
			{
				words += NumberToWordsForTCF(number / 1000000) + "million ";
				number %= 1000000;
			}

			if ((number / 1000) > 0)
			{
				words += NumberToWordsForTCF(number / 1000) + "thousand ";
				number %= 1000;
			}

			if ((number / 100) > 0)
			{
				words += NumberToWordsForTCF(number / 100) + "hundred ";
				number %= 100;
			}

			if (number > 0)
			{
				var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
				var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

				if (number < 20)
					words += unitsMap[number];
				else
				{
					words += tensMap[number / 10];
					if ((number % 10) > 0)
						words += " " + unitsMap[number % 10];
				}

				words += " ";
			}

			return words.ToUpper();
		}

		public static string MassagingValue(string dataValue)
		{
			string result = null;
			if (!string.IsNullOrWhiteSpace(dataValue))
			{
				result = Regex.Replace(dataValue, @"[^a-zA-Z0-9\s]", " ");
				result = Regex.Replace(result, @"\s{2,}", " ");
			}
			return result;
		}

		public static object GetDictionaryValue(Dictionary<string, object> dictionary, string key)
		{
			if (dictionary.ContainsKey(key) == false)
				throw new Exception(String.Format("{0} not provided in dictionary", key));
			return dictionary[key];
		}

		public static string GetDictionaryValueIfExists(Dictionary<string, object> dictionary, string key)
		{
			string value = null;
			if (dictionary.ContainsKey(key))
			{
				return Convert.ToString(dictionary[key]);
			}
			return value;
		}

		public static bool GetBoolDictionaryValueIfExists(Dictionary<string, object> dictionary, string key)
		{			
			bool value = false;
			if (dictionary.ContainsKey(key))
			{
				return Convert.ToBoolean(dictionary[key]);
			}
			return value;
		}

		public static decimal GetDecimalDictionaryValueIfExists(Dictionary<string, object> dictionary, string key)
		{
			decimal value = 0.0M;
			if (dictionary.ContainsKey(key))
			{
				return Convert.ToDecimal(dictionary[key]);
			}
			return value;
		}

		public static long GetLongDictionaryValueIfExists(Dictionary<string, object> dictionary, string key)
		{
			long value = 0;
			if (dictionary.ContainsKey(key))
			{
				return Convert.ToInt64(dictionary[key]);
			}
			return value;
		}

		public static string safeSQLString(string s)
		{
			if (s != null)
				return s.Replace("\\", "").Replace("'", "''");
			else
				return null;

		}
		public static string getLastFour(string value, bool property = false)//used safeSQLString() too for some Tostring() in Data classeses
		{
			string temp = safeSQLString(value);
			if (property == false && !string.IsNullOrWhiteSpace(value) && value.Length > 4)
			{
				return "****" + value.Substring(value.Length - 4, 4);
			}
			if (string.IsNullOrEmpty(temp) || temp.Length < 4)
			{
				return " ";
			}
			return value.Substring(temp.Length - 4, 4);
		}
		public static string MaskSensitiveData(string value, string property)
		{
			if (string.IsNullOrWhiteSpace(value))
				return string.Empty;
			else if (value.Length < 4)
				return value;
			string maskedValue = value;
			switch (property)
			{
				case "SSN":
					maskedValue = "XXXX-XX-" + value.Substring(value.Length - 4, 4);
					break;
				case "GovernmentId":
				case "accountNumber":
					maskedValue = "****" + value.Substring(value.Length - 4, 4);
					break;
				case "AlloyID":
					maskedValue = value.Length < 6 ? value : value.Substring(0, 6) + "XXXXXX" + value.Substring(value.Length - 4, 4);
					break;
				case "AccountNumber":
					maskedValue = "****" + value.Substring(value.Length - 4, 4);
					break;
			}
			return maskedValue;
		}
		public static string cardLastFour(string value) // for any card(visa or masterCard or AmericanExpress or Diners Club etc..) it will return last four digits 
		{
			if (string.IsNullOrEmpty(value) || value.Length < 4)
			{
				return " 0000 ";
			}
			return value.Substring(value.Length - 4, 4);
		}

		public static string TrimString(string value)
		{
			if (string.IsNullOrEmpty(value))
				return value;
			return value.Trim();
		}

		public static int[] GetCertegyDeclineCodes()
		{
			int[] declineCodes = new int[] { 1, 2, 4, 16, 22, 23, 24 };
			return declineCodes;
		}

		public static string GetProcessor(int ChannelPartnerId, int MajorCode)
		{
			string Processor;
			switch (MajorCode)
			{
				case (int)ExceptionMajorCodes.CHECK_PROCESSING_EXCEPTION:
					switch (ChannelPartnerId)
					{
						case (int)ExceptionChannelPartners.Synovus:
						case (int)ExceptionChannelPartners.TCF:
						case (int)ExceptionChannelPartners.Carver:
							Processor = "INGO";
							break;
						default:
							Processor = null;
							break;
					}
					break;
				case (int)ExceptionMajorCodes.FUNDS_EXCEPTION:
					switch (ChannelPartnerId)
					{
						case (int)ExceptionChannelPartners.Synovus:
							Processor = "VisaDPS";
							break;
						case (int)ExceptionChannelPartners.TCF:
							Processor = "VISA";
							break;
						default:
							Processor = null;
							break;
					}
					break;
				case (int)ExceptionMajorCodes.BILL_PAY_EXCEPTION:
				case (int)ExceptionMajorCodes.MONEY_TRANSFER_EXCEPTION:
					switch (ChannelPartnerId)
					{
						case (int)ExceptionChannelPartners.Synovus:
						case (int)ExceptionChannelPartners.TCF:
						case (int)ExceptionChannelPartners.Carver:
							Processor = "Western Union";
							break;
						case (int)ExceptionChannelPartners.MGI:
						case (int)ExceptionChannelPartners.Redstone:
							Processor = "MoneyGram";
							break;
						default:
							Processor = null;
							break;
					}
					break;
				case (int)ExceptionMajorCodes.MONEY_ORDER_EXCEPTION:
					switch (ChannelPartnerId)
					{
						case (int)ExceptionChannelPartners.Synovus:
							Processor = "Continental";
							break;
						case (int)ExceptionChannelPartners.TCF:
							Processor = "TCF";
							break;
						default:
							Processor = null;
							break;
					}
					break;
				case (int)ExceptionMajorCodes.CLIENT_CUSTOMER_EXCEPTION:
					switch (ChannelPartnerId)
					{
						case (int)ExceptionChannelPartners.Synovus:
							Processor = "FIS";
							break;
						case (int)ExceptionChannelPartners.TCF:
							Processor = "RCIF";
							break;
						default:
							Processor = null;
							break;
					}
					break;
				default:
					Processor = null;
					break;
			}
			return Processor;
		}

		/// <summary>
		/// Method to get random long number
		/// </summary>
		public static long GetLongRandomNumber(Int32 iMax)
		{
			RandomCryptoServiceProvider randomCryptoServiceProvider = new RandomCryptoServiceProvider();
			var sBuilder = new StringBuilder();
			while (iMax > 0)
			{
				sBuilder.Append(randomCryptoServiceProvider.Next(10).ToString());
				iMax = iMax - 1;
			}
			string sRandom = sBuilder.ToString();
			long lRandom = Convert.ToInt64(sRandom);
			return lRandom;
		}

		/// <summary>
		/// Method to Get customer current age by DOB
		/// </summary>
		/// <param name="dateOfBirth">Customer DOB</param>
		/// <returns>Customer Current Age</returns>
		public static int GetCustomerAgeByDateOfBirth(DateTime dateOfBirth)
		{
			int currentAge = (DateTime.Today.Year - dateOfBirth.Year);
			if (dateOfBirth > DateTime.Today.AddYears(-currentAge))
			{ currentAge--; }

			return currentAge;
		}

		//Method to differentiate SSN and ITIN
		public static string GetIDCode(string SSN)
		{
			if (!string.IsNullOrEmpty(SSN))
			{
				if (SSN.Length == 9 && SSN.Substring(0, 1) == "9")
				{
					int value = Convert.ToInt16(SSN[3].ToString() + SSN[4].ToString());
					if (value >= 70 & value <= 88 | value >= 90 & value <= 92 | value >= 94 & value <= 99)
					{
						return "I";
					}
				}
				return "S";
			}
			return string.Empty;
		}
		//AL-2999 : As VisaDPS, I need to truncate profile element values sent from Alloy
		public static string MassagingValue(string value, int size)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				if (value.Length > size)
					value = value.Substring(0, size);
			}
			return value;
		}
	}
	public enum Product
	{
		ProcessCheck = 1,
		BillPayment,
		MoneyTransfer,
		ReceiveMoney,
		MoneyOrder,
		ProductCredential,
		TransactionHistory
	}

	public enum Processor
	{
		INGO = 1,
		WesternUnion,
		MoneyGram,
		VISA,
		TCF,
		Continental,
		TSys
	}

	public enum PermissionTypes
	{
		CanUnparkTransactions
	}

	public enum ProfileStatus
	{
		Active = 1,
		Inactive = 0,
		Closed = 2
	};

	public enum CheckEntryTypes
	{
		ScanWithImage = 1,
		ScanWithoutImage = 2,
		Manual = 3
	}

	public enum UserRoles
	{
		Teller = 1,
		Manager = 2,
		ComplianceManager = 3,
		SystemAdmin = 4
	}

	public enum ExceptionMajorCodes
	{
		CASH_EXCEPTION = 1000,
		CUSTOMER_EXCEPTION = 1001,
		CHECK_PROCESSING_EXCEPTION = 1002,
		FUNDS_EXCEPTION = 1003,
		BILL_PAY_EXCEPTION = 1004,
		MONEY_TRANSFER_EXCEPTION = 1005,
		MONEY_ORDER_EXCEPTION = 1006,
		COMPLIANCE_EXCEPTION = 1008,
		PARTNER_EXCEPTION = 1010,
		CLIENT_CUSTOMER_EXCEPTION = 1011,
		SYNOVUS_CUSTOMER_EXCEPTION = 1013
	}

	public enum ExceptionChannelPartners
	{
		MGI = 1,
		Centris = 27,
		Carver = 28,
		Synovus = 33,
		TCF = 34,
		Redstone = 35
	}

	public enum ChannelPartnersCompliance
	{
		MGICompliance = 1,
		CarverCompliance = 28,
		SynovusCompliance = 33,
		TCFCompliance = 34,
		RedstoneCompliance = 35,
	}

	/// <summary>
	/// AL-550
	/// </summary>
	public enum CustomerScreen
	{
		PersonalInfo,
		Identification,
		Employment,
		PinDetails,
		ProfileSummary
	}
}
