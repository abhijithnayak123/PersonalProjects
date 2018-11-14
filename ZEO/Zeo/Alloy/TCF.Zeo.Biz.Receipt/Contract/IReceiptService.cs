using System.Collections.Generic;
using TCF.Zeo.Common.Data;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Receipt.Contract
{
    public interface IReceiptService 
	{

        /// <summary>
        /// This method to get check receipt data based on check transaction Id. 
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
        Data.Receipt GetCheckReceipt(long transactionId, bool isReprint, commonData.ZeoContext context);

        /// <summary>
        /// This method to get fund receipt data based on fund transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for billpay transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
        Data.Receipt GetBillPayReceipt(long transactionId, bool isReprint, commonData.ZeoContext context);

        /// <summary>
        /// This method to get money transfer receipt data based on money transfer transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for money transfer transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
        Data.Receipt GetMoneyTransferReceipt(long transactionId, bool isReprint, commonData.ZeoContext context);

        /// <summary>
        /// This method to get fund receipt data based on fund transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for fund transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
        Data.Receipt GetFundsReceipt(long transactionId, bool isReprint, commonData.ZeoContext context);

        /// <summary>
        /// This method to get money order receipt data based on money order transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for money order transaction</param>
        /// <param name="isReprint">This indicates whether the receipt is getting re-printed or not</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		Data.Receipt GetMoneyOrderReceipt(long transactionId, bool isReprint, commonData.ZeoContext context);

        /// <summary>
        /// This method to get dodd frank receipt data based on transaction Id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for check transaction</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		Data.Receipt GetDoddFrankReceipt(long transactionId, commonData.ZeoContext context);


        /// <summary>
        /// This method to get summary receipt data based on shopping cart Id for reprint.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for transaction</param>
        /// <param name="transactiontype">the transaction type [Send Money, Money Order, Check Processing, BillPay,]</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		Data.Receipt GetSummaryReceipt(long customerSessionId, commonData.ZeoContext context);

        /// <summary>
        /// This method to get coupon code receipt data, when IsReferral is true.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Receipt Data</returns>
		Data.Receipt GetCouponReceipt(long customerSessionId, commonData.ZeoContext context);

        /// <summary>
        /// This method will get the cash drawer receipt Data
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        Data.CashDrawerReceipt GetCashDrawerReceipt(long agentId, long locationId, commonData.ZeoContext context);
    }
}
