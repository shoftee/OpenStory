using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace OpenStory.Services.Contracts
{
    [ServiceContract(Namespace = null)]
    interface IServiceStateChanged
    {
        [OperationContract(IsOneWay = true)]
        void OnServiceStateChanged(ServiceState state);
    }
}
