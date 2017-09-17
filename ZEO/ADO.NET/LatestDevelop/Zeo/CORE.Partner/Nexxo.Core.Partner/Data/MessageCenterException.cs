// -----------------------------------------------------------------------
// <copyright file="MessageException.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Core.Partner.Data
{
	using MGI.Common.Sys;
	using MGI.Common.Util;
	using System;

    /// TODO: Update summary.
    /// </summary>
	public class MessageCenterException : AlloyException
    {
		const string ProductCode = "1000";
		static string AlloyCode = ((int)ProviderId.Alloy).ToString();

		public MessageCenterException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, AlloyCode, alloyErrorCode, innerException)
		{
		}

		static public string AGENT_MESSAGE_CREATE_FAILED = "3400";
		static public string AGENT_MESSAGE_UPDATE_FAILED = "3401";
		public static string AGENT_MESSAGE_DELETE_FAILED = "3402";
		public static string AGENT_MESSAGE_LOOKUP_FAILED = "3403";
		public static string AGENT_MESSAGE_ADD_FAILED = "3404";
	
	}
	
}
