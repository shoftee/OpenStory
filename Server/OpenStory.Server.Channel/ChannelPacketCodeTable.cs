using OpenStory.Common;

namespace OpenStory.Server.Channel
{
    internal sealed class ChannelPacketCodeTable : PacketCodeTable
    {
        public ChannelPacketCodeTable()
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
