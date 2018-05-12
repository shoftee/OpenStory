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

        private readonly IAuthenticator _authenticator;
        private readonly IAuthToNexusRequestHandler _nexus;

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
            _authenticator = authenticator;
            _nexus = nexus;

            LoginAttempts = 0;
            State = AuthClientState.NotLoggedIn;
        }

        /// <inheritdoc/>
        protected override void ProcessPacket(PacketProcessingEventArgs args)
        {
            var reader = args.Reader;
            switch (args.Label)
            {
                case "Authenticate":
                    HandleAuthentication(reader);
                    break;

                case "ValidatePin":
                    HandlePinValidation(reader);
                    break;

                case "AssignPin":
                    HandlePinAssignment(reader);
                    break;

                case "WorldListRequest":
                case "WorldListRefresh":
                    HandleWorldListRequest(reader);
                    break;

                case "ChannelSelect":
                    HandleChannelSelect(reader);
                    break;

                case "CharacterListRequest":
                    HandleCharacterListRequest(reader);
                    break;

                case "CharacterSelect":
                    HandleCharacterSelect(reader);
                    break;
            }
        }

        private void HandleAuthentication(IUnsafePacketReader reader)
        {
            if (State != AuthClientState.NotLoggedIn)
            {
                Disconnect("Invalid client authentication state.");
                return;
            }

            // TODO: more stuff to read, later.
            IAccountSession accountSession;
            Account account;
            var result = _authenticator.Authenticate(reader, out accountSession, out account);
            if (result == AuthenticationResult.Success)
            {
                AccountSession = accountSession;
                Account = account;

                if (!account.Gender.HasValue)
                {
                    State = AuthClientState.SetGender;
                }
                else
                {
                    // TODO: cover cases where further authentication is not required and this should be set to LoggedIn.
                    if (account.AccountPin == null)
                    {
                        State = AuthClientState.SetPin;
                    }
                    else
                    {
                        State = AuthClientState.AskPin;
                    }
                }
            }
            else if (LoginAttempts++ > MaxLoginAttempts)
            {
                Disconnect("Too many login attempts.");
                return;
            }

            ServerSession.WritePacket(AuthResponse(result, account));
            ServerSession.WritePacket(CheckPinResponse());
        }

        private void HandleWorldListRequest(IUnsafePacketReader reader)
        {
            ServerSession.WritePacket(WorldListResponse());
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
            // TODO: Configuring the server to not require PINs.

            var action = reader.ReadByte<PinRequestType>();

            reader.Skip(5);

            switch (action)
            {
                case PinRequestType.PinNotSet:
                    State = AuthClientState.AskPin;

                    ServerSession.WritePacket(SetPinResponse());
                    break;

                case PinRequestType.CheckPin:
                    if (CheckPin(reader))
                    {
                        State = AuthClientState.LoggedIn;

                        ServerSession.WritePacket(PinAcceptedResponse());
                    }
                    else
                    {
                        ServerSession.WritePacket(InvalidPinResponse());
                    }
                    break;

                case PinRequestType.AssignPin:
                    if (CheckPin(reader))
                    {
                        State = AuthClientState.LoggedIn;

                        ServerSession.WritePacket(SetPinResponse());
                    }
                    else
                    {
                        ServerSession.WritePacket(InvalidPinResponse());
                    }
                    break;
            }
        }
    }
}
