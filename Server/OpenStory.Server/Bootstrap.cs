using System;
using System.ComponentModel;
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
    [Localizable(true)]
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
                error = String.Format(Errors.BootstrapParseError, error);
                return null;
            }

            var nexusConnectionInfo = NexusConnectionInfo.FromParameterList(parameters, out error);
            if (error != null)
            {
                error = String.Format(Errors.BootstrapValidationError, error);
                return null;
            }

            var result = GetServiceConfiguration(nexusConnectionInfo);
            if (!CheckOperationResult(result, out error))
            {
                error = String.Format(Errors.BootstrapConnectionError, error);
                return null;
            }

            var configuration = result.Result;
            var service = provider();
            if (!service.Configure(configuration, out error))
            {
                error = String.Format(Errors.BootstrapConfigurationError, error);
                return null;
            }

            OS.Initialize().Services().Host(service).Done();

            if (!service.OpenServiceHost(out error))
            {
                error = String.Format(Errors.BootstrapHostingError, error);
                return null;
            }

            return service;
        }

        private static ServiceOperationResult<ServiceConfiguration> GetServiceConfiguration(NexusConnectionInfo info)
        {
            using (var nexus = new NexusServiceClient(info.NexusUri))
            {
                var result = nexus.GetServiceConfiguration(info.AccessToken);
                return result;
            }
        }

        private static bool CheckOperationResult(IServiceOperationResult result, out string error)
        {
            switch (result.OperationState)
            {
                case OperationState.Success:
                    error = null;
                    return true;

                case OperationState.FailedLocally:
                    error = String.Format(Errors.BootstrapCouldNotConnectToNexus, result.Error);
                    return false;

                case OperationState.Refused:
                    error = Errors.BootstrapRequestRefused;
                    return false;

                default:
                    error = String.Format(Errors.BootstrapNexusGenericError, result.Error);
                    return false;
            }
        }
    }
}
