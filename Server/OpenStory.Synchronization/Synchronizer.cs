using System;

namespace OpenStory.Synchronization
{
    public static partial class Synchronizer
    {
        private static readonly QueueScheduler GlobalQueue = new QueueScheduler();

        /// <summary>
        /// Creates a synchronization wrapper around an object.
        /// </summary>
        /// <typeparam name="T">The type of the object to wrap.</typeparam>
        /// <param name="obj">The object to wrap.</param>
        /// <returns>a <see cref="ISynchronized{T}"/> wrapper around <paramref name="obj"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is <c>null</c>.</exception>
        public static ISynchronized<T> Synchronize<T>(T obj)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            return new Synchronized<T>(obj, GlobalQueue);
        }

        /// <summary>
        /// Directly schedules an action for execution.
        /// </summary>
        /// <remarks>
        /// This method may cause synchronization problems if <paramref name="action"/> accesses dependent objects.
        /// </remarks>
        /// <param name="action">The action to schedule for execution.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is <c>null</c>.</exception>
        public static void ScheduleAction(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            GlobalQueue.ScheduleAction(action);
        }
    }
}
