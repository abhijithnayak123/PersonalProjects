
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.TransactionalLogging.Impl;
using NUnit.Framework;
using PNUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Common.TransactionalLogging.UnitTests
{
    [TestFixture]
    public class TransactionLoggingTest
    {
        TransactionLogEntry objlog;
        private void Setup()
        {
			objlog = new TransactionLogEntry();
            //Description descx = new Description();            
            //descx._description = "test description 2";
            //descx._descriptionLog = "transactionLogging";          

            //List<Description> descobj = new List<Description>();
            //descobj.Add(descx);

            //objlog.ApplicationServer = TLogApplicationServer.ALLOY;
            //objlog.Desc = descobj;
            objlog.EventSeverity = EventSeverity.CRITICAL;
            objlog.MethodName = "Test fail";        
            objlog.HostDevice = "test local";
            objlog.Timestamps = DateTime.Now.ToString();

        }

        [Test]
        public void SaveTransactionLogging()
        {
            Setup();

			TransactionLogImpl obj = new TransactionLogImpl();
            obj.Savelog(objlog);

            //  Assert.AreNotEqual(null, impl.PartnerCustomerService.Lookup(111));
        }

        [Test]
        public void GetTransactionLogs()
        {
            Setup();

			TransactionLogImpl obj = new TransactionLogImpl(); 
           var lstlogs  = obj.Readlog();
           //Assert.AreEqual("testServer", lstlogs.Result[0].ApplicationServer);

         //  Assert.AreEqual(1, lstlogs.Result.Count());

        }

         [Test]
        public void LogactivityExists()
        {
			TransactionLogImpl obj = new TransactionLogImpl();
            var lstlogs = obj.Readlog();
            Assert.IsNotNull(lstlogs);
        }

    }

}
