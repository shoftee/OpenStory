using System.Security.Cryptography;
using System.Text;

namespace OpenStory.Cryptography
{
    public static class LoginCrypto
    {
        private static readonly MD5CryptoServiceProvider MD5CryptoProvider;

        static LoginCrypto()
        {
            MD5CryptoProvider = new MD5CryptoServiceProvider();
        }

        public static string GetAuthenticationHash(string username, string password)
        {
            string str = username.ToLowerInvariant() + " " + password;
            return GetMD5HashString(str);
        }

        private static string GetMD5HashString(string str)
        {
            byte[] strBytes = Encoding.UTF7.GetBytes(str);
            byte[] hashBytes = MD5CryptoProvider.ComputeHash(strBytes);
            return ByteUtils.ByteToHex(hashBytes);
        }
    }
}