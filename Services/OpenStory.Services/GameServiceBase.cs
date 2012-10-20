using System;
using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services
{
    /// <summary>
    /// Represents a base class for game services.
    /// </summary>
    public abstract class GameServiceBase : RegisteredServiceBase, IGameService, IDisposable
    {
        private bool isDisposed;
        private ServiceHost serviceHost;
        private Uri serviceUri;

        /// <summary>
        /// Gets the URI where this service is hosted.
        /// </summary>
        protected Uri ServiceUri
        {
            get
            {
                ThrowIfDisposed();

                return this.serviceUri;
            }
        }

        /// <summary>
        /// Configures the game service.
        /// </summary>
        /// <param name="configuration">The configuration information.</param>
        /// <param name="error">A variable to hold a human-readable error message.</param>
        /// <returns><c>true</c> if configuration was successful; otherwise, <c>false</c>.</returns>
        public bool Configure(ServiceConfiguration configuration, out string error)
        {
            ThrowIfDisposed();

            var uri = configuration.Get<Uri>("ServiceUri");
            if (uri == null)
            {
                error = "Service endpoint URI missing from configuration.";
                return false;
            }

            if (!this.OnConfiguring(configuration, out error))
            {
                return false;
            }

            this.serviceUri = uri;
            return true;
        }

        /// <summary>
        /// Called when the services is configuring itself.
        /// </summary>
        /// <remarks>
        /// When overriding this method in a derived class, please call the base implementation first 
        /// and return <c>false</c> if it returns <c>false</c>.
        /// </remarks>
        /// <param name="configuration">The configuration information.</param>
        /// <param name="error">A variable to hold a human-readable error message.</param>
        /// <returns><c>true</c> if configuration was successful; otherwise, <c>false</c>.</returns>
        protected virtual bool OnConfiguring(ServiceConfiguration configuration, out string error)
        {
            error = null;
            return true;
        }

        /// <summary>
        /// Attempts to open a service host for this game service.
        /// </summary>
        /// <param name="error">A variable to hold a human-readable error message.</param>
        public void OpenServiceHost(out string error)
        {
            ThrowIfDisposed();

            if (this.serviceHost != null)
            {
                error = "There was a previous attempt to host this service instance.";
                return;
            }

            bool success = true;
            ServiceHost host = null;
            try
            {
                host = new ServiceHost(this, this.ServiceUri);
                host.Open();

                this.serviceHost = host;
                error = null;
            }
            catch (Exception exception)
            {
                error = exception.ToString();
                success = false;
            }
            finally
            {
                if (!success && host != null)
                {
                    host.Close();
                }
            }
        }

        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("serviceHost");
            }
        }

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called when the object is to be disposed.
        /// </summary>
        /// <param name="disposing">Whether this method is called during disposal or finalization.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
            {
                var host = this.serviceHost;
                if (host != null)
                {
                    host.Close();
                }

                this.isDisposed = true;
            }
        }

        #endregion
    }
}