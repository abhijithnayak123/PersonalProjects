using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Data;
using System.ServiceModel;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Contract
{
    public interface IManageLocations
    {
        /// <summary>
        /// This method is to fetch the location by location name
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent session</param>
        /// <param name="locationName">Lacation Name.</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Location Details.</returns>
		Location GetByName(long agentSessionId, string locationName, MGIContext mgiContext);

        /// <summary>
        /// This method is to fetch the collection of all locations
        /// </summary>
        /// <returns>List Of Location.</returns>
        List<Location> GetAll();

        /// <summary>
        /// To Fetch the collection of all location based on channel partner.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent session</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List of Locations</returns>
		[OperationContract(Name = "GetAllLocationByChannelPartnerId")]
		List<Location> GetAll(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// To create the location PNTR Database.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent session</param>
        /// <param name="manageLocation">A transient instance of Location [Class] </param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Unique Identifier Location.</returns>
		long Create(long agentSessionId, Location manageLocation, MGIContext mgiContext);

        /// <summary>
        /// To Update Location.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent session</param>
        /// <param name="manageLocation">A transient instance of Location[Class] containing Updated State</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Updated Status of Location</returns>
		bool Update(long agentSessionId, Location manageLocation, MGIContext mgiContext);

        /// <summary>
        /// To LookUp the location in PNTR database. Based on Location Id.
        /// </summary>
        /// <param name="locationId">This is the unique identifier for Location</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Location Details</returns>
		Location Lookup(long locationId, MGIContext mgiContext);
    }
}
