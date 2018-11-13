using MGI.Common.Util;
using MGI.Cxn.BillPay.MG.AgentConnectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.BillPay.MG.Contract
{
	public interface IIO
	{
		/// <summary>
		/// This method contains billpay validation request information
		/// </summary>
		/// <param name="validationRequest">contains billpay validation request</param>
		/// <param name="MgiContext">This is the common dictionary parameter used to pass supplemental information</param>
		/// <returns>bill pay validation response</returns>
		BpValidationResponse BpValidation(BpValidationRequest validationRequest, MGIContext MgiContext);

		/// <summary>
		/// This method used for bill pay biller search
		/// </summary>
		/// <param name="billerSearchRequest">contains biller search request</param>
		/// <returns>biller details</returns>
		BillerSearchResponse BillerSearch(BillerSearchRequest billerSearchRequest);
	}
}