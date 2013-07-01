using OpenStory.Common.Game;
using OpenStory.Server.Auth.Policy;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Provides methods for querying an authentication server.
    /// </summary>
    internal interface IAuthServer : IGameServer
    {
        /// <summary>
        /// Gets a <see cref="IWorld"/> instance by the World's ID.
        /// </summary>
        /// <param name="worldId">The ID of the world.</param>
        /// <returns>an <see cref="IWorld"/> object which represents the world with the given ID.</returns>
        IWorld GetWorldById(int worldId);

        /// <summary>
        /// Retrieves the authentication policy for this <see cref="IAuthServer"/>.
        /// </summary>
        /// <returns>an instance of <see cref="IAuthPolicy{TCredentials}"/>.</returns>
        IAuthPolicy<SimpleCredentials> GetAuthPolicy();
    }
}
