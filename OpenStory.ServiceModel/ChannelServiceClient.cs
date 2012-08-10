using System;

namespace OpenStory.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ChannelServiceClient : GameServiceClient<IChannelService>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        public ChannelServiceClient(Uri uri)
            : base(uri)
        {
        }
    }
}