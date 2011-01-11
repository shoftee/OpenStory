using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Game
{
    class Skill : ISkill
    {
        public int Id { get; private set; }
        public Element Element { get; private set; }
        public int Level { get; private set; }
        public int AnimationTime { get; private set; }
        public int RequiredSkill { get; private set; }
        public bool IsAction { get; private set; }
        public bool IsInvisible { get; private set; }
        public bool IsChargeSkill { get; private set; }

        private Skill(int id)
        {
            this.Id = id;
        }

        public static Skill LoadFromData(int skillId)
        {
            // TODO: Hmmm.
            throw new NotImplementedException();
        }
    }
}
