using System;

namespace MGI.Common.Sys
{
	public class ProviderException : Exception
	{
		public int ProviderId { get; internal set; }
		public string Code { get; internal set; }

		public string ProductCode { get; set; }
		public string ProviderCode { get; set; }
		public string ProviderErrorCode { get; set; }

		public ProviderException()
		{
		}

		public ProviderException(string productCode, string providerCode, string providerErrorCode, string providerMessage, Exception innerException)
			: base(providerMessage, innerException)
		{
			this.ProductCode = productCode;
			this.ProviderCode = providerCode;
			this.ProviderErrorCode = providerErrorCode;
		}

		public ProviderException(string productCode, string providerCode, string providerErrorCode, string providerMessage)
			: base(providerMessage)
		{
			this.ProductCode = productCode;
			this.ProviderCode = providerCode;
			this.ProviderErrorCode = providerErrorCode;
		}

		/// <summary>
		/// Holds provider error info
		/// </summary>
		/// <param name="ProviderId">Provider Id</param>
		/// <param name="ProviderErrorCode">code returned from provider</param>
		/// <param name="Message">associated provider message</param>
		public ProviderException(int ProviderId, string ProviderErrorCode, string Message)
			: base(Message)
		{
			this.ProviderId = ProviderId;
			this.Code = ProviderErrorCode;
		}

		public static string PROVIDER_FAULT_ERROR = "5000";
		public static string PROVIDER_ENDPOINTNOTFOUND_ERROR = "5001";
		public static string PROVIDER_COMMUNICATION_ERROR = "5002";
		public static string PROVIDER_TIMEOUT_ERROR = "5003";
	}
}
