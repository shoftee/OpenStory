using OpenStory.Server.Data.Providers;
using OpenStory.Server.Modules;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// Provides methods and components for managing server data.
    /// </summary>
    public class DataManager : ManagerBase<DataManager>
    {
        /// <summary>
        /// The name of the IBanProvider component.
        /// </summary>
        public const string BansKey = "Bans";

        /// <summary>
        /// The name of the IAccountProvider component.
        /// </summary>
        public const string AccountsKey = "Accounts";

        /// <summary>
        /// Gets the ban data provider.
        /// </summary>
        public IBanProvider Bans { get; private set; }

        /// <summary>
        /// Gets the account data provider.
        /// </summary>
        public IAccountProvider Accounts { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DataManager"/>.
        /// </summary>
        public DataManager()
        {
            base.AllowComponent<IBanProvider>(BansKey);
            base.AllowComponent<IAccountProvider>(AccountsKey);
        }

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (base.CheckComponent(BansKey))
            {
                this.Bans = base.GetComponent<IBanProvider>(BansKey);
            }
            if (base.CheckComponent(AccountsKey))
            {
                this.Accounts = base.GetComponent<IAccountProvider>(AccountsKey);
            }
        }
    }
}
