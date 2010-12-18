using System;
using System.Security.Cryptography;
using OpenMaple.Tools;

namespace OpenMaple.Cryptography
{
    static class RSAUtils
    {
        public static RSACryptoServiceProvider GetRSACrypto(string modulus)
        {
            if (modulus.Length != 256)
            {
                throw new ArgumentException("Argument modulus must be a string of 256 hex digits.");
            }
            RSAParameters key = new RSAParameters
            {
                Modulus = ByteUtils.HexToByte(modulus),
                Exponent = new byte[] { 0x11 }
            };
            RSACryptoServiceProvider crypto = new RSACryptoServiceProvider();
            crypto.ImportParameters(key);
            return crypto;
        }

        public static RSACryptoServiceProvider GetRSACrypto(byte[] modulus)
        {
            if (modulus.Length != 128)
            {
                throw new ArgumentException("Argument modulus must be an array of 128 bytes.");
            }
            RSAParameters key = new RSAParameters
            {
                Modulus = modulus,
                Exponent = new byte[] { 0x11 }
            };
            RSACryptoServiceProvider crypto = new RSACryptoServiceProvider();
            crypto.ImportParameters(key);
            return crypto;
        }

        public static byte[] EncryptPassword(RSACryptoServiceProvider crypto, string passwordString)
        {
            /* 1. take password as string
             * 2. get bytes for string
             * 3. md5 compute hash
             * 4. get hex'ed string from 3
             * 5. lower case it (dont know if this is required, but im doing it atm)
             * 6. get bytes for string
             * 7. encrypt with rsa
             * 8. get string from bytes */
            System.Text.UTF7Encoding encoder = new System.Text.UTF7Encoding();
            MD5CryptoServiceProvider hash = new MD5CryptoServiceProvider();
            byte[] passwordBytes = encoder.GetBytes(passwordString);
            passwordBytes = hash.ComputeHash(passwordBytes);
            string lowercase = ByteUtils.ByteToHex(passwordBytes).ToLower();
            return crypto.Encrypt(ByteUtils.StringToByte(lowercase), true);
        }
    }
}
