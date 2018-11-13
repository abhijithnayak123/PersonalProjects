using MGI.Biz.Common.Data;
using MGI.Biz.FundsEngine.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;

namespace MGI.Biz.FundsEngine.Contract
{
	public interface IFundsEngine
	{
		/// <summary>
        /// Used for adding a GPR account to the customer in CXE, PTNR and CXN layer
		/// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="fundsAccount">Customer profile information</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Unique Id associated with the account</returns>
		long Add(long customerSessionId, FundsAccount fundsAccount, MGIContext mgiContext);

		/// <summary>
        ///  Authenticate the Funds account.
		/// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="accountIdentifier">GPR card has a unique number used for transactions</param>
        /// <param name="authenticationInfo">This parameter is not being used and should be removed</param>
        /// <param name="encryptionKey">This parameter is not being used and should be removed</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Updates the status of card</returns>
		bool AuthenticateCard(long customerSessionId, string accountIdentifier, string authenticationInfo, string encryptionKey, MGIContext mgiContext);

		/// <summary>
        /// Validates the request and stages the withdraw transaction
		/// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making withdraw transaction</returns>
		long Withdraw(long customerSessionId, Funds funds, MGIContext mgiContext);

		/// <summary>
        /// Validates the request and stages the load transaction
		/// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making load transaction</returns>
		long Load(long customerSessionId, Funds funds, MGIContext mgiContext);

		/// <summary>
        /// Commit the Withdraw Or Load transaction.
		/// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="cxeTrxId">Unique Id associated with fund transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <param name="cardNumber">GPR card has a unique number used for transactions</param>
		/// <returns>Provides the state of transaction</returns>
		int Commit(long customerSessionId, long cxeTrxId, MGIContext mgiContext, string cardNumber = "");

		/// <summary>
		/// A transaction staged for activation
		/// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="funds">Contains information related to fund transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making card activation</returns>
		long Activate(long customerSessionId, Funds funds, MGIContext context);

		/// <summary>
		/// Get the balance amount on the card.
		/// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Card Balance</returns>
		Biz.FundsEngine.Data.CardInfo GetBalance(long customerSessionId, MGIContext mgiContext);

		/// <summary>
        /// Used for fetching customer profile information.
		/// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Customer profile information</returns>
		FundsAccount GetAccount(long customerSessionId, MGIContext mgiContext);

		/// <summary>
		/// Gets transaction and customer details
		/// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="Id">Unique Id associated with the transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Transaction and account details</returns>
		Funds Get(long customerSessionId, long Id, MGIContext mgiContext);

		/// <summary>
		/// Get Fee for a particular type of funds transaction
		/// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="amount">Load/Withdraw amount in fund transaction</param>
        /// <param name="fundsType">Type of fund transaction</param> 
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Partner-configured fee object for given fundsType</returns>
		TransactionFee GetFee(long customerSessionId, decimal amount, FundType fundsType, MGIContext mgiContext);

		/// <summary>
        /// Update the transaction amount for the funds transaction
		/// </summary>
        /// <param name="cxeFundTrxId">Unique Id associated with fund transaction</param>
        /// <param name="amount">Updated amount for the fund transaction</param>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="fundType">Type of fund transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Unique transactionId associated with each transaction</returns>
		long UpdateAmount(long cxeFundTrxId, decimal amount, long customerSessionId, FundType fundType, MGIContext mgiContext);

		/// <summary>
        /// Get the minimum load amount based on load type ie initial or successive load.
		/// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="initialLoad">Flag to determine the load is during activation or transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Minimum amount required for a particular load transaction</returns>
		decimal GetMinimumLoadAmount(long customerSessionId, bool initialLoad, MGIContext mgiContext);

        /// <summary>
        /// Deletes the card account associated with the customer
        /// </summary>
        /// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
        /// <param name="fundsId">Unique Id associated with the card account</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void Cancel(long customerSessionId, long fundsId, MGIContext mgiContext);

		/// <summary>
		/// Getting transaction history of visa card transactions
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="request">Transaction history selection type</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>All trnsactions</returns>
		List<TransactionHistory> GetTransactionHistory(long customerSessionId, TransactionHistoryRequest request, MGIContext mgiContext);

		/// <summary>
		///  Closing existing account(card)
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		bool CloseAccount(long customerSessionId, MGIContext mgiContext);

		/// <summary>
		/// updating the card status
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="cardMaintenanceInfo">shipping type and updated card status</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		bool UpdateCardStatus(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);

		/// <summary>
		/// Replace the existing card
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="cardMaintenanceInfo">shipping type and updated card status</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		bool ReplaceCard(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);


		/// <summary>
		/// Get ChannelPartner Shipping Type for Visa Card
		/// </summary>			
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		List<MGI.Biz.FundsEngine.Data.ShippingTypes> GetShippingTypes(MGIContext mgiContext);

		/// <summary>
		/// Get Shipping Type Fee
		/// </summary>
		/// <param name="cardMaintenanceInfo"></param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		double GetShippingFee(CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);

		/// <summary>
		/// Associating the card with Alloy Customer
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="fundsAccount">Customer profile information</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Unique Id associated with the account</returns>
		long AssociateCard(long customerSessionId, FundsAccount fundsAccount, MGIContext mgiContext);

		/// <summary>
		/// This method is to get the Visa Fee
		/// </summary>
		/// <param name="cardMaintenanceInfo">Card maintenance details</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		double GetFundFee(CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);


		/// <summary>
		/// A transaction staged for AddOnCard order
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="funds">Contains information related to fund transaction</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Transaction Id used for making card activation</returns>
		long IssueAddOnCard(long customerSessionId, Funds funds, MGIContext mgiContext);
	}
}
