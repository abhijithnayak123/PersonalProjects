// -----------------------------------------------------------------------
// <copyright file="DefaultTimeProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
namespace MGI.TimeStamp
{
    
    /// <summary>
    /// The default time strategy. 
    /// </summary>
    public class DefaultTimeStrategy : ITimeStrategy
    {
        /**
         * Get the current ticks based on the MS-core System DateTime. 
         */
        public long Ticks()
        {
            return DateTime.UtcNow.Ticks;
        }
    }
}
