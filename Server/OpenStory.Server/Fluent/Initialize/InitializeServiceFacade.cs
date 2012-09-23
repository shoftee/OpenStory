using System;
using OpenStory.Server.Modules.Services;
using OpenStory.Services;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Initialize
{
    internal sealed class InitializeServiceFacade : NestedFacade<IInitializeFacade>, IInitializeServiceFacade
    {
        private readonly ServiceManager manager;

        private Guid accessToken;

        public InitializeServiceFacade(IInitializeFacade parent)
            : base(parent)
        {
            this.manager = new ServiceManager();
        }

        #region Implementation of IInitializeServiceFacade

        public IInitializeServiceFacade Host<TGameService>(TGameService local)
            where TGameService : class, IGameService
        {
            this.manager.RegisterComponent(ServiceManager.LocalServiceKey, local);
            return this;
        }

        public IInitializeServiceFacade Through(INexusService nexus)
        {
            this.manager.RegisterComponent(ServiceManager.NexusServiceKey, nexus);
            return this;
        }

        public INestedFacade<IInitializeFacade> WithAccessToken(Guid token)
        {
            this.accessToken = token;
            return this;
        }

        #endregion

        public override IInitializeFacade Done()
        {
            this.manager.Initialize();
            ServiceConfiguration configuration;
            this.manager.Nexus.TryGetServiceConfiguration(this.accessToken, out configuration);

            var uriString = configuration["ServiceUri"];

            // TODO: Return the ServiceHost reference somehow.
            ServiceHelpers.OpenServiceHost(this.manager.Local, new Uri(uriString));

            ServiceManager.RegisterDefault(this.manager);

            return base.Done();
        }
    }
}