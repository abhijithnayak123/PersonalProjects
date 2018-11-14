using System.Collections.Generic;

using MGI.Core.CXE.Data;

namespace MGI.Core.CXE.Contract
{
	public interface ICustomerService
	{

		/// <summary>
		/// This method is to register new customer
		/// </summary>
		/// <param name="customer">This is customer details</param>
		/// <returns>Customer details</returns>
		Customer Register(Customer customer);

		/// <summary>
		/// This method is to get the customer details by alloy id
		/// </summary>
		/// <param name="alloyId">This is customer alloy id</param>
		/// <returns>Customer details</returns>
		Customer Lookup(long alloyId);

		/// <summary>
		/// This method is to get the customer details by customer search criteria
		/// </summary>
		/// <param name="criteria">This is customer search criteria</param>
		/// <returns>Collection of customer details</returns>
		List<Customer> Lookup(CustomerSearchCriteria criteria);

		/// <summary>
		/// This method is to update customer profile, government id and employment details
		/// </summary>
		/// <param name="customer">This is customer details</param>
		/// <returns></returns>
		void Save(Customer customer);

		/// <summary>
		/// This method is to get the customer id by phone number and PIN
		/// </summary>
		/// <param name="Phone">This is phone number</param>
		/// <param name="PIN">This is PIN</param>
		/// <returns>Unique identifier of  customer details</returns>
		long Get(string Phone, string PIN);

		/// <summary>
		/// This method is to get the customer details by channel partner id, first name and last name
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <param name="firstName">This is first name</param>
		/// <param name="lastName">This is last name</param>
		/// <returns>Customer details</returns>
		Customer Lookup(long channelPartnerId, string firstName, string lastName);

		/// <summary>
		/// This method is to check whether the customer is active  or not
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <param name="firstName">This is first name</param>
		/// <param name="lastName">This is last name</param>
		/// <returns>Customer details</returns>
		void ValidateStatus(long alloyId);

		/// <summary>
		/// This method is to  get the customer details by alloy id
		/// </summary>
		/// <param name="alloyId">This is alloy id</param>
		/// <returns>Customer details.</returns>
		Customer Lookup(string alloyId);

	}
}
