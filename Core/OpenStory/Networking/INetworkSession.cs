﻿using System;

namespace OpenStory.Networking
{
    /// <summary>
    /// Provides some network-related stuff.
    /// </summary>
    public interface INetworkSession
    {
        /// <summary>
        /// Occurs when the session begins closing.
        /// </summary>
        event EventHandler<ConnectionClosingEventArgs> Closing;

        /// <summary>
        /// Occurs when there's a connection error.
        /// </summary>
        event EventHandler<SocketErrorEventArgs> SocketError;

        /// <summary>
        /// Closes the session.
        /// </summary>
        /// <param name="reason">The reason for closing the session.</param>
        void Close(string reason);
    }
}