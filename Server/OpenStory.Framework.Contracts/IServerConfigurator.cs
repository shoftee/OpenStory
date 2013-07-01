namespace OpenStory.Framework.Contracts
{
    public interface IServerConfigurator
    {
        bool CheckConfiguration(ServiceConfiguration configuration, out string error);
    }
}
