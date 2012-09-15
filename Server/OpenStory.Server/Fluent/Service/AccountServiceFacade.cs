using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Service
{
    internal sealed class AccountServiceFacade : NestedFacade<IServiceFacade>, IAccountServiceFacade
    {
        private readonly ServiceManager manager;

        public AccountServiceFacade(IServiceFacade parent)
            : base(parent)
        {
            this.manager = ServiceManager.GetManager();
        }

        #region Implementation of IServiceGetterSetterFacade<IAccountService,IAccountServiceFacade>

        public IAccountService Get()
        {
            throw new NotImplementedException();
        }

        public IAccountServiceFacade Set(IAccountService service)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}