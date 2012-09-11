using OpenStory.Server.Auth.Data.Providers;
using OpenStory.Server.Data;

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
        public IAuthCharacterProvider Characters { get; private set; }

        /// <summary>
        /// Gets the World data provider.
        /// </summary>
        public IWorldInfoProvider Worlds { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="AuthDataManager"/>.
        /// </summary>
        public AuthDataManager()
        {
            base.RequireComponent<IAuthCharacterProvider>("Characters");
            base.RequireComponent<IWorldInfoProvider>("Worlds");
        }
    }
}
