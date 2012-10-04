using System;
using OpenStory.Server.Modules.Services;
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

        #region Implementation of IServiceGetterFacade<IAccountService,IAccountServiceFacade>

        public IAccountService Get()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}