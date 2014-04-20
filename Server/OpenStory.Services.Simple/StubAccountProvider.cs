using OpenStory.Framework.Contracts;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Services.Simple
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
