using System;
using System.Collections.Generic;
using System.Linq;
using CXNMoneyTransfer = MGI.Cxn.MoneyTransfer.Contract;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Common.Util;
using MGI.Unit.Test.MockClasses;
using Moq;
using CXEContract = MGI.Core.CXE.Contract;
using MoneyTransferCommit = MGI.Core.CXE.Data.Transactions.Commit.MoneyTransfer;
using MoneyTransferStage = MGI.Core.CXE.Data.Transactions.Stage.MoneyTransfer;
using CXEData = MGI.Core.CXE.Data;
using AutoMapper;

namespace MGI.Unit.Test.MockData
{
	public class MockMoneyTransferProcessor : IntializMoqObject
    {
		public MockMoneyTransferProcessor()
		{
			Mapper.CreateMap<MoneyTransferStage, MoneyTransferCommit>();
		}

		#region CXN Money Transfer Service
		public CXNMoneyTransfer.IMoneyTransfer CreateInstanceOfCXNMoneyTransfer()
		{
			CXNMoneyTransferService = _moqRepository.Create<CXNMoneyTransfer.IMoneyTransfer>();

			CXNMoneyTransferService.Setup(moq => moq.GetDeliveryServices(It.IsAny<DeliveryServiceRequest>(), It.IsAny<MGIContext>())).Returns(
			(DeliveryServiceRequest request, MGIContext mgiContext) =>
			{
				return deliveryServices(request.CountryCode);
			});

			CXNMoneyTransferService.Setup(moq => moq.GetFee(It.IsAny<FeeRequest>(), It.IsAny<MGIContext>())).Returns(
			(FeeRequest feeRequest, MGIContext mgiContext) =>
			{
				return feeResponse;
			});

			CXNMoneyTransferService.Setup(moq => moq.Validate(It.IsAny<ValidateRequest>(), It.IsAny<MGIContext>())).Returns(
			(ValidateRequest validateRequest, MGIContext mgiContext) =>
			{
				return validateResponse;
			});

			CXNMoneyTransferService.Setup(moq => moq.Search(It.IsAny<SearchRequest>(), It.IsAny<MGIContext>())).Returns(
			(SearchRequest searchRequest, MGIContext mgiContext) =>
			{
				return searchResponse;
			});

			CXNMoneyTransferService.Setup(moq => moq.GetStatus(It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(
			(string confirmationNumber, MGIContext mgiContext) =>
			{
				return "Y";
			});

			CXNMoneyTransferService.Setup(moq => moq.GetRefundReasons(It.IsAny<ReasonRequest>(), It.IsAny<MGIContext>())).Returns(
			(ReasonRequest reasonRequest, MGIContext mgiContext) =>
			{
				return refundReasons;
			});

			CXNMoneyTransferService.Setup(moq => moq.Modify(It.IsAny<long>(), It.IsAny<MGIContext>())).Callback(
			(long transactionId, MGIContext mgiContext) =>
			{
			});

			CXNMoneyTransferService.Setup(moq => moq.Refund(It.IsAny<RefundRequest>(), It.IsAny<MGIContext>())).Returns(
			(RefundRequest refundRequest, MGIContext mgiContext) =>
			{
				return "true";
			});

			CXNMoneyTransferService.Setup(moq => moq.StageModify(It.IsAny<ModifyRequest>(), It.IsAny<MGIContext>())).Returns(
			(ModifyRequest modifyRequest, MGIContext mgiContext) =>
			{
				Transaction transaction = transactions.FirstOrDefault(x => x.TransactionID == modifyRequest.TransactionId.ToString());
				if (transaction != null)
				{
					ModifyResponse resp = new ModifyResponse();
					resp.CancelTransactionId = Convert.ToInt64(transaction.TransactionID);
					transactions.Remove(transaction);

					Transaction trx = new Transaction();
					trx.ReceiverFirstName = modifyRequest.FirstName;
					trx.ReceiverLastName = modifyRequest.LastName;
					trx.TestAnswer = modifyRequest.TestAnswer;
					trx.TestQuestion = modifyRequest.TestQuestion;
					trx.TransactionID = Convert.ToString(1000000001 + transactions.Count() + 1);
					transactions.Add(trx);
					resp.ModifyTransactionId = Convert.ToInt64(trx.TransactionID);
					return resp;
				}
				return new ModifyResponse() { ModifyTransactionId = 1000000001, CancelTransactionId = 1000000002 };
			});

			CXNMoneyTransferService.Setup(moq => moq.StageRefund(It.IsAny<RefundRequest>(), It.IsAny<MGIContext>())).Returns(
			(RefundRequest refundRequest, MGIContext mgiContext) =>
			{
				return 1000000001;
			});

			CXNMoneyTransferService.Setup(moq => moq.GetReceiverLastTransaction(It.IsAny<long>(), It.IsAny<MGIContext>())).Returns(transactions.FirstOrDefault());

			CXNMoneyTransferService.Setup(moq => moq.GetTransaction(It.IsAny<TransactionRequest>(), It.IsAny<MGIContext>())).Returns(
			(TransactionRequest transactionRequest, MGIContext mgiContext) =>
			{
				Transaction transaction = transactions.FirstOrDefault(x => x.TransactionID == transactionRequest.TransactionId.ToString());
				if (transaction == null)
				{
					transaction = new Cxn.MoneyTransfer.Data.Transaction()
					{
						Fee = 20,
						ChannelPartnerId = 33,
						MetaData = new Dictionary<string, object>(),
						Receiver = receivers[0],
						ConfirmationNumber = "1000",
						TransactionID = "1000000000",
						TransactionType = "1",
						OriginatingCountryCode = "USA"
					};
					transaction.MetaData.Add("ExpectedPayoutCity", "Test");
					transactions.Add(transaction);
				}
				return transaction;
			});

			CXNMoneyTransferService.Setup(moq => moq.UseGoldcard(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(true);

			CXNMoneyTransferService.Setup(moq => moq.AddAccount(It.IsAny<Account>(), It.IsAny<MGIContext>())).Returns(
			(Account account, MGIContext mgiContext) =>
			{
				account.Id = 1000000001 + (accounts.Count() + 1);
				accounts.Add(account);
				return account.Id;
			});

			CXNMoneyTransferService.Setup(moq => moq.UpdateAccount(It.IsAny<Account>(), It.IsAny<MGIContext>())).Returns(
			(Account account, MGIContext mgiContext) =>
			{
				var acct = accounts.FirstOrDefault(a => a.Id == account.Id);
				if (acct != null)
				{
					accounts.Remove(acct);
					accounts.Add(account);
				}
				return account.Id;
			});

			CXNMoneyTransferService.Setup(moq => moq.GetAccount(It.IsAny<long>(), It.IsAny<MGIContext>())).Returns(
			(long cxnAccountId, MGIContext mgiContext) =>
			{
				return accounts.FirstOrDefault();
			});

			CXNMoneyTransferService.Setup(moq => moq.Commit(It.IsAny<long>(), It.IsAny<MGIContext>())).Returns(true);

			CXNMoneyTransferService.Setup(moq => moq.SaveReceiver(It.IsAny<Receiver>(), It.IsAny<MGIContext>())).Returns(
			(Receiver receiver, MGIContext mgiContext) =>
			{
				Receiver rec = receivers.FirstOrDefault(r => r.Id == receiver.Id);
				if (rec != null)
					receivers.Remove(rec);
				receiver.Id = 1000000001 + (receivers.Count() + 1);
                receiver.Status = "Active";
				receivers.Add(receiver);
				return receiver.Id;
			});

			CXNMoneyTransferService.Setup(moq => moq.GetFrequentReceivers(It.IsAny<long>())).Returns(
			(long CustomerId) =>
			{
				return receivers;
			});

			CXNMoneyTransferService.Setup(moq => moq.GetReceivers(It.IsAny<long>(), It.IsAny<string>())).Returns(
			(long CustomerId, string lastName) =>
			{
				return receivers;
			});


			CXNMoneyTransferService.Setup(moq => moq.GetReceiver(It.IsAny<long>())).Returns(
			(long Id) =>
			{
				return receivers.FirstOrDefault();
			});

			CXNMoneyTransferService.Setup(moq => moq.GetReceiver(It.IsAny<long>(), It.IsAny<string>())).Returns(
			(long CustomerId, string fullName) =>
			{
				return receivers.FirstOrDefault();
			});

			CXNMoneyTransferService.Setup(moq => moq.IsSWBState(It.IsAny<string>())).Returns(true);

			CXNMoneyTransferService.Setup(moq => moq.WUCardEnrollment(It.IsAny<Account>(), It.IsAny<PaymentDetails>(), It.IsAny<MGIContext>())).Returns(
				new CardDetails() { AccountNumber = "1592634875", CounterId = "9900004", ForiegnRefNum = "1234", ForiegnSystemId = "12345678" });

			CXNMoneyTransferService.Setup(moq => moq.WUCardLookup(It.IsAny<long>(), It.IsAny<CardLookupDetails>(), It.IsAny<MGIContext>())).Returns(new CardLookupDetails() { 
				AccountNumber = "1592634875", sessionid = "1000000004", Sender = accounts.ToArray() });

			CXNMoneyTransferService.Setup(moq => moq.GetWUCardAccount(It.IsAny<long>())).Returns(true);

			CXNMoneyTransferService.Setup(moq => moq.DisplayWUCardAccountInfo(It.IsAny<long>())).Returns(accounts.FirstOrDefault());

			CXNMoneyTransferService.Setup(moq => moq.GetPastReceivers(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(true);

			CXNMoneyTransferService.Setup(moq => moq.GetCardInfo(It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(new CardInfo() { TotalPointsEarned = "55" });

			CXNMoneyTransferService.Setup(moq => moq.GetProviderAttributes(It.IsAny<AttributeRequest>(), It.IsAny<MGIContext>())).Returns(new List<Field>()
			{
				new Field(){ Label = "Test"}
			});

			return CXNMoneyTransferService.Object;
		} 
		#endregion

		#region CXE Money Transfer Service
		public CXEContract.IMoneyTransferService CreateInstanceOfCXEMoneyTransfer()
		{
			CXEMoneyTransferService = _moqRepository.Create<CXEContract.IMoneyTransferService>();

			CXEMoneyTransferService.Setup(moq => moq.Create(It.IsAny<MoneyTransferStage>())).Returns(
			(MoneyTransferStage moneytransfer) =>
			{
				moneytransfer.Id = 1000000001 + (stagedmoneyTransfers.Count() + 1);
				stagedmoneyTransfers.Add(moneytransfer);
				return stagedmoneyTransfers.Find(a => a.Id == moneytransfer.Id).Id;
			});


			CXEMoneyTransferService.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<CXEData.TransactionStates>(), It.IsAny<string>())).Callback(
			(long Id, CXEData.TransactionStates state, string timezone) =>
			{
				MoneyTransferStage stagedMoneyTransfer = stagedmoneyTransfers.Find(a => a.Id == Id);
				if (stagedMoneyTransfer != null)
				{
					stagedMoneyTransfer.Status = (int)state;
					stagedmoneyTransfers.Add(stagedMoneyTransfer);
				}
			});


			CXEMoneyTransferService.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<CXEData.TransactionStates>(), It.IsAny<string>(), It.IsAny<string>())).Callback(
			(long Id, CXEData.TransactionStates state, string timezone, string confirmationNumber) =>
			{
				MoneyTransferStage stagedMoneyTransfer = stagedmoneyTransfers.Find(a => a.Id == Id);
				if (stagedMoneyTransfer != null)
				{
					stagedMoneyTransfer.Status = (int)state;
					stagedMoneyTransfer.ConfirmationNumber = confirmationNumber;
					stagedmoneyTransfers.Add(stagedMoneyTransfer);
				}
			});


			CXEMoneyTransferService.Setup(moq => moq.Commit(It.IsAny<long>())).Callback(
			(long Id) =>
			{
				MoneyTransferStage stagedMoneyTransfer = stagedmoneyTransfers.Find(a => a.Id == Id);
				MoneyTransferCommit moneyTransferCommit = new MoneyTransferCommit();
				if (stagedMoneyTransfer != null)
				{
					moneyTransferCommit = AutoMapper.Mapper.Map<MoneyTransferCommit>(stagedMoneyTransfer);
					committedMoneyTransfers.Add(moneyTransferCommit);
				}
			});

			CXEMoneyTransferService.Setup(moq => moq.Get(It.IsAny<long>())).Returns(
			(long Id) =>
			{
				return committedMoneyTransfers.Find(a => a.Id == Id);

			});

			CXEMoneyTransferService.Setup(moq => moq.GetStage(It.IsAny<long>())).Returns(
			(long Id) =>
			{
				return stagedmoneyTransfers.Find(a => a.Id == Id);
			});

			CXEMoneyTransferService.Setup(moq => moq.Update(It.IsAny<MoneyTransferStage>(), It.IsAny<string>())).Callback(
			(MoneyTransferStage moneytransfer, string timezone) =>
			{
				MoneyTransferStage stagedMoneyTransfer = stagedmoneyTransfers.Find(a => a.Id == moneytransfer.Id);
				if (stagedMoneyTransfer != null)
				{
					stagedMoneyTransfer = moneytransfer;
					stagedmoneyTransfers.Add(stagedMoneyTransfer);
				}
			});

			return CXEMoneyTransferService.Object;
		} 
		#endregion
    }
}
