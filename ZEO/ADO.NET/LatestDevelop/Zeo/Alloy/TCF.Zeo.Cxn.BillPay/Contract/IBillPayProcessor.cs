using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.BillPay.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.BillPay.Contract
{
    public interface IBillPayProcessor
    {
        /// <summary>
        /// Validates the specified CXN account identifier.
        /// </summary>
        /// <param name="cxnAccountID">The CXN account identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        BillPayValidateResponse Validate(long cxnTransactionId, BillPayment billPayment, ZeoContext context);

        /// <summary>
        /// Commits the specified transaction identifier.
        /// </summary>
        /// <param name="cxnTransactionId">The transaction identifier.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        BillPayTransaction Commit(long cxnTransactionId, ZeoContext context);

        /// <summary>
        /// Gets the bill pay account.
        /// </summary>
        /// <param name="customerSessionId">Customer Session Id</param>
        /// <returns></returns>
        long GetBillPayAccountId(long customerSessionId, string timeZone, ZeoContext context);

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <param name="transactionID">The transaction identifier.</param>
        /// <returns></returns>
        BillPayTransaction GetTransaction(long cxnTransactionId, ZeoContext context);

        // Added for User Story # US1646.
        //IWUCommon WuCommon { private get; set; }

        /// <summary>
        /// Gets the locations.
        /// </summary>
        /// <param name="billerName"></param>
        /// <param name="accountNumber"></param>
        /// <param name="amount"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<Location> GetLocations(string billerName, string accountNumber, decimal amount, ZeoContext context);

        /// <summary>
        /// Gets the fee.
        /// </summary>
        /// <param name="billerName"></param>
        /// <param name="accountNumber"></param>
        /// <param name="amount"></param>
        /// <param name="location"></param>
        /// <param name="billPayRequest"></param>
        /// <param name="context"></param>
        /// <returns></returns>
		Fee GetFee(long cxnTransactionId, string billerName, string accountNumber, decimal amount, Location location, ZeoContext context);

        /// <summary>
        /// Gets the biller information.
        /// </summary>
        /// <param name="billerName">Name of the biller.</param>
		/// <param name="context">The context.</param>
        /// <returns></returns>
		BillerInfo GetBillerInfo(string billerName, ZeoContext context);

        /// <summary>
        /// Gets the provider attributes.
        /// </summary>
        /// <param name="billerName">Name of the biller.</param>
        /// <param name="locationName">Name of the location.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
		List<Field> GetProviderAttributes(string billerName, string locationName, ZeoContext context);

        /// <summary>
        /// Method to import Past Biller from WU for User Story # US1646.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<Biller> GetPastBillers(string cardNumber, ZeoContext context);

        /// <summary>
        /// Gets the card information.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
		/// <param name="mgiContext">The context.</param>
        /// <returns></returns>
		CardInfo GetCardInfo(string cardNumber, ZeoContext context);

        /// <summary>
        /// Update WU gold card points for the transaction
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="cardNumber"></param>
        /// <param name="context"></param>
        void UpdateBillPayGoldCardPoints(long transactionId, string cardNumber, ZeoContext context);
    }
}
