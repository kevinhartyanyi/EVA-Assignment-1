using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    
    public enum Direction { Left, Right }
    public class Ship
    {
        #region Members

        private Position _pos;
        private Direction _direct;
        private Random _rand;
        private int _mapEnd;
        private int _id;

        public int ID { get { return _id; } }

        public Position Pos
        {
            get { return _pos; }
        }

        public Direction Direction
        {
            get { return _direct; }
            set { _direct = value; }
        }
        #endregion

        public Ship(int x, int y, Direction d, int end, int id)
        {
            _pos = new Position(x, y);
            Direction = d;
            _rand = new Random();
            _mapEnd = end;
            _id = id;
        }

        public void Move(int move)
        {
            if (Direction == Direction.Left)
            {
                if(_pos._x - move < 0)
                {
                    _pos._x += move;
                    Direction = Direction.Right;
                }
                else
                    _pos._x -= move;
            }
            else if(Direction == Direction.Right)
            {
                if (_pos._x + move >= _mapEnd)
                {
                    _pos._x -= move;
                    Direction = Direction.Left;
                }
                else
                    _pos._x += move;
            }
        }                

        public Bomb Drop(int bombID)
        {
            switch (_rand.Next(0, 3))
            {
                case 0:
                    return new Bomb(new Position(Pos._x, Pos._y + 1), Bomb_Type.Light, bombID, _mapEnd);
                case 1:
                    return new Bomb(new Position(Pos._x, Pos._y + 1), Bomb_Type.Medium, bombID, _mapEnd);
                case 2:
                    return new Bomb(new Position(Pos._x, Pos._y + 1), Bomb_Type.Heavy, bombID, _mapEnd);
            }
            return null;
        }

    }
}
