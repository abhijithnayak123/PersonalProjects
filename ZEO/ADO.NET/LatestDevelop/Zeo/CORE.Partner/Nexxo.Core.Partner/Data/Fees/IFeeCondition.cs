using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using MGI.Common.Util;

namespace MGI.Core.Partner.Data.Fees
{
	public abstract class IFeeCondition
	{
		private enum CompareTypes
		{
			Equal = 1,
			NotEqual,
			In,
			NotIn,
			GreaterThan,
			LessThan,
			GreaterThanOrEqual,
			LessThanOrEqual
		}

		private FeeAdjustmentCondition condition;
		public FeeAdjustmentCondition Condition { set { condition = value; } }

		private CompareTypes compareType { get { return (CompareTypes)condition.CompareType; } }

		public abstract bool MeetsCondition(CustomerSession session, List<Transactions.Transaction> transactions, MGIContext mgiContext);

		// generic compare method to use for int, decimal, long, DateTime
		protected bool meetsCondition<T>(T customerValue) where T : IComparable<T>
		{
			if (compareType == CompareTypes.In || compareType == CompareTypes.NotIn)
			{
				string[] strList = getConditionValues();
				T[] conditionValues = new T[strList.Length];
				int i = 0;
				foreach (string s in strList)
					conditionValues[i++] = parseVal<T>(s);

				if (compareType == CompareTypes.In)
					return conditionValues.Contains(customerValue);
				else
					return !conditionValues.Contains(customerValue);
			}

			T conditionValue = parseVal<T>(condition.ConditionValue);

			switch (compareType)
			{
				case CompareTypes.Equal:
					return customerValue.Equals(conditionValue);
				case CompareTypes.NotEqual:
					return !customerValue.Equals(conditionValue);
				case CompareTypes.GreaterThan:
					return customerValue.CompareTo(conditionValue) > 0;
				case CompareTypes.LessThan:
					return customerValue.CompareTo(conditionValue) < 0;
				case CompareTypes.GreaterThanOrEqual:
					return customerValue.CompareTo(conditionValue) >= 0;
				case CompareTypes.LessThanOrEqual:
					return customerValue.CompareTo(conditionValue) <= 0;
			}

			return false;
		}

		// special compare method for bool - only Equal makes sense
		protected bool meetsBoolCondition(bool customerValue)
		{
			bool conditionValue = bool.Parse(condition.ConditionValue);

			if (compareType == CompareTypes.Equal)
				return conditionValue == customerValue;

			throw new Exception("Invalid compare type for boolean fee adjustment conditions");
		}

		// special compare method for strings so that case insensitive compare may be used
		// greater than/less than compares not valid
		protected bool meetsStringCondition(string customerValue)
		{
			if (compareType == CompareTypes.In || compareType == CompareTypes.NotIn)
			{
				string[] conditionValues = getConditionValues();

				if (compareType == CompareTypes.In)
					return conditionValues.Contains(customerValue, StringComparer.CurrentCultureIgnoreCase);
				else
					return !conditionValues.Contains(customerValue, StringComparer.CurrentCultureIgnoreCase);
			}

			if (compareType == CompareTypes.Equal)
				return condition.ConditionValue.Equals(customerValue, StringComparison.CurrentCultureIgnoreCase);
			if (compareType == CompareTypes.NotEqual)
				return !condition.ConditionValue.Equals(customerValue, StringComparison.CurrentCultureIgnoreCase);

			throw new Exception("Invalid compare type for string fee adjustment conditions");
		}

		// special method for Guid since only greater than/less than do not make sense
		protected bool meetsGuidCondition(Guid customerValue)
		{
			if (compareType == CompareTypes.In || compareType == CompareTypes.NotIn)
			{
				string[] strList = getConditionValues();
				Guid[] conditionValues = new Guid[strList.Length];
				int i = 0;
				foreach (string s in strList)
					conditionValues[i++] = new Guid(s);

				if (compareType == CompareTypes.In)
					return conditionValues.Contains(customerValue);
				else
					return !conditionValues.Contains(customerValue);
			}

			Guid conditionValue = new Guid(condition.ConditionValue);

			if (compareType == CompareTypes.Equal)
				return customerValue == conditionValue;
			else
				return customerValue != conditionValue;

			throw new Exception("Invalid compare type for Guid fee adjustment conditions");
		}

		private static T parseVal<T>(string input)
		{
			var converter = TypeDescriptor.GetConverter(typeof(T));
			if (converter != null)
			{
				//Cast ConvertFromString(string text) : object to (T)
				return (T)converter.ConvertFromString(input);
			}
			return default(T);
		}

		private string[] getConditionValues()
		{
			return condition.ConditionValue.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
