// -----------------------------------------------------------------------
// <copyright file="Clock.cs" company="Nexxo Financial">
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
namespace MGI.TimeStamp
{
    /**
     * The abstraction to get the current DateTime. The Nexxo applications should use the "Nexxo System Clock" to 
     * get the time. There should be no direct calls to the System DateTime functions.
     */
    public static class Clock
    {
        private static readonly ITimeStrategy DefaultTimeStrategy = new DefaultTimeStrategy();
        private static ITimeStrategy _TimeStrategy = DefaultTimeStrategy;

        private static Boolean _AllowTimeTravel = false;

        public static Boolean AllowTimeTravel {
            private set { _AllowTimeTravel = value; }
            get { return _AllowTimeTravel; }
        }

        /**
         * Change the time strategy for the Clock. Use with caution, as this can change the Application DateTime behavior...
         */
        public static ITimeStrategy TimeStrategy 
        {
            // is there a point getting the current time strategy ?? not providing the getter for now !!
            set {
                _TimeStrategy = value;
                AllowTimeTravel = (_TimeStrategy is TimeMachineStrategy) ? true : false;
            }
        }

        /**
         * Get the current date time of the Clock. The Clock is dependent on the time provider.
         */
        public static DateTime DateTime
        {
            get {
                return new DateTime(_TimeStrategy.Ticks());
            }
        }

        public static DateTime Now()
        {
            return DateTime;
        }

        public static DateTime Now(String tz)
        {
            return (String.IsNullOrEmpty(tz))
                       ? Now()
                       : DateTimeWithTimeZone(tz);
        }

        /**
         * Reset the Clock time strategy to the default time strategy. 
         */
        public static void Reset() 
        {
            TimeStrategy = DefaultTimeStrategy;
        }
        /// <summary>
        /// Changes for timestamp
        /// </summary>
        /// <param name="timezone"></param>
        /// <returns></returns>
        public static DateTime DateTimeWithTimeZone(string timezone)
        {
            DateTime lastModifiedDate = DateTime.Now;
            try
            {
                lastModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(MGI.TimeStamp.Clock.DateTime, TimeZoneInfo.FindSystemTimeZoneById(timezone));
            }
            catch
            {
                lastModifiedDate = DateTime.Now;
            }
            return lastModifiedDate; 
        }
		public static DateTime ConvertDateTimeWithTimeZone(string destinationTimeZone, string sourceTimeZone, string mmddyy, string time)
		{
			DateTime DTWithTimeZone = DateTime.Now;
			try
			{
				int mm = Convert.ToInt32(mmddyy.Split('-')[0]);
				int dd = Convert.ToInt32(mmddyy.Split('-')[1]);
				int yy = Convert.ToInt32(mmddyy.Split('-')[2]);
				sourceTimeZone = GetTimeZoneId(sourceTimeZone);
				int hr = Convert.ToInt32(time.Substring(0, 2));
				int mn = Convert.ToInt32(time.Substring(2, 2));
				if (time.Substring(4).Equals("P"))
				{
					hr = hr + 12;
				}
				DTWithTimeZone = new DateTime(yy, mm, dd, hr, mn, 0);
				TimeZoneInfo sourcetimeZone = TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZone);
				TimeZoneInfo destinationtimeZone = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZone);
				DTWithTimeZone = TimeZoneInfo.ConvertTime(DTWithTimeZone, destinationtimeZone, sourcetimeZone);
			}
			catch 
			{

			}
			return DTWithTimeZone;

		}

		private static string GetTimeZoneId(string id)
		{
			string timeZoneId = "";
			try
			{
				Dictionary<string, string> timeZones = new Dictionary<string, string>();
				timeZones.Add("PST", "Pacific Standard Time");
				timeZones.Add("MST", "Mountain Standard Time");
				timeZones.Add("EST", "Eastern Standard Time");
				timeZones.Add("CST", "Central Standard Time");
				if (timeZones.ContainsKey(id))
				{
					timeZoneId = timeZones[id];
				}
			}
			catch 
			{

			}
			return timeZoneId;
		}
    }
}
