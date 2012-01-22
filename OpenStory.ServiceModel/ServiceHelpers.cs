using System.ServiceModel;

namespace OpenStory.ServiceModel
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
    }
}