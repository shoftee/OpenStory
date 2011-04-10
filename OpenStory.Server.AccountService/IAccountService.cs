using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Server.Data;

namespace OpenStory.Server.AccountService
{
    /// <summary>
    /// Provides access to the AccountService.
    /// </summary>
    public interface IAccountService
    {
        bool IsActive(int accountId);

        IAccountSession RegisterSession(Account account);
    }

    interface ISessionManager
    {
        void UnregisterSession(int accountId);
    }
}
