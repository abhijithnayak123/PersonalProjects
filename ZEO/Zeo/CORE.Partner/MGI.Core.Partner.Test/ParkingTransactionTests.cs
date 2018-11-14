using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Spring.Data.Core;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Core.Partner.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
//using Moq;


namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class ParkingTransactionTests : AbstractPartnerTest
	{
		private long _customerId = 1000000000000010;
       // private string _agentId = "200000";
		private long _acctId = 1000000004;
		string TimezoneID = "Central Standard Time";
		private ICustomerService _custSvc;
		public ICustomerService CustomerService { set { _custSvc = value; } }

		private ICustomerSessionService _custSessionSvc;
		public ICustomerSessionService CustomerSessionService { set { _custSessionSvc = value; } }

		private IAgentSessionService _agtSessionSvc;
		public IAgentSessionService AgentSessionService { set { _agtSessionSvc = value; } }

		private ITransactionService<BillPay> _billpaySvc;
		public ITransactionService<BillPay> BillPayService { set { _billpaySvc = value; } }

		

		private CustomerSession CreateCustomerSession()
		{
			long agentSessionId = 1000004237;
            AgentSession agtSess = _agtSessionSvc.Lookup(agentSessionId);
            Customer customer = _custSvc.Lookup(_customerId);
			  
            CustomerSession customersession = _custSessionSvc.Create(agtSess, customer, false,  TimezoneID);
            Assert.IsTrue(customersession.Id > 0);
           // SetComplete();
            return customersession;
		}

		[Test]
		public void AddCartToCustomerSession()
		{
			CustomerSession sess = CreateCustomerSession();

			sess.AddShoppingCart("");

			Assert.IsTrue( sess.HasActiveShoppingCart() );

			_custSessionSvc.Save( sess );

			Guid cartGuid = (Guid)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select cartRowguid from tCustomerSessionShoppingCarts where customerSessionRowguid='{0}'", sess.rowguid ) );
           
            Assert.IsNotNull(sess.ActiveShoppingCart);
			Assert.IsTrue( cartGuid == sess.ActiveShoppingCart.rowguid );
            Assert.IsTrue(sess.ActiveShoppingCart.IsParked == false);
            Assert.IsTrue(sess.ActiveShoppingCart.Active);

		}


        [Test]
        public void AddParkingShoppingCartToCustomerSession()
        {
            CustomerSession sess = CreateCustomerSession();

            sess.AddShoppingCart("Pacific Standard Time");

            Assert.IsTrue(sess.HasActiveShoppingCart());

            _custSessionSvc.Save(sess);

            Guid cartGuid = (Guid)AdoTemplate.ExecuteScalar(CommandType.Text, string.Format("select cartRowguid from tCustomerSessionShoppingCarts where customerSessionRowguid='{0}'", sess.rowguid));

            Assert.IsTrue(cartGuid == sess.ActiveShoppingCart.rowguid);

            sess.AddParkingShoppingCart("Pacific Standard Time");

            _custSessionSvc.Save(sess);

          
            Assert.IsNotNull(sess.ActiveShoppingCart);
            Assert.IsNotNull(sess.ParkingShoppingCart);
          
            Assert.IsTrue(sess.ActiveShoppingCart.IsParked == false);
            Assert.IsTrue(sess.ActiveShoppingCart.Active);

            Assert.IsTrue(sess.ParkingShoppingCart.Active);
            Assert.IsTrue(sess.ParkingShoppingCart.IsParked);

        }


		[Test]
		public void AddBillPayTransactionToCart()
		{
            
			CustomerSession sess = CreateCustomerSession();

            sess.AddShoppingCart("Pacific Standard Time");

            _custSessionSvc.Save(sess);

			long bpId = 100000093;

			//SessionSetupHelper.SetupBillPayTxn( AdoTemplate, bpId, sess.Id, _acctId );

			BillPay bp = _billpaySvc.Lookup( bpId );           

			sess.ActiveShoppingCart.AddTransaction( bp );

			_custSessionSvc.Save( sess );

			Guid txnGuid = (Guid)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select txnRowguid from tShoppingCartTransactions where cartRowguid='{0}'", sess.ActiveShoppingCart.rowguid ) );

			Assert.IsTrue( txnGuid == bp.rowguid );
			Console.WriteLine( sess.ActiveShoppingCart.ShoppingCartTransactions.Count );

            //sess.ActiveShoppingCart.RemoveTransaction(bp);

            //_custSessionSvc.Save( sess );

            //int bpCount = (int)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select count(*) from tShoppingCartTransactions where cartRowguid='{0}'", sess.ActiveShoppingCart.rowguid ) );

            //Assert.IsTrue( bpCount == 0 );

            //Console.WriteLine(sess.ActiveShoppingCart.ShoppingCartTransactions.Count);

			int bpTxnCount = (int)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select count(*) from tTxn_BillPay where txnRowguid='{0}'", bp.rowguid ) );
			Assert.IsTrue( bpTxnCount==1 );			
		}


        [Test]
        public void ParkBillPayTransactionToCart()
        {

            CustomerSession sess = CreateCustomerSession();

            sess.AddShoppingCart("Pacific Standard Time");

            _custSessionSvc.Save(sess);

			long bpId = 1000000413;

        //    SessionSetupHelper.SetupBillPayTxn(AdoTemplate, bpId, sess.Id, _acctId);

            BillPay bp = _billpaySvc.Lookup(bpId);

            sess.ActiveShoppingCart.AddTransaction(bp);

            _custSessionSvc.Save(sess);

            Guid txnGuid = (Guid)AdoTemplate.ExecuteScalar(CommandType.Text, string.Format("select txnRowguid from tShoppingCartTransactions where cartRowguid='{0}'", sess.ActiveShoppingCart.rowguid));

            Assert.IsTrue(txnGuid == bp.rowguid);
            Console.WriteLine(sess.ActiveShoppingCart.ShoppingCartTransactions.Count);           

            int bpTxnCount = (int)AdoTemplate.ExecuteScalar(CommandType.Text, string.Format("select count(*) from tTxn_BillPay where txnRowguid='{0}'", bp.rowguid));
            Assert.IsTrue(bpTxnCount == 1);

            //Parking ShoppingCart

            if (sess.ParkingShoppingCart == null)
                sess.AddParkingShoppingCart("Pacific Standard Time");

            _custSessionSvc.Save(sess);         

            sess.ActiveShoppingCart.ParkTransaction(bp, sess.ParkingShoppingCart);

            _custSessionSvc.Update(sess);

           // SetComplete();

             txnGuid = (Guid)AdoTemplate.ExecuteScalar(CommandType.Text, string.Format("select cartRowguid from tShoppingCartTransactions where txnRowguid='{0}'", bp.rowguid));

             Assert.IsTrue(txnGuid == sess.ParkingShoppingCart.rowguid);

        }


        [Test]
        [ExpectedException]
        public void CannotParkBillPayTransactionToCart()
        {

            CustomerSession sess = CreateCustomerSession();

            sess.AddShoppingCart("Pacific Standard Time");

            _custSessionSvc.Save(sess);

			long bpId = 1000000014;

            SessionSetupHelper.SetupBillPayTxn(AdoTemplate, bpId, sess.Id, _acctId);

            BillPay bp = _billpaySvc.Lookup(bpId);

            sess.ActiveShoppingCart.AddTransaction(bp);

            _custSessionSvc.Save(sess);

            Guid txnGuid = (Guid)AdoTemplate.ExecuteScalar(CommandType.Text, string.Format("select txnRowguid from tShoppingCartTransactions where cartRowguid='{0}'", sess.ActiveShoppingCart.rowguid));

            Assert.IsTrue(txnGuid == bp.rowguid);
            Console.WriteLine(sess.ActiveShoppingCart.ShoppingCartTransactions.Count);

            int bpTxnCount = (int)AdoTemplate.ExecuteScalar(CommandType.Text, string.Format("select count(*) from tTxn_BillPay where txnRowguid='{0}'", bp.rowguid));
            Assert.IsTrue(bpTxnCount == 1);

            //Parking ShoppingCart - Without Parking Shopping Cart

            sess.ActiveShoppingCart.ParkTransaction(bp, sess.ParkingShoppingCart);
                      

        }
		
	
       // US1488 Unparking Transactions 

        [Test]
        [ExpectedException] // Customer Session Not found 2000002366
        public void Cannot_GetParkingTransactions()
        {

            CustomerSession customersession = _custSessionSvc.Lookup(2000002366);

            ShoppingCart shoppingCart =  _custSessionSvc.GetParkingShoppingCart(customersession);

            Assert.IsTrue(shoppingCart.ShoppingCartTransactions.Count > 0);            

        }


        [Test]
        // Customer Session found 1000002365 with Parking Transaction
        public void Can_GetParkingTransactions()
		{

			CustomerSession customersession = _custSessionSvc.Lookup(1000003528);

            ShoppingCart shoppingCart = _custSessionSvc.GetParkingShoppingCart(customersession);

            Assert.IsTrue(shoppingCart.ShoppingCartTransactions.Count > 0);
        }


        [Test]
        // Customer Session found 1000002366 without Parking shoppingCart
        public void CanGetSession_CannotGetparkingShoppingCart()
		{

			CustomerSession customersession = _custSessionSvc.Lookup(1000000037);

            ShoppingCart shoppingCart = _custSessionSvc.GetParkingShoppingCart(customersession);

            Assert.IsNull(shoppingCart);
        }



    }
}
