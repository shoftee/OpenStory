namespace OpenStory.Server.Synchronization
{
    /// <summary>
    /// Provides methods for running an operation.
    /// </summary>
    public interface IRunnable
    {
        /// <summary>
        /// Executes the encapsulated action.
        /// </summary>
        void Run();
    }
}