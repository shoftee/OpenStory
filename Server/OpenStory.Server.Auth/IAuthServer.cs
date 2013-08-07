using OpenStory.Common.Game;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Provides methods for querying an authentication server.
    /// </summary>
    public interface IAuthServer
    {
        /// <summary>
        /// Gets a <see cref="IWorld"/> instance by the World's ID.
        /// </summary>
        /// <param name="worldId">The ID of the world.</param>
        /// <returns>an <see cref="IWorld"/> object which represents the world with the given ID.</returns>
        IWorld GetWorldById(int worldId);
    }
}
