using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Service
{
    internal sealed class AuthServiceFacade : NestedFacade<IServiceFacade>, IAuthServiceFacade
    {
        public AuthServiceFacade(IServiceFacade parent)
            : base(parent)
        {
        }

        #region Implementation of IServiceGetterFacade<IAuthService,IAuthServiceFacade>

        public IAuthService Get()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}