using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent
{
    internal class InitializeFacade : IInitializeFacade
    {
        /// <inheritdoc />
        public IInitializeManagerFacade<DataManager> DataManager()
        {
            return new InitializeManagerFacade<DataManager>(this);
        }
    }
}
