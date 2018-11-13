using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CheckStatus = MGI.Channel.Shared.Server.Data.Check;

namespace MGI.Integration.Test
{
	[TestFixture]
	public partial class AlloyIntegrationTestFixture : BaseFixture
	{
		[SetUp]
		public void Setup_Check()
		{
			Client = new Desktop();			
			
		}
		#region IngoTest Cases
		
		[TestCase("Synovus")]
		[TestCase("Carver")]
		//[TestCase("TCF")]
		public void DoCheckProcessIT(string channelPartnerName)
		{
			var transactionHistory = DoCheckProcess(channelPartnerName);
			Assert.That(transactionHistory, Is.Not.Null);
		}
		
		[TestCase("Synovus")]
		//[TestCase("TCF")]
		[TestCase("Carver")]
		public void RemoveCheckProcessIT(string channelPartnerName)
		{
			bool isCheckRemoved = RemoveCheckProcess(channelPartnerName);
			Assert.That(isCheckRemoved, Is.True);
		}

		[TestCase("Synovus")]
		//[TestCase("TCF")]
		[TestCase("Carver")]
		public void ParkCheckProcessIT(string channelPartnerName)
		{
			bool isCheckParked = ParkCheckProcess(channelPartnerName);
			Assert.That(isCheckParked, Is.True);
		}

		[TestCase("Synovus")]
		//[TestCase("TCF")]
		[TestCase("Carver")]
		public void UnParkCheckProcessIT(string channelPartnerName)
		{
			CustomerSession customersession = InitiateCustomerSession(channelPartnerName);
			ShoppingCart cart = Client.ShoppingCart(customersession.CustomerSessionId.ToString());
			bool isCheckUnParked = false;
			if(cart.Checks.Count > 0)
			{
				isCheckUnParked = true;
			}
			ShoppingCartCheckoutStatus status = Client.Checkout(customersession.CustomerSessionId, Convert.ToDecimal(120), "", ShoppingCartCheckoutStatus.InitialCheckout, context);
			Assert.That(status, Is.EqualTo(ShoppingCartCheckoutStatus.Completed));

		}
		
		//[TestCase("TCF")]
		[TestCase("Synovus")]
		public void ParkCheckProcessWithManualPromoIT(string channelParnerName)
		{
			bool isCheckParked = ParkCheckProcessWithManualPromo(channelParnerName);
			Assert.That(isCheckParked, Is.True);
		}

		
		//[TestCase("TCF")]
		[TestCase("Synovus")]
		public void UnParkCheckProcessWithManualPromoIT(string channelParnerName)
		{
			CustomerSession customerSssion = InitiateCustomerSession(channelParnerName);
			ShoppingCart cart = Client.ShoppingCart(customerSssion.CustomerSessionId.ToString());
			bool isCheckUnParked = false;
			if (cart.Checks.Count > 0)
			{
				isCheckUnParked = true;
			}
			ShoppingCartCheckoutStatus status = Client.Checkout(customerSssion.CustomerSessionId, Convert.ToDecimal(120), "", ShoppingCartCheckoutStatus.InitialCheckout, context);
			Assert.That(status, Is.EqualTo(ShoppingCartCheckoutStatus.Completed));
		}
		#endregion

		#region Private Methods

		private void PerformCheckProcess(string channelPartnerName, long CustomerSessionId, MGI.Channel.DMS.Server.Data.MGIContext context)
		{
			ProcesscheckTransaction(CustomerSessionId, "", context);			
		}

		private void ProcesscheckTransaction(long customerSessionId, string promoManualCode,MGI.Channel.DMS.Server.Data.MGIContext context )
		{
			Desktop Client = new Desktop();
			context = new Channel.DMS.Server.Data.MGIContext()
			{
				TimeZone = "Eastern Standard Time",
				CompanyToken = "Simulator",
				URL = "http://beta.chexar.net/webservice/"

			};
			CheckData checkData = new CheckData();
			var checkTypes = Client.GetCheckTypes(customerSessionId, context);
			var transactionFee = GetCheckFee(customerSessionId, promoManualCode);

			CheckSubmission checkSubmit = new CheckSubmission()
			{
				IsSystemApplied = transactionFee.IsSystemApplied,
				ImageFormat = "TIFF",
				FrontImageTIFF = Convert.FromBase64String(checkData.FrontImage_Tiff),
				FrontImage = Convert.FromBase64String(checkData.FrontImage),
				BackImageTIFF = Convert.FromBase64String(checkData.BackImage_Tiff),
				BackImage = Convert.FromBase64String(checkData.BackImage),
				MICR = checkData.MICR,
				AccountNumber = checkData.AccountNumber,
				RoutingNumber = checkData.RoutingNumber,
				CheckNumber = checkData.CheckNumber,
				MicrEntryType = (int)CheckEntryTypes.ScanWithImage,
				Fee = transactionFee.BaseFee,
				IssueDate = DateTime.Now,
				CheckType = checkTypes.Where(x => x.Text.ToLower() != "select").FirstOrDefault().Text,
				Amount = GetRandomAmount()
			};
			CheckStatus checkStatus = new CheckStatus();
			checkStatus = Client.SubmitCheck(customerSessionId.ToString(), checkSubmit, context);
			if (checkStatus.Status.ToLower() == "approved")
			{
				ShoppingCart cart = Client.ShoppingCart(customerSessionId.ToString());
			}
		}

		private List<TransactionHistory> DoCheckProcess(string channelPartnerName)
		{		
			CustomerSession customersession = InitiateCustomerSession(channelPartnerName);
			PerformCheckProcess(channelPartnerName, Convert.ToInt64(customersession.CustomerSessionId), context);
			
			ShoppingCartCheckoutStatus status = Client.Checkout(customersession.CustomerSessionId, Convert.ToDecimal(120), "", ShoppingCartCheckoutStatus.InitialCheckout, context);
			TransactionHistoryRequest request = new TransactionHistoryRequest()
			{
				DateRange = 1
			};

			var items = client.GetTransactionHistory(Convert.ToInt64(customersession.CustomerSessionId), customersession.Customer.CIN, string.Empty, string.Empty, DateTime.Now.AddDays(-1), context);
			return items;
		}

		private TransactionFee GetCheckFee(long customerSessionId,string promoCode)
		{
			MGI.Channel.DMS.Server.Data.MGIContext context = new Channel.DMS.Server.Data.MGIContext();
			var checkTypes = Client.GetCheckTypes(customerSessionId, context);
			CheckSubmission checkSubmit = new CheckSubmission();
			if(!string.IsNullOrEmpty(promoCode))
			{
				checkSubmit.PromoCode = promoCode;
				checkSubmit.IsSystemApplied = false;
			}
			else
			{
				checkSubmit.IsSystemApplied = true;
			}
			
			checkSubmit.CheckType = checkTypes.Where(x => x.Text.ToLower() != "select").FirstOrDefault().Text;
			checkSubmit.Amount = GetRandomAmount(); 
			TransactionFee CheckFee = Client.GetCheckFee(customerSessionId.ToString(), checkSubmit, context);
			return CheckFee;
		}

		private List<SelectListItem> GetCheckTypes(long customerSessionId)
		{
			MGI.Channel.DMS.Server.Data.MGIContext context = new Channel.DMS.Server.Data.MGIContext();
			List<SelectListItem> checkType = Client.GetCheckTypes(customerSessionId, context);
			return checkType;
		}


		private bool RemoveCheckProcess(string channelPartnerName)
		{
			CustomerSession customersession = InitiateCustomerSession(channelPartnerName);
			PerformCheckProcess(channelPartnerName, Convert.ToInt64(customersession.CustomerSessionId), context);
			bool isCheckRemoved = false;
			ShoppingCart cart = Client.ShoppingCart(customersession.CustomerSessionId);
			if (cart.Checks.Count > 0)
			{
				string checkId = cart.Checks.FirstOrDefault().Id;
				Client.RemoveCheck(Convert.ToInt64(customersession.CustomerSessionId), Convert.ToInt64(checkId), MgiContext);
			}
			cart = Client.ShoppingCart(customersession.CustomerSessionId);
			if(cart.Checks.Count == 0)
			{
				isCheckRemoved = true;
			}
			return isCheckRemoved;
		}

		private bool ParkCheckProcess(string channelPartnerName)
		{
			CustomerSession customersession = InitiateCustomerSession(channelPartnerName);
			PerformCheckProcess(channelPartnerName, Convert.ToInt64(customersession.CustomerSessionId), context);
			bool ischeckParked = IsParkedCheck(customersession.CustomerSessionId);
			return ischeckParked;
		}

		private bool ParkCheckProcessWithManualPromo(string channelParnerName)
		{
			CustomerSession customersession = InitiateCustomerSession(channelParnerName);
			PerformCheckProcessWithPromo(channelParnerName, Convert.ToInt64(customersession.CustomerSessionId), context);
			bool ischeckParked = IsParkedCheck(customersession.CustomerSessionId);
			return ischeckParked;
		}

		private bool IsParkedCheck(string customerSessionId)
		{
			bool isCheckParked = false;
			ShoppingCart cart = Client.ShoppingCart(customerSessionId);
			if (cart.Checks.Count > 0)
			{
				string checkId = cart.Checks.FirstOrDefault().Id;
				Client.ParkCheck(long.Parse(customerSessionId), long.Parse(checkId));
			}
			cart = Client.ShoppingCart(customerSessionId);

			if (cart.Checks.Count == 0)
			{
				isCheckParked = true;
			}
			return isCheckParked;
		}


		private void PerformCheckProcessWithPromo(string channelPartnerName, long customerSessionId, MGI.Channel.DMS.Server.Data.MGIContext context)
		{
			string promoManualCode = null;
			if (channelPartnerName == "Synovus")
			{
				promoManualCode = "JewelEmpPayroll";
			}
			ProcesscheckTransaction(customerSessionId, promoManualCode, context);
		}
		#endregion

		#region Existing codes Commented
		//private void GetCheckLogin(long customerSessionId)
		//{

		//	ChannelPartnerProductProvider channelPartnerProductProvider = Client.GetChannelPartner("Synovus", context).Providers.Find(x => x.ProcessorName.ToLower() == "ingo");
		//	// channelPartnerProductProvider will not be null if the processor is ingo
		//	if (channelPartnerProductProvider != null)
		//	{
		//		CheckLogin chexarLogin;

		//		chexarLogin = Client.GetCheckLogin(customerSessionId, context);

		//		if (chexarLogin == null)
		//			chexarLogin = new CheckLogin();

		//		context.URL = chexarLogin.URL;
		//		context.BranchId = chexarLogin.BranchId;
		//		context.CompanyToken = chexarLogin.CompanyToken;
		//		context.EmployeeId = chexarLogin.EmployeeId;
		//	}
		//}

		//private List<TransactionHistory> GetCheckProcess(long customerSessionId)
		//{
			//CheckData checkData = new CheckData();
			//var checkTypes = Client.GetCheckTypes(customerSessionId, context);
			
			//CheckSubmission checkSubmit = new CheckSubmission()
			//{
			//	IsSystemApplied = GetCheckFee(customerSessionId).IsSystemApplied,
			//	ImageFormat = "TIFF",
			//	FrontImageTIFF = Convert.FromBase64String(checkData.FrontImage_Tiff),
			//	FrontImage = Convert.FromBase64String(checkData.FrontImage),
			//	BackImageTIFF = Convert.FromBase64String(checkData.BackImage_Tiff),
			//	BackImage = Convert.FromBase64String(checkData.BackImage),
			//	MICR = checkData.MICR,
			//	AccountNumber = checkData.AccountNumber,
			//	RoutingNumber = checkData.RoutingNumber,
			//	CheckNumber = checkData.CheckNumber,
			//	MicrEntryType = (int)CheckEntryTypes.ScanWithImage,
			//	Fee = GetCheckFee(customerSessionId).BaseFee,
			//	IssueDate = DateTime.Now,
			//	CheckType = checkTypes.Where(x => x.Text.ToLower() != "select").FirstOrDefault().Text,
			//	Amount = GetRandomAmount()
			//};
			//GetCheckLogin(Convert.ToInt64(customerSessionId));
			//CheckStatus checkStatus = new CheckStatus();
			//checkStatus = Client.SubmitCheck(customerSessionId.ToString(), checkSubmit, context);
			//if (checkStatus.Status.ToLower() == "approved")
			//{
			//	context.IsReferral = false;
			//	ShoppingCart cart = Client.ShoppingCart(customerSessionId.ToString());
			//	ShoppingCartCheckoutStatus status = Client.Checkout(customerSessionId.ToString(), cart.CheckTotal, cart.CheckTotal, 0, "", ShoppingCartCheckoutStatus.InitialCheckout, context);
			//}
			//TransactionHistoryRequest request = new TransactionHistoryRequest()
			//{
			//	DateRange = 1
			//};

			//var items = client.GetTransactionHistory(Convert.ToInt64(customerSessionId), customerSession.Customer.CIN, string.Empty, string.Empty, DateTime.Now.AddDays(-1), context);
			//context.IsAvailable = true;
			//client.UpdateCounterId(Convert.ToInt64(customerSessionId), context);
			//return items;
	//	}


		//[Test]
		//public void Can_Remove_Ingo_Check_From_Shoppingcart()
		//{
		//	GetChannelPartnerDataSynovus();

		//	AgentSession = GetAgentSession();

		//	AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "LEWIS", new DateTime(1980, 02, 02));
		//	CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

		//	CheckStatus checkStatus = GetCheckProcessingData();

		//	Assert.That(checkStatus.Status.ToLower(), Is.EqualTo("approved"));

		//	if (checkStatus.Status.ToLower() == "approved")
		//	{
		//		ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
		//		if (cart.Checks.Count != 0)
		//		{
		//			string checkId = cart.Checks.FirstOrDefault().Id;
		//			Client.RemoveCheckFromCart(Convert.ToInt64(CustomerSession.CustomerSessionId), Convert.ToInt64(checkId), MgiContext);
		//		}
		//		ShoppingCart removeCheckCart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
		//		Assert.That(removeCheckCart.Checks.Count, Is.EqualTo(0));
		//	}
		//}

		//[Test]
		//public void Can_Perform_Ingo_Check_Cashing()
		//{
		//	GetChannelPartnerDataSynovus();

		//	AgentSession = GetAgentSession();

		//	AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "LEWIS", new DateTime(1980, 02, 02));

		//	CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

		//	CheckStatus checkStatus = GetCheckProcessingData();

		//	Assert.That(checkStatus.Status.ToLower(), Is.EqualTo("approved"));

		//	if (checkStatus.Status.ToLower() == "approved")
		//	{
		//		MgiContext.IsReferral = false;
		//		ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
		//		ShoppingCartCheckoutStatus status = Client.Checkout(CustomerSession.CustomerSessionId, cart.CheckTotal, cart.CheckTotal, 0, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
		//		Assert.That(status, Is.EqualTo(ShoppingCartCheckoutStatus.Completed));
		//	}
		//}

		//[Test]
		//public void Can_Park_Ingo_Check_Cashing()
		//{
		//	GetChannelPartnerDataSynovus();

		//	AgentSession = GetAgentSession();

		//	AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "LEWIS", new DateTime(1980, 02, 02));
		//	CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

		//	CheckStatus checkStatus = GetCheckProcessingData();

		//	Assert.That(checkStatus.Status.ToLower(), Is.EqualTo("approved"));

		//	if (checkStatus.Status.ToLower() == "approved")
		//	{
		//		ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
		//		if (cart.Checks.Count != 0)
		//		{
		//			string checkId = cart.Checks.FirstOrDefault().Id;
		//			Client.ParkCheck(long.Parse(CustomerSession.CustomerSessionId), long.Parse(checkId));
		//		}
		//		ShoppingCart parkCheckCart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
		//		Assert.That(parkCheckCart.Checks.Count, Is.EqualTo(0));
		//	}
		//}

		//[Test]
		//public void Can_ParkUnPark_Ingo_Check_Cashing()
		//{
		//	GetChannelPartnerDataSynovus();

		//	AgentSession = GetAgentSession();

		//	AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "LEWIS", new DateTime(1980, 02, 02));

		//	CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

		//	ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);

		//	Assert.That(cart.Checks.Count, Is.GreaterThan(0));
		//}

		//#endregion

		//#region Certegy Test Cases
		//[Test]
		//public void Can_Perform_Certegy_Check_Cashing()
		//{
		//	GetChannelPartnerDataRedstone();

		//	AgentSession = GetAgentSession();

		//	AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "Smith", new DateTime(1960, 01, 01));
		//	CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

		//	CheckStatus checkStatus = GetCheckProcessingData();

		//	Assert.That(checkStatus.Status.ToLower(), Is.EqualTo("approved"));

		//	if (checkStatus.Status.ToLower() == "approved")
		//	{
		//		ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
		//		if (cart.Checks.Count != 0)
		//		{
		//			string checkId = cart.Checks.FirstOrDefault().Id;
		//			bool isUpdateStatusOnRemoval = Client.RemoveCheque(long.Parse(CustomerSession.CustomerSessionId), long.Parse(checkId), MgiContext);					
		//		} 

		//		MgiContext.IsReferral = false;				
		//		ShoppingCartCheckoutStatus status = Client.Checkout(CustomerSession.CustomerSessionId, cart.CheckTotal, cart.CheckTotal, 0, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
		//		Assert.That(status, Is.EqualTo(ShoppingCartCheckoutStatus.Completed));
		//	}
		//}

		//[Test]
		//public void Can_Decline_Certegy_Check_Cashing()
		//{
		//	GetChannelPartnerDataRedstone();

		//	AgentSession = GetAgentSession();
		//	AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "Smith", new DateTime(1960, 01, 01));
		//	CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

		//	CheckStatus checkStatus = GetCheckProcessingData();

		//	Assert.That(checkStatus.Status.ToLower(), Is.EqualTo("declined"));

		//	if (checkStatus.Status.ToLower() == "declined")
		//	{
		//		ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
		//		Assert.That(cart.Checks, Is.Null);
		//		Assert.That(checkStatus.Status.ToLower(), Is.EqualTo("declined"));

		//	}
		//}

		//[Test]
		//public void Can_Remove_Certegy_Check_From_Shoppingcart()
		//{
		//	GetChannelPartnerDataRedstone();

		//	AgentSession = GetAgentSession();

		//	AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "Smith", new DateTime(1960, 01, 01));
		//	CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

		//	CheckStatus checkStatus = GetCheckProcessingData();

		//	Assert.That(checkStatus.Status.ToLower(), Is.EqualTo("approved"));

		//	if (checkStatus.Status.ToLower() == "approved")
		//	{
		//		ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
		//		if (cart.Checks.Count != 0)
		//		{
		//			string checkId = cart.Checks.FirstOrDefault().Id;
		//			bool isUpdateStatusOnRemoval = Client.RemoveCheque(long.Parse(CustomerSession.CustomerSessionId), long.Parse(checkId), MgiContext);

		//			if (!isUpdateStatusOnRemoval)
		//			{
		//				Client.RemoveCheckFromCart(long.Parse(CustomerSession.CustomerSessionId), long.Parse(checkId), MgiContext);
		//			}
		//		}
		//		ShoppingCart removeCheckCart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
		//		Assert.That(removeCheckCart.Checks.Count, Is.EqualTo(0));
		//	}
		//}
		
		//private CheckStatus GetCheckProcessingData()
		//{
		//	MgiContext.CertegySiteId = "1109541077350101";
		//	MgiContext.TerminalName = TerminalName;
		//	CheckData checkData = new CheckData();

		//	var checkTypes = Client.GetCheckTypes(Convert.ToInt64(CustomerSession.CustomerSessionId), MgiContext);
		//	CheckSubmission checkSubmit = new CheckSubmission();
		//	checkSubmit.IsSystemApplied = false;
		//	checkSubmit.CheckType = checkTypes.Where(x => x.Text.ToLower() != "select").FirstOrDefault().Text;
		//	checkSubmit.Amount = 100;

		//	TransactionFee trxFee = Client.GetCheckFee(CustomerSession.CustomerSessionId, checkSubmit, MgiContext);

		//	checkSubmit.IsSystemApplied = trxFee.IsSystemApplied;
		//	checkSubmit.ImageFormat = "TIFF";
		//	checkSubmit.FrontImageTIFF = Convert.FromBase64String(checkData.FrontImage_Tiff);
		//	checkSubmit.FrontImage = Convert.FromBase64String(checkData.FrontImage);
		//	checkSubmit.BackImage = Convert.FromBase64String(checkData.BackImage);
		//	checkSubmit.BackImageTIFF = Convert.FromBase64String(checkData.BackImage_Tiff);
		//	checkSubmit.MICR = checkData.MICR;
		//	checkSubmit.AccountNumber = checkData.AccountNumber;
		//	checkSubmit.RoutingNumber = checkData.RoutingNumber;
		//	checkSubmit.CheckNumber = checkData.CheckNumber;
		//	checkSubmit.MicrEntryType = (int)CheckEntryTypes.ScanWithImage;

		//	checkSubmit.Fee = trxFee.BaseFee;
		//	checkSubmit.IssueDate = DateTime.Now;

		//	GetCheckLogin(Convert.ToInt64(CustomerSession.CustomerSessionId));

		//	CheckStatus checkStatus = new CheckStatus();
		//	checkStatus = Client.SubmitCheck(CustomerSession.CustomerSessionId, checkSubmit, MgiContext);
		//	return checkStatus;
		//}	
		#endregion

		
	}
}
