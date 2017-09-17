using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MGI.Peripheral.Printer.Contract;
using MGI.Peripheral.Queue.Contract;
using MGI.Peripheral.CheckPrinter.Contract;
using MGI.Peripheral.CheckFranking.Contract;

namespace MGI.Peripheral.Queue.Impl
{
	public class JobQueue : IQueue
	{
		public void SetPrintJob(PrintData printData)
		{
			AddJob(printData, "Print");
		}
		public bool SetScanJob(String imageFormat)
		{
			return AddJob(imageFormat, "Scan");
		}
        public void SetCheckPrintJob(CheckPrintData printData)
        {
            AddJob(printData, "CheckPrint");
        }

        public void SetCheckFrankJob(CheckFrankData printData)
        {
            AddJob(printData, "CheckFrank");
        }
        
		public PrintData GetPrintJob()
		{
			return (PrintData)GetJob("Print");
		}
		public String GetScanJob()
		{
			return (String)GetJob("Scan");
		}
        public CheckPrintData GetCheckPrintJob()
        {
            return (CheckPrintData)GetJob("CheckPrint");
        }

        public CheckFrankData GetCheckFrankJob()
        {
            return (CheckFrankData)GetJob("CheckFrank");
        }

		public void SetJobComplete()
		{
			Queue.Jobs.RemoveAt(0);
			Queue.JobStatus.RemoveAt(0);
			Queue.JobType.RemoveAt(0);
		}

		private Object GetJob(String jobType)
		{
			Object retObj = null;
			while (true)
			{
				bool jobInProgress = false;
				if (Queue.Jobs.Count() <= 0)
					break;
				for (int i = 0; i < Queue.JobStatus.Count(); i++)
				{
					//Check if there is a job in progress
					if (Queue.JobStatus[i] == "Processing")
						jobInProgress = true;
				}
				if (jobInProgress == false)
				{
					Queue.JobStatus[0] = "Processing";
					//Do Print of First Job
					Trace.WriteLine("Performing Job in Queue", DateTime.Now.ToString());
					if (Queue.JobType[0] == "Print" && jobType == "Print")
						retObj = Queue.Jobs[0];
					if (Queue.JobType[0] == "Scan" && jobType == "Scan")
						retObj = Queue.Jobs[0];
                    if (Queue.JobType[0] == "CheckPrint" && jobType == "CheckPrint")
                        retObj = Queue.Jobs[0];
                    if (Queue.JobType[0] == "CheckFrank" && jobType == "CheckFrank")
                        retObj = Queue.Jobs[0];
                    break;
				}
				else
				{
					Thread.Sleep(1000 * 3);
					Trace.WriteLine("Waiting for a job to be completed", DateTime.Now.ToString());
				}
			}
			//Wait for a few seconds
			return retObj;
		}

		private bool AddJob(Object obj, String jobType)
		{
			if (jobType == "Scan" && obj == null) obj = String.Empty;
			//We Do Not want to add two scan jobs
			if (jobType == "Scan")
			{
				for (int i = 0; i < Queue.JobType.Count(); i++)
				{
					//Check if there is a job in progress
					if (Queue.JobType[i] == "Scan")
						return false;//Resource Busy, Scan Job Already in
				}
			}

            //Dont want to add more than one check print job at a time
            if (jobType == "CheckPrint")
            {
                for (int i = 0; i < Queue.JobType.Count(); i++)
                {
                    //Check if there is a check print job in progress
                    if (Queue.JobType[i] == "CheckPrint")
                        return false;//Resource Busy, Check Print Job Already in
                }
            }

            if (jobType == "CheckFrank")
            {
                for (int i = 0; i < Queue.JobType.Count(); i++)
                {
                    //Check if there is a check print job in progress
                    if (Queue.JobType[i] == "CheckFrank")
                        return false;//Resource Busy, Check Print Job Already in
                }
            }
     
			Queue.Jobs.Add(obj);
			Queue.JobStatus.Add("Pending");
			Queue.JobType.Add(jobType);
			Trace.WriteLine("Print Jobs = " + Queue.Jobs.ToString(), DateTime.Now.ToString());
			return true;//SUCCESS
		}
	}
}
