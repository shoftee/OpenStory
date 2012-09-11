using System;

namespace OpenStory.Server.Modules
{
    internal struct ComponentKey : IEquatable<ComponentKey>
    {
        public string Name { get; private set; }
        public bool IsRequired { get; private set; }

        public ComponentKey(string name, bool isRequired)
            : this()
        {
            this.Name = name;
            this.IsRequired = isRequired;
        }

        public override bool Equals(object obj)
        {
            if (obj is ComponentKey)
            {
                return this.Equals((ComponentKey)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(ComponentKey other)
        {
            return string.Equals(this.Name, other.Name) && this.IsRequired.Equals(other.IsRequired);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Name != null ? this.Name.GetHashCode() : 0) * 397) ^ this.IsRequired.GetHashCode();
            }
        }
    }
}