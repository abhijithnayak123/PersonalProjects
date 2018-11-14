using System.Collections.Generic;
using MGI.Cxn.Common.Processor.Contract;
using BIZPtrnData =MGI.Biz.Partner.Data;
using BIZReceiptData= MGI.Biz.Receipt.Data;
using MGI.Common.Util;
namespace MGI.Biz.Receipt.Contract
{
    public interface IReceiptService : IProcessor
	{
        /// <summary>
        /// This method to get check receipt data based on check transaction Id. 
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
        List<Data.Receipt> GetCheckReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext);
        
        /// <summary>
        /// This method to get fund receipt data based on fund transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for billpay transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		List<Data.Receipt> GetBillPayReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext);
        
        /// <summary>
        /// This method to get money transfer receipt data based on money transfer transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for money transfer transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		List<Data.Receipt> GetMoneyTransferReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext);
        
        /// <summary>
        /// This method to get fund receipt data based on fund transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for fund transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		List<Data.Receipt> GetFundsReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext);

        /// <summary>
        /// This method to get money order receipt data based on money order transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for money order transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		List<Data.Receipt> GetMoneyOrderReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext);

        /// <summary>
        /// This method to get dodd frank receipt data based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		List<Data.Receipt> GetDoddFrankReceipt(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get summary receipt data based on shopping cart Id.
        /// </summary>
        /// <param name="cart">A transient instance of ShoppingCart[Class]</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		List<Data.Receipt> GetSummaryReceipt(BIZPtrnData.ShoppingCart cart, long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get summary receipt data based on shopping cart Id for reprint.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for transaction</param>
        /// <param name="transactiontype">the transaction type [Send Money, Money Order, Check Processing, BillPay,]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		List<Data.Receipt> GetSummaryReceipt(long agentSessionId, long customerSessionId, long transactionId, string transactiontype, MGIContext mgiContext);

        /// <summary>
        /// This method to get fund receipt data based on fund transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="fundTransactions">A transient instance of Funds[Class]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		List<Data.Receipt> GetFundsActivationReceipt(long agentSessionId, long customerSessionId, List<BIZPtrnData.Transactions.Funds> fundTransactions, MGIContext mgiContext);

        /// <summary>
        /// This method to get coupon code receipt data, when IsReferral is true.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		List<Data.Receipt> GetCouponReceipt(long customerSessionId, MGIContext mgiContext);

		List<Data.Receipt> GetCheckDeclinedReceiptData(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext);

	}
}
