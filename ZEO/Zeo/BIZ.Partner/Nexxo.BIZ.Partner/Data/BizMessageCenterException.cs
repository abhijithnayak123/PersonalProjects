// -----------------------------------------------------------------------
// <copyright file="MessageException.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Biz.Partner.Data
{
	using MGI.Common.Sys;
	using MGI.Common.Util;
	using System;

	/// TODO: Update summary.
	/// </summary>
	public class BizMessageCenterException : AlloyException
	{
		const string ProductCode = "1000";
		static string AlloyCode = ((int)ProviderId.Alloy).ToString();

		public BizMessageCenterException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, AlloyCode, alloyErrorCode, innerException)
		{
		}

		static public string AGENT_MESSAGE_UPDATE_FAILED = "4400";
		static public string AGENT_MESSAGE_GET_FAILED	 = "4401";
	}
	
}
