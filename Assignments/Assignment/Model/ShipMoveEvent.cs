using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public class ShipMoveEvent : EventArgs
    {
        public int ID { get; private set; }
        public Position Position { get; private set; }

        public ShipMoveEvent(int x, int y, int id)
        {
            Position = new Position(x, y);
            ID = id;
        }
    }
}
