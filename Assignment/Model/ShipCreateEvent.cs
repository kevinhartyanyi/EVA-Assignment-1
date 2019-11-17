using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    class ShipCreateEvent : EventArgs
    {
        public int ID { get; private set; }
        public Position Position { get; private set; }

        public ShipCreateEvent(int id, Position pos)
        {
            ID = id;
            Position = pos;
        }
    }
}
