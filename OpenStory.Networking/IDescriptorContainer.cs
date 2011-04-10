using System.Net.Sockets;

namespace OpenStory.Networking
{
    interface IDescriptorContainer
    {
        Socket Socket { get; }
        bool IsActive { get; }

        void Close();
    }
}