using CoreComplianceContract = MGI.Core.Compliance.Contract;
using CoreComplianceData = MGI.Core.Compliance.Data;
using CorePartnerContract = MGI.Core.Partner.Contract;
using CorePartnerData = MGI.Core.Partner.Data;
using Moq;
using System.Collections.Generic;
using MGI.Core.Compliance.Data;
using MGI.Unit.Test.MockClasses;
using MGI.Biz.Compliance.Data;
using System;
namespace MGI.Unit.Test
{
	public class MockCompliance : IntializMoqObject
	{
		#region Core Partner Compliance Transaction Service
		public CorePartnerContract.IComplianceTransactionService CreateInstanceOfComplianceTransaction()
		{
			List<CorePartnerData.ComplianceTransaction> transactions = new List<CorePartnerData.ComplianceTransaction>() 
			{
				new CorePartnerData.ComplianceTransaction(){ Amount = 100, DTServerCreate = DateTime.Now, CustomerId = 1000000000, DTTerminalCreate = DateTime.Now, Fee = 5, Id = 1000000000}
			};

			Mock<CorePartnerContract.IComplianceTransactionService> ComplianceTransactionService = _moqRepository.Create<CorePartnerContract.IComplianceTransactionService>();

			ComplianceTransactionService.Setup(moq => moq.Get(It.IsAny<long>())).Returns(
				(long a) =>
				{
					return transactions;
				});


			ComplianceTransactionService.Setup(moq => moq.Get(It.IsAny<long>(), It.IsAny<long>())).Returns(
				(long a, long f) =>
				{
					return transactions;
				});

			ComplianceTransactionService.Setup(moq => moq.Get(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>())).Returns(
				(long a, long c, string some) =>
				{
					return transactions;
				});

			return ComplianceTransactionService.Object;
		} 
		#endregion

		#region Core Compliance Program Service
		public CoreComplianceContract.IComplianceProgramService CreateInstanceOfComplianceProgramService()
		{
			CoreComplianceData.ComplianceProgram program = new CoreComplianceData.ComplianceProgram()
			{
				Name = "TCFCompliance",
				Id = It.IsAny<long>(),
				LimitTypes = new List<LimitType>()
				{
					new LimitType() {Id = It.IsAny<long>(), Name = TransactionTypes.Check.ToString() , 
						Limits = new List<Limit>() { new Limit() { Id = It.IsAny<long>(), PerTransactionMaximum = 20000, PerTransactionMinimum = 10, RollingLimits = "1:100;2:200;"}}},
					new LimitType() {Id = It.IsAny<long>(), Name = TransactionTypes.ActivateGPR.ToString(), 
						Limits = new List<Limit>() { new Limit() { Id = It.IsAny<long>(), PerTransactionMaximum = 30000, PerTransactionMinimum = 20, RollingLimits = "1:100;2:200;"}}},
					new LimitType() {Id = It.IsAny<long>(), Name = TransactionTypes.BillPay.ToString(), 
						Limits = new List<Limit>() { new Limit() { Id = It.IsAny<long>(), PerTransactionMaximum = 30000, PerTransactionMinimum = 20, RollingLimits = "1:100;2:200;"}}},
					new LimitType() {Id = It.IsAny<long>(), Name = TransactionTypes.CashWithdrawal.ToString(), 
						Limits = new List<Limit>() { new Limit() { Id = It.IsAny<long>(), PerTransactionMaximum = 30000, PerTransactionMinimum = 20, RollingLimits = "1:100;2:200;"}}},
					new LimitType() {Id = It.IsAny<long>(), Name = TransactionTypes.DebitGPR.ToString(), 
						Limits = new List<Limit>() { new Limit() { Id = It.IsAny<long>(), PerTransactionMaximum = 30000, PerTransactionMinimum = 20, RollingLimits = "1:100;2:200;"}}},
					new LimitType() {Id = It.IsAny<long>(), Name = TransactionTypes.Funds.ToString(), 
						Limits = new List<Limit>() { new Limit() { Id = It.IsAny<long>(), PerTransactionMaximum = 30000, PerTransactionMinimum = 20, RollingLimits = "1:100;2:200;"}}},
					new LimitType() {Id = It.IsAny<long>(), Name = TransactionTypes.LoadToGPR.ToString(), 
						Limits = new List<Limit>() { new Limit() { Id = It.IsAny<long>(), PerTransactionMaximum = 30000, PerTransactionMinimum = 20, RollingLimits = "1:100;2:200;"}}},
					new LimitType() {Id = It.IsAny<long>(), Name = TransactionTypes.MoneyOrder.ToString(), 
						Limits = new List<Limit>() { new Limit() { Id = It.IsAny<long>(), PerTransactionMaximum = 30000, PerTransactionMinimum = 20, RollingLimits = "1:100;2:200;"}}},
					new LimitType() {Id = It.IsAny<long>(), Name = TransactionTypes.MoneyTransfer.ToString(), 
						Limits = new List<Limit>() { new Limit() { Id = It.IsAny<long>(), PerTransactionMaximum = 30000, PerTransactionMinimum = 20, RollingLimits = "1:100;2:200;"}}},
					new LimitType() {Id = It.IsAny<long>(), Name = TransactionTypes.Cash.ToString(), 
						Limits = new List<Limit>() { new Limit() { Id = It.IsAny<long>(), PerTransactionMaximum = 30000, PerTransactionMinimum = 20, RollingLimits = "1:100;2:200;"}}},
				}

			};

			Mock<CoreComplianceContract.IComplianceProgramService> CoreComplianceProgramService = _moqRepository.Create<CoreComplianceContract.IComplianceProgramService>();

			CoreComplianceProgramService.Setup(moq => moq.Get(It.IsAny<string>())).Returns(
				(string complianceProgramName) =>
				{
					if (complianceProgramName == "TestCompliance")
					{
						return null;
					}
					return program;
				});

			return CoreComplianceProgramService.Object;
		} 
		#endregion
	}
}
