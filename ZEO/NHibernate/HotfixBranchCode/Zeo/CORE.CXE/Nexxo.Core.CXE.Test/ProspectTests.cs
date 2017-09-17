using System;
using System.Collections.Generic;
using System.Linq;
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
	public class ProspectTests
	{
		private CustomerServiceImpl custSvc;
		private Mock<IRepository<Prospect>> mokProspectRepo;
		private Mock<IRepository<Customer>> mokCustomerRepo;
		private Mock<IIDNumberBuilder> mokIDBuilder;

		[TestFixtureSetUp]
		public void setup()
		{
			custSvc = new CustomerServiceImpl();
			mokProspectRepo = new Mock<IRepository<Prospect>>();
			mokCustomerRepo = new Mock<IRepository<Customer>>();
			mokIDBuilder = new Mock<IIDNumberBuilder>();
			custSvc.ProspectRepo = mokProspectRepo.Object;
			custSvc.CustomerRepo = mokCustomerRepo.Object;
			custSvc.IDBuilder = mokIDBuilder.Object;
		}

		[Test]
		public void SaveProspectSetsDates()
		{
			Prospect prospect = new Prospect { FirstName = "blah" };
			mokProspectRepo.Setup( m => m.SaveOrUpdate( It.IsAny<Prospect>() ) ).Returns(true);
			long randPAN = new Random().Next( 100 );
			mokIDBuilder.Setup( m => m.NextPAN() ).Returns( randPAN );

			custSvc.SaveProspect( prospect );
			Assert.IsTrue( prospect.DTCreate > DateTime.Today );
			Assert.IsTrue( prospect.DTLastMod == null );
			Assert.IsTrue( prospect.PAN == randPAN );

			DateTime dtCreate = prospect.DTCreate;

			// id would be assigned by db
			prospect.id = Guid.NewGuid();

			prospect.LastName = "dibb";
			custSvc.SaveProspect( prospect );
			Assert.IsTrue( prospect.DTCreate == dtCreate );
			Assert.IsTrue( prospect.DTLastMod > DateTime.Today );

			DateTime dtLastMod = (DateTime)prospect.DTLastMod;
			prospect.City = "flam";

			Console.WriteLine( dtLastMod.ToString( "mmssffff" ) + " : " + ((DateTime)prospect.DTLastMod).ToString( "mmssffff" ) );

			System.Threading.Thread.Sleep( 10 );

			custSvc.SaveProspect( prospect );
			Console.WriteLine( dtLastMod.ToString( "mmssffff" ) + " : " + ( (DateTime)prospect.DTLastMod ).ToString( "mmssffff" ) );
			Assert.IsTrue( prospect.DTLastMod > dtLastMod );
		}

		//[Test]
		//public void RegisterMaps()
		//{
		//    DateTime pDTCreate = new DateTime( 2013, 4, 11, 16, 39, 01 );
		//    Prospect pros = new Prospect
		//    {
		//        FirstName = "glim",
		//        LastName = "glam",
		//        DTCreate = pDTCreate,
		//        DTLastMod = new DateTime( 2013, 4, 11, 16, 40, 01 )
		//    };
		//    mokProspectRepo.Setup( m => m.FindBy( It.IsAny<System.Linq.Expressions.Expression<Func<Prospect, bool>>>() ) ).Returns( pros );

		//    mokCustomerRepo.Setup( m => m.Add( It.IsAny<CustomerProfile>() ) ).Returns( ( CustomerProfile cp ) => cp.DTCreate > pDTCreate );

		//    custSvc.Register( 101 );
		//}

	}
}
