using System;

using MGI.Core.Partner.Data;
using System.Collections.Generic;

namespace MGI.Core.Partner.Contract
{
	public interface IShoppingCartService
	{
		//ShoppingCart Create();

		/// <summary>
		/// This method is to update the status of shopping cart service
		/// </summary>		
		/// <param name="id">This is unique identifier of shopping cart service</param>
		/// <param name="status">This is status of shopping cart</param>
		/// <returns></returns>
		void Update(long id, ShoppingCartStatus status);

		/// <summary>
		/// This method is to update the shopping cart service
		/// </summary>		
		/// <param name="id">This is unique identifier of shopping cart service</param>
		/// <param name="isReferral">This is referral status of shopping cart</param>
		/// <returns></returns>
		void Update(long id, bool isReferral);

		/// <summary>
		/// This method is to get shopping cart details
		/// </summary>		
		/// <param name="id">This is unique identifier of shopping cart.</param>
		/// <returns>Shopping cart details</returns>
		ShoppingCart Lookup(long id);

		/// <summary>
		/// This method is to get collection of shopping cart details
		/// </summary>		
		/// <param name="alloyId">This is Alloy id</param>
		/// <returns>Collection of shopping cart details</returns>
		List<ShoppingCart> LookupForCustomer(long alloyId);

        IList<ShoppingCart> GetAllParkedShoppingCarts();
	}
}
