namespace OpenStory.Server.Login
{
    public interface ILoginServer
    {
        IWorld GetWorldById(int worldId);
    }
}