namespace OpenStory.AccountService
{
    internal interface ISessionManager
    {
        void UnregisterSession(int accountId);
    }
}