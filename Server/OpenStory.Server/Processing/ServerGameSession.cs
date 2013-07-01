using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using OpenStory.Common;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Networking;
using OpenStory.Server.Fluent;
using CommonExceptions = OpenStory.Framework.Model.Common.Exceptions;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Handles asynchronous packet processing.
    /// </summary>
    [Localizable(true)]
    internal sealed class ServerGameSession : IServerSession
    {
        /// <summary>
        /// The event is raised when a new packet arrives and the instance is not in the process of pushing data.
        /// </summary>
        public event EventHandler ReadyForPush;

        /// <inheritdoc />
        public event EventHandler<PacketProcessingEventArgs> PacketProcessing;

        /// <inheritdoc />
        public event EventHandler Closing
        {
            add { this.networkSession.Closing += value; }
            remove { this.networkSession.Closing -= value; }
        }

        /// <inheritdoc />
        public int NetworkSessionId
        {
            get { return this.networkSession.NetworkSessionId; }
        }

        private readonly ServerNetworkSession networkSession;

        private readonly ConcurrentQueue<byte[]> packets;

        private readonly Func<ushort, string> getLabel;

        private readonly AtomicBoolean isPushing;

        public ServerGameSession(ServerNetworkSession networkSession, Func<ushort, string> getLabelCallback)
        {
            this.networkSession = networkSession;
            this.packets = new ConcurrentQueue<byte[]>();
            this.getLabel = getLabelCallback;

            this.networkSession.PacketReceived += this.HandlePacketReceived;

            this.isPushing = false;
        }

        /// <inheritdoc />
        public void Start(RollingIvFactory factory, HandshakeInfo info)
        {
            this.networkSession.Start(factory, info);
        }

        /// <inheritdoc />
        public void Close()
        {
            this.networkSession.Close();
        }

        /// <inheritdoc />
        public void WritePacket(byte[] packet)
        {
            this.networkSession.WritePacket(packet);
        }

        #region Pushing events

        public void Push()
        {
            if (this.PacketProcessing == null)
            {
                throw new InvalidOperationException(CommonExceptions.PacketProcessingEventHasNoSubscriber);
            }

            // CompareExchange returns the original value, hence:
            // => true means we were already pushing, don't start a second one.
            // => false means we were not pushing and we just toggled it, so we should start now.
            if (!this.isPushing.CompareExchange(comparand: false, newValue: true))
            {
                this.StartPushing();
            }
        }

        private void StartPushing()
        {
            byte[] packet;
            bool success = this.packets.TryDequeue(out packet);

            // This can only be false when we are called by the asynchronous continuation,
            // which doesn't have a good way of communicating its results to us,
            // so it can't dequeue the next packet and pass it. So we do it here instead <3
            if (success)
            {
                // PushAsync will return false if we completed synchronously
                // and we would therefore like to try pushing another one right away.
                while (!this.PushAsync(packet))
                {
                    // ContinuePushSynchronous will return false if there are no more packets to push.
                    if (!this.ContinuePushSynchronous(out packet))
                    {
                        break;
                    }
                }
            }
            else
            {
                this.isPushing.Exchange(newValue: false);
            }
        }

        private bool PushAsync(byte[] packet)
        {
            var reader = new PacketReader(packet);
            ushort opCode;
            if (!reader.TryReadUInt16(out opCode))
            {
                LogPacketWithoutOpCode();

                this.networkSession.Close();

                // Bad packet. We kill the session and stop pushing.
                return true;
            }

            string label = this.getLabel.Invoke(opCode);
            if (label == null)
            {
                // Bad SERVER. Most likely. We don't know this packet.
                LogUnknownPacket(opCode, reader);

                // Take the blame and try the next one! :<
                return false;
            }

            // Invoke event asynchronously.
            var args = new PacketProcessingEventArgs(label, reader);
            var result = this.BeginInvoke(args);

            // If we completed synchronously, we'll take another one right away.
            // Otherwise, leave it to the asynchronous continuation.
            return result.CompletedSynchronously;
        }

        private IAsyncResult BeginInvoke(PacketProcessingEventArgs args)
        {
            var handler = this.PacketProcessing;
            var result = handler.BeginInvoke(this, args, this.ContinuePushAsynchronous, null);
            return result;
        }

        private bool ContinuePushSynchronous(out byte[] packet)
        {
            bool hasNext = this.packets.TryDequeue(out packet);

            if (!hasNext)
            {
                this.isPushing.Exchange(newValue: false);
            }

            return hasNext;
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

            if (!this.isPushing.Value)
            {
                this.OnReadyForPush();
            }
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