using System;
using System.ServiceModel;

namespace OpenStory.Services
{
    /// <summary>
    /// Provides helper methods for game services.
    /// </summary>
    public static class ServiceHelpers
    {
        /// <summary>
        /// Provides a TCP binding.
        /// </summary>
        /// <returns>
        /// a new instance of <see cref="NetTcpBinding"/> with <see cref="SecurityMode"/> set to <see cref="SecurityMode.Transport"/>.
        /// </returns>
        public static NetTcpBinding GetTcpBinding()
        {
            var binding = new NetTcpBinding(SecurityMode.Transport);
            return binding;
        }

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
    }
}
