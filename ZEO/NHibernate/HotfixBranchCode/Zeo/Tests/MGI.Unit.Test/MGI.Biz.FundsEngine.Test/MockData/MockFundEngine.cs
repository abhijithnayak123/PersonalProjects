using CXEContract = MGI.Core.CXE.Contract;
using CXNContract = MGI.Cxn.Fund.Contract;
using CXEData = MGI.Core.CXE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using MGI.Unit.Test.MockClasses;
using AutoMapper;
using MGI.Cxn.Fund.Contract;
using CXNData = MGI.Cxn.Fund.Data;
using MGI.Common.Util;

namespace MGI.Unit.Test.MGI.Biz.FundsEngine.Test.MockData
{
	public class MockFundEngine : IntializMoqObject
	{
		public MockFundEngine()
		{
			Mapper.CreateMap<CXEData.Transactions.Stage.Funds, CXEData.Transactions.Commit.Funds>();
		}

		#region Core CXE Fund Service Fake Object
		public CXEContract.IFundsService CreateInstanceFundService()
		{
			CXEFundsService = _moqRepository.Create<CXEContract.IFundsService>();

			CXEFundsService.Setup(moq => moq.Commit(It.IsAny<long>(), It.IsAny<string>())).Callback(
				(long id, string timeZone) =>
				{
					var existingFundTrxn = stageFunds.Find(a => a.Id == id);
					if (existingFundTrxn != null)
					{
						existingFundTrxn.Status = (int)CXEData.TransactionStates.Committed;
						var commitFund = Mapper.Map<CXEData.Transactions.Stage.Funds, CXEData.Transactions.Commit.Funds>(existingFundTrxn);
						commitFunds.Add(commitFund);
					}
				});

			CXEFundsService.Setup(moq => moq.Create(It.IsAny<CXEData.Transactions.Stage.Funds>())).Returns(
				(CXEData.Transactions.Stage.Funds stageFund) =>
				{
					stageFund.Id = 1000000000 + (stageFunds.Count() + 1);
					stageFund.rowguid = Guid.NewGuid();
					stageFunds.Add(stageFund);
					return stageFund.Id;
				});

			CXEFundsService.Setup(moq => moq.Get(It.IsAny<long>())).Returns(
				(long id) =>
				{
					return commitFunds.Find(a => a.Id == id);
				});

			CXEFundsService.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<CXEData.TransactionStates>(), It.IsAny<string>())).Callback(
				(long id, CXEData.TransactionStates states, string timeZone) =>
				{
					var existingFundTrxn = stageFunds.Find(a => a.Id == id);
					if (existingFundTrxn != null)
					{
						stageFunds.Remove(existingFundTrxn);
						existingFundTrxn.Status = (int)states;
						stageFunds.Add(existingFundTrxn);
					}
				});

			CXEFundsService.Setup(moq => moq.UpdateAmount(It.IsAny<long>(), It.IsAny<decimal>(), It.IsAny<string>())).Callback(
				(long id, decimal amount, string timeZone) =>
				{
					var existingFundTrxn = stageFunds.Find(a => a.Id == id);
					if (existingFundTrxn != null)
					{
						stageFunds.Remove(existingFundTrxn);
						existingFundTrxn.Amount = amount;
						stageFunds.Add(existingFundTrxn);
					}
				});

			return CXEFundsService.Object;
		}
		#endregion

		#region CXE Contract Fund Processor Fake Object
		public IFundProcessor CreateInstanceOfFundProcessor()
		{
			ProcessorRouter = _moqRepository.Create<IFundProcessor>();

			CXNData.ProcessorResult processorResult = new CXNData.ProcessorResult() { IsSuccess = true };

			ProcessorRouter.Setup(moq => moq.Activate(It.IsAny<long>(), It.IsAny<CXNData.FundRequest>(), It.IsAny<MGIContext>(), out processorResult)).Returns(1000000000);

			ProcessorRouter.Setup(moq => moq.Authenticate(It.IsAny<string>(), It.IsAny<MGIContext>(), out processorResult)).Returns(1000000000);

			ProcessorRouter.Setup(moq => moq.Cancel(It.IsAny<long>(), It.IsAny<MGIContext>()));

			ProcessorRouter.Setup(moq => moq.CloseAccount(It.IsAny<long>(), It.IsAny<MGIContext>())).Returns(true);

			ProcessorRouter.Setup(moq => moq.Commit(It.IsAny<long>(), It.IsAny<MGIContext>(), out processorResult, It.IsAny<string>()));

			ProcessorRouter.Setup(moq => moq.Get(It.IsAny<long>(), It.IsAny<MGIContext>())).Returns(fundTrxns.FirstOrDefault());

			ProcessorRouter.Setup(moq => moq.GetBalance(It.IsAny<long>(), It.IsAny<MGIContext>(), out processorResult)).Returns(new CXNData.CardInfo());

			ProcessorRouter.Setup(moq => moq.GetPanForCardNumber(It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(
				(string cardNumber, MGIContext mgiContext) => 
				{
					long cxeAccount = 10000000000000000;
					if (cardNumber == "4756756000186663" || cardNumber == "4756756000186664")
					{
						cxeAccount = 0;
					}
					return cxeAccount;
				});

			ProcessorRouter.Setup(moq => moq.GetTransactionHistory(It.IsAny<long>(), It.IsAny<CXNData.TransactionHistoryRequest>(), It.IsAny<MGIContext>())).Returns(new List<CXNData.TransactionHistory>());

			ProcessorRouter.Setup(moq => moq.Load(It.IsAny<long>(), It.IsAny<CXNData.FundRequest>(), It.IsAny<MGIContext>(), out processorResult)).Returns(1000000000);

			ProcessorRouter.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(new CXNData.CardAccount());

			ProcessorRouter.Setup(moq => moq.LookupCardAccount(It.IsAny<long>(), It.IsAny<bool>())).Returns(new CXNData.CardAccount());

			ProcessorRouter.Setup(moq => moq.Register(It.IsAny<CXNData.CardAccount>(), It.IsAny<MGIContext>(), out processorResult)).Returns(1000000000);

			ProcessorRouter.Setup(moq => moq.ReplaceCard(It.IsAny<long>(), It.IsAny<CXNData.CardMaintenanceInfo>(), It.IsAny<MGIContext>())).Returns(true);

			ProcessorRouter.Setup(moq => moq.UpdateAccount(It.IsAny<CXNData.CardAccount>(), It.IsAny<MGIContext>())).Returns(1000000000);

			ProcessorRouter.Setup(moq => moq.UpdateAmount(It.IsAny<long>(), It.IsAny<CXNData.FundRequest>(), It.IsAny<string>())).Returns(1000000000);

			ProcessorRouter.Setup(moq => moq.UpdateCardStatus(It.IsAny<long>(), It.IsAny<CXNData.CardMaintenanceInfo>(), It.IsAny<MGIContext>())).Returns(true);

			ProcessorRouter.Setup(moq => moq.UpdateRegistrationDetails(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>()));

			ProcessorRouter.Setup(moq => moq.UpdateRegistrationDetails(It.IsAny<CXNData.CardAccount>(), It.IsAny<MGIContext>()));

			ProcessorRouter.Setup(moq => moq.Withdraw(It.IsAny<long>(), It.IsAny<CXNData.FundRequest>(), It.IsAny<MGIContext>(), out processorResult)).Returns(1000000000);

			ProcessorRouter.Setup(moq => moq.GetShippingTypes(It.IsAny<long>())).Returns(lstshippingTypes);

			ProcessorRouter.Setup(moq => moq.GetShippingFee(It.IsAny<CXNData.CardMaintenanceInfo>(), It.IsAny<MGIContext>())).Returns(double.MaxValue);

			ProcessorRouter.Setup(moq => moq.AssociateCard(It.IsAny<CXNData.CardAccount>(), It.IsAny<MGIContext>(), It.IsAny<bool>())).Returns(
				(CXNData.CardAccount cardAccount, MGIContext mgiContext, bool isNewCard) =>
				{
					long cxnAccountId = 1000000000;
					if (cardAccount.CardNumber == "4756756000186664")
					{
						cxnAccountId = 0;
					}
				return cxnAccountId;
				});
			ProcessorRouter.Setup(moq => moq.GetFundFee(It.IsAny<CXNData.CardMaintenanceInfo>(), It.IsAny<MGIContext>())).Returns(double.MaxValue);
			return ProcessorRouter.Object;
		}
		#endregion
	}
}
