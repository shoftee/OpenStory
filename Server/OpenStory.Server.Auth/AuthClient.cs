using System;
using Ninject.Extensions.Logging;
using OpenStory.Common.Game;
using OpenStory.Common.IO;
using OpenStory.Framework.Contracts;
using OpenStory.Framework.Model.Common;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Represents a client for the Authentication Server.
    /// </summary>
    public sealed partial class AuthClient : ClientBase
    {
        /// <summary>
        /// Denotes the maximum number of allowed failed login attempts before the client is disconnected.
        /// </summary>
        private const int MaxLoginAttempts = 3;

        private readonly IAuthenticator authenticator;
        private readonly IAuthToNexusRequestHandler nexus;

        /// <summary>
        /// Gets the number of failed login attempts for this client.
        /// </summary>
        public int LoginAttempts { get; private set; }

        /// <summary>
        /// Gets the current state of the client.
        /// </summary>
        public AuthClientState State { get; private set; }

        /// <inheritdoc/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthClient"/> class and binds it with a network session.
        /// </summary>
        /// <param name="authenticator">The <see cref="IAuthenticator"/> to use for authenticating the user.</param>
        /// <param name="nexus">The <see cref="IAuthToNexusRequestHandler"/> to query for... world stuff.</param>
        /// <param name="serverSession"><inheritdoc/></param>
        /// <param name="packetFactory"><inheritdoc/></param>
        /// <param name="logger"><inheritdoc/></param>
        public AuthClient(IAuthenticator authenticator, IAuthToNexusRequestHandler nexus, IServerSession serverSession, IPacketFactory packetFactory, ILogger logger)
            : base(serverSession, packetFactory, logger)
        {
            this.authenticator = authenticator;
            this.nexus = nexus;

            this.LoginAttempts = 0;
            this.State = AuthClientState.NotLoggedIn;
        }

        /// <inheritdoc/>
        protected override void ProcessPacket(PacketProcessingEventArgs args)
        {
            var reader = args.Reader;
            switch (args.Label)
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

        private void HandleAuthentication(IUnsafePacketReader reader)
        {
            if (this.State != AuthClientState.NotLoggedIn)
            {
                this.Disconnect("Invalid client authentication state.");
                return;
            }

            // TODO: more stuff to read, later.
            IAccountSession accountSession;
            Account account;
            var result = this.authenticator.Authenticate(reader, out accountSession, out account);
            if (result == AuthenticationResult.Success)
            {
                this.AccountSession = accountSession;
                this.Account = account;

                if (!account.Gender.HasValue)
                {
                    this.State = AuthClientState.SetGender;
                }
                else
                {
                    // TODO: cover cases where further authentication is not required and this should be set to LoggedIn.
                    if (account.AccountPin == null)
                    {
                        this.State = AuthClientState.SetPin;
                    }
                    else
                    {
                        this.State = AuthClientState.AskPin;
                    }
                }
            }
            else if (this.LoginAttempts++ > MaxLoginAttempts)
            {
                this.Disconnect("Too many login attempts.");
                return;
            }

            this.ServerSession.WritePacket(this.AuthResponse(result, account));
        }

        private void HandleWorldListRequest(IUnsafePacketReader reader)
        {
            this.ServerSession.WritePacket(this.WorldListResponse());
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
