namespace OpenMaple.Server.Login
{
    public interface ILoginServer
    {
        IWorld GetWorldById(int worldId);
    }
}