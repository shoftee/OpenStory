using OpenStory.Framework.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Registers sessions for scheduling their packet processing.
    /// </summary>
    public interface IPacketScheduler
    {
        /// <summary>
        /// Registers a session for processing.
        /// </summary>
        /// <param name="session">The session to register.</param>
        void Register(IServerSession session);
    }
}