namespace OpenStory.Framework.Contracts
{
    public interface IServerConfigurator
    {
        void ValidateConfiguration(ServiceConfiguration configuration);
    }
}
