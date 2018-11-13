using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;

using NUnit.Framework;

using Spring.Context;
using Spring.Context.Support;
using MGI.Channel.Shared.Server.Data;
using Spring.Testing.NUnit;

namespace MGI.Channel.DMS.Server.Test
{
    [TestFixture]
    public class CashEngineTest : AbstractTransactionalSpringContextTests
    {
       
      private static string DESKTOP_ENGINE = "DesktopEngine";
		private Dictionary<string, object> _context = new Dictionary<string, object>();
        private Dictionary<string, string> context = new Dictionary<string, string>();

        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Channel.DMS.Server.Test/MGI.Channel.DMS.Server.Test/hibernate.cfg.xml" }; }    
            
        }
  
        IDesktopService DeskTopTest { get; set; }
        [SetUp]
        public void Setup()
        {
            IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            DeskTopTest = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);
        }
        //[SetUp]
        //public void Setup()
        //{
        //    IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
        //    DeskTopTest = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);

        //    _context.Add("ChannelPartnerId", "34");
        //    _context.Add("Language", "EN");
        //}
        //private static string DESKTOP_ENGINE = "DesktopEngine";

        //IDesktopService DesktopEngine { get; set; }
        //public CashEngineTest()
        //{
        //    IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
        //    DesktopEngine = (IDesktopService)ctx.GetObject(DESKTOP_ENGINE);
        //    DesktopEngine.SetSelf(DesktopEngine);
        //    //log4net.Config.XmlConfigurator.Configure();
        //}
        [Test]
        public void CashInCashOutPurseTest()
        {
			//AgentSession Session = null; //DeskTopTest.Authenticate("anil", "Anil@123", "centris", "Anil");

            //CustomerAuthentication customer = new CustomerAuthentication();

            //customer.AlloyID = 1000000000001010;
            ////string customerSessionId = "1000000000000240";
            //context.Add("ChannelPartnerId", "34");
            //context.Add("Language", "EN");
			//CustomerSession customerSession
			//		= DeskTopTest.InitiateCustomerSession(Session.SessionId, customer, _context);

           // Purse purse = DeskTopTest.GetPurse(customerSessionId,customer.AlloyID, Session.SessionId, context);

			//decimal originalBakance = purse.Balance;

			//purse.Balance = 10;

			//CashTransaction afterBalance = DeskTopTest.CashInPurse( customerSession.CustomerSessionId, purse );

			//Assert.AreEqual( afterBalance.Purse.Balance, originalBakance + 10 );

			//afterBalance = DeskTopTest.CashOutPurse( customerSession.CustomerSessionId, afterBalance.Purse );

			//Assert.AreEqual( afterBalance.Purse.Balance, 0 );
        }
    }
}