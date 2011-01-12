namespace OpenStory.Server.Login
{
    public interface IChannel
    {
        byte Id { get; }
        byte WorldId { get; }
        string Name { get; }

        int ChannelLoad { get; }
    }
}