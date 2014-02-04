using OpenStory.Framework.Contracts;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Provides methods for operating with server network sessions.
    /// </summary>
    public interface IServerOperator : IConfigurableService
    {
        /// <summary>
        /// Registers a new session into the server.
        /// </summary>
        /// <param name="session">The server session to register.</param>
        void RegisterSession(IServerSession session);
    }
}
