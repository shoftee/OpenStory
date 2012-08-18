using OpenStory.Common.Authentication;

namespace OpenStory.Server.Authentication
{
    /// <summary>
    /// Provides authentication methods.
    /// </summary>
    /// <typeparam name="TCredentials">The type of objects that will be used as credentials.</typeparam>
    public interface IAuthPolicy<in TCredentials>
    {
        /// <summary>
        /// Attempts to authenticate the given account credentials.
        /// </summary>
        /// <param name="credentials">The name of the account.</param>
        /// <param name="session">An <see cref="IAccountSession"/> variable to hold the resulting session.</param>
        /// <returns>an <see cref="AuthenticationResult"/> for the operation.</returns>
        AuthenticationResult Authenticate(TCredentials credentials, out IAccountSession session);
    }
}
