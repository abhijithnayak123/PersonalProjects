using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.CXE.Data;
using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Impl;

using NHibernate;
using NHibernate.Context;

using Spring.Context;
using Spring.Context.Support;

using NUnit.Framework;

using BillPayStage = MGI.Core.CXE.Data.Transactions.Stage.BillPay;
using BillPayCommit = MGI.Core.CXE.Data.Transactions.Commit.BillPay;

using Account = MGI.Core.CXE.Data.Account;

//using Spring.Context;
//using Spring.Context.Support;
using Spring.Testing.NUnit;

namespace MGI.Core.CXE.Test.BillPay
{
	[TestFixture]
	public class BillPayServiceTest : AbstractTransactionalSpringContextTests
	{

		public IBillPayService CXEBillPayService { get; set; }
		public IAccountService CXEAccountService { get; set; }
		public ICustomerService CXECustomerService { get; set; }

		protected override string[] ConfigLocations
		{
			//get { return new string[] { "assembly://MGI.Core.CXE.Impl/MGI.Core.CXE.Impl/CXESpring.xml" }; }
			get { return new string[] { "assembly://MGI.Core.CXE.Test/MGI.Core.CXE.Test/CXETestSpring.xml" }; }
		}

		[SetUp]
		public void Setup()
		{
			//IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			//BillPayService = (IBillPayService)ctx.GetObject("BillPayService");
			//AccountService = (IAccountService)ctx.GetObject("AccountService");
			//session = (ISession)ctx.GetObject("session");
		}

		[Test]
		public void Can_CreateBillPayStage_Test()
		{
			long billPayId;

			BillPayStage billpaystage = new BillPayStage();

			Customer cust = CXECustomerService.Lookup(1000000000003880);
			Account account = cust.GetAccount(1000001018);
						
			billpaystage.rowguid = Guid.NewGuid();
			billpaystage.AccountNumber = "1234561830";
			billpaystage.Amount = 10;
			billpaystage.Fee = 1;
			billpaystage.DTTerminalCreate = DateTime.Now;
			billpaystage.DTTerminalLastModified = DateTime.Now;
			billpaystage.DTServerCreate = DateTime.Now;
			billpaystage.DTServerLastModified = DateTime.Now;
			billpaystage.Status = (int)TransactionStates.Authorized;
			billpaystage.ProductId = 100025908;
			billpaystage.ProductName = "REGIONAL ACCEPTANCE";
			billpaystage.Account = account;

			billPayId = CXEBillPayService.Create(billpaystage);

			Assert.IsNotNull(billPayId);
		}
				

		[Test]
		public void Can_CommitBillPayStage_Test()
		{
			long billPayId;

			BillPayStage billpaystage = new BillPayStage();

			Customer cust = CXECustomerService.Lookup(1000000000003880);
			Account account = cust.GetAccount(1000001018);

			billpaystage.rowguid = Guid.NewGuid();
			billpaystage.AccountNumber = "1234561830";
			billpaystage.Amount = 10;
			billpaystage.Fee = 1;
			billpaystage.DTTerminalCreate = DateTime.Now;
			billpaystage.DTTerminalLastModified = DateTime.Now;
			billpaystage.DTServerCreate = DateTime.Now;
			billpaystage.DTServerLastModified = DateTime.Now;
			billpaystage.Status = (int)TransactionStates.Authorized;
			billpaystage.ProductId = 100025908;
			billpaystage.ProductName = "REGIONAL ACCEPTANCE";
			billpaystage.Account = account;

			billPayId = CXEBillPayService.Create(billpaystage);

			CXEBillPayService.Commit(billPayId);

			BillPayCommit billpaycommit = new BillPayCommit();
			billpaycommit = CXEBillPayService.Get(billPayId);

			Assert.IsTrue(billpaycommit.Id == billPayId);

		}

		[Test]
		public void Can_GetBillPayCommit_Test()
		{
			long billPayId = 1000000004;

			BillPayCommit objBillPayCommit = new BillPayCommit();


			objBillPayCommit = CXEBillPayService.Get(billPayId);


			Assert.IsTrue(objBillPayCommit.Id == billPayId);
		}

		[Test]
		public void Cannot_CreateBillPayStage_Test()
		{
			long billPayId = 0;

			// CallSessionContext.Bind(session);

			try
			{

				BillPayStage billpaystage = new BillPayStage();

				Account CustomerAccount = new Account();
				// Account CustomerAccount = CXEAccountService.LookupCustomerBillPayAccount(555555512312, 2323);

				billpaystage.rowguid = Guid.NewGuid();
				billpaystage.AccountNumber = "51234567890";
				billpaystage.Amount = 10;
				billpaystage.Fee = 1;
				billpaystage.DTServerCreate = DateTime.Now;
                billpaystage.DTTerminalCreate = DateTime.Now;
				billpaystage.Status = (int)TransactionStates.Authorized;
				billpaystage.ProductId = 125;
				billpaystage.ProductName = "My Power Company";
				billpaystage.Account = CustomerAccount;

				billPayId = CXEBillPayService.Create(billpaystage);


			}
			catch
			{
				Assert.IsTrue(billPayId == 0);
			}
		}

		[Test]
		public void Cannot_UpdateBillPayStage_Test()
		{
			long billPayId = 10000000102321;



			try
			{


				CXEBillPayService.Update(billPayId, TransactionStates.Failed, null);


			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex.Message != null);
			}
		}

		[Test]
		public void Cannot_CommitBillPayStage_Test()
		{
			long billPayId = 1000000008213;


			try
			{

				CXEBillPayService.Commit(billPayId);

			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex.Message != null);
			}
		}

		[Test]
		public void Cannot_GetBillPayCommit_Test()
		{
			long billPayId = 1000000008343;
			BillPayCommit billpaycommit = new BillPayCommit();
			billpaycommit = CXEBillPayService.Get(billPayId);

			Assert.IsTrue(billpaycommit == null);
			
		}

        //[Test]
        //public void Cannot_GetBillPayStage_Test()
        //{
        //    long billPayId = 1000000008343;

        //    BillPayStage objBillPayStage = new BillPayStage();

        //    //    CallSessionContext.Bind(session);

        //    try
        //    {

        //        //  objBillPayStage = CXEBillPayService.GetStage(billPayId);

        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.IsTrue(objBillPayStage.Id != billPayId);
        //    }
        //}
	}
}
