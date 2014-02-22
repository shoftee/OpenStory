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
            // No dependencies
            Bind<IServiceContainer<INexusToWorldRequestHandler>>().To<WorldContainer>().InSingletonScope();

            // NexusServer
            // ^ WorldContainer
            Bind<IAuthToNexusRequestHandler>().To<NexusServer>().InSingletonScope();
        }
    }
}
