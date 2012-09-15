using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Server.Modules;
using OpenStory.Services.Contracts;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents a service manager.
    /// </summary>
    public class ServiceManager : ManagerBase<ServiceManager>
    {
        /// <summary>
        /// The key for the LocalService component.
        /// </summary>
        public const string LocalServiceKey = "LocalService";

        /// <summary>
        /// The key for the NexusService component.
        /// </summary>
        public const string NexusServiceKey = "NexusService";

        /// <summary>
        /// Gets the service reference registered as local.
        /// </summary>
        public IGameService LocalService { get; private set; }

        /// <summary>
        /// Get or sets the reference for the nexus service.
        /// </summary>
        public INexusService NexusService { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceManager"/>.
        /// </summary>
        public ServiceManager()
        {
            base.AllowComponent<INexusService>(NexusServiceKey);
            base.AllowComponent<IGameService>(LocalServiceKey);
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (base.CheckComponent(NexusServiceKey))
            {
                this.NexusService = base.GetComponent<INexusService>(NexusServiceKey);
            }

            if (base.CheckComponent(LocalServiceKey))
            {
                this.LocalService = base.GetComponent<IGameService>(LocalServiceKey);
            }
        }

        /// <summary>
        /// Returns the local service reference.
        /// </summary>
        /// <typeparam name="TGameService">The actual type of the local service.</typeparam>
        /// <returns>
        /// the local service, cast to <typeparamref name="TGameService"/>; 
        /// <c>null</c> if the service is of a different type or there is no local service registered.
        /// </returns>
        public TGameService LocalAs<TGameService>()
            where TGameService : class, IGameService
        {
            return this.LocalService as TGameService;
        }
    }
}
