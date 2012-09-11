using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Service
{
    internal sealed class AuthServiceFacade : NestedFacade<IServiceFacade>, IAuthServiceFacade
    {
        public AuthServiceFacade(IServiceFacade parent)
            : base(parent)
        {
        }

        #region Implementation of IServiceGetterSetterFacade<IAuthService,IAuthServiceFacade>

        public IAuthService Get()
        {
            throw new System.NotImplementedException();
        }

        public IAuthServiceFacade Set(IAuthService service)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}