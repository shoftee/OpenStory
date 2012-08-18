using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Services.Auth
{
    /// <summary>
    /// Represents a pair of simple account credentials.
    /// </summary>
    internal sealed class SimpleCredentials
    {
        /// <summary>
        /// Gets the account name.
        /// </summary>
        public string AccountName { get; private set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleCredentials"/>.
        /// </summary>
        /// <param name="accountName">The account name.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="ArgumentNullException">Throws if any of the provided parameters is <c>null</c>.</exception>
        public SimpleCredentials(string accountName, string password)
        {
            if (accountName == null)
            {
                throw new ArgumentNullException("accountName");
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            this.AccountName = accountName;
            this.Password = password;
        }
    }
}
