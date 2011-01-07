using System;
using OpenMaple.Tools;

namespace OpenMaple.Cryptography
{
    class CustomEncryption
    {
        public static void Encrypt(byte[] data)
        {
            for (int j = 0; j < 6; j++)
            {
                byte remember = 0;
                byte dataLength = (byte) (data.Length & 0xFF);
                if ((j & 1) == 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        byte current = ByteUtils.RollLeft(data[i], 3);
                        current += dataLength;

                        current ^= remember;
                        remember = current;

                        current = ByteUtils.RollRight(current, dataLength & 0xFF);
                        current = (byte) (~current & 0xFF);
                        current += 0x48;
                        data[i] = current;

                        dataLength--;
                    }
                }
                else
                {
                    for (int i = data.Length - 1; i >= 0; i--)
                    {
                        byte current = ByteUtils.RollLeft(data[i], 4);
                        current += dataLength;

                        current ^= remember;
                        remember = current;

                        current ^= 0x13;
                        current = ByteUtils.RollRight(current, 3);
                        data[i] = current;

                        dataLength--;
                    }
                }
            }
        }

        public static void Decrypt(byte[] data)
        {
            for (int j = 1; j <= 6; j++)
            {
                byte remember = 0;
                byte dataLength = (byte) (data.Length & 0xFF);
                byte tmp;

                if ((j & 1) == 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        byte current = data[i];
                        current -= 0x48;
                        current = (byte) (~current & 0xFF);
                        current = ByteUtils.RollLeft(current, dataLength & 0xFF);

                        tmp = current;
                        current ^= remember;
                        remember = tmp;

                        current -= dataLength;
                        data[i] = ByteUtils.RollRight(current, 3);

                        dataLength--;
                    }
                }
                else
                {
                    for (int i = data.Length - 1; i >= 0; i--)
                    {
                        byte current = ByteUtils.RollLeft(data[i], 3);
                        current ^= 0x13;

                        tmp = current;
                        current ^= remember;
                        remember = tmp;

                        current -= dataLength;
                        data[i] = ByteUtils.RollRight(current, 4);

                        dataLength--;
                    }
                }
            }
        }
    }
}
