using Spring.Testing.NUnit;
using Moq;
using CXEContract = MGI.Core.CXE.Contract;
using PTNRContract = MGI.Core.Partner.Contract;
using PTNRImpl = MGI.Biz.Partner.Impl;
using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Check.Contract;
using CXNMoneyTransfer = MGI.Cxn.MoneyTransfer.Contract;
using MGI.Cxn.Customer.Contract;
using MGI.Cxn.Partner.TCF.Contract;
using MGI.Biz.Events.Contract;
using MGI.Cxn.BillPay.Contract;
using MGI.Core.CXE.Contract;
using CXNContract = MGI.Cxn.Fund.Contract;

namespace MGI.Unit.Test
{
	public class BaseClass_Fixture : AbstractTransactionalSpringContextTests
	{
		public static Moq.MockRepository _moqRepository { get; set; }

		public BaseClass_Fixture()
		{
			_moqRepository = new Moq.MockRepository(Moq.MockBehavior.Default);
		}

		protected override string[] ConfigLocations
		{
			get
			{
				return new string[] { "assembly://MGI.Unit.Test/MGI.Unit.Test/MGI.Unit.Test.Spring.xml" };
			}
		}

		public static Mock<CXEContract.IBillPayService> CXEBillPayService { get; set; }

		public static Mock<IFundProcessor> ProcessorRouter { get; set; }

		public static Mock<CXEContract.IFundsService> CXEFundsService { get; set; }

		public static Mock<CXEContract.IMoneyOrderService> CxeMoneyOrderSvc { get; set; }

		public static Mock<CXEContract.ICheckService> CxeCheckSvc { get; set; }

		public static Mock<ICheckProcessor> CheckProcessor { get; set; }

		public static Mock<CXEContract.IMoneyTransferService> CXEMoneyTransferService { get; set; }

		public static Mock<CXNMoneyTransfer.IMoneyTransfer> CXNMoneyTransferService { get; set; }

		public static Mock<PTNRContract.IManageUsers> ManageUserService { get; set; }

		public static Mock<PTNRContract.IAgentSessionService> AgentSessionService { get; set; }

		public static Mock<PTNRContract.ICustomerService> PTNRCustomerService { get; set; }

		public static Mock<IClientCustomerService> CxnClientCustomerService { get; set; }

		public static Mock<IFlushProcessor> GatewayService { get; set; }

		public static Mock<CXEContract.ICustomerService> CXECustomerService { get; set; }

		public static Mock<INexxoBizEventPublisher> EventPublisher { get; set; }

		public static Mock<IBillPayProcessor> BillPayService { get; set; }

		public static Mock<IBillPaySetupService> CXEBillPaySetup { get; set; }

		public static Mock<PTNRImpl.IProspectFieldValidator> ProspectFieldValidator { get; set; }

		public static Mock<Core.CXE.Contract.ICashService> CashService { get; set; }

                public static Mock<CXNContract.IFundProcessor> CXNFundsService { get; set; }
	
	}
}
