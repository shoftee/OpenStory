using System;

namespace OpenStory.Common.Tools
{
    /// <summary>
    /// Provides static methods for time-related operations.
    /// </summary>
    public static class Time
    {
        private static readonly DateTimeOffset Epoch = new DateTimeOffset(new DateTime(1970, 1, 1));

        /// <summary>
        /// Gets <see cref="DateTimeOffset.UtcNow"/> as Epoch time.
        /// </summary>
        /// <remarks>
        /// This method should be equivalent to Java's System.currentTimeMillis().
        /// </remarks>
        /// <returns>the number of milliseconds since 1st January 1970.</returns>
        public static long GetMillisecondsSinceEpoch()
        {
            return (long)(Epoch - DateTimeOffset.UtcNow).TotalMilliseconds;
        }

        /// <summary>
        /// Gets <see cref="DateTimeOffset.UtcNow"/> as FileTime.
        /// </summary>
        /// <returns>the number of 100ns intervals since 1st January 1600.</returns>
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
