namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Provides methods for creating server sessions.
    /// </summary>
    public interface IServerSessionFactory
    {
        /// <summary>
        /// Creates a server session on top of the provided socket.
        /// </summary>
        /// <returns>a new <see cref="IServerSession"/> object.</returns>
        IServerSession CreateSession();
    }
}