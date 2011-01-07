using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMaple.Threading
{
    static partial class Synchronizer
    {
        private static readonly QueueScheduler GlobalQueue = new QueueScheduler();

        public static ISynchronized<T> Synchronize<T>(T item)
            where T : class
        {
            return new Synchronized<T>(item, GlobalQueue);
        }

        public static void Execute(Action action)
        {
            GlobalQueue.EnqueueDirectly(action);
        }
    }
}
