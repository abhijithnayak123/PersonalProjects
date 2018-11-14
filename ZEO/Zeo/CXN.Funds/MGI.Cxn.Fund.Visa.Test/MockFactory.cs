using MGI.Common.DataAccess.Contract;
using MGI.Common.DataProtection.Contract;
using MGI.Cxn.Fund.Visa.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MGI.Cxn.Fund.Visa.Test
{
	internal class MockFactory
	{
		#region Private Member

		private static Moq.MockRepository _moqFactory;

		private List<Account> accounts = new List<Account>() { 
			new Account()
			{ 
				 CardNumber = "5645897823124557",
				 ZipCode = "90005",
				 Activated = true,
				 CardAliasId = "1234567891",
				 DTAccountClosed = new DateTime(2015, 09, 19),
				 Id = 100000001,
				 Address1 = "BAY",
				 Address2 = "Street",
				 FirstName = "Nitish",
				 LastName = "Biradar",
				 City = "Bangalore",
				 Country ="India",
				 DateOfBirth = new DateTime(1990, 05, 01),
				 SSN = "123456789",
				 State = "KA",
				 Phone = "125896312",
				 DTServerCreate = DateTime.Now 
			}
		};

		private List<MGI.Cxn.Fund.Data.TransactionHistory> transactionHistorys = new List<Fund.Data.TransactionHistory>() {
			new MGI.Cxn.Fund.Data.TransactionHistory()
			{ 
				ActualBalance = 100, 
				AvailableBalance = 90, 
				DeclineReason = string.Empty, 
				Location = "ABCD", 
				MerchantName = "ABCD", 
				PostedDateTime = new DateTime(), 
				TransactionAmount = 55, 
				TransactionDateTime = DateTime.Now, 
				TransactionDescription = "Load"
			}
		};

		private List<CardInfo> cardInformations = new List<CardInfo>() {
			new CardInfo() 
			{
 				 AliasId = 0000000111125454544, 
				 Balance = 100, 
				 CardIssueDate = new DateTime(2000, 10, 10), 
				 CurrencyCode = "US", 
				 ProxyId = "0000000000079614192", 
				 ExpirationMonth = 11, 
				 ExpirationYear = 2018, 
				 Status = "Active", 
				 SubClientNodeId = 1233, 
				 CardNumber = "5645897823134558"
			},
			new CardInfo(){
				AliasId = 0000000111125454545,
				Balance = 120,
				CardIssueDate = new DateTime(2001, 10, 10),
				CurrencyCode = "US",
				ProxyId = "0000000000079614193",
				ExpirationMonth = 11,
				ExpirationYear = 2018,
				Status = "Active",
				SubClientNodeId = 1245,
				CardNumber = "5645897823134552"
			},
			new CardInfo(){
				AliasId = 0000000111125454546,
				Balance = 120,
				CardIssueDate = new DateTime(2001, 10, 10),
				CurrencyCode = "US",
				ProxyId = "0000000000079614194",
				ExpirationMonth = 11,
				ExpirationYear = 2018,
				Status = "Active",
				SubClientNodeId = 1245,
				CardNumber = "5645897823134548"
			}
		};

		private List<Transaction> transactions = new List<Transaction>() { };
		//TO DO
		private Dictionary<long, string> collectionpsedoDDA = new Dictionary<long, string>() { 
			{0000000111125454544, "39900000000096242"},
			{0000000111125454545, "39900000000096242"},
			{0000000111125454546, "39900000000096242"}
		};

		#endregion

		#region Constructor

		MockFactory()
		{
			_moqFactory = new Moq.MockRepository(Moq.MockBehavior.Default);
		} 

		#endregion

		#region Public Methods

		public object CreateInstanceIIO()
		{
			var obj = _moqFactory.Create<MGI.Cxn.Fund.Visa.Contract.IIO>();

			obj.Setup(moq => moq.ReplaceCard(It.IsAny<long>(), It.IsAny<MGI.Cxn.Fund.Data.CardMaintenanceInfo>(), It.IsAny<Credential>())).Returns(true);

			obj.Setup(moq => moq.GetCardInfoByProxyId(It.IsAny<string>(), It.IsAny<Credential>())).Returns(
				(string proxyId, Credential _credential) =>
				{
					return cardInformations.Find(a => a.ProxyId == proxyId);
				});

			obj.Setup(moq => moq.GetPsedoDDAFromAliasId(It.IsAny<long>(), It.IsAny<Credential>())).Returns(
				(long aliasId, Credential _credential) => 
				{
					return collectionpsedoDDA[aliasId];
				});

			obj.Setup(moq => moq.GetBalance(It.IsAny<long>(), It.IsAny<Data.Credential>())).Returns(GetCardBalance());

			obj.Setup(moq => moq.CloseAccount(It.IsAny<long>(), It.IsAny<Credential>())).Returns(true);

			obj.Setup(moq => moq.UpdateCardStatus(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<Credential>())).Returns(true);

			obj.Setup(moq => moq.GetTransactionHistory(It.IsAny<MGI.Cxn.Fund.Data.TransactionHistoryRequest>(), It.IsAny<Credential>())).Returns(transactionHistorys);

			obj.Setup(moq => moq.IssueCard(It.IsAny<Account>(), It.IsAny<double>(), It.IsAny<CardInfo>(), It.IsAny<Credential>())).Returns(new CardPurchaseResponse() { AccountAliasId = 123456789, ConfirmationNumber = "1234567890123456" });

			obj.Setup(moq => moq.Load(It.IsAny<long>(), It.IsAny<double>(), It.IsAny<Credential>())).Returns(new LoadResponse() { ReloadAliasId = 123456, TransactionKey = 159159, TransationId = "100000002" });

			obj.Setup(moq => moq.Withdraw(It.IsAny<long>(), It.IsAny<double>(), It.IsAny<Credential>())).Returns(true);

			return obj.Object;
		}

		public object CreateInstanceOfVisaAccountRepo()
		{
			var obj = _moqFactory.Create<IRepository<Account>>();

			obj.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<Account, bool>>>())).Returns(
				new Func<Expression<Func<Account, bool>>, Account>(expr => accounts.Where(expr.Compile()).FirstOrDefault()));

			obj.Setup(moq => moq.Add(It.IsAny<Account>())).Returns(
				(Account account) =>
				{
					account.Id = 100000001 + (accounts.Count() + 1);
					accounts.Add(account);
					return accounts.Find(a => a.Id == account.Id);
				});

			obj.Setup(moq => moq.UpdateWithFlush(It.IsAny<Account>())).Returns(
				(Account account) =>
				{
					Account exitingAccount = accounts.Find(a => a.Id == account.Id);
					if (exitingAccount != null)
					{
						exitingAccount = account;
						accounts.Add(exitingAccount);
					}
					return true;
				});

			obj.Setup(moq => moq.Update(It.IsAny<Account>())).Returns(
				(Account account) =>
				{
					Account exitingAccount = accounts.Find(a => a.Id == account.Id);
					if (exitingAccount != null)
					{
						exitingAccount = account;
						accounts.Add(exitingAccount);
					}
					return true;
				});

			obj.Setup(moq => moq.SaveOrUpdate(It.IsAny<Account>())).Returns(
				(Account account) =>
				{
					Account exitingAccount = accounts.Find(a => a.Id == account.Id);
					if (exitingAccount != null)
					{
						exitingAccount = account;
						accounts.Add(exitingAccount);
					}
					else
					{
						account.Id = 100000001 + (accounts.Count() + 1);
						accounts.Add(account);
					}
					return true;
				});

			obj.Setup(moq => moq.Merge(It.IsAny<Account>())).Returns(
				(Account account) =>
				{
					Account exitingAccount = accounts.Find(a => a.Id == account.Id);
					if (exitingAccount != null)
					{
						exitingAccount = account;
						accounts.Add(exitingAccount);
					}
					return true;
				});

			obj.Setup(moq => moq.AddWithFlush(It.IsAny<Account>())).Returns(
				(Account account) =>
				{
					account.Id = 100000001 + (accounts.Count() + 1);
					account.Activated = true;
					accounts.Add(account);
					return accounts.Find(x => x.Id == account.Id);
				});

			obj.Setup(moq => moq.FilterBy(It.IsAny<Expression<Func<Account, bool>>>())).Returns(
				new Func<Expression<Func<Account, bool>>, IQueryable<Account>>(expr => accounts.Where(expr.Compile()).AsQueryable()));

			return obj.Object;
		}

		public object CreateInstanceOfVisaTransaction()
		{
			var obj = _moqFactory.Create<IRepository<Transaction>>();

			obj.Setup(x => x.Add(It.IsAny<Transaction>())).Returns(
				(Transaction transaction) =>
				{
					transaction.Id = 1000000000 + (transactions.Count() + 1);
					transactions.Add(transaction);
					return transactions.Where(a => a.Id == transaction.Id);
				});

			obj.Setup(moq => moq.UpdateWithFlush(It.IsAny<Transaction>())).Returns(
				(Transaction transaction) =>
				{
					var existingTransaction = transactions.Find(a => a.Id == transaction.Id);
					if (existingTransaction != null)
					{
						existingTransaction = transaction;
						transactions.Add(existingTransaction);
					}
					return true;
				});

			obj.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<Transaction, bool>>>())).Returns(
				new Func<Expression<Func<Transaction, bool>>, Transaction>(expr => transactions.Where(expr.Compile()).FirstOrDefault()));

			obj.Setup(moq => moq.AddWithFlush(It.IsAny<Transaction>())).Returns(
				(Transaction transaction) =>
				{
					transaction.Id = 1000000000 + (transactions.Count() + 1);
					transactions.Add(transaction);
					return (object)(transactions.Find(a => a.Id == transaction.Id));
				});

			obj.Setup(moq => moq.FilterBy(It.IsAny<Expression<Func<Transaction, bool>>>())).Returns(
				new Func<Expression<Func<Transaction, bool>>, IQueryable<Transaction>>(expr => transactions.Where(expr.Compile()).AsQueryable()));

			return obj.Object;
		}

		public object CreateInstanceOfCardClassRepo()
		{
			var obj = _moqFactory.Create<IRepository<CardClass>>();

			obj.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<CardClass, bool>>>())).Returns(new CardClass() { Class = 1, StateCode = "CA" });

			return obj.Object;
		}

		public object CreateInstanceOfShippingFeeRepo()
		{
			var obj = _moqFactory.Create<IRepository<VisaFee>>();

			obj.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<VisaFee, bool>>>())).Returns(new VisaFee() { Fee = 10 });

			return obj.Object;
		}

		public object CreateInstanceOfVisaCredentialRepo()
		{
			var obj = _moqFactory.Create<IRepository<Credential>>();

			obj.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<Credential, bool>>>())).Returns(new Credential());

			return obj.Object;
		}

		public object CreateInstanceOfDataProtection()
		{
			var obj = _moqFactory.Create<IDataProtectionService>();

			obj.Setup(moq => moq.Encrypt(It.IsAny<string>(), It.IsAny<int>())).Returns(
				(string cardNumber, int count) =>
				{
					return cardNumber;
				});

			obj.Setup(moq => moq.Decrypt(It.IsAny<string>(), It.IsAny<int>())).Returns(
				(string cardNumber, int count) =>
				{
					return cardNumber;
				});

			return obj.Object;
		}
		
		#endregion

		#region Private Methods

		private CardBalance GetCardBalance()
		{
			CardBalance cardBalance = new CardBalance()
			{
				AccountBalance = 120,
				Balance = 100,
				CardStatus = "Active"
			};
			return cardBalance;
		}
 
		#endregion
	}
}
