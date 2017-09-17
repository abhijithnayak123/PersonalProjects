using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Data
{
	public class BizChannelPartnerException : AlloyException
	{
		public static string ProductCode = "1000";
		public static string ProviderCode = ((int)ProviderId.Alloy).ToString();

		public BizChannelPartnerException(string alloyErrorCode, Exception innerException)
			: base(ProductCode, ProviderCode, alloyErrorCode, innerException)
		{
		}
		
		public static readonly string CHANNEL_PARTNER_GET_FAILED = "4100";
		public static readonly string CHANNEL_PARTNER_LOCATIONS_GET_FAILED = "4101";
		public static readonly string CHANNEL_PARTNER_GROUP_GET_FAILED = "4102";
		public static readonly string TIPS_AND_OFFERS_GET_FAILED = "4103";
		public static readonly string CHANNEL_PARTNER_CERTIFICATE_INFO_GET_FAILED = "4104";

	}
}
