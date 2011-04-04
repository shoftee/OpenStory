using System.Security.Cryptography;
using System.Text;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Provides methods for the legacy login password encryption.
    /// </summary>
    public static class LoginCrypto
    {
        private static readonly MD5CryptoServiceProvider MD5CryptoProvider =
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
        /// <param name="lowercase">Whether to return lowercase hex digits or not. Default value is <c>false</c>.</param>
        /// <returns>A string </returns>
        public static string GetMD5HashString(string str, bool lowercase = false)
        {
            byte[] strBytes = Encoding.UTF7.GetBytes(str);
            byte[] hashBytes = MD5CryptoProvider.ComputeHash(strBytes);
            return ByteHelpers.ByteToHex(hashBytes, lowercase);
        }

        /// <summary>
        /// Returns an RSA-encrypted MD5 hash of the given password, using the given <see cref="RSAParameters"/>.
        /// </summary>
        /// <param name="password">The password string to encrypt.</param>
        /// <param name="parameters">The parameters to use for the encryption.</param>
        /// <returns>The encrypted string.</returns>
        public static string RsaEncryptPassword(string password, RSAParameters parameters)
        {
            RSA rsa = GetRsaWithParameters(parameters);

            string passwordHash = GetMD5HashString(password, true);
            byte[] passwordHashBytes = Encoding.UTF7.GetBytes(passwordHash);

            byte[] encryptedHashBytes = rsa.EncryptValue(passwordHashBytes);
            return ByteHelpers.ByteToHex(encryptedHashBytes);
        }
    }
}