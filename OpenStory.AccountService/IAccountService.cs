using System.ServiceModel;
using OpenStory.Server;
using OpenStory.Server.Data;

namespace OpenStory.AccountService
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        bool IsActive(int accountId);

        [OperationContract]
        IAccountSession RegisterSession(Account account);
    }
}