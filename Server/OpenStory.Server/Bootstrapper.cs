using System;
using System.Threading;

using OpenStory.Services.Contracts;

using Ninject.Extensions.Logging;

namespace OpenStory.Server
{
    /// <summary>
    /// Bootstrapper.
    /// </summary>
    public sealed class Bootstrapper
    {
        private readonly IGenericServiceFactory serviceFactory;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        /// <param name="serviceFactory">The service factory used to created instances for bootstrapping.</param>
        /// <param name="logger">The logger to use.</param>
        public Bootstrapper(IGenericServiceFactory serviceFactory, ILogger logger)
        {
            this.serviceFactory = serviceFactory;
            this.logger = logger;
        }

        /// <summary>
        /// Starts the whole thing.
        /// </summary>
        public void Start()
        {
            try
            {
                using (var host = serviceFactory.CreateServiceHost())
                {
                    host.Open();
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, ServerStrings.BootstrapGenericError);
            }
        }
    }
}
