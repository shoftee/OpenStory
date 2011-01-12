using System;

namespace OpenMaple
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ServerModuleAttribute : Attribute
    {
        private readonly InitializationStage stage;

        public ServerModuleAttribute(InitializationStage initializationStage)
        {
            this.stage = initializationStage;
        }

        public InitializationStage InitializationStage
        {
            get { return this.stage; }
        }
    }

    public enum InitializationStage
    {
        StartUp = 0,
        Settings = 1,
        Storage = 2,
    }
}