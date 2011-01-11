using System.Collections.Generic;

namespace OpenMaple.Server.Login
{
    public interface IWorld
    {
        int Id { get; }
        string Name { get; }
        WorldStatus Status { get; }
        int ChannelCount { get; }

        IEnumerable<IChannel> Channels { get; }
    }
}