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
				new Trxn(){ 
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.NewGuid(),
					Id = 1000000020,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					CXEState = 1,
					CXNState = 1,
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

		#region PTNR transactions for Money transfer

		public List<PTNRData.Transactions.MoneyTransfer> sendMoneyTransactions = new List<PTNRData.Transactions.MoneyTransfer>()
		{
			new PTNRData.Transactions.MoneyTransfer(){
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
				new PTNRData.Transactions.MoneyTransfer(){
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
				new PTNRData.Transactions.MoneyTransfer(){
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
					CustomerSession = customerSessions.FirstOrDefault()
					
				},
				new PTNRData.Transactions.MoneyTransfer(){
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
				new PTNRData.Transactions.MoneyTransfer(){ // Added To run get receiver last transaction
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
					CXNState = 10
				},
				new PTNRData.Transactions.MoneyTransfer(){ // Added To run get receiver last transaction
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
					CXNState = 10
				},
				new PTNRData.Transactions.MoneyTransfer(){ 
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
				new PTNRData.Transactions.MoneyTransfer(){ 
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.NewGuid(),
					Id = 1000000010,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					CXEState = 10,
					CXNState = 10,
					TransferType = 2
				},
					new PTNRData.Transactions.MoneyTransfer(){ 
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.NewGuid(),
					Id = 1000000011,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					CXEState = 10,
					CXNState = 10,
					TransferType = 3
				},
				new PTNRData.Transactions.MoneyTransfer(){
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000002,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),
					Id = 1000000012,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					
				},
				new PTNRData.Transactions.MoneyTransfer(){
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.NewGuid(),
					Id = 1000000013,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault()
				}
		};

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

		public PTNRContract.ITransactionService<PTNRData.Transactions.MoneyTransfer> CreatePTNRMoneyTransfer()
		{
			Mock<PTNRContract.ITransactionService<PTNRData.Transactions.MoneyTransfer>> moneyTransferService = _moqRepository.Create<PTNRContract.ITransactionService<PTNRData.Transactions.MoneyTransfer>>();
			moneyTransferService.Setup(moq => moq.Create(It.IsAny<PTNRData.Transactions.MoneyTransfer>())).Callback(
				(PTNRData.Transactions.MoneyTransfer trxn) =>
				{
					trxn.rowguid = Guid.NewGuid();
					trxn.Id = 1000000000 + (sendMoneyTransactions.Count());
					sendMoneyTransactions.Add(trxn);
				});

			moneyTransferService.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long id) =>
				{
					return sendMoneyTransactions.Find(a => a.Id == id);
				});
			moneyTransferService.Setup(moq => moq.UpdateStates(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Callback(
				(long id, int cxeState, int cxnState, string desc) =>
				{
					var exitingTransaction = sendMoneyTransactions.Find(a => a.Id == id);
					if (exitingTransaction != null)
					{
						sendMoneyTransactions.Remove(exitingTransaction);
						exitingTransaction.CXEState = cxeState;
						exitingTransaction.CXNState = cxnState;
						exitingTransaction.Description = desc;
						sendMoneyTransactions.Add(exitingTransaction);
					}
				});
			moneyTransferService.Setup(moq => moq.UpdateFee(It.IsAny<long>(), It.IsAny<decimal>())).Callback(
				(long id, decimal fee) =>
				{
					var exitingTransaction = sendMoneyTransactions.Find(a => a.Id == id);
					if (exitingTransaction != null)
					{
						sendMoneyTransactions.Remove(exitingTransaction);
						exitingTransaction.Fee = fee;
						sendMoneyTransactions.Add(exitingTransaction);
					}
				});

			moneyTransferService.Setup(moq => moq.Update(It.IsAny<PTNRData.Transactions.MoneyTransfer>())).Callback(
				(PTNRData.Transactions.MoneyTransfer trxn) =>
				{
					var exitingTransaction = sendMoneyTransactions.Find(a => a.Id == trxn.Id);
					if (exitingTransaction != null)
					{
						sendMoneyTransactions.Remove(exitingTransaction);
						exitingTransaction = trxn;
						sendMoneyTransactions.Add(exitingTransaction);
					}
				});

			moneyTransferService.Setup(moq => moq.UpdateAmount(It.IsAny<long>(), It.IsAny<decimal>())).Callback(
				(long id, decimal amount) =>
				{
					var exitingTransaction = sendMoneyTransactions.Find(a => a.Id == id);
					if (exitingTransaction != null)
					{
						sendMoneyTransactions.Remove(exitingTransaction);
						exitingTransaction.Amount = amount;
						sendMoneyTransactions.Add(exitingTransaction);
					}
				});
			return moneyTransferService.Object;
		}

		public PTNRContract.ITransactionService<PTNRData.Transactions.MoneyOrder> CreatePTNRMoneyOrder()
		{
			List<PTNRData.Transactions.MoneyOrder> moneyOrders = new List<PTNRData.Transactions.MoneyOrder>() 
            {
                new PTNRData.Transactions.MoneyOrder()
                {
                    Id = 1000000000,
                    rowguid = Guid.Parse("03EDDFE6-9ECF-436F-8564-BF531DDF91DF"),
                    Amount = 100,
                           ConfirmationNumber = "12345",
                           CXEId = 1000000001,
                           CXNId = 1000000000000001,
                           Fee = 5,
                           DTServerCreate = DateTime.Now,
                           DTTerminalCreate = DateTime.Now,
                           Description = "Test",
                           Account = ptnrAccounts.FirstOrDefault(),
                           CustomerSession = customerSessions.FirstOrDefault(),
                    FeeAdjustments= new List<PTNRData.Transactions.TransactionFeeAdjustment>()
                    { 
                        new PTNRData.Transactions.TransactionFeeAdjustment(){
                            DTServerCreate=DateTime.Now,
                            DTServerLastModified=DateTime.Now,
                            DTTerminalCreate=DateTime.Now,
                            DTTerminalLastModified=DateTime.Now,
                            Id= 1000000000,
                            IsActive=true,
                            rowguid= Guid.NewGuid(),
                            
                            feeAdjustment = new PTNRData.Fees.FeeAdjustment()
                                {
                                    PromotionType = "Referral",
                                    
                                }
                        }
                    },
                },
                    new PTNRData.Transactions.MoneyOrder()
                    {
                    Id = 1000000001,
                    rowguid = Guid.NewGuid(),
                    Amount = 100,
                           ConfirmationNumber = "12345",
                           CXEId = 1000000001,
                           CXNId = 1000000000000001,
                           Fee = 5,
                           DTServerCreate = DateTime.Now,
                           DTTerminalCreate = DateTime.Now,
                           Description = "Test",
                           Account = ptnrAccounts.FirstOrDefault(),
                           CustomerSession = customerSessions.FirstOrDefault(),
                    FeeAdjustments= new List<PTNRData.Transactions.TransactionFeeAdjustment>()
                    { 
                        new PTNRData.Transactions.TransactionFeeAdjustment(){
                            DTServerCreate=DateTime.Now,
                            DTServerLastModified=DateTime.Now,
                            DTTerminalCreate=DateTime.Now,
                            DTTerminalLastModified=DateTime.Now,
                            Id= 1000000000,
                            IsActive=true,
                            rowguid= Guid.NewGuid(),
                            feeAdjustment = new PTNRData.Fees.FeeAdjustment()
                                {
                                    PromotionType = "Referral",
                                    
                                }
                        }
                    },
                   
                  
                    RoutingNumber = "12345678",
                    CheckNumber = "1245678",          
                         AccountNumber = "11111111",
                    
                },
                                    new PTNRData.Transactions.MoneyOrder()
                    {
                    Id = 1000000001,
                    rowguid = Guid.NewGuid(),
                    Amount = 100,
                           ConfirmationNumber = "12345",
                           CXEId = 1000000001,
                           CXNId = 1000000000000001,
                           Fee = 5,
                           DTServerCreate = DateTime.Now,
                           DTTerminalCreate = DateTime.Now,
                           Description = "Test",
                           Account = ptnrAccounts.FirstOrDefault(),
                           CustomerSession = customerSessions.FirstOrDefault(),
                    FeeAdjustments= new List<PTNRData.Transactions.TransactionFeeAdjustment>()
                    { 
                        new PTNRData.Transactions.TransactionFeeAdjustment(){
                            DTServerCreate=DateTime.Now,
                            DTServerLastModified=DateTime.Now,
                            DTTerminalCreate=DateTime.Now,
                            DTTerminalLastModified=DateTime.Now,
                            Id= 1000000000,
                            IsActive=true,
                            rowguid= Guid.NewGuid(),
                            feeAdjustment = new PTNRData.Fees.FeeAdjustment()
                                {
                                    PromotionType = "Referral",
                                    
                                }
                       }
                    },
                   
                  
                    RoutingNumber = "12345678",
                    CheckNumber = "1245678",          
                         AccountNumber = "11111111",
                    DiscountName="Test",
                },
                new PTNRData.Transactions.MoneyOrder()
                {
                    Id = 1000000002,
                    rowguid = Guid.NewGuid(),
                    Amount = 100,
                           ConfirmationNumber = "12345",
                           CXEId = 1000000001,
                           CXNId = 1000000000000001,
                           Fee = 5,
                           DTServerCreate = DateTime.Now,
                           DTTerminalCreate = DateTime.Now,
                           Description = "Test",
                           Account = ptnrAccounts.FirstOrDefault(),
                           CustomerSession = customerSessions.FirstOrDefault(),
                    FeeAdjustments= new List<PTNRData.Transactions.TransactionFeeAdjustment>()
                    { 
                        new PTNRData.Transactions.TransactionFeeAdjustment(){
                            DTServerCreate=DateTime.Now,
                            DTServerLastModified=DateTime.Now,
                            DTTerminalCreate=DateTime.Now,
                            DTTerminalLastModified=DateTime.Now,
                            Id= 1000000000,
                            IsActive=true,
                            rowguid= Guid.NewGuid(),
                            feeAdjustment = new PTNRData.Fees.FeeAdjustment()
                                {
                                    PromotionType = "Referral",
                                    
                                }
                        }
                    },
                },
            };

			Mock<PTNRContract.ITransactionService<PTNRData.Transactions.MoneyOrder>> PTNRMoneyOrder = _moqRepository.Create<PTNRContract.ITransactionService<PTNRData.Transactions.MoneyOrder>>();

			PTNRMoneyOrder.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long transactionid) =>
				{
					var existingMoneyOrder = moneyOrders.Find(x => x.Id == transactionid);
					if (existingMoneyOrder == null)
					{
						existingMoneyOrder = moneyOrders.FirstOrDefault();
					}
					return existingMoneyOrder;
				});

			PTNRMoneyOrder.Setup(moq => moq.GetAllForCustomer(It.IsAny<long>())).Returns(moneyOrders);

			PTNRMoneyOrder.Setup(moq => moq.Create(It.IsAny<PTNRData.Transactions.MoneyOrder>())).Callback(
				(PTNRData.Transactions.MoneyOrder moneyOrder) =>
				{
					moneyOrder.rowguid = Guid.NewGuid();
					moneyOrder.Id = 1000000000 + (moneyOrders.Count());
					moneyOrders.Add(moneyOrder);
				});

			PTNRMoneyOrder.Setup(moq => moq.GetAll(It.IsAny<long>())).Returns(
				(long customerId) =>
				{
					return moneyOrders;
				});

			PTNRMoneyOrder.Setup(moq => moq.Update(It.IsAny<PTNRData.Transactions.MoneyOrder>())).Callback(
				(PTNRData.Transactions.MoneyOrder moneyOrder) =>
				{
					var existingMoneyOrder = moneyOrders.Find(x => x.Id == moneyOrder.Id);
					if (existingMoneyOrder != null)
					{
						moneyOrders.Remove(existingMoneyOrder);
						existingMoneyOrder = moneyOrder;
						moneyOrders.Add(existingMoneyOrder);
					}
				});

			return PTNRMoneyOrder.Object;
		}

		
		public PTNRContract.ITransactionService<PTNRData.Transactions.Funds> CreatePTNRFundTransaction()
		{
			List<PTNRData.Transactions.Funds> ptnrFunds = new List<PTNRData.Transactions.Funds>() 
			{
 				new PTNRData.Transactions.Funds()
				{
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),
					Id = 1000000000,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					AddOnCustomerId = 1000000001,
				},
				new PTNRData.Transactions.Funds()
				{
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),
					Id = 1000000001,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					AddOnCustomerId = 1000000001,
				},
				new PTNRData.Transactions.Funds()
				{
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
					AddOnCustomerId = 1000000002,
				},
				new PTNRData.Transactions.Funds()
				{
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),
					Id = 1000000003,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					AddOnCustomerId = 1000000001,
				},
				new PTNRData.Transactions.Funds()
				{
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),
					Id = 1000000004,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					AddOnCustomerId = 1000000001,
					FundType = 2,
				},
				new PTNRData.Transactions.Funds()
				{
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 0,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.NewGuid(),
					Id = 1000000010,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					CXEState = 10,
					CXNState = 10,
				},
				new PTNRData.Transactions.Funds()
				{
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000001,
					CXNId = 1000000001,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),
					Id = 1000000005,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					AddOnCustomerId = 1000000000000002,
					FundType = 2,
				},
				new PTNRData.Transactions.Funds()
				{
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000002,
					CXNId = 1000000002,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),
					Id = 1000000006,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					FundType = 2,
				},
				new PTNRData.Transactions.Funds()
				{
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000003,
					CXNId = 1000000003,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),
					Id = 1000000007,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					FundType = 2,
				},
				new PTNRData.Transactions.Funds()
				{
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000004,
					CXNId = 1000000004,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),
					Id = 1000000008,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					FundType = 2,
				},
				new PTNRData.Transactions.Funds()
				{
					Amount = 100,
					ConfirmationNumber = "12345",
					CXEId = 1000000005,
					CXNId = 1000000005,
					Fee = 5,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"),
					Id = 1000000009,
					Description = "Test",
					Account = ptnrAccounts.FirstOrDefault(),
					CustomerSession = customerSessions.FirstOrDefault(),
					FundType = 2,
				},
			};

			Mock<PTNRContract.ITransactionService<PTNRData.Transactions.Funds>> PTNRFundsService = _moqRepository.Create<PTNRContract.ITransactionService<PTNRData.Transactions.Funds>>();

			PTNRFundsService.Setup(moq => moq.Create(It.IsAny<PTNRData.Transactions.Funds>())).Callback(
				(PTNRData.Transactions.Funds fund) => 
				{
					fund.Id = 1000000000 + (ptnrFunds.Count());
					fund.rowguid = Guid.NewGuid();
					ptnrFunds.Add(fund);
				});

			PTNRFundsService.Setup(moq => moq.GetAllForCustomer(It.IsAny<long>())).Returns(
				(long customerId) => 
				{
					return ptnrFunds;
				});

			PTNRFundsService.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long trxnId) => 
				{
					return ptnrFunds.Find(x => x.Id == trxnId);
				});

			PTNRFundsService.Setup(moq => moq.UpdateAmount(It.IsAny<long>(), It.IsAny<decimal>())).Callback(
				(long id, decimal amount) =>
				{
					var exitingTransaction = ptnrFunds.Find(a => a.Id == id);
					if (exitingTransaction != null)
					{
						ptnrFunds.Remove(exitingTransaction);
						exitingTransaction.Amount = amount;
						ptnrFunds.Add(exitingTransaction);
					}
				});

			PTNRFundsService.Setup(moq => moq.UpdateStates(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Callback(
				(long id, int cxeState, int cxnState, string desc) =>
				{
					var exitingTransaction = ptnrFunds.Find(a => a.Id == id);
					if (exitingTransaction != null)
					{
						ptnrFunds.Remove(exitingTransaction);
						exitingTransaction.CXEState = cxeState;
						exitingTransaction.CXNState = cxnState;
						exitingTransaction.Description = desc;
						ptnrFunds.Add(exitingTransaction);
					}
				});

			PTNRFundsService.Setup(moq => moq.UpdateTransactionDetails(It.IsAny<PTNRData.Transactions.Funds>())).Callback(
				(PTNRData.Transactions.Funds fund) => 
				{
					var exitingTransaction = ptnrFunds.Find(a => a.Id == fund.Id);
					if (exitingTransaction != null)
					{
						ptnrFunds.Remove(exitingTransaction);
						exitingTransaction = fund;
						ptnrFunds.Add(exitingTransaction);
					}
				});

			return PTNRFundsService.Object;
		}
	}
}
