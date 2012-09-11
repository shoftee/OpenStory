namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Provides properties and methods for registration and notification of player groups.
    /// </summary>
    /// <typeparam name="TUpdateInfo">The type that will be used for notification information.</typeparam>
    public interface IPlayerGroup<TUpdateInfo> : IPlayerGroup
    {
        /// <summary>
        /// Processes an update to the group state.
        /// </summary>
        /// <param name="updateInfo">The object containing the update information.</param>
        void Update(TUpdateInfo updateInfo);
    }
}
