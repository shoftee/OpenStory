using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using OpenStory.Common.Tools;

namespace OpenStory.Server.Emulation
{
    internal class Launcher
    {
    }

    internal class OpenStoryModule
    {
        public AssemblyName AssemblyName { get; private set; }

        private readonly ReadOnlyCollection<string> arguments;

        public string[] Arguments
        {
            get { return arguments.ToArray(); }
        }

        public OpenStoryModule(AssemblyName assemblyName, params string[] args)
        {
            this.AssemblyName = assemblyName;

            this.arguments = args.ToReadOnly();
        }
    }
}
