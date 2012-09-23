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
        /// The name of the LocalService component.
        /// </summary>
        public const string LocalServiceKey = "LocalService";

        /// <summary>
        /// Gets the service reference registered as local.
        /// </summary>
        public IManagedService LocalService { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceManager"/>.
        /// </summary>
        public ServiceManager()
        {
            base.RequireComponent<IManagedService>(LocalServiceKey);
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (base.CheckComponent(LocalServiceKey))
            {
                this.LocalService = base.GetComponent<IManagedService>(LocalServiceKey);
            }
        }

        /// <summary>
        /// Returns the local service reference.
        /// </summary>
        /// <typeparam name="TManagedService">The actual type of the local service.</typeparam>
        /// <returns>
        /// the local service, cast to <typeparamref name="TManagedService"/>; 
        /// <c>null</c> if the service is of a different type or there is no local service registered.
        /// </returns>
        public TManagedService LocalAs<TManagedService>()
            where TManagedService : class, IManagedService
        {
            return this.LocalService as TManagedService;
        }
    }
}
