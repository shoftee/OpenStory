using System.Net.Sockets;
using OpenStory.Cryptography;

namespace OpenStory.Networking
{
    interface IDescriptorContainer
    {
        Socket Socket { get; }
        bool IsActive { get; }

        void Close();
    }
}