using System.ComponentModel;

namespace OpenStory.Server.Modules.Sample
{
    /// <summary>
    /// An example manager component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IExampleInternalService
    {
        /// <summary>
        /// Does something.
        /// </summary>
        /// <param name="name">Your name plx.</param>
        /// <returns>you tell me.</returns>
        string DoSomething(string name);
    }
}