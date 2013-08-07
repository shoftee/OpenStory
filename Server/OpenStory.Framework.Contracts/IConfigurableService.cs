namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Provides methods for configuring a service.
    /// </summary>
    public interface IConfigurableService
    {
        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="configuration">The configuration object.</param>
        void Configure(ServiceConfiguration configuration);
    }
}
