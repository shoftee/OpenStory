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
    /// <typeparam name="TManagedService">The type for the service contract.</typeparam>
    public abstract class ManagedServiceClient<TManagedService> : DuplexClientBase<TManagedService>, IManagedService, IServiceStateChangedHandler
        where TManagedService : class, IManagedService
    {
        /// <summary>
        /// Raised after the service state changes.
        /// </summary>
        public event EventHandler<ServiceStateEventArgs> ServiceStateChanged;

        /// <summary>
        /// Initialized a new instance of <see cref="ManagedServiceClient{TGameService}"/> with the specified endpoint address.
        /// </summary>
        /// <param name="uri">The URI of the service to connect to.</param>
        protected ManagedServiceClient(Uri uri)
            : base(new InstanceContext(new ServiceStateChangedCallback()), new NetTcpBinding(SecurityMode.Transport), new EndpointAddress(uri))
        {
            var callback = (ServiceStateChangedCallback)base.InnerDuplexChannel.CallbackInstance.GetServiceInstance();
            callback.Handler = this;
        }

        #region Implementation of IManagedService

        /// <inheritdoc />
        public ServiceOperationResult Initialize()
        {
            return HandleCommunicationExceptions(() => base.Channel.Initialize());
        }

        /// <inheritdoc />
        public ServiceOperationResult Start()
        {
            return HandleCommunicationExceptions(() => base.Channel.Start());
        }

        /// <inheritdoc />
        public ServiceOperationResult Stop()
        {
            return HandleCommunicationExceptions(() => base.Channel.Stop());
        }

        /// <inheritdoc />
        public ServiceOperationResult GetServiceState()
        {
            return HandleCommunicationExceptions(() => base.Channel.GetServiceState());
        }

        private static ServiceOperationResult HandleCommunicationExceptions(Func<ServiceOperationResult> func)
        {
            try
            {
                var remote = func();
                var local = ServiceOperationResult.FromRemoteResult(remote);
                return local;
            }
            catch (CommunicationException communicationException)
            {
                return new ServiceOperationResult(communicationException);
            }
            catch (TimeoutException timeoutException)
            {
                return new ServiceOperationResult(timeoutException);
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
