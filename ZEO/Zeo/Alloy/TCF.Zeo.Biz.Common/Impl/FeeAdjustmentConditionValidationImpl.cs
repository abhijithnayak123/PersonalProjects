using System;
using System.Collections.Generic;
using System.Linq;
using CorePartner = TCF.Zeo.Core;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Core.Data;
using System.ComponentModel;
using TCF.Zeo.Biz.Common.Contract;
using TCF.Zeo.Core.Impl;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Biz.Common.Impl
{
    public class FeeAdjustmentConditionValidation : IFeeAdjustmentConditionValidation
    {
        CorePartner.Contract.ICustomerService CustomerService = new ZeoCoreImpl();

        #region Verifying all conditions
        /// <summary>
        /// Checking whether there is any feeAdjustment on the customer selected checktype.
        /// If its valid check type, then we are adding to collection of feeAdjustment 
        /// </summary>
        /// <param name="feeAdjustment"></param>
        /// <param name="checkType"></param>
        /// <param name="feeAdjustments"></param>     
        public bool CheckTypeCondition(int checkType, CompareTypes compareType, string conditionValue, ZeoContext context)
        {
            bool isValid = MeetsCondition<int>(checkType, compareType, conditionValue);
            return isValid;
        }

        /// <summary>
        /// Checking for the promocode is valid promocode or not
        /// If its valida promocode, then we are adding to the collection of feeAdjustments
        /// </summary>
        /// <param name="feeAdjustment"></param>
        /// <param name="promoCode"></param>
        /// <param name="feeAdjustments"></param>
        public bool CodeCondition(string promoCode, CompareTypes compareType, string conditionValue, ZeoContext context)
        {
            bool isValid = MeetsStringCondition(promoCode, compareType, conditionValue);
            return isValid;
        }

        public bool GroupCondition(CompareTypes compareType, string conditionValue, ZeoContext context)
        {
            CustomerProfile customer = CustomerService.GetCustomer(context);

            if (customer != null)
            {
                if (MeetsStringCondition(customer.Group1, compareType, conditionValue))
                    return true;
                else if (MeetsStringCondition(customer.Group2, compareType, conditionValue))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checking for the locationid is valid for fee adjustment or not
        /// If its valid locationid, then we are adding to the collection of feeAdjustments
        /// </summary>
        /// <param name="feeAdjustment"></param>
        /// <param name="locationId"></param>
        /// <param name="feeAdjustments"></param>
        public bool LocationCondition(string locationId, CompareTypes compareType, string conditionValue, ZeoContext context)
        {
            bool isValid = MeetsCondition<string>(locationId, compareType, conditionValue);
            return isValid;
        }

        public bool RegistrationDateCondition(CompareTypes compareType, string conditionValue, ZeoContext context)
        {
            var customer = CustomerService.GetCustomer(context);
            if (customer != null)
            {
                bool isValid = MeetsCondition<DateTime>(customer.DTServerCreate, compareType, conditionValue);
                return isValid;
            }
            return false;
        }

        public bool TransactionAmountCondition(decimal amount, CompareTypes compareType, string conditionValue, ZeoContext context)
        {
            bool isValid = MeetsCondition<decimal>(amount, compareType, conditionValue);
            return isValid;
        }

        public bool DaysSinceRegistrationCondition(CompareTypes compareType, string conditionValue, ZeoContext context)
        {
            CustomerProfile customer = CustomerService.GetCustomer(context);
            if (customer != null)
            {
                bool isValid = MeetsCondition<int>((DateTime.Today - customer.DTServerCreate.Date).Days, compareType, conditionValue);
                return isValid;
            }
            return false;
        }

        public bool ReferralCondition(CompareTypes compareType, string conditionValue, ZeoContext context)
        {
            CustomerProfile customer = CustomerService.GetCustomer(context);
            if (customer != null)
            {
                if (!string.IsNullOrEmpty(customer.ReferralCode))
                {
                    return true;
                }
            }
            return false;
        }

        public bool TransactionCountCondition(TransactionType transactionType, long transactionId, CompareTypes compareType, string conditionValue, ZeoContext context)
        {
            int count = 0;
            CorePartner.Contract.IFeeService feeService = new CorePartner.Impl.ZeoCoreImpl();
            count = feeService.GetTransactionCount(transactionType, transactionId, context.CustomerId, context);
            bool isValid = MeetsCondition<int>(count, compareType, conditionValue);
            return isValid;
        }
        #endregion

        #region special method to compare the fee adjustments

        protected bool MeetsCondition<T>(T customerValue, CompareTypes compareType, string conditionValue) where T : IComparable<T>
        {
            if (compareType == CompareTypes.In || compareType == CompareTypes.NotIn)
            {
                string[] strList = GetConditionValues(conditionValue);
                T[] conditionValues = new T[strList.Length];
                int i = 0;
                foreach (string s in strList)
                    conditionValues[i++] = ParseVal<T>(s);

                if (compareType == CompareTypes.In)
                    return conditionValues.Contains(customerValue);
                else
                    return !conditionValues.Contains(customerValue);
            }

            T convertedConditionValue = ParseVal<T>(conditionValue);

            switch (compareType)
            {
                case CompareTypes.Equal:
                    return customerValue.Equals(convertedConditionValue);
                case CompareTypes.NotEqual:
                    return !customerValue.Equals(convertedConditionValue);
                case CompareTypes.GreaterThan:
                    return customerValue.CompareTo(convertedConditionValue) > 0;
                case CompareTypes.LessThan:
                    return customerValue.CompareTo(convertedConditionValue) < 0;
                case CompareTypes.GreaterThanOrEqual:
                    return customerValue.CompareTo(convertedConditionValue) >= 0;
                case CompareTypes.LessThanOrEqual:
                    return customerValue.CompareTo(convertedConditionValue) <= 0;
            }

            return false;
        }

        private static T ParseVal<T>(string input)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                //Cast ConvertFromString(string text) : object to (T)
                return (T)converter.ConvertFromString(input);
            }
            return default(T);
        }

        private string[] GetConditionValues(string conditionValue)
        {
            return conditionValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }

        // special method for Guid since only greater than/less than do not make sense
        // special compare method for strings so that case insensitive compare may be used
        // greater than/less than compares not valid
        protected bool MeetsStringCondition(string customerValue, CompareTypes compareType, string conditionValue)
        {
            if (compareType == CompareTypes.In || compareType == CompareTypes.NotIn)
            {
                string[] conditionValues = GetConditionValues(conditionValue);

                if (compareType == CompareTypes.In)
                    return conditionValues.Contains(customerValue, StringComparer.CurrentCultureIgnoreCase);
                else
                    return !conditionValues.Contains(customerValue, StringComparer.CurrentCultureIgnoreCase);
            }

            if (compareType == CompareTypes.Equal)
                return conditionValue.Equals(customerValue, StringComparison.CurrentCultureIgnoreCase);
            if (compareType == CompareTypes.NotEqual)
                return !conditionValue.Equals(customerValue, StringComparison.CurrentCultureIgnoreCase);

            throw new Exception("Invalid compare type for string fee adjustment conditions");
        }

        public bool AggregateCondition(ref FeeAdjustment feeAdjustment, TransactionType transactionType, long transactionId, CompareTypes compareType, string conditionValue, ZeoContext context)
        {
            CorePartner.Contract.IFeeService feeService = new CorePartner.Impl.ZeoCoreImpl();
            string promoCode = feeAdjustment.Name;
            int trxCount = feeService.GetTransactionsCountByPromoCode(ref promoCode, transactionType, transactionId, feeAdjustment.SystemApplied, feeAdjustment.DTStart, context);
            feeAdjustment.Description = promoCode; // Promo Description is formatted in SP and send back from core layer.
            return MeetsCondition<int>(trxCount, compareType, conditionValue);
        }

        public bool CommittedTransactionCount(FeeAdjustment feeAdjustment, TransactionType transactionType, long transactionId, CompareTypes compareType, string conditionValue, ZeoContext context)
        {
            CorePartner.Contract.IFeeService feeService = new CorePartner.Impl.ZeoCoreImpl();
            int trxCount = feeService.GetCommittedTrxCountByPromoCode(feeAdjustment.Name, transactionType, transactionId, feeAdjustment.SystemApplied, feeAdjustment.DTStart, context);
            return MeetsCondition<int>(trxCount, compareType, conditionValue);
        }



        #endregion

    }
}
