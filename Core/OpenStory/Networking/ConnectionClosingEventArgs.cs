using System;

namespace OpenStory.Networking
{
    /// <summary>
    /// Describes a reason for the connection being closed.
    /// </summary>
    public class ConnectionClosingEventArgs : EventArgs
    {
        internal static readonly ConnectionClosingEventArgs NoReason = new ConnectionClosingEventArgs("(no reason supplied)");

        /// <summary>
        /// Gets the reason for the event.
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionClosingEventArgs"/> class.
        /// </summary>
        internal ConnectionClosingEventArgs(string reason)
        {
            Guard.NotNullOrEmpty(() => reason, reason);
            this.Reason = reason;
        }
    }
}