using System.Security.Cryptography;
using System.Text;
using OpenStory.Common;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Provides methods for the legacy login password encryption.
    /// </summary>
    public static class LoginCrypto
    {
        private static readonly MD5CryptoServiceProvider Md5CryptoProvider =
            new MD5CryptoServiceProvider();

        private static RSA GetRsaWithParameters(RSAParameters parameters)
        {
            RSA rsa = RSA.Create();
            rsa.ImportParameters(parameters);
            return rsa;
        }

        /// <summary>
        /// Returns an MD5 hash string for a given string.
        /// </summary>
        /// <param name="str">The string to hash.</param>
        /// <param name="lowercase">Whether to return lowercase hex digits or not. Default value is <see langword="false"/>.</param>
        /// <returns>the MD5 hash of the provided string.</returns>
        public static string GetMd5HashString(string str, bool lowercase = false)
        {
            byte[] bytes = Encoding.UTF7.GetBytes(str);
            byte[] hashed = Md5CryptoProvider.ComputeHash(bytes);
            return hashed.ToHex(lowercase);
        }

        /// <summary>
        /// Returns an RSA-encrypted MD5 hash of the given password, using the given <see cref="RSAParameters"/>.
        /// </summary>
        /// <param name="password">The password string to encrypt.</param>
        /// <param name="parameters">The parameters to use for the encryption.</param>
        /// <returns>the encrypted string.</returns>
        public static string RsaEncryptPassword(string password, RSAParameters parameters)
        {
            string passwordHash = GetMd5HashString(password, true);
            byte[] passwordHashBytes = Encoding.UTF7.GetBytes(passwordHash);

            using (var rsa = GetRsaWithParameters(parameters))
            {
                byte[] encryptedHashBytes = rsa.EncryptValue(passwordHashBytes);
                return encryptedHashBytes.ToHex();
            }
        }

        /// <summary>
        /// Generates an integer for the "patch location" value sent during handshake.
        /// </summary>
        /// <param name="version">The game version.</param>
        /// <param name="subversion">The game sub-version.</param>
        /// <param name="remove">Whether to remove. Something.</param>
        /// <returns>the generated patch location number.</returns>
        public static int GeneratePatchLocation(short version, byte subversion, bool remove)
        {
            // Thanks to Diamondo25 for this.
            int location = 0;
            location ^= version & 0x7FFF;
            if (remove)
            {
                location ^= 0x8000;
            }

            location ^= subversion << 16;
            return location;
        }
    }
}
