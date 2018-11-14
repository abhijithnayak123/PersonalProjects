using MGI.Common.Util;
using MGI.Cxn.Fund.Visa.Contract;
using MGI.Cxn.Fund.Visa.Data;
using MGI.Cxn.Fund.Visa.Prepaid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace MGI.Cxn.Fund.Visa.Impl
{
	public class SimulatorIO : IIO
	{
		public TLoggerCommon MongoDBLogger { private get; set; }

		public CardInfo GetCardInfoByProxyId(string proxyId, Credential credential)
		{
			CardInfo cardInformation = new CardInfo();
			cardInformation.AliasId = Convert.ToInt64(proxyId);
			cardInformation.CardNumber = GetCreditCardNumber();
			cardInformation.Balance = 0;
			cardInformation.CurrencyCode = "0";
			cardInformation.ExpirationMonth = 11;
			cardInformation.ExpirationYear = 2018;
			cardInformation.SubClientNodeId = 12094;
			cardInformation.ProxyId = proxyId;
			cardInformation.Status = "1";
			cardInformation.PromotionCode = "EMPLOYEE";
			return cardInformation;
		}

		public string GetPsedoDDAFromAliasId(long aliasId, Credential credential)
		{
			return Convert.ToString(aliasId);
		}

		public CardBalance GetBalance(long aliasId, Credential credential)
		{
			CardBalance cardBalance = new CardBalance()
			{
				AccountBalance = 100,
				Balance = 100,
				CardStatus = "3"
			};

			return cardBalance;
		}

		public LoadResponse Load(long aliasId, double loadAmount, Credential credential)
		{
			LoadResponse visaTransaction = new LoadResponse()
			{
				TransationId = null,
				ReloadAliasId = 0,
				TransactionKey = 212307763059261074
			};

			if (loadAmount > 6000)
			{
				throw new VisaProviderException("2322", "Simulator limit voilation detected");
			}
			return visaTransaction;
		}


		public bool Withdraw(long aliasId, double loadAmount, Credential credential)
		{
			return true;
		}

		public CardPurchaseResponse IssueCard(Account account, double initialLoadAmount, CardInfo cardInformation, Credential credential)
		{
			CardPurchaseResponse issueCardResponse = null;

			issueCardResponse = new CardPurchaseResponse()
			{
				AccountAliasId = cardInformation.AliasId,
				ConfirmationNumber = "212307763059261074"
			};

			return issueCardResponse;
		}

		public static string GetCreditCardNumber()
		{
			long creditCardNumber = 0;
			bool results = false;
			while (!results)
			{
				creditCardNumber = (NexxoUtil.GetLongRandomNumber(16));
				results = Mod10Check(Convert.ToString(creditCardNumber));
			}
			return Convert.ToString(creditCardNumber);
		}

		public static bool Mod10Check(string creditCardNumber)
		{
			if (string.IsNullOrEmpty(creditCardNumber))
			{
				return false;
			}
			int sumOfDigits = creditCardNumber.Where((e) => e >= '0' && e <= '9')
							.Reverse()
							.Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
							.Sum((e) => e / 10 + e % 10);

			return sumOfDigits % 10 == 0;
		}

		public bool CloseAccount(long aliasId, Credential credential)
		{
			return true;
		}

		public bool UpdateCardStatus(long aliasId, string cardStatus, Credential credential)
		{
			return true;
		}

		public bool ReplaceCard(long aliasId, Fund.Data.CardMaintenanceInfo cardMaintenanceInfo, Credential credential)
		{
			return true;
		}

		public List<Fund.Data.TransactionHistory> GetTransactionHistory(Fund.Data.TransactionHistoryRequest request, Credential credential)
		{
			List<MGI.Cxn.Fund.Data.TransactionHistory> transactionHistoryList = new List<MGI.Cxn.Fund.Data.TransactionHistory>();
			MGI.Cxn.Fund.Data.TransactionHistory trx = new MGI.Cxn.Fund.Data.TransactionHistory()
			{
				TransactionAmount = 100,
				PostedDateTime = DateTime.Now,
				TransactionDateTime = DateTime.Now,
				MerchantName = "Alloy",
				Location = "MoneyGram",
				TransactionDescription = "Nothing",
				DeclineReason = "Everything",
				AvailableBalance = 10,
				ActualBalance = 10

			};
			transactionHistoryList.Add(trx);

			return transactionHistoryList;
		}


		public CardInfo GetCardInfoByCardNumber(string cardNumber, Credential credential)
		{
			return new CardInfo();
		}

		public CardInfo GetCardHolderInfo(long aliasId, Credential credential)
		{
			CardInfo cardInformation = new CardInfo();
			cardInformation.CardNumber = "4756756000171434";
			cardInformation.AliasId = 90000001204419;
			cardInformation.PrimaryAliasId = 90000001204419;
			return cardInformation;
		}
		public CardPurchaseResponse CompanianCardOrder(long aliasId, Credential credential, Fund.Visa.Data.Account cardAccount, MGIContext mgiContext)
		{
			CardPurchaseResponse issueCardResponse = null;

			issueCardResponse = new CardPurchaseResponse()
			{
				AccountAliasId = aliasId,
				ConfirmationNumber = "212307763059261074"
			};

			return issueCardResponse;
		}
	}
}
