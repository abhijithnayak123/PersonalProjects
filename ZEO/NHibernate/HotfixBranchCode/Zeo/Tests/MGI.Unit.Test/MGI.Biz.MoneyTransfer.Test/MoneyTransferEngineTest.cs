using NUnit.Framework;
using MGI.Biz.MoneyTransfer.Contract;
using MGI.Unit.Test;
using Moq;
using MGI.Biz.MoneyTransfer.Data;
using MGI.Common.Util;
using CXNData = MGI.Cxn.MoneyTransfer.Data;
using CXEData = MGI.Core.CXE.Data;
using System.Collections.Generic;

namespace MGI.Biz.MoneyTransfer.Test
{
    [TestFixture]
    public class MoneyTransferEngineTest : BaseClass_Fixture
    {
		public IMoneyTransferEngine BIZMoneyTransferEngine { get; set; }
		
		[Test]
		public void Can_Get_Fee()
		{
			long customerSessionId = 1000000004;
			FeeRequest feeRequest = new FeeRequest() { TransactionId = 1000000001 };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			FeeResponse feeResponse = BIZMoneyTransferEngine.GetFee(customerSessionId, feeRequest, mgiContext);

			Assert.IsNotNull(feeResponse);
			CXNMoneyTransferService.Verify(moq => moq.GetFee(It.IsAny<CXNData.FeeRequest>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_Fee_WithOut_TransactionId()
		{
			long customerSessionId = 1000000004;
			FeeRequest feeRequest = new FeeRequest() { };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			FeeResponse feeResponse = BIZMoneyTransferEngine.GetFee(customerSessionId, feeRequest, mgiContext);

			Assert.IsNotNull(feeResponse);
			CXNMoneyTransferService.Verify(moq => moq.GetFee(It.IsAny<CXNData.FeeRequest>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_IsSWBState()
		{
			long customerSessionId = 1000000004;
			string stateCode = "CA";
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			bool status = BIZMoneyTransferEngine.IsSWBState(customerSessionId, stateCode, mgiContext);

			Assert.AreEqual(status, true);
			CXNMoneyTransferService.Verify(moq => moq.IsSWBState(It.IsAny<string>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Validate_Money_Transfer()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			ValidateRequest validateRequest = new ValidateRequest() { Amount = 100, TransactionId = 1000000004, TransferType = TransferType.SendMoney };

			ValidateResponse validateResponse = BIZMoneyTransferEngine.Validate(customerSessionId, validateRequest, mgiContext);

			Assert.IsNotNull(validateResponse);

			CXNMoneyTransferService.Verify(moq => moq.GetTransaction(It.IsAny<CXNData.TransactionRequest>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
			CXEMoneyTransferService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<CXEData.TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Validate_Receive_Money_Transfer()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			ValidateRequest validateRequest = new ValidateRequest() { Amount = 100, TransactionId = 1000000003, TransferType = TransferType.ReceiveMoney };

			ValidateResponse validateResponse = BIZMoneyTransferEngine.Validate(customerSessionId, validateRequest, mgiContext);

			Assert.IsNotNull(validateResponse);

			CXNMoneyTransferService.Verify(moq => moq.GetTransaction(It.IsAny<CXNData.TransactionRequest>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
			CXEMoneyTransferService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<CXEData.TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Commit_Money_Transfer()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF", SMTrxType = "Release" };
			long ptrnTransactionId = 1000000004;

			int id = BIZMoneyTransferEngine.Commit(customerSessionId, ptrnTransactionId, mgiContext);

			Assert.AreEqual(id, (int)CXEData.TransactionStates.Committed);
			CXEMoneyTransferService.Verify(moq => moq.Commit(It.IsAny<long>()), Times.Exactly(1));
			CXEMoneyTransferService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<CXEData.TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());
			CXNMoneyTransferService.Verify(moq => moq.Commit(It.IsAny<long>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Add_Receiver()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			Receiver receiver = new Receiver() { };

			long receiverId = BIZMoneyTransferEngine.AddReceiver(customerSessionId, receiver, mgiContext);

			Assert.AreNotEqual(receiverId, 0);
		}

		[Test]
		public void Can_Edit_Receiver()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			Receiver receiver = new Receiver() { Id = 1000000000 };

			long receiverId = BIZMoneyTransferEngine.EditReceiver(customerSessionId, receiver, mgiContext);

			Assert.AreNotEqual(receiverId, 0);
		}

		[Test]
		public void Can_Get_ReceiverById()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			long id = 1000000000;

			Receiver receiver = BIZMoneyTransferEngine.GetReceiver(customerSessionId, id, mgiContext);

			Assert.IsNotNull(receiver);
			CXNMoneyTransferService.Verify(moq => moq.GetReceiver(It.IsAny<long>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_ReceiverByName()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			string receiverFullName = "John Smith";

			Receiver receiver = BIZMoneyTransferEngine.GetReceiver(customerSessionId, receiverFullName, mgiContext);

			Assert.IsNotNull(receiver);
			CXNMoneyTransferService.Verify(moq => moq.GetReceiver(It.IsAny<long>(), It.IsAny<string>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_All_Receiver_By_LastName()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			string receiverLastName = "Smith";

			List<Receiver> receivers = BIZMoneyTransferEngine.GetReceivers(customerSessionId, receiverLastName, mgiContext);

			Assert.AreNotEqual(receivers.Count, 0);
			CXNMoneyTransferService.Verify(moq => moq.GetReceivers(It.IsAny<long>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_All_Active_Receiver()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			string receiverLastName = "Smith";

			List<Receiver> receivers = BIZMoneyTransferEngine.GetReceivers(customerSessionId, receiverLastName, mgiContext);

			Assert.AreNotEqual(receivers.Count, 0);
			CXNMoneyTransferService.Verify(moq => moq.GetReceivers(It.IsAny<long>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_Frequent_Receiver()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			List<Receiver> receivers = BIZMoneyTransferEngine.GetFrequentReceivers(customerSessionId, mgiContext);

			Assert.AreNotEqual(receivers.Count, 0);
			CXNMoneyTransferService.Verify(moq => moq.GetFrequentReceivers(It.IsAny<long>()), Times.Exactly(1));
		}

		[Test]
		public void  Can_Get_Active_Receiver()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			string lastName = "Biradar";

			List<Receiver> receivers = BIZMoneyTransferEngine.GetActiveReceivers(customerSessionId,lastName, mgiContext);

			Assert.AreNotEqual(receivers.Count, 0);
		}

		[Test]
		public void Can_Get_Money_Transfer_Transaction()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			TransactionRequest transactionRequest = new TransactionRequest() { CXNTransactionId = 1000000004 };

			MoneyTransferTransaction mtTransaction = BIZMoneyTransferEngine.Get(customerSessionId, transactionRequest, mgiContext);

			Assert.IsNotNull(mtTransaction);
			CXNMoneyTransferService.Verify(moq => moq.GetTransaction(It.IsAny<CXNData.TransactionRequest>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Get_Receiver_Last_Transaction()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			long id = 1000000000;

			MoneyTransferTransaction mtTransaction = BIZMoneyTransferEngine.GetReceiverLastTransaction(customerSessionId, id, mgiContext);

			Assert.IsNotNull(mtTransaction);
			CXNMoneyTransferService.Verify(moq => moq.GetReceiverLastTransaction(It.IsAny<long>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Update_WUAccount()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF", ChannelPartnerId = 34 };
			string wuCardNumber = "1000000000";

			bool status = BIZMoneyTransferEngine.UpdateAccount(customerSessionId, wuCardNumber, mgiContext);

			Assert.True(status);
			CXNMoneyTransferService.Verify(moq => moq.UseGoldcard(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Receive_Money_Transaction()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			ReceiveMoneyRequest receiveMoneyRequest = new ReceiveMoneyRequest() { };

			MoneyTransferTransaction mtTransaction = BIZMoneyTransferEngine.Get(customerSessionId, receiveMoneyRequest, mgiContext);

			Assert.IsNotNull(mtTransaction);
			CXNMoneyTransferService.Verify(moq => moq.GetTransaction(It.IsAny<CXNData.TransactionRequest>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
			CXEMoneyTransferService.Verify(moq => moq.Create(It.IsAny<CXEData.Transactions.Stage.MoneyTransfer>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_WU_Card_Enrollment()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			PaymentDetails paymentDetails = new PaymentDetails() { };

			CardDetails cardDetails = BIZMoneyTransferEngine.WUCardEnrollment(customerSessionId, paymentDetails, mgiContext);

			Assert.IsNotNull(cardDetails);
			CXNMoneyTransferService.Verify(moq => moq.WUCardEnrollment(It.IsAny<CXNData.Account>(), It.IsAny<CXNData.PaymentDetails>(), It.IsAny<MGIContext>()), Times.Exactly(1));
			CXNMoneyTransferService.Verify(moq => moq.UpdateAccount(It.IsAny<CXNData.Account>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_WU_Card_LookUp()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			CardLookupDetails cardLookupDetails = new CardLookupDetails() { };

			CardLookupDetails cardLookupDetail = BIZMoneyTransferEngine.WUCardLookup(customerSessionId, cardLookupDetails, mgiContext);

			Assert.IsNotNull(cardLookupDetail);
			CXNMoneyTransferService.Verify(moq => moq.WUCardLookup(It.IsAny<long>(), It.IsAny<CXNData.CardLookupDetails>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_WUCardAccount()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			bool status = BIZMoneyTransferEngine.GetAccount(customerSessionId, mgiContext);

			Assert.True(status);
			CXNMoneyTransferService.Verify(moq => moq.GetWUCardAccount(It.IsAny<long>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_WUCardAccountInfo()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			Account account = BIZMoneyTransferEngine.DisplayWUCardAccountInfo(customerSessionId, mgiContext);

			Assert.IsNotNull(account);
			CXNMoneyTransferService.Verify(moq => moq.DisplayWUCardAccountInfo(It.IsAny<long>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Cancel_MoneyTrasfer()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			long transactionId = 1000000004;

			BIZMoneyTransferEngine.Cancel(customerSessionId, transactionId, mgiContext);
			CXEMoneyTransferService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<CXEData.TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Add_Past_Receiver()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF", ProductType = "SendMoney" };
			string cardNumber = "";

			BIZMoneyTransferEngine.AddPastReceivers(customerSessionId, cardNumber, mgiContext);

			CXNMoneyTransferService.Verify(moq => moq.GetPastReceivers(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Card_Info()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };

			CardInfo cardInfo = BIZMoneyTransferEngine.GetCardInfo(customerSessionId, mgiContext);

			Assert.IsNotNull(cardInfo);
			CXNMoneyTransferService.Verify(moq=>moq.GetCardInfo(It.IsAny<string>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Money_Transfer_Status()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			string confirmationNumber = "";

			string status = BIZMoneyTransferEngine.GetStatus(customerSessionId, confirmationNumber, mgiContext);

			Assert.AreEqual(status, "Y");
			CXNMoneyTransferService.Verify(moq => moq.GetStatus(It.IsAny<string>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Search_Money_Transfer_For_Modify()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			SearchRequest searchRequest = new SearchRequest() { SearchRequestType = SearchRequestType.Refund };

			SearchResponse searchResponse = BIZMoneyTransferEngine.Search(customerSessionId, searchRequest, mgiContext);

			Assert.IsNotNull(searchResponse);
			CXNMoneyTransferService.Verify(moq => moq.Search(It.IsAny<CXNData.SearchRequest>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Search_Money_Transfer_With_RefundWithStage()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			SearchRequest searchRequest = new SearchRequest() { TransactionId = 1000000001, SearchRequestType = SearchRequestType.RefundWithStage };

			SearchResponse searchResponse = BIZMoneyTransferEngine.Search(customerSessionId, searchRequest, mgiContext);

			Assert.IsNotNull(searchResponse);
			CXNMoneyTransferService.Verify(moq => moq.Search(It.IsAny<CXNData.SearchRequest>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}


		[Test]
		public void Can_Initiate_Stage_Modify()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			ModifyRequest modifyRequest = new ModifyRequest() { TransactionId = 1000000004 };

			ModifyResponse modifyResponse = BIZMoneyTransferEngine.StageModify(customerSessionId, modifyRequest, mgiContext);

			Assert.IsNotNull(modifyResponse);
			CXNMoneyTransferService.Verify(moq => moq.StageModify(It.IsAny<CXNData.ModifyRequest>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Stage_Refund()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			RefundRequest refundRequest = new RefundRequest() { TransactionId = 1000000004 };

			long id = BIZMoneyTransferEngine.StageRefund(customerSessionId, refundRequest, mgiContext);

			Assert.AreNotEqual(id, 0);
			CXNMoneyTransferService.Verify(moq => moq.StageRefund(It.IsAny<CXNData.RefundRequest>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Autorized_Modify()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			ModifyRequest modifyRequest = new ModifyRequest() { CancelTransactionId = 1000000000, ModifyTransactionId = 1000000001 };

			BIZMoneyTransferEngine.AuthorizeModify(customerSessionId, modifyRequest, mgiContext);

			CXEMoneyTransferService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<CXEData.TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Modify_Money_Transfer()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			ModifyRequest modifyRequest = new ModifyRequest() { CancelTransactionId = 1000000001, ModifyTransactionId = 1000000001 };

			int statusInInt = BIZMoneyTransferEngine.Modify(customerSessionId, modifyRequest, mgiContext);

			Assert.AreEqual(statusInInt, 4);
			CXNMoneyTransferService.Verify(moq => moq.Modify(It.IsAny<long>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Refund_Transaction()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			RefundRequest refundRequest = new RefundRequest() { CancelTransactionId = 1000000000, RefundTransactionId = 1000000001 };

			string conformationNumber = BIZMoneyTransferEngine.Refund(customerSessionId, refundRequest, mgiContext);

			Assert.AreEqual(conformationNumber, "true");
			CXNMoneyTransferService.Verify(moq => moq.Refund(It.IsAny<CXNData.RefundRequest>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Delivery_Services()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF", CustomerSessionId = customerSessionId };
			DeliveryServiceRequest request = new DeliveryServiceRequest() { CountryCode = "US" };

			List<DeliveryService> deliveryService = BIZMoneyTransferEngine.GetDeliveryServices(customerSessionId, request, mgiContext);

			Assert.AreNotEqual(deliveryService.Count, 0);
			CXNMoneyTransferService.Verify(moq => moq.GetDeliveryServices(It.IsAny<CXNData.DeliveryServiceRequest>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_GetProviderAttributes()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			AttributeRequest attributeRequest = new AttributeRequest() { };

			List<Field> fields = BIZMoneyTransferEngine.GetProviderAttributes(customerSessionId, attributeRequest, mgiContext);

			Assert.AreNotEqual(fields.Count, 0);
			CXNMoneyTransferService.Verify(moq => moq.GetProviderAttributes(It.IsAny<CXNData.AttributeRequest>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Delete_Favorite_Receiver()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "Synovus" , ChannelPartnerId = 33 };

			BIZMoneyTransferEngine.DeleteFavoriteReceiver(customerSessionId, 1000000000, mgiContext);			

			CXNMoneyTransferService.Verify(moq => moq.DeleteFavoriteReceiver(It.IsAny<CXNData.Receiver>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Update_Transaction_Status_To_Failed()
		{
			long customerSessionId = 1000000004;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerName = "TCF" };
			long transactionId = 1000000004;

			BIZMoneyTransferEngine.UpdateTransactionStatus(customerSessionId, transactionId, mgiContext);
			CXEMoneyTransferService.Verify(moq => moq.Update(It.IsAny<long>(), It.IsAny<CXEData.TransactionStates>(), It.IsAny<string>()), Times.AtLeastOnce());
		}
    }
}
