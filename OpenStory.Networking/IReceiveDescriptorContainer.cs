using System.Net.Sockets;
using OpenStory.Cryptography;

namespace OpenStory.Networking
{
    interface IReceiveDescriptorContainer
    {
        Socket Socket { get; }
        bool IsDisconnected { get; }

        AesEncryption ReceiveCrypto { get; }

        void Close();
    }
}