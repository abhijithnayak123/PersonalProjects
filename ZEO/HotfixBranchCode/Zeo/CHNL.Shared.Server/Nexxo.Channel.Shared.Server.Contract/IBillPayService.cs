using System;
using System.Collections.Generic;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;


namespace MGI.Channel.Shared.Server.Contract
{
    public interface IBillPayService
    {

        #region Biller Related Methods
		/// <summary>
		/// This method to get biller.
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for customerSession</param>
		/// <param name="channelPartnerId">This is unique identifier for channel partner</param>
		/// <param name="searchTerm">this is the first 4 digit of biller Name</param>
		/// <param name="context"></param>
		/// <returns>List Of Biller</returns>
        List<MGI.Channel.Shared.Server.Data.Product> GetBillers(long customerSessionId, long channelPartnerId, string searchTerm, MGIContext context);

		/// <summary>
		/// This method to get Frequent Biller for current customer.
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for customerSession</param>
		/// <param name="alloyId">This is unique identifier for customer</param>
		/// <param name="context"></param>
		/// <returns>List Of Biller</returns>
		List<MGI.Channel.Shared.Server.Data.Product> GetFrequentBillers(long customerSessionId, long alloyId, MGIContext context);

		/// <summary>
		/// This method to add favorite biller.
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="favoriteBiller"></param>
		/// <param name="context"></param>
        void AddFavoriteBiller(long customerSessionId, FavoriteBiller favoriteBiller, MGIContext context);

        bool UpdateFavoriteBillerAccountNumber(long customerSessionId, long billerId, string accountNumber, MGIContext context);

        bool UpdateFavoriteBillerStatus(long customerSessionId, long billerId, bool status, MGIContext context);

        BillerInfo GetBillerInfo(long customerSessionId, string billerNameOrCode, MGIContext context);

        FavoriteBiller GetFavoriteBiller(long customerSessionId, string billerNameOrCode, MGIContext context);

		//Begin TA-191 Changes
		//       User Story Number: TA-191 | Server |   Developed by: Sunil Shetty     Date: 21.04.2015
		//       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. we are sending Customer session id and biller id.
		// the customer session id will get AlloyID/Pan ID and biller ID will help us to disable biller from tCustomerPreferedProducts table
		void DeleteFavoriteBiller(long customerSessionId, long billerID, MGIContext context);

        #endregion

        #region BillPay Trx Methods

        BillFee GetBillPaymentFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation location, MGIContext context);

        List<Field> GetBillPaymentProviderAttributes(long customerSessionId, string billerNameOrCode, string location, MGIContext context);

        long ValidateBillPayment(long customerSessionId, BillPayment billPayment, MGIContext context);

        void StageBillPayment(long customerSessionId, long transactionId, MGIContext context);

       // void CommitBillPayment(long customerSessionId, long transactionId, MGIContext context);

        BillPayTransaction GetBillerLastTransaction(long customerSessionId, string billerCode, long alloyId, MGIContext context);

        #endregion

    }
}
