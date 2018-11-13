using System;

using MGI.Core.Partner.Data;
using MGI.Common.Util;

namespace MGI.Core.Partner.Contract
{
	public interface ICustomerService
	{
		/// <summary>
		/// This method is to create the customer
		/// </summary>
		/// <param name="CXEId">This unique identifier of customer details</param>
		/// <param name="IsPartnerAccountHolder">Whether the customer is partner account holder or not</param>
		/// <param name="ReferralCode">This is referral code for  customer</param>
		/// <param name="ChannelPartnerId">This is channel partner id</param>
		/// <param name="AgentGuid">This is Agent session id for customer</param>
		/// <param name="CustProfileStatus">This is customer profile status whether the customer is active or inactive</param>
		/// <returns>Customer details</returns>
		Customer Create(Customer customer);

		/// <summary>
		/// This method is to update the customer.
		/// </summary>
		/// <param name="customer">This is customer details</param>
		/// <returns></returns>
		void Update(Customer customer);

		/// <summary>
		/// This method is to get the customer details by account id and provider id
		/// </summary>
		/// <param name="cxnAccountId">This is account id to get the customer details</param>\
		/// <param name="providerId">This is provider id to get the provider details</param>
		/// <returns>Customer details</returns>
		Customer LookupByCXNAccountId(long cxnAccountId, int providerId);

		/// <summary>
		/// This method is to get the customer details by id.
		/// </summary>
		/// <param name="Id">This is unique identifier of customer in PTNR</param>
		/// <returns>Customer details</returns>	
		Customer Lookup(long Id);

		/// This method is to get the customer details by CXE Id
		/// </summary>
		/// <param name="CxeId">This is unique identifier of customer in CXE</param>
		/// <returns>Customer details</returns>	
		Customer LookupByCxeId(long CxeId);

		/// <summary>
		/// This method is to get the prospect details by alloy id.
		/// </summary>
		/// <param name="alloyId">This is alloy id of prospects</param>
		/// <returns>Prospect details</returns>
		Prospect LookupProspect(long alloyId);

		/// <summary>
		/// This method is to save the prospect details
		/// </summary>
		/// <param name="prospect">This is prospect details.</param>
		/// <returns></returns>
		void SaveProspect(Prospect prospect);

		/// <summary>
		/// This method is to save the confirmation identification
		/// </summary>
		/// <param name="status">This is status of confirmation identification.</param>
		/// <param name="agentId">This is agent id</param>
		/// <param name="customerSessionId">This is customer session id</param>
		/// <returns>Confirm identity</returns>
		string ConfirmIdentity(long agentId, long customerSessionId, bool status);

		/// <summary>
		/// This method is to save the customer group setting
		/// </summary>
		/// <param name="CustomerGroupSetting">This is customer group setting details.</param>
		/// <returns></returns>
		void SaveGroupSetting(CustomerGroupSetting CustomerGroupSetting);
	}
}
