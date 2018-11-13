using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Core.Partner.Data.Fees;
using MGI.Common.Util;

namespace MGI.Core.Partner.Contract
{
	public interface IFeeAdjustmentService
	{
		/// <summary>
		/// This method is to get collection of fee adjustment details 
		/// </summary>
		/// <param name="transactionType">This is fee adjustment transaction type</param>
		/// <param name="session">This is customer session</param>
		/// <param name="transactions">This is collection of transaction details</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Fee adjustment details</returns>
		List<FeeAdjustment> GetApplicableAdjustments(FeeAdjustmentTransactionType transactionType, CustomerSession session, List<Transaction> transactions, MGIContext mgiContext);

		/// <summary>
		/// This method is to get collection of fee adjustments by channel partner details
		/// </summary>
		/// <returns>Fee adjustment details</returns> 
		List<FeeAdjustment> Lookup(ChannelPartner channelPartner);

		/// <summary>
		/// This method is to delete the fee adjustments by check transaction id.
		/// </summary>
		/// <param name="checkTransactionId">This is check  transaction id. </param>
		/// <returns></returns> 
		void DeleteFeeAdjustments(Guid checkTransactionId);
	}
}
