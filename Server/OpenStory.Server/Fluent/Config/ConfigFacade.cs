using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Fluent.Config
{
    internal sealed class ConfigFacade : IConfigFacade
    {
        #region Implementation of IConfigFacade

        public IDomainConfigFacade Domain(string domainName)
        {
            return new DomainConfigFacade(this, domainName);
        }

        #endregion
    }
}
