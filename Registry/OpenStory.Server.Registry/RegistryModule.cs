using Ninject.Extensions.Factory;
using Ninject.Modules;
using OpenStory.Services;
using OpenStory.Services.Contracts;
using OpenStory.Services.Registry;

namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Registry module.
    /// </summary>
    public class RegistryModule : NinjectModule
    {
        /// <inheritdoc/>
        public override void Load()
        {
            Bind<IRegistryService>().To<RegistryService>();
            Bind<IServiceFactory<IRegistryService>>().ToFactory();

            Bind<IServiceHostFactory>().To<RegistryServiceHostFactory>();
        }
    }
}
