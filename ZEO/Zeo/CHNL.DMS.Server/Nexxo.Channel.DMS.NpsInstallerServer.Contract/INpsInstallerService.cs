using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MGI.Common.Sys;
using MGI.Channel.DMS.NpsInstallerServer.Data;

namespace MGI.Channel.DMS.NpsInstallerServer.Contract
{
	[ServiceContract]
	public interface INpsInstallerService
	{
        /// <summary>
        /// This method to add the terminal details.
        /// </summary>
        /// <param name="npsTerminal">A transient instance of NpsTerminal[Class]</param>
        /// <returns>bool</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        bool CreateTerminal(NpsTerminal npsTerminal);

        /// <summary>
        /// This method to Update the terminal details.
        /// </summary>
        /// <param name="npsTerminal">A transient instance of NpsTerminal[Class] containing updated state</param>
        /// <returns>bool</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        bool UpdateTerminal(NpsTerminal npsTerminal);

        /// <summary>
        /// To Fetch all location based on agent Name and Password and channel partner name.
        /// </summary>
        /// <param name="agentName">the agent name</param>
        /// <param name="agentPassword">the agent password</param>
        /// <param name="channelPartnerName">the channel partner name</param>
        /// <returns>List Of Locations</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        List<string> Locations(string agentName, string agentPassword, string channelPartnerName);

        /// <summary>
        /// To fetch Nps Terminal based on System IP Address.
        /// </summary>
        /// <param name="ipAddress">the system ip address</param>
        /// <returns>NpsTerminal Details</returns>
		[OperationContract(Name = "LookupNpsInstallerTerminalByIpAddress")]
		[FaultContract(typeof(NexxoSOAPFault))]
        NpsTerminal LookupTerminal(string ipAddress);

        /// <summary>
        /// To fetch all locations.
        /// </summary>
        /// <returns>List Of Locations</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<Location> GetAllLocations();
	}
}
