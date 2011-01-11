using System;

namespace OpenMaple.Game
{
    class KeyBinding : IEquatable<KeyBinding>
    {
        public bool HasChanged { get; set; }
        public byte ActionTypeId { get; private set; }
        public int ActionId { get; private set; }

        public KeyBinding(byte type, int action)
        {
            this.ActionTypeId = type;
            this.ActionId = action;
        }

        public void Change(byte type, int action)
        {
            if (this.ActionTypeId == type && this.ActionId == action) return;

            this.HasChanged = true;
            this.ActionTypeId = type;
            this.ActionId = action;
        }

        public bool Equals(KeyBinding other)
        {
            return this.ActionTypeId == other.ActionTypeId && this.ActionId == other.ActionId;
        }
    }
}