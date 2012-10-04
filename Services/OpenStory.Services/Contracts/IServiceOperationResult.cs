using System;
using System.Runtime.Serialization;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides properties of a service operation result.
    /// </summary>
    public interface IServiceOperationResult
    {
        /// <summary>
        /// Gets the state of the operation.
        /// </summary>
        [DataMember]
        OperationState OperationState { get; }

        /// <summary>
        /// Gets the exception that was thrown, if any.
        /// </summary>
        [DataMember]
        Exception Error { get; }

        /// <summary>
        /// Gets the state of the called game service.
        /// </summary>
        [DataMember]
        ServiceState ServiceState { get; }
    }
}