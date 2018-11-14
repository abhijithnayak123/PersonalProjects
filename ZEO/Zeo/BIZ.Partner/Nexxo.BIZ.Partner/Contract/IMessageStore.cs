using MGI.Biz.Partner.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Contract
{
	public interface IMessageStore
	{
		/// <summary>
		/// To retrieve error message from Database
		/// </summary>
		/// <param name="messageKey">Error Code</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Error Message</returns>
		Message GetMessage(string messageKey, MGIContext mgiContext);
	}
}
