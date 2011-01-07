using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenMaple.Threading
{
    class QueueScheduler : IScheduler
    {
        private AtomicBoolean isWorking;

        private ConcurrentQueue<ISchedulable> objectOrder;

        private ConcurrentQueue<Action> executionQueue;

        public QueueScheduler()
        {
            this.isWorking = new AtomicBoolean(false);
            this.objectOrder = new ConcurrentQueue<ISchedulable>();
            this.executionQueue = new ConcurrentQueue<Action>();
        }

        public void EnqueueObject(ISchedulable schedulable)
        {
            objectOrder.Enqueue(schedulable);
            this.ExecutePending();
        }

        public void EnqueueDirectly(Action action)
        {
            executionQueue.Enqueue(action);
            this.ExecutePending();
        }

        private void ExecutePending()
        {
            ISchedulable scheduled;
            Action action;
            while (objectOrder.TryDequeue(out scheduled))
            {
                executionQueue.Enqueue(scheduled.GetPendingAction());
            }

            if (this.isWorking.CompareExchange(comparand: false, newValue: true)) return;

            while (executionQueue.TryDequeue(out action))
            {
                Task.Factory.StartNew(action, TaskCreationOptions.PreferFairness);
            }
            this.isWorking.Exchange(false);
        }
    }

    interface IScheduler
    {
        void EnqueueObject(ISchedulable schedulable);
    }

    interface ISchedulable
    {
        Action GetPendingAction();
    }
}
