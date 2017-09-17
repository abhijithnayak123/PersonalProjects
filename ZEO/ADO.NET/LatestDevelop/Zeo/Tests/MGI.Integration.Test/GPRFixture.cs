using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.Common;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Integration.Test
{
	[TestFixture]
	class GPRFixture : BaseFixture
	{
		[SetUp]
		public void Setup()
		{
			Client = new Desktop();
		}

		#region TSys_Test_cases
		[Test]
		public void Can_Perform_TSys_GPR_Load()
		{
			GetChannelPartnerDataSynovus();

			AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "LEWIS", new DateTime(1980, 02, 02));

			Response customerResponse = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);
            if (VerifyException(customerResponse)) throw new AlloyWebException(customerResponse.Error.Details); 
            CustomerSession = (CustomerSession)customerResponse.Result;

			decimal LoadToCard = 30;
			decimal LoadFee = (decimal)((Response)Client.GetFundsFee(Convert.ToInt64(CustomerSession.CustomerSessionId), LoadToCard, FundType.Credit, MgiContext)).Result;

			Funds fund = new Funds() { Amount = LoadToCard, Fee = LoadFee };

			Client.Load(CustomerSession.CustomerSessionId, fund, MgiContext);

			Response cartResponse = Client.ShoppingCart(CustomerSession.CustomerSessionId);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;
			MgiContext.IsReferral = false;

			Response statusResponse = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
            if (VerifyException(statusResponse)) throw new Exception(statusResponse.Error.Details);
            ShoppingCartCheckoutStatus status = (ShoppingCartCheckoutStatus)statusResponse.Result;
			Assert.That(status, Is.EqualTo(ShoppingCartCheckoutStatus.Completed));
		}

		[Test]
		public void Can_Perform_TSys_GPR_Withdraw()
		{
			GetChannelPartnerDataSynovus();

			AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "LEWIS", new DateTime(1980, 02, 02));

			Response customerResponse = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);
            if (VerifyException(customerResponse)) throw new AlloyWebException(customerResponse.Error.Details); 
            CustomerSession = (CustomerSession)customerResponse.Result;

			decimal withdrawFromCard = 130;
			decimal withdrawFee = (decimal)((Response)Client.GetFundsFee(Convert.ToInt64(CustomerSession.CustomerSessionId), withdrawFromCard, FundType.Debit, MgiContext)).Result;

			Funds fund = new Funds() { Amount = withdrawFromCard, Fee = withdrawFee };

			Client.Withdraw(CustomerSession.CustomerSessionId, fund, MgiContext);

			Response cartResponse = Client.ShoppingCart(CustomerSession.CustomerSessionId);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;
			MgiContext.IsReferral = false;

			Response statusResponse = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
            if (VerifyException(statusResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCartCheckoutStatus status = (ShoppingCartCheckoutStatus)statusResponse.Result;
			Assert.That(status, Is.EqualTo(ShoppingCartCheckoutStatus.CashOverCounter));
		}


		#endregion

		#region Visa_Test_Cases
		[Test]
		public void Can_Perform_Visa_GPR_Load()
		{
			GetChannelPartnerDataTCF();

			AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "SCOTT", new DateTime(1990, 12, 12));

			Dictionary<string, object> ssoAttributes = new Dictionary<string, object>()
			{
				{"UserID", "ZeoMGI"},
				{"TellerNum", "98001"},
				{"BranchNum", "43"},
				{"BankNum", "99"},
				{"DPSBranchID", "12392"},
				{"LawsonID", "104"},
				{"LU", "23B7"},
				{"CashDrawer", "980"},
				{"AmPmInd", "A"},
				{"MachineName", "001-MGIw7"},
				{"BusinessDate", "2015-06-10"}
			};

			if (MgiContext != null)
			{
				if (MgiContext.Context != null)
				{
					MgiContext.Context.Add("SSOAttributes", ssoAttributes);
				}
			}

			Response customerResponse = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);
            if (VerifyException(customerResponse)) throw new AlloyWebException(customerResponse.Error.Details); 
            CustomerSession = (CustomerSession)customerResponse.Result;

			decimal LoadToCard = 30;
			decimal LoadFee = (decimal)((Response)Client.GetFundsFee(Convert.ToInt64(CustomerSession.CustomerSessionId), LoadToCard, FundType.Credit, MgiContext)).Result;

			Funds fund = new Funds() { Amount = LoadToCard, Fee = LoadFee };

			Client.Load(CustomerSession.CustomerSessionId, fund, MgiContext);
			Response cartResponse = Client.ShoppingCart(CustomerSession.CustomerSessionId);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;
			MgiContext.IsReferral = false;
			Response statusResponse;
            ShoppingCartCheckoutStatus status;
			try
			{
				statusResponse = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
                if (VerifyException(statusResponse)) throw new Exception(statusResponse.Error.Details);
                status = (ShoppingCartCheckoutStatus)statusResponse.Result;
			}
			catch (Exception)
			{
				statusResponse = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
                if (VerifyException(statusResponse)) throw new Exception(statusResponse.Error.Details);
                status = (ShoppingCartCheckoutStatus)statusResponse.Result;
			}
			Assert.That(statusResponse, Is.EqualTo(ShoppingCartCheckoutStatus.Completed));
		}

		[Test]
		public void Can_Perform_Visa_GPR_Withdraw()
		{
			GetChannelPartnerDataTCF();

			AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "SCOTT", new DateTime(1990, 12, 12));

			Dictionary<string, object> ssoAttributes = new Dictionary<string, object>()
			{
				{"UserID", "ZeoMGI"},
				{"TellerNum", "98001"},
				{"BranchNum", "43"},
				{"BankNum", "99"},
				{"DPSBranchID", "12392"},
				{"LawsonID", "104"},
				{"LU", "23B7"},
				{"CashDrawer", "980"},
				{"AmPmInd", "A"},
				{"MachineName", "001-MGIw7"},
				{"BusinessDate", "2015-06-10"}
			};

			if (MgiContext != null)
			{
				if (MgiContext.Context != null)
				{
					MgiContext.Context.Add("SSOAttributes", ssoAttributes);
				}
			}

			Response customerResponse = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);
            if (VerifyException(customerResponse)) throw new AlloyWebException(customerResponse.Error.Details);
            CustomerSession = (CustomerSession)customerResponse.Result;

			decimal withdrawFromCard = 130;
			decimal withdrawFee = (decimal)((Response)Client.GetFundsFee(Convert.ToInt64(CustomerSession.CustomerSessionId), withdrawFromCard, FundType.Debit, MgiContext)).Result;

			Funds fund = new Funds() { Amount = withdrawFromCard, Fee = withdrawFee };

			Client.Withdraw(CustomerSession.CustomerSessionId, fund, MgiContext);
			Response cartResponse = Client.ShoppingCart(CustomerSession.CustomerSessionId);
            if (VerifyException(cartResponse)) throw new Exception(cartResponse.Error.Details);
            ShoppingCart cart = cartResponse.Result as ShoppingCart;
			MgiContext.IsReferral = false;
            Response statusResponse;
			ShoppingCartCheckoutStatus status;
			try
			{
				statusResponse = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
                if (VerifyException(statusResponse)) throw new Exception(statusResponse.Error.Details);
                status = (ShoppingCartCheckoutStatus)statusResponse.Result;
			}
			catch (Exception)
			{
				statusResponse = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
                if (VerifyException(statusResponse)) throw new Exception(statusResponse.Error.Details);
                status = (ShoppingCartCheckoutStatus)statusResponse.Result;
			}

			Assert.That(status, Is.EqualTo(ShoppingCartCheckoutStatus.CashOverCounter));
		}
		#endregion

	}
}
