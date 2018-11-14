using System;
using System.Collections.Generic;

using MGI.Core.Partner.Data;

namespace MGI.Core.Partner.Contract
{
	public interface IComplianceTransactionService
	{
		/// <summary>
		/// This method is to get the collection of compliance transaction by customer id
		/// </summary>
		/// <param name="customerId">This is customer id of compliance transaction</param>
		/// <returns>Collection of compliance transaction</returns>
		List<ComplianceTransaction> Get(long customerId);

		/// <summary>
		/// This method is to get the collection of compliance transaction by customer id and recipient id
		/// </summary>
		/// <param name="customerId">This is customer id of compliance transaction</param>
		/// <param name="xRecipientId">This is recipient id of compliance transaction</param>
		/// <returns>Collection of compliance transaction</returns>
		List<ComplianceTransaction> Get(long customerId, long xRecipientId);

		/// <summary>
		/// This method is to get the collection of compliance transaction by customer id, product id and account number
		/// </summary>
		/// <param name="customerId">This is customer id of compliance transaction</param>
		/// <param name="ProductId">This is product id of compliance transaction</param>
		/// <param name="AccountNumber">This is account number of compliance transaction</param>
		/// <returns>Collection of compliance transaction</returns>
		List<ComplianceTransaction> Get(long customerId, long ProductId, string AccountNumber);

	}
}