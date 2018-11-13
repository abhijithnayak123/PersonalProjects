using System;
using NUnit.Framework;
using MGI.Biz.CPEngine.Data;
using MGI.Common.Util;
using MGI.Biz.CPEngine.Contract;
using MGI.Unit.Test;
using Moq;
using CXEData = MGI.Core.CXE.Data;
using MGI.Cxn.Check.Data;
using BizCommon = MGI.Biz.Common.Data;
using PTNRData = MGI.Core.Partner.Data;
using PTNRContract = MGI.Core.Partner.Contract;
namespace MGI.Biz.MoneyOrder.Test
{
	[TestFixture]
	public class CPEngineServiceImplTest : BaseClass_Fixture
	{
		public ICPEngineService CPEngineService { get; set; }
		public PTNRContract.ITransactionService<PTNRData.Transactions.Check> PTNRCheckSvc { get; set; }

		private CheckSubmission GetCheckSubmission()
		{
			return new CheckSubmission()
			{
				Amount = 100,
				IsSystemApplied = true,
				CheckType = "check",
				MICR = "o001003ot075911603t182380188280o",
				IssueDate = new DateTime(2015, 01, 01),
				FrontImageTIFF = new byte[100],
				BackImageTIFF = new byte[100],
				FrontImage = new byte[100],
				BackImage = new byte[100],
			};
		}

		[Test]
		public void Can_Submit_CheckProcessing()
		{
			long customerSessionId = 1000000003;
			CheckSubmission checkSubmission = GetCheckSubmission();
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			Check check = CPEngineService.Submit(checkSubmission, customerSessionId, mgiContext);

			Assert.IsNotNull(check);
			CxeCheckSvc.Verify(moq => moq.Create(It.IsAny<CXEData.Transactions.Stage.Check>(), It.IsAny<string>()), Times.Exactly(1));
			CheckProcessor.Verify(moq => moq.Register(It.IsAny<CheckAccount>(), It.IsAny<MGIContext>(), It.IsAny<string>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Check_Status()
		{
			long customerSessionId = 1000000003;
			string checkId = "2000000000";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			Check status = CPEngineService.GetStatus(customerSessionId, checkId, true, mgiContext);

			Assert.IsNotNull(status);
			CheckProcessor.Verify(moq => moq.Status(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
			CheckProcessor.Verify(moq => moq.Get(It.IsAny<long>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_Check_Status_For_Declined()
		{
			long customerSessionId = 1000000003;
			string checkId = "1000000004";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			Check status = CPEngineService.GetStatus(customerSessionId, checkId, true, mgiContext);

			Assert.IsNotNull(status);
			CheckProcessor.Verify(moq => moq.Status(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
			CheckProcessor.Verify(moq => moq.Update(It.IsAny<CheckTrx>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Cancel_Check_Trxn()
		{
			long customerSessionId = 1000000003;
			string checkId = "1000000003";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			bool status = CPEngineService.Cancel(customerSessionId, checkId, mgiContext);

			Assert.True(status);
			CheckProcessor.Verify(moq => moq.Status(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>()), Times.Exactly(1));
			CheckProcessor.Verify(moq=>moq.Cancel(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Update_Status_On_Removal()
		{
			long customerSessionId = 1000000003;
			long checkId = 1000000001;

			CPEngineService.UpdateStatusOnRemoval(customerSessionId, checkId);

			CxeCheckSvc.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_ReSubmit_Check()
		{
			long customerSessionId = 1000000003;
			long checkId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			bool status = CPEngineService.Resubmit(customerSessionId, checkId, mgiContext);

			Assert.True(status);
			CheckProcessor.Verify(moq => moq.Get(It.IsAny<long>()), Times.Exactly(5));
			CxeCheckSvc.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Commit_CheckProcessing()
		{
			long customerSessionId = 1000000003;
			string checkId = "1000000001";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			CPEngineService.Commit(checkId, customerSessionId, mgiContext);

			CheckProcessor.Verify(moq => moq.Status(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
			CheckProcessor.Verify(moq => moq.Commit(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Check_Fee()
		{
			long customerSessionId = 1000000003;
			CheckSubmission checkSubmission = GetCheckSubmission();
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			BizCommon.TransactionFee fee = CPEngineService.GetFee(customerSessionId, checkSubmission, mgiContext);

			Assert.IsNotNull(fee);
		}

		[Test]
		public void Can_Get_Check_Transaction()
		{
			long agentSessionId = 1000000000;
			long customerSessionId = 1000000003;
			string checkId = "1000000001";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			CheckTransaction checkTransaction = CPEngineService.GetTransaction(agentSessionId, customerSessionId, checkId, mgiContext);

			Assert.IsNotNull(checkTransaction);
			CheckProcessor.Verify(moq => moq.Get(It.IsAny<long>()), Times.AtLeastOnce());
			CxeCheckSvc.Verify(moq => moq.GetImages(It.IsAny<long>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_Check_Declined_Trxn()
		{
			long agentSessionId = 1000000000;
			long customerSessionId = 1000000003;
			string checkId = "1000000003";
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			CheckTransaction checkTransaction = CPEngineService.GetTransaction(agentSessionId, customerSessionId, checkId, mgiContext);

			Assert.IsNotNull(checkTransaction);
			CheckProcessor.Verify(moq => moq.Get(It.IsAny<long>()), Times.Exactly(1));
			CheckProcessor.Verify(moq => moq.Update(It.IsAny<CheckTrx>(), It.IsAny<MGIContext>()), Times.Exactly(1));
			CxeCheckSvc.Verify(moq => moq.GetImages(It.IsAny<long>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Check_ProcessorInfo()
		{
			long agentSessionId = 1000000000;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			MGI.Biz.CPEngine.Data.CheckProcessorInfo checkProcessorInfo = CPEngineService.GetCheckProcessorInfo(agentSessionId, mgiContext);

			Assert.IsNotNull(checkProcessorInfo);
			CheckProcessor.Verify(moq => moq.GetCheckProcessorInfo(It.IsAny<string>()), Times.Exactly(1));
		}

		[Test]
		public void Can_UpdateTransaction_Frank()
		{
			long customerSessionId = 1000000003;
			long transactionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			CPEngineService.UpdateTransactionFranked(customerSessionId, transactionId, mgiContext);

			CheckProcessor.Verify(moq => moq.UpdateTransactionFranked(It.IsAny<long>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Chexar_Session()
		{
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 33;

			ChexarLogin chexarLogin = CPEngineService.GetChexarSessions(mgiContext);

			Assert.IsNotNull(chexarLogin);
			CheckProcessor.Verify(moq => moq.GetCheckSessions(It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_Check_FrankingData()
		{
			long customerSessionId = 1000000003;
			long transactionId = 1000000001;
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerName = "Synovus";
			mgiContext.ChannelPartnerRowGuid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			mgiContext.ChannelPartnerId = 34;

			string checkPrint = CPEngineService.GetCheckFrankingData(customerSessionId, transactionId, mgiContext);

			Assert.IsNotNullOrEmpty(checkPrint);
		}

		[Test]
		public void Can_ReSubmit_Check_Transaction()
		{
			Assert.False(CPEngineService.CanResubmit(0, "0", new MGIContext()));
		}

		[Test]
		public void Can_Get_Check_Type()
		{
			Assert.AreNotEqual(CPEngineService.GetCheckTypes(0, new MGIContext()), 0);
		}
	}
}
