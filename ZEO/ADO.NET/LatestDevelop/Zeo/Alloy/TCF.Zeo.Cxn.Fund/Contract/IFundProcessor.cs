using TCF.Zeo.Common.Data;
using System.Collections.Generic;
using TCF.Zeo.Cxn.Fund.Data;

namespace TCF.Zeo.Cxn.Fund.Contract
{
    public interface IFundProcessor
    {
        /// <summary>
		/// Used for registering the card account with GPR provider
		/// </summary>
		/// <param name="cardAccount">This hold the customer profile information</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Card account unique identifier</returns>
		long Register(CardAccount cardAccount, ZeoContext context);

        /// <summary>
        /// Used for getting the balance amount associated with the GPR card
        /// </summary>
        /// <param name="Context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Balance amount present in the GPR card</returns>
        CardBalanceInfo GetBalance(ZeoContext context);

        /// <summary>
        /// Validates the request, account and the context for channel partner details and stages the load transaction
        /// </summary>
        /// <param name="fundRequest">It contains the amount used for transaction</param>
        /// <param name="Context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making the load transaction</returns>
        long Load(FundRequest fundRequest, ZeoContext context);

        /// <summary>
        /// Validates the request amount, account and the context for channel partner details and stages the withdraw transaction
        /// </summary>
        /// <param name="fundRequest">It contains the amount used for transaction</param>
        /// <param name="Context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Transaction Id used for making the withdraw transaction</returns>
        long Withdraw(FundRequest fundRequest, ZeoContext context);

        /// <summary>
        /// Commits the activation, load and withdraw transaction
        /// </summary>
        /// <param name="transactionId">Unique Id associated with each transaction</param>
        /// <param name="Context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Unique Id associated with the account containing the card number</returns>
        /// <param name="cardNumber">GPR card has a unique number used for transactions</param>
        void Commit(long transactionId, CustomerInfo customer, ZeoContext context, string cardNumber = "");

        /// <summary>
        /// Used for stage a transaction so that the card number can be activated for the associated customer
        /// </summary>
        /// <param name="fundRequest">It contains the amount used for activation</param>
        /// <param name="Context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Unique transactionId made for activation</returns>
        long Activate(FundRequest fundRequest, ZeoContext context);

        /// <summary>
        /// Used for fetching the card customer details associated with accountId
        /// This method is introduced to fix the issue faced with CardAccount Lookup(long accountId);
        /// </summary>
        /// <param name="isCardAccountActivated">This property used to update tVisa_Account against Card Activation</param>
        /// <param name="Context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Account details of the customer along with the decrypted card number</returns>
		// Made changes for ALM-5234,ALM-5235 - Due to active status flag, this is returning zero records even if the account exists in the database
        CardAccount Lookup(ZeoContext context, bool? isCardAccountActivated = null);

        /// <summary>
        /// Used for fetching the details associated with card number during card customer search
        /// </summary>
        /// <param name="CustomerId">Customer has a unique number</param>
        /// <param name="cardNumber">GPR card has a unique number used for transactions</param>
        /// <returns>Unique Account Id associated with the card customer's account</returns>
        bool AccountExists(long CustomerId, bool? isActive, ZeoContext context);

        /// <summary>
        /// Used for updating the amount during a transaction
        /// </summary>
        /// <param name="cxnFundTrxId">Unique Fund transaction Id present in partner database</param>
        /// <param name="amount">Updated amount for the transction</param>
        /// <param name="timezone">Time during the transaction</param>
        /// <returns>Unique transactionId associated with each transaction</returns>
        long UpdateAmount(long cxnFundTrxId, FundRequest fundRequest, string timezone, ZeoContext context);

        /// <summary>
        /// Used for fetching the recent card transaction and customer details
        /// </summary>
        /// <param name="cxnFundTrxId">Unique Fund transaction Id present in partner database</param>
        /// <param name="Context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Details of a recent transaction with customer details</returns>
        FundTrx Get(long cxnFundTrxId, bool isEditTransaction, ZeoContext context);

        /// <summary>
        /// Updating the card number and other card details associated with the account
        /// </summary>
        /// <param name="cardAccount">This hold the customer profile information</param>
        /// <param name="Context">This is the common dictionary parameter used to pass supplimental information</param>
        void UpdateRegistrationDetails(CardAccount cardAccount, ZeoContext context);

        /// <summary>
        /// Deleting the card account associated with the customer
        /// </summary>
        /// <param name="Context">This is the common dictionary parameter used to pass supplimental information</param>
        void Cancel(ZeoContext context);

        /// <summary>
        /// Used for updating the fund account of the card customer
        /// </summary>
        /// <param name="cardAccount">This hold the card customer profile information</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Unique Id associated with the account containing the card</returns>
        long UpdateAccount(CardAccount cardAccount, ZeoContext context);

        /// <summary>
        /// Getting transaction history of visa card transactions
        /// </summary>
        /// <param name="request">Transaction history selection type</param>
        /// <param name="Context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>All trnsactions</returns>
        List<TransactionHistory> GetTransactionHistory(TransactionHistoryRequest request, ZeoContext context);

        /// <summary>
        ///  Closing existing account(card)
        /// </summary>
        /// <param name="Context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        bool CloseAccount(ZeoContext context);

        /// <summary>
        /// updating the card status
        /// </summary>
        /// <param name="cardMaintenanceInfo">shipping type and updated card status</param>
        /// <param name="Context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        bool UpdateCardStatus(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context);

        /// <summary>
        /// Replace the existing card
        /// </summary>
        /// <param name="cardMaintenanceInfo">shipping type and updated card status</param>
        /// <param name="Context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        bool ReplaceCard(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context);

        /// <summary>
        /// Get Channel Partner Shipping Type for Existing Card
        /// </summary>
        /// <param name="Context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        List<ShippingTypes> GetShippingTypes(long channelPartnerId, ZeoContext context);

        /// <summary>
        /// Get Channel Partner Shipping Type for Existing Card
        /// </summary>
        /// <param name="cardMaintenanceInfo">shipping type and updated card status</param>
        /// <param name="Context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        double GetShippingFee(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context);

        /// <summary>
        /// Get Channel Partner FundFee for Existing Card
        /// </summary>
        /// <param name="feeType">Ihis passes Card Fee Type Information</param>
        /// <param name="Context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        double GetFundFee(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context);

        /// <summary>
        /// Associate's the visa Card with the Alloy customer
        /// </summary>
        /// <param name="cardNumber">This is a 16digit Card Number</param>
        /// <param name="Context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>CardAccount</returns>
        CustomerCard AssociateCard(CardAccount cardAccount, CustomerInfo customer, ZeoContext context, bool isNewCard = false);

        /// <summary>
        /// Gets the Card number associated with the customer
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        string GetPrepaidCardNumber(ZeoContext context);
    }
}
