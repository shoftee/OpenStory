using System.ServiceModel;
using OpenStory.Server.Accounts;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Account
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    internal sealed class AccountService : RegisteredServiceBase<AccountServer>
    {
        public AccountService(AccountServer accountServer)
            : base(accountServer)
        {
        }
    }
}
