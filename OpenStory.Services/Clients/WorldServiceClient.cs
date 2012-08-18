using System;

namespace OpenStory.Services.Contracts
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