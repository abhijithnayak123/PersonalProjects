// -----------------------------------------------------------------------
// <copyright file="ITimeProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace MGI.TimeStamp
{
    

    /// <summary>
    /// The Time strategy - this will give the current number of ticks from epoch...
    /// </summary>
    public interface ITimeStrategy
    {
        long Ticks();
    }
}
