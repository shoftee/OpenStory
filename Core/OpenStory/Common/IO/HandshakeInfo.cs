namespace OpenStory.Common.IO
{
    /// <summary>
    /// Represents handshake information.
    /// </summary>
    public class HandshakeInfo
    {
        /// <summary>
        /// Gets or sets the 16-bit header for the handshake.
        /// </summary>
        public ushort Header { get; protected set; }

        /// <summary>
        /// Gets or sets the game version.
        /// </summary>
        public ushort Version { get; protected set; }

        /// <summary>
        /// Gets or sets the game sub-version.
        /// </summary>
        public string SubVersion { get; protected set; }

        /// <summary>
        /// Gets or sets the Client IV to be used.
        /// </summary>
        public byte[] ClientIv { get; protected set; }

        /// <summary>
        /// Gets or sets the Server IV to be used.
        /// </summary>
        public byte[] ServerIv { get; protected set; }

        /// <summary>
        /// Gets or sets the server identifier.
        /// </summary>
        public byte ServerId { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="HandshakeInfo"/>.
        /// </summary>
        public HandshakeInfo()
        {
        }

    }
}