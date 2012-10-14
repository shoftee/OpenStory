using System.Runtime.Serialization;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// Provides operations for a data input feed.
    /// </summary>
    /// <typeparam name="TSerializable">The type of data that is being sent.</typeparam>
    public interface ISendFeed<in TSerializable>
    {
        /// <summary>
        /// Pushes the provided data into the feed.
        /// </summary>
        /// <param name="data">The data to push.</param>
        void Push(TSerializable data);
    }
}
