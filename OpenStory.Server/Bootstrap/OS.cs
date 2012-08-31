using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Server.Modules;
using OpenStory.Server.Modules.Default;

namespace OpenStory.Server.Bootstrap
{
    /// <summary>
    /// The OpenStory management entry point.
    /// </summary>
    public sealed class OS
    {
        private static readonly OS Instance = new OS();

        private readonly DataManager data;

        /// <summary>
        /// Gets the current DataManager instance.
        /// </summary>
        public static DataManager Data
        {
            get { return Instance.data; }
        }

        private OS()
        {
            DataManager.RegisterDefault(new DefaultDataManager());

            this.data = DataManager.GetManager();
        }
    }
}
