using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Core.Partner.Data.Fees;


namespace MGI.Core.Partner.Contract
{
	public interface ICustomerFeeAdjustmentService
	{

		/// <summary>
		/// This method is to create the customer fee adjustments
		/// </summary>
		/// <param name="customerFeeAdjustment">This is customer fee adjustment details</param>
		/// <returns>Boolean value which is the customer is created or not</returns>
		bool Create(CustomerFeeAdjustments customerFeeAdjustment);

		/// <summary>
		/// This method is to update the customer fee adjustments
		/// </summary>
		/// <param name="customerFeeAdjustment">This is customer fee adjustment details</param>
		/// <returns>Boolean value which is the customer is updated or not</returns>
		bool Update(CustomerFeeAdjustments customerFeeAdjustment);

		/// <summary>
		/// This method is to update the customer fee adjustments
		/// </summary>
		/// <param name="customerId">This is customer id</param>
		/// <param name="adjustment">This is fee adjustment details</param>
		/// <returns>Customer fee adjustments details</returns>
		CustomerFeeAdjustments lookup(long customerId, FeeAdjustment adjustment);

	}
}
