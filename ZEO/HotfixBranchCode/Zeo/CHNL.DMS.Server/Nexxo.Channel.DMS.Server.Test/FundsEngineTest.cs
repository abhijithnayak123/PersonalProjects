using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MGI.Channel.DMS.Server.Contract;
using Spring.Context;
//using Nexxo.Core.ShoppingCart.Data;
using MGI.Channel.DMS.Server.Data;
using NHibernate;
using MGI.Channel.Shared.Server.Data;
using Spring.Testing.NUnit;

namespace MGI.Channel.DMS.Server.Test
{
    [TestFixture]
    public class FundsEngine : AbstractTransactionalSpringContextTests
    {

        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Channel.DMS.Server.Test/MGI.Channel.DMS.Server.Test/hibernate.cfg.xml" }; }

        }      
        
        private static string DESKTOP_ENGINE = "DesktopEngine";
        //ISession session;
        //private static string CartId = "D5ABC0FA-0C4D-498C-9EFA-9663A816CDB1";
        IDesktopService DeskTopTest { get; set; }

        [SetUp]
        public void Setup()
        {
            IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            DeskTopTest = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);
            //session = (ISession)ctx.GetObject("session");
        }

		//[Test]
		//public void BeginCreditTest()
		//{
		//	AgentSession agentSession = null;// DeskTopTest.Authenticate("manoj", "Mano@123", "centris", "Anil");

		//	CustomerAuthentication customer = new CustomerAuthentication();

		//	//	customer.AlloyID = "1000000008582551";

		//	Dictionary<string, object> context = new Dictionary<string, object>();
		//	context.Add( "ChannelPartnerId", "28" );
		//	context.Add( "Language", "EN" );

		//	CustomerSession customerSession = DeskTopTest.InitiateCustomerSession( agentSession.SessionId, customer, context );
		//	//DeskTopTest.AuthenticateCard(1212121, "", "", "", agentSession.SessionId, customerSession.CustomerSessionId);

		//	//DeskTopTest.BeginCredit(Int64.Parse(customer.AlloyID), "", 10, 2, agentSession.SessionId, customerSession.CustomerSessionId);
		//}
    }
}