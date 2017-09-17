// -----------------------------------------------------------------------
// <copyright file="IEventPublisher.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Biz.Events.Contract
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INexxoBizEventPublisher
    {
        void Publish(string channelPartner, NexxoBizEvent bizEvent);
    }
}
