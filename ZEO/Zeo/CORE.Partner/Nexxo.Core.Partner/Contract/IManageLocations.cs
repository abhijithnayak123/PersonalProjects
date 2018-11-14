using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Core.Partner.Data;

namespace MGI.Core.Partner.Contract
{
	public interface IManageLocations
	{

		/// <summary>
		/// This method is to get the location by location name
		/// </summary>
		/// <param name="locationName">This is location name</param>
		/// <returns>This is Location details</returns>
		Location GetByName(string locationName);

		/// <summary>
		/// This method is to get the collection of all location 
		/// </summary>
		/// <returns>Collection of location details</returns>
		List<Location> GetAll();

		/// <summary>
		/// This method is to create the location
		/// </summary>
		/// <param name="location">This is location details</param>
		/// <returns>Created status of locations</returns>
		long Create(Location location);

		/// <summary>
		/// This method is to update the location details
		/// </summary>
		/// <param name="location">This is location details</param>
		/// <returns>Updated status of locations</returns>
		bool Update(Location location);

		/// <summary>
		/// This method is to get the location details by location id
		/// </summary>
		/// <param name="locationId">This is unique identifier of location details</param>
		/// <returns>This is location details</returns>
		Location Lookup(long locationId);

		/// <summary>
		/// This method is to get the location details by location id
		/// </summary>
		/// <param name="locationId">This is row Guid of location details</param>
		/// <returns>This is location details</returns>
		Location Lookup(Guid locationId);
	}
}
