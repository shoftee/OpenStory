using System.ServiceModel;

namespace OpenStory.Server.Common
{
    /// <summary>
    /// Provides helper methods for dealing with the long-name-ness of WCF.
    /// </summary>
    public static class ServiceHelpers
    {
        /// <summary>
        /// God damn can't they just call it IPC like before?
        /// </summary>
        /// <returns>Fucking long name.</returns>
        public static NetNamedPipeBinding GetBinding()
        {
            return new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
        }
    }
}