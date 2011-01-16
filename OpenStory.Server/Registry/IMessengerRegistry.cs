namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Provides methods for creation and access of Messenger sessions.
    /// </summary>
    public interface IMessengerRegistry
    {
        /// <summary>
        /// Creates a new messenger session.
        /// </summary>
        /// <param name="initiator">The initiator of the messenger session.</param>
        /// <returns>An <see cref="IMessenger"/> object representing the new session.</returns>
        IMessenger CreateMessenger(IPlayer initiator);

        /// <summary>
        /// Gets a messenger session by its ID.
        /// </summary>
        /// <param name="messengerId">The ID of the messenger session to query.</param>
        /// <returns>An <see cref="IMessenger"/> object representing the session if it was found.</returns>
        IMessenger GetById(int messengerId);
    }
}