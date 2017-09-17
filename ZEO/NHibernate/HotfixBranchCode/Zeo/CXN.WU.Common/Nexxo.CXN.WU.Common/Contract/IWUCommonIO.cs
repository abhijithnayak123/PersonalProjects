using MGI.Common.Util;
using MGI.Cxn.WU.Common.Data;
using System;
using System.Collections.Generic;

namespace MGI.Cxn.WU.Common.Contract
{
	public interface IWUCommonIO
	{
		/// <summary>
		/// This method is used for WU Card Enrollement
		/// </summary>
		/// <param name="sender">This field is used to the get sender details</param>
		/// <param name="paymentDetails">This field is used to get the Payment details example Currency code</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>WU Card Details</returns>
		CardDetails WUCardEnrollment(Sender sender, PaymentDetails paymentDetails, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get WU request of channel, WUCertificate, WUServiceUrl, ForeignRemoteSystem
		/// from location and account identifier 
		/// </summary>
		/// <param name="channelPartnerId">Channel Parnter ID</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>WU Card Request Details</returns>
		WUBaseRequestResponse CreateRequest(long channelPartnerId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get WU AgentBanner message.
		/// </summary>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Context of agent banner messages</returns>
		List<AgentBanners> GetWUAgentBannerMsgs(MGIContext mgiContext);

		/// <summary>
		/// This method is used for WU card customer lookup
		/// </summary>
		/// <param name="wucardlookupreq">Request of WU card lookup details.</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>WU Card Lookup</returns>
		CardLookupDetails WUCardLookup(CardLookUpRequest wucardlookupreq, MGIContext mgiContext);


		/// <summary>
		/// This method is used to WU Card Name Mathcing
		/// </summary>
		/// <param name="wuCardLookupReq">Request of WU card lookup details</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>WU Card Lookup Details</returns>
		CardLookupDetails WUCardLookupForCardNumber(CardLookUpRequest wuCardLookupReq, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get cardlookupdetails collection will contain combination of Past Biller and Receivers Collection.
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="wucardlookupreq">Request of WU card lookup details</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Past Bill and Receivers</returns>
		List<Receiver> WUPastBillersReceivers(long customerSessionId, CardLookUpRequest wucardlookupreq, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the Government ID type mapping
		/// </summary>
		/// <param name="idType">ID Type</param>
		/// <returns> Dictionary of Government ID Mapping</returns>
		string GetGovtIDType(string idType);

		/// <summary>
		/// This method is used to retrive the WU card related information
		/// </summary>
		/// <param name="request">>Request of WU card lookup details</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>WU card information</returns>
		CardInfo GetCardInfo(CardLookUpRequest request, MGIContext mgiContext);
        
		/// <summary>
		/// This method is used to get the WU South west border location
		/// </summary>
		/// <param name="stateLocation">State Location</param>
		/// <returns>SWB state</returns>
		bool IsSWBState(string stateLocation);

		/// <summary>
		/// This method is used to get SW Flag information
		/// </summary>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>SW Flag information</returns>
		SwbFlaInfo BuildSwbFlaInfo(MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the Build general name of Agent first and last name
		/// </summary>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Build general name</returns>
		GeneralName BuildGeneralName(MGIContext mgiContext);
		
		/// <summary>
		///  Trim Occupation at 29 th Character
		/// </summary>
		/// <param name="occupation"></param>
		/// <returns>Trim of occupation length</returns>
		string TrimOccupation(string occupation);

		/// <summary>
		///  This method is used to get the Country name
		/// </summary>
		/// <param name="countryCode">Country code from tWUnion_Countries table in CXN database</param>
		/// <returns>Country Name</returns>
		string GetCountryName(string countryCode);

		/// <summary>
		/// This method takes only ssn exception message, if they are ssn exception the it send default provider error exception
		/// </summary>
		/// <param name="ExceptionMessage">Exception Message</param>
		/// <returns>Exception message</returns>
		int GetExceptionMessage(string ExceptionMessage);
	}
}
