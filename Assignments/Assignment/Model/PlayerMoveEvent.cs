using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public class PlayerMoveEvent : EventArgs
    {
        public Position Position { get; private set; }

        public PlayerMoveEvent(Position pos)
        {
            Position = pos;
        }
    }
}
