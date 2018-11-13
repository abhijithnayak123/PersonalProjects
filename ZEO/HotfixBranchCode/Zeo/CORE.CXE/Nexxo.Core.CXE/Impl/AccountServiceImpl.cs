using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;

using MGI.Common.DataAccess.Contract;

namespace MGI.Core.CXE.Impl
{
	public class AccountServiceImpl : IAccountService
	{
		private IRepository<Account> _accountRepo;
		public IRepository<Account> AccountRepo
		{
			set { _accountRepo = value; }
		}

		public Account AddCustomerCashAccount( Customer customer )
		{
			return AddCustomerAccount( customer, (int)AccountTypes.Cash );
		}

		public Account AddCustomerFundsAccount( Customer customer )
		{
			return AddCustomerAccount( customer, (int)AccountTypes.Funds );
		}

		public Account AddCustomerCheckAccount( Customer customer )
		{
			return AddCustomerAccount( customer, (int)AccountTypes.Check );
		}

		public Account AddCustomerBillPayAccount( Customer customer )
		{
			return AddCustomerAccount( customer, (int)AccountTypes.BillPay );
		}

		public Account AddCustomerMoneyOrderAccount( Customer customer )
		{
			return AddCustomerAccount( customer, (int)AccountTypes.MoneyOrder );
		}

		public Account AddCustomerMoneyTransferAccount( Customer customer )
		{
			return AddCustomerAccount( customer, (int)AccountTypes.MoneyTransfer );
		}

		private Account AddCustomerAccount( Customer customer, int accountType )
		{
			Account acct = new Account
			{
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = customer.DTTerminalCreate,
				Customer = customer,
				Type = accountType
			};

			_accountRepo.AddWithFlush( acct );

			return acct;
		}
	}
}
