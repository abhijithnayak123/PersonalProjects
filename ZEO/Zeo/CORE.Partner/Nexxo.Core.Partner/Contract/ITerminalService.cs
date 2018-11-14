using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data;
using MGI.Common.Util;

namespace MGI.Core.Partner.Contract
{
	public interface ITerminalService
	{
		/// <summary>
		/// This method is to get the terminal details by primary key.
		/// </summary>
		/// <param name="PK">This is primary key of terminal</param>
		/// <returns>Terminal details</returns>
		Terminal Lookup(Guid PK);

		/// <summary>
		/// This method is to get the terminal details by id.
		/// </summary>
		/// <param name="Id">This is unique identifier of terminal</param>
		/// <returns>Terminal details</returns>
		Terminal Lookup(long Id);

		/// <summary>
		/// This method is to get the terminal details by terminal name and channel partner details.
		/// </summary>
		/// <param name="terminalName">This is terminal name</param>
		/// <param name="channelPartner">This is channel partner details</param>
		/// <param name="mgiContext"> This is the common class parameter used to pass supplimental information</param>
		/// <returns>Terminal details</returns>
		Terminal Lookup(string terminalName, ChannelPartner channelPartner, MGIContext mgiContext);

		/// <summary>
		/// This method is to create the terminal
		/// </summary>		
		/// <param name="terminal">This is terminal details</param>
		/// <returns>Guid of terminal</returns>
		Guid Create(Terminal terminal);

		/// <summary>
		/// This method is to update the terminal
		/// </summary>		
		/// <param name="terminal">This is terminal details</param>
		/// <returns>Updated status of terminal</returns>
		bool Update(Terminal terminal);
	}
}
