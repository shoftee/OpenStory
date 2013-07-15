using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using OpenStory.Services.Contracts;

namespace OpenStory.Services
{
    /// <summary>
    /// Represents a base class for registered services.
    /// </summary>
    public abstract class RegisteredServiceBase : IRegisteredService
    {
        private ServiceState serviceState;

        private Task initializeTask;
        private Task startTask;
        private Task stopTask;

        private readonly List<IServiceStateChanged> initializeSubscribers;
        private readonly List<IServiceStateChanged> startSubscribers;
        private readonly List<IServiceStateChanged> stopSubscribers;

        /// <summary>
        /// Initializes a new instance of <see cref="RegisteredServiceBase"/>.
        /// </summary>
        protected RegisteredServiceBase()
        {
            this.serviceState = ServiceState.NotInitialized;

            this.initializeSubscribers = new List<IServiceStateChanged>();
            this.startSubscribers = new List<IServiceStateChanged>();
            this.stopSubscribers = new List<IServiceStateChanged>();
        }

        #region IRegisteredService members

        /// <inheritdoc />
        public ServiceOperationResult Initialize()
        {
            SubscribeForStates(
                this.initializeSubscribers, 
                this.serviceState,
                ServiceState.NotInitialized,
                ServiceState.Initializing);

            if (this.serviceState == ServiceState.NotInitialized)
            {
                this.HandleStateChange(this.serviceState, ServiceState.Initializing);

                var task = this.GetInitializeTask();
                if (task.Status == TaskStatus.Created)
                {
                    task.Start();
                }
            }

            return new ServiceOperationResult(this.serviceState);
        }

        /// <inheritdoc />
        public ServiceOperationResult Start()
        {
            SubscribeForStates(
                this.startSubscribers, 
                this.serviceState,
                ServiceState.Ready,
                ServiceState.NotInitialized,
                ServiceState.Initializing,
                ServiceState.Starting);

            if (this.serviceState == ServiceState.Ready)
            {
                this.HandleStateChange(this.serviceState, ServiceState.Starting);

                var task = this.GetStartTask();
                if (task.Status == TaskStatus.Created)
                {
                    task.Start();
                }
            }

            return new ServiceOperationResult(this.serviceState);
        }

        /// <inheritdoc />
        public ServiceOperationResult Stop()
        {
            SubscribeForStates(
                this.stopSubscribers, 
                this.serviceState,
                ServiceState.Running,
                ServiceState.Stopping);

            if (this.serviceState == ServiceState.Running)
            {
                this.HandleStateChange(this.serviceState, ServiceState.Stopping);

                var task = this.GetStopTask();
                if (task.Status == TaskStatus.Created)
                {
                    task.Start();
                }
            }

            return new ServiceOperationResult(this.serviceState);
        }

        /// <inheritdoc />
        public ServiceOperationResult Ping()
        {
            return new ServiceOperationResult(this.serviceState);
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
            var channel = GetCallbackChannel();
            if (!subscribers.Contains(channel))
            {
                subscribers.Add(channel);
            }
        }

        private static IServiceStateChanged GetCallbackChannel()
        {
            return OperationContext.Current.GetCallbackChannel<IServiceStateChanged>();
        }

        private static void Notify(List<IServiceStateChanged> subscribers, ServiceState state, bool clear)
        {
            lock (subscribers)
            {
                var badSubscribers = new List<IServiceStateChanged>();
                foreach (var subscriber in subscribers)
                {
// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable PossibleInvalidCastException
                    var communcationObject = (ICommunicationObject)subscriber;
// ReSharper restore PossibleInvalidCastException
// ReSharper restore SuspiciousTypeConversion.Global
                    if (communcationObject.State == CommunicationState.Opened)
                    {
                        subscriber.OnServiceStateChanged(state);
                    }
                    else
                    {
                        // Keep a list of unusable subscribers.
                        badSubscribers.Add(subscriber);
                    }
                }

                if (clear)
                {
                    subscribers.Clear();
                }
                else if (badSubscribers.Count > 0)
                {
                    // If we're not gonna clear all of 'em out, clear out just the bad ones.
                    foreach (var badSubscriber in badSubscribers)
                    {
                        subscribers.Remove(badSubscriber);
                    }
                }
            }
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

        #endregion

        #endregion

        /// <summary>
        /// Executed when the service enters the <see cref="ServiceState.Initializing"/> state.
        /// </summary>
        protected virtual void OnInitializing()
        {
        }

        /// <summary>
        /// Executed when the service enters the <see cref="ServiceState.Starting"/> state.
        /// </summary>
        protected virtual void OnStarting()
        {
        }

        /// <summary>
        /// Executed when the service enters the <see cref="ServiceState.Stopping"/> state.
        /// </summary>
        protected virtual void OnStopping()
        {
        }

        private static void SubscribeForStates(List<IServiceStateChanged> subscribers, ServiceState currentState, params ServiceState[] states)
        {
            if (states.Any(state => state == currentState))
            {
                SubscribeForCallback(subscribers);
            }
        }
    }
}
