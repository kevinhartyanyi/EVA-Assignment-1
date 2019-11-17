using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    enum Type { Nothing, Ship, LightBomb, MediumBomb, HeavyBomb }
    class Elem
    {
        Position pos;
        int id;
        Type ty;
        public Position Position { get { return pos; } set { pos = value; } }

        public int ID { get { return id; } }

        public Type Type { get { return ty; } }

        public Elem()
        {
            id = -1;
            pos = new Position();
            ty = Type.Nothing;
        }

        public Elem(Position p, int ID, Type typ)
        {
            id = ID;
            pos = p;
            ty = typ;
        }

        public void SetElem(Position p, int ID, Type typ)
        {
            pos = p;
            id = ID;
            ty = typ;
        }

        public void SetPosition(int x, int y)
        {
            pos.SetPosition(x, y);
        }
    }
}
