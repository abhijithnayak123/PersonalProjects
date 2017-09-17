using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MGI.Common.Service
{
	public class GenericTimer
	{
		private long frequencyMilliSeconds;
		private Timer myTimer;
		private Action actionToBePerformed;
		private TimeSpan firstTimeInterval;
		private bool timerTaskSuccess;

		public GenericTimer(double frequency, DateTime scheduledTime, Action actionToBePerformed)
		{
			var startTimeDiff = TimeSpan.Compare(DateTime.Now.TimeOfDay, scheduledTime.TimeOfDay); // returns -1 if t1 is shorter than t2

			firstTimeInterval = new TimeSpan();
			switch (startTimeDiff)
				{
					case 1: firstTimeInterval = DateTime.Now.Subtract(scheduledTime); //gets time interval to start timer
						break;
					case 0: firstTimeInterval =  new TimeSpan(0);
						break;
					case -1: firstTimeInterval = scheduledTime.Subtract(DateTime.Now);
						break;

				}
			//frequencyMilliSeconds = (frequencyHours * 60 * 60000);
			frequencyMilliSeconds = (long) frequency;
			this.actionToBePerformed = actionToBePerformed;
		}

		public GenericTimer(double frequencyMinutes, Action actionToBePerformed) //This is for CP Monitor
	    {
			this.frequencyMilliSeconds = (long) frequencyMinutes;
			firstTimeInterval = new TimeSpan(0);
			this.actionToBePerformed = actionToBePerformed;
	    }

		public void ServiceTimeStart()
		{

			var timerCallBack = new TimerCallback(OnTimedEvent);
			myTimer = new Timer(timerCallBack, null, (long)firstTimeInterval.TotalMilliseconds, frequencyMilliSeconds);
			timerTaskSuccess = false;

		}

		private void OnTimedEvent(object state)
		{
			try
			{
				this.actionToBePerformed();
				timerTaskSuccess = true;
			}
			catch(Exception ex) //if exception occurs in archiving process set flag as false
			{
				timerTaskSuccess = false;
			}
			finally
			{
				if(!timerTaskSuccess)
				{
					myTimer.Change(Timeout.Infinite, Timeout.Infinite); 
					myTimer.Dispose(); //timer stops 
				}
			}
		}

	}
}
