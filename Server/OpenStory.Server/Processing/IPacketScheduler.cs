using OpenStory.Framework.Contracts;

namespace OpenStory.Server.Processing
{
    internal interface IPacketScheduler
    {
        void Register(IServerSession session);
    }
}