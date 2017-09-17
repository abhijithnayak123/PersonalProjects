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
            TLogEntry objlog = new TLogEntry();
            objlog.ApplicationServer = "test";

            Description descx= new Description();
            descx._description ="test description 2";
            descx._descriptionLog = "transactionLogging";

            List<Description> descobj = new List<Description>();

            descobj.Add(descx);

            objlog.Desc = descobj;

            objlog.EventSeverity = TLogEntryLevel.CRITICAL;
            objlog.FunctionName = "Test fail";
            objlog.GlobalIdentifiers = "global test indentifier";
            objlog.HostDevice = "test local";
            objlog.Timestamps = DateTime.Now;


           
            TLogImpl obj = new TLogImpl();

            obj.Savelog(objlog);
            obj.Readlog();
            Console.ReadLine();

        }
    }
}
