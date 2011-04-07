using System;
using System.Collections.Generic;
using OpenStory.Common.IO;

namespace OpenStory.Server.Data
{
    internal class SendOpCodeStore
    {
        private static Dictionary<string, ushort> opCodeValues;

        private static void Initialize()
        {
            opCodeValues = new Dictionary<string, ushort>(256);
            // TODO: Load from DB.
        }

        public static ushort GetByName(string name)
        {
            ushort value;
            if (!opCodeValues.TryGetValue(name, out value))
            {
                throw new InvalidOperationException("There is no value mapped to this op code name.");
            }
            return value;
        }
    }

    internal class ReceiveOpCodeStore
    {
        private static Dictionary<ushort, string> opCodeNames;

        private static void Initialize()
        {
            opCodeNames = new Dictionary<ushort, string>(256);
            // TODO: Load from DB.
        }

        public static string GetByValue(ushort value)
        {
            string name;
            if (!opCodeNames.TryGetValue(value, out name))
            {
                throw new InvalidOperationException("There is no name mapped to this op code value.");
            }
            return name;
        }
    }
}