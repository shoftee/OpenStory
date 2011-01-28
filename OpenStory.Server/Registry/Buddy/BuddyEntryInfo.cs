using System;

namespace OpenStory.Server.Registry.Buddy
{
    internal struct BuddyEntryInfo
    {
        public BuddyEntryInfo(int id, string name)
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