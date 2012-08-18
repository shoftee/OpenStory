﻿using OpenStory.Common.Authentication;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Auth
{
    /// <summary>
    /// Provides methods for querying an auth server.
    /// </summary>
    internal interface IAuthServer
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
        /// <param name="accountInfo">An <see cref="IAccountSession"/> variable to hold the resulting session.</param>
        /// <returns>An <see cref="AuthenticationResult"/> for the operation.</returns>
        AuthenticationResult Authenticate(string accountName, string password, out IAccountSession accountInfo);
    }
}