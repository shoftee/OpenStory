using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPlayerFacetFactory
    {
        /// <summary>
        /// 
        /// </summary>
        TPlayerFacet CreateFacet<TPlayerFacet>(CharacterKey characterKey)
            where TPlayerFacet : IPlayerFacet;
    }
}
