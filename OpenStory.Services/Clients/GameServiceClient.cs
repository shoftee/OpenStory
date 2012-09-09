using System;
using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a game service client.
    /// </summary>
    /// <remarks>
    /// This class is abstract.
    /// </remarks>
    /// <typeparam name="TGameService">The type for the service contract.</typeparam>
    public abstract class GameServiceClient<TGameService> : ClientBase<TGameService>, IGameService
        where TGameService : class, IGameService
    {
        /// <summary>
        /// Initializes a new instance of <see cref="GameServiceClient{TGameService}"/> with the specified endpoint address.
        /// </summary>
        /// <param name="uri">The endpoint URI for the service.</param>
        protected GameServiceClient(Uri uri)
            : base(ServiceHelpers.GetTcpBinding(), new EndpointAddress(uri))
        {
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
    }
}
