namespace OpenStory.Common.Game
{
    /// <summary>
    /// The results of PIN operations.
    /// </summary>
    public enum PinResponseType
    {
        /// <summary>
        /// The specified PIN was accepted.
        /// </summary>
        [PacketValue(0)]
        PinAccepted,

        /// <summary>
        /// Set the specified PIN.
        /// </summary>
        [PacketValue(1)]
        SetPin,

        /// <summary>
        /// The entered PIN was invalid.
        /// </summary>
        [PacketValue(2)]
        InvalidPin,

        /// <summary>
        /// Check the entered PIN.
        /// </summary>
        [PacketValue(4)]
        CheckPin,
    }
}
