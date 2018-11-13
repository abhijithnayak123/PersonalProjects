using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.CXE.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
using Moq;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Mapping.ByCode;
using NHibernate.Context;

namespace MGI.Core.CXE.Test
{
	[TestFixture]
	public class IdTypeServiceTests
	{
		public IdTypeServiceImpl _svc;
		public Mock<IRepository<IdType>> _mokRepo;

		[SetUp]
		public void setup()
		{
			_svc = new IdTypeServiceImpl();
			_mokRepo = new Mock<IRepository<IdType>>();

			_svc.IdTypeRepo = _mokRepo.Object;
		}

		[Test]
		public void FindIdTypeFinds()
		{
			IdType idtype = new IdType { Name = "test", Country = "test", HasExpirationDate = false, Mask = "test" };
			_mokRepo.Setup( m => m.FindBy( It.IsAny<Expression<Func<IdType, bool>>>() ) ).Returns( idtype );

			IdType result = _svc.Find( "test", "test", null );
			Assert.IsTrue( result.Mask == "test" );
		}

		[Test]
		public void FindIdTypeFails()
		{
			_mokRepo.Setup( m => m.FindBy( It.IsAny<Expression<Func<IdType, bool>>>() ) ).Throws( new Exception() );

			IdType result = _svc.Find( "test", "test", "" );
			Assert.IsNull( result );
		}
	}
}
