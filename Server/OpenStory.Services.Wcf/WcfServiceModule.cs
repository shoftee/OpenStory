using Ninject.Modules;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Wcf
{
    /// <summary>
    /// Adds WCF dependencies!
    /// </summary>
    public class WcfServiceModule : NinjectModule
    {
        /// <inheritdoc/>
        public override void Load()
        {
            Bind<IServiceClientProvider<IRegistryService>>().To<ServiceClientProvider<IRegistryService>>();
            Bind<IBootstrapper>().To<WcfBootstrapper>().InSingletonScope();
        }
    }
}
