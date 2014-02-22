using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
