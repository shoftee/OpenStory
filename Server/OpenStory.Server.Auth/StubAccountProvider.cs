using OpenStory.Framework.Contracts;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.Auth
{
    class StubAccountProvider : IAccountProvider
    {
        public Account LoadByUserName(string userName)
        {
            return null;
        }

        public void Save(Account account)
        {
            
        }
    }
}
