namespace OpenStory.Server.Game
{
    public interface ISkill
    {
        int Id { get; }
        int AnimationTime { get; }
        bool IsInvisible { get; }
        bool IsChargeSkill { get; }
    }
}