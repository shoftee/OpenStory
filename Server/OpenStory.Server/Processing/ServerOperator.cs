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
        private readonly IClientFactory<TClient> clientFactory;

        /// <summary>
        /// Gets the list of registered clients.
        /// </summary>
        protected List<TClient> Clients { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerOperator{TClient}"/>.
        /// </summary>
        protected ServerOperator(IClientFactory<TClient> clientFactory)
        {
            this.Clients = new List<TClient>();
            this.clientFactory = clientFactory;
        }

        /// <inheritdoc />
        public abstract void Configure(ServiceConfiguration configuration);
        
        /// <inheritdoc />
        public void RegisterSession(IServerSession session)
        {
            var client = this.clientFactory.CreateClient(session);
            this.Clients.Add(client);
        }
    }
}
