using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services
{
    /// <summary>
    /// Represents a base class for game services.
    /// </summary>
    public abstract class GameServiceBase : RegisteredServiceBase, IGameService, IDisposable
    {
        private bool isDisposed;

        private Uri serviceUri;
        private ServiceConfiguration serviceConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameServiceBase"/> class.
        /// </summary>
        protected GameServiceBase()
        {
        }

        /// <summary>
        /// Gets the configuration data for this service.
        /// </summary>
        protected ServiceConfiguration ServiceConfiguration
        {
            get
            {
                this.ThrowIfDisposed();
                return this.serviceConfiguration;
            }
        }

        /// <summary>
        /// Gets the URI where this service is hosted.
        /// </summary>
        public Uri ServiceUri
        {
            get
            {
                this.ThrowIfDisposed();
                return this.serviceUri;
            }
        }

        /// <summary>
        /// Configures the game service.
        /// </summary>
        /// <param name="configuration">The configuration information.</param>
        public void Configure(ServiceConfiguration configuration)
        {
            this.ThrowIfDisposed();

            this.ConfigureInternal(configuration);

            this.OnConfiguring(configuration);

            this.serviceConfiguration = configuration;
        }

        private void ConfigureInternal(ServiceConfiguration configuration)
        {
            var uri = configuration.Get<Uri>(ServiceSettings.Uri.Key);
            if (uri == null)
            {
                throw new ServiceConfigurationException("Service endpoint URI missing from configuration.");
            }

            this.serviceUri = uri;
        }

        /// <summary>
        /// Called when the service is being configured.
        /// </summary>
        /// <remarks>
        /// When overriding this method in a derived class, please call the base implementation first.
        /// </remarks>
        /// <param name="configuration">The configuration information.</param>
        protected virtual void OnConfiguring(ServiceConfiguration configuration)
        {
        }

        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
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
                this.isDisposed = true;
            }
        }

        #endregion
    }
}