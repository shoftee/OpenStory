namespace OpenStory.Synchronization
{
    /// <summary>
    /// Provides methods for scheduling operations on <see cref="IRunnable"/> objects..
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        /// Schedules an <see cref="IRunnable" /> to be processed at a later time.
        /// </summary>
        /// <param name="runnable">The ISchedulable to schedule for processing.</param>
        void Schedule(IRunnable runnable);
    }
}