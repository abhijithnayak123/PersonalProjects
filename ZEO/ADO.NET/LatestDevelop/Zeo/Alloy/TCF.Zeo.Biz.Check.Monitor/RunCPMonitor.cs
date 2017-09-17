using System;
using System.Diagnostics;

namespace TCF.Zeo.Biz.Check.Monitor
{
    public class RunCPMonitor
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Time :" + DateTime.Now);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            ICPMonitor cpMonitor = new CPMonitor();
            cpMonitor.Run(args);

            watch.Stop();

            Console.WriteLine("End Time :" + DateTime.Now);

            Console.WriteLine("Elapsed MilliSeconds :" + watch.ElapsedMilliseconds);
        }
    }
}
