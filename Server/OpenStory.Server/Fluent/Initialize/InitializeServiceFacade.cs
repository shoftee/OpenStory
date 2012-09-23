using System;
using OpenStory.Services;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Initialize
{
    internal sealed class InitializeServiceFacade : NestedFacade<IInitializeFacade>, IInitializeServiceFacade
    {
        private readonly ServiceManager manager;

        private Guid accessToken;
        private INexusServiceFragment nexusFragment;

        public InitializeServiceFacade(IInitializeFacade parent)
            : base(parent)
        {
            manager = new ServiceManager();
        }

        #region Implementation of IInitializeServiceFacade

        public IInitializeServiceFacade Host(IManagedService local, INexusServiceFragment fragment)
        {
            manager.RegisterComponent(ServiceManager.LocalServiceKey, local);
            this.nexusFragment = fragment;
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
            Uri uri;
            this.nexusFragment.TryGetServiceUri(this.accessToken, out uri);

            // TODO: Return the ServiceHost reference somehow.
            ServiceHelpers.OpenServiceHost(this.manager.LocalService, uri);

            ServiceManager.RegisterDefault(this.manager);

            return base.Done();
        }
    }
}