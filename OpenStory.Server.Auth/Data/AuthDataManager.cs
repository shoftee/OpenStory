using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Server.Auth.Data.Providers;
using OpenStory.Server.Modules;

namespace OpenStory.Server.Auth.Data
{
    /// <summary>
    /// The DataManager class for the Authentication server.
    /// </summary>
    public sealed class AuthDataManager : DataManager
    {
        /// <summary>
        /// Gets the Character data provider.
        /// </summary>
        public ICharacterDataProvider Characters { get; private set; }

        /// <summary>
        /// Gets the World data provider.
        /// </summary>
        public IWorldDataProvider Worlds { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="AuthDataManager"/>.
        /// </summary>
        public AuthDataManager()
        {
            base.RequireComponent("Characters", typeof(ICharacterDataProvider));
            base.RequireComponent("Worlds", typeof(IWorldDataProvider));
        }
    }
}
