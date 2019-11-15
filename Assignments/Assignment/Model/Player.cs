using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public enum Move { Up, Down, Left, Right}
    class Player
    {
        private Position _pos;
        private int _mapSize;

        public Position Position { get { return _pos; } }

        public void Move(Move move)
        {
            switch (move)
            {
                case Model.Move.Up:
                    if((_pos._y -= 1) >= 0)
                        _pos._y -= 1;
                    break;
                case Model.Move.Down:
                    if ((_pos._y += 1) <= _mapSize)
                        _pos._y += 1;
                    break;
                case Model.Move.Left:
                    if ((_pos._x -= 1) >= 0)
                        _pos._x -= 1;
                    break;
                case Model.Move.Right:
                    if ((_pos._x += 1) <= _mapSize)
                        _pos._x += 1;
                    break;
                default:
                    break;
            }
        }

        public Player(int x, int y, int mapSize)
        {
            _pos = new Position(x, y);
            _mapSize = mapSize;
        }

    }
}
