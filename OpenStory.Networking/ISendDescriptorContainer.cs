using System.Net.Sockets;
using OpenStory.Cryptography;

namespace OpenStory.Networking
{
    interface ISendDescriptorContainer
    {
        Socket Socket { get; }
        bool IsDisconnected { get; }

        AesEncryption SendCrypto { get; }

        void Close();
    }
}