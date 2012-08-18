using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WorldServiceClient : GameServiceClient<IWorldService>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        public WorldServiceClient(Uri uri)
            : base(uri)
        {
        }
    }
}