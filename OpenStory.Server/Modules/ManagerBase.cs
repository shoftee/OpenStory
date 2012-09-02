using System;
using System.Collections.Generic;

namespace OpenStory.Server.Modules
{
    /// <summary>
    /// Represents a base class for server managers.
    /// </summary>
    public abstract class ManagerBase
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
        /// Initializes a new instance of <see cref="ManagerBase"/>.
        /// </summary>
        protected ManagerBase()
        {
            this.types = new Dictionary<string, Type>();
            this.instances = new Dictionary<string, object>();
        }

        /// <summary>
        /// Runs the initialization checks on this instance and marks it as initialized.
        /// </summary>
        public void Initialize()
        {
            ThrowIfInitialized();

            this.OnInitializing();
            InitializeAndThrowOnError();

            this.isInitialized = true;
            this.OnInitialized();
        }

        /// <summary>
        /// A hook to the start of the <see cref="Initialize()"/> method.
        /// </summary>
        /// <remarks>
        /// When overriding, call the method of the base class after your logic.
        /// </remarks>
        protected virtual void OnInitializing()
        {
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
                throw GetUnknownComponentNameException(name);
            }
            if (!required.IsAssignableFrom(type))
            {
                throw GetIncompatibleTypeException(instance, required.FullName);
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
                throw GetUnknownComponentNameException(name);
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
                throw GetModuleNotInitializedException();
            }
        }

        /// <summary>
        /// Throws a <see cref="InvalidOperationException"/> if the instance is initialized.
        /// </summary>
        protected void ThrowIfInitialized()
        {
            if (this.isInitialized)
            {
                throw GetModuleInitializedException();
            }
        }

        /// <summary>
        /// Runs the initialization checks for this module.
        /// </summary>
        /// <remarks>
        /// When there are no errors, <paramref name="error"/> will be set to <c>null</c> before the method returns.
        /// </remarks>
        /// <param name="error">A variable to hold an error message.</param>
        /// <returns><c>true</c> if there were no errors; otherwise, <c>false</c>.</returns>
        protected bool RunInitializationCheck(out string error)
        {
            foreach (var entry in this.instances)
            {
                string name = entry.Key;
                object instance = entry.Value;
                if (instance == null)
                {
                    const string NotInitializedFormat = "The component '{0}' has not been initialized.";

                    error = String.Format(NotInitializedFormat, name);
                    return false;
                }
            }

            error = null;
            return true;
        }

        private static InvalidOperationException GetModuleNotInitializedException()
        {
            return new InvalidOperationException("The module has not been initialized yet.");
        }

        private static InvalidOperationException GetModuleInitializedException()
        {
            return new InvalidOperationException("You cannot change this module because it has been initialized.");
        }

        private static ArgumentOutOfRangeException GetIncompatibleTypeException(object instance, string typeFullName)
        {
            const string Format = "'instance' must be assignable to '{0}'";

            string message = String.Format(Format, typeFullName);
            return new ArgumentOutOfRangeException("instance", message);
        }

        private static ArgumentOutOfRangeException GetUnknownComponentNameException(string name)
        {
            return new ArgumentOutOfRangeException("name", name, "'name' must be the name of a required component.");
        }
    }
}
