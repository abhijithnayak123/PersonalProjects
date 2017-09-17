using System;
using System.Linq;
using System.Threading;

using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Core.Partner.Impl;

using Moq;
using NUnit.Framework;

namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class CustomerSessionTests
	{
		private CustomerSessionServiceImpl _svc;
		private Mock<IRepository<CustomerSession>> _moqCustSessRepo;

		[TestFixtureSetUp]
		public void setup()
		{
			_moqCustSessRepo = new Mock<IRepository<CustomerSession>>();
			_svc = new CustomerSessionServiceImpl();
			_svc.CustomerSessionRepo = _moqCustSessRepo.Object;
		}

		[Test]
		public void NewCustomerSession()
		{
			CustomerSession customerSession = CreateNewSession();

			Assert.IsTrue( customerSession.DTStart>DateTime.MinValue );
		}

		//[Test]
		//public void Add

		private CustomerSession CreateNewSession()
		{
			Customer customer = new Customer { rowguid = Guid.NewGuid(), Id = 101, CXEId = 101, DTCreate = DateTime.Now };
			AgentSession agentSession = new AgentSession
			{
				rowguid = Guid.NewGuid(),
				AgentId = "dowbits",
				//Terminal = new Terminal { rowguid = Guid.NewGuid() }
			};

			_moqCustSessRepo.Setup( m => m.Add( It.IsAny<CustomerSession>() ) ).Returns( true );
			return _svc.Create( agentSession, customer );
		}

		[Test]
		public void HasNoShoppingCart()
		{
			CustomerSession sess = CreateNewSession();

			Assert.IsFalse( sess.HasActiveShoppingCart() );
		}

		[Test]
		public void HasActiveShoppingCart()
		{
			CustomerSession sess = CreateNewSession();

			sess.AddShoppingCart();

			Assert.IsTrue( sess.HasActiveShoppingCart() );
		}

		[Test]
		public void HasInactiveShoppingCart()
		{
			CustomerSession sess = CreateNewSession();

			sess.AddShoppingCart();

			sess.ActiveShoppingCart.CloseShoppingCart();

			Assert.IsFalse( sess.HasActiveShoppingCart() );
		}

		[Test]
		public void HasOneActiveAndOneInactiveCart()
		{
			CustomerSession sess = CreateNewSession();

			sess.AddShoppingCart();

			sess.ActiveShoppingCart.CloseShoppingCart();

			Thread.Sleep( 10 );

			sess.AddShoppingCart();

			Assert.IsTrue( sess.HasActiveShoppingCart() );
			Assert.IsTrue( sess.ActiveShoppingCart.DTCreate > sess.ShoppingCarts.First<ShoppingCart>( sc => !sc.Active ).DTCreate );
		}

		[Test]
		public void AddTransactionToCart()
		{
			CustomerSession sess = CreateNewSession();

			sess.AddShoppingCart();

			BillPay bp = new BillPay();

			sess.ActiveShoppingCart.AddTransaction( bp );
		}

		[Test]
		public void CloseSessionCart()
		{
			CustomerSession sess = CreateNewSession();

			sess.AddShoppingCart();

			sess.ActiveShoppingCart.CloseShoppingCart();

			Assert.IsFalse( sess.HasActiveShoppingCart() );
		}

		[Test]
		public void AddTransactionToClosedSesssionCart()
		{
			CustomerSession sess = CreateNewSession();

			sess.AddShoppingCart();
			ShoppingCart cart = sess.ActiveShoppingCart;

			sess.ActiveShoppingCart.CloseShoppingCart();

			Assert.Throws<Exception>( () => cart.AddTransaction( new BillPay() ) );

			Assert.IsFalse( sess.HasActiveShoppingCart() );
		}
	}
}
