using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.CXE.Impl;

using MGI.Common.DataAccess.Impl;
//using MGI.Channel.DMS.Server.Contract;
using NUnit.Framework;
using Moq;

using Spring.Testing.NUnit;

namespace MGI.Core.CXE.Test
{
	public class CustomerServiceDBTests : AbstractTransactionalDbProviderSpringContextTests
	{
		//private ICustomerService _custSvc;
		public ICustomerService CXECustomerService { get; set; }
		//public ICustomerService CustomerService { set { _custSvc = value; } }
	
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Core.CXE.Test/MGI.Core.CXE.Test/springCustTest.xml" }; }
		}

		private Guid SetupCustomer(long alloyId, string fName, string LName, DateTime DOB, string phone, string govtId, string occupation)
		{
			if ( DOB == DateTime.MinValue )
				DOB = DateTime.Today.AddYears( -20 );
			// setup customer to lookup
			Guid _customerPK = Guid.NewGuid();
			AdoTemplate.ExecuteNonQuery( CommandType.Text,
				string.Format("INSERT tCustomers(rowguid,Id,DTCreate,FirstName,DOB) VALUES('{0}',{1},getdate(),'{2}','{3}')", _customerPK, alloyId, fName,DOB));

			AdoTemplate.ExecuteScalar(CommandType.Text, "select count(*) from tCustomers");

			AdoTemplate.ExecuteNonQuery( CommandType.Text, 
				string.Format( "INSERT tCustomerEmploymentDetails(CustomerPK,Occupation,DTCreate) VALUES('{0}','{1}',getdate())", _customerPK, occupation ) );
			if ( !string.IsNullOrEmpty( govtId ) )
			{
				//Guid idtypePK = (Guid)AdoTemplate.ExecuteScalar( CommandType.Text, "select rowguid from tIdTypes where Id=110" );
				AdoTemplate.ExecuteNonQuery( CommandType.Text,
					string.Format( "INSERT tCustomerGovernmentIdDetails(CustomerPK,IdTypeId,Identification,DTCreate) VALUES('{0}','{1}','{2}',getdate())", _customerPK, 110, govtId ) );
			}
			return _customerPK;
		}

		[Test]
		public void LookupCustomer()
		{
			long alloyId = 100999;
			Guid _customerPK = SetupCustomer(alloyId, "slim", "weeks", DateTime.MinValue, "1234567", "D2345", "joker");
			//Customer c = _custSvc.Lookup( alloyId );		
			Customer c = CXECustomerService.Lookup(alloyId);

			Assert.IsTrue( c.Id == alloyId );
			Assert.IsTrue( c.FirstName == "slim" );
			Assert.IsTrue( c.EmploymentDetails.Occupation == "joker" );
			//Assert.IsTrue( c.GovernmentId.Id == 110 );
		}

		//[Test]
		//public void GetCustomerAccounts()
		//{
		//	long alloyId = 100999;
		//	Guid customerPK = SetupCustomer( alloyId, "slim", "weeks", DateTime.MinValue, "1234567", "D2345", "joker" );

		//	AdoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "INSERT tCustomerAccounts(rowguid,type,dtcreate,customerpk) values(newid(),1,getdate(),'{0}')", customerPK ) );
		//	AdoTemplate.ExecuteNonQuery( CommandType.Text, string.Format( "INSERT tCustomerAccounts(rowguid,type,dtcreate,customerpk) values(newid(),4,getdate(),'{0}')", customerPK ) );

		//	//Customer c = _custSvc.Lookup( alloyId );
		//	Customer c = CXECustomerService.Lookup(alloyId);

		//	Account cashAcct = c.GetAccount( (int)AccountTypes.Cash );
		//	Assert.IsTrue( cashAcct.Type == 1 );
		//	Account bpAcct = c.GetAccount( (int)AccountTypes.BillPay);
		//	//Assert.IsTrue( bpAcct.Provider == 7 );
		//	Account moAcct = c.GetAccount( (int)AccountTypes.MoneyOrder);
		//	Assert.IsTrue( moAcct == null );
		//}

		[Test]
		public void SearchCustomers()
		{
			SetupCustomer( 8888888, "slim", "weeks", new DateTime(1977,2,3), "2345678", "D0001", "plant" );
			SetupCustomer( 6666666, "jack", "month", DateTime.MinValue, "1234567", "D2346", "water" );
			SetupCustomer( 1111111, "pore", "years", DateTime.MinValue, "3333333", "D0002", "desk" );
			SetupCustomer( 2222222, "jims", "redds", DateTime.MinValue, "4444444", "D0002", "chair" );
			SetupCustomer( 3333333, "harv", "weeks", DateTime.MinValue, "1234567", "D2345", "plant" );
			SetupCustomer( 4444444, "welt", "woods", DateTime.MinValue, "5555555", "D2347", "earth" );
			SetupCustomer( 5555555, "slim", "pills", new DateTime(1976,3,4), "2345679", "D2348", "fire" );

			CustomerSearchCriteria criteria = new CustomerSearchCriteria();
			_search( criteria, 0 );
			criteria.FirstName = "slim";
			_search( criteria, 2 );
			criteria.DateOfBirth = new DateTime( 1977, 2, 3 );
			_search( criteria, 1 );
			criteria.FirstName = "";
			criteria.DateOfBirth = null;
			criteria.AlloyID = 3333333;
			_search( criteria, 1 );
		}

		private void _search( CustomerSearchCriteria criteria, int count )
		{
			List<Customer> c = CXECustomerService.Lookup(criteria);

			Assert.IsTrue( c.Count == count );
		}

		[Test]
		public void SaveCustomer()
		{
			long alloyId = 10291;
			Guid custG = SetupCustomer(alloyId, "mike", "dowd", DateTime.Today.AddYears(-25), "2839481", "", "taster");

			Customer c = new Customer();
			c.Id = alloyId;
			//c.Profile = new CustomerProfile { FirstName = "mike", LastName = "dowd", DOB = DateTime.Today.AddYears( -25 ), Phone1 = "2839481" };
			c.EmploymentDetails = new CustomerEmploymentDetails { Occupation = "taster" };

			//IdType idtype = new IdType { Country = "UNITED STATES", HasExpirationDate = true, Id = 110, Mask = "^[a-zA-Z]\\d{7}$", Name = "DRIVER'S LICENSE", rowguid = new Guid( "368FEFD9-CB33-4EA7-9186-06F98E8A1C68" ), State = "CALIFORNIA" };

			c.GovernmentId = new CustomerGovernmentId { Identification = "D345345", ExpirationDate = DateTime.Today.AddYears( 5 ) };

			//_custSvc.Save( c );
			CXECustomerService.Save(c);

			int countGovtId = (int)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select count(*) from tCustomerGovernmentIdDetails where CustomerPK='{0}'", custG ) );
			Assert.IsTrue( countGovtId == 1 );

			DateTime dtcreate = (DateTime)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select DTCreate from tCustomerGovernmentIdDetails where CustomerPK='{0}'", custG ) );
			Assert.IsTrue( dtcreate > DateTime.Today );

		}
	}
}
