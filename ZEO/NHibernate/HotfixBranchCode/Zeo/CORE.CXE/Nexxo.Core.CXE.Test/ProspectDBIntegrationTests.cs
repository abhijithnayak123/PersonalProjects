using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.CXE.Impl;
using MGI.Biz.Partner.Data;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
using Moq;

using Spring.Testing.NUnit;

namespace MGI.Core.CXE.Test
{
	[TestFixture]
	public class ProspectDBIntegrationTests : AbstractTransactionalDbProviderSpringContextTests
	{
		private ICustomerService _custSvc;
		public ICustomerService CustomerService { set { _custSvc = value; } }

		private IdTypeServiceImpl _idTypeSvc;
		public IdTypeServiceImpl IdTypeService { set { _idTypeSvc = value; } }

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Core.CXE.Test/MGI.Core.CXE.Test/CXETestSpring.xml" }; }
		}

		[Test]
		public void SaveAndLookupProspectTest()
		{
			Prospect p = new Prospect();
			p.FName = "glim";
			p.LName = "glam";
			//p.PAN = 100111;
			p.DTCreate = DateTime.Now;

			p.AddEmploymentDetails( "fline", "gls", "12345" );

			_custSvc.SaveProspect( p );

			long tempPAN = p.PAN;
			Console.WriteLine( tempPAN );

			Assert.IsTrue( p.id!=null && p.id != Guid.Empty );

			//get an idType
			IdType idType = _idTypeSvc.Find( "DRIVER'S LICENSE", "UNITED STATES", "CALIFORNIA" );

			p.AddGovernmentId( idType, "gla", new DateTime( 2011, 10, 10 ), new DateTime( 2016, 10, 10 ) );
			_custSvc.SaveProspect( p );

			p.IsAccountHolder = true;
			p.ReferralCode = "08981234";
			p.PIN = "1234";

			_custSvc.SaveProspect( p );

			string refCode = AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select ReferralCode from tProspects where PAN={0}", tempPAN ) ).ToString();
			bool acctHld = (bool)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select IsAccountHolder from tProspects where PAN={0}", tempPAN ) );
			string pin = AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select PIN from tProspects where PAN={0}", tempPAN ) ).ToString();

			Assert.IsTrue( acctHld );
			Assert.IsTrue( refCode == "08981234" );
			Assert.IsTrue( pin == "1234" );

			//Assert.IsTrue( p.GovernmentId.Identification == "gla" );

			//Prospect pLookup = _custSvc.LookupProspect( tempPAN );

			//Assert.IsTrue( pLookup.FirstName == "glim" );
		}

		private void SetupProspect( long id )
		{
			// setup prospect to register
			Guid ProspectPK = Guid.NewGuid();
			AdoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "INSERT tProspects(id,PAN,FirstName,LastName,PIN,DTCreate) VALUES('{0}',{1},'slim','weeks','1234',getdate())", ProspectPK, id ) );
			AdoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "INSERT tProspectEmploymentDetails(ProspectId,PAN,Occupation,DTCreate) VALUES('{0}',100999,'joker',getdate())", ProspectPK ) );
			Guid idtypePK = (Guid)AdoTemplate.ExecuteScalar( CommandType.Text, "select rowguid from tIdTypes where Id=110" );
			AdoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "INSERT tProspectGovernmentIdDetails(ProspectId,PAN,IdTypePK,DTCreate) VALUES('{0}',100999,'{1}',getdate())", ProspectPK, idtypePK ) );
		}

		[Test]
		public void LookupAndSaveProspect()
		{
			long pan = 100999;
			SetupProspect( pan );

			Prospect p = _custSvc.LookupProspect( pan );

			p.Phone1 = "1234567890";
			p.Phone1Provider = "verizon";
			p.Phone1Type = "mobile";

			_custSvc.SaveProspect( p );
		}

		[Test]
		public void RegisterTest()
		{
			long pan = 100998;

			SetupProspect( pan );

			Customer customer = _custSvc.Register( pan );

			Guid custPK = (Guid)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select rowguid from tCustomers where Id={0}", pan ) );
			Guid idTypePK = (Guid)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select IdTypePK from tCustomerGovernmentIdDetails where CustomerPK='{0}'", custPK ) );
			string pin = AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select PIN from tCustomerProfiles where CustomerPK='{0}'", custPK ) ).ToString();

			Assert.IsTrue( custPK != Guid.Empty );
			Assert.IsTrue( idTypePK != Guid.Empty );
			Assert.IsTrue( pin == "1234" );
		}

		[Test]
		public void LookupProspectRemoveGovtId()
		{
			long pan = 100999;
			SetupProspect( pan );

			Prospect p = _custSvc.LookupProspect( pan );

			Guid idTypePK = p.GovernmentId.IdType.rowguid;

			p.GovernmentId = null;

			_custSvc.SaveProspect( p );

			// doesn't delete prospect
			int prospectCount = (int)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select count(*) from tProspects where id='{0}'", p.id ) );
			Assert.IsTrue( prospectCount == 1 );
			// deletes GovtIdDetails
			int prospectIdCount = (int)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select count(*) from tProspectGovernmentIdDetails where ProspectId='{0}'", p.id ) );
			Assert.IsTrue( prospectIdCount == 0 );
			// doesn't delete IdType
			int idTypeCount = (int)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select count(*) from tIdTypes where rowguid='{0}'", idTypePK ) );
			Assert.IsTrue( idTypeCount == 1 );
		}
	}
}