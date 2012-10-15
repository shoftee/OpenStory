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
        /// The name of the <see cref="Characters"/> component.
        /// </summary>
        public const string CharactersKey = "Characters";

        /// <summary>
        /// The name of the <see cref="Worlds"/> component.
        /// </summary>
        public const string WorldsKey = "Worlds";

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
            base.AllowComponent<IAuthCharacterProvider>(CharactersKey);
            base.AllowComponent<IWorldInfoProvider>(WorldsKey);
        }

        /// <summary><inheritdoc /></summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (base.CheckComponent(CharactersKey))
            {
                this.Characters = base.GetComponent<IAuthCharacterProvider>(CharactersKey);
            }

            if (base.CheckComponent(WorldsKey))
            {
                this.Worlds = base.GetComponent<IWorldInfoProvider>(WorldsKey);
            }
        }
    }
}
