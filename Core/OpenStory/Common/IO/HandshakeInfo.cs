namespace OpenStory.Common.IO
{
    /// <summary>
    /// Represents handshake information.
    /// </summary>
    public abstract class HandshakeInfo
    {
        /// <summary>
        /// Gets or sets the 16-bit header for the handshake.
        /// </summary>
        public ushort? Header { get; protected set; }

        /// <summary>
        /// Gets or sets the game version.
        /// </summary>
        public ushort Version { get; protected set; }

        /// <summary>
        /// Gets or sets the game sub-version.
        /// </summary>
        public string PatchLocation { get; protected set; }

        /// <summary>
        /// Gets or sets the Client IV to be used.
        /// </summary>
        public byte[] ClientIv { get; protected set; }

        /// <summary>
        /// Gets or sets the Server IV to be used.
        /// </summary>
        public byte[] ServerIv { get; protected set; }

        /// <summary>
        /// Gets or sets the server locale identifier.
        /// </summary>
        public byte LocaleId { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeInfo"/> class.
        /// </summary>
        protected HandshakeInfo()
        {
        }
    }
}