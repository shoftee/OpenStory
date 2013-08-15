using System;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Specifies the state of a service operation.
    /// </summary>
    [Serializable]
    public enum OperationState
    {
        /// <summary>
        /// The operation was successfully executed.
        /// </summary>
        Success,

        /// <summary>
        /// The operation failed during the communication between the local and the target endpoints.
        /// </summary>
        /// <remarks>
        /// This may happen if the local endpoint cannot reach the target.
        /// </remarks>
        FailedLocally,

        /// <summary>
        /// The operation failed during a call executed on the remote endpoint.
        /// </summary>
        /// <remarks>
        /// This may happen if the remote endpoint calls a third service and that call fails.
        /// </remarks>
        FailedRemotely,
    }
}