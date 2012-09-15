using System;
using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a client for a generic game service.
    /// </summary>
    /// <remarks>
    /// This class is abstract.
    /// </remarks>
    /// <typeparam name="TGameService">The type for the service contract.</typeparam>
    public abstract class GameServiceClient<TGameService> : DuplexClientBase<TGameService>, IGameService, IServiceStateChangedHandler
        where TGameService : class, IGameService
    {
        /// <summary>
        /// Raised after the service state changes.
        /// </summary>
        public event EventHandler<ServiceStateEventArgs> ServiceStateChanged;

        /// <summary>
        /// Initialized a new instance of <see cref="GameServiceClient{TGameService}"/> with the specified endpoint address.
        /// </summary>
        /// <param name="uri">The URI of the service to connect to.</param>
        protected GameServiceClient(Uri uri)
            : base(new InstanceContext(new ServiceStateChangedCallback()), ServiceHelpers.GetTcpBinding(), new EndpointAddress(uri))
        {
            var callback = (ServiceStateChangedCallback)base.InnerDuplexChannel.CallbackInstance.GetServiceInstance();
            callback.Handler = this;
        }

        #region Implementation of IGameService

        /// <inheritdoc />
        public ServiceState Initialize()
        {
            return HandleCommunicationExceptions(() => base.Channel.Initialize());
        }

        /// <inheritdoc />
        public ServiceState Start()
        {
            return HandleCommunicationExceptions(() => base.Channel.Start());
        }

        /// <inheritdoc />
        public ServiceState Stop()
        {
            return HandleCommunicationExceptions(() => base.Channel.Stop());
        }

        /// <inheritdoc />
        public ServiceState GetServiceState()
        {
            return HandleCommunicationExceptions(() => base.Channel.GetServiceState());
        }

        private static ServiceState HandleCommunicationExceptions(Func<ServiceState> func)
        {
            try
            {
                return func();
            }
            catch (EndpointNotFoundException)
            {
                return ServiceState.Unknown;
            }
            catch (TimeoutException)
            {
                return ServiceState.Unknown;
            }
        }

        #endregion

        #region Implementation of IServiceStateChangedHandler

        void IServiceStateChangedHandler.OnServiceStateChanged(ServiceState newState)
        {
            var args = new ServiceStateEventArgs(newState);
            var handler = this.ServiceStateChanged;
            if (handler != null)
            {
                handler.Invoke(this, args);
            }
        }

        #endregion

        private sealed class ServiceStateChangedCallback : IServiceStateChanged
        {
            public IServiceStateChangedHandler Handler { private get; set; }

            #region Implementation of IServiceStateChanged

            public void OnServiceStateChanged(ServiceState newState)
            {
                var handler = this.Handler;
                if (handler != null)
                {
                    handler.OnServiceStateChanged(newState);
                }
            }

            #endregion
        }

    }
}
