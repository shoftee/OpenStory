using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Constants;
using OpenMaple.Game;
using OpenMaple.Server;

namespace OpenMaple.Game
{
    class Character
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        private Character()
        {
        }
    }
}
