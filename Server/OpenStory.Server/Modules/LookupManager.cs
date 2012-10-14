using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Server.Registry;

namespace OpenStory.Server.Modules
{
    /// <summary>
    /// The manager responsible for various game lookup services.
    /// </summary>
    public sealed class LookupManager : ManagerBase<LookupManager>
    {
        /// <summary>
        /// The name of the "Location" component.
        /// </summary>
        public const string LocationKey = "Location";

        /// <summary>
        /// Gets the location registry component.
        /// </summary>
        public ILocationRegistry Location { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="LookupManager"/>.
        /// </summary>
        public LookupManager()
        {
            base.AllowComponent<ILocationRegistry>(LocationKey);
        }

        /// <summary><inheritdoc /></summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.Location = base.GetComponent<ILocationRegistry>(LocationKey);
        }
    }
}
