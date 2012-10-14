using System.ComponentModel;
using OpenStory.Server.Modules;
using OpenStory.Server.Registry;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// The entry point for the look-up fluent interface.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ILookupFacade : IFluentInterface
    {
        /// <summary>
        /// Gets the location registry.
        /// </summary>
        ILocationRegistry Location();
    }

    internal sealed class LookupFacade : ILookupFacade
    {
        private readonly LookupManager manager;

        public LookupFacade()
        {
            this.manager = LookupManager.GetManager();
        }

        #region Implementation of ILookupFacade

        public ILocationRegistry Location()
        {
            return this.manager.Location;
        }

        #endregion
    }
}