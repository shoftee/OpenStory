using System.Net.Sockets;

namespace OpenStory.Networking
{
    internal interface IDescriptorContainer
    {
        Socket Socket { get; }
        bool IsActive { get; }

        void Close();
    }
}