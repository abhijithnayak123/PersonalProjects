using System;

using MGI.Core.Partner.Data;

namespace MGI.Core.Partner.Contract
{
	public interface ICustomerSessionService
	{

		/// <summary>
		/// This method is to create the customer session
		/// </summary>
		/// <param name="agentSession">This is agent session details</param>
		/// <param name="customer">This is customer details</param>
		/// <param name="cardPresent"></param>
		/// <param name="TimezoneID">This is time zone</param>
		/// <returns>Customer Session details</returns>
		CustomerSession Create(AgentSession agentSession, Customer customer, bool cardPresent, string TimezoneID);

		/// <summary>
		/// This method is to get the customer details
		/// </summary>
		/// <param name="id">This is unique identifier of customer session</param>
		/// <returns>Customer session details</returns>
		CustomerSession Lookup(long id);

		/// <summary>
		/// This method is to update the customer session
		/// </summary>
		/// <param name="customerSession">Customer session details</param>
		/// <returns></returns>
		void Update(CustomerSession customerSession);

		/// <summary>
		/// This method is to save any changes to customer session or dependent objects (like ShoppingCart)
		/// </summary>
		/// <param name="customerSession">Customer session details</param>
		/// <returns></returns>
		void Save(CustomerSession customerSession);

		/// <summary>
		/// This method is to ends the customer session
		/// </summary>
		/// <param name="customerSession">Customer session details</param>
		void End(CustomerSession customerSession);

		/// <summary>
		/// This method is to get the previous session parked shopping cart as active shopping cart
		/// </summary>
		/// <param name="customerSession">This is customer session details</param>
		/// <returns>Shopping cart details</returns>
		ShoppingCart GetParkingShoppingCart(CustomerSession customerSession);

	}
}
