namespace OpenStory.Services
{
    /// <summary>
    /// Represents a thingie which starts the rest of the thingies.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Starts ALL the things!
        /// </summary>
        void Start();
    }
}