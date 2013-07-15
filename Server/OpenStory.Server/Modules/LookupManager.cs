using OpenStory.Server.Registry;

namespace OpenStory.Server.Modules
{
    /// <summary>
    /// The manager responsible for various game lookup services.
    /// </summary>
    public sealed class LookupManager : ManagerBase<LookupManager>
    {
        /// <summary>
        /// The name of the <see cref="Location"/> component.
        /// </summary>
        public const string LocationKey = "Location";

        /// <summary>
        /// The name of the <see cref="Players"/> component.
        /// </summary>
        public const string PlayersKey = "Players";

        /// <summary>
        /// Gets the location registry component.
        /// </summary>
        public ILocationRegistry Location { get; private set; }

        /// <summary>
        /// Gets the identity registry component.
        /// </summary>
        public IPlayerRegistry Players { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupManager"/> class.
        /// </summary>
        public LookupManager()
        {
            this.AllowComponent<ILocationRegistry>(LocationKey);
            this.AllowComponent<IPlayerRegistry>(PlayersKey);
        }

        /// <summary><inheritdoc /></summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.Location = this.GetComponent<ILocationRegistry>(LocationKey);
            this.Players = this.GetComponent<IPlayerRegistry>(PlayersKey);
        }
    }
}
