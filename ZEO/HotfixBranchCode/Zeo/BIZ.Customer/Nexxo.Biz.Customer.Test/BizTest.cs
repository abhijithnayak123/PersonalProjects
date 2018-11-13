using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MGI.Biz.Customer.Contract;
using MGI.Biz.Customer.Data;
using MGI.Biz.Customer.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;

using Spring.Testing.NUnit;
using Spring.Data.Core;

namespace MGI.Biz.Customer.Test
{
	[TestFixture]
	public class BizTest : AbstractTransactionalSpringContextTests
	{
		private CustomerServiceImpl _bizCustEng;
		//public CustomerServiceImpl BIZCustomerEngine { set { _bizCustEng = value; } }

		private AdoTemplate _cxeAdo;
		private AdoTemplate _partnerAdo;
		private AdoTemplate _fvFundsAdo;

		private SessionContext context;

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Biz.Customer.Test/MGI.Biz.Customer.Test/BizSpring.xml" }; }
		}

		[SetUp]
		public void setup()
		{
			context = new SessionContext
			{
				AgentId = 1,
				AgentName = "bill",
				AppName = "mdTest",
				ChannelPartnerId = 27,
				LocationAgentId = 2,
				LocationId = Guid.NewGuid()
			};

			_cxeAdo = (AdoTemplate)applicationContext.GetObject( "cxeAdo" );
			_partnerAdo = (AdoTemplate)applicationContext.GetObject( "partnerAdo" );
			_fvFundsAdo = (AdoTemplate)applicationContext.GetObject( "fvFundsAdo" );
			_bizCustEng = (CustomerServiceImpl)applicationContext.GetObject( "InterceptedBizCustomerEngine" );
		}

		[Test]
		public void SaveAProspect()
		{
			Prospect prospect = new Prospect
			{
				FName = "mike",
				LName = "test",
			};

			string pan = _bizCustEng.SaveProspect( context, prospect );

			Assert.That( pan != string.Empty, "no pan!" );

			DataSet dsFields = _cxeAdo.DataSetCreate( CommandType.Text, string.Format( "select FirstName, LastName, DOB, DTLastMod from tProspects where PAN={0}", pan ) );
			
			Assert.That( dsFields.Tables[0].Rows[0]["FirstName"].ToString() == "MIKE", "not mike!" );
			Assert.That( dsFields.Tables[0].Rows[0]["LastName"].ToString() == "TEST", "not test!" );
		}

		[Test]
		public void SaveExistingProspect()
		{
			Guid prospectGuid = InsertProspect(100111);

			Prospect prospect = new Prospect
			{
				FName = "FASHION",
				LName = "DELL",
				DOB = new DateTime( 1982, 12, 3 )
			};

			_bizCustEng.SaveProspect( context, 100111, prospect );

			//SetComplete();
			//EndTransaction();

			DataSet dsProspect = _cxeAdo.DataSetCreate( CommandType.Text, string.Format( "select FirstName, LastName, DOB, DTLastMod from tProspects where id='{0}'", prospectGuid ) );
			Assert.IsTrue( dsProspect.Tables[0].Rows[0]["FirstName"].ToString() == "FASHION" );
			Assert.IsTrue( dsProspect.Tables[0].Rows[0]["LastName"].ToString() == "DELL" );
		}

		private Guid InsertProspect(long pan)
		{
			Guid prospectGuid = Guid.NewGuid();
			_cxeAdo.ExecuteNonQuery( CommandType.Text, string.Format( "INSERT tProspects (id,PAN,firstname,lastname,dtcreate) values('{0}',{1},'TEST','TESTER',getdate())", prospectGuid, pan ) );
			_cxeAdo.ExecuteNonQuery( CommandType.Text, string.Format( "INSERT tProspectEmploymentDetails (ProspectId,occupation,employer,employerphone,dtcreate) values('{0}','lumberjack','krusty klown kollege','4157778888',getdate())", prospectGuid ) );
			return prospectGuid;
		}

		[Test]
		public void Register()
		{
			long pan = 100112;

			Guid prospectGuid = InsertProspect( pan );

			_bizCustEng.Register( context, pan );

			Guid custGuid = (Guid)_cxeAdo.ExecuteScalar( CommandType.Text, string.Format( "select rowguid from tCustomers where id={0}", pan ) );
			Assert.IsTrue( custGuid!=Guid.Empty );
			DataSet dsCustProfile = _cxeAdo.DataSetCreate( CommandType.Text, string.Format( "select FirstName, LastName from tCustomerProfiles where customerpk='{0}'", custGuid ) );
			Assert.IsTrue( dsCustProfile.Tables[0].Rows[0]["FirstName"].ToString() == "TEST" );
			string occupation = _cxeAdo.ExecuteScalar( CommandType.Text, string.Format( "select occupation from tCustomerEmploymentDetails where customerpk='{0}'", custGuid ) ).ToString();
			Assert.IsTrue( occupation.ToUpper() == "LUMBERJACK" );

			int pCust = (int)_partnerAdo.ExecuteScalar( CommandType.Text, string.Format( "select count(*) from tPartnerCustomers where cxeid={0}", pan ) );
			Assert.IsTrue( pCust == 1 );
		}

		[Test]
		public void InitiateSession()
		{
			long agSessionId = InsertAgentSession();
			long pan = 100113;
			InsertCustomer( pan );

			CustomerSession cs = _bizCustEng.InitiateCustomerSession( agSessionId.ToString(), pan );

			Assert.IsNotNullOrEmpty( cs.CustomerSessionId );
			Assert.IsTrue( cs.Customer.Profile.FirstName == "TEST" );

			_bizCustEng.ConcludeCustomerSession( cs.CustomerSessionId );
			DateTime dtend = (DateTime)_partnerAdo.ExecuteScalar( CommandType.Text, string.Format( "select DTEnd from tCustomerSessions where Id={0}", cs.CustomerSessionId ) );
			Assert.IsTrue( dtend > DateTime.MinValue );
		}

		private long InsertAgentSession()
		{
			Guid locGuid = Guid.NewGuid();
			_partnerAdo.ExecuteNonQuery( CommandType.Text, string.Format( "insert tLocations(rowguid,name,address,phone,channelpartnerid,dtcreate) values('{0}','test','123 test','1231234567',1,getdate())", locGuid ) );
			Guid terminalGuid = Guid.NewGuid();
			_partnerAdo.ExecuteNonQuery( CommandType.Text, string.Format( "insert tTerminals(rowguid,Id,LocationPK,dtcreate) values('{0}','md002','{1}',getdate())", terminalGuid, locGuid ) );
			Guid agSessGuid = Guid.NewGuid();
			_partnerAdo.ExecuteNonQuery( CommandType.Text, string.Format( "insert tAgentSessions(rowguid,AgentId,TerminalPK,dtcreate) values('{0}','mdTest','{1}',getdate())", agSessGuid, terminalGuid ) );
			return (long)_partnerAdo.ExecuteScalar( CommandType.Text, string.Format( "select Id from tAgentSessions where rowguid='{0}'", agSessGuid ) );
		}

		private void InsertCustomer( long pan )
		{
			Guid customerGuid = Guid.NewGuid();
			_cxeAdo.ExecuteNonQuery( CommandType.Text, string.Format( "INSERT tCustomers (rowguid,id,dtcreate) values('{0}',{1},getdate())", customerGuid, pan ) );
			_cxeAdo.ExecuteNonQuery( CommandType.Text, string.Format( "INSERT tCustomerProfiles (CustomerPK,FirstName,LastName,dtcreate) values('{0}','TEST','TESTER',getdate())", customerGuid, pan ) );

			_partnerAdo.ExecuteNonQuery( CommandType.Text, string.Format( "insert tPartnerCustomers(rowguid,Id,CXEId,dtcreate) values(newid(),{0},{0},getdate())", pan ) );
		}

		[Test]
		public void SearchCustomers()
		{
			Dictionary<string, object> searchContext = new Dictionary<string, object>();
			List<CustomerSearchResult> customers = _bizCustEng.Search( "1", new CustomerSearchCriteria { FirstName = "MIKE", LastName="TEST" }, searchContext );

			Console.WriteLine( customers.Count );

			//Console.WriteLine( customers[0].DOB.ToString() );
			//Console.WriteLine( customers[1].DOB.ToString() );
		}

		[Test]
		public void SearchByCardnumber()
		{

		}

		[Test]
		public void LookupProspect()
		{
			Guid pg = InsertProspect( 102 );

			Prospect prospect = _bizCustEng.GetProspect( new SessionContext(), 102 );
		}

		[Test]
		public void DuplicateThrowsBizException()
		{
			//Guid prospectGuid = InsertProspect( 11 );
			//InsertCustomer( 11 );

			//TestExceptionHelper.MinorCodeMatch<BizCustomerException>( () => _bizCustEng.Register( context, 11 ), (int)MGI.Core.CXE.Contract.CXECustomerException.REGISTRATION_FAILED_DUPLICATE_ID );
		}

		[Test]
		public void GetTransactionHistory()
		{
			List<TransactionHistory> transactionHistory = _bizCustEng.GetTransactionHistory( 1000000000000290 );
			Assert.That( transactionHistory.Count == 8, "Get transaction history failed" );
		}

		private Guid SetupPartnerCustomer( long PAN, bool acctHolder, string refCode )
		{
			Guid partnerPK = Guid.NewGuid();
			_partnerAdo.ExecuteNonQuery( CommandType.Text, string.Format( "insert tPartnerCustomers(rowguid,Id,CXEId,DTCreate,ReferralCode,IsPartnerAccountHolder) values('{0}',{1},{1},getdate(),'{2}',{3})", partnerPK, PAN, refCode, ( acctHolder ? 1 : 0 ) ) );
			return partnerPK;
		}

		[Test]
		public void TestGetCustomer()
		{
			Guid custG = SetupCustomer( 10229, "trell", "mongo", DateTime.Today.AddYears( -25 ), "2340987", "D2342341", "rifter" );
			Guid partnerG = SetupPartnerCustomer( 10229, true, "23423456" );

			Data.Customer bizCustomer = _bizCustEng.GetCustomer( "1", 10229 );

			Assert.IsTrue( bizCustomer.ID.IDType == "DRIVER'S LICENSE" );
			Assert.IsTrue( bizCustomer.Profile.IsPartnerAccountHolder );
			Assert.IsTrue( bizCustomer.Profile.ReferralCode == "23423456" );
		}

		private Guid SetupCustomer( long PAN, string fName, string LName, DateTime DOB, string phone, string govtId, string occupation )
		{
			if ( DOB == DateTime.MinValue )
				DOB = DateTime.Today.AddYears( -20 );
			// setup customer to lookup
			Guid _customerPK = Guid.NewGuid();
			_cxeAdo.ExecuteNonQuery( CommandType.Text,
				string.Format( "INSERT tCustomers(rowguid,Id,DTCreate) VALUES('{0}',{1},getdate())", _customerPK, PAN ) );
			_cxeAdo.ExecuteNonQuery( CommandType.Text,
				string.Format( "INSERT tCustomerProfiles(CustomerPK,FirstName,LastName,DOB,Phone1,DTCreate) VALUES('{0}','{1}','{2}','{3}','{4}',getdate())", _customerPK, fName, LName, DOB, phone ) );
			_cxeAdo.ExecuteNonQuery( CommandType.Text,
				string.Format( "INSERT tCustomerEmploymentDetails(CustomerPK,Occupation,DTCreate) VALUES('{0}','{1}',getdate())", _customerPK, occupation ) );
			if ( !string.IsNullOrEmpty( govtId ) )
			{
				Guid idtypePK = (Guid)_cxeAdo.ExecuteScalar( CommandType.Text, "select rowguid from tIdTypes where Id=110" );
				_cxeAdo.ExecuteNonQuery( CommandType.Text,
					string.Format( "INSERT tCustomerGovernmentIdDetails(CustomerPK,IdTypePK,Identification,DTCreate) VALUES('{0}','{1}','{2}',getdate())", _customerPK, idtypePK, govtId ) );
			}
			return _customerPK;
		}

		[Test]
		public void SaveAnExistingCustomer()
		{
			Guid custG = SetupCustomer( 10229, "trell", "mongo", DateTime.Today.AddYears( -25 ), "2340987", "", "rifter" );

			Data.Customer bizCustomer = _bizCustEng.GetCustomer( "1", 10229 );

			bizCustomer.Profile.City = "pitts";
			bizCustomer.ID = new Identification { Country = "UNITED STATES", State = "CALIFORNIA", ExpirationDate = DateTime.Today.AddYears( 5 ), IDType = "DRIVER'S LICENSE", ID = "D2342344" };

			_bizCustEng.SaveCustomer( "1", 10229, bizCustomer );

			string city = _cxeAdo.ExecuteScalar( CommandType.Text, string.Format( "select city from tCustomerProfiles where CustomerPK='{0}'", custG ) ).ToString();
			Assert.IsTrue( city == "pitts" );
			int idCount = (int)_cxeAdo.ExecuteScalar( CommandType.Text, string.Format( "select count(*) from tCustomerGovernmentIdDetails where CustomerPK='{0}'", custG ) );
			Assert.IsTrue( idCount == 1 );
		}
	}
}
