using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using Spring.Context;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Test
{
	public class ReceiptFixture : AbstractPartnerTest
	{
		//public IReceiptService BIZPartnerReceiptService { private get; set; }
		MGI.Biz.Receipt.Contract.IReceiptService BIZPtnrReceiptService { get; set; }
		private static string BIZ_PartnerReceiptService = "BIZReceiptService";

		[SetUp]
		public void Setup()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			BIZPtnrReceiptService = (MGI.Biz.Receipt.Contract.IReceiptService)ctx.GetObject(BIZ_PartnerReceiptService);
			//	session = (ISession)ctx.GetObject("session");
		}

		[Test]
		public void GetCheckReceiptTest()
		{
            long customerSessionId = 0L;
            long agentSessionId = 0L;
            MGIContext mgiContext = new MGIContext();
            List<MGI.Biz.Receipt.Data.Receipt> receipt = BIZPtnrReceiptService.GetCheckReceipt(agentSessionId, customerSessionId, 1000000071, true, mgiContext);

			Assert.IsTrue(receipt != null);
			
		}

		[Test]
		public void GetFundsReceiptTest()
		{
			//TODO: Revisit this to set proper context.
            long customerSessionId = 0L;
            long agentSessionId = 0L;
            MGIContext mgiContext = new MGIContext();
			mgiContext.ProcessorId = 14;
			mgiContext.ChannelPartnerId = 34;
            List<MGI.Biz.Receipt.Data.Receipt> receipt = BIZPtnrReceiptService.GetFundsReceipt(agentSessionId, customerSessionId, 1000000050, true, mgiContext);
			Assert.IsTrue(receipt != null);

			//List<MGI.Biz.Receipt.Data.Receipt> receipt = BIZPtnrReceiptService.GetFundsReceipt(1000000114, true, new Dictionary<string, object>() { });

			//Assert.IsTrue(receipt != null);
			
		}
	}
}
