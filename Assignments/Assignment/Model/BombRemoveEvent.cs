using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public class BombRemoveEvent : EventArgs
    {
        public int ID { get; private set; }

        public BombRemoveEvent(int id)
        {
            ID = id;
        }
    }
}
