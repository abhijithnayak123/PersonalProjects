
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.TransactionalLogging.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTranLogs
{
    public class Program
    {
        public static void Main(string[] args)
        {
			TransactionLogEntry objlog = new TransactionLogEntry();
            objlog.ApplicationServer = TransactionLogApplicationServer.ALLOY;

            //Description descx= new Description();
            //descx._description ="test description .net 4 framework";
            //descx._descriptionLog = "transaction log with .net 40";            

            //List<Description> descobj = new List<Description>();
            //descobj.Add(descx);

            //objlog.Desc = descobj;
            objlog.EventSeverity = EventSeverity.CRITICAL;
            objlog.MethodName = "Test .net 4 framework";            
            

            objlog.HostDevice = "test local .net 4 framework";
            objlog.Timestamps = DateTime.Now.ToString();


			TransactionLogImpl obj = new TransactionLogImpl();

            obj.Savelog(objlog);
           // obj.ReadSinglelog();
            Console.ReadLine();

        }
    }
}
