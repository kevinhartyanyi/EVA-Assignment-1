using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public class Position
    {
        public int _x;
        public int _y;

        public void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Position(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Position(Position p)
        {
            _x = p._x;
            _y = p._y;
        }

        public Position()
        {
            _x = 0;
            _y = 0;
        }
    }
}
