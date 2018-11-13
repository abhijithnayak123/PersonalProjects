using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.CXE.Impl;

using NUnit.Framework;
using Moq;

using Spring.Testing.NUnit;


namespace MGI.Core.CXE.Test
{
	[TestFixture]
	public class AccountServiceDBTest : CXETransactionalDBProviderSpringContextTests
	{
		private ICustomerService _custSvc;
		public ICustomerService CustomerService { set { _custSvc = value; } }

		private IAccountService _acctSvc;
		public IAccountService AccountService { set { _acctSvc = value; } }

		private Guid CreateCustomer(long alloyId)
		{
			Guid custG = Guid.NewGuid();
			AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tCustomers(rowguid,id,dtcreate) values('{0}',{1},getdate())", custG, alloyId));
			return custG;
		}

		private long AddAccount( Guid custGuid, int accountType )
		{
			return (long)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "insert into tCustomerAccounts( rowguid, Type, CustomerPK, DTCreate ) values(NEWID(), {0}, '{1}', GETDATE()); select CAST(SCOPE_IDENTITY() AS BIGINT);", accountType, custGuid ) );
		}

		[Test]
		public void CreateAccountTest()
		{
			// create a customer record
			Guid custG = CreateCustomer( 1099 );

			Customer c = _custSvc.Lookup( 1099 );

			Account acct = _acctSvc.AddCustomerCashAccount( c );
			Assert.IsTrue( acct.Type == (int)AccountTypes.Cash );

			Account bpAcct = _acctSvc.AddCustomerBillPayAccount( c );
			Assert.IsTrue( bpAcct.Type == (int)AccountTypes.BillPay );

			Account checkAcct = _acctSvc.AddCustomerCheckAccount( c );
			Assert.IsTrue( checkAcct.Type == (int)AccountTypes.Check );

			Account fundsAcct = _acctSvc.AddCustomerFundsAccount( c );
			Assert.IsTrue( fundsAcct.Type == (int)AccountTypes.Funds );

			Account moAcct = _acctSvc.AddCustomerMoneyOrderAccount( c );
			Assert.IsTrue( moAcct.Type == (int)AccountTypes.MoneyOrder );

			Account mtAcct = _acctSvc.AddCustomerMoneyTransferAccount( c );
			Assert.IsTrue( mtAcct.Type == (int)AccountTypes.MoneyTransfer );

			int accountCount = (int)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select count(*) from tCustomerAccounts where CustomerPK='{0}'", custG ) );
			Assert.IsTrue( accountCount == 6 );
		}

		[Test]
		public void LookupExistingAccount()
		{
			Guid custG = CreateCustomer( 1098 );
			long cashAcctId = AddAccount( custG, (int)AccountTypes.Cash );
			long bpAcctId = AddAccount( custG, (int)AccountTypes.BillPay );

			Customer c = _custSvc.Lookup( 1098 );
			Account cash = c.GetAccount( cashAcctId );
			Assert.IsTrue( cash.Type == (int)AccountTypes.Cash );
			Account bp = c.GetAccount( bpAcctId );
			Assert.IsTrue( bp.Type == (int)AccountTypes.BillPay );

			Account mt = c.GetAccount( 111 );
			Assert.IsNull( mt );
		}
	}
}
