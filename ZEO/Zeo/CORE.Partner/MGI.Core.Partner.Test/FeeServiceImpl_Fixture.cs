using System;

using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Data;


using NUnit.Framework;


using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Fees;
using MGI.Core.Partner.Data.Transactions;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Impl;
using MGI.Core.Partner.Test;
using Spring.Data.Core;
using Spring.Data.Generic;

namespace MGI.Core.Partner.Test
{
    [TestFixture]

	public class FeeServiceImpl_Fixture : AbstractPartnerTest
	{
		public FeeServiceImpl PartnerFeeService { private get; set; }

		private ICustomerSessionService _custSessionSvc;
		public ICustomerSessionService CustomerSessionService { set { _custSessionSvc = value; } }

		private IRepository<ChannelPartner> _channelPartnerRepo;
		public IRepository<ChannelPartner> ChannelPartnerRepo { set { _channelPartnerRepo = value; } }

		private IChannelPartnerGroupService _groupSvc;
		public IChannelPartnerGroupService GroupSvc { set { _groupSvc = value; } }

		private long _customerId = 101101;
		private long _acctId = 555;
		private string _agentId = "1234"; //"500001";
		Dictionary<string, object> otherData = new Dictionary<string, object>();
		MGI.Common.Util.MGIContext mgiContext = new Common.Util.MGIContext() { };
		//public class FeeServiceTests
		//{
		//	private FeeServiceImpl PartnerFeeService;


        [TestFixtureSetUp]
        public void Init()
        {
            PartnerFeeService = new FeeServiceImpl();
        }


		[SetUp]
		public void TestSetup()
		{
			
		}

		private CustomerSession SetupSession(int channelPartnerId)
		{
			long a = SessionSetupHelper.SetupAgentSession(AdoTemplate, _agentId, channelPartnerId);
			Guid c = SessionSetupHelper.CreateCustomerAndAccount(AdoTemplate, _customerId, _acctId, channelPartnerId);
			long s = SessionSetupHelper.SetupCustomerSession(AdoTemplate, c, _agentId);

			CustomerSession session = _custSessionSvc.Lookup(s);
			Trace.WriteLine("ChannelPartnerId: " + session.AgentSession.Terminal.ChannelPartner.rowguid);

			return session;
		}

		private ChannelPartnerGroup CreateChannelPartnerGroup(string groupName)
		{
			ChannelPartner cp = _channelPartnerRepo.FindBy(c => c.Name == "Synovus");
			ChannelPartnerGroup g = new ChannelPartnerGroup
			{
				channelPartner = cp,
				Name = groupName,
				DTServerCreate = DateTime.Now
			};

			_groupSvc.Create(g);

			return g;
		}


        [Test]
        public void CheckFeeTestsCentris()
        {
			CustomerSession session = SetupSession(27);
			var transactions = new List<Check>();

			//public TransactionFee GetCheckFee(CustomerSession session, List<Check> transactions, decimal amount, int checkType, Dictionary<string, object> mgiContext)
			
			Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(session, transactions, 0, 1, mgiContext).NetFee);
			Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(session, transactions, 10, 1, mgiContext).NetFee);
			Assert.AreEqual(6m, PartnerFeeService.GetCheckFee(session, transactions, 100, 1, mgiContext).NetFee);
			Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(session, transactions, 10, 2, mgiContext).NetFee);
			Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(session, transactions, 100, 2, mgiContext).NetFee);
			Assert.AreEqual(4m, PartnerFeeService.GetCheckFee(session, transactions, 200, 2, mgiContext).NetFee);
			Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(session, transactions, 10, 6, mgiContext).NetFee);
			Assert.AreEqual(4m, PartnerFeeService.GetCheckFee(session, transactions, 100, 6, mgiContext).NetFee);
			Assert.AreEqual(8m, PartnerFeeService.GetCheckFee(session, transactions, 200, 6, mgiContext).NetFee);
			//public TransactionFee GetCheckFee(CustomerSession session, List<Check> transactions, decimal amount, int checkType, Dictionary<string, object> mgiContext)
			//Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(session, transactions, 27, 0, 1).NetFee);
			//Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(27, 10, 1, mgiContext));
			//Assert.AreEqual(6m, PartnerFeeService.GetCheckFee(27, 100, 1, mgiContext));
			//Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(27, 10, 2, mgiContext));
			//Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(27, 100, 2, mgiContext));
			//Assert.AreEqual(4m, PartnerFeeService.GetCheckFee(27, 200, 2));
			//Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(27, 10, 6));
			//Assert.AreEqual(4m, PartnerFeeService.GetCheckFee(27, 100, 6));
			//Assert.AreEqual(8m, PartnerFeeService.GetCheckFee(27, 200, 6));

			//Assert.AreEqual(1m, PartnerFeeService.GetCheckFee(33, 0, 1));
			//Assert.AreEqual(1m, PartnerFeeService.GetCheckFee(33, 10, 1));
			//Assert.AreEqual(2.9m, PartnerFeeService.GetCheckFee(33, 100, 1));
			//Assert.AreEqual(1m, PartnerFeeService.GetCheckFee(33, 10, 2));
			//Assert.AreEqual(1.5m, PartnerFeeService.GetCheckFee(33, 100, 2));
			//Assert.AreEqual(3m, PartnerFeeService.GetCheckFee(33, 200, 2));
			//Assert.AreEqual(1m, PartnerFeeService.GetCheckFee(33, 10, 6));
			//Assert.AreEqual(2.9m, PartnerFeeService.GetCheckFee(33, 100, 6));
			//Assert.AreEqual(5.8m, PartnerFeeService.GetCheckFee(33, 200, 6));
			//Assert.AreEqual(4.9m, PartnerFeeService.GetCheckFee(33, 100, 5));

        }

		[Test]
		public void CheckFeeTestsSynovus()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(session, transactions, 0, 1, mgiContext).NetFee);
			Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(session, transactions, 10, 1, mgiContext).NetFee);
			Assert.AreEqual(3.9m, PartnerFeeService.GetCheckFee(session, transactions, 100, 1, mgiContext).NetFee);
			Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(session, transactions, 10, 2, mgiContext).NetFee);
			Assert.AreEqual(2.5m, PartnerFeeService.GetCheckFee(session, transactions, 100, 2, mgiContext).NetFee);
			Assert.AreEqual(4m, PartnerFeeService.GetCheckFee(session, transactions, 200, 2, mgiContext).NetFee);
			Assert.AreEqual(2m, PartnerFeeService.GetCheckFee(session, transactions, 10, 6, mgiContext).NetFee);
			Assert.AreEqual(3.9m, PartnerFeeService.GetCheckFee(session, transactions, 100, 6, mgiContext).NetFee);
			Assert.AreEqual(6.8m, PartnerFeeService.GetCheckFee(session, transactions, 200, 6, mgiContext).NetFee);
			Assert.AreEqual(5.9m, PartnerFeeService.GetCheckFee(session, transactions, 100, 5, mgiContext).NetFee);
		}

        [Test]
        [ExpectedException(typeof(TransactionServiceException))]
        public void CheckFeesExceptionBadType()
        {
			var transactions = new List<Check>();
			PartnerFeeService.GetCheckFee(SetupSession(34),transactions, 100, 1000, mgiContext);

			//PartnerFeeService.GetCheckFee(27, 100, 1000);

        }

        [Test]
        public void FundsFeeTestsCentris()
        {

			CustomerSession session = SetupSession(27);
			var transactions = new List<Funds>();

			Assert.AreEqual(2m, PartnerFeeService.GetFundsFee(session, transactions, 20, 0, mgiContext).NetFee);
			Assert.AreEqual(2m, PartnerFeeService.GetFundsFee(session, transactions, 20, 1, mgiContext).NetFee);
			Assert.AreEqual(0m, PartnerFeeService.GetFundsFee(session, transactions, 0, 2, mgiContext).NetFee);
        }

		[Test]
		public void FundsFeeTestsSynovus()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Funds>();

			Assert.AreEqual(0m, PartnerFeeService.GetFundsFee(session, transactions, 20, 0, mgiContext).NetFee);
			Assert.AreEqual(0m, PartnerFeeService.GetFundsFee(session, transactions, 20, 1, mgiContext).NetFee);
			Assert.AreEqual(0m, PartnerFeeService.GetFundsFee(session, transactions, 0, 2, mgiContext).NetFee);
		}

		//	PartnerFeeService.GetCheckFee(1, 100, 1000);
		//}

        [Test]
        public void FundsFeeTests()
        {
			CustomerSession session = SetupSession(33);
			var transactions = new List<Funds>();

			Assert.AreNotEqual(PartnerFeeService.GetFundsFee(session, transactions, 34, 0, mgiContext), 2m);
			Assert.AreNotEqual(PartnerFeeService.GetFundsFee(session, transactions, 27, 1, mgiContext), 2m);
			Assert.AreNotEqual(PartnerFeeService.GetFundsFee(session,transactions, 27, 2,mgiContext), 0m);

			Assert.AreNotEqual(PartnerFeeService.GetFundsFee(session, transactions, 33, 0, mgiContext), 0m);
			Assert.AreNotEqual(PartnerFeeService.GetFundsFee(session, transactions, 33, 1, mgiContext), 0m);
			Assert.AreNotEqual(PartnerFeeService.GetFundsFee(session, transactions, 33, 2, mgiContext), 0m);
        }


        [Test]
        [ExpectedException(typeof(TransactionServiceException))]
        public void FundsFeesExceptionBadType()
        {

			PartnerFeeService.GetFundsFee(SetupSession(27), new List<Funds>(), 20, 4, mgiContext);

			PartnerFeeService.GetFundsFee(SetupSession(27), new List<Funds>(), 27, 4, mgiContext);

        }

        [Test]
        [ExpectedException(typeof(TransactionServiceException))]
        public void FundsFeesExceptionBadPartner()
        {
			
			PartnerFeeService.GetFundsFee(SetupSession(27), new List<Funds>(), 20, 4, mgiContext);

            PartnerFeeService.GetFundsFee(SetupSession(27), new List<Funds>(), 0, 4, mgiContext);

        }

        [Test]
        public void MoneyOrderFeeTests()
        {
			//public TransactionFee GetMoneyOrderFee(CustomerSession session, List<MoneyOrder> transactions, decimal amount, Dictionary<string, object> mgiContext)
			Assert.AreEqual(1, PartnerFeeService.GetMoneyOrderFee(SetupSession(33), new List<MoneyOrder>(), 100, mgiContext).NetFee);

			Assert.AreEqual(1, PartnerFeeService.GetMoneyOrderFee(SetupSession(33), new List<MoneyOrder>(), 0, mgiContext).NetFee);

        }

		[Test]
		public void CheckTypeDiscountRate()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			string adjustmentName = "check type discount";

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, '{2}','{3}',1,-.30,0,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, adjustmentName, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(12, 'check type')");
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', 5, 3,'1,2,3',getdate())", adjustmentPK));

			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(5.06m, f.NetFee);
			Assert.AreEqual(-1.74m, f.DiscountApplied);
			Assert.AreEqual(adjustmentName, f.DiscountName);
		}

		[Test]
		public void CheckTypeSurchargeRate()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, 'check type surcharge','{2}',1,.1,0,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(12, 'check type')");
			//adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditionTypes(Id, Name) values({0}, 'check type')", (int)ConditionTypes.CheckType));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 1,'1',getdate())", adjustmentPK, (int)ConditionTypes.CheckType));

			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(6.8m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);
		}

		[Test]
		public void CheckTypeDiscountAmount()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			string adjustmentName = "check type discount";

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, '{2}','{3}',1,0,-1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, adjustmentName, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(12, 'check type')");
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', 5, 3,'1,2,3',getdate())", adjustmentPK));

			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(5.8m, f.NetFee);
			Assert.AreEqual(-1m, f.DiscountApplied);
			Assert.AreEqual(adjustmentName, f.DiscountName);
		}

		[Test]
		public void CheckTypeSurchargeAmount()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, 'check type surcharge','{2}',1,0,1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(12, 'check type')");
			//adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditionTypes(Id, Name) values({0}, 'check type')", (int)ConditionTypes.CheckType));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 3,'1',getdate())", adjustmentPK, (int)ConditionTypes.CheckType));

			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(6.8m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);
		}

		[Test]
		public void CheckTypeSurchargeAndDiscount()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			string discountName = "check type discount";

			// discount
			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, '{2}','{3}',1,0,-1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, discountName, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(12, 'check type')");
			//adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditionTypes(Id, Name) values({0}, 'check type')", (int)ConditionTypes.CheckType));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 3,'1',getdate())", adjustmentPK, (int)ConditionTypes.CheckType));

			// surcharge
			adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, 'check type surcharge','{2}',1,0,1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 3,'1',getdate())", adjustmentPK, (int)ConditionTypes.CheckType));


			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(5.8m, f.NetFee);
			Assert.AreEqual(-1m, f.DiscountApplied);
			Assert.AreEqual(discountName, f.DiscountName);
		}

		[Test]
		public void CheckTypeApplicableAndNot()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			// discount
			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, 'check type discount','{2}',1,0,-1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(12, 'check type')");
		//	adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditionTypes(Id, Name) values({0}, 'check type')", (int)ConditionTypes.CheckType));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 1,'2',getdate())", adjustmentPK, (int)ConditionTypes.CheckType));

			// surcharge
			adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, 'check type surcharge','{2}',1,0,1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 1,'1',getdate())", adjustmentPK, (int)ConditionTypes.CheckType));

			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(6.8m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);
		}

		[Test]
		public void MultipleConditions()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			string adjustmentName = "check type and amount discount";

			// discount
			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, '{2}','{3}',1,0,-1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, adjustmentName, DateTime.Today, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(12, 'check type')");
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(11, 'Transaction Amount')");
			//adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditionTypes(Id, Name) values({0}, 'Check Type')", (int)ConditionTypes.CheckType));
			//adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditionTypes(Id, Name) values({0}, 'Transaction Amount')", (int)ConditionTypes.TransactionAmount));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 1,'1',getdate())", adjustmentPK, (int)ConditionTypes.CheckType));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 7,'200',getdate())", adjustmentPK, (int)ConditionTypes.TransactionAmount));

			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(5.8m, f.NetFee);
			Assert.AreEqual(-1m, f.DiscountApplied);
			Assert.AreEqual(adjustmentName, f.DiscountName);

			f = PartnerFeeService.GetCheckFee(session, transactions, 199, 1, mgiContext);
			Assert.AreEqual(6.77m, f.BaseFee);
			Assert.AreEqual(6.77m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);

			f = PartnerFeeService.GetCheckFee(session, transactions, 200, 2, mgiContext);
			Assert.AreEqual(4m, f.BaseFee);
			Assert.AreEqual(4m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);
		}

		[Test]
		public void TransactionCountSurcharge()
		{
			CustomerSession session = SetupSession(34);
			var transactions = new List<Check>();

			string adjustmentName = "trans count adjustment";

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, '{2}','{3}',1,0,1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, adjustmentName, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(15, 'check type')");
			//adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditionTypes(Id, Name) values({0}, 'check type')", (int)ConditionTypes.TransactionCount));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 1,'0',getdate())", adjustmentPK, (int)ConditionTypes.TransactionCount));

			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(7m, f.BaseFee);
			Assert.AreEqual(7m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);

			transactions.Add(new Check());

			f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6m, f.BaseFee);
			Assert.AreEqual(6m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);
		}

		[Test]
		public void GroupDiscountAmount()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			ChannelPartnerGroup g = CreateChannelPartnerGroup("Group 1");
			session.Customer.AddtoGroup(g);

			string adjustmentName = "Group discount";

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, '{2}','{3}',1,0,-1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, adjustmentName, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(12, 'group')");
		//	adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditionTypes(Id, Name) values({0}, 'group')", (int)ConditionTypes.Group));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 3,'Group 1,Group 2,Group 3',getdate())", adjustmentPK, (int)ConditionTypes.Group));

			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(5.8m, f.NetFee);
			Assert.AreEqual(-1m, f.DiscountApplied);
			Assert.AreEqual(adjustmentName, f.DiscountName);

			session.Customer.Groups.RemoveAt(0);

			f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(6.8m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);

			// add it back to Group 1
			session.Customer.AddtoGroup(g);

			// add to Group 2
			g = CreateChannelPartnerGroup("Group 2");
			session.Customer.AddtoGroup(g);

			f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(5.8m, f.NetFee);
			Assert.AreEqual(-1m, f.DiscountApplied);
			Assert.AreEqual(adjustmentName, f.DiscountName);

			session.Customer.Groups.RemoveAt(1);

			f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(5.8m, f.NetFee);
			Assert.AreEqual(-1m, f.DiscountApplied);
			Assert.AreEqual(adjustmentName, f.DiscountName);

			session.Customer.Groups.RemoveAt(0);

			f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(6.8m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);
		}

		[Test]
		public void AdjustmentAmountGreaterThanBaseFee()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			string adjustmentName = "check type discount";

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, '{2}','{3}',1,0,-1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, adjustmentName, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(15, 'check type')");
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', 5, 3,'1,2,3',getdate())", adjustmentPK));

			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 20, 1, mgiContext);
			Assert.AreEqual(2m, f.BaseFee);
			Assert.AreEqual(1m, f.NetFee);
			Assert.AreEqual(-1m, f.DiscountApplied);
			Assert.AreEqual(adjustmentName, f.DiscountName);
		}

		[Test]
		public void SurchargeAndDiscountWithSmallFee()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			string discountName = "check type discount";

			// discount
			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, '{2}','{3}',1,0,-1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, discountName, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(15, 'check type')");
			//adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditionTypes(Id, Name) values({0}, 'check type')", (int)ConditionTypes.CheckType));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 1,'1',getdate())", adjustmentPK, (int)ConditionTypes.CheckType));

			// surcharge
			adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, 'check type surcharge','{2}',1,0,1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 1,'1',getdate())", adjustmentPK, (int)ConditionTypes.CheckType));


			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 20, 1, mgiContext);
			Assert.AreEqual(2m, f.BaseFee);
			Assert.AreEqual(1m, f.NetFee);
			Assert.AreEqual(-1m, f.DiscountApplied);
			Assert.AreEqual(discountName, f.DiscountName);
		}

		[Test]
		public void ExpiredAdjustment()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<Check>();

			string adjustmentName = "check type discount";

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, DTEnd, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', 1, '{2}','{3}', '{4}',1,0,-1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, adjustmentName, "1/1/2014", "3/1/2014"));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(12, 'check type')");
			//adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(5, 'check type')");
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', 5, 3,'1,2,3',getdate())", adjustmentPK));

			TransactionFee f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(6.8m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);

			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("update tChannelPartnerFeeAdjustments set DTEnd = getdate() where rowguid = '{0}'", adjustmentPK));
			
			f = PartnerFeeService.GetCheckFee(session, transactions, 200, 1, mgiContext);
			Assert.AreEqual(6.8m, f.BaseFee);
			Assert.AreEqual(5.8m, f.NetFee);
			Assert.AreEqual(-1m, f.DiscountApplied);
			Assert.AreEqual(adjustmentName, f.DiscountName);
		}

		[Test]
		public void MoneyOrderDiscountAmount()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<MoneyOrder>();

			string adjustmentName = "MO discount";

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', {2}, '{3}','{4}',1,0,-.5,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, (int)FeeAdjustmentTransactionType.MoneyOrder, adjustmentName, DateTime.Today));

			TransactionFee f = PartnerFeeService.GetMoneyOrderFee(session, transactions, 200, mgiContext);
			Assert.AreEqual(1m, f.BaseFee);
			Assert.AreEqual(0.5m, f.NetFee);
			Assert.AreEqual(-.5m, f.DiscountApplied);
			Assert.AreEqual(adjustmentName, f.DiscountName);
		}

		[Test]
		public void MoneyOrderSurchargeAmount()
		{
			CustomerSession session = SetupSession(33);
			var transactions = new List<MoneyOrder>();

			string adjustmentName = "MO surcharge";

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', {2}, '{3}','{4}',1,0,1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, (int)FeeAdjustmentTransactionType.MoneyOrder, adjustmentName, DateTime.Today));
			
			TransactionFee f = PartnerFeeService.GetMoneyOrderFee(session, transactions, 200, mgiContext);
			Assert.AreEqual(2m, f.BaseFee);
			Assert.AreEqual(2m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);
		}

				[Test]
		public void FundsDiscountAmount()
		{
			CustomerSession session = SetupSession(27);
			var transactions = new List<Funds>();

			string adjustmentName = "Funds discount";

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', {2}, '{3}','{4}',1,0,-1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, (int)FeeAdjustmentTransactionType.FundsDebit, adjustmentName, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(12, 'Transaction Amount')");
			//adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditionTypes(Id, Name) values({0}, 'Transaction Amount')", (int)ConditionTypes.TransactionAmount));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 7,'100',getdate())", adjustmentPK, (int)ConditionTypes.TransactionAmount));

			TransactionFee f = PartnerFeeService.GetFundsFee(session, transactions, 100, 0, mgiContext);
			Assert.AreEqual(2m, f.BaseFee);
			Assert.AreEqual(1m, f.NetFee);
			Assert.AreEqual(-1m, f.DiscountApplied);
			Assert.AreEqual(adjustmentName, f.DiscountName);

			f = PartnerFeeService.GetFundsFee(session, transactions, 50, 0, mgiContext);
			Assert.AreEqual(2m, f.BaseFee);
			Assert.AreEqual(2m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);
		}

		[Test]
		public void FundsSurchargeAmount()
		{
			CustomerSession session = SetupSession(27);
			var transactions = new List<Funds>();

			string adjustmentName = "Funds surcharge";

			Guid adjustmentPK = Guid.NewGuid();
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tChannelPartnerFeeAdjustments(rowguid, ChannelPartnerPK, TransactionType, Name, DTStart, SystemApplied, adjustmentRate, adjustmentamount,DTCreate) values('{0}','{1}', {2}, '{3}','{4}',1,0,1,getdate())", adjustmentPK, session.AgentSession.Terminal.ChannelPartner.rowguid, (int)FeeAdjustmentTransactionType.FundsCredit, adjustmentName, DateTime.Today));
			adoTemplate.ExecuteNonQuery(CommandType.Text, "insert tFeeAdjustmentConditionTypes(Id, Name) values(12, 'Transaction Amount')");
			//adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditionTypes(Id, Name) values({0}, 'Transaction Amount')", (int)ConditionTypes.TransactionAmount));
			adoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tFeeAdjustmentConditions(rowguid, FeeAdjustmentPK, ConditionType, CompareType, ConditionValue,DTCreate) values(newid(),'{0}', {1}, 7,'100',getdate())", adjustmentPK, (int)ConditionTypes.TransactionAmount));
			
			TransactionFee f = PartnerFeeService.GetFundsFee(session, transactions, 100, 1,mgiContext);
			Assert.AreEqual(3m, f.BaseFee);
			Assert.AreEqual(3m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);

			f = PartnerFeeService.GetFundsFee(session, transactions, 99, 1,mgiContext);
			Assert.AreEqual(2m, f.BaseFee);
			Assert.AreEqual(2m, f.NetFee);
			Assert.AreEqual(0m, f.DiscountApplied);
			Assert.AreEqual(string.Empty, f.DiscountName);
		}
    }
}
