using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
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