using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Modules.Config
{
    /// <summary>
    /// The manager responsible for accessing and modifying configuration entries.
    /// </summary>
    public sealed class ConfigManager : ManagerBase<ConfigManager>
    {
        /// <summary>
        /// The name of the "Provider" component.
        /// </summary>
        public const string ProviderKey = "Provider";

        /// <summary>
        /// Gets the provider object for this manager.
        /// </summary>
        public IConfigDataProvider Provider { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ConfigManager"/>.
        /// </summary>
        public ConfigManager()
        {
            base.RequireComponent<IConfigDataProvider>(ProviderKey);
        }

        /// <summary><inheritdoc /></summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.Provider = base.GetComponent<IConfigDataProvider>(ProviderKey);
        }

        /// <summary>
        /// Gets a <see cref="DomainConfig"/> object for a specified domain name.
        /// </summary>
        /// <param name="domainName">The name of the configuration domain.</param>
        /// <returns>an instance of <see cref="DomainConfig"/>.</returns>
        public DomainConfig GetDomainConfig(string domainName)
        {
            var result = new DomainConfig(domainName, this.Provider);
            return result;
        }
    }
}
