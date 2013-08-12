using System.Collections.Generic;
using OpenStory.Framework.Contracts;
using OpenStory.Services;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// A base class for <see cref="IServerOperator"/> implementations.
    /// </summary>
    /// <typeparam name="TClient">The type of the clients to handle in this <see cref="IServerOperator"/>.</typeparam>
    public abstract class ServerOperator<TClient> : IServerOperator
        where TClient : ClientBase
    {
        /// <summary>
        /// Gets the list of registered clients.
        /// </summary>
        protected List<TClient> Clients { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerOperator{TClient}"/>.
        /// </summary>
        protected ServerOperator()
        {
            this.Clients = new List<TClient>();
        }

        /// <inheritdoc />
        public abstract void Configure(ServiceConfiguration configuration);
        
        /// <inheritdoc />
        public void RegisterSession(IServerSession session)
        {
            var client = this.CreateClient(session);
            this.Clients.Add(client);
        }

        /// <summary>
        /// Creates a client of type <typeparamref name="TClient"/>.
        /// </summary>
        /// <param name="session">The session to create a client for.</param>
        /// <returns>the constructed client for the session.</returns>
        protected abstract TClient CreateClient(IServerSession session);
    }
}
