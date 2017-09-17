using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Fund.Data;
using System.Collections.Generic;

namespace TCF.Zeo.Cxn.Fund.Visa.Contract
{
    public interface IIO
    {
        /// <summary>
        /// This method is used to get the Card Information based on Proxy ID
        /// </summary>
        /// <param name="proxyId">Proxy Id</param>
        /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL </param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Card Informations</returns>
        CardInfo GetCardInfoByProxyId(string proxyId, Credential credential, ZeoContext context);

        /// <summary>
        /// This method is used to get the GetPsedoDDA From AliasId 
        /// </summary>
        /// <param name="aliasId">Alias Id</param>
        /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Account Number</returns>
        string GetPsedoDDAFromAliasId(long aliasId, Credential credential, ZeoContext context);

        /// <summary>
        /// This method is used for GPR card Activation
        /// </summary>
        /// <param name="account">Required the account details based on ProxyId, Pseudo number, username</param>
        /// <param name="initialLoadAmount">Initial Load Amount</param>
        /// <param name="cardInformation">Card Information</param>
        /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Issue Card Response</returns>
        CardPurchaseResponse IssueCard(CardAccount account, CustomerInfo customer, double initialLoadAmount, CardInfo cardInformation, Credential credential, ZeoContext context);

        /// <summary>
        /// This method is used to get the Card Balanace details
        /// </summary>
        /// <param name="aliasId">Alias Id</param>
        /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Card Balance</returns>
        CardBalanceInfo GetBalance(long aliasId, Credential credential, ZeoContext context);

        /// <summary>
        /// This method is used to load the amount in GPR card
        /// </summary>
        /// <param name="aliasId">Alias Id</param>
        /// <param name="loadAmount">Load Amount</param>
        /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns> Transaction Information</returns>
        LoadResponse Load(long aliasId, double loadAmount, Credential credential, ZeoContext context);

        /// <summary>
        /// This method is used to withdraw the amount in GPR card 
        /// </summary>
        /// <param name="aliasId">Alias Id</param>
        /// <param name="loadAmount">Load Amount</param>
        /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Boolean variable withdraw Success</returns>
        bool Withdraw(long aliasId, double loadAmount, Credential credential, ZeoContext context);

        /// <summary>
        /// This method to close the card account
        /// </summary>
        /// <param name="aliasId">Alias Id</param>
        /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Status</returns>
        bool CloseAccount(long aliasId, Credential credential, ZeoContext context);

        /// <summary>
        /// This method to update the card status.
        /// </summary>
        /// <param name="aliasId">Alias Id</param>
        /// <param name="cardStatus">The card status</param>
        /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Update status</returns>
        bool UpdateCardStatus(long aliasId, string cardStatus, Credential credential, ZeoContext context);

        /// <summary>
        /// This method to replace the card.
        /// </summary>
        /// <param name="aliasId">Alias Id</param>
        /// <param name="cardMaintenanceInfo"></param>
        /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        bool ReplaceCard(long aliasId, CardMaintenanceInfo cardMaintenanceInfo, Credential credential, ZeoContext context);

        /// <summary>
        /// This method to get all GPR transaction history.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Transactions</returns>
        List<TransactionHistory> GetTransactionHistory(TransactionHistoryRequest request, Credential credential, ZeoContext context);

        /// <summary>
        /// To Search card information in Visa by card number
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="credential"></param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        CardInfo GetCardInfoByCardNumber(string cardNumber, Credential credential, ZeoContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aliasId"></param>
        /// <param name="credential"></param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        CardInfo GetCardHolderInfo(long aliasId, Credential credential, ZeoContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aliasId"></param>
        /// <param name="account"></param>
        /// <param name="credential"></param>
        /// <param name="cardAccount"></param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        CardPurchaseResponse CompanianCardOrder(long aliasId, Credential credential, CustomerInfo customer, CardAccount cardAccount, ZeoContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aliasId"></param>
        /// <param name="credential"></param>
        /// <param name="context"></param>
        AccountHistory GetAccountholderInfo(long aliasId, Credential credential, ZeoContext context);
    }
}
