using OpenStory.Services.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Represents a generic game server.
    /// </summary>
    public abstract class GameServerBase : IRegisteredService
    {
        /// <inheritdoc />
        public void Initialize(OsServiceConfiguration serviceConfiguration)
        {
            OnInitializing(serviceConfiguration);
        }

        /// <inheritdoc />
        public void Start()
        {
            OnStarting();
        }

        /// <inheritdoc />
        public void Stop()
        {
            OnStopping();
        }

        /// <inheritdoc />
        public void Ping()
        {
        }

        /// <summary>
        /// Executed when the server is being initialized.
        /// </summary>
        protected virtual void OnInitializing(OsServiceConfiguration serviceConfiguration)
        {
        }

        /// <summary>
        /// Executed when the server is being started.
        /// </summary>
        protected virtual void OnStarting()
        {
        }

        /// <summary>
        /// Executed when the server is being stopped.
        /// </summary>
        protected virtual void OnStopping()
        {
        }
    }
}