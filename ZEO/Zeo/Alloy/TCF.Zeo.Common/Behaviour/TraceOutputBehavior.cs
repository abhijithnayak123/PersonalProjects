using System;
using System.ServiceModel.Description;

namespace TCF.Zeo.Common.Behaviour
{
    public class TraceOutputBehavior : IEndpointBehavior
    {
        public TraceOutputMessageInspector mi { get; set; }
        public TraceOutputBehavior()
        {
            mi = new TraceOutputMessageInspector();
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
