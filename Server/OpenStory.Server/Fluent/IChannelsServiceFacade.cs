using System.ComponentModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides a fluent interface for managing channel services.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IChannelsServiceFacade : IFluentInterface
    {
        /// <summary>
        /// Gets the service reference for a specific channel.
        /// </summary>
        /// <param name="id">The identifier of the channel.</param>
        IChannelService Get(int id);
    }
}