using OpenStory.Server.Modules.Config;

namespace OpenStory.Server.Fluent.Config
{
    internal sealed class DomainConfigFacade : NestedFacade<IConfigFacade>, IDomainConfigFacade
    {
        private readonly DomainConfig config;

        public DomainConfigFacade(IConfigFacade parent, string domainName)
            : base(parent)
        {
            this.config = ConfigManager.GetManager().GetDomainConfig(domainName);
        }

        #region Implementation of IDomainConfigFacade

        /// <inheritdoc />
        public IDomainConfigFacade Get<T>(string key, out T value)
        {
            value = this.config.Get<T>(key);
            return this;
        }

        /// <inheritdoc />
        public T Get<T>(string key)
        {
            return this.config.Get<T>(key);
        }

        /// <inheritdoc />
        public IDomainConfigFacade Set<T>(string key, T newValue)
        {
            this.config.Set(key, newValue);
            return this;
        }

        /// <inheritdoc />
        public IDomainConfigFacade GetAll(out DomainConfig config)
        {
            config = this.config;
            return this;
        }

        /// <inheritdoc />
        public DomainConfig GetAll()
        {
            return this.config;
        }

        #endregion
    }
}