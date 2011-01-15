using System.Net.Sockets;
using OpenStory.Cryptography;

namespace OpenStory.Server.Networking
{
    interface IReceiveDescriptorContainer
    {
        Socket Socket { get; }
        bool IsDisconnected { get; }

        AesEncryption ReceiveCrypto { get; }

        void Close();
    }
}