using System.ComponentModel;

namespace OpenStory.Server.Modules.Sample
{
    /// <summary>
    /// An example manager component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IExampleService
    {
        /// <summary>
        /// Does something awesome.
        /// </summary>
        /// <param name="name">Your name plx.</param>
        /// <returns>you tell me.</returns>
        string DoSomethingAwesome(string name);
    }
}