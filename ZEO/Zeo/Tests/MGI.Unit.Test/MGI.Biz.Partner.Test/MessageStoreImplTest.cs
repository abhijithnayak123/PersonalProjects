using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Unit.Test.MGI.Biz.Partner.Test
{
	[TestFixture]
	public class MessageStoreImplTest : BaseClass_Fixture
	{
		public IMessageStore BIZPartnerUserService { get; set; }
		
		[Test]
		public void GetMessage()
		{
			MGIContext mgiContext = new MGIContext();
			mgiContext.ChannelPartnerId = 1;
			Message message = BIZPartnerUserService.GetMessage("1006.100.8000", mgiContext);
			Assert.IsNotNull(message.Content);
		}
	}
}
