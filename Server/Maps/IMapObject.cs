using System.Drawing;
using OpenMaple.Constants;

namespace OpenMaple.Server.Maps
{
    interface IMapObject
    {
        int ObjectId { get; }
        MapObjectType Type { get; }
        Point Position { get; }

        void SendSpawnData(ChannelClient client);
        void SendDestroyData(ChannelClient client);
    }
}