using System.Net.Sockets;
using OpenMaple.Cryptography;

namespace OpenMaple.Networking
{
    interface INetworkSession
    {
        AesEncryption SendCrypto { get; }
        AesEncryption ReceiveCrypto { get; }

        string RemoteAddress { get; }

        void Write(IPacket packet);

        void Open(Socket clientSocket);
        void Close();
    }
}