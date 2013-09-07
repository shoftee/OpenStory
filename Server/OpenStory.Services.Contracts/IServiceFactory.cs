namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Represents a factory for service objects.
    /// </summary>
    public interface IServiceFactory<out TService>
    {
        /// <summary>
        /// Creates an instance of the <typeparamref name="TService"/> type.
        /// </summary>
        TService CreateService();
    }
}
