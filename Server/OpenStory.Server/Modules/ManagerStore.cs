using System;
using System.Collections.Generic;

namespace OpenStory.Server.Modules
{
    /// <summary>
    /// This class is pro as hell.
    /// </summary>
    /// <typeparam name="TManagerBase">The type of managers to store.</typeparam>
    internal sealed class ManagerStore<TManagerBase>
        where TManagerBase : ManagerBase
    {
        private readonly Dictionary<Type, TManagerBase> managers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerStore{TManagerBase}"/> class.
        /// </summary>
        public ManagerStore()
        {
            this.managers = new Dictionary<Type, TManagerBase>();
        }

        /// <summary>
        /// Registers the default <typeparamref name="TManagerBase"/> object for this instance..
        /// </summary>
        /// <param name="manager">The manager object to use as a default.</param>
        public void RegisterDefault(TManagerBase manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            Type baseType = typeof(TManagerBase);
            this.AddManagerEntry(baseType, manager);

            Type actualType = manager.GetType();
            if (baseType != actualType)
            {
                this.AddManagerEntry(actualType, manager);
            }
        }

        /// <summary>
        /// Registers a manager object for the <typeparamref name="TManager"/> type.
        /// </summary>
        /// <typeparam name="TManager">The type to register a manager object for.</typeparam>
        /// <param name="manager">The manager object to register.</param>
        public void RegisterManager<TManager>(TManager manager)
            where TManager : TManagerBase
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            this.AddManagerEntry(typeof(TManager), manager);
        }

        /// <summary>
        /// Retrieves a manager object.
        /// </summary>
        /// <typeparam name="TManager">The type of the manager object to retrieve.</typeparam>
        /// <returns>an object of type <typeparamref name="TManager"/>, or <c>null</c> if none such was found.</returns>
        public TManager GetManager<TManager>()
            where TManager : TManagerBase
        {
            TManagerBase manager;
            if (this.managers.TryGetValue(typeof(TManager), out manager))
            {
                return (TManager)manager;
            }
            else
            {
                return null;
            }
        }

        private void AddManagerEntry(Type type, TManagerBase manager)
        {
            if (this.managers.ContainsKey(type))
            {
                this.managers[type] = manager;
            }
            else
            {
                this.managers.Add(type, manager);
            }
        }
    }
}
