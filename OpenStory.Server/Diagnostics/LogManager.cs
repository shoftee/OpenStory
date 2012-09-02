using OpenStory.Server.Modules;

namespace OpenStory.Server.Diagnostics
{
    /// <summary>
    /// Represents a logging manager.
    /// </summary>
    public sealed class LogManager : ManagerBase<LogManager>
    {
        /// <summary>
        /// The component name for the Logger object.
        /// </summary>
        public const string LoggerKey = "Logger";

        /// <summary>
        /// The singleton instance for the LogManager.
        /// </summary>
        public static readonly LogManager Instance = new LogManager();

        private ILogger Logger { get; set; }

        private LogManager()
        {
            base.RequireComponent(LoggerKey, typeof(ILogger));
        }

        /// <inheritdoc />
        protected override void OnInitializing()
        {
            // If there is no internal logger registered at this point, register the default one.
            if (!this.CheckComponent(LoggerKey))
            {
                this.RegisterComponent(LoggerKey, new DefaultLogger());
            }

            base.OnInitializing();
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.Logger = base.GetComponent<ILogger>(LoggerKey);
        }

        /// <inheritdoc cref="ILogger.Info(string, object[])" />
        public void Info(string format, params object[] args)
        {
            this.Logger.Info(format, args);
        }

        /// <inheritdoc cref="ILogger.Warning(string, object[])" />
        public void Warning(string format, params object[] args)
        {
            this.Logger.Warning(format, args);
        }

        /// <inheritdoc cref="ILogger.Error(string, object[])" />
        public void Error(string format, params object[] args)
        {
            this.Logger.Error(format, args);
        }
    }
}
