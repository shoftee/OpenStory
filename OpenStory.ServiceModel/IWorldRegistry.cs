using System.ServiceModel;

namespace OpenStory.ServiceModel
{
    /// <summary>
    /// Provides methods for targeted broadcasting to a whole game world.
    /// </summary>
    [ServiceContract]
    public interface IWorldRegistry
    {
        /// <summary>
        /// Broadcasts a notification.
        /// </summary>
        /// <param name="update">An encapsulated update to broadcast.</param>
        void Broadcast(IUpdate update);
    }
}
