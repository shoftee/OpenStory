using OpenStory.Common.Auth;
using OpenStory.Common.Data;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Provides methods for querying an auth server.
    /// </summary>
    internal interface IAuthServer
    {
        /// <summary>
        /// Gets the <see cref="IOpCodeTable"/> for this server.
        /// </summary>
        IOpCodeTable OpCodes { get; }

        /// <summary>
        /// Gets a <see cref="IWorld"/> instance by the World's ID.
        /// </summary>
        /// <param name="worldId">The ID of the world.</param>
        /// <returns>An <see cref="IWorld"/> object which represents the world with the given ID.</returns>
        IWorld GetWorldById(int worldId);

        /// <summary>
        /// Retrieves the authentication policy for this <see cref="IAuthServer"/>.
        /// </summary>
        /// <returns>an instance of <see cref="OpenStory.Server.Auth.IAuthPolicy{TCredentials}"/>.</returns>
        IAuthPolicy<SimpleCredentials> GetAuthPolicy();
    }
}