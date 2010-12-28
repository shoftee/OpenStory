namespace OpenMaple.Server
{
    interface IPlayerExtension
    {
        int PlayerId { get; }
        void Update(IPlayer player);
        void Release();
    }
}