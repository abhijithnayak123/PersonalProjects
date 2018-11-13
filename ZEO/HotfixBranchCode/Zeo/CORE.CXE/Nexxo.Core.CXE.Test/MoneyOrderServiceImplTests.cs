using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.CXE.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
using Moq;
using Spring.Testing.NUnit;
using Account = MGI.Core.CXE.Data.Account;
using MoneyOrderStage = MGI.Core.CXE.Data.Transactions.Stage.MoneyOrder;


namespace MGI.Core.CXE.MoneyOrderServiceImplTests
{

    [TestFixture]
    public class MoneyOrderServiceImplTests : AbstractTransactionalDbProviderSpringContextTests
    {
        public IMoneyOrderService CXEMoneyOrderService { get; set; }
        public IAccountService CXEAccountService { get; set; }

        private ICustomerService _custSvc;
        public ICustomerService CustomerService { set { _custSvc = value; } }

        //Todo :select any id from [CXE].[dbo].[tCustomers] table as customerId
		private long customerId = 1000000000003880;

        //Todo :select any id from [CXE].[dbo].[tCustomerAccounts] table as accountId
		private long accountId = 1000001018;
        //int newMONumber = 1234567;
        private long MOId;
        private decimal Amount = 100m;
        private decimal Fee = 2m;
        //private long customerId = 1088;
        //private Guid accountPK;
        private string timeZone;
        [TestFixtureSetUp]
        public void fixtSetup()
        {
            timeZone = "Pacific Standard Time";
            //setup a MoneyOrder account to use
            //Guid customerPK = Guid.NewGuid();
            //accountPK = Guid.NewGuid();
            //AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tCustomers(rowguid,id,DTCreate) values('{0}',{1},getdate())", customerPK, customerId));
            //AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tCustomerAccounts(rowguid,type,provider,dtcreate,customerpk) values('{0}',3,1,getdate(),'{1}')", accountPK, customerPK));

        }


        [Test]
        public void Create()
        {
            MOId = CreateMoneyOrder();

            Assert.IsTrue(MOId > 0);

           // StopStartTransation();

            MoneyOrderStage moneyOrderStageOut = CXEMoneyOrderService.GetStage(MOId);

            Assert.IsTrue(moneyOrderStageOut.Amount == Amount
                && moneyOrderStageOut.Fee == Fee
                && moneyOrderStageOut.Status == (int)TransactionStates.Authorized);
        }

        [Test]
        public void UpdateMOStatus()
        {
            MOId = CreateMoneyOrder();

			//Update Check Number
			string accountNumber = "182380188280";
			string routingNumber = "075911603";
			string MICR = "o000917ot075911603t182380188280o";

			string moneyOrderNumber = "434";
			CXEMoneyOrderService.Update(MOId, moneyOrderNumber, accountNumber, routingNumber, MICR, timeZone);

            int newStatus = (int)TransactionStates.Committed;

            CXEMoneyOrderService.Update(MOId, newStatus, timeZone);

            StopStartTransation();

            Data.Transactions.Stage.MoneyOrder moneyOrderStage = CXEMoneyOrderService.GetStage(MOId);

            Assert.IsTrue(moneyOrderStage.Status == newStatus
                && moneyOrderStage.Amount == Amount
                && moneyOrderStage.Fee == Fee);
        }

        [Test]
        public void UpdateMONumber()
        {
            MOId = CreateMoneyOrder();
			string accountNumber =  "182380188280";
			string routingNumber =  "075911603";
			string MICR = "o000917ot075911603t182380188280o";

			string newMONumber = "434";
            CXEMoneyOrderService.Update(MOId, newMONumber, accountNumber, routingNumber, MICR, timeZone);

            StopStartTransation();
            Data.Transactions.Stage.MoneyOrder moneyOrderStage = CXEMoneyOrderService.GetStage(MOId);
            //MoneyOrderStage moneyOrderStage = CXEMoneyOrderService.GetStage(MOId);

            Assert.IsTrue(moneyOrderStage.MoneyOrderCheckNumber == Convert.ToString(newMONumber)
                && moneyOrderStage.Amount == Amount
                && moneyOrderStage.Fee == Fee);
        }

        [Test]
        public void Commit()
        {
            long moneyOrderId = CreateMoneyOrder();

            //Update Check Number
			string accountNumber = "182380188280";
			string routingNumber = "075911603";
			string MICR = "o000917ot075911603t182380188280o";

			string moneyOrderNumber = "434";
			CXEMoneyOrderService.Update(moneyOrderId, moneyOrderNumber, accountNumber, routingNumber, MICR, timeZone);

            //Update Stage status to commited
            int newStatus = (int)TransactionStates.Committed;
			CXEMoneyOrderService.Update(moneyOrderId, newStatus, timeZone);

            //Commit Transaction
			CXEMoneyOrderService.Commit(moneyOrderId);

            StopStartTransation();

			Data.Transactions.Commit.MoneyOrder moneyOrderCommit = CXEMoneyOrderService.Get(moneyOrderId);

            Assert.IsTrue(moneyOrderCommit.Amount == Amount
                && moneyOrderCommit.Fee == Fee
				&& moneyOrderCommit.MoneyOrderCheckNumber == moneyOrderNumber
                && moneyOrderCommit.Status == (int)TransactionStates.Committed);
        }

        [Test]
        public void StageACheckAudit()
        {


            MOId = CreateMoneyOrder();


           // StopStartTransation();

            //Update Stage status to Authorized
            int Status = (int)TransactionStates.Authorized;
            CXEMoneyOrderService.Update(MOId, Status, timeZone);

           // StopStartTransation();

            //Update Stage status to Processing
            Status = (int)TransactionStates.Processing;
            CXEMoneyOrderService.Update(MOId, Status, timeZone);

           // StopStartTransation();

            //Update Check Number
            int newMONumber = 434;
            CXEMoneyOrderService.Update(MOId, newMONumber, timeZone);

           // StopStartTransation();

            //Update Stage status to commited
            int newStatus = (int)TransactionStates.Committed;
            CXEMoneyOrderService.Update(MOId, newStatus, timeZone);

           // StopStartTransation();

            //Commit Transaction
            CXEMoneyOrderService.Commit(MOId);

            StopStartTransation();

            Data.Transactions.Commit.MoneyOrder moneyOrderCommit = CXEMoneyOrderService.Get(MOId);

            Assert.IsTrue(moneyOrderCommit.Amount == Amount
                && moneyOrderCommit.Fee == Fee             
                && moneyOrderCommit.Status == (int)TransactionStates.Committed);

			//SetComplete();
			//EndTransaction();
			//StartNewTransaction();

            int noOfRecords = (int)AdoTemplate.ExecuteScalar(CommandType.Text,
                                         "select count(*) from tTxn_MoneyOrder_Stage_AUD where id=" + MOId.ToString());
            Assert.IsNotNull(noOfRecords);
            //Assert.IsTrue(16 == noOfRecords);
            //AdoTemplate.ExecuteNonQuery(CommandType.Text, "delete from  tTxn_MoneyOrder_Stage");
            //AdoTemplate.ExecuteNonQuery(CommandType.Text, "delete from  tTxn_MoneyOrder_Stage_AUD");
        }

        private long CreateMoneyOrder()
        {
            Customer cust = _custSvc.Lookup(customerId);
            Account moAcct = cust.GetAccount(accountId);
            MoneyOrderStage moneyOrderStageIn = new MoneyOrderStage()
            {
                Account = moAcct,
                Fee = Fee,
				Amount = Amount,
                PurchaseDate = DateTime.Now,
				MoneyOrderCheckNumber = "123456",
                Status = (int)TransactionStates.Authorized
            };
            long moneyOrderId = CXEMoneyOrderService.Create(moneyOrderStageIn, timeZone);

            MoneyOrderStage moneyOrderStageOut = CXEMoneyOrderService.GetStage(moneyOrderId);

            Assert.IsTrue(moneyOrderStageOut.Account == moneyOrderStageIn.Account
                && moneyOrderStageOut.Amount == moneyOrderStageIn.Amount
                && moneyOrderStageOut.Fee == moneyOrderStageIn.Fee
                && moneyOrderStageOut.PurchaseDate == moneyOrderStageIn.PurchaseDate
                && moneyOrderStageOut.Status == moneyOrderStageIn.Status);
            return moneyOrderId;

        }

        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Core.CXE.Test/MGI.Core.CXE.Test/CXETestSpring.xml" }; }
        }

		public void StopStartTransation()
		{
			SetComplete();
			EndTransaction();
			StartNewTransaction();
		}
    }
}
