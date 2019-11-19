using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Assignment.Model
{
    public class ModelValues
    {
        private Timer _gameTimer;
        private int _gameTime;
        private int _mapSize;
        private List<Ship> _ships;
        private Player _player;
        private int _bombIDCount;
        private List<Bomb> _bombs;
        private Timer _difficultyTimer;
        public int gameTime { get { return _gameTime; } set { _gameTime = value; } }
        public double difficultyTime { get { return _difficultyTimer.Interval; } set { _difficultyTimer.Interval = value; } }

        public int mapSize { get { return _mapSize; } set { _mapSize = value; } }

        public int playerX { get { return _player.Position._x; } set { _player.Position._x = value; } }
        public int playerY { get { return _player.Position._y; } set { _player.Position._y = value; } }
        public List<Ship> ships { get { return _ships; } set { _ships = value; } }
        public List<Bomb> bombs { get { return _bombs; } set { _bombs = value; } }
        public Timer difficultyTimer { get { return _difficultyTimer; } set { _difficultyTimer = value; } }
        public Timer gameTimer { get { return _gameTimer; } set { _gameTimer = value; } }

        public int bombID { get { return _bombIDCount; } set { _bombIDCount = value; } }

        public ModelValues()
        {
            _gameTimer = new Timer(1000);
            _difficultyTimer = new Timer(1000);
            _player = new Player(0,0,0);
            _ships = new List<Ship>();
            _bombs = new List<Bomb>();
        }


        
    }
}
