// -----------------------------------------------------------------------
// <copyright file="IProcessorRouter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Cxn.Common.Processor.Util
{
	using MGI.Cxn.Common.Processor.Contract;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public interface IProcessorRouter
	{
		/// <summary>
		/// This method is to get the processors based on channel partner.
		/// </summary>
		/// <param name="channelPartner">This method is to get the processors based on give parameters</param>
		/// <returns></returns>
		IProcessor GetProcessor(string channelPartner);

		/// <summary>
		/// This method is to get the processors based on give parameters
		/// </summary>
		/// <param name="channelPartner">This specifies name of channel partner</param>
		/// <param name="providerName">This is a provider name </param>
		/// <returns></returns>
		IProcessor GetProcessor(string channelPartner, string providerName);

		//Changes for MGI 
		/// <summary>
		/// This method is to get the providers.
		/// </summary>
		/// <param name="channelPartner">This specifies name of channel partner</param>
		/// <returns></returns>
		string GetProvider(string channelPartner);
	}
}
