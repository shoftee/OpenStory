using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenStory.Server.Emulation
{
    class Launcher
    {
    }

    class OpenStoryModule
    {
        public AssemblyName AssemblyName { get; private set; }

        private List<string> arguments;

        public string[] Arguments
        {
            get { return arguments.ToArray(); }
        }

        public OpenStoryModule(AssemblyName assemblyName, params string[] args)
        {
            this.AssemblyName = assemblyName;

            this.arguments = args.ToList();
        }
    }
}
