using System;

namespace MGI.Common.Sys
{
	public class ProviderException : Exception
	{
		public int ProviderId { get; internal set; }
		public string Code { get; internal set; }

		/// <summary>
		/// Holds provider error info
		/// </summary>
		/// <param name="ProviderId">Provider Id</param>
		/// <param name="ProviderErrorCode">code returned from provider</param>
		public ProviderException(int ProviderId, string ProviderErrorCode)
			: this(ProviderId, ProviderErrorCode, string.Empty)
		{
		}

		/// <summary>
		/// Holds provider error info
		/// </summary>
		/// <param name="ProviderId">Provider Id</param>
		/// <param name="ProviderErrorCode">code returned from provider</param>
		/// <param name="Message">associated provider message</param>
		public ProviderException(int ProviderId, string ProviderErrorCode, string Message)
			: base( Message )
		{
			this.ProviderId = ProviderId;
			this.Code = ProviderErrorCode;
		}
	}
}
