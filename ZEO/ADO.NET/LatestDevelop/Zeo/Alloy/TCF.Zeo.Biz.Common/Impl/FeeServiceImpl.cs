using System;
using System.Collections.Generic;
using System.Linq;
using TCF.Zeo.Common.Data;
using CorePartner = TCF.Zeo.Core;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Core.Data;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.Common.Contract;
using TCF.Zeo.Common.Util;
using CommonData = TCF.Zeo.Common;
using TCF.Zeo.Biz.Common.Data.Exceptions;

namespace TCF.Zeo.Biz.Common.Impl
{
    public class FeeServiceImpl : IFeeService
    {
        #region Dependencies
        CorePartner.Contract.IFeeService feeService = new CorePartner.Impl.ZeoCoreImpl();

        CorePartner.Contract.ICustomerFeeAdjustmentService customerFeeAdjustment = new CorePartner.Impl.ZeoCoreImpl();

        IFeeAdjustmentConditionValidation feeAdjCondition = new FeeAdjustmentConditionValidation();

        CorePartner.Contract.IPricingCluster pricingClusterService = new CorePartner.Impl.ZeoCoreImpl();

        List<CorePartner.Data.FeeAdjustment> allPosibleFeeAdjustments = new List<FeeAdjustment>();

        #endregion

        public TransactionFee GetFee(TransactionType transactionType, decimal amount, int checkType, CommonData.Data.ZeoContext context, long transactionId = 0)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(context.PromotionCode))
                {
                    if (!feeService.ValidatePromoCode(transactionType, context.ChannelPartnerId, context.PromotionCode, context))
                    {
                        if (transactionType == TransactionType.MoneyOrder)
                            throw new MoneyOrderException(MoneyOrderException.MONEYORDER_PROMOCODE_INVALID);
                        else if (transactionType == TransactionType.ProcessCheck)
                            throw new CheckException(CheckException.PROMOCODE_INVALID);
                    }
                }
                TransactionFee transactionFee = new TransactionFee();

                string prodctType = checkType == 0 ? null : Convert.ToString(checkType);

                decimal baseFee = decimal.Zero;

                List<Pricing> pricings = pricingClusterService.GetBaseFee(transactionType, context.ChannelPartnerId, Convert.ToInt64(context.LocationId), prodctType, context);

                baseFee = GetFee(amount, pricings);

                if (context.IsParked && context.IsSystemApplied)
                {
                    return new TransactionFee()
                    {
                        BaseFee = baseFee,
                        NetFee = baseFee,
                        DiscountApplied = 0,
                        AdditionalFee = 0,
                        DiscountName = string.Empty,
                        DiscountDescription = string.Empty
                    };
                }

                List<CorePartner.Data.FeeAdjustment> feeAdjustments = new List<CorePartner.Data.FeeAdjustment>();
                ///Getting all posible feeAdjustments based on transaction type and channelpartner
                allPosibleFeeAdjustments = feeService.GetChannelPartnerFeeAdj(transactionType, context.ChannelPartnerId, context);

                feeAdjustments = GetApplicableAdjustments(transactionType, transactionId, allPosibleFeeAdjustments, amount, checkType, context);
                //For Now the Referral Promotions is not applicable for TCF When  
                //GetCustomerFeeAdjustments(transactionType, context, ref feeAdjustments);

                transactionFee = CalculateMaxFeeAdjustment(baseFee, context.PromotionCode, feeAdjustments);

                return transactionFee;

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                if (transactionType == TransactionType.MoneyOrder)
                    throw new MoneyOrderException(MoneyOrderException.MONEYORDER_GETFEE_FAILED, ex);
                throw;
            }
        }

        private List<FeeAdjustment> GetApplicableAdjustments(TransactionType transactionType, long transactionId, List<FeeAdjustment> allPosibleFeeAdjustments, decimal amount, int checkType, CommonData.Data.ZeoContext context)
        {
            List<FeeAdjustment> feeAdjustments = new List<FeeAdjustment>();

            FeeAdjustment feeAdj;

            foreach (FeeAdjustment item in allPosibleFeeAdjustments)
            {
                feeAdj = item;
                bool isValid = true;

                foreach (var condition in feeAdj.Conditions)
                {
                    if (isValid)
                    {
                        switch ((Helper.FeeAdjustmentConditions)condition.ConditionType)
                        {
                            case Helper.FeeAdjustmentConditions.CheckType:
                                isValid = feeAdjCondition.CheckTypeCondition(checkType, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
                                break;
                            case Helper.FeeAdjustmentConditions.Group:
                                isValid = feeAdjCondition.GroupCondition((CompareTypes)condition.CompareType, condition.ConditionValue, context);
                                break;
                            case Helper.FeeAdjustmentConditions.Location:
                                isValid = feeAdjCondition.LocationCondition(context.LocationId, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
                                break;
                            case Helper.FeeAdjustmentConditions.RegistrationDate:
                                isValid = feeAdjCondition.RegistrationDateCondition((CompareTypes)condition.CompareType, condition.ConditionValue, context);
                                break;
                            case Helper.FeeAdjustmentConditions.DaysSinceRegistration:
                                isValid = feeAdjCondition.DaysSinceRegistrationCondition((CompareTypes)condition.CompareType, condition.ConditionValue, context);
                                break;
                            case Helper.FeeAdjustmentConditions.TransactionAmount:
                                isValid = feeAdjCondition.TransactionAmountCondition(amount, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
                                break;
                            case Helper.FeeAdjustmentConditions.TransactionCount:
                                isValid = feeAdjCondition.TransactionCountCondition(transactionType, transactionId, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
                                break;
                            case Helper.FeeAdjustmentConditions.Referral: // US1800 Referral & Referree Promotions
                                isValid = feeAdjCondition.ReferralCondition((CompareTypes)condition.CompareType, condition.ConditionValue, context);
                                break;
                            case Helper.FeeAdjustmentConditions.Code: //US1799 Targeted promotions for check cashing and money order 
                                if (!string.IsNullOrWhiteSpace(context.PromotionCode))
                                    isValid = feeAdjCondition.CodeCondition(context.PromotionCode, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
                                else
                                    isValid = false;
                                break;
                            case Helper.FeeAdjustmentConditions.Aggregate: // Added for "THREETHENFREE" Promo - May have to revisit the entire promo engine later.
                                isValid = feeAdjCondition.AggregateCondition(ref feeAdj, transactionType, transactionId, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
                                break;
                            default:
                                isValid = false;
                                break;
                        }
                    }
                    else
                        break;  //If the 1 first condition pass and the 2nd condition failed no need to check for 3rd condition so we need to break loop.

                }
                //All the condition pass we need to add feeAdjustment to the feeAdjustment list.
                if (isValid)
                    feeAdjustments.Add(feeAdj);
            }

            return feeAdjustments;
        }

        private void GetCustomerFeeAdjustments(TransactionType transactionType, CommonData.Data.ZeoContext context, ref List<FeeAdjustment> feeAdjustments)
        {
            var customerFeeAdjustments = customerFeeAdjustment.LookupCustomerFeeAdjustments(transactionType, context.CustomerId, context);
            List<TransactionFeeAdjustment> transactions = new List<TransactionFeeAdjustment>();
            if (customerFeeAdjustments != null && customerFeeAdjustments.Count > 0)
            {
                transactions = feeService.GetAuthorizedTransaction(transactionType, context.CustomerId, context);
                foreach (CorePartner.Data.FeeAdjustment item in allPosibleFeeAdjustments)
                {
                    var customerFeeAdj = customerFeeAdjustments.FindAll(x => x.Id == item.Id).ToList();

                    var addedFeeAdjustments = transactions.FindAll(X => X.FeeAdjustmentId == item.Id).ToList();

                    if ((customerFeeAdj.Count() - addedFeeAdjustments.Count()) > 0)
                    {
                        feeAdjustments.Add(customerFeeAdj.Find(x => x.Id == item.Id));
                    }
                }
            }
        }

        private decimal GetFee(decimal amount, List<Pricing> pricings)
        {
            decimal baseFee = 0;
            decimal totalFee = 0;

            if (pricings.Count > 0)
            {
                foreach (var objPricing in pricings)
                {
                    if (MeetCondition(amount, objPricing))
                    {
                        if (objPricing.IsPercentage) //For percentage  
                        {
                            totalFee = amount * (objPricing.Value / 100);  //percentage value will be in value column   

                            if (totalFee > objPricing.MinimumFee)
                                baseFee = totalFee;
                            else
                                baseFee = objPricing.MinimumFee;
                        }
                        else
                        {
                            baseFee = objPricing.Value;
                        }

                        return decimal.Round(baseFee, 2);
                    }
                }
            }

            return baseFee;
        }

        private bool MeetCondition(decimal amount, Pricing objPricing)
        {
            switch (objPricing.CompareType)
            {
                case 5:
                    return amount > objPricing.MinimumAmount;
                case 6:
                    return amount < objPricing.MaximumAmount;
                case 7:
                    return amount >= objPricing.MinimumAmount;
                case 8:
                    return amount <= objPricing.MaximumAmount;
                case 9:
                    return amount >= objPricing.MinimumAmount && amount < objPricing.MaximumAmount;
                default:
                    return true;  // For percentage and Fixed Fee

            }
        }

        #region MaxAdj calculation

        private TransactionFee CalculateMaxFeeAdjustment(decimal baseFee, string promoCode, List<FeeAdjustment> feeAdjustments)
        {
            TransactionFee transactionFee = new TransactionFee();
            FeeAdjustment _DiscountAdjustment = new FeeAdjustment();
            FeeAdjustment Adjustments = GetMaxAdjustment(baseFee, feeAdjustments);
            //US1799 Targeted promotions for check cashing and money order 
            if (!string.IsNullOrWhiteSpace(promoCode))
            {
                if (feeAdjustments.Exists(x => x.PromotionType != null && x.PromotionType.ToLower() == PromotionType.Code.ToString().ToLower() && !string.IsNullOrWhiteSpace(x.Name) && x.Name.ToLower() == promoCode.ToLower()))
                    _DiscountAdjustment = feeAdjustments.Find(x => x.PromotionType != null && x.PromotionType.ToLower() == PromotionType.Code.ToString().ToLower() && x.SystemApplied == false && x.Name.ToLower() == promoCode.ToLower());
                else
                {//US1799 Changes Ends Here
                    if (Adjustments.AdjustmentAmount < decimal.Zero || Adjustments.AdjustmentRate < decimal.Zero)
                    {
                        _DiscountAdjustment = Adjustments;
                    }
                }
            }
            else
            {
                if (Adjustments.AdjustmentAmount <= decimal.Zero || Adjustments.AdjustmentRate < decimal.Zero)
                {
                    _DiscountAdjustment = Adjustments;
                }
            }

            // discount adjustment should be applied against original baseFee, before any surcharges
            decimal discountAmt = decimal.Zero;
            if (_DiscountAdjustment != null)
                discountAmt = _DiscountAdjustment.AdjustmentRate < decimal.Zero ? decimal.Round(baseFee * _DiscountAdjustment.AdjustmentRate, 2) : decimal.Round(_DiscountAdjustment.AdjustmentAmount, 2);

            transactionFee.DiscountApplied = baseFee < Math.Abs(discountAmt) ? (baseFee * -1) : discountAmt; // DiscountApplied is always a negative number, so just add negative value in baseFee as (-1)

            FeeAdjustment surCharge = feeAdjustments.Find(a => a.AdjustmentAmount > decimal.Zero || a.AdjustmentRate > decimal.Zero);

            transactionFee.BaseFee = baseFee;
            if (surCharge != null)
            {
                decimal surchargeAdj = (surCharge.AdjustmentRate > decimal.Zero ? decimal.Round(baseFee * surCharge.AdjustmentRate, 2) : surCharge.AdjustmentAmount);
                transactionFee.AdditionalFee = surchargeAdj;
                transactionFee.BaseFee += surchargeAdj;
            }
            //US1799 Targeted promotions for check cashing and money order 
            if (_DiscountAdjustment != null)
            {
                transactionFee.DiscountName = _DiscountAdjustment.Name;
                transactionFee.DiscountDescription = _DiscountAdjustment.Description;
                transactionFee.IsSystemApplied = _DiscountAdjustment.SystemApplied;
                transactionFee.FeeAdjustmentId = _DiscountAdjustment.Id;
            }
            else
            {
                transactionFee.DiscountName = string.Empty;
                transactionFee.DiscountDescription = string.Empty;
                transactionFee.IsSystemApplied = true;
            }
            //US1799 Changes Ends Here
            // DiscountApplied is always a negative number, so just add
            transactionFee.NetFee = transactionFee.BaseFee + transactionFee.DiscountApplied;

            return transactionFee;
        }

        private FeeAdjustment GetMaxAdjustment(decimal baseFee, List<FeeAdjustment> allAdjustments)
        {
            FeeAdjustment adjustment = new FeeAdjustment();
            decimal discountAmt = decimal.Zero;

            foreach (var adj in allAdjustments)
            {
                if (discountAmt >= (adj.AdjustmentRate < decimal.Zero ? decimal.Round(baseFee * adj.AdjustmentRate, 2) : adj.AdjustmentAmount))
                {
                    discountAmt = adj.AdjustmentRate < decimal.Zero ? decimal.Round(baseFee * adj.AdjustmentRate, 2) : adj.AdjustmentAmount;
                    adjustment = adj;
                }
            }

            return adjustment;
        }
        #endregion
    }
}
