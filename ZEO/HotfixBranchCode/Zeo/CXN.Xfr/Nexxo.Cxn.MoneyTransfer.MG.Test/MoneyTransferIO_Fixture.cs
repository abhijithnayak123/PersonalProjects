using MGI.CXN.MG.Common.Data;
using MGI.Cxn.MoneyTransfer.Contract;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.MoneyTransfer.MG.AgentConnectService;
using MGI.Cxn.MoneyTransfer.MG.Impl;
using NUnit.Framework;
using Spring.Testing.NUnit;
using System;
using System.Collections.Generic;
using MGData = MGI.Cxn.MoneyTransfer.MG.Data;
using MGI.Common.DataAccess.Contract;
using MGI.Cxn.MoneyTransfer.MG.Contract;
using MGI.Common.Util;


namespace MGI.Cxn.MoneyTransfer.MG.Test
{
    [TestFixture]
    public class MoneyTransferIO_Fixture : AbstractTransactionalDbProviderSpringContextTests
    {
		public MGIContext MgiContext { get; set; }
        public IMoneyTransfer CXNMoneyTransferProcessor { get; set; }

        public IIO IO { get; set; }

        public IRepository<MGData.Transaction> MGTransactionLogRepo { private get; set; }

        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Cxn.MoneyTransfer.MG.Test/MGI.Cxn.MoneyTransfer.MG.Test/MGI.Cxn.MoneyTransfer.MG.Test.Spring.xml" }; }
        }
		[TestFixtureSetUp]
		public void SetUpAttribute()
		{
			MgiContext = new MGIContext()
			{
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				ChannelPartnerId = 1,
				ProviderId = 302
			};
		}

        [Test]
        public void Can_GetFee()
        {
            FeeRequest request = PopulateFeeRequest();

			FeeResponse response = CXNMoneyTransferProcessor.GetFee(request, MgiContext);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.FeeInformations, Is.Not.Empty);
        }

        [Test]
        public void Can_AddAccount()
        {
            var account = PopulateAccount();

			long id = CXNMoneyTransferProcessor.AddAccount(account, MgiContext);

            Assert.That(id, Is.GreaterThan(0));
        }

        [Test]
        public void Can_UpdateAccount()
        {
            var account = PopulateAccount();

			long accountId = CXNMoneyTransferProcessor.AddAccount(account, MgiContext);

            Assert.That(accountId, Is.GreaterThan(0));

            Account accountGetAdd = new Account();

			accountGetAdd = CXNMoneyTransferProcessor.GetAccount(accountId, MgiContext);
            accountGetAdd.FirstName = "RAJARAM";
            accountGetAdd.LastName = "MOHANROI";

			long idUpdate = CXNMoneyTransferProcessor.UpdateAccount(accountGetAdd, MgiContext);

            Account accountGetUpdate = new Account();
			accountGetUpdate = CXNMoneyTransferProcessor.GetAccount(idUpdate, MgiContext);

            Assert.That(accountGetUpdate.FirstName, Is.EqualTo("RAJARAM"));
            Assert.That(accountGetUpdate.LastName, Is.EqualTo("MOHANROI"));
        }

        [Test]
        public void Can_GetAccount()
        {
            var account = PopulateAccount();

			long id = CXNMoneyTransferProcessor.AddAccount(account, MgiContext);

            Assert.That(id, Is.GreaterThan(0));

            Account accountGet = new Account();

			accountGet = CXNMoneyTransferProcessor.GetAccount(id, MgiContext);

            Assert.That(accountGet.Id, Is.GreaterThan(0));
            Assert.That(accountGet.FirstName, Is.EqualTo("RAJU"));
        }

        [Test]
        public void Can_SaveReceiver()
        {
            Receiver receiver = new Receiver()
            {
                FirstName = "Demo",
                LastName = "Test",
                MiddleName = "T",
                Status = "Active",
                Country = "USA",
                Address = "# 123, San Bruno",
                State_Province = "California",
                ZipCode = "94010",
                PickupCountry = "USA",
                PickupState_Province = "California",
                DeliveryMethod = "WILL_CALL",
                CustomerId = 1000111111146053
            };

            long customerId = (long)receiver.CustomerId;

			long receiverId = CXNMoneyTransferProcessor.SaveReceiver(receiver, MgiContext);
            Assert.That(receiverId, Is.AtLeast(1000000000));

            receiver = CXNMoneyTransferProcessor.GetReceiver(receiverId);
            Assert.That(receiver.FirstName, Is.EqualTo("Demo"));
            Assert.That(receiver.MiddleName, Is.EqualTo("T"));
            Assert.That(receiver.LastName, Is.EqualTo("Test"));

            receiver = CXNMoneyTransferProcessor.GetReceiver(customerId, string.Format("{0} {1}", receiver.FirstName, receiver.LastName));
            Assert.That(receiver.FirstName, Is.EqualTo("Demo"));
            Assert.That(receiver.Status, Is.EqualTo("Active"));

            List<Receiver> receivers = CXNMoneyTransferProcessor.GetReceivers(customerId, receiver.LastName);
            Assert.That(receivers, Is.Not.Empty);

            receivers = CXNMoneyTransferProcessor.GetFrequentReceivers(customerId);
            Assert.That(receivers, Is.Not.Empty);

        }

        [Test]
        public void Can_SendValidateCXNMethod()
        {
            var account = PopulateAccount();
            FeeRequest request = PopulateFeeRequestForSendValidateCXN();

			long accountId = CXNMoneyTransferProcessor.AddAccount(account, MgiContext);
            Assert.That(accountId, Is.GreaterThan(0));

			FeeResponse response = CXNMoneyTransferProcessor.GetFee(request, MgiContext);

            ValidateRequest validaterequest = PopulateValidateRequest(response, null);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.FeeInformations.Count, Is.GreaterThan(0));

			ValidateResponse mtresponse = CXNMoneyTransferProcessor.Validate(validaterequest, MgiContext);

            Assert.That(mtresponse, Is.Not.Null);
            Assert.IsTrue(mtresponse.TransactionId == response.TransactionId);
        }

        [Test]
        public void Can_SendCommitCXNMethod()
        {
            var account = PopulateAccount();
            FeeRequest request = PopulateFeeRequestForCommitCXN();


            MgiContext.SMTrxType = (MGData.MTReleaseStatus.Release).ToString();

			long accountId = CXNMoneyTransferProcessor.AddAccount(account, MgiContext);
            Assert.That(accountId, Is.GreaterThan(0));

			FeeResponse response = CXNMoneyTransferProcessor.GetFee(request, MgiContext);

            ValidateRequest validaterequest = PopulateValidateRequest(response, null);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.FeeInformations.Count, Is.GreaterThan(0));

			ValidateResponse mtresponse = CXNMoneyTransferProcessor.Validate(validaterequest, MgiContext);

            Assert.That(mtresponse, Is.Not.Null);
            Assert.IsTrue(mtresponse.TransactionId == response.TransactionId);

			bool iserror = CXNMoneyTransferProcessor.Commit(mtresponse.TransactionId, MgiContext);

            Assert.IsFalse(iserror);
        }

        [Test]
        public void Can_SendCommitCXNMethodWithMoreInputs()
        {
            PerformSendMoneyTransaction(null, 200);
        }

        #region Receive money IO
        [Test]
        public void Can_GetReferenceNumber()
        {  
            ReferenceNumberRequest referenceNumberRequest = new ReferenceNumberRequest();
            BaseRequest request = PopulateBaseRequest();

            AutoMapper.Mapper.CreateMap<BaseRequest, ReferenceNumberRequest>();
            AutoMapper.Mapper.Map(request, referenceNumberRequest);
            referenceNumberRequest.referenceNumber = "48672617";

            ReferenceNumberResponse referenceNumberResponse = IO.RequestReferenceNumber(referenceNumberRequest);
            Assert.That(referenceNumberResponse.transactionStatus, Is.Not.Null);
            Assert.That(referenceNumberResponse.okForAgent, Is.Not.Null);
            Assert.That(referenceNumberResponse.okForPickup, Is.Not.Null);
        }

        private BaseRequest PopulateBaseRequest()
        {
            BaseRequest baseRequest = new BaseRequest()
            {
                AgentID = "43685767",
                AgentSequence = "1",
                Token = "TEST",
                Language = "en",
                TimeStamp = DateTime.Now,
                ApiVersion = "1305",
                ClientSoftwareVersion = "10.2"
            };
            return baseRequest;
        }

        #endregion

        #region Receive money Gateway

        [Test]
        public void Can_ReceiveProviderAttributes()
        {
			var account = PopulateAccount();
			FeeRequest request = PopulateFeeRequestForSendValidateCXN();

            string timezone = "Central Mountain Time";
			MgiContext.TimeZone = timezone;
			MgiContext.ChannelPartnerId = 34;
			MgiContext.ProviderId = 302;

			long accountId = CXNMoneyTransferProcessor.AddAccount(account, MgiContext);
			Assert.That(accountId, Is.GreaterThan(0));

			FeeResponse response = CXNMoneyTransferProcessor.GetFee(request, MgiContext);

            AttributeRequest attr = new AttributeRequest()
            {
                Amount = 100,
                ReceiveCountry = "USA",
                ReceiveCurrencyCode = "USD",
                TransferType = MoneyTransferType.Receive,
				DeliveryService = new DeliveryService() { Code = "BANK_DEPOSIT", Name = "Account Deposit" }
            };

			List<Field> field = CXNMoneyTransferProcessor.GetProviderAttributes(attr, MgiContext);

            Assert.That(field, Is.Not.Null);
            Assert.AreEqual(field.Count, 0);
        }

        [Test]
        public void Can_ReceiveMoneyGetTransactionMethod()
        {
            Receiver receiver = AddReceiver(MgiContext);

            long sendMoneyTransactionId = PerformSendMoneyTransaction(receiver, 201);

            TransactionRequest transactionRequest = new TransactionRequest();
            transactionRequest.TransactionId = sendMoneyTransactionId;

			Transaction sendMoneytransaction = CXNMoneyTransferProcessor.GetTransaction(transactionRequest, MgiContext);

            TransactionRequest receiveMoneyTransactionRequest = new TransactionRequest();
            receiveMoneyTransactionRequest.TransactionRequestType = TransactionRequestType.ReceiveTransaction;
            receiveMoneyTransactionRequest.ConfirmationNumber = sendMoneytransaction.ReferenceNo;

			Transaction receiveMoneyTransaction = CXNMoneyTransferProcessor.GetTransaction(receiveMoneyTransactionRequest, MgiContext);

			char[] splitchar = { ' ' };
			string[] ReceiverFirstName = receiveMoneyTransaction.ReceiverFirstName.Split(splitchar);
			Assert.AreEqual(sendMoneytransaction.ReceiverFirstName.ToLower(), ReceiverFirstName[0].ToLower());            
            Assert.AreEqual(sendMoneytransaction.ReceiverLastName.ToLower(), receiveMoneyTransaction.ReceiverLastName.ToLower());
            Assert.IsTrue(sendMoneytransaction.TransactionAmount == receiveMoneyTransaction.DestinationPrincipalAmount);
            Assert.AreEqual(sendMoneytransaction.DestinationCountryCode.ToLower(), receiveMoneyTransaction.OriginatingCountryCode.ToLower());
            //Assert.That(receiveMoneyTransaction.TestQuestion, Is.Not.Null);
            //Assert.That(receiveMoneyTransaction.TestAnswer, Is.Not.Null);
        }

        [Test]
        public void Can_ReceiveMoneyValidateMethod()
        {
			Receiver receiver = AddReceiver(MgiContext);

            long sendMoneyTransactionId = PerformSendMoneyTransaction(receiver, 202);

            TransactionRequest transactionRequest = new TransactionRequest();
            transactionRequest.TransactionId = sendMoneyTransactionId;

			Transaction sendMoneytransaction = CXNMoneyTransferProcessor.GetTransaction(transactionRequest, MgiContext);

            TransactionRequest receiveMoneyTransactionRequest = new TransactionRequest();
            receiveMoneyTransactionRequest.TransactionRequestType = TransactionRequestType.ReceiveTransaction;
            receiveMoneyTransactionRequest.ConfirmationNumber = sendMoneytransaction.ReferenceNo;

			Transaction receiveMoneyTransaction = CXNMoneyTransferProcessor.GetTransaction(receiveMoneyTransactionRequest, MgiContext);
			char[] splitchar = { ' ' };
			string[] ReceiverFirstName = receiveMoneyTransaction.ReceiverFirstName.Split(splitchar);
			Assert.AreEqual(sendMoneytransaction.ReceiverFirstName.ToLower(), ReceiverFirstName[0].ToLower());            
            Assert.AreEqual(sendMoneytransaction.ReceiverLastName.ToLower(), receiveMoneyTransaction.ReceiverLastName.ToLower());
            Assert.IsTrue(sendMoneytransaction.TransactionAmount == receiveMoneyTransaction.DestinationPrincipalAmount);
            Assert.AreEqual(sendMoneytransaction.DestinationCountryCode.ToLower(), receiveMoneyTransaction.OriginatingCountryCode.ToLower());

            ValidateRequest request = PopulateReceiveMoneyValidateRequest(receiveMoneyTransaction);
			ValidateResponse response = CXNMoneyTransferProcessor.Validate(request, MgiContext);

            Assert.That(response, Is.Not.Null);
            Assert.IsTrue(request.TransactionId == response.TransactionId);
        }

        [Test]
        public void Can_ReceiveMoneyCommitMethod()
        {
			Receiver receiver = AddReceiver(MgiContext);

            long sendMoneyTransactionId = PerformSendMoneyTransaction(receiver, 205);

            TransactionRequest transactionRequest = new TransactionRequest();
            transactionRequest.TransactionId = sendMoneyTransactionId;

			Transaction sendMoneytransaction = CXNMoneyTransferProcessor.GetTransaction(transactionRequest, MgiContext);

            TransactionRequest receiveMoneyTransactionRequest = new TransactionRequest();
            receiveMoneyTransactionRequest.TransactionRequestType = TransactionRequestType.ReceiveTransaction;
            receiveMoneyTransactionRequest.ConfirmationNumber = sendMoneytransaction.ReferenceNo;

			Transaction receiveMoneyTransaction = CXNMoneyTransferProcessor.GetTransaction(receiveMoneyTransactionRequest, MgiContext);
			char[] splitchar = { ' ' };
			string[]  ReceiverFirstName = receiveMoneyTransaction.ReceiverFirstName.Split(splitchar);
			Assert.AreEqual(sendMoneytransaction.ReceiverFirstName.ToLower(), ReceiverFirstName[0].ToLower());            
            Assert.IsTrue(sendMoneytransaction.TransactionAmount == receiveMoneyTransaction.DestinationPrincipalAmount);
            Assert.AreEqual(sendMoneytransaction.DestinationCountryCode.ToLower(), receiveMoneyTransaction.OriginatingCountryCode.ToLower());

            ValidateRequest request = PopulateReceiveMoneyValidateRequest(receiveMoneyTransaction);
			ValidateResponse response = CXNMoneyTransferProcessor.Validate(request, MgiContext);

            Assert.That(response, Is.Not.Null);
            Assert.IsTrue(request.TransactionId == response.TransactionId);

            MgiContext.RMTrxType = (MGData.MTReleaseStatus.Release).ToString();

			bool iserror = CXNMoneyTransferProcessor.Commit(response.TransactionId, MgiContext);

            MGData.Transaction transaction = MGTransactionLogRepo.FindBy(x => x.Id == response.TransactionId);

            Assert.IsFalse(iserror);
            Assert.IsTrue(transaction.RequestResponseType == (short)RequestResponseType.CommitResponse);
        }

        #endregion

        #region ModifySendMoney

        [Test]
        public void CanSearch()
        {
			Receiver receiver = AddReceiver(MgiContext);
            long sendMoneyTransactionId = PerformSendMoneyTransaction(receiver, 206);
            TransactionRequest transactionRequest = new TransactionRequest();
            transactionRequest.TransactionId = sendMoneyTransactionId;
			Transaction sendMoneytransaction = CXNMoneyTransferProcessor.GetTransaction(transactionRequest, MgiContext);
            Assert.That(sendMoneytransaction.ReferenceNo, Is.Not.Null);
            SearchRequest searchRequest = new SearchRequest()
            {
                ConfirmationNumber = sendMoneytransaction.ReferenceNo
            };
			SearchResponse searchResponse = CXNMoneyTransferProcessor.Search(searchRequest, MgiContext);
            Assert.That(searchResponse.TransactionStatus, Is.Not.Null);
        }

        [Test]
        public void CanModify()
        {
			Receiver receiver = AddReceiver(MgiContext);
            long sendMoneyTransactionId = PerformSendMoneyTransaction(receiver, 207);
            TransactionRequest transactionRequest = new TransactionRequest();
            transactionRequest.TransactionId = sendMoneyTransactionId;
			Transaction sendMoneytransaction = CXNMoneyTransferProcessor.GetTransaction(transactionRequest, MgiContext);
            Assert.That(sendMoneytransaction.ReferenceNo, Is.Not.Null);
            SearchRequest searchRequest = new SearchRequest()
            {
                ConfirmationNumber = sendMoneytransaction.ReferenceNo
            };
			SearchResponse searchResponse = CXNMoneyTransferProcessor.Search(searchRequest, MgiContext);
            Assert.AreEqual(searchResponse.TransactionStatus, "AVAIL");
            ModifyRequest modifiedRequest = ModifiedReceiver(sendMoneytransaction);
			ModifyResponse modifyResponse = CXNMoneyTransferProcessor.StageModify(modifiedRequest, MgiContext);
			CXNMoneyTransferProcessor.Modify(modifyResponse.ModifyTransactionId, MgiContext);
        }


        #endregion ModifySendMoney


        #region RefundSendMoney

        [Test]
        public void CanRefundSendMoney()
        {
			Receiver receiver = AddReceiver(MgiContext);
            decimal amt = (decimal)64.5;
            long sendMoneyTransactionId = PerformSendMoneyTransactionOutsideUSA(receiver, amt); //Use the method, PerformSendMoneyTransaction(receiver) for sending money to US
            TransactionRequest transactionRequest = new TransactionRequest();

            transactionRequest.TransactionId = sendMoneyTransactionId;
			Transaction sendMoneytransaction = CXNMoneyTransferProcessor.GetTransaction(transactionRequest, MgiContext);
            Assert.That(sendMoneytransaction.ReferenceNo, Is.Not.Null);

            SearchRequest searchRequest = new SearchRequest()
            {
                ConfirmationNumber = sendMoneytransaction.ReferenceNo
            };
			SearchResponse searchResponse = CXNMoneyTransferProcessor.Search(searchRequest, MgiContext);
            Assert.AreEqual(searchResponse.TransactionStatus, "AVAIL");

            RefundRequest refundRequest = new RefundRequest();
            refundRequest.TransactionId = sendMoneyTransactionId;

            refundRequest.RefundStatus = searchResponse.RefundStatus;
            refundRequest.ReasonCode = "WRONG_SERVICE";
            refundRequest.ReferenceNumber = sendMoneytransaction.ReferenceNo;

			long cxnTrxId = CXNMoneyTransferProcessor.StageRefund(refundRequest, MgiContext);

			CXNMoneyTransferProcessor.Commit(cxnTrxId, MgiContext);

            SetComplete(); //Uncomment if the transaction table entries are required
        }

        #endregion

        #region Private Methods

        private ModifyRequest ModifiedReceiver(Transaction sendMoneytransaction)
        {
            ModifyRequest modifyRequest = new ModifyRequest()
            {
                FirstName = "ModifiedFirst",
                MiddleName = "ModifiedMiddle",
                LastName = "ModifiedLast",
                SecondLastName = "ModifiedSecond",
                TransactionId = Convert.ToInt64(sendMoneytransaction.TransactionID),

            };
            return modifyRequest;
        }

        private SendValidationRequest GetSendValidationRequest(FeeResponse feeresponse)
        {
            SendValidationRequest request = new SendValidationRequest()
            {
                agentID = "43685767",
                agentSequence = "1",
                token = "TEST",
                apiVersion = "1305",
                clientSoftwareVersion = "10.2",
                timeStamp = DateTime.Now,
                agentCustomerNumber = "123123131",
                destinationCountry = "USA",
                destinationState = "MT",
                deliveryOption = "WILL_CALL",
                receiveCurrency = "USD",
                senderFirstName = "MITCH",
                senderLastName = "CRUZ",
                senderAddress = "100 TROUT ST",
                senderCity = "SAN RAFAEL",
                senderState = "CA",
                senderZipCode = "94901",
                senderCountry = "USA",
                senderHomePhone = "2084505103",
                receiverFirstName = "JOE",
                receiverLastName = "BIGHAT",
                receiverCountry = "USA",
                senderDOB = Convert.ToDateTime("1980-02-14"),
                sendCurrency = "USD",
                consumerId = "0",
                primaryReceiptLanguage = "ENG",
                secondaryReceiptLanguage = "SPA",
                amount = 10.12M,
                feeAmount = 12.34M,
                senderPhotoIdIssueDate = DateTime.Now,
                agentTransactionId = feeresponse.TransactionId.ToString()
            };

            return request;
        }

        private Account PopulateAccount()
        {
            Account account = new Account()
            {
                FirstName = "RAJU",
                LastName = "MATHAPATI",
                Address = "TEST1",
                City = "GULBARGA",
                PostalCode = "58510",
                State = "AA",
                ContactPhone = "2345678910",
                Email = "ABC@AB.COM",
                MobilePhone = "4578901223",
                LevelCode = "testcode",
                SmsNotificationFlag = "true"
            };

            return account;
        }

        private PaymentDetails PopulatePaymentDetails()
        {
            PaymentDetails paymentDetails = new PaymentDetails()
            {
                DeliveryMethod = "WILL_CALL",
                DestinationCountryCode = "USA",
                DestinationCurrencyCode = "USD",
                DestinationPrincipalAmount = 100,
                ExchangeRate = 1,
                OriginatingCountryCode = "USA",
                OriginatingCurrencyCode = "USD",
                TranascationType = "1",
                OriginatorsPrincipalAmount = 300,
                TransferTax = 300,
                PromotionsCode = "1308fivfghfgheoff",
                Fee = 700
            };
            return paymentDetails;
        }

        private FeeRequest PopulateFeeRequest()
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata["Country"] = "US";
            metadata["State"] = "CA";

            DeliveryService service = new DeliveryService()
            {
                Code = "TestCode",
                Name = "TestName"
            };


            FeeRequest request = new FeeRequest()
            {
                AccountId = 1000000000,
                Amount = 125,
                DeliveryService = service,
                ReceiverFirstName = "TestFirstName",
                ReceiverMiddleName = "TestMiddleName",
                ReceiverLastName = "TestLastName",
                ReceiveCountryCode = "USA",
                MetaData = metadata
            };

            return request;
        }

        private FeeRequest PopulateFeeRequestForSendValidateCXN()
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata["Country"] = "US";
            metadata["State"] = "CA";

            DeliveryService service = new DeliveryService()
            {
                Code = "TestCode",
                Name = "TestName"
            };

            FeeRequest request = new FeeRequest()
            {
                AccountId = 1000000000,
                Amount = 130,
                DeliveryService = service,
                ReceiverFirstName = "TestFirstName",
                ReceiverMiddleName = "TestMiddleName",
                ReceiverLastName = "TestLastName",
                ReceiveCountryCode = "USA",
                MetaData = metadata
            };

            return request;
        }

        private FeeRequest PopulateFeeRequestForSendCommitCXN(Receiver receiver, decimal amount)
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata["Country"] = "US";
            metadata["State"] = "CA";

            DeliveryService service = new DeliveryService()
            {
                Code = "TestCode",
                Name = "TestName"
            };


            FeeRequest request = new FeeRequest()
            {
                AccountId = 1000000000,
                Amount = amount,
                DeliveryService = service,
                ReceiverId = receiver != null ? receiver.Id : 0,
                ReceiverFirstName = receiver != null ? receiver.FirstName : "Test First Name",
                ReceiverMiddleName = receiver != null ? receiver.MiddleName : "Test Middle Name",
                ReceiverLastName = receiver != null ? receiver.LastName : "Test Last Name",
                ReceiveCountryCode = "USA",
                MetaData = metadata
            };

            return request;
        }

        private FeeRequest PopulateFeeRequestForSendCommitCXNOutSideUSA(Receiver receiver, decimal amount)
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata["Country"] = "CAN";
            metadata["State"] = "SK";

            DeliveryService service = new DeliveryService()
            {
                Code = "TestCode",
                Name = "TestName"
            };


            FeeRequest request = new FeeRequest()
            {
                AccountId = 1000000000,
                Amount = amount,
                DeliveryService = service,
                ReceiverId = receiver != null ? receiver.Id : 0,
                ReceiverFirstName = receiver != null ? receiver.FirstName : "Test First Name",
                ReceiverMiddleName = receiver != null ? receiver.MiddleName : "Test Middle Name",
                ReceiverLastName = receiver != null ? receiver.LastName : "Test Last Name",
                ReceiveCountryCode = "CAN",
                MetaData = metadata
            };

            return request;
        }

        private FeeRequest PopulateFeeRequestForCommitCXN()
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata["Country"] = "US";
            metadata["State"] = "CA";

            DeliveryService service = new DeliveryService()
            {
                Code = "TestCode",
                Name = "TestName"
            };


            FeeRequest request = new FeeRequest()
            {
                AccountId = 1000000000,
                Amount = 155,
                DeliveryService = service,
                ReceiverFirstName = "TestFirstName",
                ReceiverMiddleName = "TestMiddleName",
                ReceiverLastName = "TestLastName",
                ReceiveCountryCode = "USA",
                MetaData = metadata
            };

            return request;
        }

        private ValidateRequest PopulateValidateRequest(FeeResponse response, Receiver receiver)
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata["Country"] = "US";
            metadata["State"] = "CA";

            ValidateRequest request = new ValidateRequest()
            {
                DateOfBirth = "1990-07-26",
                IdentificationQuestion = "Test Question",
                IdentificationAnswer = "Test Answer",
                Occupation = "Engineer",
                PersonalMessage = "Message",
                ReceiverId = receiver != null ? receiver.Id : 0,
                ReceiverFirstName = receiver != null ? receiver.FirstName : "First Name",
                ReceiverLastName = receiver != null ? receiver.LastName : "Last Name",
                ReceiveCurrency = "USD",
                State = "CA",
                TransferType = MoneyTransferType.Send,
                TransactionId = response.TransactionId,
                DeliveryService = "WILL_CALL",
                PrimaryIdPlaceOfIssue = "CALIFORNIA",
                PrimaryIdCountryOfIssue = "UNITED STATES",
                CountryOfBirth = "UNITED STATES",
                PrimaryIdNumber = "k3210123",
                MetaData = metadata

            };

            return request;
        }

        private ValidateRequest PopulateValidateRequestOutsideUSA(FeeResponse response, Receiver receiver)
        {
            ValidateRequest request = new ValidateRequest()
            {
                DateOfBirth = "07/26/1990",
                IdentificationQuestion = "Test Question",
                IdentificationAnswer = "Test Answer",
                Occupation = "Engineer",
                PersonalMessage = "Message",
                ReceiverId = receiver != null ? receiver.Id : 0,
                ReceiverFirstName = receiver != null ? receiver.FirstName : "First Name",
                ReceiverLastName = receiver != null ? receiver.LastName : "Last Name",
                ReceiveCurrency = "CAD",
                State = "SK",
                TransferType = MoneyTransferType.Send,
                TransactionId = response.TransactionId,
                DeliveryService = "WILL_CALL",
                PrimaryIdPlaceOfIssue = "CALIFORNIA",
                PrimaryIdCountryOfIssue = "UNITED STATES",
                CountryOfBirth = "UNITED STATES",
                PrimaryIdNumber = "k3210123",
				MetaData = new Dictionary<string,object>()
            };

            return request;
        }

        private long PerformSendMoneyTransaction(Receiver receiver, decimal amount)
        {
            var account = PopulateAccount();
            FeeRequest request = PopulateFeeRequestForSendCommitCXN(receiver, amount);

            long accountId = CXNMoneyTransferProcessor.AddAccount(account, MgiContext);
            Assert.That(accountId, Is.GreaterThan(0));

			FeeResponse response = CXNMoneyTransferProcessor.GetFee(request, MgiContext);

            ValidateRequest validaterequest = PopulateValidateRequest(response, receiver);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.FeeInformations.Count, Is.GreaterThan(0));

			ValidateResponse mtresponse = CXNMoneyTransferProcessor.Validate(validaterequest, MgiContext);

            Assert.That(mtresponse, Is.Not.Null);
            Assert.IsTrue(mtresponse.TransactionId == response.TransactionId);

            MgiContext.SMTrxType = (MGData.MTReleaseStatus.Release).ToString();

			bool iserror = CXNMoneyTransferProcessor.Commit(mtresponse.TransactionId, MgiContext);

            Assert.IsFalse(iserror);

            return mtresponse.TransactionId;
        }

        private long PerformSendMoneyTransactionOutsideUSA(Receiver receiver, decimal amount)
        {
            var account = PopulateAccount();
            FeeRequest request = PopulateFeeRequestForSendCommitCXNOutSideUSA(receiver, amount);

			long accountId = CXNMoneyTransferProcessor.AddAccount(account, MgiContext);
            Assert.That(accountId, Is.GreaterThan(0));

			FeeResponse response = CXNMoneyTransferProcessor.GetFee(request, MgiContext);

            ValidateRequest validaterequest = PopulateValidateRequestOutsideUSA(response, receiver);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.FeeInformations.Count, Is.GreaterThan(0));

            ValidateResponse mtresponse = CXNMoneyTransferProcessor.Validate(validaterequest, MgiContext);

            Assert.That(mtresponse, Is.Not.Null);
            Assert.IsTrue(mtresponse.TransactionId == response.TransactionId);

            MgiContext.SMTrxType = (MGData.MTReleaseStatus.Release).ToString();

			bool iserror = CXNMoneyTransferProcessor.Commit(mtresponse.TransactionId, MgiContext);

            Assert.IsFalse(iserror);

            SetComplete();

            return mtresponse.TransactionId;
        }

        private Receiver AddReceiver(MGIContext mgiContext)
        {
            Receiver receiver = new Receiver()
            {
                FirstName = "Demo",
                LastName = "Test",
                MiddleName = "T",
                Status = "Active",
                Country = "USA",
                Address = "# 123, San Bruno",
                State_Province = "California",
                ZipCode = "94010",
                PickupCountry = "USA",
                PickupState_Province = "California",
                DeliveryMethod = "WILL_CALL",
                CustomerId = 1000000000046050
            };

            long customerId = (long)receiver.CustomerId;

            Receiver receiverDetail = CXNMoneyTransferProcessor.GetReceiver(customerId, string.Format("{0} {1}", receiver.FirstName, receiver.LastName));
            if (receiverDetail == null)
            {
                long receiverId = CXNMoneyTransferProcessor.SaveReceiver(receiver, mgiContext);
                receiver = CXNMoneyTransferProcessor.GetReceiver(receiverId);
                return receiver;
            }
            else
                return receiverDetail;
        }

        private ValidateRequest PopulateReceiveMoneyValidateRequest(Transaction receiveMoney)
        {
            //values are coded since those are coming from customer session
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata["receiverAddress"] = "111 ANZA BLVD";
            metadata["receiverCity"] = "BURLINGAME";
            metadata["receiverZipCode"] = "94010";
            metadata["receiverState"] = "CA";
            metadata["receiverPhone"] = "4503764872";

            ValidateRequest validate = new ValidateRequest()
            {
                TransferType = MoneyTransferType.Receive,
                TransactionId = Convert.ToInt64(receiveMoney.TransactionID),
                DateOfBirth = "1950-10-10",
                ReceiveCurrency = receiveMoney.DestinationCurrencyCode,
                PrimaryIdPlaceOfIssue = "CALIFORNIA",
                PrimaryIdType = "DRIVER'S LICENSE",
                PrimaryIdCountryOfIssue = "UNITED STATES",
                PrimaryIdNumber = "K3210123",
                CountryOfBirth = "UNITED STATES",
                Occupation = "BANK EMPLOYEE",
                SecondIdNumber = "405052153",
                MetaData = metadata
            };

            return validate;
        }

        #endregion

    }
}
