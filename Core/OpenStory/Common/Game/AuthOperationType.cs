namespace OpenStory.Common.Game
{
    /// <summary>
    /// The type of an auth operation.
    /// </summary>
    public enum AuthOperationType
    {
        /// <summary>
        /// The operation for gender selection.
        /// </summary>
        [PacketValue(0x0A)]
        GenderSelect,

        /// <summary>
        /// The operation for PIN selection.
        /// </summary>
        [PacketValue(0x0B)]
        PinSelect,
    }
}