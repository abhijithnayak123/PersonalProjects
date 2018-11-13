using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data.Fees;

namespace MGI.Core.Partner.Data
{
	public class TransactionFee
	{
		public decimal BaseFee { get; private set; }
		public decimal DiscountApplied { get; private set; }
		public decimal AdditionalFee { get; private set; }
		public decimal NetFee { get; set; }
		public List<FeeAdjustment> Adjustments { get; private set; }
		//US1799 Targeted promotions for check cashing and money order 
		public bool IsSystemApplied { set; get; }

		private FeeAdjustment _DiscountAdjustment;

		public string DiscountName
		{
			get
			{
				return (_DiscountAdjustment != null) ? _DiscountAdjustment.Name : string.Empty;
			}
			set { }//US1799 Targeted promotions for check cashing and money order 
		}

		public string DiscountDescription
		{
			get
			{
				return (_DiscountAdjustment != null) ? _DiscountAdjustment.Description : string.Empty;
			}
			set {  } //US1799 Targeted promotions for check cashing and money order 
		}

		public TransactionFee(decimal baseFee, List<FeeAdjustment> adjustments)
		{
			Adjustments = getMaxAdjustment(baseFee, adjustments);
			//US1799 Targeted promotions for check cashing and money order 
			if (adjustments.Exists(x => x.PromotionType != null && x.PromotionType.ToLower() ==  PromotionType.Code.ToString().ToLower()))
				_DiscountAdjustment = adjustments.Find(x => x.PromotionType != null && x.PromotionType.ToLower() == PromotionType.Code.ToString().ToLower() && x.SystemApplied == false);
			else //US1799 Changes Ends Here
				_DiscountAdjustment = Adjustments.Find(a => a.AdjustmentAmount < decimal.Zero || a.AdjustmentRate < decimal.Zero);

			// discount adjustment should be applied against original baseFee, before any surcharges
			decimal discountAmt = decimal.Zero;
			if (_DiscountAdjustment != null)
                discountAmt = _DiscountAdjustment.AdjustmentRate < decimal.Zero ? decimal.Round(baseFee * _DiscountAdjustment.AdjustmentRate, 2) : decimal.Round(_DiscountAdjustment.AdjustmentAmount, 2);

			DiscountApplied = baseFee < Math.Abs(discountAmt) ? (baseFee * -1): discountAmt; // DiscountApplied is always a negative number, so just add negative value in baseFee as (-1)

			FeeAdjustment surCharge = adjustments.Find(a => a.AdjustmentAmount > decimal.Zero || a.AdjustmentRate > decimal.Zero);

			BaseFee = baseFee;
			if (surCharge != null)
			{
				decimal surchargeAdj = (surCharge.AdjustmentRate > decimal.Zero ? decimal.Round(baseFee * surCharge.AdjustmentRate, 2) : surCharge.AdjustmentAmount);
				AdditionalFee = surchargeAdj;
				BaseFee += surchargeAdj;
			}
			//US1799 Targeted promotions for check cashing and money order 
			if (_DiscountAdjustment != null)
			{
				DiscountName = _DiscountAdjustment.Name;
				DiscountDescription = _DiscountAdjustment.Description;
				IsSystemApplied = _DiscountAdjustment.SystemApplied;
			}
			else
			{
				DiscountName = string.Empty;
				DiscountDescription = string.Empty;
				IsSystemApplied = true;
			}
			//US1799 Changes Ends Here
			// DiscountApplied is always a negative number, so just add
			NetFee = BaseFee + DiscountApplied;
		}

		private List<FeeAdjustment> getMaxAdjustment(decimal baseFee, List<FeeAdjustment> allAdjustments)
		{
			List<FeeAdjustment> adjustments = new List<FeeAdjustment>();
			FeeAdjustment adjustment = null;
			decimal discountAmt = decimal.Zero;

			foreach (var adj in allAdjustments)
			{
				if (discountAmt > (adj.AdjustmentRate < decimal.Zero ? decimal.Round(baseFee * adj.AdjustmentRate, 2) : adj.AdjustmentAmount))
				{
					discountAmt = adj.AdjustmentRate < decimal.Zero ? decimal.Round(baseFee * adj.AdjustmentRate, 2) : adj.AdjustmentAmount;
					adjustment = adj;
				}
			}
			if(adjustment != null)
				adjustments.Add(adjustment);
			return adjustments;
		}
	}
}
