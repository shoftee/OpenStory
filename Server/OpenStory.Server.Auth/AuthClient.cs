using System;
using OpenStory.Common.Game;
using OpenStory.Common.IO;
using OpenStory.Server.Auth.Policy;
using OpenStory.Server.Processing;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Represents a client for the Authentication Server.
    /// </summary>
    internal sealed class AuthClient : ClientBase
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
        /// Gets a value indicating whether the client has authenticated.
        /// </summary>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Gets the current state of the client.
        /// </summary>
        public AuthClientState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthClient"/> class and binds it with a network session.
        /// </summary>
        /// <param name="server">The authentication server instance which is handling this client.</param>
        /// <param name="session">The network session to bind the instance to.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any of the provider parameters is <c>null</c>.
        /// </exception>
        public AuthClient(IAuthServer server, IServerSession session)
            : base(server, session)
        {
            this.LoginAttempts = 0;
            this.IsAuthenticated = false;
            this.State = AuthClientState.PreAuthentication;

            this.server = server;
        }

        protected override void ProcessPacket(PacketProcessingEventArgs args)
        {
            string label = args.Label;
            var reader = args.Reader;
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

        #region Authentication

        private void HandleAuthentication(IUnsafePacketReader reader)
        {
            if (this.State != AuthClientState.PreAuthentication)
            {
                this.Disconnect("Invalid client authentication state.");
                return;
            }

            // TODO: more stuff to read, later.
            var userName = reader.ReadLengthString();
            var password = reader.ReadLengthString();

            var credentials = new SimpleCredentials(userName, password);

            IAccountSession accountSession;
            var authPolicy = this.server.GetAuthPolicy();
            var result = authPolicy.Authenticate(credentials, out accountSession);
            if (result == AuthenticationResult.Success)
            {
                this.IsAuthenticated = true;
                this.AccountSession = accountSession;
                this.State = AuthClientState.PostAuthentication;
            }
            else if (this.LoginAttempts++ > MaxLoginAttempts)
            {
                this.Disconnect("Too many login attempts.");
                return;
            }

            byte[] packet;
            using (var builder = this.server.NewPacket("AuthenticationResponse"))
            {
                builder.WriteInt32((int)result);
                builder.WriteInt16(0x0000);

                if (this.IsAuthenticated)
                {
                    builder.WriteInt32(accountSession.AccountId);
                    builder.WriteZeroes(5);
                    builder.WriteLengthString(accountSession.AccountName);
                    builder.WriteByte(2);
                    builder.WriteByte(0);
                    builder.WriteInt64(0);
                    builder.WriteByte(0);
                    builder.WriteInt64(0);
                    builder.WriteInt32(0);
                    builder.WriteInt16(257);
                    builder.WriteInt32(0);
                    builder.WriteInt32(0);
                }

                packet = builder.ToByteArray();
            }

            this.Session.WritePacket(packet);
        }

        #endregion

        private void HandleWorldListRequest(IUnsafePacketReader reader)
        {
            throw new NotImplementedException();
        }

        private void HandleCharacterSelect(IUnsafePacketReader reader)
        {
            throw new NotImplementedException();
        }

        private void HandleCharacterListRequest(IUnsafePacketReader reader)
        {
            throw new NotImplementedException();
        }

        private void HandleChannelSelect(IUnsafePacketReader reader)
        {
            throw new NotImplementedException();
        }
        
        private void HandlePinAssignment(IUnsafePacketReader reader)
        {
            throw new NotImplementedException();
        }

        private void HandlePinValidation(IUnsafePacketReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
