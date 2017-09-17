using System.Collections.Generic;
using MGI.Biz.BillPay.Data;
using MGI.Common.Util;

namespace MGI.Biz.BillPay.Contract
{
    public interface IBillPayService
    {
        //Commented By sakahatech
        //long Validate(long customerSessionId, long billerID, BillPayment payment, MGIContext mgiContext);
        /// <summary>
        /// This method to commit the billpay transaction.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="transactionId">This is the unique identifier for bill payment</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        void Commit(long customerSessionId, long transactionId, MGIContext mgiContext);
        
        /// <summary>
        /// This method is used to Get frequent billers for customer
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="alloyId">This is the unique identifier for alloy customer</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        List<MGI.Biz.BillPay.Data.Product> GetPreferredProducts(long customerSessionId, long alloyId, MGIContext mgiContext);

        /// <summary>
        /// THis method to fetch the Locations for billpay.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="billerName">the billar name</param>
        /// <param name="accountNumber">the account number for biller</param>
        /// <param name="amount">the amount</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>BillPay Location Details</returns>
		BillPayLocation GetLocations(long customerSessionId, string billerName, string accountNumber, decimal amount, MGIContext mgiContext);

        /// <summary>
        /// THis method to fetch fee for billpay.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="billerName">the billar name</param>
        /// <param name="accountNumber">the account number for biller</param>
        /// <param name="amount">the amount</param>
        /// <param name="location">A transient instance of Location[Class]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Fee Details</returns>
		Fee GetFee(long customerSessionId, string billerName, string accountNumber, decimal amount, Location location, MGIContext mgiContext);

        /// <summary>
        /// This method is used to get biller information like delivery option, message etc.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="billerNameOrCode">This has biller Name or Code</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Biller information like deliveryoption, denominatins and many more</returns>
		BillerInfo GetBillerInfo(long customerSessionId, string billerNameOrCode, MGIContext mgiContext);

        /// <summary>
        /// It retrieves the dynamic fields required to make the bill pay or send money transaction based on parameters 
        /// like amount, country and delivery service
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="billerName">The biller Name</param>
        /// <param name="location">the location Name</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>dynamic fields required</returns>
		List<Field> GetProviderAttributes(long customerSessionId, string billerName, string location, MGIContext mgiContext);

        /// <summary>
        /// To fetch favorite biller based on biller name.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="billerName">the biller name</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Favorite Biller Details</returns>
		FavoriteBiller GetFavoriteBiller(long customerSessionId, string billerName, MGIContext mgiContext);

        /// <summary>
        /// This method to update Western Union Gold card Details.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="WUGoldCardNumber">Western union gold</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>bool</returns>
		long UpdateWUCardDetails(long customerSessionId, string WUGoldCardNumber, MGIContext mgiContext);

        /// <summary>
        /// To fetch billpay transaction details.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="ptnrTrxId">This is the unique identifier for biller</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>BillPay Transaction Details</returns>
		BillPayTransaction GetTransaction(long customerSessionId, long ptnrTrxId, MGIContext mgiContext);

        /// <summary>
        /// This method to fetch billpay transaction fee.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="providerName">This is Provider Name</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>fee</returns>
		decimal GetBillPayFee(long customerSessionId, string providerName, MGIContext mgiContext);

        /// <summary>
        /// This method is used to get Validate BillPay request
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="billPayment">A transient instance of BillPayment[Class]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>bool</returns>
		long Validate(long customerSessionId, BillPayment billPayment, MGIContext mgiContext);

        /// <summary>
        /// This method to add billpay transaction.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="transactionId">This is the unique identifier for billpay transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void Add(long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This method to used to get card info[Western Union Gold Card].
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Card Info Details</returns>
		CardInfo GetCardInfo(long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method is for adding past billers in the DMS DB's for User Story # US# US1646.
        /// </summary>
        /// <param name="customerSessionId">Session ID</param>
        /// <param name="cardNumber">Card Number</param>
		/// <param name="mgiContext">Context</param>
		void AddPastBillers(long customerSessionId, string cardNumber, MGIContext mgiContext);

        /// <summary>
        /// This method is for adding favorite billers in the Alloy DB's.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="favoriteBiller">A transient instance of FavoriteBiller[Class]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void AddFavoriteBiller(long customerSessionId, FavoriteBiller favoriteBiller, MGIContext mgiContext);
        
        /// <summary>
        /// This method to Updating the favorite biller account number.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="billerId">This is the unique identifier for biller</param>
        /// <param name="accountNumber"></param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
		bool UpdateFavoriteBillerAccountNumber(long customerSessionId, long billerId, string accountNumber, MGIContext mgiContext);

        /// <summary>
        /// /// This method is used to Update Favorite Biller Status
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="billerId">This is the unique identifier for biller</param>
        /// <param name="status">This is has status of the bool</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>bool</returns>
		bool UpdateFavoriteBillerStatus(long customerSessionId, long billerId, bool status, MGIContext mgiContext);
        
        /// <summary>
        /// The user / teller will get Billers Last Transaction.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="billerCode">This contains billercode</param>
        /// <param name="alloyId">This is the unique identifier for alloy customer</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Billpay Transaction Details</returns>
		BillPayTransaction GetBillerLastTransaction(long customerSessionId, string billerCode, long alloyId, MGIContext mgiContext);

        /// <summary>
        /// This method is used to Cancel the Bill Payment Transaction
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="transactionId">This is the unique identifier for billpay transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void Cancel(long customerSessionId, long transactionId, MGIContext mgiContext);

		//Begin TA-191 Changes
		//       User Story Number: TA-191 | Biz |   Developed by: Sunil Shetty     Date: 21.04.2015
		//       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. we are sending Customer session id and biller id.
		// the customer session id will get AlloyID/Pan ID and biller ID will help us to disable biller from tCustomerPreferedProducts table
		/// <summary>
		/// This method to delete favorite biller form favorite billers list based on biller id and customer session id..
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="billerId">This is the unique identifier for biller</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void DeleteFavoriteBiller(long customerSessionId, long billerId, MGIContext mgiContext);

		/// <summary>
		/// This method used for updating transaction status to faild when the faild responce come from processor.
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="transactionId">This is the unique identifier for biller</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void UpdateTransactionStatus(long customerSessionId, long transactionId, MGIContext mgiContext);
	}
}
