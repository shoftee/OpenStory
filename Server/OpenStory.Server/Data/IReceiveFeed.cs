using System;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// Provides methods for an data output feed.
    /// </summary>
    /// <typeparam name="TSerializable">The type of data that is being received.</typeparam>
    public interface IReceiveFeed<in TSerializable>
    {
        /// <summary>
        /// Handles incoming data.
        /// </summary>
        /// <param name="data">The data to process.</param>
        void OnNext(TSerializable data);

        /// <summary>
        /// Handles incoming errors.
        /// </summary>
        /// <param name="error">The error that occured.</param>
        void OnError(Exception error);

        /// <summary>
        /// Handles feed clean-up.
        /// </summary>
        void OnCompleted();
    }
}
