// -----------------------------------------------------------------------
// <copyright file="EventPublisher.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Biz.Events.Impl
{
    using MGI.Biz.Events.Contract;

    using System.Collections.Generic;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DefaultNexxoBizEventPublisher : INexxoBizEventPublisher
    {
        // this is a dictionary of dictionary
        // the first key is teh channel partner.
        // for a channel partner you can find the list of listeners for an event.
        public Dictionary<string, Dictionary<string, INexxoBizEventListener>> NexxoBizEventListeners {private get; set;}

        public void Publish(string channelPartner, NexxoBizEvent bizEvent) { 
            // see if the dictionary is present for the channel partner.
            if (NexxoBizEventListeners.ContainsKey(channelPartner) && NexxoBizEventListeners[channelPartner] != null)
            {
                Dictionary<string, INexxoBizEventListener> cpListeners = NexxoBizEventListeners[channelPartner];

                if (null == cpListeners)
                {
                    return;
                }

                // now check, if the event has a listener
				if (cpListeners.ContainsKey(bizEvent.Name))
				{
					INexxoBizEventListener NxoBizEventListener = cpListeners[bizEvent.Name];
					if (null != NxoBizEventListener)
					{
						NxoBizEventListener.Notify(bizEvent); // this will notify the biz event listener. No-need to handle exceptions??
					}
				}
            }
            else
                return;
        }
    }
}
