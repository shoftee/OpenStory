using System;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for creating client channels to services.
    /// </summary>
    /// <typeparam name="TChannel">The type of the service.</typeparam>
    public interface IServiceClientProvider<out TChannel>
        where TChannel : class
    {
        /// <summary>
        /// Gets a service channel using discovery.
        /// </summary>
        TChannel CreateChannel();
    }
}