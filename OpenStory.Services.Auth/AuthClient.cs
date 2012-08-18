using System;
using OpenStory.Common.Authentication;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Server;

namespace OpenStory.Services.Auth
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

        private readonly IAuthServer server;

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

        /// <summary>
        /// Initializes a new instance of <see cref="AuthClient"/>.
        /// and binds it with a network session.
        /// </summary>
        /// <param name="networkSession">The network session to bind the instance to.</param>
        /// <param name="server">The authentication server instance which is handling this client.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any of the provider parameters is <c>null</c>.
        /// </exception>
        public AuthClient(ServerSession networkSession, IAuthServer server)
            : base(networkSession)
        {
            if (networkSession == null)
            {
                throw new ArgumentNullException("networkSession");
            }
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            this.LoginAttempts = 0;
            this.IsAuthenticated = false;
            this.State = AuthClientState.PreAuthentication;

            this.server = server;
        }

        protected override void ProcessPacket(ushort opCode, PacketReader reader)
        {
            string label;
            if (!this.server.OpCodes.TryGetIncomingLabel(opCode, out label))
            {
                Log.WriteWarning("Unknown Op Code 0x{0:X} - {1}", opCode,
                                 reader.ReadFully().ToHex());
                return;
            }

            switch (label)
            {
                case "Authenticate":
                    this.HandleAuthentication(reader);
                    break;
                case "ValidatePin":
                    this.HandlePinValidation(reader);
                    break;
                case "AssignPin":
                    this.HandlePinAssignment(reader);
                    break;
                case "WorldListRequest":
                case "WorldListRefresh":
                    this.HandleWorldListRequest(reader);
                    break;
                case "ChannelSelect":
                    this.HandleChannelSelect(reader);
                    break;
                case "CharacterListRequest":
                    this.HandleCharacterListRequest(reader);
                    break;
                case "CharacterSelect":
                    this.HandleCharacterSelect(reader);
                    break;
            }
        }

        private void HandleCharacterSelect(PacketReader reader)
        {
            throw new NotImplementedException();
        }

        private void HandleCharacterListRequest(PacketReader reader)
        {
            throw new NotImplementedException();
        }

        private void HandleChannelSelect(PacketReader reader)
        {
            throw new NotImplementedException();
        }

        private void HandleWorldListRequest(PacketReader reader)
        {
            throw new NotImplementedException();
        }

        private void HandlePinAssignment(PacketReader reader)
        {
            throw new NotImplementedException();
        }

        private void HandlePinValidation(PacketReader reader)
        {
            throw new NotImplementedException();
        }

        private void HandleAuthentication(PacketReader reader)
        {
            if (this.State != AuthClientState.PreAuthentication) goto Disconnect;

            string userName;
            if (!reader.TryReadLengthString(out userName)) goto Disconnect;

            string password;
            if (!reader.TryReadLengthString(out password)) goto Disconnect;

            // TODO: more stuff to read, later.
            var authPolicy = this.server.GetAuthPolicy();
            var credentials = new SimpleCredentials(userName, password);

            IAccountSession accountSession;
            AuthenticationResult result = authPolicy.Authenticate(credentials, out accountSession);
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

            using (var builder = this.server.OpCodes.NewPacket("AuthenticationResponse"))
            {
                builder.WriteInt32((int)result);
                builder.WriteInt16(0x0000);
                this.Session.WritePacket(builder.ToByteArray());
            }
            return;

        Disconnect:
            base.Disconnect();
        }
    }
}