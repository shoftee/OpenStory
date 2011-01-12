using System.Collections.Generic;

namespace OpenStory.Server.Login
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