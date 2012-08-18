using OpenStory.Common.Data;

namespace OpenStory.AuthService
{
    sealed class AuthServerPackets : OpCodeTable
    {
        public AuthServerPackets()
        {
            this.LoadOpCodes();
        }

        #region Overrides of OpCodeTable

        /// <inheritdoc select="summary"/>
        protected override void LoadOpCodesInternal()
        {
            this.AddIncoming(0x0011, "Pong");
            this.AddIncoming(0x0019, "RsaCryptoRequest");
            this.AddIncoming(0x0001, "Authenticate");
            this.AddIncoming(0x0008, "ValidatePin");
            this.AddIncoming(0x0009, "AssignPin");
            this.AddIncoming(0x000A, "WorldListRequest");
            this.AddIncoming(0x000B, "WorldListRefresh");
            this.AddIncoming(0x0005, "ChannelSelect");
            this.AddIncoming(0x0004, "CharacterListRequest");
            this.AddIncoming(0x000C, "CharacterSelect");
            this.AddIncoming(0x0012, "ErrorLog");

            this.AddOutgoing("Ping", 0x000F);
            this.AddOutgoing("RsaCrypto", 0x0016);
            this.AddOutgoing("RsaCryptoEnd", 0x0012);
            this.AddOutgoing("AuthenticationResponse", 0x0000);
            this.AddOutgoing("PinValidationResponse", 0x0006);
            this.AddOutgoing("PinAssignResponse", 0x0007);
            this.AddOutgoing("WorldInformation", 0x0008);
            this.AddOutgoing("ServerStatus", 0x0002);
            this.AddOutgoing("CharacterList", 0x0009);
            this.AddOutgoing("ServerEndpoint", 0x000A);
        }

        #endregion
    }
}
