using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Service
{
    internal sealed class AccountServiceFacade : NestedFacade<IServiceFacade>, IAccountServiceFacade
    {
        public AccountServiceFacade(IServiceFacade parent)
            : base(parent)
        {
        }

        #region Implementation of IServiceGetterSetterFacade<IAccountService,IAccountServiceFacade>

        public IAccountService Get()
        {
            throw new System.NotImplementedException();
        }

        public IAccountServiceFacade Set(IAccountService service)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}