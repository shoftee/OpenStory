using OpenStory.Common;

namespace OpenStory.Services.Simple
{
    /// <summary>
    /// Lovingly stolen from Vana.
    /// </summary>
    class AuthPacketCodeTableV75 : PacketCodeTable
    {
        public AuthPacketCodeTableV75()
        {
            this.LoadPacketCodes();
        }

        protected override void LoadPacketCodesInternal()
        {
            this.AddIncoming();
            this.AddOutgoing();
        }

        private void AddIncoming()
        {
            // CMSG_AUTHENTICATION 0x01
            //this.AddIncoming(0x01, "Authenticate");

            // CMSG_WORLD_LIST_REFRESH 0x04 // Click back after select channel
            //this.AddIncoming(0x04, "WorldListRefresh");

            // CMSG_PLAYER_LIST 0x05
            // CMSG_WORLD_STATUS 0x06
            // CMSG_ACCOUNT_GENDER 0x08
            // CMSG_PIN 0x09
            // CMSG_REGISTER_PIN 0x0a
            // CMSG_WORLD_LIST 0x0b
            // CMSG_PLAYER_GLOBAL_LIST 0x0d
            // CMSG_PLAYER_GLOBAL_LIST_CHANNEL_CONNECT 0x0e
            // CMSG_REQUEST_NAME_CHANGE 0x10
            // CMSG_REQUEST_CHARACTER_TRANSFER 0x12
            // CMSG_CHANNEL_CONNECT 0x13
            // CMSG_PLAYER_LOAD 0x14
            // CMSG_PLAYER_NAME_CHECK 0x15
            // CMSG_PLAYER_CREATE 0x16
            // CMSG_PLAYER_DELETE 0x18
            // CMSG_PONG 0x19
            this.AddIncoming(0x19, "Pong");

            // CMSG_CLIENT_ERROR 0x1a
            this.AddIncoming(0x1a, "ClientError");

            // CMSG_LOGIN_RETURN 0x1d
        }

        private void AddOutgoing()
        {
            // SMSG_AUTHENTICATION 0x00
            //this.AddOutgoing("Authentication", 0x00);

            // SMSG_WORLD_STATUS 0x03
            //this.AddOutgoing("WorldStatus", 0x03);

            // SMSG_ACCOUNT_GENDER_DONE 0x04
            //this.AddOutgoing("AccountGenderDone", 0x04);

            // SMSG_MESSAGE_TRY_AGAIN 0x05 // Shows a popup with "Try again!" :P
            //this.AddOutgoing("MessageTryAgain", 0x05);

            // SMSG_PIN 0x06
            //this.AddOutgoing("Pin", 0x06);

            // SMSG_PIN_ASSIGNED 0x07
            //this.AddOutgoing("PinAssigned", 0x07);

            // SMSG_PLAYER_GLOBAL_LIST 0x08
            //this.AddOutgoing("PlayerGlobalList", 0x08);

            // SMSG_WORLD_LIST 0x0a
            //this.AddOutgoing("WorldList", 0x0a);

            // SMSG_PLAYER_LIST 0x0b
            //this.AddOutgoing("PlayerList", 0x0b);

            // SMSG_CHANNEL_CONNECT 0x0c
            //this.AddOutgoing("ChannelConnect", 0x0c);

            // SMSG_PLAYER_NAME_CHECK 0x0d
            //this.AddOutgoing("PlayerNameCheck", 0x0d);

            // SMSG_PLAYER_CREATE 0x0e
            //this.AddOutgoing("PlayerCreate", 0x0e);

            // SMSG_PLAYER_DELETE 0x0f
            //this.AddOutgoing("PlayerDelete", 0x0f);

            // SMSG_CHANNEL_CHANGE 0x10
            //this.AddOutgoing("ChannelChange", 0x10);

            // SMSG_PING 0x11
            this.AddOutgoing("Ping", 0x11);

            // SMSG_CHANNEL_SELECT 0x14
            //this.AddOutgoing("ChannelSelect", 0x014);
        }
    }
}
