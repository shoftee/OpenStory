using System.ServiceModel;
using System.ServiceModel.Description;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Registry
{
    /// <summary>
    /// Represents a client for a generic game service.
    /// </summary>
    public sealed class RegisteredServiceClient : DuplexClientBase<IRegisteredService>, IRegisteredService
    {
        /// <summary>
        /// Initialized a new instance of <see cref="RegisteredServiceClient"/> with the specified endpoint address.
        /// </summary>
        /// <param name="endpoint">The service endpoint information.</param>
        /// <param name="stateChangedHandler">The handler for the service state changes.</param>
        public RegisteredServiceClient(ServiceEndpoint endpoint, IServiceStateChanged stateChangedHandler)
            : base(stateChangedHandler, endpoint)
        {
        }

        #region Implementation of IRegisteredService

        /// <inheritdoc />
        public ServiceOperationResult Initialize()
        {
            var result = ServiceOperationResult.Of(() => base.Channel.Initialize());
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult Start()
        {
            var result = ServiceOperationResult.Of(() => base.Channel.Start());
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult Stop()
        {
            var result = ServiceOperationResult.Of(() => base.Channel.Stop());
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult Ping()
        {
            var result = ServiceOperationResult.Of(() => base.Channel.Ping());
            return result;
        }

        #endregion
    }
}
