using OpenStory.Server.Modules.Config;

namespace OpenStory.Server.Fluent.Config
{
    internal sealed class DomainConfigFacade : NestedFacade<IConfigFacade>, IDomainConfigFacade
    {
        private readonly DomainConfig domainConfig;

        public DomainConfigFacade(IConfigFacade parent, string domainName)
            : base(parent)
        {
            this.domainConfig = ConfigManager.GetManager().GetDomainConfig(domainName);
        }

        #region Implementation of IDomainConfigFacade

        /// <inheritdoc />
        public IDomainConfigFacade Get<T>(string key, out T value)
        {
            value = this.domainConfig.Get<T>(key);
            return this;
        }

        /// <inheritdoc />
        public T Get<T>(string key)
        {
            return this.domainConfig.Get<T>(key);
        }

        /// <inheritdoc />
        public IDomainConfigFacade Set<T>(string key, T newValue)
        {
            this.domainConfig.Set(key, newValue);
            return this;
        }

        /// <inheritdoc />
        public IDomainConfigFacade GetAll(out DomainConfig config)
        {
            config = this.domainConfig;
            return this;
        }

        /// <inheritdoc />
        public DomainConfig GetAll()
        {
            return this.domainConfig;
        }

        #endregion
    }
}