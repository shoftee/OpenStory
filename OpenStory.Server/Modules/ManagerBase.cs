using System;
using System.Collections.Generic;

namespace OpenStory.Server.Modules
{
    /// <summary>
    /// Represents a base class for server modules.
    /// </summary>
    public class ManagerBase
    {
        private bool isInitialized;

        private readonly Dictionary<string, Type> types;
        private readonly Dictionary<string, object> instances;

        /// <summary>
        /// Gets a map of the required components and their accepted types.
        /// </summary>
        protected Dictionary<string, Type> RequiredComponents
        {
            get { return new Dictionary<string, Type>(types); }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ManagerBase"/>
        /// </summary>
        protected ManagerBase()
        {
            this.types = new Dictionary<string, Type>();
            this.instances = new Dictionary<string, object>();
        }

        /// <summary>
        /// Runs the initialization checks and marks it as initialized.
        /// </summary>
        public void Initialize()
        {
            InitializeAndThrowOnError();

            this.isInitialized = true;
            this.OnInitialized();
        }

        /// <summary>
        /// A hook to the end of the <see cref="Initialize()"/> method.
        /// </summary>
        /// <remarks>
        /// When overriding, call the method of the base class before your logic.
        /// </remarks>
        protected virtual void OnInitialized()
        {
        }

        /// <summary>
        /// Runs the initialization checks on this instance and throws on error.
        /// </summary>
        private void InitializeAndThrowOnError()
        {
            string error;
            bool success = this.RunInitializationCheck(out error);
            if (!success)
            {
                throw new InvalidOperationException(error);
            }
        }

        /// <summary>
        /// Marks a component name and associated type as required.
        /// </summary>
        /// <param name="name">The name of the component to require.</param>
        /// <param name="type">The type of the component.</param>
        protected void RequireComponent(string name, Type type)
        {
            ThrowIfInitialized();
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            this.types.Add(name, type);
            this.instances.Add(name, null);
        }

        /// <summary>
        /// Marks a component name and associated type as required.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <param name="name">The name of the component to require.</param>
        protected void RequireComponent<TComponent>(string name)
        {
            ThrowIfInitialized();
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.RequireComponent(name, typeof(TComponent));
        }

        /// <summary>
        /// Registers an object to a component entry for this module.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="instance">The instance for the component entry.</param>
        public void RegisterComponent(string name, object instance)
        {
            ThrowIfInitialized();
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            Type type = instance.GetType();

            Type required;
            if (!this.types.TryGetValue(name, out required))
            {
                throw new ArgumentOutOfRangeException("name", name, "'name' must be the name of a registered component.");
            }

            if (!required.IsAssignableFrom(type))
            {
                throw new ArgumentOutOfRangeException("instance", "'instance' is of an incompatible type.");
            }

            this.types[name] = required;
        }

        /// <summary>
        /// Gets the component entry instance for the specified name.
        /// </summary>
        /// <typeparam name="TComponent">The type to cast the result to.</typeparam>
        /// <param name="name">The name of the component.</param>
        /// <returns>the registered instance, cast to <typeparamref name="TComponent"/>.</returns>
        protected TComponent GetComponent<TComponent>(string name)
        {
            ThrowIfNotInitialized();
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (!this.types.ContainsKey(name))
            {
                throw new ArgumentOutOfRangeException("name", name, "'name' must be the name of a registered component.");
            }

            return (TComponent)this.instances[name];
        }

        /// <summary>
        /// Throws a <see cref="InvalidOperationException"/> if the instance is not initialized.
        /// </summary>
        protected void ThrowIfNotInitialized()
        {
            if (!this.isInitialized)
            {
                throw new InvalidOperationException("The module has not been initialized yet.");
            }
        }

        /// <summary>
        /// Throws a <see cref="InvalidOperationException"/> if the instance is initialized.
        /// </summary>
        private void ThrowIfInitialized()
        {
            if (this.isInitialized)
            {
                throw new InvalidOperationException("You cannot change this module because it has been initialized.");
            }
        }

        /// <summary>
        /// Runs the initialization checks for this module.
        /// </summary>
        /// <remarks>
        /// When there are no errors, <paramref name="error"/> will be set to null before the method returns.
        /// </remarks>
        /// <param name="error">A variable to hold an error message.</param>
        /// <returns><c>true</c> if there were no errors; otherwise, <c>false</c>.</returns>
        protected bool RunInitializationCheck(out string error)
        {
            const string NotInitializedFormat = "The component '{0}' has not been initialized.";

            foreach (var entry in this.instances)
            {
                string name = entry.Key;
                object instance = entry.Value;
                if (instance == null)
                {
                    error = String.Format(NotInitializedFormat, name);
                    return false;
                }
            }

            error = null;
            return true;
        }
    }
}
