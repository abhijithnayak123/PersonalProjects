using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Core.Partner.Data;

namespace MGI.Core.Partner.Contract
{
	public interface ILocationCounterIdService
	{

		/// <summary>
		/// This method is to get the location counter Id
		/// </summary>
		/// <param name="locationID">This is location id</param>
		/// <param name="providerId">This is provider id</param>		
		/// <returns>Location counter id</returns>
		string Get(Guid locationID, int providerId);

		/// <summary>
		/// This method is to get the location counter Id details by location id, counter id and provider id
		/// </summary>
		/// <param name="locationID">This is location id</param>
		/// <param name="providerId">This is provider id</param>	
		/// <param name="counterId">This is counter id</param>
		/// <returns>Location counter id details</returns>
		LocationCounterId Get(Guid locationID, string counterId, int providerId);

		/// <summary>
		/// This method is to get the location counter Id details by location id and counter id 
		/// </summary>
		/// <param name="locationID">This is location id</param>	
		/// <param name="counterId">This is counter id</param>
		/// <returns>Location counter id details</returns>
		LocationCounterId Get(Guid locationID, string counterId);

		/// <summary>
		/// This method is to update the location counter id details
		/// </summary>
		/// <param name="locationCounterId">This is location counter id</param>	
		/// <returns>Updated status of location counter id details</returns>
		bool Update(LocationCounterId locationCounterId);

	}
}
