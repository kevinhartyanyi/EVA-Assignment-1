using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public enum Move { Up, Down, Left, Right}
    public class Player
    {
        private Position _pos;
        private int _mapSize;

        public Position Position { get { return _pos; } }

        public void Move(Move move)
        {
            Console.WriteLine("Player Position before: " + _pos._x + " " + _pos._y);
            switch (move)
            {
                case Model.Move.Up:
                    if((_pos._y - 1) >= 1)
                        _pos._y -= 1;
                    break;
                case Model.Move.Down:
                    if ((_pos._y + 1) < _mapSize)
                        _pos._y += 1;
                    break;
                case Model.Move.Left:
                    if ((_pos._x - 1) >= 0)
                        _pos._x -= 1;
                    break;
                case Model.Move.Right:
                    if ((_pos._x + 1) < _mapSize)
                        _pos._x += 1;
                    break;
                default:
                    break;
            }
            Console.WriteLine("Player Position after: " + _pos._x + " " + _pos._y);
        }

        public Player(int x, int y, int mapSize)
        {
            _pos = new Position(x, y);
            _mapSize = mapSize;
        }

    }
}
