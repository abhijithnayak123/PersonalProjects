using MGI.Common.Util;
using MGI.Cxn.Common.Processor.Contract;
using MGI.Cxn.Fund.Data;
using System.Collections.Generic;

namespace MGI.Cxn.Fund.Contract
{
	public interface IFundProcessor : IProcessor
	{
		/// <summary>
		/// Used for registering the card account with GPR provider
		/// </summary>
		/// <param name="cardAccount">This hold the customer profile information</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <param name="processorResult">This hold the status like success or failure and also hold exception details</param>
		/// <returns>Card account unique identifier</returns>
		long Register(CardAccount cardAccount, MGIContext mgiContext, out ProcessorResult processorResult);

		/// <summary>
		/// Used for validating the card number and account associated with it
		/// </summary>
		/// <param name="cardNumber">GPR card has a unique number used for transactions</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <param name="processorResult">This hold the status like success or failure and also hold exception details</param>
		/// <returns>Unique Id associated with the account containing the card</returns>
		long Authenticate(string cardNumber, MGIContext mgiContext, out ProcessorResult processorResult);

		/// <summary>
		/// Used for getting the balance amount associated with the GPR card
		/// </summary>
		/// <param name="accountId">Unique Id associated with the account containing the card</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <param name="processorResult">This hold the status like success or failure and also hold exception details</param>
		/// <returns>Balance amount present in the GPR card</returns>
		CardInfo GetBalance(long accountId, MGIContext mgiContext, out ProcessorResult processorResult);

		/// <summary>
		/// Validates the request, account and the context for channel partner details and stages the load transaction
		/// </summary>
		/// <param name="accountId">Unique Id associated with the account containing the card</param>
		/// <param name="fundRequest">It contains the amount used for transaction</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <param name="processorResult">This hold the status like success or failure and also hold exception details</param>
		/// <returns>Transaction Id used for making the load transaction</returns>
		long Load(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult);

		/// <summary>
		/// Validates the request amount, account and the context for channel partner details and stages the withdraw transaction
		/// </summary>
		/// <param name="accountId">Unique Id associated with the account containing the card</param>
		/// <param name="fundRequest">It contains the amount used for transaction</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <param name="processorResult">This hold the status like success or failure and also hold exception details</param>
		/// <returns>Transaction Id used for making the withdraw transaction</returns>
		long Withdraw(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult);

		/// <summary>
		/// Commits the activation, load and withdraw transaction
		/// </summary>
		/// <param name="transactionId">Unique Id associated with each transaction</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <param name="processorResult">This hold the status like success or failure and also hold exception details</param>
		/// <returns>Unique Id associated with the account containing the card number</returns>
		/// <param name="cardNumber">GPR card has a unique number used for transactions</param>
		void Commit(long transactionId, MGIContext mgiContext, out ProcessorResult processorResult, string cardNumber = "");

		/// <summary>
		/// Used for stage a transaction so that the card number can be activated for the associated customer
		/// </summary>
		/// <param name="accountId">Unique Id associated with the account containing the card</param>
		/// <param name="fundRequest">It contains the amount used for activation</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <param name="processorResult">This hold the status like success or failure and also hold exception details</param>
		/// <returns>Unique transactionId made for activation</returns>
		long Activate(long accountId, FundRequest fundRequest, MGIContext mgiContext, out ProcessorResult processorResult);

		/// <summary>
		/// Used for fetching the card customer details associated with accountId 
		/// </summary>
		/// <param name="accountId">Unique Id associated with the account containing the card</param>
		/// <returns>Account details of the customer along with the decrypted card number</returns>
		CardAccount Lookup(long accountId);

		/// <summary>
		/// Used for fetching the card customer details associated with accountId
		/// This method is introduced to fix the issue faced with CardAccount Lookup(long accountId);
		/// </summary>
		/// <param name="accountId">Unique Id associated with the account containing the card</param>
		/// <param name="isCardAccountActivated">This property used to update tVisa_Account against Card Activation</param>
		/// <returns>Account details of the customer along with the decrypted card number</returns>
		CardAccount LookupCardAccount(long accountId, bool isCardAccountActivated = false);

		/// <summary>
		/// Used for fetching the details associated with card number during card customer search
		/// </summary>
		/// <param name="cardNumber">GPR card has a unique number used for transactions</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Unique Account Id associated with the card customer's account</returns>
		long GetPanForCardNumber(string cardNumber, MGIContext context);

		/// <summary>
		/// Used for updating the amount during a transaction
		/// </summary>
		/// <param name="cxnFundTrxId">Unique Fund transaction Id present in partner database</param>
		/// <param name="amount">Updated amount for the transction</param>
		/// <param name="timezone">Time during the transaction</param>
		/// <returns>Unique transactionId associated with each transaction</returns>
		long UpdateAmount(long cxnFundTrxId, FundRequest fundRequest, string timezone);

		/// <summary>
		/// Used for fetching the recent card transaction and customer details
		/// </summary>
		/// <param name="cxnFundTrxId">Unique Fund transaction Id present in partner database</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Details of a recent transaction with customer details</returns>
		FundTrx Get(long cxnFundTrxId, MGIContext mgiContext);

		/// <summary>
		/// Updating the card number associated with the account
		/// </summary>
		/// <param name="cxnAccountId">Unique Id associated with the cxn layer account containing the card</param>
		/// <param name="cardNumber">GPR card has a unique number used for transactions</param>
		/// <param name="timezone">Time during the transaction</param>
		void UpdateRegistrationDetails(long cxnAccountId, string cardNumber, string timezone);

		/// <summary>
		/// Updating the card number and other card details associated with the account
		/// </summary>
		/// <param name="cardAccount">This hold the customer profile information</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		void UpdateRegistrationDetails(CardAccount cardAccount, MGIContext mgiContext);

		/// <summary>
		/// Deleting the card account associated with the customer
		/// </summary>
		/// <param name="accountId">Unique Id associated with the account containing the card</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		void Cancel(long accountId, MGIContext mgiContext);

		/// <summary>
		/// Used for updating the fund account of the card customer
		/// </summary>
		/// <param name="cardAccount">This hold the card customer profile information</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Unique Id associated with the account containing the card</returns>
		long UpdateAccount(CardAccount cardAccount, MGIContext mgiContext);

		/// <summary>
		/// Getting transaction history of visa card transactions
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="request">Transaction history selection type</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>All trnsactions</returns>
		List<TransactionHistory> GetTransactionHistory(long accountId, TransactionHistoryRequest request, MGIContext mgiContext);

		/// <summary>
		///  Closing existing account(card)
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		bool CloseAccount(long accountId, MGIContext mgiContext);

		/// <summary>
		/// updating the card status
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="cardMaintenanceInfo">shipping type and updated card status</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		bool UpdateCardStatus(long accountId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);

		/// <summary>
		/// Replace the existing card
		/// </summary>
		/// <param name="customerSessionId">Unique Id allocated at the time of CustomerSession</param>
		/// <param name="cardMaintenanceInfo">shipping type and updated card status</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		bool ReplaceCard(long accountId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);

		/// <summary>
		/// Get Channel Partner Shipping Type for Existing Card
		/// </summary>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		List<MGI.Cxn.Fund.Data.ShippingTypes> GetShippingTypes(long channelPartnerId);

		/// <summary>
		/// Get Channel Partner Shipping Type for Existing Card
		/// </summary>
		/// <param name="cardMaintenanceInfo">shipping type and updated card status</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		double GetShippingFee(CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);

		/// <summary>
		/// Get Channel Partner FundFee for Existing Card
		/// </summary>
		/// <param name="feeType">Ihis passes Card Fee Type Information</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		double GetFundFee(CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext);

		/// <summary>
		/// Associate's the visa Card with the Alloy customer
		/// </summary>
		/// <param name="cardNumber">This is a 16digit Card Number</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>CardAccount</returns>
		long AssociateCard(CardAccount cardAccount, MGIContext mgiContext, bool isNewCard = false);
	}
}
