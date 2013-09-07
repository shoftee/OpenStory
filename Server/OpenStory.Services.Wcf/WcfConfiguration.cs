using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace OpenStory.Services.Wcf
{
    /// <summary>
    /// Represents a configuration for a WCF service.
    /// </summary>
    public class WcfConfiguration
    {
        /// <summary>
        /// Gets the channel type of the service.
        /// </summary>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets the list of base addresses for the service.
        /// </summary>
        public IEnumerable<Uri> BaseAddresses { get; private set; }

        /// <summary>
        /// Gets the configuration callback for the service.
        /// </summary>
        public Action<ServiceHost> ConfigurationAction { get; private set; }

        private WcfConfiguration(Type serviceType, IEnumerable<Uri> baseAddresses, Action<ServiceHost> configurationAction)
        {
            this.ServiceType = serviceType;
            this.BaseAddresses = baseAddresses;
            this.ConfigurationAction = configurationAction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfConfiguration"/> class.
        /// </summary>
        public static WcfConfiguration Create<TService>(Uri baseAddress, Action<ServiceHost> configurationAction)
        {
            return new WcfConfiguration(typeof(TService), new[] { baseAddress }, configurationAction);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfConfiguration"/> class.
        /// </summary>
        public static WcfConfiguration Create<TService>(Action<ServiceHost> configurationAction, params Uri[] baseAddresses)
        {
            return new WcfConfiguration(typeof(TService), baseAddresses, configurationAction);
        }
    }
}