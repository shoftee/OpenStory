using OpenStory.Services.Contracts;
using Ninject.Modules;

namespace OpenStory.Server.Nexus
{
    /// <summary>
    /// Nexus server module.
    /// </summary>
    public class NexusServerModule : NinjectModule
    {
        /// <inheritdoc />
        public override void Load()
        {
            Bind<IServiceContainer<INexusToWorldRequestHandler>>().To<WorldContainer>().InSingletonScope();
            Bind<IAuthToNexusRequestHandler>().To<NexusServer>().InSingletonScope();
        }
    }
}
