using System;
using Ninject.Extensions.Logging;
using Ninject.Syntax;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Represents a base implementation of the <see cref="IBootstrapper"/> interface.
    /// </summary>
    public abstract class BootstrapperBase : IBootstrapper
    {
        /// <summary>
        /// Gets the resolution root for the bootstrapper.
        /// </summary>
        protected IResolutionRoot ResolutionRoot { get; private set; }

        /// <summary>
        /// Gets the logger for this bootstrapper.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Initializes it all.
        /// </summary>
        protected BootstrapperBase(IResolutionRoot resolutionRoot, ILogger logger)
        {
            ResolutionRoot = resolutionRoot;
            Logger = logger;
        }

        /// <inheritdoc/>
        public void Start()
        {
            try
            {
                Logger.Info("Starting services...");
                OnStarting();
                Logger.Info("All services started.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Encountered an error while bootstrapping.");
            }
        }

        /// <summary>
        /// A hook to the middle of the public <see cref="Start()"/> method.
        /// </summary>
        /// <remarks>
        /// Implement this method for your custom bootstrapper logic.
        /// </remarks>
        protected abstract void OnStarting();
    }
}