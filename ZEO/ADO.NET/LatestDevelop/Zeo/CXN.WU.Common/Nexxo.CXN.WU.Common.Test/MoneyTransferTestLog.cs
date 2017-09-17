using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;
using NHibernate;
using NHibernate.Context;

using MGI.Cxn.MoneyTransfer.WU.Impl;
using MGI.Cxn.MoneyTransfer.WU.Data;
using MGI.Cxn.MoneyTransfer.Data;
using MGI.Cxn.MoneyTransfer.Contract;
using MGI.Cxn.MoneyTransfer.WU.Contract;

namespace MGI.Cxn.MoneyTransfer.WU.Test
{
    public class MoneyTransferTestLog
    {
        protected IWUMoneyTransfer Processor;
        
        [SetUp]
        public void Setup()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            Processor = (IWUMoneyTransfer)ctx.GetObject("MoneyTransfer");
        }

        [Test]
        public void Can_AddLog()
        {
            WesternUnionLog log = new WesternUnionLog();

            log.ReceiverFirstName = "reciever-1";
            log.SenderFirstName = "sender-1";
            log.SenderAddressCity = "Banglore";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                CallSessionContext.Bind(session);
                using (ITransaction txn = session.BeginTransaction())
                {
                    bool saveResult = Processor.SaveWULog(log);
                    txn.Commit();

                    Assert.IsTrue(saveResult);
                }
            }
        }
    

        [Test]
        public void Should_not_AddLog()
        {
            try
            {
                WesternUnionLog log = new WesternUnionLog();

                log.ReceiverFirstName = "reciever-1";
                log.SenderFirstName = "sender-1";
                log.SenderAddressCity = "0123456789 0123456789 0123456789 0123456789 0123456789";

                using (ISession session = NHibernateHelper.OpenSession())
                {
                    CallSessionContext.Bind(session);
                    using (ITransaction txn = session.BeginTransaction())
                    {
                        bool saveResult = Processor.SaveWULog(log);
                        txn.Commit();

                        Assert.IsTrue(saveResult);
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.IndexOf("could not insert") >= 0);
            }
            
        }

    }
}
