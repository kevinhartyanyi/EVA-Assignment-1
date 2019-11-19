using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public class BombCreateEvent : EventArgs
    {
        public int ID { get; private set; }
        public Position Position { get; private set; }
        public Bomb_Type BombType { get; private set; }

        public BombCreateEvent(int id, Position pos, Bomb_Type bType)
        {
            ID = id;
            Position = pos;
            BombType = bType;
        }
    }
}
