using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.CXE.Data;

namespace MGI.Core.CXE.Contract
{
	public interface IBillPaySetupService
	{
		/// <summary>
		/// This method is to create the customer prefered product
		/// </summary>
		/// <param name="customerPrefered">This is customer prefered product details</param>
		/// <returns>Unique identifier of customer prefered product</returns>
		long Create(CustomerPreferedProduct customerPrefered);

		/// <summary>
		/// This method is to update the customer prefered product
		/// </summary>
		/// <param name="customerPrefered">This is customer prefered product details to be updated</param>
		/// <param name="timezone">This is time zone</param>
		/// <returns>Updated status for customer prefered product</returns>
		bool Update(CustomerPreferedProduct customerPrefered, string timezone);

		/// <summary>
		/// This method is to get the customer perfered product details by product id and alloy id.
		/// </summary>
		/// <param name="ProductId">This is product id to get customer prefered product details</param>
		/// <param name="alloyId">This is alloy id to get customer prefered product details</param>
		/// <returns>customer prefered product details</returns>
		CustomerPreferedProduct Get(long ProductId, long alloyId);

		/// <summary>
		/// This method is to get the customer perfered product details by product id and customer id.
		/// </summary>
		/// <param name="ProductId">This is product id to get customer prefered product details</param>
		/// <param name="customerID">This is customer id to get customer prefered product details</param>
		/// <returns>customer prefered product details</returns>
		CustomerPreferedProduct GetBillerReceiverIndex(long productId, long customerID);

		/// <summary>
		/// This method is to get the collection of product ids from the customer perfered product.
		/// </summary>		
		/// <param name="alloyId">This is alloy id to get the collection of product ids</param>
		/// <returns>Collection of product ids</returns>
		long[] GetPrefered(long alloyId);
	}
}
