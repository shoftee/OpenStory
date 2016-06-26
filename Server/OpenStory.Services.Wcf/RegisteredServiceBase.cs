using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Wcf
{
    /// <summary>
    /// Represents a base class for registered services.
    /// </summary>
    public abstract class RegisteredServiceBase<TRegisteredService> : IRegisteredService
        where TRegisteredService : IRegisteredService
    {
        private ServiceState serviceState;

        private Task initializeTask;
        private Task startTask;
        private Task stopTask;

        private readonly List<IServiceStateChanged> initializeSubscribers;
        private readonly List<IServiceStateChanged> startSubscribers;
        private readonly List<IServiceStateChanged> stopSubscribers;

        /// <summary>
        /// Gets the underlying service.
        /// </summary>
        public TRegisteredService Service { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredServiceBase{TRegisteredService}"/> class.
        /// </summary>
        protected RegisteredServiceBase(TRegisteredService service)
        {
            this.Service = service;

            this.serviceState = ServiceState.NotInitialized;

            this.initializeSubscribers = new List<IServiceStateChanged>();
            this.startSubscribers = new List<IServiceStateChanged>();
            this.stopSubscribers = new List<IServiceStateChanged>();
        }

        #region IRegisteredService members

        /// <inheritdoc />
        public void Initialize(OsServiceConfiguration serviceConfiguration)
        {
            SubscribeForStates(
                this.initializeSubscribers,
                this.serviceState,
                ServiceState.NotInitialized,
                ServiceState.Initializing);

            if (this.serviceState != ServiceState.NotInitialized)
            {
                return;
            }

            this.HandleStateChange(this.serviceState, ServiceState.Initializing);

            var task = this.GetInitializeTask(serviceConfiguration);
            if (task.Status == TaskStatus.Created)
            {
                task.Start();
            }
        }

        /// <inheritdoc />
        public void Start()
        {
            SubscribeForStates(
                this.startSubscribers,
                this.serviceState,
                ServiceState.Ready,
                ServiceState.NotInitialized,
                ServiceState.Initializing,
                ServiceState.Starting);

            if (this.serviceState != ServiceState.Ready)
            {
                return;
            }

            this.HandleStateChange(this.serviceState, ServiceState.Starting);

            var task = this.GetStartTask();
            if (task.Status == TaskStatus.Created)
            {
                task.Start();
            }
        }

        /// <inheritdoc />
        public void Stop()
        {
            SubscribeForStates(
                this.stopSubscribers,
                this.serviceState,
                ServiceState.Running,
                ServiceState.Stopping);

            if (this.serviceState != ServiceState.Running)
            {
                return;
            }

            this.HandleStateChange(this.serviceState, ServiceState.Stopping);

            var task = this.GetStopTask();
            if (task.Status == TaskStatus.Created)
            {
                task.Start();
            }
        }

        /// <inheritdoc />
        public void Ping()
        {
        }

        #region Private methods

        private void CompleteInitialization(Task task)
        {
            this.HandleStateChange(this.serviceState, ServiceState.Ready);
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
                Notify(list, enterState, exitState, clear);
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

        private static void Notify(List<IServiceStateChanged> subscribers, ServiceState oldState, ServiceState newState, bool clear)
        {
            lock (subscribers)
            {
                var unusableSubscribers = new List<IServiceStateChanged>();
                foreach (var subscriber in subscribers)
                {
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    var communcationObject = (ICommunicationObject)subscriber;
                    if (communcationObject.State == CommunicationState.Opened)
                    {
                        subscriber.OnServiceStateChanged(oldState, newState);
                    }
                    else
                    {
                        unusableSubscribers.Add(subscriber);
                    }
                }

                if (clear)
                {
                    subscribers.Clear();
                }
                else if (unusableSubscribers.Count > 0)
                {
                    // If we're not gonna clear all of 'em out, clear out just the unusable ones.
                    foreach (var badSubscriber in unusableSubscribers)
                    {
                        subscribers.Remove(badSubscriber);
                    }
                }
            }
        }

        private Task GetInitializeTask(OsServiceConfiguration serviceConfiguration)
        {
            return SetNewTaskIfNecessary(ref this.initializeTask, () => this.OnInitializing(serviceConfiguration), this.CompleteInitialization);
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
        protected virtual void OnInitializing(OsServiceConfiguration serviceConfiguration)
        {
            this.Service.Initialize(serviceConfiguration);
        }

        /// <summary>
        /// Executed when the service enters the <see cref="ServiceState.Starting"/> state.
        /// </summary>
        protected virtual void OnStarting()
        {
            this.Service.Start();
        }

        /// <summary>
        /// Executed when the service enters the <see cref="ServiceState.Stopping"/> state.
        /// </summary>
        protected virtual void OnStopping()
        {
            this.Service.Stop();
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
