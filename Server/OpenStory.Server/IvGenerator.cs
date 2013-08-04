using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OpenStory.Server
{
    /// <summary>
    /// A utility class that generates IV byte arrays.
    /// </summary>
    public static class IvGenerator
    {
        #region IV generation

        private static readonly RNGCryptoServiceProvider Crypto = new RNGCryptoServiceProvider();

        /// <summary>
        /// Returns a new non-zero 4-byte IV array.
        /// </summary>
        /// <returns>a generated 4-byte IV array.</returns>
        public static byte[] GetNewIv()
        {
            var iv = new byte[4];
            Crypto.GetNonZeroBytes(iv);
            return iv;
        }

        #endregion
    }
}
