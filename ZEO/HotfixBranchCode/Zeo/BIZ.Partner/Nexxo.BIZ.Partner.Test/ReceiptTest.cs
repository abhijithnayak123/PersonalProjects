using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
//using Moq;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Data;

using PTNRData = MGI.Core.Partner.Data;
using PTNRContract = MGI.Core.Partner.Contract;

using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Impl;
using Spring.Context;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Test
{
	[TestFixture]
	public class ReceiptTest : AbstractPartnerTest
	{
		MGI.Biz.Receipt.Contract.IReceiptService BIZPtnrReceiptService { get; set; }
		private static string BIZ_PartnerReceiptService = "BIZReceiptService";

		[SetUp]
		public void Setup()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			BIZPtnrReceiptService = (MGI.Biz.Receipt.Contract.IReceiptService)ctx.GetObject(BIZ_PartnerReceiptService);
		}
		//private ReceiptServiceImpl svc = new ReceiptServiceImpl();
		//private Mock<PTNRContract.IChannelPartnerService> moqCPSvc = new Mock<PTNRContract.IChannelPartnerService>();
		//private Mock<PTNRContract.ITerminalService> moqTerminalSvc = new Mock<PTNRContract.ITerminalService>();

		//private Mock<PTNRContract.ITransactionService<PTNRData.Transactions.Check>> moqCheckSvc = new Mock<PTNRContract.ITransactionService<PTNRData.Transactions.Check>>();
		//private Mock<CXNCheckContract.ICheckProcessor> moqCxnCheckSvc = new Mock<CXNCheckContract.ICheckProcessor>();
		//private Mock<PTNRContract.ITransactionService<PTNRData.Transactions.Funds>> moqFundsSvc = new Mock<PTNRContract.ITransactionService<PTNRData.Transactions.Funds>>();

		//[TestFixtureSetUp]
		//public void testFixtureSetup()
		//{
		//    svc.PtnrCheckSvc = moqCheckSvc.Object;
		//    svc.PtnrFundsSvc = moqFundsSvc.Object;
		//    svc.CxnCheckSvc = moqCxnCheckSvc.Object;
		//    svc.ChannelPartnerSvc = moqCPSvc.Object;
		//    svc.ReceiptRepo = new ReceiptTemplateRepo("http://172.18.100.13:8081/", "Nxo_ReceiptsRepo");
		//    svc.TerminalSvc = moqTerminalSvc.Object;
		//}

		[Test]
		public void GetCheckReceiptTest()
		{
            long customerSessionId = 0L;
            long agentSessionId = 0L;
            List<MGI.Biz.Receipt.Data.Receipt> receipt = BIZPtnrReceiptService.GetCheckReceipt(agentSessionId, customerSessionId, 1000000125, true, new MGIContext() { });
			Assert.IsTrue(receipt != null);

		}

		[Test]
		public void GetFundsReceiptTest()
		{
            long customerSessionId = 0L;
            long agentSessionId = 0L;
            MGIContext mgiContext = new MGIContext();
			mgiContext.ProcessorId = 14;
			mgiContext.ChannelPartnerId = 34;
            List<MGI.Biz.Receipt.Data.Receipt> receipt = BIZPtnrReceiptService.GetFundsReceipt(agentSessionId, customerSessionId, 1000000050, true, mgiContext);
			Assert.IsTrue(receipt != null);

		}
	}
}
