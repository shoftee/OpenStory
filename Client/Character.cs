using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Client
{
    class Character
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public bool IsGameMaster { get; private set; }

        public Client Client { get; private set; }

        public BuddyList BuddyList { get; private set; }
    }
}
