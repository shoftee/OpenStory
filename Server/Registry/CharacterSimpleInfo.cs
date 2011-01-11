using System;

namespace OpenMaple.Server.Registry
{
    struct CharacterSimpleInfo
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public CharacterSimpleInfo(int id, string name)
            : this()
        {
            if (name == null) throw new ArgumentNullException("name");
            this.Id = id;
            this.Name = name;
        }
    }
}