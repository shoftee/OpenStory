using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;

namespace OpenStory.Services.Wcf
{
    class NinjectServiceBehavior : IServiceBehavior
    {
        private readonly IResolutionRoot resolutionRoot;

        public NinjectServiceBehavior(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(
            ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            var serviceType = serviceDescription.ServiceType;

            var dispatchers = serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>();
            foreach (var channelDispatcher in dispatchers)
            {
                foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                {
                    var dispatchRuntime = endpointDispatcher.DispatchRuntime;
                    dispatchRuntime.InstanceProvider = this.GetInstanceProvider(serviceType);
                }
            }
        }

        private IInstanceProvider GetInstanceProvider(Type serviceType)
        {
            // Get NinjectInstanceProvider by injection because we want to give it the kernel.
            var serviceTypeArgument = new ConstructorArgument("serviceType", serviceType);
            var provider = this.resolutionRoot.Get<NinjectInstanceProvider>(serviceTypeArgument);
            return provider;
        }
    }
}
