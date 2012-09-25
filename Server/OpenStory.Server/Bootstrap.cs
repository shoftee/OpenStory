using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using OpenStory.Common.Tools;
using OpenStory.Server.Fluent;
using OpenStory.Services;
using OpenStory.Services.Clients;
using OpenStory.Services.Contracts;

namespace OpenStory.Server
{
    /// <summary>
    /// Bootstrapper.
    /// </summary>
    public static class Bootstrap
    {
        /// <summary>
        /// Bootstraps a service instance.
        /// </summary>
        /// <typeparam name="TGameService">The concrete type of the service.</typeparam>
        /// <param name="provider">A delegate which provides the boostrapper with a service object.</param>
        /// <param name="error">A varible to hold any error messages.</param>
        /// <returns>an instance of <typeparamref name="TGameService"/>, or <c>null</c> if there was an error.</returns>
        public static TGameService Service<TGameService>(Func<TGameService> provider, out string error)
            where TGameService : GameServiceBase
        {
            var parameters = ParameterList.FromEnvironment(out error);
            if (error != null)
            {
                error = "Parse error: " + error;
                return null;
            }

            var nexusConnectionInfo = NexusConnectionInfo.FromParameterList(parameters, out error);
            if (error != null)
            {
                error = "Validation error: " + error;
                return null;
            }

            ServiceConfiguration configuration;
            var result = GetServiceConfiguration(nexusConnectionInfo, out configuration);

            var success = CheckOperationResult(result, out error);
            if (!success)
            {
                error = "Nexus connection error: " + error;
                return null;
            }

            var service = provider();
            if (!service.Configure(configuration, out error))
            {
                error = "Configuration error: " + error;
                return null;
            }

            OS.Initialize().Services().Host(service).Done();

            service.OpenServiceHost(out error);
            if (error != null)
            {
                error = "Service hosting error: " + error;
                return null;
            }

            return service;
        }

        private static ServiceOperationResult GetServiceConfiguration(NexusConnectionInfo info, out ServiceConfiguration configuration)
        {
            using (var nexus = new NexusServiceClient(info.NexusUri))
            {
                ServiceOperationResult result = nexus.TryGetServiceConfiguration(info.AccessToken, out configuration);
                return result;
            }
        }

        private static bool CheckOperationResult(ServiceOperationResult result, out string error)
        {
            switch (result.OperationState)
            {
                case OperationState.Success:
                    error = null;
                    return true;

                case OperationState.FailedLocally:
                    error = String.Format("Could not connect to the Nexus service: {0}", result.Error);
                    return false;

                case OperationState.Refused:
                    error = "The Nexus service refused the request. Are you sure your token is authorized?";
                    return false;

                default:
                    error = String.Format("The Nexus service encountered a problem when processing your request: {0}", result.Error);
                    return false;

            }
        }
    }
}
