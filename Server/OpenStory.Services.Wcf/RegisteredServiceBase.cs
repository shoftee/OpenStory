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
        private ServiceState _serviceState;

        private Task _initializeTask;
        private Task _startTask;
        private Task _stopTask;

        private readonly List<IServiceStateChanged> _initializeSubscribers;
        private readonly List<IServiceStateChanged> _startSubscribers;
        private readonly List<IServiceStateChanged> _stopSubscribers;

        /// <summary>
        /// Gets the underlying service.
        /// </summary>
        public TRegisteredService Service { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredServiceBase{TRegisteredService}"/> class.
        /// </summary>
        protected RegisteredServiceBase(TRegisteredService service)
        {
            Service = service;

            _serviceState = ServiceState.NotInitialized;

            _initializeSubscribers = new List<IServiceStateChanged>();
            _startSubscribers = new List<IServiceStateChanged>();
            _stopSubscribers = new List<IServiceStateChanged>();
        }

        #region IRegisteredService members

        /// <inheritdoc />
        public void Initialize(OsServiceConfiguration serviceConfiguration)
        {
            SubscribeForStates(
                _initializeSubscribers,
                _serviceState,
                ServiceState.NotInitialized,
                ServiceState.Initializing);

            if (_serviceState != ServiceState.NotInitialized)
            {
                return;
            }

            HandleStateChange(_serviceState, ServiceState.Initializing);

            var task = GetInitializeTask(serviceConfiguration);
            if (task.Status == TaskStatus.Created)
            {
                task.Start();
            }
        }

        /// <inheritdoc />
        public void Start()
        {
            SubscribeForStates(
                _startSubscribers,
                _serviceState,
                ServiceState.Ready,
                ServiceState.NotInitialized,
                ServiceState.Initializing,
                ServiceState.Starting);

            if (_serviceState != ServiceState.Ready)
            {
                return;
            }

            HandleStateChange(_serviceState, ServiceState.Starting);

            var task = GetStartTask();
            if (task.Status == TaskStatus.Created)
            {
                task.Start();
            }
        }

        /// <inheritdoc />
        public void Stop()
        {
            SubscribeForStates(
                _stopSubscribers,
                _serviceState,
                ServiceState.Running,
                ServiceState.Stopping);

            if (_serviceState != ServiceState.Running)
            {
                return;
            }

            HandleStateChange(_serviceState, ServiceState.Stopping);

            var task = GetStopTask();
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
            HandleStateChange(_serviceState, ServiceState.Ready);
        }

        private void CompleteStart(Task task)
        {
            HandleStateChange(_serviceState, ServiceState.Running);
        }

        private void CompleteStop(Task task)
        {
            HandleStateChange(_serviceState, ServiceState.Ready);
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
                    list = _initializeSubscribers;
                    break;

                case ServiceState.Ready:
                    if (enterState == ServiceState.Initializing)
                    {
                        list = _initializeSubscribers;
                    }
                    else if (enterState == ServiceState.Stopping)
                    {
                        list = _stopSubscribers;
                    }

                    clear = true;
                    break;

                case ServiceState.Starting:
                    list = _startSubscribers;
                    break;

                case ServiceState.Running:
                    list = _startSubscribers;
                    clear = true;
                    break;

                case ServiceState.Stopping:
                    list = _stopSubscribers;
                    break;
            }

            _serviceState = exitState;
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
            return SetNewTaskIfNecessary(ref _initializeTask, () => OnInitializing(serviceConfiguration), CompleteInitialization);
        }

        private Task GetStartTask()
        {
            return SetNewTaskIfNecessary(ref _startTask, OnStarting, CompleteStart);
        }

        private Task GetStopTask()
        {
            return SetNewTaskIfNecessary(ref _stopTask, OnStopping, CompleteStop);
        }

        private static Task SetNewTaskIfNecessary(ref Task task, Action action, Action<Task> completion)
        {
            bool createNew =
                task == null
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
            Service.Initialize(serviceConfiguration);
        }

        /// <summary>
        /// Executed when the service enters the <see cref="ServiceState.Starting"/> state.
        /// </summary>
        protected virtual void OnStarting()
        {
            Service.Start();
        }

        /// <summary>
        /// Executed when the service enters the <see cref="ServiceState.Stopping"/> state.
        /// </summary>
        protected virtual void OnStopping()
        {
            Service.Stop();
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
