using OpenStory.Framework.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Provides methods for creating game client instances.
    /// </summary>
    /// <typeparam name="TClient">The type of the game clients.</typeparam>
    public interface IGameClientFactory<out TClient>
        where TClient : ClientBase
    {
        /// <summary>
        /// Creates a new game client.
        /// </summary>
        /// <param name="serverSession">The underlying session for the new client.</param>
        /// <returns>the new <typeparamref name="TClient"/> instance.</returns>
        TClient CreateClient(IServerSession serverSession);
    }
}