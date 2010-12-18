namespace OpenMaple.Client
{
    interface IPlayerExtension
    {
        int PlayerId { get; }
        void Update(Character character);
        void Complete();
    }
}