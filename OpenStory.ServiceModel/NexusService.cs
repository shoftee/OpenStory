using System;
using System.Collections.Generic;

namespace OpenStory.ServiceModel
{
    /// <summary>
    /// Represents an abstract services that manages other game services.
    /// </summary>
    /// <typeparam name="TGameService">The type of the game services that are managed.</typeparam>
    public abstract class NexusService<TGameService> : INexusService<TGameService>
        where TGameService : class, IGameService
    {
        #region Implementation of INexusService

        /// <inheritdoc />
        public abstract IEnumerable<TGameService> Services { get; }

        /// <inheritdoc />
        public abstract string RegisterService(Uri serviceUri);

        #endregion

        #region Implementation of IGameService

        /// <inheritdoc />
        public void Start()
        {
            throw new InvalidOperationException("The AccountService is always running.");
        }

        /// <inheritdoc />
        public void Stop()
        {
            throw new InvalidOperationException("The AccountService is always running.");
        }

        /// <inheritdoc />
        public bool Ping()
        {
            return true;
        }

        #endregion
    }
}