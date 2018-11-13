using MGI.Common.Util;
using MGI.Cxn.Check.Certegy.Certegy;
using MGI.Cxn.Check.Certegy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.Check.Certegy.Contract
{
	public interface IIO
	{
		/// <summary>
		/// This method to check the service connectivity.
		/// </summary>
		/// <param name="credential"></param>
		/// <param name="mgiContext"></param>
		/// <returns>response</returns>
		echoResponse Diagnostics(Credential credential, MGIContext mgiContext);

		/// <summary>
		/// This method to autorize check
		/// </summary>
		/// <param name="transaction"></param>
		/// <param name="credential"></param>
		/// <param name="mgiContext"></param>
		/// <returns>response</returns>
		authorizeResponse AuthorizeCheck(Transaction transaction, Credential credential, MGIContext mgiContext);

		/// <summary>
		/// This method to reverse the check/Cancel the check transaction.
		/// </summary>
		/// <param name="transaction"></param>
		/// <param name="credential"></param>
		/// <param name="mgiContext"></param>
		/// <returns>response</returns>
		reverseResponse ReverseCheck(Transaction transaction, Credential credential, MGIContext mgiContext);
	}
}
