using System.Net.Sockets;

namespace OpenStory.Server.Networking
{
    internal interface IDescriptorContainer
    {
        Socket Socket { get; }
        bool IsDisconnected { get; }

        void Close();
    }
}