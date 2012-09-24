using OpenStory.Services.Contracts;

namespace OpenStory.Services
{
    /// <summary>
    /// Represents a base class for game services.
    /// </summary>
    public abstract class GameServiceBase : RegisteredServiceBase
    {
        /// <summary>
        /// Configures the game service.
        /// </summary>
        /// <param name="configuration">The configuration information.</param>
        /// <param name="error">A variable to hold a human-readable error message.</param>
        /// <returns><c>true</c> if configuration was successful; otherwise, <c>false</c>.</returns>
        public abstract bool Configure(ServiceConfiguration configuration, out string error);
    }
}