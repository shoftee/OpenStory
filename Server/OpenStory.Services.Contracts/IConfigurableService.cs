namespace OpenStory.Services.Contracts
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
        void Configure(OsServiceConfiguration configuration);
    }
}
