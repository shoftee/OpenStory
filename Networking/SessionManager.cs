using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace OpenMaple.Networking
{
    partial class SessionManager
    {
        private const int PoolCapacity = 200;

        private readonly ConcurrentBag<NetworkSession> pool;

        private static readonly SessionManager Instance = new SessionManager();
        private SessionManager()
        {
            this.pool = new ConcurrentBag<NetworkSession>();
        }

        /// <summary>
        /// Provides a new session instance for the given socket.
        /// </summary>
        /// <param name="socket">The socket to bind the new session instance to.</param>
        /// <returns>A new session bound to <paramref name="socket"/>.</returns>
        /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="socket"/> is null.</exception>
        public static INetworkSession GetNewSession(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            NetworkSession networkSession;
            if (Instance.pool.IsEmpty || !Instance.pool.TryTake(out networkSession))
            {
                networkSession = new NetworkSession();
            }
            networkSession.Open(socket);
            return networkSession;
        }
    }
}