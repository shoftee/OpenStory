using Ninject.Modules;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Wcf
{
    /// <summary>
    /// Represents a ninject module for WCF stuff.
    /// </summary>
    public class WcfServiceModule : NinjectModule
    {
        /// <inheritdoc/>
        public override void Load()
        {
            Bind<NexusConnectionInfo>().ToProvider<EnvironmentNexusConnectionProvider>();
            Bind<IBootstrapper>().To<WcfBootstrapper>();
        }
    }
}