// -----------------------------------------------------------------------
// <copyright file="TimeMachineStrategy.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
namespace MGI.TimeStamp
{
    /// <summary>
    /// The TimeMachine time strategy. It starts with the time it is created.
    /// You can then
    ///     - add time (months, days, years etc.)
    ///     - go to any time by setting the new DateTime.
    ///     
    /// After manipulating the time, when you get the time through this implementation, it will give you the new manipulated time.
    /// </summary>
    public class TimeMachineStrategy : ITimeStrategy
    {
        private long _Time;
        private static int _DAYS_IN_A_WEEK = 7;
        
        /**
         * Create the TimeMachine strategy with the current DateTime.
         */
        public TimeMachineStrategy() {
            _Time = DateTime.UtcNow.Ticks;
        }

        // TimeStrategy interface implementation - provides the current ticks ...
        public long Ticks()
        {
            return _Time; // whatever the current time is !!
        }

        // ------- time manipulation functions ----
        public DateTime CurrentDateTime
        {
            private get {
                return new DateTime(_Time);
            }

            set {
                _Time = value.Ticks;
            }
        }
        
        public void AddMonths(int months) {
            _Time = CurrentDateTime.AddMonths(months).Ticks;
        }

        public void AddYears(int years) {
            _Time = CurrentDateTime.AddYears(years).Ticks;
        }

        public void AddDays(int days) {
            _Time = CurrentDateTime.AddDays(days).Ticks;
        }

        public void AddHours(int hours) {
            _Time = CurrentDateTime.AddHours(hours).Ticks;
        }

        public void AddMinutes(int minutes) {
            _Time = CurrentDateTime.AddMinutes(minutes).Ticks;
        }

        public void AddWeeks(int weeks) {
            _Time = CurrentDateTime.AddDays(_DAYS_IN_A_WEEK * weeks).Ticks;
        }

        // ------- end of time manipulation functions ----
    }
}
