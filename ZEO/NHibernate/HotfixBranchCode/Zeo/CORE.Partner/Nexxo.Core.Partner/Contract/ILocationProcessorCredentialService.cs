using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Core.Partner.Data;

namespace MGI.Core.Partner.Contract
{
	public interface ILocationProcessorCredentialService
	{
		/// <summary>
		/// This method is to get the collection of location processor credentials
		/// </summary>
		/// <param name="locationId">This is location id</param>
		/// <returns>Collection of location processor credentials</returns>
		IList<LocationProcessorCredentials> Get(long locationId);

		/// <summary>
		/// This method is to save the location processor credentials
		/// </summary>
		/// <param name="processorDetails">This is location processor credentials</param>
		/// <returns>Saved status of location processor credentials</returns>
		bool Save(LocationProcessorCredentials processorDetails);
	}
}
