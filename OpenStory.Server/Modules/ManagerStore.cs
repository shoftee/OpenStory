using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Modules
{
    /// <summary>
    /// This class is pro as hell.
    /// </summary>
    /// <typeparam name="TManagerBase"></typeparam>
    internal sealed class ManagerStore<TManagerBase>
        where TManagerBase : ManagerBase
    {
        private readonly Dictionary<Type, TManagerBase> managers;

        public ManagerStore()
        {
            this.managers = new Dictionary<Type, TManagerBase>();
        }

        public bool RegisterDefault(TManagerBase manager)
        {
            Type type = typeof(TManagerBase);
            if (this.managers.ContainsKey(type))
            {
                this.managers[type] = manager;
                return true;
            }
            else
            {
                this.managers.Add(type, manager);
                return false;
            }
        }

        public void RegisterManager<TManager>(TManager manager)
            where TManager : TManagerBase
        {
            this.managers.Add(typeof(TManager), manager);
        }

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


    }
}
