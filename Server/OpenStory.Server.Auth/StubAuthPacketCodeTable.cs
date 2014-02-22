using OpenStory.Common;

namespace OpenStory.Server.Auth
{
    internal sealed class StubAuthPacketCodeTable : PacketCodeTable
    {
        public StubAuthPacketCodeTable()
        {
            this.LoadPacketCodes();
        }

        #region Overrides of PacketCodeTable

        /// <inheritdoc select="summary"/>
        protected override void LoadPacketCodesInternal()
        {
        }

        #endregion
    }
}
