using OpenStory.Common.Game;
using OpenStory.Common.IO;

namespace OpenStory.Server.Channel.Maps
{
    internal sealed class FieldLimits : IntFlags
    {
        private const int Count = 18;

        public bool this[FieldLimitLabel label]
        {
            get { return this[(int)label]; }
            set { this[(int)label] = value; }
        }

        public FieldLimits()
            : base(Count)
        {
        }

        public FieldLimits(int flags)
            : base(Count)
        {
            for (int i = 0; i < Count; i++)
            {
                var offset = (1 << i);
                var value = flags & offset;
                this[i] = value != 0;
            }
        }

        public FieldLimits(FieldLimits other)
            : base(other)
        {
        }
    }
}