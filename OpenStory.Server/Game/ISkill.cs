namespace OpenStory.Server.Game
{
    /// <summary>
    /// Provides properties for game Skills.
    /// </summary>
    public interface ISkill
    {
        /// <summary>
        /// Gets the ID of the skill.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the animation time for the skill.
        /// </summary>
        int AnimationTime { get; }

        /// <summary>
        /// Denotes whether the skill is invisible to other players.
        /// </summary>
        bool IsInvisible { get; }

        /// <summary>
        /// Denotes whether the skill is a Charge-type skill.
        /// </summary>
        bool IsChargeSkill { get; }
    }
}