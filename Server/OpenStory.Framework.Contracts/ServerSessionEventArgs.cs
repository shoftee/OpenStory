using System;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Contains details for a <see cref="IServerSession"/>-related event.
    /// </summary>
    public class ServerSessionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the related <see cref="IServerSession"/> object.
        /// </summary>
        public IServerSession ServerSession { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSessionEventArgs"/> class.
        /// </summary>
        /// <param name="serverSession">The <see cref="IServerSession"/> instance.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="serverSession"/> is <see langword="null"/>.
        /// </exception>
        public ServerSessionEventArgs(IServerSession serverSession)
        {
            if (serverSession == null)
            {
                throw new ArgumentNullException(nameof(serverSession));
            }

            ServerSession = serverSession;
        }
    }
}