using MGI.Common.Util;
using MGI.Cxn.Fund.Visa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Fund.Visa.Contract
{
	public interface IIO
	{
		/// <summary>
		/// This method is used to get the Card Information based on Proxy ID
		/// </summary>
		/// <param name="proxyId">Proxy Id</param>
		/// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL </param>
		/// <returns>Card Informations</returns>
		CardInfo GetCardInfoByProxyId(string proxyId, Credential credential);
		 
		/// <summary>
		 /// This method is used to get the GetPsedoDDA From AliasId 
		/// </summary>
		/// <param name="aliasId">Alias Id</param>
		 /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
		/// <returns>Account Number</returns>
		 string GetPsedoDDAFromAliasId(long aliasId, Credential credential);
		 
		/// <summary>
		/// This method is used for GPR card Activation
		/// </summary>
		/// <param name="account">Required the account details based on ProxyId, Pseudo number, username</param>
		/// <param name="initialLoadAmount">Initial Load Amount</param>
		/// <param name="cardInformation">Card Information</param>
		 /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
		/// <returns>Issue Card Response</returns>
		 CardPurchaseResponse IssueCard(Account account, double initialLoadAmount, CardInfo cardInformation, Credential credential);
		 
		/// <summary>
		/// This method is used to get the Card Balanace details
		/// </summary>
		/// <param name="aliasId">Alias Id</param>
		 /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
		/// <returns>Card Balance</returns>
		 CardBalance GetBalance(long aliasId, Credential credential);
		 
		/// <summary>
		/// This method is used to load the amount in GPR card
		/// </summary>
		/// <param name="aliasId">Alias Id</param>
		/// <param name="loadAmount">Load Amount</param>
		 /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
		/// <returns> Transaction Information</returns>
		 LoadResponse Load(long aliasId, double loadAmount, Credential credential);
		 
		/// <summary>
		/// This method is used to withdraw the amount in GPR card 
		/// </summary>
		/// <param name="aliasId">Alias Id</param>
		/// <param name="loadAmount">Load Amount</param>
		 /// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
		/// <returns>Boolean variable withdraw Success</returns>
		bool Withdraw(long aliasId, double loadAmount, Credential credential);

		/// <summary>
		/// This method to close the card account
		/// </summary>
		/// <param name="aliasId">Alias Id</param>
		/// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
		/// <returns>Status</returns>
		bool CloseAccount(long aliasId, Credential credential);

		/// <summary>
		/// This method to update the card status.
		/// </summary>
		/// <param name="aliasId">Alias Id</param>
		/// <param name="cardStatus">The card status</param>
		/// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
		/// <returns>Update status</returns>
		bool UpdateCardStatus(long aliasId, string cardStatus, Credential credential);

		/// <summary>
		/// This method to replace the card.
		/// </summary>
		/// <param name="aliasId">Alias Id</param>
		/// <param name="cardMaintenanceInfo"></param>
		/// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
		/// <returns></returns>
		bool ReplaceCard(long aliasId, MGI.Cxn.Fund.Data.CardMaintenanceInfo cardMaintenanceInfo, Credential credential);

		/// <summary>
		/// This method to get all GPR transaction history.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="credential">Credentials based on Visa certificate to access the username and password, ServiceURL</param>
		/// <returns>List Of Transactions</returns>
		List<MGI.Cxn.Fund.Data.TransactionHistory> GetTransactionHistory(MGI.Cxn.Fund.Data.TransactionHistoryRequest request, Credential credential);

		/// <summary>
		/// To Search card information in Visa by card number
		/// </summary>
		/// <param name="cardNumber"></param>
		/// <param name="credential"></param>
		/// <returns></returns>
		Data.CardInfo GetCardInfoByCardNumber(string cardNumber, Credential credential);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aliasId"></param>
		/// <param name="credential"></param>
		/// <returns></returns>
		Data.CardInfo GetCardHolderInfo(long aliasId, Credential credential);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aliasId"></param>
		/// <param name="account"></param>
		/// <param name="credential"></param>
		/// <param name="cardAccount"></param>
		/// <returns></returns>
		CardPurchaseResponse CompanianCardOrder(long aliasId, Credential credential, Fund.Visa.Data.Account cardAccount, MGIContext mgiContext);
	}
}
