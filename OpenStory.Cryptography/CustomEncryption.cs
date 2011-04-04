using System;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Provides encryption and decryption static methods for the MapleStory custom data transformation.
    /// </summary>
    public static class CustomEncryption
    {
        /// <summary>
        /// Encrypts an array in-place.
        /// </summary>
        /// <remarks>
        /// The array given in <paramref name="data"/> will be modified.
        /// </remarks>
        /// <param name="data">The array to encrypt.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data" /> is <c>null</c>.</exception>
        public static void Encrypt(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");

            int length = data.Length;
            byte truncatedLength = unchecked((byte) length);
            for (int j = 0; j < 6; j++)
            {
                if ((j & 1) != 0)
                {
                    OddEncryptTransform(data, length, truncatedLength);
                }
                else
                {
                    EvenEncryptTransform(data, length, truncatedLength);
                }
            }
        }

        /// <summary>
        /// Performs the encryption transform for the even case.
        /// </summary>
        /// <param name="data">The data to transform.</param>
        /// <param name="length">The length of the data to transform.</param>
        /// <param name="lengthByte">An initial length byte for the transformation.</param>
        private static void EvenEncryptTransform(byte[] data, int length, byte lengthByte)
        {
            byte remember = 0;
            for (int i = 0; i < length; i++)
            {
                byte current = ByteHelpers.RollLeft(data[i], 3);
                current += lengthByte;

                current ^= remember;
                remember = current;

                current = ByteHelpers.RollRight(current, lengthByte);
                current = (byte) (~current & 0xFF);
                current += 0x48;
                data[i] = current;

                lengthByte--;
            }
        }

        /// <summary>
        /// Performs the encryption transform for the odd case.
        /// </summary>
        /// <param name="data">The data to transform.</param>
        /// <param name="length">The length of the data to transform.</param>
        /// <param name="lengthByte">An initial length byte for the transformation.</param>
        private static void OddEncryptTransform(byte[] data, int length, byte lengthByte)
        {
            byte remember = 0;
            for (int i = length - 1; i >= 0; i--)
            {
                byte current = ByteHelpers.RollLeft(data[i], 4);
                current += lengthByte;

                current ^= remember;
                remember = current;

                current ^= 0x13;
                current = ByteHelpers.RollRight(current, 3);
                data[i] = current;

                lengthByte--;
            }
        }

        /// <summary>
        /// Decrypts an array in-place.
        /// </summary>
        /// <remarks>
        /// The array given in <paramref name="data"/> will be modified.
        /// </remarks>
        /// <param name="data">The array to decrypt.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data" /> is <c>null</c>.</exception>
        public static void Decrypt(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
           
            int length = data.Length;
            byte truncatedLength = unchecked((byte) length);
            for (int j = 1; j <= 6; j++)
            {
                if ((j & 1) != 0)
                {
                    OddDecryptTransform(data, length, truncatedLength);
                }
                else
                {
                    EvenDecryptTransform(data, length, truncatedLength);
                }
            }
        }

        /// <summary>
        /// Performs the decryption transform for the even case.
        /// </summary>
        /// <param name="data">The data to transform.</param>
        /// <param name="length">The length of the data to transform.</param>
        /// <param name="lengthByte">An initial length byte for the transformation.</param>
        private static void EvenDecryptTransform(byte[] data, int length, byte lengthByte)
        {
            byte remember = 0;
            for (int i = 0; i < length; i++)
            {
                byte current = data[i];
                current -= 0x48;
                current = unchecked((byte) (~current));
                current = ByteHelpers.RollLeft(current, lengthByte);

                byte tmp = current;
                current ^= remember;
                remember = tmp;

                current -= lengthByte;
                data[i] = ByteHelpers.RollRight(current, 3);

                lengthByte--;
            }
        }

        /// <summary>
        /// Performs the decryption transform for the odd case.
        /// </summary>
        /// <param name="data">The data to transform.</param>
        /// <param name="length">The length of the data to transform.</param>
        /// <param name="lengthByte">An initial length byte for the transformation.</param>
        private static void OddDecryptTransform(byte[] data, int length, byte lengthByte)
        {
            byte remember = 0;
            for (int i = length - 1; i >= 0; i--)
            {
                byte current = ByteHelpers.RollLeft(data[i], 3);
                current ^= 0x13;

                byte tmp = current;
                current ^= remember;
                remember = tmp;

                current -= lengthByte;
                data[i] = ByteHelpers.RollRight(current, 4);

                lengthByte--;
            }
        }
    }
}