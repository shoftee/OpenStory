using System;
using System.Collections.Generic;
using System.Threading;

namespace OpenMaple.Threading
{
    partial class TimeScheduler
    {
        /// <summary>
        /// Represents a cancellable scheduled task.
        /// </summary>
        class ScheduledTask
        {
            private Action action;
            private CancellationToken token;

            /// <summary>
            /// The time at which this task is scheduled to execute.
            /// </summary>
            public DateTime ScheduledTime { get; private set; }

            /// <summary>
            /// Gets the timestamp for when the task was cancelled.
            /// </summary>
            public DateTime? TimeCancelled { get; private set; }

            /// <summary>
            /// Initializes a new ScheduledTask, with the given action, at the given scheduled time, and with the given <see cref="CancellationToken">CancellationToken</see>.
            /// </summary>
            /// <param name="action">The action to schedule for execution.</param>
            /// <param name="scheduledTime">The time to execute the action at.</param>
            /// <param name="token">A CancellationToken used to cancel the action.</param>
            /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="action"/>is null.</exception>
            public ScheduledTask(Action action, DateTime scheduledTime, CancellationToken token)
            {
                if (action == null) throw new ArgumentNullException("action");
                this.action = action;
                this.ScheduledTime = scheduledTime;
                this.token = token;
                this.TimeCancelled = null;
                this.token.Register(() =>
                {
                    this.TimeCancelled = DateTime.Now;
                });
            }
        }

        /// <summary>
        /// Represents a one-way chronological list of <see cref="ScheduledTask">ScheduledTask</see> objects.
        /// </summary>
        class Timeline
        {
            /// <summary>
            /// Represents a single-link node for a Timleine.
            /// </summary>
            class TimelineNode
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
                /// <param name="next">Optional. The TimelineNode to use as a next node reference. The default value is null.</param>
                public TimelineNode(ScheduledTask task, TimelineNode next = null)
                {
                    this.Task = task;
                    this.Next = next;
                }
            }

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
            /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="task"/> is null.</exception>
            /// <exception cref="InvalidOperationException">The exception is thrown if <paramref name="task"/> is already cancelled.</exception>
            public void Insert(ScheduledTask task)
            {
                if (task == null) throw new ArgumentNullException("task");
                if (task.TimeCancelled.HasValue)
                {
                    throw new InvalidOperationException("Cannot add a task that is already cancelled.");
                }

                if (this.front == null || this.front.Task.ScheduledTime > task.ScheduledTime)
                {
                    this.AddFront(new TimelineNode(task));
                    return;
                }

                TimelineNode current = front;
                TimelineNode next = front.Next;
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
            /// <returns>A list of all scheduled tasks which are due for exectuion.</returns>
            public IEnumerable<ScheduledTask> GetAllDue()
            {
                List<ScheduledTask> tasks = new List<ScheduledTask>();

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
                if (node == null) throw new ArgumentNullException("node");
                if (newNode == null) throw new ArgumentNullException("newNode");

                if (node.Next != null)
                {
                    newNode.Next = node.Next;
                }
                node.Next = newNode;
            }
        }
    }
}
