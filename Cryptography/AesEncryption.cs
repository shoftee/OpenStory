using System;
using System.Security.Cryptography;
using OpenMaple.Tools;

namespace OpenMaple.Cryptography
{
    class AesEncryption
    {
        // TODO: Load these from somewhere?
        // Magic!
        private static readonly byte[] TransformTable = new byte[] 
        { 
            0xEC, 0x3F, 0x77, 0xA4, 0x45, 0xD0,	0x71, 0xBF,  0xB7, 0x98, 0x20, 0xFC, 0x4B, 0xE9, 0xB3, 0xE1, 
            0x5C, 0x22, 0xF7, 0x0C, 0x44, 0x1B, 0x81, 0xBD,  0x63, 0x8D, 0xD4, 0xC3, 0xF2, 0x10, 0x19, 0xE0, 
            0xFB, 0xA1, 0x6E, 0x66, 0xEA, 0xAE, 0xD6, 0xCE,  0x06, 0x18, 0x4E, 0xEB, 0x78, 0x95, 0xDB, 0xBA, 
            0xB6, 0x42, 0x7A, 0x2A, 0x83, 0x0B, 0x54, 0x67,  0x6D, 0xE8, 0x65, 0xE7, 0x2F, 0x07, 0xF3, 0xAA, 
            0x27, 0x7B, 0x85, 0xB0, 0x26, 0xFD, 0x8B, 0xA9,  0xFA, 0xBE, 0xA8, 0xD7, 0xCB, 0xCC, 0x92, 0xDA, 
            0xF9, 0x93, 0x60, 0x2D, 0xDD, 0xD2, 0xA2, 0x9B,  0x39, 0x5F, 0x82, 0x21, 0x4C, 0x69, 0xF8, 0x31, 
            0x87, 0xEE, 0x8E, 0xAD, 0x8C, 0x6A, 0xBC, 0xB5,  0x6B, 0x59, 0x13, 0xF1, 0x04, 0x00, 0xF6, 0x5A, 
            0x35, 0x79, 0x48, 0x8F, 0x15, 0xCD, 0x97, 0x57,  0x12, 0x3E, 0x37, 0xFF, 0x9D, 0x4F, 0x51, 0xF5, 
            0xA3, 0x70, 0xBB, 0x14, 0x75, 0xC2, 0xB8, 0x72,  0xC0, 0xED, 0x7D, 0x68, 0xC9, 0x2E, 0x0D, 0x62, 
            0x46, 0x17, 0x11, 0x4D, 0x6C, 0xC4, 0x7E, 0x53,  0xC1, 0x25, 0xC7, 0x9A, 0x1C, 0x88, 0x58, 0x2C, 
            0x89, 0xDC, 0x02, 0x64, 0x40, 0x01, 0x5D, 0x38,  0xA5, 0xE2, 0xAF, 0x55, 0xD5, 0xEF, 0x1A, 0x7C, 
            0xA7, 0x5B, 0xA6, 0x6F, 0x86, 0x9F, 0x73, 0xE6,  0x0A, 0xDE, 0x2B, 0x99, 0x4A, 0x47, 0x9C, 0xDF, 
            0x09, 0x76, 0x9E, 0x30, 0x0E, 0xE4, 0xB2, 0x94,  0xA0, 0x3B, 0x34, 0x1D, 0x28, 0x0F, 0x36, 0xE3, 
            0x23, 0xB4, 0x03, 0xD8, 0x90, 0xC8, 0x3C, 0xFE,  0x5E, 0x32, 0x24, 0x50, 0x1F, 0x3A, 0x43, 0x8A, 
            0x96, 0x41, 0x74, 0xAC, 0x52, 0x33, 0xF0, 0xD9,  0x29, 0x80, 0xB1, 0x16, 0xD3, 0xAB, 0x91, 0xB9, 
            0x84, 0x7F, 0x61, 0x1E, 0xCF, 0xC5, 0xD1, 0x56,  0x3D, 0xCA, 0xF4, 0x05, 0xC6, 0xE5, 0x08, 0x49 
        };

        // More magic!
        private static readonly byte[] Key = 
        {
            0x13, 0x00, 0x00, 0x00,  0x08, 0x00, 0x00, 0x00,
            0x06, 0x00, 0x00, 0x00,  0xB4, 0x00, 0x00, 0x00,
            0x1B, 0x00, 0x00, 0x00,  0x0F, 0x00, 0x00, 0x00,
            0x33, 0x00, 0x00, 0x00,  0x52, 0x00, 0x00, 0x00 
        };

        private byte[] iv;
        private short version;

        public AesEncryption(byte[] iv, short gameVersion)
        {
            if (iv.Length != 4)
            {
                throw new ArgumentException("Argument iv does not have exactly 4 elements.");
            }
            this.iv = iv;

            // Flip the version back to little-endian.
            this.version = (short) (((gameVersion >> 8) & 0xFF) | ((gameVersion & 0xFF) << 8));
        }

        public byte[] Encrypt(byte[] data)
        {
            int length = data.Length;

            RijndaelManaged cipher = new RijndaelManaged
                                         {
                                             Padding = PaddingMode.None,
                                             Mode = CipherMode.ECB,
                                             Key = Key
                                         };

            ICryptoTransform decryptor = cipher.CreateEncryptor();
            int remaining = length;
            int llength = 0x05B0;
            int start = 0;

            byte[] transformed = new byte[length];
            Buffer.BlockCopy(data, 0, transformed, 0, length);
            while (remaining > 0)
            {
                // The AES IV has to be 128 bits or more.
                byte[] myIv = ByteUtils.MultiplyBytes(this.iv, 4, 4);
                int ivLength = myIv.Length;
                if (remaining < llength) llength = remaining;
                for (int i = start; i < (start + llength); i++)
                {
                    if ((i - start) % ivLength == 0)
                    {
                        byte[] newIv = decryptor.TransformFinalBlock(myIv, 0, ivLength);
                        for (int j = 0; j < ivLength; j++)
                        {
                            myIv[j] = newIv[j];
                        }
                    }
                    // xor the data against the IV
                    transformed[i] = (byte) (data[i] ^ (myIv[(i - start) % ivLength]));
                }
                start += llength;
                remaining -= llength;
                llength = 0x05B4;
            }
            this.Update();
            return data;
        }

        private void Update()
        {
            this.iv = TransformIv(this.iv);
        }

        public byte[] GetPacketHeader(int length)
        {
            // Kinky packet header transformations.
            uint iiv = (uint) ((this.iv[3] & 0xFF) | (this.iv[2] << 8) & 0xFF00);

            iiv ^= (uint) this.version;
            uint mlength = (((uint) length << 8) & 0xFF00) | ((uint) length >> 8);
            uint xoredIv = iiv ^ mlength;

            byte[] ret = new byte[4];
            ret[0] = (byte) ((iiv >> 8) & 0xFF);
            ret[1] = (byte) (iiv & 0xFF);
            ret[2] = (byte) ((xoredIv >> 8) & 0xFF);
            ret[3] = (byte) (xoredIv & 0xFF);
            return ret;
        }

        public static int GetPacketLength(byte[] packetHeader)
        {
            if (packetHeader.Length < 4)
            {
                throw new ArgumentException("Argument packetHeader does not have 4 elements.");
            }
            // More kinky packet header transformations.
            return (((packetHeader[0] ^ packetHeader[2]) & 0xFF) |
                (((packetHeader[1] ^ packetHeader[3]) << 8) & 0xFF00));
        }

        public bool CheckPacket(byte[] packet)
        {
            bool first = ((packet[0] ^ this.iv[2]) & 0xFF) == ((this.version >> 8) & 0xFF);
            bool second = (((packet[1] ^ this.iv[3]) & 0xFF) == (this.version & 0xFF));
            return first && second;
        }

        private static byte[] TransformIv(byte[] oldIv)
        {
            byte[] magic = { 0xf2, 0x53, 0x50, 0xc6 }; // magic byte array

            for (int i = 0; i < 4; i++)
            {
                TransformMagic(oldIv[i], ref magic);
            }
            return magic;
        }

        // Transforms the IV for the next packet, takes the magic array as a reference for convenience.
        private static void TransformMagic(byte input, ref byte[] magic)
        {
            byte elina = magic[1];
            byte anna = input;
            byte moritz = TransformTable[elina];
            moritz -= input;
            magic[0] += moritz; // magic[0] setto!

            moritz = magic[2];
            moritz ^= TransformTable[anna];
            elina -= moritz;
            magic[1] = elina; // magic[1] setto!

            elina = magic[3];
            moritz = elina;
            elina -= magic[0];
            moritz = TransformTable[moritz];
            moritz += input;
            moritz ^= magic[2];
            magic[2] = moritz; // magic[2] setto!

            elina += TransformTable[anna];
            magic[3] = elina; // magic[3] setto!

            // Gattai!
            uint merry = (uint) ((magic[0]) | (magic[1] << 8) | (magic[2] << 16) | (magic[3] << 24));
            uint returnValue = merry;
            returnValue >>= 0x1D;
            merry <<= 3;
            returnValue |= merry;
            magic[0] = (byte) (returnValue & 0xFF);
            magic[1] = (byte) ((returnValue >> 8) & 0xFF);
            magic[2] = (byte) ((returnValue >> 16) & 0xFF);
            magic[3] = (byte) ((returnValue >> 24) & 0xFF);
        }
    }
}
