using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Contract;
using MGI.Common.DataAccess.Contract;
using MGI.Core.Partner.Data.Fees;

namespace MGI.Core.Partner.Impl
{
	public class CustomerFeeAdjustmentServiceImpl : ICustomerFeeAdjustmentService
	{

		public IRepository<CustomerFeeAdjustments> CustomerFeeAdjustmentsRepo { private get; set; }

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
		/// Creating Customer Fee Adjustment
		/// </summary>
		/// <param name="customerFeeAdjustment"></param>
		/// <returns></returns>
		public bool Create(Data.Fees.CustomerFeeAdjustments customerFeeAdjustment)
		{
			try
			{
				CustomerFeeAdjustmentsRepo.AddWithFlush(customerFeeAdjustment);

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
		/// Lookup for Customer Fee Adjustment
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="adjustment"></param>
		/// <returns></returns>
		public CustomerFeeAdjustments lookup(long customerId, FeeAdjustment adjustment)
		{
			try
			{
				var customerFeeAdjustments = CustomerFeeAdjustmentsRepo.FilterBy(x => x.CustomerID == customerId && x.feeAdjustment == adjustment && x.IsAvailed == false).FirstOrDefault();
				return customerFeeAdjustments;
			
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee (DMS Promotions Wave 3)
		/// Update for Customer Fee Adjustment
		/// </summary>
		/// <param name="customerFeeAdjustment"></param>
		/// <returns></returns>
		public bool Update(CustomerFeeAdjustments customerFeeAdjustment)
		{
			try
			{
				CustomerFeeAdjustmentsRepo.UpdateWithFlush(customerFeeAdjustment);

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
