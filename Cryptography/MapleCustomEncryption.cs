using OpenMaple.Tools;

namespace OpenMaple.Cryptography {
    class MapleCustomEncryption {

        public static byte[] Encrypt(byte[] data) {
            for (int j = 0; j < 6; j++) {
                byte remember = 0;
                byte dataLength = (byte) (data.Length & 0xFF);
                if (j % 2 == 0) {
                    for (int i = 0; i < data.Length; i++) {
                        byte cur = data[i];
                        cur = ByteUtils.RollLeft(cur, 3);
                        cur += dataLength;
                        cur ^= remember;
                        remember = cur;
                        cur = ByteUtils.RollRight(cur, dataLength & 0xFF);
                        cur = ((byte) ((~cur) & 0xFF));
                        cur += 0x48;
                        dataLength--;
                        data[i] = cur;
                    }
                } else {
                    for (int i = data.Length - 1; i >= 0; i--) {
                        byte cur = data[i];
                        cur = ByteUtils.RollLeft(cur, 4);
                        cur += dataLength;
                        cur ^= remember;
                        remember = cur;
                        cur ^= 0x13;
                        cur = ByteUtils.RollRight(cur, 3);
                        dataLength--;
                        data[i] = cur;
                    }
                }
            }
            return data;
        }

        public static byte[] Decrypt(byte[] data) {
            for (int j = 1; j <= 6; j++) {
                byte remember = 0;
                byte dataLength = (byte) (data.Length & 0xFF);
                byte nextRemember = 0;

                if (j % 2 == 0) {
                    for (int i = 0; i < data.Length; i++) {
                        byte cur = data[i];
                        cur -= 0x48;
                        cur = ((byte) ((~cur) & 0xFF));
                        cur = ByteUtils.RollLeft(cur, dataLength & 0xFF);
                        nextRemember = cur;
                        cur ^= remember;
                        remember = nextRemember;
                        cur -= dataLength;
                        cur = ByteUtils.RollRight(cur, 3);
                        data[i] = cur;
                        dataLength--;
                    }
                } else {
                    for (int i = data.Length - 1; i >= 0; i--) {
                        byte cur = data[i];
                        cur = ByteUtils.RollLeft(cur, 3);
                        cur ^= 0x13;
                        nextRemember = cur;
                        cur ^= remember;
                        remember = nextRemember;
                        cur -= dataLength;
                        cur = ByteUtils.RollRight(cur, 4);
                        data[i] = cur;
                        dataLength--;
                    }
                }
            }
            return data;
        }
    }
}
