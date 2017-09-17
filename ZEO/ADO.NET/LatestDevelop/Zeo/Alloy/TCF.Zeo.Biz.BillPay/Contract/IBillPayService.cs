using TCF.Channel.Zeo.Data;
using System.Collections.Generic;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.BillPay.Contract
{
    public interface IBillPayService
    {
        /// <summary>
        /// THis method to fetch fee for billpay.
        /// </summary>
        /// <param name="billerName">the billar name</param>
        /// <param name="accountNumber">the account number for biller</param>
        /// <param name="amount">the amount</param>
        /// <param name="location">A transient instance of Location[Class]</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Fee Details</returns>
        BillPayFee GetFee(long transactionId, string billerName, string accountNumber, decimal amount, BillerLocation location, commonData.ZeoContext context);

        /// <summary>
        /// THis method to fetch the Locations for billpay.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
        /// <param name="billerName">the billar name</param>
        /// <param name="accountNumber">the account number for biller</param>
        /// <param name="amount">the amount</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>BillPay Location Details</returns>
		BillPayLocation GetLocations(long transactionId, string billerName, string accountNumber, decimal amount, commonData.ZeoContext context);

        /// <summary>
        /// This method to add billpay transaction.
        /// </summary>
        /// <param name="context">This is the unique identifier for billpay transaction</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		void Submit(long transactionId, commonData.ZeoContext context);

        /// <summary>
        /// This method to commit the billpay transaction.
        /// </summary>      
        /// <param name="transactionId">This is the unique identifier for bill payment</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
        void Commit(long transactionId, commonData.ZeoContext context);

        /// <summary>
        /// To fetch billpay transaction details.
        /// </summary>
        /// <param name="ptnrTrxId">This is the unique identifier for biller</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>BillPay Transaction Details</returns>
        BillPayTransaction GetTransaction(long transactionId, commonData.ZeoContext context);

        /// <summary>
        /// This method is used to get Validate BillPay request
        /// </summary>
        /// <param name="billPayment">A transient instance of BillPayment[Class]</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>bool</returns>
        BillPayValidateResponse Validate(long transactionId, BillPayment billPayment, commonData.ZeoContext context);

        /// <summary>
        /// This method is used to Cancel the Bill Payment Transaction
        /// </summary>
        /// <param name="transactionId">This is the unique identifier for billpay transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        void Cancel(long transactionId, commonData.ZeoContext context);

        /// <summary>
        /// This method is used to Get frequent billers for customer
        /// </summary>      
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
        List<FavoriteBiller> GetFrequentBillers(commonData.ZeoContext context);

        /// <summary>
        /// This method is used to get biller information like delivery option, message etc.
        /// </summary>
        /// <param name="billerNameOrCode">This has biller Name or Code</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Biller information like deliveryoption, denominatins and many more</returns>
        BillerInfo GetBillerInfo(string billerNameOrCode, commonData.ZeoContext context);

        /// <summary>
        /// It retrieves the dynamic fields required to make the bill pay or send money transaction based on parameters 
        /// like amount, country and delivery service
        /// </summary>
        /// <param name="billerName">The biller Name</param>
        /// <param name="location">the location Name</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>dynamic fields required</returns>
		List<Field> GetProviderAttributes(string billerName, string location, commonData.ZeoContext context);

        /// <summary>
        /// To fetch favorite biller based on biller name.
        /// </summary>
        /// <param name="billerName">the biller name</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Favorite Biller Details</returns>
		FavoriteBiller GetBillerDetails(string billerName, commonData.ZeoContext context);

        /// <summary>
        /// This method to delete favorite biller form favorite billers list based on biller id and customer session id..
        /// </summary>
        /// <param name="billerId">This is the unique identifier for biller</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        List<FavoriteBiller> DeleteFavouriteBiller(long billerId, commonData.ZeoContext context);

        /// <summary>
        /// This method is for adding past billers in the DMS DB's for User Story # US# US1646.
        /// </summary>
        /// <param name="cardNumber">Card Number</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        List<FavoriteBiller> AddPastBillers(string cardNumber, commonData.ZeoContext context);

        List<string> GetBillers(string searchTerm, int channelPartnerID, commonData.ZeoContext context);

        /// <summary>
        /// This method is used to get card info[Western Union Gold Card].
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Card Info Details</returns>
		CardInfo GetCardInfo(commonData.ZeoContext context);

        /// <summary>
        /// This method is used to update the bill pay transation state
        /// </summary>
        /// <param name="transactionId">this is transaction Id</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        void UpdateTransactionState(long transactionId, int state, commonData.ZeoContext context);
    }
}
