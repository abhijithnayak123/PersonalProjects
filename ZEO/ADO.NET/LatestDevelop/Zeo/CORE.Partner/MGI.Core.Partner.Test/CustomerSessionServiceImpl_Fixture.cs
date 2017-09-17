using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Core.Partner.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
using Spring.Data.Generic;
using Spring.Data.Core;
using MGI.Core.Partner.Test;
//using Moq;


namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class CustomerSessionDBIntegrationTests : AbstractPartnerTest
	{
		private long _customerId = 1000000000000200;
		private string _agentId = "500051";
		private long _acctId = 1000001025;

		private ICustomerService _custSvc;
		public ICustomerService CustomerService { set { _custSvc = value; } }

		private ICustomerSessionService _custSessionSvc;
		public ICustomerSessionService CustomerSessionService { set { _custSessionSvc = value; } }

		private IAgentSessionService _agtSessionSvc;
		public IAgentSessionService AgentSessionService { set { _agtSessionSvc = value; } }

		private ITransactionService<BillPay> _billpaySvc;
		public ITransactionService<BillPay> BillPayService { set { _billpaySvc = value; } }

		[Test]
		public void StartCustomerSession()
		{
			CustomerSession sess = CreateCustomerSession();

			Assert.IsTrue( sess.Id > 0 );
		}

		[Test]
		public void LookupCustomerSession()
		{
			//CustomerSession sess = CreateCustomerSession();

			//Console.WriteLine( sess.Id );

			CustomerSession lookupSess = _custSessionSvc.Lookup(1000000004);

			Assert.IsTrue( lookupSess.Id > 0 );
		}

		private CustomerSession CreateCustomerSession()
		{
			string TimezoneID = "";
			long agentSessionId = SessionSetupHelper.SetupAgentSession( AdoTemplate, _agentId, 34 );
			//SessionSetupHelper.CreateCustomerAndAccount( AdoTemplate, _customerId, _acctId, 34);

			AgentSession agtSess = _agtSessionSvc.Lookup( agentSessionId );
			Customer customer = _custSvc.Lookup( _customerId );			
			CustomerSession sess = _custSessionSvc.Create(agtSess,customer,true,TimezoneID);
			return sess;
		}

		[Test]
		public void AddCartToCustomerSession()
		{
			CustomerSession sess = CreateCustomerSession();

			sess.AddShoppingCart("");

			Assert.IsTrue( sess.HasActiveShoppingCart() );

			_custSessionSvc.Save( sess );

			Guid cartGuid = (Guid)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select cartRowguid from tCustomerSessionShoppingCarts where customerSessionRowguid='{0}'", sess.rowguid ) );

			Assert.IsTrue( cartGuid == sess.ActiveShoppingCart.rowguid );
		}

		[Test]
		public void AddTransactionToCart()
		{
			CustomerSession sess = CreateCustomerSession();

			sess.AddShoppingCart("");

			long bpId = 125;

			SessionSetupHelper.SetupBillPayTxn( AdoTemplate, bpId, sess.Id, _acctId );

			BillPay bp = _billpaySvc.Lookup( bpId );

			sess.ActiveShoppingCart.AddTransaction( bp );

			_custSessionSvc.Save( sess );

			Guid txnGuid = (Guid)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select txnRowguid from tShoppingCartTransactions where cartRowguid='{0}'", sess.ActiveShoppingCart.rowguid ) );

			Assert.IsTrue( txnGuid == bp.rowguid );
			Console.WriteLine( sess.ActiveShoppingCart.ShoppingCartTransactions.Count );

			sess.ActiveShoppingCart.RemoveTransaction(bp);

			_custSessionSvc.Save( sess );

			int bpCount = (int)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select count(*) from tShoppingCartTransactions where cartRowguid='{0}'", sess.ActiveShoppingCart.rowguid ) );

			Assert.IsTrue( bpCount == 1 );

			Console.WriteLine(sess.ActiveShoppingCart.ShoppingCartTransactions.Count);

			int bpTxnCount = (int)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select count(*) from tTxn_BillPay where txnRowguid='{0}'", bp.rowguid ) );
			Assert.IsTrue( bpTxnCount==1 );
			//SetComplete();
			//EndTransaction();
		}

		[Test]
		public void CloseCart()
		{
			CustomerSession sess = CreateCustomerSession();

			sess.AddShoppingCart("");

			_custSessionSvc.Save( sess );
			Guid cartGuid = sess.ActiveShoppingCart.rowguid;
			Console.WriteLine( cartGuid );

			sess.ActiveShoppingCart.CloseShoppingCart();

			_custSessionSvc.Save( sess );

			bool active = (bool)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select active from tShoppingCarts where cartRowguid='{0}'", cartGuid ) );

			Assert.IsFalse( active );
		}

		[Test]
		public void savecardpresent()
		{
			long _customerId = 1000000000000200;
			long agentSessionId = 1000003584;
			AgentSession agtSess = _agtSessionSvc.Lookup(agentSessionId);
			Customer customer = _custSvc.Lookup(_customerId);
			CustomerSession sess = _custSessionSvc.Create(agtSess, customer, false,agtSess.Terminal.Location.TimezoneID);			
			Assert.IsTrue(sess.Id > 0);
			//SetComplete();
		}
	}
}
