using OpenStory.Common.Game;
using OpenStory.Common.IO;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.Auth
{
    partial class AuthClient
    {
        private byte[] AuthResponse(AuthenticationResult result, Account account)
        {
            using (var builder = this.PacketFactory.CreatePacket("Authentication"))
            {
                builder.WriteInt32(result);

                builder.WriteInt16(0x0000);

                if (result == AuthenticationResult.Success)
                {
                    builder.WriteInt32(account.AccountId);

                    if (this.State == AuthClientState.SetGender)
                    {
                        builder.WriteByte(AuthOperationType.GenderSelect);
                    }
                    else if (this.State == AuthClientState.SetPin)
                    {
                        builder.WriteByte(AuthOperationType.PinSelect);
                    }
                    else
                    {
                        builder.WriteByte(account.Gender);
                    }

                    // Enables commands like /c, /ch, /m, /h (etc.), but disables trading
                    builder.WriteBoolean(account.IsGameMaster);

                    // Seems like 0x80 is a "MWLB" account - I doubt it... it disables attacking and allows GM fly
                    // 0x40, 0x20 (and probably 0x10, 0x8, 0x4, 0x2, and 0x1) don't appear to confer any particular benefits, restrictions, or functionality
                    // (Although I didn't test client GM commands or anything of the sort)
                    builder.WriteByte(account.IsGameMaster ? 0x80 : 0x00);

                    builder.WriteBoolean(account.IsGameMaster || account.IsGameMasterHelper);

                    builder.WriteLengthString(account.UserName);

                    builder.WriteByte(2);

                    // TODO: quiet ban support?
                    //builder.WriteByte(account.QuietBanReason);
                    builder.WriteByte(0);
                    //builder.WriteTimestamp(account.QuietBanTime);
                    builder.WriteInt64(0);

                    // TODO: Creation time
                    builder.WriteTimestamp(account.CreationTime);

                    builder.WriteInt32(0);
                }

                return builder.ToByteArray();
            }
        }

        private byte[] SetPinResponse()
        {
            return this.PinResponse(PinResponseType.SetPin);
        }

        private byte[] InvalidPinResponse()
        {
            return this.PinResponse(PinResponseType.InvalidPin);
        }

        private byte[] PinAcceptedResponse()
        {
            return this.PinResponse(PinResponseType.PinAccepted);
        }

        private byte[] CheckPinResponse()
        {
            return this.PinResponse(PinResponseType.CheckPin);
        }

        private byte[] PinResponse(PinResponseType result)
        {
            using (var builder = PacketFactory.CreatePacket("PinResponse"))
            {
                builder.WriteByte(result);

                return builder.ToByteArray();
            }
        }

        private byte[] WorldListResponse()
        {
            var worlds = this.nexus.GetWorlds();

            using (var builder = this.PacketFactory.CreatePacket("WorldListRequest"))
            {
                foreach (var world in worlds)
                {
                    builder.WriteWorld(world);
                }

                return builder.ToByteArray();
            }
        }
    }
}
