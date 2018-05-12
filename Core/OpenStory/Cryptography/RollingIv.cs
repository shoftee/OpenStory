using System;
using OpenStory.Common;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents an AES encryption transformer.
    /// </summary>
    public sealed class RollingIv
    {
        private readonly ICryptoAlgorithm _algorithm;
        private readonly ushort _versionMask;

        private byte[] _iv;

        /// <summary>
        /// Initializes a new instance of the <see cref="RollingIv"/> class.
        /// </summary>
        /// <param name="algorithm">The <see cref="ICryptoAlgorithm"/> instance for this session.</param>
        /// <param name="initialIv">The initial IV for this instance.</param>
        /// <param name="versionMask">The version mask used for header creation.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="algorithm"/> or <paramref name="initialIv"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="initialIv"/> does not have exactly 4 elements.
        /// </exception>
        public RollingIv(ICryptoAlgorithm algorithm, byte[] initialIv, ushort versionMask)
        {
            Guard.NotNull(() => algorithm, algorithm);
            Guard.NotNull(() => initialIv, initialIv);

            if (initialIv.Length != 4)
            {
                throw new ArgumentException(CommonStrings.IvMustBe4Bytes, nameof(initialIv));
            }

            _algorithm = algorithm;
            _iv = initialIv.FastClone();

            // Flip the version mask.
            _versionMask = (ushort)((versionMask >> 8) | ((versionMask & 0xFF) << 8));
        }

        /// <summary>
        /// Transforms the specified data in-place.
        /// </summary>
        /// <param name="data">The array to transform. This array will be directly modified.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data" /> is <see langword="null"/>.</exception>
        public void Transform(byte[] data)
        {
            Guard.NotNull(() => data, data);

            _algorithm.TransformArraySegment(data, _iv, 0, data.Length);

            _iv = _algorithm.ShuffleIv(_iv);
        }

        /// <summary>
        /// Constructs a packet header.
        /// </summary>
        /// <param name="length">The length of the packet to make a header for.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="length"/> is less than 2.
        /// </exception>
        /// <returns>the 4-byte header for a packet with the specified length.</returns>
        public byte[] ConstructHeader(int length)
        {
            if (length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, CommonStrings.PacketLengthMustBeMoreThan2Bytes);
            }

            int encodedVersion = ((_iv[2] << 8) | _iv[3]) ^ _versionMask;
            int encodedLength = encodedVersion ^ (((length & 0xFF) << 8) | (length >> 8));

            var header = new byte[4];
            unchecked
            {
                header[0] = (byte)(encodedVersion >> 8);
                header[1] = (byte)encodedVersion;
                header[2] = (byte)(encodedLength >> 8);
                header[3] = (byte)encodedLength;
            }

            return header;
        }

        /// <summary>
        /// Reads a packet header from an array and extracts the packet's length.
        /// </summary>
        /// <param name="header">The array to read from.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="header"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="header"/> has less than 4 elements.
        /// </exception>
        /// <returns>the packet length extracted from the array.</returns>
        public static int GetPacketLength(byte[] header)
        {
            Guard.NotNull(() => header, header);

            if (header.Length < 4)
            {
                var message = string.Format(CommonStrings.SegmentTooShort, 4);
                throw new ArgumentException(message, nameof(header));
            }

            return ((header[1] ^ header[3]) << 8) | (header[0] ^ header[2]);
        }

        /// <summary>
        /// Determines whether the start of an array is a valid packet header.
        /// </summary>
        /// <param name="header">The raw packet data to validate.</param>
        /// <returns><see langword="true"/> if the header is valid; otherwise, <see langword="false"/>.</returns>
        public bool ValidateHeader(byte[] header)
        {
            Guard.NotNull(() => header, header);

            if (header.Length < 4)
            {
                var message = string.Format(CommonStrings.SegmentTooShort, 4);
                throw new ArgumentException(message, nameof(header));
            }

            return ValidateHeaderInternal(header, _iv, _versionMask);
        }

        /// <summary>
        /// Attempts to extract the length of a packet from its header.
        /// </summary>
        /// <remarks>
        /// When overriding this method in a derived class,
        /// do not call the base implementation.
        /// </remarks>
        /// <param name="header">The header byte array to process.</param>
        /// <param name="length">A variable to hold the result.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="header"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="header"/> has less than 4 elements.
        /// </exception>
        /// <returns><see langword="true"/> if the extraction was successful; otherwise, <see langword="false"/>.</returns>
        public bool TryGetLength(byte[] header, out int length)
        {
            Guard.NotNull(() => header, header);

            if (header.Length < 4)
            {
                var message = string.Format(CommonStrings.SegmentTooShort, 4);
                throw new ArgumentException(message, nameof(header));
            }

            if (ValidateHeaderInternal(header, _iv, _versionMask))
            {
                length = ((header[1] ^ header[3]) << 8) | (header[0] ^ header[2]);
                return true;
            }
            else
            {
                length = default(int);
                return false;
            }
        }

        private static bool ValidateHeaderInternal(byte[] header, byte[] iv, ushort versionMask)
        {
            ushort extractedVersion = GetVersionInternal(header, iv);
            return extractedVersion == versionMask;
        }

        /// <summary>
        /// Extracts a version from the header using a specified IV.
        /// </summary>
        /// <param name="header">The header byte array.</param>
        /// <param name="iv">The IV to use for the decoding.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="header"/>
        /// or <paramref name="iv"/> are <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="header"/> has less than 4 elements or
        /// if <paramref name="iv"/> doesn't have exactly 4 elements.
        /// </exception>
        /// <returns>the version mask encoded into the header.</returns>
        public static ushort GetVersion(byte[] header, byte[] iv)
        {
            Guard.NotNull(() => header, header);
            Guard.NotNull(() => iv, iv);

            if (header.Length < 4)
            {
                var message = string.Format(CommonStrings.SegmentTooShort, 4);
                throw new ArgumentException(message, nameof(header));
            }

            if (iv.Length != 4)
            {
                throw new ArgumentException(CommonStrings.IvMustBe4Bytes, nameof(iv));
            }

            return GetVersionInternal(header, iv);
        }

        private static ushort GetVersionInternal(byte[] header, byte[] iv)
        {
            var encodedVersion = (ushort)((header[0] << 8) | header[1]);
            var xorSegment = (ushort)((iv[2] << 8) | iv[3]);
            return (ushort)(encodedVersion ^ xorSegment);
        }
    }
}
