using System;

namespace OpenStory.Server.Registry
{
    internal struct CharacterSimpleInfo
    {
        public CharacterSimpleInfo(int id, string name)
            : this()
        {
            if (name == null) throw new ArgumentNullException("name");
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
    }
}