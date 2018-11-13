using MGI.Cxn.BillPay.Data;
using MGI.Cxn.BillPay.Contract;
using MGI.Cxn.BillPay.WU.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spring.Testing.NUnit;
using MGI.Common.Util;

namespace MGI.Cxn.BillPay.WU.Test
{
	[TestFixture]
	public class WUGateway_Fixture : AbstractTransactionalDbProviderSpringContextTests
	{

		public IBillPayProcessor BillPayProcessor { get; set; }
		public MGIContext MgiContext { get; set; }

		protected override string[] ConfigLocations
		{
            get { return new string[] { "assembly://MGI.Cxn.BillPay.WU.Test/MGI.Cxn.BillPay.WU.Test/MGI.Cxn.BillPay.WU.Test.Spring.xml"}; }
		}

		private MGIContext GetMGIContext()
		{
			MGIContext mgiContext = new MGIContext()
			{
				TimeZone = "Eastern Standard Time",
				AgentId = 500021,
				WUCounterId = "990000402",
				LocationName = "TCF Service Desk",
				ChannelPartnerId = 34,
				ProviderId = 401,
				TrxId = 1000010045,
				CxnAccountId = 1000000011
			};
			return mgiContext;
		}

		[Test]
		public void Can_UpdateGoldCardPoints()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();

			MgiContext = GetMGIContext();

			string cardpoints = "110";
			BillPayProcessor.UpdateGoldCardPoints(1000010045, cardpoints, MgiContext);

			BillPayTransaction billPayTrx = BillPayProcessor.GetTransaction(1000010045);
			string wucardPoints = billPayTrx.MetaData["WuCardTotalPointsEarned"].ToString();
			Assert.That(wucardPoints, Is.EqualTo(cardpoints));
		}
		[Test]
		public void Can_Validate_BillPayment()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
			long cxnAccountID = 1000000000; //1000000000
			BillPayRequest request = new BillPayRequest()
			{
				//CardNumber = "500549934",
				ProductName = "FEDERAL BUREAU OF PRISON",
				CustomerAddress1 = "1200 STEPHENSON HIGHWAY",
				CustomerCity = "TROY",
				CustomerDateOfBirth = Convert.ToDateTime("02/09/1987"),
				CustomerFirstName = "Manikandan",
				CustomerLastName = "SYNOVUS",
				CustomerPhoneNumber = "9738835532",
				CustomerState = "MI",
				CustomerZip = "48083",
				Amount = 100,
				Fee = 9,
				PrimaryIdType = "DRIVER'S LICENSE",
				PrimaryIdNumber = "937498327",
				PrimaryIdPlaceOfIssue = "US",
				PrimaryIdCountryOfIssue = "US",
				SecondIdType = "SSN",
				SecondIdCountryOfIssue = "US",
				SecondIdNumber = "974329847",
				//PromoCode = "A0027",
                MetaData=metadata,
				AccountNumber = "00168104DEANS"
               
               
			};
			MgiContext = GetMGIContext();

			long transactionID = BillPayProcessor.Validate(cxnAccountID, request, MgiContext);

			Assert.That(transactionID, Is.AtLeast(1));

			SetComplete();
		}

		[Test]
		public void Can_Commit_BillPayment()
		{
			long transactionID = 1000000100;
			MgiContext = GetMGIContext();
			MgiContext.RequestType = "HOLD";
			long commitTrxId = BillPayProcessor.Commit(transactionID, MgiContext);

			Assert.That(commitTrxId, Is.AtLeast(1));
		}

		[Test]
		public void Can_Add_BillPay_Account()
		{
			BillPayRequest request = new BillPayRequest()
			{
				CardNumber = "100318516",
				CustomerFirstName = "ATUL",
				CustomerLastName = "BONDE",
				CustomerAddress1 = "100 Summit Ave",
				CustomerAddress2 = "ADDRESS2",
				CustomerCity = "MAHWAH",
				CustomerState = "NJ",
				CustomerZip = "07645",
				CustomerStreet = "STREET",
				CustomerDateOfBirth = Convert.ToDateTime("01/02/1953"),
				CustomerEmail = "surendra.p@opus.com",
				CustomerPhoneNumber = "2018202100"
			};

			string timeZone = string.Empty;

			long cxnId = BillPayProcessor.AddBillPayAccount(request, timeZone);

			Assert.That(cxnId, Is.AtLeast(1));

			SetComplete();
		}

		[Test]
		public void Can_GetLocations()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
			BillPayRequest request = new BillPayRequest()
			{
				//CardNumber = "500549934",
				ProductName = "REGIONAL ACCEPTANCE",
				CustomerAddress1 = "ADD 1 ADD2",
				CustomerCity = "BURLINGAME",
				CustomerDateOfBirth = Convert.ToDateTime("05/05/1995"),
				CustomerFirstName = "ASHOK",
				CustomerLastName = "KUMAR",
				CustomerPhoneNumber = "6506856856",
				CustomerState = "CA",
				CustomerZip = "94010",
				Amount = 100,
				Fee = 9,
				PrimaryIdType = "DRIVER'S LICENSE",
				PrimaryIdNumber = "A1234567",
				PrimaryIdPlaceOfIssue = "CA",
				PrimaryIdCountryOfIssue = "US",
				SecondIdType = "SSN",
				SecondIdCountryOfIssue = "US",
				SecondIdNumber = "546657768",
				//PromoCode = "A0027",
                MetaData=metadata,
                AccountNumber="1234561830"
               
               
			};
			string billerName = "REGIONAL ACCEPTANCE";
			string accountNumber = "1234561830";
			long amount = 1515;
			MgiContext = GetMGIContext();
			MgiContext.Context = new Dictionary<string, object>();
			MgiContext.Context.Add("BillPayRequest", request);
			List<Location> locations = BillPayProcessor.GetLocations(billerName, accountNumber, amount, MgiContext);

			Assert.That(locations, Is.Not.Empty);
		}

		[Test]
		public void Can_GetFee_With_Location()
		{
			string billerName = "REGIONAL ACCEPTANCE";
			string accountNumber = "1234561830";
            Location location = new Location() { Name = "KINGFISH FL", Type = "03" };
			long amount = 1515;
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
            BillPayRequest bilpayreq = new BillPayRequest() { Amount = 90, ProductName = billerName, CustomerAddress1 = "JAYAYANAGAR", CustomerCity = "EWEW", CustomerDateOfBirth = Convert.ToDateTime("11/11/1990"), CustomerFirstName = "KARUN", CustomerLastName = "BANGALORE", CustomerPhoneNumber = "4532156789", MetaData = metadata };
			MgiContext = GetMGIContext();
			MgiContext.Context = new Dictionary<string, object>();
			MgiContext.Context.Add("BillPayRequest", bilpayreq);
            
			IO ioClass = new IO();
			Fee fee = BillPayProcessor.GetFee(billerName, accountNumber, amount, location, MgiContext);

			Assert.That(fee, Is.Not.Null);
			Assert.That(fee.DeliveryMethods, Is.Not.Empty);
		}

		[Test]
		public void Can_GetFee_Without_Location()
		{
            string billerName = "REGIONAL ACCEPTANCE";
            string accountNumber = "1234561830";
            Location location = new Location() { Name = "KINGFISH FL", Type = "03" };
            long amount = 1515;
			Dictionary<string, object> metadata = new Dictionary<string, object>();
			metadata = GetMetaData();
            BillPayRequest bilpayreq = new BillPayRequest() { Amount = 90, ProductName = billerName, CustomerAddress1 = "JAYAYANAGAR", CustomerCity = "EWEW", CustomerDateOfBirth = Convert.ToDateTime("11/11/1990"), CustomerFirstName = "KARUN", CustomerLastName = "BANGALORE", CustomerPhoneNumber = "4532156789", MetaData = metadata };
			MgiContext = GetMGIContext();
			MgiContext.Context = new Dictionary<string, object>();
			MgiContext.Context.Add("BillPayRequest", bilpayreq);

			IO ioClass = new IO();
			Fee fee = BillPayProcessor.GetFee(billerName, accountNumber, amount, location, MgiContext);

			Assert.That(fee, Is.Not.Null);
			Assert.That(fee.DeliveryMethods, Is.Not.Empty);
		}

		[Test]
		public void Can_GetBillerMessage()
		{
			string biller = "REGIONAL ACCEPTANCE";
			string errorMessage = string.Empty;
			MgiContext = GetMGIContext();

			BillerInfo billerInfo = BillPayProcessor.GetBillerInfo(biller, MgiContext);

            Assert.That(billerInfo.Message, Is.Not.Null);
		}

		[Test]
		public void Can_GetProviderAttributes()
		{
			// for PA01 (Doesn't work)
			//string billerName = "AMERICAN SUZUKI";
			//string locationName = "ASFS FL";

			//// for PA02 (Works)
			string billerName = "REGIONAL ACCEPTANCE";
			string locationName = "REGA NC";

			// for PA03 (Doesn't work)
			//string billerName = "AMERICAN HOME MORT SERVICING";
			//string locationName = "OPTION CA";

			// for PA04 (Works)
			//string billerName = "CONNECTICUT DOC";
			//string locationName = "CTDOC CT"; 

			// for PA05 (Works)
			//string billerName = "AIRVOICE GSM";
			//string locationName = "ADT235552";

			// for PA06
			//string billerName = "METROPCS";
			//string locationName = "ADA261594";

			MgiContext = GetMGIContext();

			List<Field> fields = BillPayProcessor.GetProviderAttributes(billerName, locationName, MgiContext);

			Assert.That(fields, Is.Not.Null);
			//Assert.That(fields, Is.Not.Empty);
		}

		[Test]
		public void Can_GetTransaction()
		{
			long trxId = 1000000003;

			BillPayTransaction trx = BillPayProcessor.GetTransaction(trxId);

			Assert.That(trx, Is.Not.Null);
		}

		#region Private Methods
		private Dictionary<string, object> GetMetaData()
		{
			Dictionary<string, object> metadata = new Dictionary<string, object>()
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
			return metadata;
		}
		#endregion Private Methods
	}
}
