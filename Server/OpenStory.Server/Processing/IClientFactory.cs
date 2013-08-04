using OpenStory.Framework.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Provides methods for creating clients.
    /// </summary>
    /// <typeparam name="TClient">The type of the client, derived from <see cref="ClientBase"/>.</typeparam>
    public interface IClientFactory<out TClient>
        where TClient : ClientBase
    {
        /// <summary>
        /// Creates a <typeparamref name="TClient"/> instance for the provided <see cref="IServerSession"/>.
        /// </summary>
        /// <param name="session">The <see cref="IServerSession"/> to create a client for.</param>
        /// <returns>The created <typeparamref name="TClient"/>.</returns>
        TClient CreateClient(IServerSession session);
    }
}
