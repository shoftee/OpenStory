using System.Drawing;

namespace OpenStory.Server.Channel.Maps
{
    internal interface IMapObject
    {
        int ObjectId { get; }
        MapObjectType Type { get; }
        Point Position { get; }
    }
}
