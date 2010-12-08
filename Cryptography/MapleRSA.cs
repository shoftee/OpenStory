using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Client {
    sealed class MapleRSA {
        private MapleRSA() { }
        public static RSACryptoServiceProvider gimmeRSACrypto(string modulus) {
            if (modulus.Length != 256)
                throw new ArgumentException("Argument modulus must be a string of 256 hex digits.");
            RSAParameters key = new RSAParameters();
            key.Modulus = Utils.HexToByte(modulus);
            key.Exponent = new byte[] { 0x11 };
            RSACryptoServiceProvider crypto = new RSACryptoServiceProvider();
            crypto.ImportParameters(key);
            return crypto;
        }

        public static RSACryptoServiceProvider gimmeRSACrypto(byte[] modulus) {
            if (modulus.Length != 128)
                throw new ArgumentException("Argument modulus must be an array of 128 bytes.");
            RSAParameters key = new RSAParameters();
            key.Modulus = modulus;
            key.Exponent = new byte[] { 0x11 };
            RSACryptoServiceProvider crypto = new RSACryptoServiceProvider();
            crypto.ImportParameters(key);
            return crypto;
        }

        public static byte[] EncryptPassword(RSACryptoServiceProvider crypto, string password) {
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
            byte[] passbytes = encoder.GetBytes(password);
            passbytes = hash.ComputeHash(passbytes);
            string secondstring = Utils.ByteToHex(passbytes).ToLower();
            return crypto.Encrypt(Utils.StringToByte(secondstring), true);
        }
    }
}
