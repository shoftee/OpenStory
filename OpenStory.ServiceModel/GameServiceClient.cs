using System;
using System.ServiceModel;

namespace OpenStory.ServiceModel
{
    /// <summary>
    /// An abstract game service client.
    /// </summary>
    /// <typeparam name="T">The type for the service contract.</typeparam>
    public abstract class GameServiceClient<T> : ClientBase<T>, IGameService
        where T : class, IGameService
    {

        /// <summary>
        /// Initializes a new instance of GameServiceClient with the specified endpoint address.
        /// </summary>
        /// <param name="uri">The endpoint URI for the service.</param>
        protected GameServiceClient(Uri uri)
            : base(ServiceHelpers.GetPipeBinding(), new EndpointAddress(uri))
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
        public virtual bool Ping()
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
