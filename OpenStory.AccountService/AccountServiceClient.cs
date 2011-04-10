using System.ServiceModel;
using OpenStory.Server;
using OpenStory.Server.Data;

namespace OpenStory.AccountService
{
    public class AccountServiceClient : ClientBase<IAccountService>, IAccountService
    {
        public AccountServiceClient()
            : base(ServiceHelpers.GetBinding(), new EndpointAddress(ServerConstants.AccountServiceUri)) {}

        #region IAccountService Members

        public bool IsActive(int accountId)
        {
            return base.Channel.IsActive(accountId);
        }

        public IAccountSession RegisterSession(Account account)
        {
            return base.Channel.RegisterSession(account);
        }

        #endregion
    }
}