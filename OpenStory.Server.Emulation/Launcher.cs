using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenStory.Server.Emulation
{
    internal class Launcher
    {
    }

    internal class OpenStoryModule
    {
        public string AssemblyPath { get; private set; }

        public OpenStoryModule(string assemblyPath)
        {
            this.AssemblyPath = assemblyPath;
        }
    }
}
