using System;
using System.ServiceModel.Description;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Modules.Services
{
    /// <summary>
    /// Represents a service manager.
    /// </summary>
    public class ServiceManager : ManagerBase<ServiceManager>
    {
        /// <summary>
        /// The name of the LocalService component.
        /// </summary>
        public const string LocalServiceKey = @"LocalService";

        /// <summary>
        /// The name of the LocalService component.
        /// </summary>
        public const string NexusServiceKey = @"NexusService";

        /// <summary>
        /// The name of the EndpointProvider component.
        /// </summary>
        public const string EndpointProviderKey = @"EndpointProvider";

        /// <summary>
        /// Gets the service reference registered as local.
        /// </summary>
        public IGameService Local { get; private set; }

        /// <summary>
        /// Gets the nexus service reference.
        /// </summary>
        public INexusService Nexus { get; private set; }

        /// <summary>
        /// Gets the endpoint provider.
        /// </summary>
        public IEndpointProvider EndpointProvider { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceManager"/>.
        /// </summary>
        public ServiceManager()
        {
            base.RequireComponent<IGameService>(LocalServiceKey);
            base.RequireComponent<INexusService>(LocalServiceKey);
            base.AllowComponent<IEndpointProvider>(EndpointProviderKey);
        }

        /// <summary><inheritdoc /></summary>
        protected override void OnInitializing()
        {
            if (!base.CheckComponent(EndpointProviderKey))
            {
                this.RegisterComponent(EndpointProviderKey, DefaultEndpointProvider.Instance);
            }

            base.OnInitializing();
        }

        /// <summary><inheritdoc /></summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.Local = base.GetComponent<IGameService>(LocalServiceKey);
            this.Nexus = base.GetComponent<INexusService>(NexusServiceKey);

            this.EndpointProvider = base.GetComponent<IEndpointProvider>(EndpointProviderKey);
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
            return this.Local as TGameService;
        }

        /// <summary>
        /// Gets 
        /// </summary>
        /// <typeparam name="TGameService"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public ServiceEndpoint GetEndpoint<TGameService>(Uri uri)
            where TGameService : class, IGameService
        {
            var endpoint = this.EndpointProvider.GetEndpoint<TGameService>(uri);
            return endpoint;
        }
    }
}
