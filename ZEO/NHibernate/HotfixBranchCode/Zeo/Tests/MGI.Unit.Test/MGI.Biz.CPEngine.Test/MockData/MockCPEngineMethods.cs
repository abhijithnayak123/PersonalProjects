using System.Linq;
using CXEContract = MGI.Core.CXE.Contract;
using CXEData = MGI.Core.CXE.Data;
using Moq;
using AutoMapper;
using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;
using MGI.Cxn.Check.Contract;
using MGI.Common.Util;
using MGI.Cxn.Check.Data;
using MGI.Unit.Test.MockClasses;

namespace MGI.Unit.Test
{
	public class MockCPEngineMethods : IntializMoqObject
	{
		public MockCPEngineMethods() : base() 
		{
			Mapper.CreateMap<CXEData.Transactions.Stage.Check, CXEData.Transactions.Commit.Check>();
		}

		#region Core CXE Check Service
		public CXEContract.ICheckService CreateInstanceOfCheckService()
		{
			CxeCheckSvc = _moqRepository.Create<CXEContract.ICheckService>();

			CxeCheckSvc.Setup(moq => moq.Commit(It.IsAny<long>())).Callback(
				(long id) =>
				{
					var existingCheckTrxn = stageChecks.Find(a => a.Id == id);
					var checkCommit = Mapper.Map<CXEData.Transactions.Stage.Check, CXEData.Transactions.Commit.Check>(existingCheckTrxn);
					checkCommit.Id = 1000000001 + (commitChecks.Count() + 1);
					checkCommit.Status = (int)CXEData.TransactionStates.Committed;
					commitChecks.Add(checkCommit);
				});

			CxeCheckSvc.Setup(moq => moq.Create(It.IsAny<CXEData.Transactions.Stage.Check>(), It.IsAny<string>())).Returns(
				(CXEData.Transactions.Stage.Check check, string timeZone) =>
				{
					check.Id = 1000000001 + (stageChecks.Count() + 1);
					check.Status = 2;
					stageChecks.Add(check);
					return check.Id;
				});

			CxeCheckSvc.Setup(moq => moq.Get(It.IsAny<long>())).Returns(
				(long id) =>
				{
					return commitChecks.Find(a => a.Id == id);
				});

			CxeCheckSvc.Setup(moq => moq.GetImages(It.IsAny<long>())).Returns(
				(long id) =>
				{
					var existingCheckTrxn = stageChecks.Find(a => a.Id == id);
					return existingCheckTrxn.Images;
				});

			CxeCheckSvc.Setup(moq => moq.GetStatus(It.IsAny<long>())).Returns(
				(long id) =>
				{
					var existingCheckTrxn = stageChecks.Find(a => a.Id == id);
					return existingCheckTrxn.Status;
				});

			CxeCheckSvc.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>())).Callback(
				(long id, int newStatus, string timeZone) =>
				{
					var existingCheckTrxn = stageChecks.Find(a => a.Id == id);
					if (existingCheckTrxn != null)
					{
						stageChecks.Remove(existingCheckTrxn);
						existingCheckTrxn.Status = newStatus;
						stageChecks.Add(existingCheckTrxn);
					}
				});

			CxeCheckSvc.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>())).Callback(
				(long id, int newStatus, decimal fee, string timeZone) =>
				{
					var existingCheckTrxn = stageChecks.Find(a => a.Id == id);
					if (existingCheckTrxn != null)
					{
						stageChecks.Remove(existingCheckTrxn);
						existingCheckTrxn.Status = newStatus;
						existingCheckTrxn.Fee = fee;
						stageChecks.Add(existingCheckTrxn);
					}
				});

			return CxeCheckSvc.Object;
		} 
		#endregion

		#region Core Message Store
		public PTNRContract.IMessageStore CreateInstanceOfMessageStore()
		{
			Mock<PTNRContract.IMessageStore> MessageStore = _moqRepository.Create<PTNRContract.IMessageStore>();

            MessageStore.Setup(moq => moq.Add(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<PTNRData.Language>(), new PTNRData.Message(), It.IsAny<string>())).Returns(long.MaxValue);

			MessageStore.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(new PTNRData.Message() { Id = 1000000000 });

			MessageStore.Setup(moq => moq.Lookup(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<PTNRData.Language>())).Returns(new PTNRData.Message() { Id = 1000000000 });

			return MessageStore.Object;
		} 
		#endregion

		#region CXN Check processor Service
		public ICheckProcessor CreateInstanceOfProcessorRouter()
		{
			CheckProcessor = _moqRepository.Create<ICheckProcessor>();

			CheckProcessor.Setup(moq => moq.Cancel(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(
				(long trxId, string timeZone, MGIContext mgiContext) =>
				{
					var existingCheckTrxn = checkTrxns.Find(a => a.Id == trxId);
					if (existingCheckTrxn != null)
					{
						checkTrxns.Remove(existingCheckTrxn);
						existingCheckTrxn.Status = CheckStatus.Canceled;
						checkTrxns.Add(existingCheckTrxn);
					}
					return true;
				});

			CheckProcessor.Setup(moq => moq.Commit(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>())).Callback(
				(long trxId, string timeZone, MGIContext mgiContext) =>
				{
					var existingCheckTrxn = checkTrxns.Find(a => a.Id == trxId);
					if (existingCheckTrxn != null)
					{
						checkTrxns.Remove(existingCheckTrxn);
						existingCheckTrxn.Status = CheckStatus.Approved;
						checkTrxns.Add(existingCheckTrxn);
					}

				});

			CheckProcessor.Setup(moq => moq.Get(It.IsAny<long>())).Returns(
				(long trxId) =>
				{
					var existTrxn = checkTrxns.Find(a => a.Id == trxId);
					if (existTrxn != null)
					{
						return existTrxn;
					}
					else
					{
						return checkTrxns.FirstOrDefault();
					}
				});

			CheckProcessor.Setup(moq => moq.GetAccount(It.IsAny<long>())).Returns(
				(long accountId) =>
				{
					var exist = checkAccounts.Find(a => a.Id == accountId);
					if (exist != null)
					{
						return exist;
					}
					else
					{
						return checkAccounts.FirstOrDefault();
					}
				});

			CheckProcessor.Setup(moq => moq.GetCheckProcessorInfo(It.IsAny<string>())).Returns(new CheckProcessorInfo());

			CheckProcessor.Setup(moq => moq.GetCheckSessions(It.IsAny<MGIContext>())).Returns(
				(MGIContext mgiContext) =>
				{
					return checkLogins.FirstOrDefault();
				});

			CheckProcessor.Setup(moq => moq.PendingChecks()).Returns(checkTrxns);

			CheckProcessor.Setup(moq => moq.Register(It.IsAny<CheckAccount>(), It.IsAny<MGIContext>(), It.IsAny<string>())).Returns(100000000);

			CheckProcessor.Setup(moq => moq.Status(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(
				(long trxId, string timeZone, MGIContext mgiContext) =>
				{
					return checkTrxns.Find(a => a.Id == trxId).Status;
				});

			CheckProcessor.Setup(moq => moq.Submit(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CheckInfo>(), It.IsAny<MGIContext>())).Returns(
				(long trxId, long accountId, CheckInfo check, MGIContext mgiContext) =>
				{
					CheckTrx checkTrxn = new CheckTrx()
					{
						Amount = check.Amount,
						Id = trxId,
						Status = CheckStatus.Approved
					};
					checkTrxns.Add(checkTrxn);
					return checkTrxn.Status;
				});

			CheckProcessor.Setup(moq => moq.Update(It.IsAny<CheckAccount>(), It.IsAny<MGIContext>())).Callback(
				(CheckAccount checkAccount, MGIContext mgiContext) =>
				{
					var existCheckAccount = checkAccounts.Find(a => a.Id == checkAccount.Id);
					if (existCheckAccount != null)
					{
						checkAccounts.Remove(existCheckAccount);
						existCheckAccount = checkAccount;
						checkAccounts.Add(existCheckAccount);
					}
				});

			CheckProcessor.Setup(moq => moq.Update(It.IsAny<CheckTrx>(), It.IsAny<MGIContext>())).Callback(
				(CheckTrx checkTrxn, MGIContext mgiContext) =>
				{
					var existCheckTrxn = checkTrxns.Find(a => a.Id == checkTrxn.Id);
					if (existCheckTrxn != null)
					{
						checkTrxns.Remove(existCheckTrxn);
						existCheckTrxn = checkTrxn;
						existCheckTrxn.Status = CheckStatus.Approved;
						checkTrxns.Add(existCheckTrxn);
					}
				});

			CheckProcessor.Setup(moq => moq.UpdateTransactionFranked(It.IsAny<long>())).Callback(
				(long trxId) =>
				{
					var existCheckTrxn = checkTrxns.Find(a => a.Id == trxId);
					if (existCheckTrxn != null)
					{
						checkTrxns.Remove(existCheckTrxn);
						existCheckTrxn.IsCheckFranked = true;
						checkTrxns.Add(existCheckTrxn);
					}
				});

			return CheckProcessor.Object;
		} 
		#endregion
	}
}
