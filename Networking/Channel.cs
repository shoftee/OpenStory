using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Networking
{
    class Channel
    {
        public byte Id { get; private set; }
        public byte WorldId { get; private set; }
        public string Name { get; private set; }

        public int ChannelLoad { get; set; }

    }
}
