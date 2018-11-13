using System.Collections.Generic;
using System.Linq;
using Moq;
using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;
using System;
using MGI.Common.Util;

namespace MGI.Unit.Test.MockClasses
{
	public class MockTransactions : IntializMoqObject
	{
		public MockTransactions() : base() { }

		#region Generic Code For Partner Transactions
		public object Create<Trxn>() where Trxn : PTNRData.Transactions.Transaction, new()
		{
			List<Trxn> transactions = new List<Trxn>() { 
				new Trxn(){
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.NewGuid(),
					Id = 1000000001,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
				},
				new Trxn(){
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),
					Id = 1000000002,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
				},
				new Trxn(){
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.NewGuid(),
					Id = 1000000003,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
				},
				new Trxn(){
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.NewGuid(),
					Id = 1000000004,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					CXEState = 10,
					CXNState = 10,
				},
				new Trxn(){ // Added To run get receiver last transaction
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.NewGuid(),
					Id = 1000000008,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					CXEState = 10,
					CXNState = 10,
				},
				new Trxn(){ // Added To run get receiver last transaction
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.NewGuid(),
					Id = 1000000009,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					CXEState = 10,
					CXNState = 10,
				},
				new Trxn(){ 
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.NewGuid(),
					Id = 2000000000,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					CXEState = 10,
					CXNState = 10,
				},
			};

			var PartnerTransactionsSvc = _moqRepository.Create<PTNRContract.ITransactionService<Trxn>>();

			PartnerTransactionsSvc.Setup(moq => moq.Create(It.IsAny<Trxn>())).Callback(
				(Trxn trxn) =>
				{
					trxn.Id = 1000000004 + (transactions.Count() + 1);
					trxn.rowguid = Guid.NewGuid();
					transactions.Add(trxn);
				});

			PartnerTransactionsSvc.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long id) =>
				{
					return transactions.Find(a => a.Id == id);
				});

			PartnerTransactionsSvc.Setup(moq => moq.GetAll(It.IsAny<long>())).Returns(
				(long customerId) =>
				{
					return transactions;
				});

			PartnerTransactionsSvc.Setup(moq => moq.GetAllForCustomer(It.IsAny<long>())).Returns(
				(long customerId) =>
				{
					return transactions;
				});

			PartnerTransactionsSvc.Setup(moq => moq.Update(It.IsAny<Trxn>())).Callback(
				(Trxn trxn) =>
				{
					var exitingTransaction = transactions.Find(a => a.Id == trxn.Id);
					if (exitingTransaction != null)
					{
						transactions.Remove(exitingTransaction);
						exitingTransaction = trxn;
						transactions.Add(exitingTransaction);
					}
				});

			PartnerTransactionsSvc.Setup(moq => moq.UpdateAmount(It.IsAny<long>(), It.IsAny<decimal>())).Callback(
				(long id, decimal amount) =>
				{
					var exitingTransaction = transactions.Find(a => a.Id == id);
					if (exitingTransaction != null)
					{
						transactions.Remove(exitingTransaction);
						exitingTransaction.Amount = amount;
						transactions.Add(exitingTransaction);
					}
				});

			PartnerTransactionsSvc.Setup(moq => moq.UpdateCXEStatus(It.IsAny<long>(), It.IsAny<int>())).Callback(
				(long id, int cxeStatus) =>
				{
					var exitingTransaction = transactions.Find(a => a.Id == id);
					if (exitingTransaction != null)
					{
						transactions.Remove(exitingTransaction);
						exitingTransaction.CXEState = cxeStatus;
						transactions.Add(exitingTransaction);
					}
				});

			PartnerTransactionsSvc.Setup(moq => moq.UpdateCXNStatus(It.IsAny<long>(), It.IsAny<int>())).Callback(
				(long id, int cxnState) =>
				{
					var exitingTransaction = transactions.Find(a => a.Id == id);
					if (exitingTransaction != null)
					{
						transactions.Remove(exitingTransaction);
						exitingTransaction.CXNState = cxnState;
						transactions.Add(exitingTransaction);
					}
				});

			PartnerTransactionsSvc.Setup(moq => moq.UpdateFee(It.IsAny<long>(), It.IsAny<decimal>())).Callback(
				(long id, decimal fee) =>
				{
					var exitingTransaction = transactions.Find(a => a.Id == id);
					if (exitingTransaction != null)
					{
						transactions.Remove(exitingTransaction);
						exitingTransaction.Fee = fee;
						transactions.Add(exitingTransaction);
					}
				});

			PartnerTransactionsSvc.Setup(moq => moq.UpdateStates(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Callback(
				(long id, int cxeState, int cxnState, string desc) =>
				{
					var exitingTransaction = transactions.Find(a => a.Id == id);
					if (exitingTransaction != null)
					{
						transactions.Remove(exitingTransaction);
						exitingTransaction.CXEState = cxeState;
						exitingTransaction.CXNState = cxnState;
						exitingTransaction.Description = desc;
						transactions.Add(exitingTransaction);
					}
				});

			PartnerTransactionsSvc.Setup(moq => moq.UpdateTransactionDetails(It.IsAny<Trxn>())).Callback(
				(Trxn trxn) =>
				{
					var exitingTransaction = transactions.Find(a => a.Id == trxn.Id);
					if (exitingTransaction != null)
					{
						transactions.Remove(exitingTransaction);
						exitingTransaction = trxn;
						transactions.Add(exitingTransaction);
					}
				});

			return PartnerTransactionsSvc.Object;
		} 
		#endregion

		public PTNRContract.IFeeService CreateInstanceOfFeeService()
		{
			Mock<PTNRContract.IFeeService> FeeService = _moqRepository.Create<PTNRContract.IFeeService>();

			FeeService.Setup(moq => moq.GetCheckFee(It.IsAny<PTNRData.CustomerSession>(), It.IsAny<List<PTNRData.Transactions.Check>>(), It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<MGIContext>())).Returns(transactionFee.FirstOrDefault());

			FeeService.Setup(moq => moq.GetBillPayFee(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<MGIContext>())).Returns(transactionFee.FirstOrDefault().NetFee);

			FeeService.Setup(moq => moq.GetFundsFee(It.IsAny<PTNRData.CustomerSession>(), It.IsAny<List<PTNRData.Transactions.Funds>>(), It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<MGIContext>())).Returns(transactionFee.FirstOrDefault());

			FeeService.Setup(moq => moq.GetMoneyOrderFee(It.IsAny<PTNRData.CustomerSession>(), It.IsAny<List<PTNRData.Transactions.MoneyOrder>>(), It.IsAny<decimal>(), It.IsAny<MGIContext>())).Returns(transactionFee.FirstOrDefault());

			return FeeService.Object;
		}

		public PTNRContract.ITransactionService<PTNRData.Transactions.Cash> CreatePTNRCashIn()
		{
			Mock<PTNRContract.ITransactionService<PTNRData.Transactions.Cash>> CashService = _moqRepository.Create<PTNRContract.ITransactionService<PTNRData.Transactions.Cash>>();

			List<PTNRData.Transactions.Cash> ptnrCash = new List<PTNRData.Transactions.Cash>() 
			{
 				new PTNRData.Transactions.Cash(){ CashType = 1, Id = 1000000000, CXEId = 1000000000}
			};

			CashService.Setup(moq => moq.Create(It.IsAny<PTNRData.Transactions.Cash>())).Callback(
				(PTNRData.Transactions.Cash trxn) =>
				{
					trxn.rowguid = Guid.NewGuid();
					trxn.Id = 1000000000 + (ptnrCash.Count());
					ptnrCash.Add(trxn);
				});

			CashService.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long id) =>
				{
					return ptnrCash.Find(a => a.Id == id);
				});

			return CashService.Object;
		}
	}
}
