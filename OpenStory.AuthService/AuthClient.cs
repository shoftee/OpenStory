using System;
using OpenStory.Common.Authentication;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Server;
using OpenStory.ServiceModel;

namespace OpenStory.AuthService
{
    /// <summary>
    /// Represents a client for the Authentication Server.
    /// </summary>
    sealed class AuthClient : AbstractClient
    {
        /// <summary>
        /// Denotes the maximum number of allowed failed 
        /// login attempts before the client is disconnected.
        /// </summary>
        private const int MaxLoginAttempts = 3;

        private IAuthServer authServer;

        /// <summary>
        /// Initializes a new instance of AuthenticationClient 
        /// and binds it with a network session.
        /// </summary>
        /// <param name="networkSession">The network session to bind the new AuthenticationClient to.</param>
        /// <param name="authServer">The authentication server instance which is handling this client.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="authServer"/> is <c>null</c>.
        /// </exception>
        public AuthClient(ServerSession networkSession, IAuthServer authServer)
            : base(networkSession)
        {
            if (authServer == null) throw new ArgumentNullException("authServer");

            this.LoginAttempts = 0;
            this.IsAuthenticated = false;
            this.State = AuthClientState.PreAuthentication;

            this.authServer = authServer;
        }

        /// <summary>
        /// Gets the number of failed login attempts for this client.
        /// </summary>
        public int LoginAttempts { get; private set; }

        /// <summary>
        /// Gets whether the client has authenticated.
        /// </summary>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Gets the current state of the client.
        /// </summary>
        public AuthClientState State { get; private set; }

        protected override void ProcessPacket(ushort opCode, PacketReader reader)
        {
            switch (opCode)
            {
                case 0x0001:
                    this.HandleAuthentication(reader);
                    break;
                default:
                    Log.WriteWarning("Unknown Op Code 0x{0:X} - {1}", opCode,
                                     ByteHelpers.ByteToHex(reader.ReadFully()));
                    break;
            }
        }

        private void HandleAuthentication(PacketReader reader)
        {
            if (this.State != AuthClientState.PreAuthentication) goto Disconnect;

            string userName;
            if (!reader.TryReadLengthString(out userName)) goto Disconnect;

            string password;
            if (!reader.TryReadLengthString(out password)) goto Disconnect;

            // TODO: more stuff to read, later.
            IAccountSession accountSession;
            AuthenticationResult result = this.authServer.Authenticate(userName, password, out accountSession);
            if (result == AuthenticationResult.Success)
            {
                this.IsAuthenticated = true;
                base.AccountSession = accountSession;
                this.State = AuthClientState.PostAuthentication;
            }
            else if (this.LoginAttempts++ > MaxLoginAttempts)
            {
                goto Disconnect;
            }

            using (var builder = new PacketBuilder(8))
            {
                // TODO: Proper op code storage.
                builder.WriteInt16(0x0000);
                builder.WriteInt32((int) result);
                builder.WriteInt16(0x0000);
                this.Session.WritePacket(builder.ToByteArray());
            }
            return;

            Disconnect:
            base.Disconnect();
        }
    }
}