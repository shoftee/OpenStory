using System;
using System.Security.Cryptography;
using System.Text;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Provides methods for the login password encryption.
    /// </summary>
    public static class LoginCrypto
    {
        private static readonly MD5CryptoServiceProvider MD5CryptoProvider =
            new MD5CryptoServiceProvider();

        /// <summary>
        /// Gets an MD5 hash of the user name and password.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="userName"/> or <paramref name="password" /> are <c>null</c>.
        /// </exception>
        /// <returns>A string of 32 uppercase hexadecimal digits representing the MD5 hash.</returns>
        public static string GetAuthenticationHash(string userName, string password)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (password == null) throw new ArgumentNullException("password");

            string str = userName.ToLowerInvariant() + " " + password;
            return GetMD5HashString(str);
        }

        private static string GetMD5HashString(string str, bool lowercase = false)
        {
            byte[] strBytes = Encoding.UTF7.GetBytes(str);
            byte[] hashBytes = MD5CryptoProvider.ComputeHash(strBytes);
            return ByteHelpers.ByteToHex(hashBytes, lowercase);
        }
    }
}