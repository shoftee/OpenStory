using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Data
{
    class OpCodeStore
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
}
