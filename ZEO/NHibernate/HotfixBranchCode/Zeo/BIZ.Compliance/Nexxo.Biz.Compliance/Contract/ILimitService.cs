using System.Collections.Generic;
using MGI.Biz.Compliance.Data;
using MGI.Common.Util;

namespace MGI.Biz.Compliance.Contract
{
	public interface ILimitService
	{
		/// <summary>
		/// This method is to calculate per transaction maximum limit
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for customerSession</param>
		/// <param name="complianceProgramName">This is channel partner specific compliance program name</param>
		/// <param name="transactionType">This is the current transaction/product type</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		/// <returns>The maximum amount allowed for that transaction</returns>
		decimal CalculateTransactionMaximumLimit(long customerSessionId, string complianceProgramName, TransactionTypes transactionType, MGIContext mgiContext);

		/// <summary>
		/// This method is to get per product minimum amount
		/// </summary>
		/// <param name="complianceProgramName">This is channel partner specific compliance program name</param>
		/// <param name="transactionType">This is the current transaction type</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		/// <returns>The minimum amount allowed for that transaction/product</returns>
		decimal GetProductMinimum(string complianceProgramName, TransactionTypes transactionType, MGIContext mgiContext);

	}
}
