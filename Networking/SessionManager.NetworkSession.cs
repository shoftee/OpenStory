using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using OpenMaple.Cryptography;
using OpenMaple.Tools;

namespace OpenMaple.Networking
{
    partial class SessionManager
    {
        class NetworkSession : INetworkSession
        {
            private static readonly short MapleVersion = Properties.Settings.Default.MapleVersion;

            private static readonly AtomicInteger RollingSessionId = new AtomicInteger(0);

            /// <summary>
            /// A unique ID for the current session.
            /// </summary>
            public int SessionId { get; private set; }

            /// <summary>
            /// The rolling AES encryption for the output stream.
            /// </summary>
            public AesEncryption SendCrypto { get; private set; }

            /// <summary>
            /// The rolling AES encryption for the input stream.
            /// </summary>
            public AesEncryption ReceiveCrypto { get; private set; }

            /// <summary>
            /// The IPv4 dotted-quad for the remote end-point of this session's socket.
            /// </summary>
            public string RemoteAddress { get; private set; }

            private Socket Socket
            {
                get
                {
                    return this.socket;
                }
                set
                {
                    this.socket = value;
                    if (value == null)
                    {
                        this.SessionId = -1;
                        this.RemoteAddress = String.Empty;
                    }
                    else
                    {
                        this.SessionId = RollingSessionId.Increment();
                        this.RemoteAddress = ((IPEndPoint) this.socket.RemoteEndPoint).Address.ToString();
                    }
                }
            }

            private Socket socket;

            /// <summary>
            /// Initializes a new NetworkSession.
            /// </summary>
            public NetworkSession()
            {
                short version = MapleVersion;

                byte[] sendIv = ByteUtils.GetFreshIv();
                this.SendCrypto = new AesEncryption(sendIv, (short) (0xFFFF - version));

                byte[] receiveIv = ByteUtils.GetFreshIv();
                this.ReceiveCrypto = new AesEncryption(receiveIv, version);

                this.Socket = null;
            }

            /// <summary>
            /// Writes the given packet to the output stream.
            /// </summary>
            /// <param name="packet">The packet to write.</param>
            /// <exception cref="InvalidOperationException">The exception is thrown if this session is not open.</exception>
            /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="packet"/> is null.</exception>
            public void Write(IPacket packet)
            {
                if (this.Socket == null) throw new InvalidOperationException("This session is not open.");
                if (packet == null) throw new ArgumentNullException("packet");
                throw new NotImplementedException();
            }

            /// <summary>
            /// Binds the NetworkSession to the given socket.
            /// </summary>
            /// <param name="clientSocket">The socket to bind the session to.</param>
            /// <exception cref="InvalidOperationException">The exception is thrown if this session is already open.</exception>
            /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="clientSocket"/> is null.</exception>
            public void Open(Socket clientSocket)
            {
                if (this.Socket != null) throw new InvalidOperationException("This session is already open.");
                if (clientSocket == null) throw new ArgumentNullException("clientSocket");
                //if (!clientSocket.Connected) throw new InvalidOperationException("This socket is not connected.");
                this.Socket = clientSocket;
            }

            /// <summary>
            /// Releases the session so it can be reused with a new socket.
            /// </summary>
            /// <exception cref="InvalidOperationException">The exception is thrown if this session is not open.</exception>
            public void Close()
            {
                if (this.Socket == null) throw new InvalidOperationException("This session is not open.");
                this.Socket.Dispose();
                this.Socket = null;
                Release();
            }

            private void Release()
            {
                if (Instance.pool.Count < PoolCapacity)
                {
                    Instance.pool.Add(this);
                }
            }
        }
    }
}
