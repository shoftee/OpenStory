using System.Net.Sockets;

namespace OpenMaple.Networking
{
    interface IDescriptorContainer
    {
        Socket Socket { get; }
        bool IsDisconnected { get; }

        void Close();
    }
}