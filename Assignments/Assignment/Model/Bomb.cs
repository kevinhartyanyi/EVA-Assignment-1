using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Assignment.Model
{
    public enum Bomb_Type { Light, Medium, Heavy }
    public class Bomb
    {
        private Position _pos;
        private Timer bTimer;
        private int _id;
        private int _mapSize;
        public EventHandler<BombStepEvent> bombStep;

        public Bomb_Type bombType;

        public Position Pos
        {
            get { return _pos; }
        }

        public int ID { get { return _id; } }

        private void OnTimerElapsed(object sender, EventArgs e)
        {
            _pos._y += 1;
            if(bombStep != null)
                bombStep(this, new BombStepEvent(ID, new Position(Pos)));
        }

        public Bomb(Position pos, Bomb_Type bType, int id, int mapSize)
        {
            _pos = pos;
            bombType = bType;
            _id = id;
            _mapSize = mapSize;
            switch (bombType)
            {
                case Bomb_Type.Light:
                    bTimer = new Timer(1000);
                    break;
                case Bomb_Type.Medium:
                    bTimer = new Timer(500);
                    break;
                case Bomb_Type.Heavy:
                    bTimer = new Timer(250);
                    break;
            }
            bTimer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
            bTimer.Enabled = true;
        }

        public void Start()
        {
            bTimer.Enabled = true;
        }

        public void Stop()
        {
            bTimer.Enabled = false;
        }

    }
}
