using System;
using System.Collections.Generic;
using System.Linq;

using MGI.Core.Compliance.Contract;
using MGI.Core.Compliance.Data;
using MGI.Core.Compliance.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Data;

using NUnit.Framework;
using MGI.Common.Util;

namespace MGI.Core.Compliance.Test
{
	[TestFixture]
	public class ComplianceProgramDBTests : AbstractComplianceDBTest
	{
		private IComplianceProgramService _complianceProgramService;
		public IComplianceProgramService ComplianceProgramService { set { _complianceProgramService = value; } }


		[Test]
		public void Should_ComplianceProgram_NotNull()
		{
			ComplianceProgram complianceProgram = _complianceProgramService.Get("SynovusCompliance");

			Assert.That(complianceProgram, Is.Not.Null);
		}

		[Test]
		public void Should_ComplianceProgram_Null()
		{
			ComplianceProgram complianceProgram = _complianceProgramService.Get("TestCompliance");

			Assert.That(complianceProgram, Is.Null);
		}

		[Test]
		public void Should_Pass_MGIBillPay_MinimumTest()
		{
			ComplianceProgram complianceProgram = _complianceProgramService.Get("MGICompliance");

			decimal limit = complianceProgram.LimitTypes.FirstOrDefault(x => x.Name == "BillPay").Limits.Select(x => x.PerTransactionMinimum).FirstOrDefault().GetValueOrDefault();
			Assert.That(limit, Is.GreaterThanOrEqualTo(20.00));
		}

		[Test]
		public void Should_Pass_CarverSendMoney_PerXMaxTest()
		{
			ComplianceProgram complianceProgram = _complianceProgramService.Get("CarverCompliance");

			decimal perXMaxLimit = complianceProgram.LimitTypes.FirstOrDefault(x => x.Name == "MoneyTransfer").Limits.Select(x => x.PerTransactionMaximum).FirstOrDefault().GetValueOrDefault();

			Assert.That(perXMaxLimit, Is.LessThanOrEqualTo(5000.00));
		}


		[Test]
		public void Should_RedStone_Have_RollingLimitsTest()
		{
			ComplianceProgram prog = _complianceProgramService.Get("RedstoneCompliance");

			string rollingLimits = prog.LimitTypes.FirstOrDefault(x => x.Name == "ProcessCheck").Limits.Select(x => x.RollingLimits).FirstOrDefault();

			Assert.That(rollingLimits, Is.Not.Null);
		}

		[Test]
		public void Should_RedStone_Get_NdaysLimitsTest()
		{
			ComplianceProgram prog = _complianceProgramService.Get("RedstoneCompliance");

			Dictionary<int, decimal> nDaysLimits = prog.LimitTypes.FirstOrDefault(x => x.Name == "ProcessCheck").Limits.Select(x => x.NDaysLimit).FirstOrDefault();

			Assert.That(nDaysLimits, Is.Not.Null);
		}

		[Test]
		public void Should_Null_Centris_Limits()
		{
			ComplianceProgram prog = _complianceProgramService.Get("CentrisTest");

			string rollingLimits = prog.LimitTypes.FirstOrDefault(x => x.Name == "ProcessCheck").Limits.Select(x => x.RollingLimits).FirstOrDefault();

			Assert.That(rollingLimits, Is.Null);
		}
	}
}
