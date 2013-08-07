using System;
using System.Collections.Generic;

namespace OpenStory.Synchronization
{
    /// <summary>
    /// Represents a scheduler which executes tasks at given points in time.
    /// </summary>
    internal partial class TimeScheduler
    {
        #region Nested type: Timeline

        /// <summary>
        /// Represents a one-way chronological list of <see cref="ScheduledTask">ScheduledTask</see> objects.
        /// </summary>
        private sealed class Timeline
        {
            private TimelineNode front;

            /// <summary>
            /// Initializes a new empty instance of the Timeline class.
            /// </summary>
            public Timeline()
            {
                this.front = null;
            }

            /// <summary>
            /// Inserts the given task into the Timeline, after the first task which is strictly chronologically before it.
            /// </summary>
            /// <param name="task">The task to insert.</param>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="task"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException">Thrown if <paramref name="task"/> is already cancelled.</exception>
            public void Insert(ScheduledTask task)
            {
                if (task == null)
                {
                    throw new ArgumentNullException("task");
                }

                if (task.Cancellation.IsCancellationRequested)
                {
                    throw new InvalidOperationException("Cannot add a task that is already cancelled.");
                }

                if (this.front == null || this.front.Task.ScheduledTime > task.ScheduledTime)
                {
                    this.AddFront(new TimelineNode(task));
                    return;
                }

                TimelineNode current = this.front;
                TimelineNode next = this.front.Next;
                while (next != null && next.Task.ScheduledTime <= task.ScheduledTime)
                {
                    current = next;
                    next = current.Next;
                }

                AddAfter(current, new TimelineNode(task, current));
            }

            /// <summary>
            /// Polls all <see cref="ScheduledTask">ScheduledTask</see> objects which are due for exectution, and removes them from the front of the timeline.
            /// </summary>
            /// <returns>a list of all scheduled tasks which are due for exectuion.</returns>
            public IEnumerable<ScheduledTask> GetAllDue()
            {
                var tasks = new List<ScheduledTask>();

                DateTime now = DateTime.Now;
                TimelineNode node = this.front;
                while (node != null && node.Task.ScheduledTime <= now)
                {
                    tasks.Add(node.Task);
                    node = node.Next;
                }

                this.front = node;
                return tasks;
            }

            private void AddFront(TimelineNode node)
            {
                node.Next = this.front;
                this.front = node;
            }

            private static void AddAfter(TimelineNode node, TimelineNode newNode)
            {
                if (node.Next != null)
                {
                    newNode.Next = node.Next;
                }

                node.Next = newNode;
            }

            #region Nested type: TimelineNode

            /// <summary>
            /// Represents a single-link node for a Timleine.
            /// </summary>
            private class TimelineNode
            {
                /// <summary>
                /// The task for this TimelineNode.
                /// </summary>
                public ScheduledTask Task { get; private set; }

                /// <summary>
                /// A reference to the next TimelineNode in the Timeline.
                /// </summary>
                public TimelineNode Next { get; set; }
                
                /// <summary>
                /// Initializes a new instance of the TimelineNode class, with the given scheduled task.
                /// </summary>
                /// <param name="task">The task for this TimelineNode.</param>
                /// <param name="next">Optional. The TimelineNode to use as a next node reference. The default value is <see langword="null"/>.</param>
                public TimelineNode(ScheduledTask task, TimelineNode next = null)
                {
                    this.Task = task;
                    this.Next = next;
                }
            }

            #endregion
        }

        #endregion
    }
}
