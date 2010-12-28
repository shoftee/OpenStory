namespace OpenMaple.Game
{
    interface ISkill
    {
        int Id { get; }
        int AnimationTime { get; }
        bool IsInvisible { get; }
        bool IsChargeSkill { get; }
    }
}