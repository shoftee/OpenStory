using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OpenStory.Server.Modules
{
    /// <summary>
    /// Represents a base class for server managers.
    /// </summary>
    [Localizable(true)]
    public abstract class ManagerBase
    {
        private bool isInitialized;

        private readonly Dictionary<string, Type> types;
        private readonly Dictionary<string, ComponentKey> keys;
        private readonly Dictionary<ComponentKey, object> instances;

        /// <summary>
        /// Initializes a new instance of <see cref="ManagerBase"/>.
        /// </summary>
        protected ManagerBase()
        {
            this.isInitialized = false;

            this.types = new Dictionary<string, Type>();
            this.keys = new Dictionary<string, ComponentKey>();
            this.instances = new Dictionary<ComponentKey, object>();
        }

        /// <summary>
        /// Runs the initialization checks on this instance and marks it as initialized.
        /// </summary>
        /// <inheritdoc cref="ThrowIfInitialized()" select="exception[@cref='InvalidOperationException']" />
        /// <inheritdoc cref="InitializeAndThrowOnError()" select="exception[@cref='InvalidOperationException']" />
        public void Initialize()
        {
            this.ThrowIfInitialized();

            this.OnInitializing();
            this.InitializeAndThrowOnError();

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
        /// <exception cref="InvalidOperationException">Thrown if there is an error during initialization.</exception>
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
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <param name="name">The name of the component to require.</param>
        /// <inheritdoc cref="ThrowIfInitialized()" select="exception[@cref='InvalidOperationException']" />
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c>.</exception>
        protected void RequireComponent<TComponent>(string name)
            where TComponent : class
        {
            this.ThrowIfInitialized();

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.CreateComponentEntries<TComponent>(name, true);
        }

        /// <summary>
        /// Marks a component name and associated type as allowed but optional.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <param name="name">The name of the component to allow.</param>
        /// <inheritdoc cref="ThrowIfInitialized()" select="exception[@cref='InvalidOperationException']" />
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c>.</exception>
        protected void AllowComponent<TComponent>(string name)
            where TComponent : class
        {
            this.ThrowIfInitialized();

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.CreateComponentEntries<TComponent>(name, false);
        }

        private void CreateComponentEntries<TComponent>(string name, bool isRequired)
        {
            this.types.Add(name, typeof(TComponent));

            var key = new ComponentKey(name, isRequired);
            this.keys.Add(name, key);

            this.instances.Add(key, null);
        }

        /// <summary>
        /// Registers an object to a component entry for this module.
        /// </summary>
        /// <typeparam name="TComponent">The type of the object.</typeparam>/>
        /// <param name="name">The name of the component.</param>
        /// <param name="instance">The instance for the component entry.</param>
        /// <inheritdoc cref="ThrowIfInitialized()" select="exception[@cref='InvalidOperationException']" />
        /// <exception cref="ArgumentNullException">Thrown if any of the parameters is <c>null</c>.</exception>
        public void RegisterComponent<TComponent>(string name, TComponent instance)
            where TComponent : class
        {
            this.ThrowIfInitialized();

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

            RegisterComponentInternal(name, instance);
        }

        private void RegisterComponentInternal(string name, object instance)
        {
            var key = this.keys[name];
            this.instances[key] = instance;
        }

        /// <summary>
        /// Gets the component entry instance for the specified name.
        /// </summary>
        /// <typeparam name="TComponent">The type to cast the result to.</typeparam>
        /// <param name="name">The name of the component.</param>
        /// <inheritdoc cref="ThrowIfNotInitialized()" select="exception[@cref='InvalidOperationException']" />
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c>.</exception>
        /// <returns>the registered instance, cast to <typeparamref name="TComponent"/>; or <c>null</c> if there is no registered instance.</returns>
        protected TComponent GetComponent<TComponent>(string name)
            where TComponent : class
        {
            this.ThrowIfNotInitialized();

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (!this.types.ContainsKey(name))
            {
                throw GetUnknownComponentNameException(name);
            }

            var key = this.keys[name];
            return this.instances[key] as TComponent;
        }

        /// <summary>
        /// Checks if a component instance is registered for the specified name.
        /// </summary>
        /// <param name="name">The name of the component slot to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c>.</exception>
        /// <returns><c>true</c> if there is an instance registered; otherwise, <c>false</c>.</returns>
        protected bool CheckComponent(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (!this.types.ContainsKey(name))
            {
                throw GetUnknownComponentNameException(name);
            }

            var key = this.keys[name];
            bool isPresent = this.instances[key] != null;
            return isPresent;

        }

        /// <summary>
        /// Throws a <see cref="InvalidOperationException"/> if the instance is not initialized.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the instance has already been fully initialized.</exception>
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
        /// <exception cref="InvalidOperationException">Thrown if the instance has not been fully initialized.</exception>
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
                var key = entry.Key;
                object instance = entry.Value;
                if (key.IsRequired && instance == null)
                {
                    const string NotInitializedFormat = "The required component '{0}' has not been initialized.";

                    error = String.Format(NotInitializedFormat, key.Name);
                    return false;
                }
            }

            error = null;
            return true;
        }

        private static InvalidOperationException GetModuleNotInitializedException()
        {
            return new InvalidOperationException(Exceptions.ModuleNotInitialized);
        }

        private static InvalidOperationException GetModuleInitializedException()
        {
            return new InvalidOperationException(Exceptions.CannotChangeInitializedModule);
        }

        private static ArgumentException GetIncompatibleTypeException(object instance, string typeFullName)
        {
            string message = String.Format(Exceptions.ObjectNotAssignableToType, typeFullName);
            return new ArgumentException("instance", message);
        }

        private static ArgumentOutOfRangeException GetUnknownComponentNameException(string name)
        {
            return new ArgumentOutOfRangeException("name", name, Exceptions.UnknownComponentName);
        }
    }
}
