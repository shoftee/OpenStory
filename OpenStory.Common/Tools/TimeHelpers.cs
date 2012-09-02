using System;

namespace OpenStory.Common.Tools
{
    /// <summary>
    /// Provides static methods for time-related operations.
    /// </summary>
    public static class TimeHelpers
    {
        private static readonly DateTimeOffset Epoch = new DateTimeOffset(new DateTime(1970, 1, 1));

        /// <summary>
        /// Gets <see cref="DateTimeOffset.UtcNow"/> as Epoch time.
        /// </summary>
        /// <remarks>
        /// This method is equivalent to Java's System.currentTimeMillis().
        /// </remarks>
        /// <returns></returns>
        public static long GetMillisecondsSinceEpoch()
        {
            return (long)(Epoch - DateTimeOffset.UtcNow).TotalMilliseconds;
        }

        /// <summary>
        /// Gets <see cref="DateTimeOffset.UtcNow"/> as FileTime.
        /// </summary>
        /// <returns></returns>
        public static long GetUtcNowAsFileTime()
        {
            return DateTimeOffset.UtcNow.ToFileTime();
        }

        /// <summary>
        /// Returns a FileTime timestamp as a DateTimeOffset.
        /// </summary>
        /// <param name="fileTime">The FileTime timestamp to convert.</param>
        /// <returns>a <see cref="DateTimeOffset"/> object equivalent to the given timestamp.</returns>
        public static DateTimeOffset GetFileTimeAsUtc(long fileTime)
        {
            return DateTimeOffset.FromFileTime(fileTime);
        }
    }
}
