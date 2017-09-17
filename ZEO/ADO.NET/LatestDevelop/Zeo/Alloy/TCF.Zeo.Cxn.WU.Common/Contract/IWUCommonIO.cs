using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.WU.Common.Data;
using System.Collections.Generic;

namespace TCF.Zeo.Cxn.WU.Common.Contract
{
	public interface IWUCommonIO
	{
        /// <summary>
        /// This method is used for WU Card Enrollement.
        /// </summary>
        /// <param name="enrollmentReq"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        CardDetails WUCardEnrollment(WUEnrollmentRequest enrollmentReq, ZeoContext context);

        /// <summary>
        /// This method is used to get WU request of channel, WUCertificate, WUServiceUrl, ForeignRemoteSystem
        /// from location and account identifier 
        /// </summary>
        /// <param name="channelPartnerId">Channel Parnter ID</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>WU Card Request Details</returns>
        WUBaseRequestResponse CreateRequest(long channelPartnerId, ZeoContext context);

        /// <summary>
        /// This method is used to get WU AgentBanner message.
        /// </summary>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Context of agent banner messages</returns>
        List<AgentBanners> GetWUAgentBannerMsgs(ZeoContext context);

        /// <summary>
        /// This method is used for WU card customer lookup
        /// </summary>
        /// <param name="wucardlookupreq">Request of WU card lookup details.</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>WU Card Lookup</returns>
        CardLookupDetails WUCardLookup(CardLookUpRequest wucardlookupreq, ZeoContext context);


        /// <summary>
        /// This method is used to WU Card Name Mathcing
        /// </summary>
        /// <param name="wuCardLookupReq">Request of WU card lookup details</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>WU Card Lookup Details</returns>
        CardLookupDetails WUCardLookupForCardNumber(CardLookUpRequest wuCardLookupReq, ZeoContext context);

        /// <summary>
        /// This method is used to get cardlookupdetails collection will contain combination of Past Biller and Receivers Collection.
        /// </summary>
        /// <param name="wucardlookupreq">Request of WU card lookup details</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Past Bill and Receivers</returns>
        List<Receiver> WUPastBillersReceivers(CardLookUpRequest wucardlookupreq, ZeoContext context);

		/// <summary>
		/// This method is used to get the Government ID type mapping
		/// </summary>
		/// <param name="idType">ID Type</param>
		/// <returns> Dictionary of Government ID Mapping</returns>
		string GetGovtIDType(string idType);

        /// <summary>
        /// This method is used to retrive the WU card related information
        /// </summary>
        /// <param name="request">Request of WU card lookup details</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>WU card information</returns>
        CardInfo GetCardInfo(CardLookUpRequest request, ZeoContext context);
        
		/// <summary>
		/// This method is used to get the WU South west border location
		/// </summary>
		/// <param name="stateLocation">State Location</param>
		/// <returns>SWB state</returns>
		bool IsSWBState(string stateLocation);

        /// <summary>
        /// This method is used to get SW Flag information
        /// </summary>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>SW Flag information</returns>
        SwbFlaInfo BuildSwbFlaInfo(ZeoContext context);

        /// <summary>
        /// This method is used to get the Build general name of Agent first and last name
        /// </summary>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Build general name</returns>
        GeneralName BuildGeneralName(ZeoContext context);
		
		/// <summary>
		///  Trim Occupation at 29 th Character
		/// </summary>
		/// <param name="occupation"></param>
		/// <returns>Trim of occupation length</returns>
		string TrimOccupation(string occupation);
	

    }
}
