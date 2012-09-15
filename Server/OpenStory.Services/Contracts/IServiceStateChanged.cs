using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// The callback interface for service state changes.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "ServiceStateChangedCallback")]
    internal interface IServiceStateChanged
    {
        [OperationContract(IsOneWay = true)]
        void OnServiceStateChanged(ServiceState newState);
    }
}
