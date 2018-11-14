using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data.Fees
{
	public enum ConditionTypes
	{
		Group = 1,
		Location = 2,
		TransactionAmount = 3,
		TransactionCount = 4,
		CheckType = 5,
		RegistrationDate = 6,
		DaysSinceRegistration = 7,
		Referral = 8, //US1800 Referral promotions – Free check cashing to referrer and referee 
		Code = 9 //US1799 Targeted promotions for check cashing and money order
	}
}
