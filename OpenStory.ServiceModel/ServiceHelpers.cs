using System.ServiceModel;

namespace OpenStory.ServiceModel
{
    /// <summary>
    /// Provides helper methods for game services.
    /// </summary>
    public static class ServiceHelpers
    {
        /// <summary>
        /// Provides a named pipe binding.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="NetNamedPipeBinding"/>
        /// with <see cref="NetNamedPipeSecurityMode">security mode</see>
        /// set to <see cref="NetNamedPipeSecurityMode.Transport"/>.
        /// </returns>
        private static NetNamedPipeBinding GetPipeBinding()
        {
            return new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
        }

        /// <summary>
        /// Provides a tcp binding.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="NetTcpBinding"/> with
        /// <see cref="SecurityMode"/> set to <see cref="SecurityMode.Transport"/>.
        /// </returns>
        public static NetTcpBinding GetTcpBinding()
        {
            var binding = new NetTcpBinding(SecurityMode.Transport);
            return binding;
        }
    }
}