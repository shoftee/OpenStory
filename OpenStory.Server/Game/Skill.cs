using System;
using OpenStory.Common;
using OpenStory.Common.Game;

namespace OpenStory.Server.Game
{
    /// <summary>
    /// Represents an in-game ability.
    /// </summary>
    public class Skill : ISkill
    {
        private Skill(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets the elemental attributes of the skill.
        /// </summary>
        public Elements Elements { get; private set; }

        /// <summary>
        /// Gets the level of mastery of the skill.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Gets the ID of a pre-requisite skill if there is one.
        /// </summary>
        public int RequiredSkill { get; private set; }

        /// <summary>
        /// Gets whether the skill is active or passive.
        /// </summary>
        public bool IsAction { get; private set; }

        #region ISkill Members

        /// <summary>
        /// Gets the ID of the skill.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the animation time for the skill.
        /// </summary>
        public int AnimationTime { get; private set; }

        /// <summary>
        /// Denotes whether the skill is invisible to other players.
        /// </summary>
        public bool IsInvisible { get; private set; }

        /// <summary>
        /// Denotes whether the skill is a Charge-type skill.
        /// </summary>
        public bool IsChargeSkill { get; private set; }

        #endregion

        /// <summary>
        /// Loads the skill data for a skill.
        /// </summary>
        /// <param name="skillId">The skill ID to load the data for.</param>
        /// <returns>A <see cref="Skill"/> object with the loaded data.</returns>
        public static Skill LoadFromData(int skillId)
        {
            // TODO: Hmmm.
            throw new NotImplementedException();
        }
    }
}