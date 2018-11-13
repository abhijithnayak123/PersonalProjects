using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using MGI.Biz.Partner.Impl;
using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Contract;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;

using pTransaction = MGI.Core.Partner.Data.Transactions.Transaction;
using pCheck = MGI.Core.Partner.Data.Transactions.Check;
using pFunds = MGI.Core.Partner.Data.Transactions.Funds;
using pBillPay = MGI.Core.Partner.Data.Transactions.BillPay;
using pMoneyOrder = MGI.Core.Partner.Data.Transactions.MoneyOrder;
using pMoneyTransfer = MGI.Core.Partner.Data.Transactions.MoneyTransfer;
using pCash = MGI.Core.Partner.Data.Transactions.Cash;

//using Moq;

namespace MGI.Biz.Partner.Test
{
    [TestFixture]
    public class ParkingShoppingCartTests : AbstractPartnerTest 
    {
		private long _customerId = 1000000000003250;
		//private string _agentId = "500003";
		//private long _acctId = 1000000000003250;
		string TimezoneID = "";
        private ICustomerSessionService _csSvc;
        public ICustomerSessionService CustomerSessionService { set { _csSvc = value; } }

        private MGI.Core.Partner.Contract.ITransactionService<pCheck> _checkTxnSvc;
        public MGI.Core.Partner.Contract.ITransactionService<pCheck> CheckTransactionService { set { _checkTxnSvc = value; } }

        //private Core.Partner.Contract.ITransactionService<pFunds> _fundsTxnSvc;
        //public Core.Partner.Contract.ITransactionService<pFunds> FundsTransactionService { set { _fundsTxnSvc = value; } }

        //private Core.Partner.Contract.ITransactionService<pBillPay> _billPayTxnSvc;
        //public Core.Partner.Contract.ITransactionService<pBillPay> BillPayTransactionService { set { _billPayTxnSvc = value; } }

        //private Core.Partner.Contract.ITransactionService<pMoneyOrder> _moneyOrderTxnSvc;
        //public Core.Partner.Contract.ITransactionService<pMoneyOrder> MoneyOrderTransactionService { set { _moneyOrderTxnSvc = value; } }

        //private Core.Partner.Contract.ITransactionService<pMoneyTransfer> _moneyTransferTxnSvc;
        //public Core.Partner.Contract.ITransactionService<pMoneyTransfer> MoneyTransferTransactionService { set { _moneyTransferTxnSvc = value; } }

        //private Core.Partner.Contract.ITransactionService<pCash> _cashTxnSvc;
        //public Core.Partner.Contract.ITransactionService<pCash> CashTransactionService { set { _cashTxnSvc = value; } }

        private MGI.Core.Partner.Contract.IShoppingCartService _scSvc;
        public MGI.Core.Partner.Contract.IShoppingCartService ShoppingCartSvc { set { _scSvc = value; } }

        private IAgentSessionService _agtSessionSvc;
        public IAgentSessionService AgentSessionService { set { _agtSessionSvc = value; } }

        private ICustomerService _custSvc;
        public ICustomerService CustomerService { set { _custSvc = value; } }

        
        private CustomerSession CreateCustomerSession()
        {
			long agentSessionId = 1000005382;
            AgentSession agtSess = _agtSessionSvc.Lookup(agentSessionId);
            Customer customer = _custSvc.Lookup(_customerId);
            CustomerSession customersession = _csSvc.Create(agtSess, customer, false,TimezoneID);
            Assert.IsTrue(customersession.Id > 0);            
            return customersession;
        }

        [Test]
        public void GetCustomerSession()
        {
            CustomerSession cs = CreateCustomerSession();
            Assert.IsNotNull(cs);
        }


       
    }
}
