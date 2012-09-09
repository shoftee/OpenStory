using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using OpenStory.Services.Contracts;

namespace OpenStory.Services
{
    /// <summary>
    /// Represents a base class for game services.
    /// </summary>
    public abstract class GameServiceBase : IGameService
    {
        private ServiceState serviceState;

        private Task initializeTask;
        private Task startTask;
        private Task stopTask;

        private readonly List<IServiceStateChanged> initializeSubscribers;
        private readonly List<IServiceStateChanged> startSubscribers;
        private readonly List<IServiceStateChanged> stopSubscribers;

        protected GameServiceBase()
        {
            this.serviceState = ServiceState.NotInitialized;

            this.initializeSubscribers = new List<IServiceStateChanged>();
            this.startSubscribers = new List<IServiceStateChanged>();
            this.stopSubscribers = new List<IServiceStateChanged>();
        }

        /// <inheritdoc />
        public ServiceState Initialize()
        {
            bool transition = false;
            switch (this.serviceState)
            {
                case ServiceState.NotInitialized:
                    SubscribeForCallback(this.initializeSubscribers);
                    transition = true;
                    break;
                case ServiceState.Initializing:
                    SubscribeForCallback(this.initializeSubscribers);
                    break;
            }

            if (transition)
            {
                this.HandleStateChange(this.serviceState, ServiceState.Initializing);

                var task = this.GetInitializeTask();
                if (task.Status == TaskStatus.Created)
                {
                    task.Start();
                }
            }

            return this.serviceState;
        }

        /// <inheritdoc />
        public ServiceState Start()
        {
            bool transition = false;
            switch (this.serviceState)
            {
                case ServiceState.Ready:
                    SubscribeForCallback(this.startSubscribers);
                    transition = true;
                    break;
                case ServiceState.NotInitialized:
                case ServiceState.Initializing:
                case ServiceState.Starting:
                    SubscribeForCallback(this.startSubscribers);
                    break;
            }

            if (transition)
            {
                this.HandleStateChange(this.serviceState, ServiceState.Starting);

                var task = this.GetStartTask();
                if (task.Status == TaskStatus.Created)
                {
                    task.Start();
                }
            }

            return this.serviceState;
        }

        /// <inheritdoc />
        public ServiceState Stop()
        {
            bool transition = false;
            switch (this.serviceState)
            {
                case ServiceState.Running:
                    SubscribeForCallback(this.stopSubscribers);
                    transition = true;
                    break;
                case ServiceState.Stopping:
                    SubscribeForCallback(this.stopSubscribers);
                    break;
            }

            if (transition)
            {
                this.HandleStateChange(this.serviceState, ServiceState.Stopping);

                var task = GetStopTask();
                if (task.Status == TaskStatus.Created)
                {
                    task.Start();
                }
            }

            return this.serviceState;
        }

        private Task GetInitializeTask()
        {
            return SetNewTaskIfNecessary(ref this.initializeTask, this.OnInitializing, this.CompleteInitialization);
        }

        private Task GetStartTask()
        {
            return SetNewTaskIfNecessary(ref this.startTask, this.OnStarting, this.CompleteStart);
        }

        private Task GetStopTask()
        {
            return SetNewTaskIfNecessary(ref this.stopTask, this.OnStopping, this.CompleteStop);
        }

        private static Task SetNewTaskIfNecessary(ref Task task, Action action, Action<Task> completion)
        {
            bool createNew = task == null
                || task.Status == TaskStatus.Faulted
                || task.Status == TaskStatus.Canceled
                || task.Status == TaskStatus.RanToCompletion;

            if (createNew)
            {
                task = new Task(action).ContinueWith(completion);
            }

            return task;
        }

        /// <inheritdoc />
        public ServiceState GetServiceState()
        {
            return this.serviceState;
        }

        protected virtual void OnInitializing()
        {
        }

        protected virtual void OnStarting()
        {
        }

        protected virtual void OnStopping()
        {
        }

        #region Private methods

        private void CompleteInitialization(Task task)
        {
            this.HandleStateChange(this.serviceState, ServiceState.Running);
        }

        private void CompleteStart(Task task)
        {
            this.HandleStateChange(this.serviceState, ServiceState.Running);
        }

        private void CompleteStop(Task task)
        {
            this.HandleStateChange(this.serviceState, ServiceState.Ready);
        }

        private void HandleStateChange(ServiceState enterState, ServiceState exitState)
        {
            if (enterState == exitState)
            {
                return;
            }

            var list = new List<IServiceStateChanged>(0);
            var clear = false;
            switch (exitState)
            {
                case ServiceState.Initializing:
                    list = this.initializeSubscribers;
                    break;
                case ServiceState.Ready:
                    if (enterState == ServiceState.Initializing)
                    {
                        list = this.initializeSubscribers;
                    }
                    else if (enterState == ServiceState.Stopping)
                    {
                        list = this.stopSubscribers;
                    }
                    clear = true;
                    break;
                case ServiceState.Starting:
                    list = this.startSubscribers;
                    break;
                case ServiceState.Running:
                    list = this.startSubscribers;
                    clear = true;
                    break;
                case ServiceState.Stopping:
                    list = this.stopSubscribers;
                    break;
            }

            this.serviceState = exitState;
            if (list.Count > 0)
            {
                Notify(list, exitState, clear);
            }
        }

        private static void SubscribeForCallback(List<IServiceStateChanged> subscribers)
        {
            var channel = OperationContext.Current.GetCallbackChannel<IServiceStateChanged>();
            if (!subscribers.Contains(channel))
            {
                subscribers.Add(channel);
            }
        }

        private static void Notify(List<IServiceStateChanged> subscribers, ServiceState state, bool clear)
        {
            foreach (var subscriber in subscribers)
            {
                subscriber.OnServiceStateChanged(state);
            }

            if (clear)
            {
                subscribers.Clear();
            }
        }

        #endregion
    }
}
