namespace OpenMaple.Server
{
    interface IPlayerExtension<in TPlayer> 
        where TPlayer : IPlayer
    {
        int PlayerId { get; }
        void Update(TPlayer player);
        void Release();
    }
}