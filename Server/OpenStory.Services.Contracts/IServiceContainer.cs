namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Hosts other services!
    /// </summary>
    public interface IServiceContainer<in TService>
    {
        /// <summary>
        /// Registers the provided service object.
        /// </summary>
        void Register(TService service);

        /// <summary>
        /// Unregisters the provided service object.
        /// </summary>
        void Unregister(TService service);
    }
}
