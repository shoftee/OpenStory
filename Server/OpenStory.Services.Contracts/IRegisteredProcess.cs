namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for starting and stopping a service.
    /// </summary>
    public interface IRegisteredProcess
    {
        /// <summary>
        /// Gets whether the server is running or not.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Starts the server.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the server.
        /// </summary>
        void Stop();
    }
}