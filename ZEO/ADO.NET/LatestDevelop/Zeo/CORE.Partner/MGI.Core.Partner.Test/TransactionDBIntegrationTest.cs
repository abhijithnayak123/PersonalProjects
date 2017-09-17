using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;

using Iesi.Collections.Generic;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Core.Partner.Data.Fees;
using MGI.Core.Partner.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
//using Moq;

using Spring.Testing.NUnit;

namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class TransactionDBIntegrationTest : AbstractPartnerTest
	{
		private ICustomerSessionService _custSessionSvc;
		public ICustomerSessionService CustomerSessionService { set { _custSessionSvc = value; } }

		private ITransactionService<BillPay> _billpaySvc;
		public ITransactionService<BillPay> BillPayService { set { _billpaySvc = value; } }

		private ITransactionService<Check> _checkSvc;
		public ITransactionService<Check> CheckService { set { _checkSvc = value; } }

		private ITransactionService<Funds> _fundsSvc;
		public ITransactionService<Funds> FundsService { set { _fundsSvc = value; } }

		private ITransactionService<Cash> _cashSvc;
		public ITransactionService<Cash> CashService { set { _cashSvc = value; } }

		private IFeeService _feeSvc;
		public IFeeService FeeSvc { set { _feeSvc = value; } }

		//private ITransactionService<Transaction> _txnSvc;
		//public ITransactionService<Transaction> TransactionService { set { _txnSvc = value; } }
		MGI.Common.Util.MGIContext mgiContext = new Common.Util.MGIContext() { };
		private long _customerId = 101101;
		private long _acctId = 555;
		private string _agentId = "1234";

		private long CreateCustomerSession(int channelPartnerId)
		{
			long a = SessionSetupHelper.SetupAgentSession( AdoTemplate, _agentId, channelPartnerId );
			Guid c = SessionSetupHelper.CreateCustomerAndAccount( AdoTemplate, _customerId, _acctId, channelPartnerId );
			return SessionSetupHelper.SetupCustomerSession( AdoTemplate, c, _agentId );
		}

		[TestFixtureSetUp]
		public void Init()
		{
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
		}

		[Test]
		public void CreateTransaction()
		{
			long custSessId = CreateCustomerSession(27);

			Console.WriteLine( custSessId );
			CustomerSession custSess = _custSessionSvc.Lookup( custSessId );

			Account bpAcct = custSess.Customer.FindAccountByCXEId( _acctId );

			BillPay bp = new BillPay(20, 1, "whee", 123, 1, 123, 1, bpAcct, custSess);

			_billpaySvc.Create( bp );

			Assert.IsTrue( bp.rowguid != null && bp.rowguid != Guid.Empty );

			Console.WriteLine( bp.Id );
		}

		[Test]
		public void LookupExistingTransaction()
		{
			// create existing transaction to lookup
			long custSessId = CreateCustomerSession(27);
			SessionSetupHelper.SetupBillPayTxn( AdoTemplate, 124, custSessId, _acctId );

			BillPay bp = _billpaySvc.Lookup( 124 );

			Assert.IsTrue( bp.rowguid != null && bp.rowguid != Guid.Empty );
		}

		[Test]
		public void UpdateExistingTransactionStates()
		{
			// create existing transaction to lookup
			long custSessId = CreateCustomerSession(27);
			long bpTxnId = 124;
			SessionSetupHelper.SetupBillPayTxn( AdoTemplate, bpTxnId, custSessId, _acctId );

			_billpaySvc.UpdateStates( bpTxnId, 2, 2 );

			int cxeState = (int)AdoTemplate.ExecuteScalar( CommandType.Text, string.Format( "select CXEState from tTxn_BillPay where Id={0}", bpTxnId ) );

			Assert.IsTrue( cxeState == 2 );
		}

		[Test]
		public void UpdateTransaction()
		{
			// create existing transaction to lookup
			long custSessId = CreateCustomerSession(27);
			long fundsTxnId = 124;
			SessionSetupHelper.SetupFundsTxn(AdoTemplate, fundsTxnId, custSessId, _acctId);

			Funds f  = _fundsSvc.Lookup(124);
			Assert.IsTrue(f.rowguid != null && f.rowguid != Guid.Empty);


			f.BaseFee = decimal.Zero;
			f.DiscountApplied = decimal.Zero;
			f.Fee = decimal.Zero;
			f.DTTerminalLastModified = DateTime.Now;
            f.DTServerLastModified = DateTime.Now;

			_fundsSvc.Update(f);

			decimal fee = (decimal)AdoTemplate.ExecuteScalar(CommandType.Text, string.Format("select Fee from tTxn_Funds where Id={0}", fundsTxnId));

			Assert.IsTrue(fee == decimal.Zero);
		}

		[Test]
		public void VerifyDiscountName()
		{
			// create existing transaction to lookup
			long custSessId = CreateCustomerSession(27);
			long fundsTxnId = 124;
			SessionSetupHelper.SetupFundsTxn(AdoTemplate, fundsTxnId, custSessId, _acctId);

			Funds f = _fundsSvc.Lookup(124);
			Assert.IsTrue(f.rowguid != null && f.rowguid != Guid.Empty);

			//Assert.AreEqual(string.Empty, f.DiscountName);
			Assert.IsNull(f.DiscountName);
		}

		//[Test]
		//public void MakeUseOfGenericTransaction()
		//{
		//	long custSessId = CreateCustomerSession();

		//	Console.WriteLine(custSessId);
		//	CustomerSession custSess = _custSessionSvc.Lookup(custSessId);

		//	Account bpAcct = custSess.Customer.FindAccountByCXEId(_acctId);

		//	BillPay bp = new BillPay(20, 1, "whee", 123, 1, 123, 1, bpAcct, custSess);

		//	_txnSvc.Create(bp);

		//	SetComplete();
		//	EndTransaction();
		//}

		[Test]
		public void CreateDiscountTransaction()
		{
			long custSessId = CreateCustomerSession(33);

			SessionSetupHelper.SetupCheckTypeDiscount(adoTemplate, 33);

			Console.WriteLine(custSessId);
			CustomerSession custSess = _custSessionSvc.Lookup(custSessId);

			Account checkAcct = custSess.Customer.FindAccountByCXEId(_acctId);

			decimal amount = 200m;

			TransactionFee fee = _feeSvc.GetCheckFee(custSess, new List<Check>(), amount, 1, mgiContext);

			Check chk = new Check(amount, fee, "whee", 123, 1, 123, 1, checkAcct, custSess);

			_checkSvc.Create(chk);

			Assert.IsTrue(chk.rowguid != null && chk.rowguid != Guid.Empty);
			Console.WriteLine(chk.Id);

			Assert.AreEqual(1, chk.FeeAdjustments.Count);
			Assert.AreEqual(-1m, chk.FeeAdjustments[0].feeAdjustment.AdjustmentAmount);
		}
	}
}
