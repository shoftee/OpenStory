using System;

namespace OpenStory.Server.Modules
{
    /// <summary>
    /// Represents a manager builder.
    /// </summary>
    /// <typeparam name="TManager">The type of manager to build.</typeparam>
    public sealed class ManagerBuilder<TManager>
        where TManager : ManagerBase<TManager>, new()
    {
        private TManager manager;

        /// <summary>
        /// Initializes a new instance of <see cref="ManagerBuilder{TManager}"/>.
        /// </summary>
        public ManagerBuilder()
        {
            this.manager = new TManager();
        }

        /// <summary>
        /// Registers a component instance to an entry.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="instance">The instance to register to the entry.</param>
        /// <returns>the current instance of the manager builder.</returns>
        public ManagerBuilder<TManager> Register(string name, object instance)
        {
            this.manager.RegisterComponent(name, instance);
            return this;
        }

        /// <summary>
        /// Builds the manager and prepares the <see cref="ManagerBuilder{TManager}"/> for a new build.
        /// </summary>
        /// <returns>the built <typeparamref name="TManager"/> instance.</returns>
        public TManager BuildAndReset()
        {
            TManager finishedModule = this.manager;
            finishedModule.Initialize();

            this.manager = new TManager();
            return finishedModule;
        }
    }
}