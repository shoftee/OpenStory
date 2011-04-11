using OpenStory.Common.Authentication;
using OpenStory.Server.Common;

namespace OpenStory.AuthService
{
    /// <summary>
    /// Provides methods for querying an auth server.
    /// </summary>
    interface IAuthServer : IAuthService
    {
        /// <summary>
        /// Gets a <see cref="IWorld"/> instance by the World's ID.
        /// </summary>
        /// <param name="worldId">The ID of the world.</param>
        /// <returns>An <see cref="IWorld"/> object which represents the world with the given ID.</returns>
        IWorld GetWorldById(int worldId);

        /// <summary>
        /// Attempts to Authenticate the given account login information.
        /// </summary>
        /// <param name="accountName">The name of the account.</param>
        /// <param name="password">The password for the account.</param>
        /// <param name="accountInfo">An <see cref="OpenStory.Server.Common.IAccountSession"/> variable to hold the resulting session.</param>
        /// <returns>An <see cref="AuthenticationResult"/> for the operation.</returns>
        AuthenticationResult Authenticate(string accountName, string password, out IAccountSession accountInfo);
    }
}