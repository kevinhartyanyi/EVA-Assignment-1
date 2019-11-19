using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public class BombMoveEvent : EventArgs
    {
        public int ID { get; private set; }
        public Position Position { get; private set; }

        public Bomb_Type BombType { get; private set; }

        public BombMoveEvent(int x, int y, int id, Bomb_Type ty)
        {
            Position = new Position(x, y);
            ID = id;
            BombType = ty;
        }
    }
}
