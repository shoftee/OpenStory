using System;
using System.ServiceModel;
using System.Threading;
using Ninject.Extensions.Logging;
using OpenStory.Services.Contracts;

namespace OpenStory.Services
{
    /// <summary>
    /// Bootstrapper.
    /// </summary>
    public sealed class Bootstrapper
    {
        private readonly IServiceHostFactory serviceFactory;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        /// <param name="serviceFactory">The service factory used to created instances for bootstrapping.</param>
        /// <param name="logger">The logger to use.</param>
        public Bootstrapper(IServiceHostFactory serviceFactory, ILogger logger)
        {
            this.serviceFactory = serviceFactory;
            this.logger = logger;
        }

        /// <summary>
        /// Starts the whole thing.
        /// </summary>
        public void Start()
        {
            ServiceHost host = null;
            try
            {
                host = this.serviceFactory.CreateServiceHost();

                this.logger.Debug("Starting service...");
                host.Open();

                this.logger.Info("Service started.");
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception exception)
            {
                this.logger.Error(exception, ServiceStrings.BootstrapGenericError);
            }
            finally
            {
                if (host != null)
                {
                    if (host.State == CommunicationState.Faulted)
                    {
                        host.Abort();
                    }
                    else
                    {
                        host.Close();
                    }

                    ((IDisposable)host).Dispose();
                }
            }
        }
    }
}
