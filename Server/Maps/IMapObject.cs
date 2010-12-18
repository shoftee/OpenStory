using System.Drawing;

namespace OpenMaple.Server.Maps
{
    interface IMapObject
    {
        int ObjectId { get; }
        MapObjectType Type { get; }
        Point Position { get; }

        void SendSpawnData(Client.Client client);
        void SendDestroyData(Client.Client client);
    }

    enum MapObjectType
    {
        Unknown = 0,
        Player,
        Summon,
        Npc,
        Reactor,
        HiredMerchant,
        PlayerShop,
        Monster,
        Item,
        Door,
        Mist
    }
}