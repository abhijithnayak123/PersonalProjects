using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.CXE.Impl;

using MGI.Common.DataAccess.Contract;

using NUnit.Framework;
using Moq;

using Spring.Testing.NUnit;

namespace MGI.Core.CXE.MoneyOrderExceptionTest
{
    [TestFixture]
    class MoneyOrderExceptionTest
    {
        MoneyOrderServiceImpl _svc = new MoneyOrderServiceImpl();
        private Mock<IRepository<Data.Transactions.Stage.MoneyOrder>> mokMoneyOrderStageRepo = new Mock<IRepository<Data.Transactions.Stage.MoneyOrder>>();
        private Mock<IRepository<Data.Transactions.Commit.MoneyOrder>> mokMoneyOrderCommitRepo = new Mock<IRepository<Data.Transactions.Commit.MoneyOrder>>();

        private string timeZone;

        [TestFixtureSetUp]
        public void fixtSetup()
        {
            timeZone = "Pacific Standard Time";
            _svc.MoneyOrderStageRepo = mokMoneyOrderStageRepo.Object;
            _svc.MoneyOrderCommitRepo = mokMoneyOrderCommitRepo.Object;
        }

        [Test]
        public void StageMoneyOrderCreateThrows()
        {
            mokMoneyOrderStageRepo.Setup(m => m.AddWithFlush(It.IsAny<Data.Transactions.Stage.MoneyOrder>())).Throws(new Exception());
            MinorCodeMatch<CXEMoneyOrderException>(() => _svc.Create(new Data.Transactions.Stage.MoneyOrder(), timeZone), CXEMoneyOrderException.MONEYORDER_CREATE_FAILED);
        }


        [Test]
        public void StageMoneyOrderUpdateStatusLookupThrows()
        {
            int newStatus = (int)TransactionStates.Authorized;
            mokMoneyOrderStageRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Stage.MoneyOrder, bool>>>())).Throws(new Exception());
			MinorCodeMatch<CXEMoneyOrderException>(() => _svc.Update(1, newStatus, timeZone), CXEMoneyOrderException.MONEYORDER_GET_FAILED);
        }

        [Test]
        public void StageMoneyOrderUpdateCheckNumberLookupThrows()
        {
            int newCheckNumber = 12345;
            mokMoneyOrderStageRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Stage.MoneyOrder, bool>>>())).Throws(new Exception());
			MinorCodeMatch<CXEMoneyOrderException>(() => _svc.Update(1, newCheckNumber, timeZone), CXEMoneyOrderException.MONEYORDER_GET_FAILED);
        }

        [Test]
        public void StageMoneyOrderUpdateStatusThrows()
        {
            int newStatus = (int)TransactionStates.Authorized;
            mokMoneyOrderStageRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Stage.MoneyOrder, bool>>>())).Returns(new Data.Transactions.Stage.MoneyOrder());
            mokMoneyOrderStageRepo.Setup(m => m.Merge(It.IsAny<Data.Transactions.Stage.MoneyOrder>())).Throws(new Exception());
            MinorCodeMatch<CXEMoneyOrderException>(() => _svc.Update(1, newStatus, timeZone), CXEMoneyOrderException.MONEYORDER_UPDATE_FAILED);
        }

        [Test]
        public void StageMoneyOrderCommitLookupThrows()
        {
            mokMoneyOrderStageRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Stage.MoneyOrder, bool>>>())).Throws(new Exception());
			MinorCodeMatch<CXEMoneyOrderException>(() => _svc.Commit(1), CXEMoneyOrderException.MONEYORDER_GET_FAILED);
        }

        [Test]
        public void StageMoneyOrderUpdateCheckNumberThrows()
        {
			int newCheckNumber = 12345;
            mokMoneyOrderStageRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Stage.MoneyOrder, bool>>>())).Returns(new Data.Transactions.Stage.MoneyOrder());
            mokMoneyOrderStageRepo.Setup(m => m.Merge(It.IsAny<Data.Transactions.Stage.MoneyOrder>())).Throws(new Exception());
            MinorCodeMatch<CXEMoneyOrderException>(() => _svc.Update(1, newCheckNumber, timeZone), CXEMoneyOrderException.MONEYORDER_UPDATE_FAILED);
        }

        [Test]
        public void CommitMoneyOrderLookupThrows()
        {
            mokMoneyOrderCommitRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Commit.MoneyOrder, bool>>>())).Throws(new Exception());
			MinorCodeMatch<CXEMoneyOrderException>(() => _svc.Get(1), CXEMoneyOrderException.MONEYORDER_GET_FAILED);
        }

        [Test]
        public void StageMoneyOrderLookupThrows()
        {
            mokMoneyOrderStageRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Stage.MoneyOrder, bool>>>())).Throws(new Exception());
			MinorCodeMatch<CXEMoneyOrderException>(() => _svc.GetStage(1), CXEMoneyOrderException.MONEYORDER_GET_FAILED);
        }

        [Test]
        public void CommitMoneyOrderCreateThrows()
        {
            mokMoneyOrderStageRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Stage.MoneyOrder, bool>>>())).Returns(new Data.Transactions.Stage.MoneyOrder());
            mokMoneyOrderCommitRepo.Setup(m => m.SaveOrUpdate(It.IsAny<Data.Transactions.Commit.MoneyOrder>())).Throws(new Exception());
            MinorCodeMatch<CXEMoneyOrderException>(() => _svc.Commit(1), CXEMoneyOrderException.MONEYORDER_COMMIT_FAILED);
        }

        /// <summary>
        /// Make sure the NexxoException minor code matches
        /// </summary>
        /// <typeparam name="T">NexxoException type</typeparam>
        /// <param name="code">Code that's being checked</param>
        /// <param name="minorCode">Minor code to match</param>
        private void MinorCodeMatch<T>(TestDelegate code, string minorCode) where T : MGI.Common.Sys.AlloyException
        {
            try
            {
                code();
                Assert.IsTrue(false);
            }
            catch (T ex)
            {
                Assert.IsTrue(ex.AlloyErrorCode == minorCode);
            }

        }
    }
}
