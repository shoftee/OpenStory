using OpenStory.Server.Login;

namespace OpenStory.Server.Emulation.Login
{
    /// <summary>
    /// Provides methods for querying a login server.
    /// </summary>
    public interface ILoginServer
    {
        /// <summary>
        /// Gets a <see cref="IWorld"/> instance by the World's ID.
        /// </summary>
        /// <param name="worldId">The ID of the world.</param>
        /// <returns>An <see cref="IWorld"/> object which represents the world with the given ID.</returns>
        IWorld GetWorldById(int worldId);
    }
}