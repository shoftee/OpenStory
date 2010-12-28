using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Tools
{
    static class DateUtils
    {
        private static readonly DateTimeOffset Epoch = new DateTimeOffset(new DateTime(1970, 1, 1), TimeSpan.Zero);
        public static long GetMillisecondsSinceEpoch()
        {
            var now = DateTimeOffset.UtcNow;
            return (long) (Epoch - now).TotalMilliseconds;
        }

        public static long GetUtcNowAsFileTime()
        {
            return DateTimeOffset.UtcNow.ToFileTime();
        }
    }
}
