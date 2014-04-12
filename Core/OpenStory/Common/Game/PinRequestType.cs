namespace OpenStory.Common.Game
{
    /// <summary>
    /// The type of the PIN request.
    /// </summary>
    public enum PinRequestType
    {
        /// <summary>
        /// A pin is not set and needs to be requested back from the server.
        /// </summary>
        [PacketValue(0)]
        PinNotSet,

        /// <summary>
        /// A pin is provided and needs to be validated.
        /// </summary>
        [PacketValue(1)]
        CheckPin,

        /// <summary>
        /// A pin is provided and needs to be validated before it can be modified.
        /// </summary>
        [PacketValue(2)]
        AssignPin,
    }
}
