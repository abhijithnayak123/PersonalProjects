using System;
using System.Collections.Generic;
using System.Linq;
using TCF.Zeo.Common.Data;
using CorePartner = TCF.Zeo.Core;
using static TCF.Zeo.Common.Util.Helper;
using Core = TCF.Zeo.Core;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.Common.Contract;
using TCF.Zeo.Common.Util;
using CommonData = TCF.Zeo.Common;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using AutoMapper;

namespace TCF.Zeo.Biz.Common.Impl
{
    public class FeeServiceImpl : IFeeService
    {
        #region Dependencies

        CorePartner.Contract.IFeeService feeService = new CorePartner.Impl.ZeoCoreImpl();

        IFeeAdjustmentConditionValidation feeAdjCondition = new FeeAdjustmentConditionValidation();

        CorePartner.Contract.IPricingCluster pricingClusterService = new CorePartner.Impl.ZeoCoreImpl();

        IMapper mapper;
        #endregion

        public FeeServiceImpl()
        {
            #region Mapping

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Core.Data.TransactionFee, TransactionFee>();
            });

            mapper = config.CreateMapper();

            #endregion
        }

        public List<TransactionFee> GetFee(TransactionType transactionType, decimal amount, int checkType, CommonData.Data.ZeoContext context, long transactionId = 0)
        {
            try
            {
                List<TransactionFee> transactionFees = new List<TransactionFee>();

                if (!string.IsNullOrWhiteSpace(context.PromotionCode) && !feeService.ValidatePromoCode(transactionType, context.ChannelPartnerId, context.PromotionCode, context))
                {
                    switch (transactionType)
                    {
                        case TransactionType.MoneyOrder:
                            throw new MoneyOrderException(MoneyOrderException.MONEYORDER_PROMOCODE_INVALID);
                        case TransactionType.ProcessCheck:
                            throw new CheckException(CheckException.PROMOCODE_INVALID);
                        default:
                            return transactionFees;
                    }
                }

                decimal baseFee = CalculateBaseFee(transactionType, amount, context, checkType);

                transactionFees = mapper.Map<List<TransactionFee>>(feeService.GetPromotionEligibility(baseFee, amount, checkType, transactionId, transactionType, context));

                if (string.IsNullOrWhiteSpace(context.PromotionCode))
                {
                    transactionFees.Add(new TransactionFee
                    {
                        BaseFee = baseFee,
                        NetFee = baseFee
                    }); 
                }

                return transactionFees;

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                if (transactionType == TransactionType.MoneyOrder)
                    throw new MoneyOrderException(MoneyOrderException.MONEYORDER_GETFEE_FAILED, ex);
                throw;
            }
        }

        public long ValidateProviderPromotion(TransactionType transactionType, decimal amount, CommonData.Data.ZeoContext context)
        {
            long errorCode = 1;

            // Error code : 1 => Provider promotion is not registered in ZEO
            // Error Code : 0 => Provider promotion is registered in ZEO, and this customer is not eligible for this promotion
            // Else :=> will consider as promotion id

            if (!string.IsNullOrWhiteSpace(context.PromotionCode) && feeService.ValidatePromoCode(transactionType, context.ChannelPartnerId, context.PromotionCode, context))
            {
                List<TransactionFee> transactionFees = mapper.Map<List<TransactionFee>>(feeService.GetPromotionEligibility(0, amount, 0, 0, transactionType, context));
                errorCode = transactionFees.Count > 0 ? transactionFees.First(i => i.PromotionCode == context.PromotionCode).PromotionId : 0;
            }

            return errorCode;
        }

        public TransactionFee ReCalculateFee(TransactionType transactionType, decimal amount, int checkType, CommonData.Data.ZeoContext context, long transactionId = 0)
        {

            decimal baseFee = CalculateBaseFee(transactionType, amount, context, checkType);
            
            List<TransactionFee> transactionFees = mapper.Map<List<TransactionFee>>(feeService.GetPromotionEligibility(baseFee, amount, checkType, transactionId, transactionType, context));
            TransactionFee trxFee = new TransactionFee();
            trxFee.BaseFee = trxFee.NetFee = baseFee;
            trxFee.AdditionalFee = trxFee.DiscountApplied = 0;

            //Temporary Fix : This is to handle the Promotion Hidden, If the applied Promo is still eligible after reclassification use it or else pass the Promocode as '' and get all the 
            // promotions and check whether there are any group promos available.

            //Check if the Applied Promotion is still applicable.
            if (!string.IsNullOrWhiteSpace(context.PromotionCode) && transactionFees.Any(i => i.PromotionCode?.ToLower() == context.PromotionCode?.ToLower()))
                trxFee = transactionFees.Find(i => i.PromotionCode?.ToLower() == context.PromotionCode?.ToLower()); // get the transaction fee if the promotion is eligible for the transaction
            else if (string.IsNullOrWhiteSpace(context.PromotionCode)) //If the Promotion is not applied
            {
                if (transactionFees.Any(x => x.IsGroupPromo == true))
                    trxFee = transactionFees.OrderByDescending(i => i.Priority).FirstOrDefault(x => x.IsGroupPromo == true); //If the transaction fee is not available for specific promotion, apply the promotion based on the priority.
            }
            else //If the promotion is not applicable for the classified check type, check whether any group promos applicable for the transaction based on the priority.
            {
                context.PromotionCode = string.Empty;
                transactionFees = mapper.Map<List<TransactionFee>>(feeService.GetPromotionEligibility(baseFee, amount, checkType, transactionId, transactionType, context));
                if (transactionFees.Any(x => x.IsGroupPromo == true))
                    trxFee = transactionFees.OrderByDescending(i => i.Priority).FirstOrDefault(x => x.IsGroupPromo == true); //If the transaction fee is not available for specific promotion, apply the promotion based on the priority.
            }

            return trxFee;

        }

        public bool CreateOrUpdateTransactionFeeAdjustment(long promotionId, TransactionType transactionType, long transactionId, CommonData.Data.ZeoContext context, long ProvisionId = 0)
        {
            Core.Data.TransactionFeeAdjustment feeAdj = new Core.Data.TransactionFeeAdjustment();
            feeAdj.PromotionId = promotionId;
            feeAdj.IsActive = context.IsParked ? false : (promotionId != 0);      //If we remove/Park the transaction from the shopping cart, If the promotion not applicable for the transaction need to update as IsActive = 0.
            feeAdj.TransactionId = transactionId;
            feeAdj.ProvisionId = ProvisionId;
            feeAdj.TransactionType = transactionType;
            feeAdj.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);
            feeAdj.DTServerLastModified = DateTime.Now;

            Core.Contract.ITrxnFeeAdjustmentService trxFeeAdjustment = new Core.Impl.TrxnFeeAdjustmentService();

            return trxFeeAdjustment.Update(feeAdj, context);

        }


        #region Base Fee calculation ( Pricing clustered)
        private decimal CalculateBaseFee(TransactionType transactionType, decimal amount, CommonData.Data.ZeoContext context, int checkType)
        {
            string productType = checkType == 0 ? null : Convert.ToString(checkType);

            List<Core.Data.Pricing> pricings = pricingClusterService.GetBaseFee(transactionType, productType, context);

            decimal totalFee = decimal.Zero;
            decimal baseFee = decimal.Zero;


            if (pricings.Count > 0)
            {
                foreach (var objPricing in pricings)
                {
                    if (MeetCondition(amount, objPricing))
                    {
                        if (objPricing.IsPercentage) //For percentage  
                        {
                            totalFee = amount * (objPricing.Value / 100);  //percentage value will be in value column   

                            if (totalFee < objPricing.MinimumFee)
                                baseFee = objPricing.MinimumFee;

                            else if (totalFee >= objPricing.MinimumFee && (objPricing.MaximumFee == 0m || totalFee <= objPricing.MaximumFee))
                                baseFee = totalFee;

                            else
                                baseFee = objPricing.MaximumFee;
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

        private bool MeetCondition(decimal amount, Core.Data.Pricing objPricing)
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
                    return amount >= objPricing.MinimumAmount && amount <= objPricing.MaximumAmount;
                default:
                    return true;  // For percentage and Fixed Fee

            }
        }


        #endregion


        //TODO: below code to be removed.
        //private List<FeeAdjustment> GetApplicableAdjustments(TransactionType transactionType, long transactionId, List<FeeAdjustment> allPosibleFeeAdjustments, decimal amount, int checkType, CommonData.Data.ZeoContext context)
        //{
        //    List<FeeAdjustment> feeAdjustments = new List<FeeAdjustment>();

        //    FeeAdjustment feeAdj;

        //    foreach (FeeAdjustment item in allPosibleFeeAdjustments)
        //    {
        //        feeAdj = item;
        //        bool isValid = true;

        //        foreach (var condition in feeAdj.Conditions)
        //        {
        //            if (isValid)
        //            {
        //                switch ((Helper.FeeAdjustmentConditions)condition.ConditionType)
        //                {
        //                    case Helper.FeeAdjustmentConditions.CheckType:
        //                        isValid = feeAdjCondition.CheckTypeCondition(checkType, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
        //                        break;
        //                    case Helper.FeeAdjustmentConditions.Group:
        //                        isValid = feeAdjCondition.GroupCondition((CompareTypes)condition.CompareType, condition.ConditionValue, context);
        //                        break;
        //                    case Helper.FeeAdjustmentConditions.Location:
        //                        isValid = feeAdjCondition.LocationCondition(context.LocationId, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
        //                        break;
        //                    case Helper.FeeAdjustmentConditions.RegistrationDate:
        //                        isValid = feeAdjCondition.RegistrationDateCondition((CompareTypes)condition.CompareType, condition.ConditionValue, context);
        //                        break;
        //                    case Helper.FeeAdjustmentConditions.DaysSinceRegistration:
        //                        isValid = feeAdjCondition.DaysSinceRegistrationCondition((CompareTypes)condition.CompareType, condition.ConditionValue, context);
        //                        break;
        //                    case Helper.FeeAdjustmentConditions.TransactionAmount:
        //                        isValid = feeAdjCondition.TransactionAmountCondition(amount, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
        //                        break;
        //                    case Helper.FeeAdjustmentConditions.TransactionCount:
        //                        isValid = feeAdjCondition.TransactionCountCondition(transactionType, transactionId, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
        //                        break;
        //                    case Helper.FeeAdjustmentConditions.Referral: // US1800 Referral & Referree Promotions
        //                        isValid = feeAdjCondition.ReferralCondition((CompareTypes)condition.CompareType, condition.ConditionValue, context);
        //                        break;
        //                    case Helper.FeeAdjustmentConditions.Code: //US1799 Targeted promotions for check cashing and money order 
        //                        if (!string.IsNullOrWhiteSpace(context.PromotionCode))
        //                            isValid = feeAdjCondition.CodeCondition(context.PromotionCode, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
        //                        else
        //                            isValid = false;
        //                        break;
        //                    case Helper.FeeAdjustmentConditions.Aggregate: // Added for "THREETHENFREE" Promo - May have to revisit the entire promo engine later.
        //                        isValid = feeAdjCondition.AggregateCondition(ref feeAdj, transactionType, transactionId, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
        //                        break;
        //                    case Helper.FeeAdjustmentConditions.CommittedTransactionCount:
        //                        isValid = feeAdjCondition.CommittedTransactionCount(feeAdj, transactionType, transactionId, (CompareTypes)condition.CompareType, condition.ConditionValue, context);
        //                        break;
        //                    default:
        //                        isValid = false;
        //                        break;
        //                }
        //            }
        //            else
        //                break;  //If the 1 first condition pass and the 2nd condition failed no need to check for 3rd condition so we need to break loop.

        //        }
        //        //All the condition pass we need to add feeAdjustment to the feeAdjustment list.
        //        if (isValid)
        //            feeAdjustments.Add(feeAdj);
        //    }

        //    return feeAdjustments;
        //}

        //private void GetCustomerFeeAdjustments(TransactionType transactionType, CommonData.Data.ZeoContext context, ref List<FeeAdjustment> feeAdjustments)
        //{
        //    var customerFeeAdjustments = customerFeeAdjustment.LookupCustomerFeeAdjustments(transactionType, context.CustomerId, context);
        //    List<TransactionFeeAdjustment> transactions = new List<TransactionFeeAdjustment>();
        //    if (customerFeeAdjustments != null && customerFeeAdjustments.Count > 0)
        //    {
        //        transactions = feeService.GetAuthorizedTransaction(transactionType, context.CustomerId, context);
        //        foreach (CorePartner.Data.FeeAdjustment item in allPosibleFeeAdjustments)
        //        {
        //            var customerFeeAdj = customerFeeAdjustments.FindAll(x => x.Id == item.Id).ToList();

        //            var addedFeeAdjustments = transactions.FindAll(X => X.FeeAdjustmentId == item.Id).ToList();

        //            if ((customerFeeAdj.Count() - addedFeeAdjustments.Count()) > 0)
        //            {
        //                feeAdjustments.Add(customerFeeAdj.Find(x => x.Id == item.Id));
        //            }
        //        }
        //    }
        //}

        //private decimal GetFee(decimal amount, List<Pricing> pricings)
        //{
        //    decimal baseFee = 0;
        //    decimal totalFee = 0;

        //    if (pricings.Count > 0)
        //    {
        //        foreach (var objPricing in pricings)
        //        {
        //            if (MeetCondition(amount, objPricing))
        //            {
        //                if (objPricing.IsPercentage) //For percentage  
        //                {
        //                    totalFee = amount * (objPricing.Value / 100);  //percentage value will be in value column   

        //                    if (totalFee > objPricing.MinimumFee)
        //                        baseFee = totalFee;
        //                    else
        //                        baseFee = objPricing.MinimumFee;
        //                }
        //                else
        //                {
        //                    baseFee = objPricing.Value;
        //                }

        //                return decimal.Round(baseFee, 2);
        //            }
        //        }
        //    }

        //    return baseFee;
        //}


        //  #region MaxAdj calculation

        //private TransactionFee CalculateMaxFeeAdjustment(decimal baseFee, string promoCode, List<FeeAdjustment> feeAdjustments)
        //{
        //    TransactionFee transactionFee = new TransactionFee();
        //    FeeAdjustment _DiscountAdjustment = new FeeAdjustment();
        //    FeeAdjustment Adjustments = GetMaxAdjustment(baseFee, feeAdjustments);
        //    //US1799 Targeted promotions for check cashing and money order 
        //    if (!string.IsNullOrWhiteSpace(promoCode))
        //    {
        //        if (feeAdjustments.Exists(x => x.PromotionType != null && x.PromotionType.ToLower() == PromotionType.Code.ToString().ToLower() && !string.IsNullOrWhiteSpace(x.Name) && x.Name.ToLower() == promoCode.ToLower()))
        //            _DiscountAdjustment = feeAdjustments.Find(x => x.PromotionType != null && x.PromotionType.ToLower() == PromotionType.Code.ToString().ToLower() && x.SystemApplied == false && x.Name.ToLower() == promoCode.ToLower());
        //        else
        //        {//US1799 Changes Ends Here
        //            if (Adjustments.AdjustmentAmount < decimal.Zero || Adjustments.AdjustmentRate < decimal.Zero)
        //            {
        //                _DiscountAdjustment = Adjustments;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (Adjustments.AdjustmentAmount <= decimal.Zero || Adjustments.AdjustmentRate < decimal.Zero)
        //        {
        //            _DiscountAdjustment = Adjustments;
        //        }
        //    }

        //    // discount adjustment should be applied against original baseFee, before any surcharges
        //    decimal discountAmt = decimal.Zero;
        //    if (_DiscountAdjustment != null)
        //        discountAmt = _DiscountAdjustment.AdjustmentRate < decimal.Zero ? decimal.Round(baseFee * _DiscountAdjustment.AdjustmentRate, 2) : decimal.Round(_DiscountAdjustment.AdjustmentAmount, 2);

        //    transactionFee.DiscountApplied = baseFee < Math.Abs(discountAmt) ? (baseFee * -1) : discountAmt; // DiscountApplied is always a negative number, so just add negative value in baseFee as (-1)

        //    FeeAdjustment surCharge = feeAdjustments.Find(a => a.AdjustmentAmount > decimal.Zero || a.AdjustmentRate > decimal.Zero);

        //    transactionFee.BaseFee = baseFee;
        //    if (surCharge != null)
        //    {
        //        decimal surchargeAdj = (surCharge.AdjustmentRate > decimal.Zero ? decimal.Round(baseFee * surCharge.AdjustmentRate, 2) : surCharge.AdjustmentAmount);
        //        transactionFee.AdditionalFee = surchargeAdj;
        //        transactionFee.BaseFee += surchargeAdj;
        //    }
        //    //US1799 Targeted promotions for check cashing and money order 
        //    if (_DiscountAdjustment != null)
        //    {
        //        transactionFee.DiscountName = _DiscountAdjustment.Name;
        //        transactionFee.DiscountDescription = _DiscountAdjustment.Description;
        //        transactionFee.IsSystemApplied = _DiscountAdjustment.SystemApplied;
        //        transactionFee.FeeAdjustmentId = _DiscountAdjustment.Id;
        //        transactionFee.IsOverridable = _DiscountAdjustment.IsOverridable;
        //    }
        //    else
        //    {
        //        transactionFee.DiscountName = string.Empty;
        //        transactionFee.DiscountDescription = string.Empty;
        //        transactionFee.IsSystemApplied = true;
        //    }
        //    //US1799 Changes Ends Here
        //    // DiscountApplied is always a negative number, so just add
        //    transactionFee.NetFee = transactionFee.BaseFee + transactionFee.DiscountApplied;

        //    return transactionFee;
        //}

        //private FeeAdjustment GetMaxAdjustment(decimal baseFee, List<FeeAdjustment> allAdjustments)
        //{
        //    FeeAdjustment adjustment = new FeeAdjustment();
        //    decimal discountAmt = decimal.Zero;

        //    List<FeeAdjustment> maxFeeAdjustments = new List<FeeAdjustment>();

        //    foreach (var adj in allAdjustments)
        //    {
        //        if (discountAmt >= (adj.AdjustmentRate < decimal.Zero ? decimal.Round(baseFee * adj.AdjustmentRate, 2) : adj.AdjustmentAmount))
        //        {
        //            discountAmt = adj.AdjustmentRate < decimal.Zero ? decimal.Round(baseFee * adj.AdjustmentRate, 2) : adj.AdjustmentAmount;
        //            maxFeeAdjustments.Add(adj);
        //        }
        //    }

        //    return GetFeeAdjustmentByPriority(maxFeeAdjustments);
        //}

        //private FeeAdjustment GetFeeAdjustmentByPriority(List<FeeAdjustment> adjustments)
        //{
        //    if (adjustments.Count() > 0)
        //        return adjustments.OrderByDescending(i => i.Priority).First();

        //    return new FeeAdjustment();
        //}
        //#endregion
    }
}
