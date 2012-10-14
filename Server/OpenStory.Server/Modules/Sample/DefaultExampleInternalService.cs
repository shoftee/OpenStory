using System.ComponentModel;

namespace OpenStory.Server.Modules.Sample
{
    /// <summary>
    /// Default implementation for an example manager component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class DefaultExampleInternalService : IExampleInternalService
    {
        public static readonly DefaultExampleInternalService Instance =
            new DefaultExampleInternalService();

        private DefaultExampleInternalService()
        {
        }

        #region Implementation of IExampleInternalService

        /// <inheritdoc />
        public string DoSomething(string name)
        {
            return name + " " + name;
        }

        #endregion
    }
}