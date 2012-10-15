using System.ComponentModel;
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
        /// Gets the character lookup facade.
        /// </summary>
        ICharacterLookupFacade Character();
    }
}