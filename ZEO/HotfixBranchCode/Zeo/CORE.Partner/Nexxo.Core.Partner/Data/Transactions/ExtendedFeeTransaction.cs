using System;
using System.Collections.Generic;
using System.Linq;

using MGI.Common.DataAccess.Data;

using MGI.Core.Partner.Data.Fees;

namespace MGI.Core.Partner.Data.Transactions
{
	public abstract class ExtendedFeeTransaction : Transaction
	{
		public virtual decimal BaseFee { get; set; }
		public virtual decimal DiscountApplied { get; set; }
		public virtual decimal AdditionalFee { get; set; }
		public virtual IList<TransactionFeeAdjustment> FeeAdjustments { get; set; }
		//US1799 Targeted promotions for check cashing and money order 
		public virtual bool IsSystemApplied { get; set; }
		public virtual string DiscountName
		{
			//US1799 Targeted promotions for check cashing and money order 
			get;
			set;
			//get
			//{
			//	TransactionFeeAdjustment discount = null;

			//	if (FeeAdjustments.Any(x => x.feeAdjustment.PromotionType != null && x.feeAdjustment.PromotionType.ToLower() == PromotionType.Code.ToString().ToLower()))
			//		discount = FeeAdjustments.FirstOrDefault(x => x.feeAdjustment.PromotionType != null && x.feeAdjustment.PromotionType.ToLower() == PromotionType.Code.ToString().ToLower() && x.feeAdjustment.SystemApplied == false);
			//	else
			//		discount = FeeAdjustments.FirstOrDefault(a => a.feeAdjustment.AdjustmentAmount < decimal.Zero || a.feeAdjustment.AdjustmentRate < decimal.Zero);

			//	return (discount != null) ? discount.feeAdjustment.Name : string.Empty;
			//}
			//set
			//{

			//}

		}
		public virtual string DiscountDescription
		{
			//US1799 Targeted promotions for check cashing and money order 
			get;
			set;
			//get
			//{
			//	TransactionFeeAdjustment discount = null;

			//	if (FeeAdjustments.Any(x => x.feeAdjustment.PromotionType != null && x.feeAdjustment.PromotionType.ToLower() == PromotionType.Code.ToString().ToLower()))
			//		discount = FeeAdjustments.FirstOrDefault(x => x.feeAdjustment.PromotionType != null && x.feeAdjustment.PromotionType.ToLower() == PromotionType.Code.ToString().ToLower() && x.feeAdjustment.SystemApplied == false);
			//	else
			//		discount = FeeAdjustments.FirstOrDefault(a => a.feeAdjustment.AdjustmentAmount < decimal.Zero || a.feeAdjustment.AdjustmentRate < decimal.Zero);

			//	return (discount != null) ? discount.feeAdjustment.Description : string.Empty;
			//}	
			//set
			//{

			//}

		}
		public ExtendedFeeTransaction()
		{
			FeeAdjustments = new List<TransactionFeeAdjustment>();
		}

		public ExtendedFeeTransaction(decimal Amount, TransactionFee fee, string Description,
			long CXEId, int CXEState, long CXNId, int CXNState,
			Account Account, CustomerSession CustomerSession)
			: base(Amount, fee.NetFee, Description, CXEId, CXEState, CXNId, CXNState, Account, CustomerSession)
		{
			this.BaseFee = fee.BaseFee;
			this.DiscountApplied = fee.DiscountApplied;
			//US1799 Targeted promotions for check cashing and money order 
			this.DiscountName = fee.DiscountName;
			this.DiscountDescription = fee.DiscountDescription;
			//US1799 Changes ends here
			FeeAdjustments = new List<TransactionFeeAdjustment>();
			AddFeeAdjustments(fee.Adjustments);
		}

		public ExtendedFeeTransaction(decimal Amount, TransactionFee fee, string Description,
			long CXEId, int CXEState, long CXNId, int CXNState,
			Account Account, CustomerSession CustomerSession, string ConfirmationNumber)
			: base(Amount, fee.NetFee, Description, CXEId, CXEState, CXNId, CXNState, Account, CustomerSession, ConfirmationNumber)
		{
			this.BaseFee = fee.BaseFee;
			this.DiscountApplied = fee.DiscountApplied;
			//US1799 Targeted promotions for check cashing and money order 
			this.DiscountName = fee.DiscountName;
			this.DiscountDescription = fee.DiscountDescription;
			//US1799 Changes Ends Here
			FeeAdjustments = new List<TransactionFeeAdjustment>();
			AddFeeAdjustments(fee.Adjustments);
		}

		public virtual void AddFeeAdjustments(List<FeeAdjustment> adjustments)
		{
			foreach (FeeAdjustment a in adjustments)
			{
				TransactionFeeAdjustment f = new TransactionFeeAdjustment
				{
					// AL-591: IsActive Status in tTxn_FeeAdjustments, will let us know the current applied Feeadjustment. this issue occured as in ODS Report we found duplicate transactions and the reason was tTxn_FeeAdjustments table having duplicate records
					// Developed by: Sunil Shetty || 03/07/2015
					feeAdjustment = a,
					transaction = this,
					DTServerCreate = DateTime.Now,
					IsActive = true
				};
				FeeAdjustments.Add(f);
			}
		}
	}
}
