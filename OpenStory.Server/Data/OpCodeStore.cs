using System;
using System.Collections.Generic;
using OpenStory.Common.IO;

namespace OpenStory.Server.Data
{
    internal class OpCodeStore
    {
        private static Dictionary<string, short> opCodes;

        private static void Initialize()
        {
            opCodes = new Dictionary<string, short>(256);
            // TODO: Load from DB.
        }

        public static short GetByName(string name)
        {
            short value;
            if (!opCodes.TryGetValue(name, out value))
            {
                throw new InvalidOperationException("There is no value mapped to this op code name.");
            }
            return value;
        }
    }

    static class Extensions
    {
        public static void WriteOpCode(this PacketBuilder packetBuilder, string opCodeName)
        {
            packetBuilder.WriteInt16(OpCodeStore.GetByName(opCodeName));
        }
    }
}