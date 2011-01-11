namespace OpenMaple.Server.Registry
{
    /// <summary>
    /// Provides methods for creation and access of Messenger sessions.
    /// </summary>
    public interface IMessengerRegistry
    {
        IMessenger CreateMessenger(IPlayer initiator);
        IMessenger GetById(int messengerId);
    }
}