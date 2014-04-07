using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Some kinda magic used to please the HackShield gods.
    /// </summary>
    public sealed class HeartbeatCrypto
    {
        /// <summary>
        /// Transforms the provided heartbeat request.
        /// </summary>
        public static int TransformClientRequest(int request)
        {
            int right = request & (~0x7F);
            int middleRight = request & (~0x1F);
            int middleLeft = (request & (0x18)) ^ 0x10;
            int left = 7 - (request & 7);

            int response = (right | middleRight | middleLeft | left) - 1;
            return response;
        }
    }
}
