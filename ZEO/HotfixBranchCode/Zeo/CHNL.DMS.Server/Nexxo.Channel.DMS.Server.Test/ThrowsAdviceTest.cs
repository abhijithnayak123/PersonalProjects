using System;
using System.Collections.Generic;
using System.ServiceModel;

using NUnit.Framework;
using Moq;

using MGI.Channel.DMS.Server.Svc.Advice;
using MGI.Common.Sys;
using CommonSysException = MGI.Common.Sys.NexxoException;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using Spring.Testing.NUnit;

namespace MGI.Channel.DMS.Server.Test
{
	[TestFixture]
    public class ThrowsAdviceTest : AbstractTransactionalSpringContextTests
	{

        protected override string[] ConfigLocations
        {
            get { return new string[] { "assembly://MGI.Channel.DMS.Server.Test/MGI.Channel.DMS.Server.Test/hibernate.cfg.xml" }; }

        }    
        
		private ThrowsFaultAdvice _adv = new ThrowsFaultAdvice();
		private Mock<IMessageStore> _mokMsgStore = new Mock<IMessageStore>();

		[SetUp]
		public void fixtSetup()
		{
			_adv.MessageStore = _mokMsgStore.Object;
		}

		[Test]
		public void TestBasicPath()
		{
			var context = new Dictionary<string, string>();
			context.Add( "ChannelPartnerId", "28" );
			context.Add( "Language", "EN" );
			Object[] args = new Object[]{ "101", "22344", context};

			var nEx = new CommonSysException( 1001, 4001, "this is not OK" );
			//_mokMsgStore.Setup( m => m.Lookup( 28, "1001.4001", Language.EN ) ).Returns( "this is OK" );
            _mokMsgStore.Setup(m => m.Lookup(28, "1001.4001", Language.EN)).Returns(new Message() { });

			CatchFaultException( () => _adv.AfterThrowing( null, args, null, nEx ), "this is OK" );
		}

		[Test]
		public void NoContextPassed()
		{
			Object[] args = new Object[] { "101", "22344" };
			var nEx = new CommonSysException( 1001, 4001, "this is not OK" );
//			_mokMsgStore.Setup( m => m.Lookup( 1, "1001.4001", Language.EN ) ).Returns( "ch part nexxo" );
            _mokMsgStore.Setup(m => m.Lookup(1, "1001.4001", Language.EN)).Returns(new Message() { });

			CatchFaultException( () => _adv.AfterThrowing( null, args, null, nEx ), "ch part nexxo" );
		}

		[Test]
		public void NoMessageFound()
		{
			Object[] args = new Object[] { "101", "22344" };
			var nEx = new CommonSysException( 1001, 4001, "this is not OK" );
//			_mokMsgStore.Setup( m => m.Lookup( 1, "1001.4001", Language.EN ) ).Returns( "" );
            _mokMsgStore.Setup(m => m.Lookup(1, "1001.4001", Language.EN)).Returns(new Message() { });

			CatchFaultException( () => _adv.AfterThrowing( null, args, null, nEx ), "this is not OK" );
		}

		[Test]
		public void InnerExIsNOTProviderEx()
		{
			Object[] args = new Object[] { "101", "22344" };
			var nEx = new CommonSysException( 1001, 4001, "this is not OK", new Exception( "this is not a provider exception" ) );
			
//			_mokMsgStore.Setup( m => m.Lookup( 1, "1001.4001", Language.EN ) ).Returns( "some message" );
            _mokMsgStore.Setup(m => m.Lookup(1, "1001.4001", Language.EN)).Returns(new Message() { });

			CatchFaultException( () => _adv.AfterThrowing( null, args, null, nEx ), "some message" );
		}

		[Test]
		public void InnerExIsProviderEx()
		{
			Object[] args = new Object[] { "101", "22344" };
			var nEx = new CommonSysException( 1001, 4001, "this is not OK", new ProviderException(1,"E201","what?") );

            //_mokMsgStore.Setup( m => m.Lookup( 1, "1001.4001", Language.EN ) ).Returns( "some message" );
            //_mokMsgStore.Setup( m => m.Lookup( 1, "1001.4001.1.E201", Language.EN ) ).Returns( "what?" );

            _mokMsgStore.Setup(m => m.Lookup(1, "1001.4001", Language.EN)).Returns(new Message() { });
            _mokMsgStore.Setup(m => m.Lookup(1, "1001.4001.1.E201", Language.EN)).Returns(new Message() { });

			CatchFaultException( () => _adv.AfterThrowing( null, args, null, nEx ), "what?" );
		}

		private void CatchFaultException( TestDelegate code, string detailsMessage )
		{
			try
			{
				code();
				Assert.IsTrue( false );
			}
			catch ( FaultException<NexxoSOAPFault> fEx )
			{
				Assert.IsTrue( fEx.Detail.Details == detailsMessage );
			}
		}
	}
}
