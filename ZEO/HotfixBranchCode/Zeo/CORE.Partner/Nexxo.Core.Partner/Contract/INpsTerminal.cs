using System;
using System.Linq;
using MGI.Core.Partner.Data;
using System.Collections.Generic;

namespace MGI.Core.Partner.Contract
{
	public interface INpsTerminal
	{

		/// <summary>
		/// This method is to create the NPS terminal
		/// </summary>
		/// <param name="npsTerminal">This is NPS terminal details</param>
		/// <returns>Created status of NPS terminal</returns>
		bool Create(NpsTerminal npsTerminal);

		/// <summary>
		/// This method is to update the NPS terminal
		/// </summary>
		/// <param name="npsTerminal">This is NPS terminal details</param>
		/// <returns>Updated status of NPS terminal</returns>
		bool Update(NpsTerminal npsTerminal);

		/// <summary>
		/// This method is to get the NPS terminal details by Id
		/// </summary>
		/// <param name="Id">This is unique identifier of NPS terminal</param>
		/// <returns>NPS terminal details</returns>
		NpsTerminal Lookup(long Id);

		/// <summary>
		/// This method is to get the NPS terminal details by GUID
		/// </summary>
		/// <param name="Id">This is GUID of NPS terminal</param>
		/// <returns>NPS terminal details</returns>
		NpsTerminal Lookup(Guid Id);

		/// <summary>
		/// This method is to get the NPS terminal details by IP adddress
		/// </summary>
		/// <param name="ipAddress">This is IP address of system</param>
		/// <returns>NPS terminal details</returns>
		NpsTerminal Lookup(string ipAddress);

		/// <summary>
		/// This method is to get the NPS terminal details by terminal name and channel partner details
		/// </summary>
		/// <param name="name">This terminal name</param>
		/// <param name="channelPartner">This is channel partner details</param>
		/// <returns>NPS terminal details</returns>
		NpsTerminal Lookup(string name, ChannelPartner channelPartner);

		/// <summary>
		/// This method is to get the NPS terminal details by location id
		/// </summary>
		/// <param name="locationPK">This is location id</param>
		/// <returns>Collection of NPS terminal details</returns>
		List<NpsTerminal> GetByLocationID(long locationPK);

		/// <summary>
		/// This method is to get the collection of all NPS terminal details 
		/// </summary>
		/// <returns>Collection of NPS terminal details</returns>
		IQueryable<NpsTerminal> GetAll();

	}
}
