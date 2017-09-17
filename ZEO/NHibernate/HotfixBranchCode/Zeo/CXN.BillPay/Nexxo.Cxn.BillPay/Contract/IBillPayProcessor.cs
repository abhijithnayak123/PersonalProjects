using System.Collections.Generic;
using MGI.Cxn.BillPay.Data;
using MGI.Cxn.Common.Processor.Contract;
using MGI.Cxn.WU.Common.Contract;
using MGI.Cxn.WU.Common.Data;
using MGI.Common.Util;


namespace MGI.Cxn.BillPay.Contract
{
	public interface IBillPayProcessor : IProcessor
    {
        /// <summary>
        /// Validates the specified CXN account identifier.
        /// </summary>
        /// <param name="cxnAccountID">The CXN account identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="mgiContext">The context.</param>
        /// <returns></returns>
        long Validate(long cxnAccountID, BillPayRequest request, MGIContext mgiContext);

        /// <summary>
        /// Commits the specified transaction identifier.
        /// </summary>
        /// <param name="transactionID">The transaction identifier.</param>
        /// <param name="mgiContext">The context.</param>
        /// <returns></returns>
        long Commit(long transactionID, MGIContext mgiContext);

        /// <summary>
        /// Adds the bill pay account.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="timezone">The timezone.</param>
        /// <returns></returns>
        long AddBillPayAccount(BillPayRequest request,string timezone);

        /// <summary>
        /// Gets the bill pay account.
        /// </summary>
        /// <param name="cxnAccountID">The CXN account identifier.</param>
        /// <returns></returns>
        BillPayAccount GetBillPayAccount(long cxnAccountID);

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <param name="transactionID">The transaction identifier.</param>
        /// <returns></returns>
        BillPayTransaction GetTransaction(long transactionID);

        // Added for User Story # US1646.
        //IWUCommon WuCommon { private get; set; }

        /// <summary>
        /// Gets the locations.
        /// </summary>
        /// <param name="billerName">Name of the biller.</param>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="mgiContext">The context.</param>
        /// <returns></returns>
		List<Location> GetLocations(string billerName, string accountNumber, decimal amount, MGIContext mgiContext);

        /// <summary>
        /// Gets the fee.
        /// </summary>
        /// <param name="billerName">Name of the biller.</param>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="location">The location.</param>
		/// <param name="mgiContext">The context.</param>
        /// <returns></returns>
		Fee GetFee(string billerName, string accountNumber, decimal amount, Location location, MGIContext mgiContext);

        /// <summary>
        /// Gets the biller information.
        /// </summary>
        /// <param name="billerName">Name of the biller.</param>
		/// <param name="mgiContext">The context.</param>
        /// <returns></returns>
		BillerInfo GetBillerInfo(string billerName, MGIContext mgiContext);

        /// <summary>
        /// Gets the provider attributes.
        /// </summary>
        /// <param name="billerName">Name of the biller.</param>
        /// <param name="locationName">Name of the location.</param>
        /// <param name="mgiContext">The context.</param>
        /// <returns></returns>
		List<Field> GetProviderAttributes(string billerName, string locationName, MGIContext mgiContext);

        /// <summary>
        /// Updates the card details.
        /// </summary>
        /// <param name="cxnAccountId">The CXN account identifier.</param>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="mgiContext">The context.</param>
        /// <param name="timezone">The timezone.</param>
        /// <returns></returns>
		long UpdateCardDetails(long cxnAccountId, string cardNumber, MGIContext mgiContext, string timezone);

        /// <summary>
        /// Gets the card information.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
		/// <param name="mgiContext">The context.</param>
        /// <returns></returns>
		Cxn.BillPay.Data.CardInfo GetCardInfo(string cardNumber, MGIContext mgiContext);

        /// <summary>
        /// Method to import Past Biller from WU for User Story # US1646.
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="cardNumber"></param>
		/// <param name="mgiContext"></param>
        /// <returns></returns>
		List<Biller> GetPastBillers(long customerSessionId, string cardNumber, MGIContext mgiContext);

        /// <summary>
        /// Gets the biller last transaction.
        /// </summary>
        /// <param name="billerCode">The biller code.</param>
        /// <param name="cxnAccountID">The CXN account identifier.</param>
		/// <param name="mgiContext">The CXN context.</param>
        /// <returns></returns>
		BillPayTransaction GetBillerLastTransaction(string billerCode, long cxnAccountID, MGIContext mgiContext);
    	
	    /// <summary>
        /// Updates the account.
        /// </summary>
        /// <param name="account">The account.</param>
		/// <param name="mgiContext">The CXN context.</param>	
		void UpdateAccount(BillPayAccount account, MGIContext mgiContext);

        /// <summary>
        /// Updates the gold card points.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="totalPointsEarned">The total points earned.</param>
        /// <param name="mgiContext">The context.</param>
		void UpdateGoldCardPoints(long transactionId, string totalPointsEarned, MGIContext mgiContext);
	}
}
