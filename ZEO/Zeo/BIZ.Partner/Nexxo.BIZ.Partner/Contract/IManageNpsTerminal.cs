using System;
using System.Collections.Generic;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Contract
{
	public interface IManageNpsTerminal
	{
        /// <summary>
        /// This method is to create the NPS terminal[PNTR Database]
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent Session</param>
        /// <param name="npsTerminal">A transient instance of NpsTerminal[Class] </param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>NpsTerminal Create Status</returns>
		bool Create(long agentSessionId, NpsTerminal npsTerminal, MGIContext mgiContext);

        /// <summary>
        /// This method is to update the NPS terminal[PNTR Database]
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent Session</param>
        /// <param name="npsTerminal">A transient instance of NpsTerminal[Class] containing Updated State</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>NpsTerminal Updated Status</returns>
		bool Update(long agentSessionId, NpsTerminal npsTerminal, MGIContext mgiContext);

        /// <summary>
        /// This method is to fetch the NPS terminal details by Id
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent Session</param>
        /// <param name="Id">This is the unique identifier for NPS Terminal</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>NpsTerminal Details</returns>
		NpsTerminal Lookup(string agentSessionId, long Id, MGIContext mgiContext);

        /// <summary>
        /// This method is to get the NPS terminal details by GUID.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent Session</param>
        /// <param name="Id">This is GUID of NPS terminal</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>NpsTerminal Details</returns>
		NpsTerminal Lookup(long agentSessionId, Guid Id, MGIContext mgiContext);

        /// <summary>
        /// This method is to get the NPS terminal details by IP adddress.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent Session</param>
        /// <param name="ipAddress">This is IP address of system</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>NpsTerminal Details</returns>
		NpsTerminal Lookup(long agentSessionId, string ipAddress, MGIContext mgiContext);

        /// <summary>
        /// This method is to get the NPS terminal details by terminal name and channel partner details
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent Session</param>
        /// <param name="name">This is the terminal name.</param>
        /// <param name="channelPartner">A transient instance of ChannelPartner[Class]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>NpsTerminal Details</returns>
		NpsTerminal Lookup(long agentSessionId, string name, ChannelPartner channelPartner, MGIContext mgiContext);

        /// <summary>
        /// This method is to get the NPS terminal details by location id.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent Session</param>
        /// <param name="locationId">This is the unique identifier for location</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of NpsTerminal Details</returns>
		List<NpsTerminal> GetByLocationID(long agentSessionId, long locationId, MGIContext mgiContext);
	}
}
