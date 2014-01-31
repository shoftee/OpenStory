using Ninject.Modules;
using OpenStory.Framework.Contracts;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.World
{
    /// <summary>
    /// World Server module.
    /// </summary>
    public sealed class WorldServerModule : NinjectModule
    {
        /// <inheritdoc/>
        public override void Load()
        {
            Bind<IWorldInfoProvider>().To<WorldInfoProvider>().InSingletonScope();
            Bind<IAuthToWorldRequestHandler, IChannelToWorldRequestHandler>().To<WorldServer>().InSingletonScope();
        }
    }
}
