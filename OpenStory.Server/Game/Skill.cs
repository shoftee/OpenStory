using System;

namespace OpenStory.Server.Game
{
    public class Skill : ISkill
    {
        private Skill(int id)
        {
            this.Id = id;
        }

        public Element Element { get; private set; }
        public int Level { get; private set; }
        public int RequiredSkill { get; private set; }
        public bool IsAction { get; private set; }

        #region ISkill Members

        public int Id { get; private set; }
        public int AnimationTime { get; private set; }
        public bool IsInvisible { get; private set; }
        public bool IsChargeSkill { get; private set; }

        #endregion

        public static Skill LoadFromData(int skillId)
        {
            // TODO: Hmmm.
            throw new NotImplementedException();
        }
    }
}