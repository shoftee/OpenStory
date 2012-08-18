using System;
using System.ServiceModel;

namespace OpenStory.ServiceModel
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

        /// <summary>
        /// A base implementation of the method, calls the proxy method.
        /// </summary>
        /// <remarks>
        /// When overriding, do not call the base implementation.
        /// </remarks>
        public virtual void Start()
        {
            try
            {
                base.Channel.Start();
            }
            catch (FaultException<InvalidOperationException> ex)
            {
                throw ex.Detail;
            }
        }

        /// <summary>
        /// A base implementation of the method, calls the proxy method.
        /// </summary>
        /// <remarks>
        /// When overriding, do not call the base implementation.
        /// </remarks>
        public virtual void Stop()
        {
            try
            {
                base.Channel.Stop();
            }
            catch (FaultException<InvalidOperationException> ex)
            {
                throw ex.Detail;
            }
        }

        /// <summary>
        /// A base implementation of the method, calls the proxy method.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the proxy method call returns <c>true</c>; 
        /// <c>false</c> if the proxy method call returns <c>false</c>, if the call throws an <see cref="EndpointNotFoundException"/> or <see cref="TimeoutException"/>.
        /// </returns>
        public bool Ping()
        {
            try
            {
                return base.Channel.Ping();
            }
            catch (EndpointNotFoundException)
            {
                return false;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        #endregion
    }
}
