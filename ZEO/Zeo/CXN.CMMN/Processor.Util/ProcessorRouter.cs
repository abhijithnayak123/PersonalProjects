// -----------------------------------------------------------------------
// <copyright file="ProcessorRouter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MGI.Cxn.Common.Processor.Util
{
    using MGI.Cxn.Common.Processor.Contract;
    using System;
    using System.Collections.Generic;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class ProcessorRouter : IProcessorRouter
	{
		public Dictionary<string, IProcessor> Processors { get; set; }
		public Dictionary<string, Dictionary<string, IProcessor>> ParentProcessors { get; set; }
		//Changes for MGI 
		public Dictionary<string, string> Providers { get; set; }
		public Dictionary<string, IProcessor> ProcessorSetupEngines { get; set; }

		public IProcessor GetProcessor(string channelPartner)
		{
			if (null == Processors) return null;
            Dictionary<string, IProcessor> ProcessorsDictWithIgnoreCase = new Dictionary<string, IProcessor>(Processors, StringComparer.InvariantCultureIgnoreCase);
            if ((ProcessorsDictWithIgnoreCase.ContainsKey(channelPartner)) && ProcessorsDictWithIgnoreCase[channelPartner] != null)
                return ProcessorsDictWithIgnoreCase[channelPartner];
			else
				return null;
		}
		public IProcessor GetProcessor(string channelPartner, string ProviderName)
		{
			if (null == ParentProcessors) return null;
            Dictionary<string, Dictionary<string, IProcessor>> ParentProcessorsDictWithIgnoreCase = new Dictionary<string, Dictionary<string, IProcessor>>(ParentProcessors, StringComparer.InvariantCultureIgnoreCase);
            if ((ParentProcessorsDictWithIgnoreCase.ContainsKey(channelPartner)) && ParentProcessorsDictWithIgnoreCase[channelPartner] != null)
            {
                Processors = ParentProcessorsDictWithIgnoreCase[channelPartner];
				if (null == Processors) return null;
				if (Processors.ContainsKey(ProviderName) && Processors[ProviderName] != null)
				{
					return Processors[ProviderName];
				}
				return null;
			}
			else
				return null;
		}
		//Changes for MGI
		public string GetProvider(string channelPartner)
		{
			if (null == Providers) return null;
            Dictionary<string, string> ProvidersDictWithIgnoreCase = new Dictionary<string, string>(Providers, StringComparer.InvariantCultureIgnoreCase);
            if (ProvidersDictWithIgnoreCase.ContainsKey(channelPartner) && ProvidersDictWithIgnoreCase[channelPartner] != null)
                return ProvidersDictWithIgnoreCase[channelPartner];
			else
				return null;
		}
	}
}
