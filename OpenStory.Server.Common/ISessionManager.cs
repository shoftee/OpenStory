namespace OpenStory.Server.Common
{
    /// <summary>
    /// Provides methods for managing account sessions.
    /// </summary>
    public interface ISessionManager
    {
        /// <summary>
        /// Unregisters the session with the specified ID.
        /// </summary>
        /// <param name="sessionId">The session to unregister.</param>
        void UnregisterSession(int sessionId);
    }
}