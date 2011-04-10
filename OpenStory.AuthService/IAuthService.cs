using System;
using System.ServiceModel;

namespace OpenStory.AuthService
{
    /// <summary>
    /// Provides methods for accessing and managing the Authentication Service.
    /// </summary>
    [ServiceContract]
    public interface IAuthService
    {
        /// <summary>
        /// Starts the authentication server.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof (InvalidOperationException))]
        void Start();

        /// <summary>
        /// Stops the authentication server.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof (InvalidOperationException))]
        void Stop();
    }
}