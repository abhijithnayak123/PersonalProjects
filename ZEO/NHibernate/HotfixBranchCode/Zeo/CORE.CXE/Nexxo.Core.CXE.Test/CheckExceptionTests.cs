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

namespace MGI.Core.CXE.Test
{
	[TestFixture]
	public class CheckExceptionTests
	{
		private CheckServiceImpl _svc = new CheckServiceImpl();
		private Mock<IRepository<Data.Transactions.Stage.Check>> mokCheckStageRepo = new Mock<IRepository<Data.Transactions.Stage.Check>>();
		private Mock<IRepository<Data.Transactions.Commit.Check>> mokCheckCommitRepo = new Mock<IRepository<Data.Transactions.Commit.Check>>();
        private string timezone = string.Empty;

		[TestFixtureSetUp]
		public void fixtSetup()
		{
			_svc.CheckStageRepo = mokCheckStageRepo.Object;
			_svc.CheckCommitRepo = mokCheckCommitRepo.Object;
		}

		[Test]
		public void StageCheckLookupThrows()
		{
			mokCheckStageRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Stage.Check, bool>>>())).Throws(new Exception());
			MinorCodeMatch<CXECheckException>(() => _svc.GetStatus(1), CXECheckException.CHECK_NOT_FOUND);
		}

		[Test]
		public void StageCheckUpdateLookupThrows()
		{
			mokCheckStageRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Stage.Check, bool>>>())).Throws(new Exception());
			MinorCodeMatch<CXECheckException>(() => _svc.Update(1, 2,null), CXECheckException.CHECK_NOT_FOUND);
		}

		[Test]
		public void CommitCheckLookupThrows()
		{
			mokCheckCommitRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Commit.Check, bool>>>())).Throws(new Exception());
			MinorCodeMatch<CXECheckException>(() => _svc.Get(1), CXECheckException.CHECK_NOT_FOUND);
		}

		[Test]
		public void StageCheckThrows()
		{
			mokCheckStageRepo.Setup(m => m.AddWithFlush(It.IsAny<Data.Transactions.Stage.Check>())).Throws(new Exception());
            MinorCodeMatch<CXECheckException>(() => _svc.Create(new Data.Transactions.Stage.Check(), timezone), CXECheckException.CHECK_CREATE_FAILED);
		}

		[Test]
		public void StageCheckUpdateThrows()
		{
			mokCheckStageRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Stage.Check, bool>>>())).Returns(new Data.Transactions.Stage.Check());
			mokCheckStageRepo.Setup(m => m.Merge(It.IsAny<Data.Transactions.Stage.Check>())).Throws(new Exception());
            MinorCodeMatch<CXECheckException>(() => _svc.Update(1, 2, timezone), CXECheckException.CHECK_UPDATE_FAILED);
		}

		[Test]
		public void CommitCheckThrows()
		{
			mokCheckStageRepo.Setup(m => m.FindBy(It.IsAny<Expression<Func<Data.Transactions.Stage.Check, bool>>>())).Returns(new Data.Transactions.Stage.Check());
			mokCheckCommitRepo.Setup(m => m.Add(It.IsAny<Data.Transactions.Commit.Check>())).Throws(new Exception());
			MinorCodeMatch<CXECheckException>(() => _svc.Commit(1), CXECheckException.CHECK_COMMIT_FAILED);
		}

		/// <summary>
		/// Make sure the NexxoException minor code matches
		/// </summary>
		/// <typeparam name="T">NexxoException type</typeparam>
		/// <param name="code">Code that's being checked</param>
		/// <param name="minorCode">Minor code to match</param>
		private void MinorCodeMatch<T>(TestDelegate code, int minorCode) where T : MGI.Common.Sys.NexxoException
		{
			try
			{
				code();
				Assert.IsTrue(false);
			}
			catch (T ex)
			{
				Assert.IsTrue(ex.MinorCode == minorCode);
			}
		}
	}
}
