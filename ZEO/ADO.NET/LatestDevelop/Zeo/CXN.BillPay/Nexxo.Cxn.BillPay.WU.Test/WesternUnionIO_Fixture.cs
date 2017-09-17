using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MGI.Cxn.BillPay.WU.Impl;
using MGI.Cxn.BillPay.WU.Data;
using MGI.Cxn.BillPay.Data;
using MGI.Cxn.WU.Common.Data;

namespace MGI.Cxn.BillPay.WU.Test
{
	[TestFixture]
	public class WesternUnionIO_Fixture
	{
		[Test]
		public void Can_GetLocations()
		{
			string billerName = "REGIONAL ACCEPTANCE";
			string accountNumber = "1234567890";
			long amount = 1515;
			WesternUnionAccount acnt = new WesternUnionAccount()
			{
				FirstName = "Karun",
				LastName = "Bangalore",
				State = "PA"
			};

			WUBaseRequestResponse wuObjects = new WUBaseRequestResponse();
			wuObjects.ServiceUrl = "https://wugateway2pi.westernunion.net";
			wuObjects.ForeignRemoteSystem = new ForeignRemoteSystem();
			wuObjects.ClientCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2("");

			Dictionary<string, object> context = new Dictionary<string, object>();
			context.Add("BaseWUObject", (object)wuObjects);

			WesternUnionBillPayIO ioClass = new WesternUnionBillPayIO();
			List<Location> locations = ioClass.GetLocations(billerName, accountNumber, amount, acnt, context);

			Assert.That(locations, Is.Not.Null);
			Assert.That(locations.Count, Is.AtLeast(1));
		}
		//commmented by RK
		//[Test]
		//public void Should_GetLocations_As_NULL()
		//{
		//    string billerName = "TESTING ONLY";
		//    string accountNumber = "6011000000005678";
		//    long amount = 1515;
		//    Dictionary<string, object> context = new Dictionary<string, object>();

		//    WesternUnionBillPayIO ioClass = new WesternUnionBillPayIO();
		//    List<Location> locations = ioClass.GetLocations(billerName, accountNumber, amount, context);

		//    Assert.That(locations, Is.Not.Null);
		//    Assert.That(locations.Count, Is.EqualTo(0));
		//}
		//commmented by RK
		//[Test]
		//public void Can_GetDeliveryMethods_With_Location()
		//{
		//    string billerName = "REGIONAL ACCEPTANCE";
		//    string accountNumber = "1234561830";
		//    Location location = new Location() { Name = "KINGFISH FL", Type = "03" };
		//    long amount = 1515;
		//    Dictionary<string, object> context = new Dictionary<string, object>();

		//    WesternUnionBillPayIO ioClass = new WesternUnionBillPayIO();
		//    var response = ioClass.GetDeliveryMethods(billerName, accountNumber, amount, location, context);

		//    Assert.That(response, Is.Not.Null);
		//    Assert.That(response.DeliveryMethods, Is.Not.Empty);
		//    Assert.That(response.DeliveryMethods.Count, Is.AtLeast(1));
		//    Assert.That(response.SessionCookie, Is.Not.Empty);
		//}
		//commmented by RK
		[Test]
		public void Can_GetDeliveryMethods_Without_Location()
		{
			string billerName = "TESTING ONLY";
			string accountNumber = "6011000000005678";
			Location location = null;
			long amount = 1515;
			Dictionary<string, object> context = new Dictionary<string, object>();
			WesternUnionAccount acnt = new WesternUnionAccount()
			{
				FirstName = "Karun",
				LastName = "Bangalore",
				State = "PA"
			};
			BillPaymentRequest brequest = new BillPaymentRequest()
			{

			};
			WesternUnionBillPayIO ioClass = new WesternUnionBillPayIO();
			var response = ioClass.GetDeliveryMethods(billerName, accountNumber, amount, location, acnt, brequest, context);

			Assert.That(response, Is.Not.Null);
			Assert.That(response.DeliveryMethods, Is.Not.Empty);
			Assert.That(response.DeliveryMethods.Count, Is.AtLeast(1));
			Assert.That(response.SessionCookie, Is.Not.Empty);
		}

		[Test]
		public void Can_GetBillerMessage()
		{
			string biller = "REGIONAL ACCEPTANCE";
			Dictionary<string, object> context = new Dictionary<string, object>();
			string message = new WesternUnionBillPayIO().GetBillerMessage(biller, context);

			Assert.That(message, Is.Not.Empty);
		}

		[Test]
		public void Should_Not_GetBillerMessage()
		{
			string biller = "REGIONAL ACCEPTANCE";
			string errorMessage = string.Empty;

			Dictionary<string, object> context = new Dictionary<string, object>();
			string message = new WesternUnionBillPayIO().GetBillerMessage(biller, context);

			Assert.That(message, Is.Empty);
		}
		//commmented by RK
		//	[Test]
		//public void Can_Validate_Payment()
		//{
		//    string errorMessage = string.Empty;
		//    BillPaymentRequest request = new BillPaymentRequest()
		//    {
		//        CxnId = 0,
		//        BillerName = "REGIONAL ACCEPTANCE",
		//        AccountNumber = "601106553",
		//        Location = "KINGFISH FL",
		//        Amount = 2000,
		//        DeliveryCode = "000",
		//        Fee = 0,
		//        SessionCookie = "070651713ZTWOTWENTY IA0201Y1308231232131412TWOTWENTY TX1511C S C LOGIC"
		//    };

		//    WesternUnionAccount account = new WesternUnionAccount()
		//    {
		//        CardNumber = "100318516",
		//        FirstName = "ATUL",
		//        LastName = "BONDE",
		//        Address1 = "100 Summit Ave",
		//        Address2 = "ADDRESS2",
		//        City = "MAHWAH",
		//        State = "NJ",
		//        PostalCode = "07645",
		//        Street = "STREET",
		//        DateOfBirth = Convert.ToDateTime("01/02/1953"),
		//        Email = "surendra.p@opus.com",
		//        ContactPhone = "2018202100",
		//    };
		//    Dictionary<string, object> context = new Dictionary<string, object>();
		//    WesternUnionBillPayIO westernUnionIO = new WesternUnionBillPayIO();
		//    long trxId = westernUnionIO.ValidatePayment(request, account, context);

		//    Assert.That(trxId, Is.AtLeast(1));
		//    Assert.That(errorMessage, Is.Null);
		//}

		//[Test]
		//public void Can_Make_Payment()
		//{
		//    string errorMessage = string.Empty;
		//    BillPaymentRequest request = new BillPaymentRequest()
		//    {
		//        CxnId = 0,
		//        BillerName = "REGIONAL ACCEPTANCE",
		//        AccountNumber = "601106553",
		//        Location = "KINGFISH FL",
		//        Amount = 2000,
		//        DeliveryCode = "000",
		//        Fee = 0,
		//        SessionCookie = "070651713ZTWOTWENTY IA0201Y1308231232131412TWOTWENTY TX1511C S C LOGIC",
		//        MTCN = "9900219149",
		//        NewMTCN = "1329489900219149"
		//    };
		//    Dictionary<string, object> context = new Dictionary<string, object>();
		//    WesternUnionBillPayIO westernUnionIO = new WesternUnionBillPayIO();
		//    westernUnionIO.MakePayment(request, context, out errorMessage);

		//    Assert.That(errorMessage, Is.Null);
		//}
	}
}
