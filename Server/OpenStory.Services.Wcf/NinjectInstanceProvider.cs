using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Ninject;

namespace OpenStory.Services.Wcf
{
    sealed class NinjectInstanceProvider : IInstanceProvider
    {
        private readonly Type serviceType;
        private readonly IKernel kernel;

        public NinjectInstanceProvider(Type serviceType, IKernel kernel)
        {
            this.serviceType = serviceType;
            this.kernel = kernel;
        }

        private object GetInstance()
        {
            return this.kernel.Get(this.serviceType);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return this.GetInstance();
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.GetInstance();
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            this.kernel.Release(instance);
        }
    }
}