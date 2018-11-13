using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data.Transactions;
using MGI.Core.Partner.Data;
using MGI.Common.Util;

namespace MGI.Core.Partner.Contract
{
	public interface IFeeService
	{

		/// <summary>
		/// This method is to get check fee details
		/// </summary>
		/// <param name="session">This is customer session</param>
		/// <param name="transactions">This is collection of checks details</param>
		/// <param name="amount">This is check amount</param>
		/// <param name="checkType">This is check type</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>		
		/// <returns>Transaction fee details</returns>
		TransactionFee GetCheckFee(CustomerSession session, List<Check> transactions, decimal amount, int checkType, MGIContext mgiContext);

		/// <summary>
		/// This method is to get fund fee details
		/// </summary>
		/// <param name="session">This is customer session</param>
		/// <param name="transactions">This is collection of funds details</param>
		/// <param name="fundsType">This is fund type</param>		
		/// <param name="amount">This is fund amount</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Transaction fee details</returns>
		TransactionFee GetFundsFee(CustomerSession session, List<Funds> transactions, decimal amount, int fundsType, MGIContext mgiContext);

		/// <summary>
		/// This method is to get money order fee details
		/// </summary>
		/// <param name="session">This is customer session</param>
		/// <param name="transactions">This is collection of money order details</param>		
		/// <param name="amount">This is money order amount</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Transaction fee details</returns>
		TransactionFee GetMoneyOrderFee(CustomerSession session, List<MoneyOrder> transactions, decimal amount, MGIContext mgiContext);

		/// <summary>
		/// This method is to get bill pay fee
		/// </summary>
		/// <param name="providerName">This is provider name</param>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>This bill pay fee</returns>
		decimal GetBillPayFee(string providerName, long channelPartnerId, MGIContext mgiContext);

	}
}
