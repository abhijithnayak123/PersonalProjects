using Spring.Context;
using System;
using System.ServiceModel.Description;

namespace MGI.Common.Util
{
	public class TraceOutputBehavior : IEndpointBehavior
	{
        public TraceOutputMessageInspector mi { get; set; }
        public TraceOutputBehavior()
        {
            IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            mi = (TraceOutputMessageInspector)ctx.GetObject("mi");
        }
		public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
		{
			clientRuntime.MessageInspectors.Add(mi);
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
		{
			throw new NotImplementedException();
		}

		public void Validate(ServiceEndpoint endpoint)
		{
		}
	}
}
