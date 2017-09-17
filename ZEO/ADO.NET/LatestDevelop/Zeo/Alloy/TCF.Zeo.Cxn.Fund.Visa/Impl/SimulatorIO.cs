using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.Fund.Data;
using TCF.Zeo.Cxn.Fund.Data.Exceptions;
using TCF.Zeo.Cxn.Fund.Visa.Data.Exceptions;
using TCF.Zeo.Cxn.Fund.Visa.Contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TCF.Zeo.Cxn.Fund.Visa.Impl
{
    #region CardInformation
    //CardNumber = 4855079200017582
    //ProxyId = 79617859
    //Date = 201811
    //Pseudo Code = 39900000000044526
    #endregion

    public class SimulatorIO : IIO
    {
        public CardInfo GetCardInfoByProxyId(string proxyId, Credential credential, ZeoContext context)
        {
            CardInfo cardInformation = new CardInfo();
            cardInformation.AliasId = 20753855;
            cardInformation.CardNumber = "4855079200017582";
            cardInformation.Balance = 30;
            cardInformation.CurrencyCode = "0";
            cardInformation.ExpirationMonth = 11;
            cardInformation.ExpirationYear = 2018;
            cardInformation.SubClientNodeId = 12094;
            cardInformation.ProxyId = "79617859";
            cardInformation.Status = "1";
            cardInformation.PromotionCode = "EMPLOYEE";
            return cardInformation;
        }

        public string GetPsedoDDAFromAliasId(long aliasId, Credential credential, ZeoContext context)
        {
            return "39900000000044526";
        }

        public CardBalanceInfo GetBalance(long aliasId, Credential credential, ZeoContext context)
        {
            CardBalanceInfo cardBalance = new CardBalanceInfo()
            {
                AccountBalance = 100,
                Balance = 100,
                CardStatus = "2"
            };

            return cardBalance;

            //throw new VisaProviderException(ProviderException.PROVIDER_COMMUNICATION_ERROR, string.Empty, null);
        }

        public LoadResponse Load(long aliasId, double loadAmount, Credential credential, ZeoContext context)
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


        public bool Withdraw(long aliasId, double loadAmount, Credential credential, ZeoContext context)
        {
            return true;
        }

        public CardPurchaseResponse IssueCard(CardAccount account, CustomerInfo customer, double initialLoadAmount, CardInfo cardInformation, Credential credential, ZeoContext context)
        {
            CardPurchaseResponse issueCardResponse = null;

            issueCardResponse = new CardPurchaseResponse()
            {
                AccountAliasId = cardInformation.AliasId,
                ConfirmationNumber = "212307763059261074"
            };

            return issueCardResponse;
        }

        public bool CloseAccount(long aliasId, Credential credential, ZeoContext context)
        {
            return true;
        }

        public bool UpdateCardStatus(long aliasId, string cardStatus, Credential credential, ZeoContext context)
        {
            return true;
        }

        public bool ReplaceCard(long aliasId, CardMaintenanceInfo cardMaintenanceInfo, Credential credential, ZeoContext context)
        {
            return true;
        }

        public List<TransactionHistory> GetTransactionHistory(TransactionHistoryRequest request, Credential credential, ZeoContext context)
        {
            List<TransactionHistory> transactionHistoryList = new List<TransactionHistory>();
            TransactionHistory trx = new TransactionHistory()
            {
                TransactionAmount = 100,
                PostedDateTime = DateTime.Now,
                TransactionDateTime = DateTime.Now,
                MerchantName = "Zeo",
                Location = "MN",
                TransactionDescription = "Nothing",
                DeclineReason = "Everything",
                AvailableBalance = 10,
                ActualBalance = 10

            };
            transactionHistoryList.Add(trx);

            return transactionHistoryList;
        }


        public CardInfo GetCardInfoByCardNumber(string cardNumber, Credential credential, ZeoContext context)
        {
            return new CardInfo();
        }

        public CardInfo GetCardHolderInfo(long aliasId, Credential credential, ZeoContext context)
        {
            CardInfo cardInformation = new CardInfo();
            cardInformation.CardNumber = "4855078900069893";
            cardInformation.AliasId = 20753855;
            cardInformation.PrimaryAliasId = 20753855;
            return cardInformation;
        }
        public CardPurchaseResponse CompanianCardOrder(long aliasId, Credential credential, CustomerInfo customer, CardAccount cardAccount, ZeoContext context)
        {
            CardPurchaseResponse issueCardResponse = null;

            issueCardResponse = new CardPurchaseResponse()
            {
                AccountAliasId = 20753855,
                ConfirmationNumber = "212307763059261074"
            };

            return issueCardResponse;
        }

        public AccountHistory GetAccountholderInfo(long aliasId, Credential credential, ZeoContext context)
        {
            return new AccountHistory()
            {
                TransactionDate = DateTime.Now.AddDays(-5)
            };
        }
    }
}
