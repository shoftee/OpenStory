namespace OpenStory.Server.Modules.Config
{
    /// <summary>
    /// Provides methods for storing and retrieving configuration entries.
    /// </summary>
    public interface IConfigDataProvider
    {
        /// <summary>
        /// Stores an object for the specified domain and key.
        /// </summary>
        /// <param name="domain">The configuration domain.</param>
        /// <param name="key">The key of the configuration entry.</param>
        /// <param name="value">The value to store.</param>
        void StoreObject(string domain, string key, object value);

        /// <summary>
        /// Retrieves an object for the specified domain and key.
        /// </summary>
        /// <param name="domain">The configuration domain.</param>
        /// <param name="key">The key of the configuration entry.</param>
        /// <returns>the retrieved object, or <c>null</c> if there was no such object.</returns>
        object GetObject(string domain, string key);
    }
}
