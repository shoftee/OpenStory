using System;
using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services
{
    /// <summary>
    /// Provides helper methods for game services.
    /// </summary>
    public static class ServiceHelpers
    {
        /// <summary>
        /// Creates a new <see cref="ServiceHost"/> around the specified service object, on the specified <see cref="Uri"/>, and opens it.
        /// </summary>
        /// <param name="service">The singleton service object instance.</param>
        /// <param name="uri">The URI to host the service on.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any of the parameters is <c>null</c>.
        /// </exception>
        /// <returns>the created <see cref="ServiceHost"/> object.</returns>
        public static ServiceHost OpenServiceHost(object service, Uri uri)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            var host = new ServiceHost(service, uri);
            host.Open();
            return host;
        }

        /// <summary>
        /// Something obvious.
        /// </summary>
        public static bool ProcessGetConfigurationResult(ServiceOperationResult result, out string error)
        {
            switch (result.OperationState)
            {
                case OperationState.Success:
                    error = null;
                    return true;

                case OperationState.FailedLocally:
                    error = String.Format("Could not connect to the Nexus service: {0}", result.Error);
                    return false;

                case OperationState.FailedRemotely:
                    error = String.Format("The Nexus service encountered a problem when processing your request: {0}", result.Error);
                    return false;

                case OperationState.Refused:
                    error = "The Nexus service refused the request. Are you sure your token is authorized?";
                    return false;

                default:
                    error = "The Nexus service response was weird, brah.";
                    return false;
            }
        }
    }
}
