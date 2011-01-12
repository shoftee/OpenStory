using System;
using OpenStory.Common;
using OpenStory.Server.Data;

namespace OpenStory.Server
{
    class SimpleAccount : IAccount
    {
        #region IAccount Members

        public int AccountId
        {
            get { throw new NotImplementedException(); }
        }

        public string UserName
        {
            get { throw new NotImplementedException(); }
        }

        public Gender Gender
        {
            get { throw new NotImplementedException(); }
        }

        public GameMasterLevel GameMasterLevel
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}