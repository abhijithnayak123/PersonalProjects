using MGI.Biz.BillPay.Contract;
using MGI.Biz.BillPay.Data;
using MGI.Biz.Compliance.Contract;
using MGI.Common.Util;
using MGI.Cxn.BillPay.Data;
using NUnit.Framework;
using Spring.Testing.NUnit;
using System;
using System.Collections.Generic;
using BizBillPayService = MGI.Biz.BillPay.Contract.IBillPayService;
namespace MGI.Biz.BillPay.Test
{
	[TestFixture]
	public class BillPaymentFixture : AbstractTransactionalSpringContextTests
	{
		public BizBillPayService BizBillPayService { get; set; }
		public MGIContext MgiContext { get; set; }

		protected override string[] ConfigLocations
		{
			get
			{
				return new string[] { "assembly://MGI.Biz.BillPay.Test/MGI.Biz.BillPay.Test/Biz.BillPay.Test.xml" };
			}
		}

		#region Private Methods
		private Dictionary<string, object> GetMetaData()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "Location", "" } ,
                { "SessionCookie", "" } ,
                { "AailableBalance", "" } ,
                { "AccountHolder", "" } ,
                { "Attention", "" } ,
                { "Reference", "" },
                { "DateOfBirth", "" } ,
                { "DeliveryCode", "000" } 
            };
			return dictionary;
		}
		private MGIContext GetMGIContext()
		{
			MGIContext mgiContext = new MGIContext() {
				TimeZone = "Eastern Standard Time",
				AgentId = 500021,
				WUCounterId = "990000402",
				LocationName = "TCF Service Desk",
				ChannelPartnerId = 34,
				ProcessorId = 13,
				TrxId = 1000000012
			};
			return mgiContext;
		}
		#endregion Private Methods

		[Test]
		public void Can_Validate_Payment()
		{

			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
			BillPayRequest request = new BillPayRequest()
			{
				MetaData = metadata
			};

			decimal billAmount = 100m;
			decimal fee = 1m;
			long billerID = 100009963;
			string BillerName = "REGIONAL ACCEPTANCE";
			MGI.Biz.BillPay.Data.Customer customer = new MGI.Biz.BillPay.Data.Customer()
			{
				AccountNumber = "8103201238102",
				FirstName = "Ashok",
				LastName = "kumar",
				City = "DENVELLI",
				PhoneNumber = "9638520147",
				DateOfBirth = new System.DateTime(1980, 10, 10),
				State = "CA",
				Zip = "95401",
				AlloyID = 23232323232
			};
			//string billerZip = "292023157";
			MgiContext = GetMGIContext();


			long customerSessionId = 1000002013; // this won't work - just making it compile. Must get the customersession in the setup.

			//long Validate(long customerSessionId, BillPayment billPayment, Dictionary<string, object> context);  customer.AccountNumber,billerID, billerZip,
			long transactionID = BizBillPayService.Validate(customerSessionId, new BillPayment { BillerName = BillerName, AccountNumber = "1234561830", PaymentAmount = billAmount, Fee = fee, billerID = billerID, MetaData = metadata }, MgiContext);

			Assert.That(transactionID, Is.AtLeast(1));
			//SetComplete();
		}


		[Test]
		public void Can_Commit_Transaction()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
			BillPayRequest request = new BillPayRequest()
			{
				MetaData = metadata
			};
			string BillerName = "REGIONAL ACCEPTANCE";
			decimal billAmount = 16m;
			decimal fee = 1m;
			long billerID = 100009963;
			Customer customer = new Customer()
			{
				AccountNumber = "8103201238102",
				FirstName = "Anil",
				LastName = "kumar",
				City = "ORLAN",
				PhoneNumber = "8654256585",
				State = "CA",
				Zip = "85465",
				AlloyID = 23232323232
			};
			MgiContext = GetMGIContext();

			long customerSessionId = 1000002013; // this won't work - just making it compile. Must get the customersession in the setup.

			long transactionID = BizBillPayService.Validate(customerSessionId, new BillPayment { BillerName = BillerName, AccountNumber = "1234561830", PaymentAmount = billAmount, Fee = fee, billerID = billerID, MetaData = metadata }, MgiContext);
			//long transactionID = BizBillPayService.Validate(customerSessionId, new BillPayment { PaymentAmount = billAmount, Fee = fee, billerID = billerID }, context);

			//transactionID = 1000000024;
			//BizBillPayService.Commit(customerSessionId, transactionID, context);
			//SetComplete();
		}

		[Test]
		public void Should_Not_Validate_Payment_When_Fee_LessThan_Amount()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
			BillPayRequest request = new BillPayRequest()
			{
				MetaData = metadata
			};
			string BillerName = "REGIONAL ACCEPTANCE";
			decimal billAmount = 9m;
			decimal fee = 2m;
			long billerID = 100009963;
			Customer customer = new Customer()
			{//8103201238102
				AccountNumber = "1234561830",
				FirstName = "Ashok",
				LastName = "kumar",
				City = "DENVELLI",
				PhoneNumber = "8654256585",
				State = "CA",
				Zip = "95401",
				AlloyID = 23232323232
			};
			MgiContext = GetMGIContext();
			long customerSessionId = 1000002013;
			//long transactionID = BizBillPayService.Validate(customerSessionId, new BillPayment { BillerName = BillerName, AccountNumber = "1234561830", PaymentAmount = billAmount, Fee = fee, billerID = billerID, MetaData = metadata }, context);
			//long transactionID = BizBillPayService.Validate(customerSessionId, new BillPayment { PaymentAmount = billAmount, Fee = fee }, context);
			BizComplianceLimitException ex = Assert.Throws<BizComplianceLimitException>(() => BizBillPayService.Validate(customerSessionId, new BillPayment { BillerName = BillerName, AccountNumber = "1234561830", PaymentAmount = billAmount, Fee = fee, billerID = billerID, MetaData = metadata }, MgiContext));
			Assert.AreEqual(1008, ex.MajorCode);
			Assert.AreEqual(6011, ex.MinorCode);
			//Assert.That(transactionID, Is.EqualTo(0));
		}



		[Test]
		public void Can_Validate_Payment_Pass()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
			BillPayRequest request = new BillPayRequest()
			{
				MetaData = metadata
			};

			decimal billAmount = 100m;
			decimal fee = 1m;
			long billerID = 100009963;
			string BillerName = "REGIONAL ACCEPTANCE";
			MGI.Biz.BillPay.Data.Customer customer = new MGI.Biz.BillPay.Data.Customer()
			{
				AccountNumber = "8103201238102",
				FirstName = "Kaushik",
				LastName = "Sakala",
				City = "DENVELLI",
				PhoneNumber = "9638520147",
				DateOfBirth = new System.DateTime(1980, 10, 10),
				State = "CA",
				Zip = "95401",
				AlloyID = 23232323232
			};
			MgiContext = GetMGIContext();

			long customerSessionId = 1000000013;
			try
			{
				long transactionID = BizBillPayService.Validate(customerSessionId, new BillPayment { BillerName = BillerName, AccountNumber = "1234561830", PaymentAmount = billAmount, Fee = fee, billerID = billerID, MetaData = metadata }, MgiContext);
				Assert.Pass("Limits Test Passed");
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}
	}
}

