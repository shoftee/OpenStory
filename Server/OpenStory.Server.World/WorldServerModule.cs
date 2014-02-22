using Ninject.Modules;
using OpenStory.Framework.Contracts;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.World
{
    /// <summary>
    /// World server module.
    /// </summary>
    public sealed class WorldServerModule : NinjectModule
    {
        /// <inheritdoc/>
        public override void Load()
        {
            // No dependencies
            Bind<IServiceContainer<IWorldToChannelRequestHandler>>().To<ChannelContainer>().InSingletonScope();
            Bind<IWorldInfoProvider>().To<StubWorldInfoProvider>().InSingletonScope();

            // WorldServer
            // ^ IServiceContainer<INexusToWorldRequestHandler>, ChannelContainer, IWorldInfoProvider
            Bind<IRegisteredService, IChannelToWorldRequestHandler>().To<WorldServer>().InSingletonScope();
        }
    }
}
