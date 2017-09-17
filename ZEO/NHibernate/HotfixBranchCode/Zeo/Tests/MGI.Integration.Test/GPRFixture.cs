using MGI.Channel.DMS.Server.Data;
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

			CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

			decimal LoadToCard = 30;
			decimal LoadFee = Client.GetFundsFee(long.Parse(CustomerSession.CustomerSessionId), LoadToCard, FundType.Credit, MgiContext).NetFee;

			Funds fund = new Funds() { Amount = LoadToCard, Fee = LoadFee };

			Client.Load(CustomerSession.CustomerSessionId, fund, MgiContext);

			ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
			
			MgiContext.IsReferral = false;
			
			ShoppingCartCheckoutStatus status = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
			
			Assert.That(status, Is.EqualTo(ShoppingCartCheckoutStatus.Completed));
		}

		[Test]
		public void Can_Perform_TSys_GPR_Withdraw()
		{
			GetChannelPartnerDataSynovus();

			AgentSession = GetAgentSession();

			AlloyId = GetCustomerAlloyId(AgentSession.SessionId, "LEWIS", new DateTime(1980, 02, 02));

			CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

			decimal withdrawFromCard = 130;
			decimal withdrawFee = Client.GetFundsFee(long.Parse(CustomerSession.CustomerSessionId), withdrawFromCard, FundType.Debit, MgiContext).NetFee;

			Funds fund = new Funds() { Amount = withdrawFromCard, Fee = withdrawFee };

			Client.Withdraw(CustomerSession.CustomerSessionId, fund, MgiContext);
			
			ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
			
			MgiContext.IsReferral = false;
			
			ShoppingCartCheckoutStatus status = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
			
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

			CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

			decimal LoadToCard = 30;
			decimal LoadFee = Client.GetFundsFee(long.Parse(CustomerSession.CustomerSessionId), LoadToCard, FundType.Credit, MgiContext).NetFee;

			Funds fund = new Funds() { Amount = LoadToCard, Fee = LoadFee };

			Client.Load(CustomerSession.CustomerSessionId, fund, MgiContext);
			ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
			MgiContext.IsReferral = false;
			ShoppingCartCheckoutStatus status;
			try
			{
				status = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
			}
			catch (Exception)
			{
				status = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
			}
			Assert.That(status, Is.EqualTo(ShoppingCartCheckoutStatus.Completed));
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

			CustomerSession = Client.InitiateCustomerSession(AgentSession.SessionId, AlloyId, 3, MgiContext);

			decimal withdrawFromCard = 130;
			decimal withdrawFee = Client.GetFundsFee(long.Parse(CustomerSession.CustomerSessionId), withdrawFromCard, FundType.Debit, MgiContext).NetFee;

			Funds fund = new Funds() { Amount = withdrawFromCard, Fee = withdrawFee };

			Client.Withdraw(CustomerSession.CustomerSessionId, fund, MgiContext);
			ShoppingCart cart = Client.ShoppingCart(CustomerSession.CustomerSessionId);
			
			MgiContext.IsReferral = false;
			ShoppingCartCheckoutStatus status;
			try
			{
				status = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
			}
			catch (Exception)
			{
				status = Client.Checkout(CustomerSession.CustomerSessionId, cart.GprCardTotal, "", ShoppingCartCheckoutStatus.InitialCheckout, MgiContext);
			}
			
			Assert.That(status, Is.EqualTo(ShoppingCartCheckoutStatus.CashOverCounter));
		}
		#endregion

	}
}
