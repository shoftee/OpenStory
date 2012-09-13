using System;
using System.Collections.Concurrent;
using OpenStory.Common;
using OpenStory.Common.IO;
using OpenStory.Cryptography;
using OpenStory.Networking;
using OpenStory.Server.Fluent;

namespace OpenStory.Server
{
    internal sealed class ServerSessionWrapper : IServerSession
    {
        public event EventHandler ReadyForPush;

        public event EventHandler<PacketProcessingEventArgs> PacketProcessing;

        public event EventHandler Closing
        {
            add { this.session.Closing += value; }
            remove { this.session.Closing -= value; }
        }

        public int NetworkSessionId { get { return this.session.NetworkSessionId; } }

        private readonly ServerSession session;

        private readonly ConcurrentQueue<byte[]> packets;

        private readonly Func<ushort, string> getLabel;

        private readonly AtomicBoolean isPushing;

        public ServerSessionWrapper(ServerSession session, Func<ushort, string> getLabelCallback)
        {
            this.session = session;
            this.packets = new ConcurrentQueue<byte[]>();
            this.getLabel = getLabelCallback;

            this.session.PacketReceived += this.HandlePacketReceived;

            this.isPushing = false;
        }

        public void Start(RollingIvFactory factory, HandshakeInfo info)
        {
            this.session.Start(factory, info);
        }

        public void Close()
        {
            this.session.Close();
        }

        public void WritePacket(byte[] packet)
        {
            this.session.WritePacket(packet);
        }

        #region Pushing events

        public void Push()
        {
            if (this.PacketProcessing == null)
            {
                throw new InvalidOperationException("PacketProcessing event must have a subscriber.");
            }

            if (!this.isPushing.CompareExchange(comparand: false, newValue: true))
            {
                return;
            }

            this.StartPushing();
        }

        private void StartPushing()
        {
            byte[] segment;
            bool success = this.packets.TryDequeue(out segment);
            if (success)
            {
                while (!this.PushAsync(segment))
                {
                    if (!this.ContinuePushSynchronous(out segment))
                    {
                        break;
                    }
                }
            }
        }

        private bool PushAsync(byte[] segment)
        {
            var reader = new PacketReader(segment);
            ushort opCode;
            if (!reader.TryReadUInt16(out opCode))
            {
                LogPacketWithoutOpCode();

                this.session.Close();

                // Bad packet. Pushing completed.
                return true;
            }

            string label = this.getLabel.Invoke(opCode);
            if (label == null)
            {
                LogUnknownPacket(opCode, reader);

                // Try the next one!
                return false;
            }

            var args = new PacketProcessingEventArgs(label, reader);
            var result = this.BeginInvoke(args);
            return result.CompletedSynchronously;

        }

        private bool ContinuePushSynchronous(out byte[] segment)
        {
            bool hasElement = this.packets.TryDequeue(out segment);

            if (!hasElement)
            {
                this.isPushing.Exchange(newValue: false);
            }

            return hasElement;
        }

        private IAsyncResult BeginInvoke(PacketProcessingEventArgs args)
        {
            var handler = this.PacketProcessing;
            var result = handler.BeginInvoke(this, args, this.ContinuePushAsynchronous, null);
            return result;
        }

        private void ContinuePushAsynchronous(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                this.StartPushing();
            }
        }

        #endregion

        #region Packet arrival handling

        private void HandlePacketReceived(object sender, PacketReceivedEventArgs e)
        {
            var bytes = e.Reader.ReadFully();

            this.packets.Enqueue(bytes);

            this.OnReadyForPush();
        }

        private void OnReadyForPush()
        {
            var handler = this.ReadyForPush;
            if (handler != null) handler.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Logging

        private static void LogPacketWithoutOpCode()
        {
            OS.Log().Info("A packet without an op-code was received.");
        }

        private static void LogUnknownPacket(ushort opCode, PacketReader reader)
        {
            const string Format = "Unknown Op Code 0x{0:4X} - {1}";
            OS.Log().Warning(Format, opCode, reader.ReadFully().ToHex());
        }

        #endregion
    }
}