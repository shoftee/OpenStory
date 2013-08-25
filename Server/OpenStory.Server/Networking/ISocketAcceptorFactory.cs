using System.Net;

namespace OpenStory.Server.Networking
{
    /// <summary>
    /// Provides methods for creating <see cref="SocketAcceptor"/> objects.
    /// </summary>
    public interface ISocketAcceptorFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SocketAcceptor"/> class.
        /// </summary>
        SocketAcceptor CreateSocketAcceptor(IPEndPoint endpoint);
    }
}
