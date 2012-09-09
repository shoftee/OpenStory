namespace OpenStory.Server.Management
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
