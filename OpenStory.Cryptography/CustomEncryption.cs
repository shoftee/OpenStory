namespace OpenStory.Cryptography
{
    public static class CustomEncryption
    {
        public static void Encrypt(byte[] data)
        {
            int length = data.Length;
            for (int j = 0; j < 6; j++)
            {
                byte remember = 0;
                var lengthByte = unchecked((byte) length);
                byte current;
                if ((j & 1) != 0)
                {
                    for (int i = length - 1; i >= 0; i--)
                    {
                        current = ByteUtils.RollLeft(data[i], 4);
                        current += lengthByte;

                        current ^= remember;
                        remember = current;

                        current ^= 0x13;
                        current = ByteUtils.RollRight(current, 3);
                        data[i] = current;

                        lengthByte--;
                    }
                }
                else
                {
                    for (int i = 0; i < length; i++)
                    {
                        current = ByteUtils.RollLeft(data[i], 3);
                        current += lengthByte;

                        current ^= remember;
                        remember = current;

                        current = ByteUtils.RollRight(current, lengthByte);
                        current = (byte) (~current & 0xFF);
                        current += 0x48;
                        data[i] = current;

                        lengthByte--;
                    }
                }
            }
        }

        public static void Decrypt(byte[] data)
        {
            int length = data.Length;
            for (int j = 1; j <= 6; j++)
            {
                byte remember = 0;
                var dataLength = unchecked((byte) length);
                byte current;
                if ((j & 1) != 0)
                {
                    for (int i = length - 1; i >= 0; i--)
                    {
                        current = ByteUtils.RollLeft(data[i], 3);
                        current ^= 0x13;

                        byte tmp = current;
                        current ^= remember;
                        remember = tmp;

                        current -= dataLength;
                        data[i] = ByteUtils.RollRight(current, 4);

                        dataLength--;
                    }
                }
                else
                {
                    for (int i = 0; i < length; i++)
                    {
                        current = data[i];
                        current -= 0x48;
                        current = unchecked((byte) (~current));
                        current = ByteUtils.RollLeft(current, dataLength);

                        byte tmp = current;
                        current ^= remember;
                        remember = tmp;

                        current -= dataLength;
                        data[i] = ByteUtils.RollRight(current, 3);

                        dataLength--;
                    }
                }
            }
        }
    }
}