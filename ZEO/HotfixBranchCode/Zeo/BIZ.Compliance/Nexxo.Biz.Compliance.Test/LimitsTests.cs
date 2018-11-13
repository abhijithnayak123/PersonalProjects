using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

using MGI.Biz.Compliance.Contract;
using MGI.Biz.Compliance.Data;
using MGI.Biz.Compliance.Impl;

using MGI.Core.Compliance.Data;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using Spring.Data.Core;

using NUnit.Framework;
using MGI.Common.Util;
using Spring.Testing.NUnit;

namespace MGI.Biz.Compliance.Test
{
	[TestFixture]
	public class LimitsTests : AbstractTransactionalSpringContextTests
	{
		public ILimitService BIZLimitService { set; get; }
		public MGIContext MgiContext { get; set; }

		[SetUp]
		public void Setup()
		{
			MgiContext = new MGIContext() { };
		}

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Biz.Compliance.Test/MGI.Biz.Compliance.Test/BizCompliance.xml" }; }

		}
		[Test]
		public void Should_BasicTxnLimitsTest_Pass()
		{
			long customerSessionId = 1000000007;
			decimal minimunAmount = BIZLimitService.GetProductMinimum("SynovusCompliance", TransactionTypes.MoneyTransfer, MgiContext);
			decimal maximumAmount = BIZLimitService.CalculateTransactionMaximumLimit(customerSessionId, "SynovusCompliance", TransactionTypes.MoneyTransfer, MgiContext);

			Assert.That(maximumAmount, Is.GreaterThan(0));
			Assert.That(minimunAmount, Is.GreaterThan(0));
		}

		#region Synovuscompliance Unit Test Cases

		[Test]
		public void Should_Synovus_MinimumLimits_Pass()
		{
			long customerSessionId = 1000000007;
			decimal minimunAmount = BIZLimitService.GetProductMinimum("SynovusCompliance", TransactionTypes.Cash, MgiContext);
			decimal maximumAmount = BIZLimitService.CalculateTransactionMaximumLimit(customerSessionId, "SynovusCompliance", TransactionTypes.Cash, MgiContext);
			decimal trxAmount = 25.00M;
			Assert.That(trxAmount, Is.GreaterThan(minimunAmount));
		}

		[Test]
		public void Should_Synovus_MinimumLimits_Fail()
		{
			long customerSessionId = 1000000007;
			decimal minimunAmount = BIZLimitService.GetProductMinimum("SynovusCompliance", TransactionTypes.MoneyTransfer, MgiContext);
			decimal maximumAmount = BIZLimitService.CalculateTransactionMaximumLimit(customerSessionId, "SynovusCompliance", TransactionTypes.MoneyTransfer, MgiContext);
			decimal trxAmount = 5.00M;
			Assert.That(trxAmount, Is.GreaterThan(minimunAmount));
		}

		[Test]
		public void Should_Synovus_MaximumLimits_Pass()
		{
			long customerSessionId = 1000000009;
			decimal minimunAmount = BIZLimitService.GetProductMinimum("SynovusCompliance", TransactionTypes.LoadToGPR, MgiContext);
			decimal maximumAmount = BIZLimitService.CalculateTransactionMaximumLimit(customerSessionId, "SynovusCompliance", TransactionTypes.LoadToGPR, MgiContext);
			decimal transactionAmount = 20.00M;
			Assert.That(transactionAmount, Is.GreaterThan(minimunAmount));
		}
		#endregion


		#region TCFcompliance Unit Test Cases

		[Test]
		public void Should_TCF_MinimumLimits_Pass()
		{
			long customerSessionId = 1000000008;
			decimal minimunAmount = BIZLimitService.GetProductMinimum("TCFCompliance", TransactionTypes.Cash, MgiContext);
			decimal maximumAmount = BIZLimitService.CalculateTransactionMaximumLimit(customerSessionId, "TCFCompliance", TransactionTypes.Cash, MgiContext);

			decimal transactionAmount = 20.00M;

			Assert.That(transactionAmount, Is.GreaterThan(minimunAmount));
		}

		[Test]
		public void Should_TCF_MinimumLimits_Fail()
		{
			long customerSessionId = 1000000008;
			decimal minimunAmount = 0.00M;
			decimal maximumAmount = 0.00M;

			minimunAmount = BIZLimitService.GetProductMinimum("TCFCompliance", TransactionTypes.Check, MgiContext);
			maximumAmount = BIZLimitService.CalculateTransactionMaximumLimit(customerSessionId, "TCFCompliance", TransactionTypes.Check, MgiContext);

			decimal transactionAmount = 1.00M;

			Assert.That(transactionAmount, Is.GreaterThan(minimunAmount));
		}

		[Test]
		public void Should_TCF_MaximumLimits_Pass()
		{
			long customerSessionId = 1000000008;
			decimal minimunAmount = BIZLimitService.GetProductMinimum("RedStoneCompliance", TransactionTypes.Check, MgiContext);
			decimal maximumAmount = BIZLimitService.CalculateTransactionMaximumLimit(customerSessionId, "RedStoneCompliance", TransactionTypes.Check, MgiContext);
			decimal transactionAmount = 500.00M;
			Assert.That(transactionAmount, Is.GreaterThan(minimunAmount));
		}
		#endregion

	}
}
