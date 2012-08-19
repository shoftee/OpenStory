namespace OpenStory.Server.Modules
{
    /// <summary>
    /// Represents a module builder.
    /// </summary>
    /// <typeparam name="TModule">The type of modules to build.</typeparam>
    public class ModuleBuilder<TModule> where TModule : ModuleBase, new()
    {
        private TModule module;

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleBuilder{TModule}"/>.
        /// </summary>
        public ModuleBuilder()
        {
            this.module = new TModule();
        }

        /// <summary>
        /// Registers a component instance to an entry.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="instance">The instance to register to the entry.</param>
        /// <returns>the current instance of the module builder.</returns>
        public ModuleBuilder<TModule> Register(string name, object instance)
        {
            this.module.RegisterComponent(name, instance);
            return this;
        }

        /// <summary>
        /// Builds the module and prepares the <see cref="ModuleBuilder{TModule}"/> for a new build.
        /// </summary>
        /// <returns>the built <typeparamref name="TModule"/> instance.</returns>
        public TModule BuildAndReset()
        {
            TModule finishedModule = this.module;
            finishedModule.Initialize();

            this.module = new TModule();
            return finishedModule;
        }
    }
}