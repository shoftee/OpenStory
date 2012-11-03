using System.ComponentModel;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides a fluent interface to the configuration manager.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IConfigFacade : IFluentInterface
    {
        /// <summary>
        /// Gets a facade for the configuration entries of a configuration domain.
        /// </summary>
        /// <param name="domainName">The name of the domain.</param>
        IDomainConfigFacade Domain(string domainName);
    }
}
