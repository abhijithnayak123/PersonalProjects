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
	public class CustomerServiceExceptionTests
	{
		private CustomerServiceImpl _svc = new CustomerServiceImpl();
		private Mock<IRepository<Customer>> mokCustRepo = new Mock<IRepository<Customer>>();
		private Mock<IRepository<Prospect>> mokProspRepo = new Mock<IRepository<Prospect>>();
		private Mock<IIDNumberBuilder> mokIDBuilder = new Mock<IIDNumberBuilder>();

		[TestFixtureSetUp]
		public void fixtSetup()
		{
			_svc.CustomerRepo = mokCustRepo.Object;
			_svc.ProspectRepo = mokProspRepo.Object;
			_svc.IDBuilder = mokIDBuilder.Object;
		}

		[Test]
		public void LookupFailThrows()
		{
			mokProspRepo.Setup( m => m.FindBy( It.IsAny<Expression<Func<Prospect, bool>>>() ) ).Throws( new Exception() );

			MinorCodeMatch<CXECustomerException>( () => _svc.LookupProspect( 1 ), CXECustomerException.PROSPECT_NOT_FOUND );
		}

		[Test]
		public void SaveProspectThrows()
		{
			mokProspRepo.Setup( m => m.SaveOrUpdate(It.IsAny<Prospect>()) ).Throws( new Exception() );

			MinorCodeMatch<CXECustomerException>( () => _svc.SaveProspect( new Prospect() ), CXECustomerException.PROSPECT_SAVE_FAILED );

			mokIDBuilder.Setup( m => m.NextPAN() ).Throws( new Exception() );

			MinorCodeMatch<CXECustomerException>( () => _svc.SaveProspect( new Prospect() ), CXECustomerException.ID_GENERATION_FAILED );
		}

		[Test]
		public void RegisterThrows()
		{
			mokProspRepo.Setup( m => m.FindBy( It.IsAny<Expression<Func<Prospect, bool>>>() ) ).Returns(new Prospect());

			mokCustRepo.Setup( m => m.AddWithFlush( It.IsAny<Customer>() ) ).Throws( new Exception() );

			MinorCodeMatch<CXECustomerException>( () => _svc.Register( 1 ), CXECustomerException.REGISTRATION_FAILED_DATABASE );

			var sqlEx = SqlExceptionHelper.Generate( 2627 );
			mokCustRepo.Setup( m => m.AddWithFlush( It.IsAny<Customer>() ) ).Throws( new Exception("", sqlEx) );

			MinorCodeMatch<CXECustomerException>( () => _svc.Register( 1 ), CXECustomerException.REGISTRATION_FAILED_DUPLICATE_ID );
		}

		[Test]
		public void LookupCustomerThrows()
		{
			mokCustRepo.Setup( m => m.FindBy( It.IsAny<Expression<Func<Customer, bool>>>() ) ).Throws( new Exception() );

			MinorCodeMatch<CXECustomerException>( () => _svc.Lookup( 101 ), CXECustomerException.CUSTOMER_NOT_FOUND );
		}

		[Test]
		public void SaveCustomerThrows()
		{
			mokCustRepo.Setup( m => m.Update( It.IsAny<Customer>() ) ).Throws( new Exception() );

			MinorCodeMatch<CXECustomerException>( () => _svc.Save( new Customer() ), CXECustomerException.CUSTOMER_UPDATE_FAILED );
		}
	
		/// <summary>
		/// Make sure the NexxoException minor code matches
		/// </summary>
		/// <typeparam name="T">NexxoException type</typeparam>
		/// <param name="code">Code that's being checked</param>
		/// <param name="minorCode">Minor code to match</param>
		private void MinorCodeMatch<T>( TestDelegate code, int minorCode ) where T : MGI.Common.Sys.NexxoException
		{
			try
			{
				code();
				Assert.IsTrue( false );
			}
			catch ( T ex )
			{
				Assert.IsTrue( ex.MinorCode == minorCode );
			}
		}
	}
}
