using System;
using System.Collections.Generic;


using NUnit.Framework;
using Spring.Testing.NUnit;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using MGI.Biz.MoneyTransfer.Data;
using MGI.Biz.MoneyTransfer.Contract;


namespace MGI.Biz.MoneyTransfer.Test
{
	[TestFixture]
	public class MoneyTransferEngineTest : AbstractTransactionalSpringContextTests
	{
		public IMoneyTransferEngine MoneyTransferEngine { private get; set; }
		public MGI.Common.Util.MGIContext MgiContext { get; set; }

		public ICustomerSessionService CustomerSessionService { private get; set; }
		[SetUp]
		public void Setup()
		{
			//IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			//moneyTransferEngine = (IMoneyTransferEngine)ctx.GetObject("WUMoneyTransferEngine");
			//_sessionCXN = (ISessionFactory)ctx.GetObject("SessionFactoryCXN");
			//_sessionCXE = (ISessionFactory)ctx.GetObject("SessionFactoryCXE");
			MgiContext = new Common.Util.MGIContext() {
				TimeZone = "Pacific Standard Time",
				ChannelPartnerId = 34,
				LocationName = "TCF",
				WUCounterId = "990000501",
				ChannelPartnerName = "TCF",
				ProcessorId = 14,
				AgentId = 500006,
				BankId = "0120",
				AgentName = "ZeoMGI",
				AgentFirstName = "System",
				AgentLastName = "Admin"
			};
		}

		protected override string[] ConfigLocations
		{
			//get { return new string[] { "assembly://MGI.Biz.MoneyTransfer.Impl/MGI.Biz.MoneyTransfer.Impl/MoneyTransfer.Biz.xml" }; }
			get { return new string[] { "assembly://MGI.Biz.MoneyTransfer.Test/MGI.Biz.MoneyTransfer.Test/Biz.MoneyTransfer.Test.xml" }; }

		}

		//[Test]
		//public void Can_RefundCommit()
		//{

		//}

		//[Test]
		//public void Can_SearchMoneyTransfer()
		//{
		//	MGI.Biz.MoneyTransfer.Data.SearchRequest searchRefundrequest = new Data.SearchRequest();
		//	//  searchRefundrequest..mtcn = "2160252185";
		//	long customerSessionID = 1000001923;//1000007857;//1000004014;

		//	string refundCancelflag = string.Empty;
		//	searchRefundrequest.SearchRequestType = Data.SearchRequestType.Refund;

		//	Dictionary<string, object> context = new Dictionary<string, object>();
		//	context.Add("ChannelPartnerId", 34);
		//	context.Add("TimeZone", "Central Mountain Time");
		//	context.Add("ProviderID", 301);
		//	//context.Add("ProviderID", 301);
		//	//MGI.Core.Partner.Data.Location location= new Location();
		//	//location.LocationName = "TestTimeStamp";
		//	//location.LocationIdentifier = "13139739";
		//	//location.ChannelPartnerId = 33;
		//	//location.TimezoneID = "Central Mountain Time";

		//	CustomerSession session = CustomerSessionService.Lookup(customerSessionID);
		//	context["Location"] = session.AgentSession.Terminal.Location;
		//	context["ProcessorID"] = 14;
		//	context["TimeZone"] = session.AgentSession.Terminal.Location.TimezoneID;
		//	context["WUCounterId"] = "990000402";

		//	MoneyTransferEngine.Search(customerSessionID, searchRefundrequest, context);
		//	SetComplete();
		//	//Assert.IsTrue(newTranID > 0);

		//}

		//[Test]
		//public void Can_Refund_Stage()
		//{

		//	RefundRequest refundRequest = new RefundRequest();

		//	refundRequest.ReasonDesc = "RCM";
		//	refundRequest.Comments = "Refund Money Transfer Testing";
		//	refundRequest.TransactionId = 1000001923;


		//	//refundRequest.RefundCancelFlag = "N";
		//	//RefundRequest.Mtcn = "6080378209";
		//	long customerSessionID = 1000001133;



		//	//RefundRequest.Mtcn = "6080378209";

		//	Dictionary<string, object> context = new Dictionary<string, object>();
		//	context.Add("ChannelPartnerId", 34);

		//	CustomerSession session = CustomerSessionService.Lookup(customerSessionID);
		//	context["Location"] = session.AgentSession.Terminal.Location;
		//	context["ProcessorID"] = 14;
		//	context["TimeZone"] = session.AgentSession.Terminal.Location.TimezoneID;

		//	//long partnerTransactionID = 1000002424;

		//	// string refundCancelflag = string.Empty;

		//	MoneyTransferEngine.Refund(customerSessionID, refundRequest, context);
		//	SetComplete();

		//	RefundRequest RequestData = new RefundRequest();

		//	MoneyTransferEngine.StageRefund(customerSessionID, refundRequest, context);
		//	SetComplete();

		//}

		//[Test]
		//public void MoneyTransfer_Test()
		//{
		//	RefundRequest RefundRequestData = new Data.RefundRequest();

		//	RefundRequestData.ReasonDesc = "RCM";
		//	RefundRequestData.Comments = "Refund Money Transfer BIZ Test";
		//	RefundRequestData.CancelTransactionId = 1000000316;
		//	RefundRequestData.RefundTransactionId = 1000001086;


		//	Dictionary<string, object> context = new Dictionary<string, object>();
		//	context.Add("ChannelPartnerId", 34);
		//	context.Add("TimeZone", "Central Mountain Time");
		//	context.Add("ProviderID", "401");
		//	context.Add("WUCounterId", 990000402);
		//	context.Add("Finacial Chargers", 0.05);
		//	context.Add("Financials principal_amount", 100);
		//	context.Add("Financials pay amount", 5);
		//	context.Add("Gross total amount", 105.05);
		//	context.Add("ReferenceNo", 92929929292);


		//	MoneyTransferEngine.Refund(1000001254, RefundRequestData, context);
		//}




		[Test]
		public void Can_Refund_MoneyTransfer()
		{
			FeeRequest feeRequest = new FeeRequest()
			{

				Amount = 100,
				ReceiveAmount = 0.00M,
				ReceiveCountryCode = "IN",
				ReceiveCountryCurrency = "INR",
				PromoCode = "",
				PersonalMessage = "",
				ReferenceNo = "",
				AccountId = 0,
				ReceiverId = 1000000009,
				TransactionId = 0,
				IsDomesticTransfer = false,
				ReceiverFirstName = "testlast",
				ReceiverLastName = "lastets",
				ReceiverSecondLastName = "",
				ReceiverMiddleName = "",
				DeliveryService = new DeliveryService
				{
					Code = "000",
					Name = "000"
				},
				MetaData = new Dictionary<string, object>()
				{
					{"ReceiveAgentId", ""},
					{"CityCode",""},
					{"CityName",""},
					{"StateName",""},
					{"TestQuestionOption",""},
					{"TestQuestion",""},
					{"TestAnswer",""},
					{"IsFixedOnSend",true}
				}
			};
			long customerSessionId = 1000001752;
			Data.FeeResponse fee = MoneyTransferEngine.GetFee(customerSessionId, feeRequest, MgiContext);
			Assert.NotNull(fee);

			var validateRequest = new MGI.Biz.MoneyTransfer.Data.ValidateRequest
			{
				Amount = 100,
				CountryOfBirth = null,
				DateOfBirth = 0m,
				DeliveryService = "000",
				Discount = 0m,
				ExchangeRate = 0m,
				Fee = 5m,
				IdentificationAnswer = null,
				IdentificationQuestion = null,
				MessageFee = 0m,
				MetaData = new Dictionary<string, object>()
					{
						{ "ExpectedPayoutCity", null },
						{ "ExpectedPayoutStateCode", "WA" },
						{ "ProceedWithLPMTError", false },
						{ "ReceiveAgentAbbr", null }
					},
				Occupation = null,
				OtherFee = 0m,
				OtherTax = 0m,
				PersonalMessage = null,
				PrimaryCountryOfIssue = null,
				PrimaryIdCountryOfIssue = null,
				PrimaryIdNumber = null,
				PrimaryIdPlaceOfIssue = null,
				PrimaryIdType = null,
				ProceedWithLPMTError = false,
				PromoCode = null,
				ReceiveAmount = 0m,
				ReceiveCurrency = null,
				ReceiverFirstName = "AngLee",
				ReceiverId = 1000000014,
				ReceiverLastName = "Lee",
				ReceiverSecondLastName = "",
				ReferenceNumber = null,
				SecondIdNumber = null,
				State = "WASHINGTON",
				Tax = 0m,
				TotalAmount = 0m,
				TransactionId = fee.TransactionId,
				TransferType = Data.TransferType.SendMoney
			};

			Data.ValidateResponse response = MoneyTransferEngine.Validate(customerSessionId, validateRequest, MgiContext);
			Assert.NotNull(response);
		}


		//[Test]
		//public void Can_Commit()
		//{
		//	long customerSessionId = 1000001752;
		//	Dictionary<string, object> context = GetContext();
		//	FeeRequest feeRequest = new FeeRequest();
		//	Data.FeeResponse fee = MoneyTransferEngine.GetFee(customerSessionId, feeRequest, context);
		//	var commitResult = MoneyTransferEngine.Commit(customerSessionId, fee.TransactionId, context);
		//	commitResult = 5;
		//	SetComplete();
		//	Assert.NotNull(commitResult);

		//	MoneyTransferTransaction transaction = new MoneyTransferTransaction();
		//	//SetComplete();
		//	Assert.That(transaction.TransactionID, Is.Not.Null);
		//	Assert.That(transaction.ReferenceNo, Is.Not.Null);
		//}

		/// <summary>
		///A test for GetRecipientProfileBySearchTerm
		///</summary>
		[Test]
		public void b_GetReceivers()
		{
			IList<Data.Receiver> recipients = MoneyTransferEngine.GetReceivers(1000000531, "SMNEEDTESTNO", MgiContext);
			Assert.IsFalse(recipients.Count > 0);
		}

		/////// <summary>
		///////A test for GetFrequentRecipients
		///////</summary>
		[Test]
		public void c_GetFrequentRecipientsTest()
		{
			long customersessionid = 1000000531;
			IList<Data.Receiver> recipients = MoneyTransferEngine.GetFrequentReceivers(customersessionid, MgiContext);
			Assert.IsTrue(recipients.Count > 0);
		}

		/////// <summary>
		///////A test for AddRecipient
		///////</summary>
		[Test]
		public void d_AddReceiver()
		{
			Random rnd = new Random();
			var firstName = "Test First Name " + rnd.Next(1, 1000).ToString();
			Data.Receiver receiver = new Data.Receiver
			{
				Address = null,
				City = null,
				CustomerId = null,
				DeliveryMethod = "000",
				DeliveryOption = null,
				FirstName = firstName,
				Id = 0,
				IsReceiverHasPhotoId = false,
				LastName = "Lee",
				MiddleName = null,
				NickName = null,
				PhoneNumber = null,
				PickupCity = null,
				PickupCountry = "US",
				PickupState_Province = "WA",
				SecondLastName = null,
				SecurityAnswer = null,
				SecurityQuestion = null,
				State_Province = null,
				Status = "Active",
				ZipCode = null
			};

			long customerSessionId = 1000001133;

			var id = MoneyTransferEngine.AddReceiver(customerSessionId, receiver, MgiContext);
			SetComplete();

			Assert.That(id, Is.Not.Null);
			Assert.That(id, Is.Not.EqualTo(0));
			//    }
			//}
		}

		/// <summary>
		///A test for EditRecipient
		///</summary>
		[Test]
		public void e_EditReceiver()
		{
			long customerSessionId = 1000000279;
			Receiver rec = MoneyTransferEngine.GetReceiver(customerSessionId, 1000000003, MgiContext);

			rec.FirstName = "Ashoka";
			rec.PhoneNumber = "9878767653";
			long editResult = MoneyTransferEngine.EditReceiver(customerSessionId, rec, MgiContext);
			Assert.IsTrue(editResult > 0);
		}

		[Test]
		public void f_GetReceiver()
		{
			long customerSessionId = 1000000279;
			Receiver rec = MoneyTransferEngine.GetReceiver(customerSessionId, 1000000003, MgiContext);

			Assert.IsTrue(rec != null);
		}

		[Test]
		public void MoneyTransfer_Limits_Pass()
		{
			ValidateRequest validateRequest = new ValidateRequest()
			{
				TransferType = MGI.Biz.MoneyTransfer.Data.TransferType.SendMoney,
				TransactionId = 1000000021,
				ReceiverId = 1000000000,
				Amount = 50,
				Fee = 5,
				Tax = 0,
				OtherFee = 0,
				MessageFee = 0,
				PersonalMessage = null,
				PromoCode = null,
				IdentificationQuestion = null,
				IdentificationAnswer = null,
				DeliveryService = "000",
				State = "CALIFORNIA",
				ReceiverFirstName = "steve",
				ReceiverLastName = "jobs",
				ReceiverSecondLastName = "",
				MetaData = new Dictionary<string, object>()
					{
						{"ExpectedPayoutCity", ""},
						{"ExpectedPayoutStateCode", "CA"},
						{"ProceedWithLPMTError", false},
						{"ReceiveAgentAbbr", ""}						
					}
			};
			try
			{
				ValidateResponse validate = MoneyTransferEngine.Validate(1000000011, validateRequest, MgiContext);
				Assert.Pass("Limits Test Passed");
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}


		}


		[Test]
		public void MoneyTransfer_Limits_Fails()
		{
			ValidateRequest validateRequest = new ValidateRequest()
			{
				TransferType = MGI.Biz.MoneyTransfer.Data.TransferType.SendMoney,
				TransactionId = 1000000021,
				ReceiverId = 1000000000,
				Amount = 5050,
				Fee = 50,
				Tax = 0,
				OtherFee = 0,
				MessageFee = 0,
				PersonalMessage = null,
				PromoCode = null,
				IdentificationQuestion = null,
				IdentificationAnswer = null,
				DeliveryService = "000",
				State = "CALIFORNIA",
				ReceiverFirstName = "steve",
				ReceiverLastName = "jobs",
				ReceiverSecondLastName = "",
				MetaData = new Dictionary<string, object>()
					{
						{"ExpectedPayoutCity", ""},
						{"ExpectedPayoutStateCode", "CA"},
						{"ProceedWithLPMTError", false},
						{"ReceiveAgentAbbr", ""}						
					}
			};

			try
			{
				ValidateResponse validate = MoneyTransferEngine.Validate(1000000011, validateRequest, MgiContext);
				Assert.Pass("Limits Test Passed");
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}
	}
}

