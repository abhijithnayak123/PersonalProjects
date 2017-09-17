using System.Timers;

namespace MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl
{
	public class EpsonTimer
	{
		Timer _timer;
		bool scanTimeout = false;

		public EpsonTimer()
		{
			InitTimer();
		}

		public void InitTimer()
		{
			scanTimeout = false;
			_timer = new Timer(1000 * 20);
			_timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
			_timer.Enabled = true;
		}

		private void _timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			scanTimeout = true;
		}

		public bool HasTimedOut()
		{
			return scanTimeout;
		}
	}
}
