using System.ComponentModel;
using OpenStory.Server.Modules.Config;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides a fluent interface for configuring a domain.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IDomainConfigFacade : INestedFacade<IConfigFacade>
    {
        /// <summary>
        /// Retrieves the value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key to look up.</param>
        /// <param name="value">A variable to hold the result.</param>
        IDomainConfigFacade Get<T>(string key, out T value);

        /// <summary>
        /// Retrieves the value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key to look up.</param>
        /// <returns>the value cast to to <typeparamref name="T"/>, or the default value for <typeparamref name="T"/> if there was no value for the key.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Sets the value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key to set the value for.</param>
        /// <param name="newValue">The value to set.</param>
        IDomainConfigFacade Set<T>(string key, T newValue);

        /// <summary>
        /// Retrieves an active list of all configuration entries for the domain.
        /// </summary>
        /// <param name="config">A variable to hold the list.</param>
        IDomainConfigFacade GetAll(out DomainConfig config);

        /// <summary>
        /// Retrieves an active list of all configuration entries for the domain.
        /// </summary>
        /// <returns>a <see cref="DomainConfig"/> object containing the configuration entries.</returns>
        DomainConfig GetAll();
    }
}