using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Contract;
using Spring.Context;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;
using Spring.Testing.NUnit;

namespace MGI.Channel.DMS.Server.Test
{
	[TestFixture]
	public class CustomerEngineTest : AbstractTransactionalSpringContextTests
	{


		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Channel.DMS.Server.Test/MGI.Channel.DMS.Server.Test/hibernate.cfg.xml" }; }

		}

		private static string DESKTOP_ENGINE = "DesktopEngine";
		public MGIContext mgiContext { get; set; }
		IDesktopService DeskTopTest { get; set; }

		[SetUp]
		public void Setup()
		{
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			DeskTopTest = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);

            mgiContext.ChannelPartnerId = 34;
            //mgiContext.Language = "EN";
		}

		[Test]
		public void RecordIdentificationConfirmation()
		{
			AgentSession agentSession = new AgentSession();  // DeskTopTest.Authenticate("Anil", "Anil@123", "centris", "Anil");
            Response response = new Response();
			CustomerSession customerSession = new CustomerSession();//DeskTopTest.InitiateCustomerSession(agentSession.SessionId, customer, _context);
			//string customerSessionId = "1234";
			//string agentSessionId = "13532";
			try
			{
                response = DeskTopTest.RecordIdentificationConfirmation(long.Parse(customerSession.CustomerSessionId), agentSession.SessionId, true, mgiContext);
			}
			catch 
			{
                Assert.AreEqual("Failed", Convert.ToString(response.Result), "Recording identification confirmation failed");
			}
		}

	}
}
