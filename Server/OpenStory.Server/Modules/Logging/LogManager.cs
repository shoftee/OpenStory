namespace OpenStory.Server.Modules.Logging
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
        /// Gets the logger of this instance.
        /// </summary>
        public ILogger Logger { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="LogManager"/>.
        /// </summary>
        public LogManager()
        {
            base.AllowComponent<ILogger>(LoggerKey);
        }

        /// <inheritdoc />
        protected override void OnInitializing()
        {
            // If there is no internal logger registered at this point, register a null logger.
            if (!this.CheckComponent(LoggerKey))
            {
                this.RegisterComponent(LoggerKey, NullLogger.Instance);
            }

            base.OnInitializing();
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.Logger = base.GetComponent<ILogger>(LoggerKey);
        }
    }
}
