using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Assignment.Model
{
    class GameControlModel
    {
        private Timer _gameTimer;
        private int _gameTime;
        private int _mapSize;
        private List<Ship> _ships;
        private Random _rand;
        private Player _player;
        private List<int> _shipID;
        private int _bombIDCount;
        private List<Bomb> _bombs;
        private Timer _difficultyTimer;

        public int gameTime { get {return _gameTime; } }
        public bool isPlaying { get { return _gameTimer.Enabled; } }

        #region Events
        public EventHandler<ShipMoveEvent> shipMove;
        public EventHandler<BombMoveEvent> bombMove;
        public EventHandler<GameOverEvent> gameOver;
        public EventHandler<ShipCreateEvent> shipCreate;
        public EventHandler<BombCreateEvent> bombCreate;
        public EventHandler<BombRemoveEvent> bombRemove;

        
        public int FindBomb(List<Bomb> cont, int id) 
        {
            int re = -1;
            for (int i = 0; i < cont.Count; i++)
            {
                if (cont[i].ID == id)
                    re = i;
            }
            return re;
        }

        private void OnShipMoveEvent(Ship ship)
        {
            if (shipMove != null)
                shipMove(this, new ShipMoveEvent(ship.Pos._x, ship.Pos._y, ship.ID));
        }

        private void OnShipCreateEvent(Ship ship)
        {
            if (ship != null)
                shipCreate(this, new ShipCreateEvent(ship.ID, new Position(ship.Pos)));
        }

        private void OnBombStepEvent(object sender, BombStepEvent e)
        {

            if (e.Position._x == _player.Position._x && e.Position._y == _player.Position._y)
            {
                OnGameOverEvent(gameTime);
            }

            int ind = FindBomb(_bombs, e.ID);
            
            if (ind != -1)
            {
                Bomb bomb = _bombs[ind];

                if (bomb.Pos._y >= _mapSize)
                {
                    OnBombRemoveEvent(bomb);
                    _bombs.RemoveAt(ind);
                }
                else
                {
                    OnBombMoveEvent(bomb);
                }
            }

        }

        private void OnBombMoveEvent(Bomb bomb)
        {
            if (bombMove != null)
                bombMove(this, new BombMoveEvent(bomb.Pos._x, bomb.Pos._y, bomb.ID, bomb.bombType));
        }
        private void OnBombCreateEvent(Bomb bomb)
        {
            if (bombMove != null)
                bombCreate(this, new BombCreateEvent(bomb.ID, new Position(bomb.Pos), bomb.bombType));
        }

        private void OnBombRemoveEvent(Bomb bomb)
        {
            if (bomb != null)
                bombRemove(this, new BombRemoveEvent(bomb.ID));
        }

        private void OnGameOverEvent(int currTime)
        {
            if (gameOver != null)
            {
                gameOver(this, new GameOverEvent(currTime));
                StopGame();
            }
        }

        private void OnTimerElapsed(object sender, EventArgs e)
        {
            _difficultyTimer.Interval = _difficultyTimer.Interval - 5;
            Console.WriteLine("Difficulty: " + _difficultyTimer.Interval);
            _gameTime += 1;
            MoveShips(1);
        }

        private void OnDifficultyTimerElapsed(object sender, EventArgs e)
        {
            int ind = _rand.Next(0, _ships.Count);
            Bomb nBomb = _ships[ind].Drop(_bombIDCount);
            _bombs.Add(nBomb);
            _bombIDCount += 1;
            OnBombCreateEvent(nBomb);
            nBomb.bombStep += new EventHandler<BombStepEvent>(OnBombStepEvent);
        }

        #endregion
        public void PlayerMove(Move direct)
        {
            _player.Move(direct);
            CheckCollision();
        }

        private void CheckCollision()
        {
            foreach (var b in _bombs)
            {
                if (b.Pos._x == _player.Position._x && b.Pos._y == _player.Position._y)
                {
                    OnGameOverEvent(_gameTime);
                    //StopGame(); TODO check if this works
                }
            }
        }

        private void MoveShips(int move)
        {
            foreach (var s in _ships)
            {
                s.Move(move);
                OnShipMoveEvent(s);
            }
        }       

       public GameControlModel(int mapSize)
        {
            _gameTimer = new Timer(1000);
            _gameTimer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
            _mapSize = mapSize;
            _ships = new List<Ship>();
            _rand = new Random();
            _shipID = new List<int>();
            _bombs = new List<Bomb>();
            _difficultyTimer = new Timer(1000);
            _difficultyTimer.Elapsed += new ElapsedEventHandler(OnDifficultyTimerElapsed);

    }

        public void StopGame()
        {
            _gameTimer.Enabled = false;
            _difficultyTimer.Enabled = false;
            foreach (var b in _bombs)
            {
                b.Stop();
            }
        }
        public void StartGame()
        {
            if (_gameTimer.Enabled)
                return;
            _gameTimer.Enabled = true;
            _difficultyTimer.Enabled = true;
            foreach (var b in _bombs)
            {
                b.Start();
            }
        }

        public void NewGame(int playerX, int playerY, int shipNumber)
        {
            _bombIDCount = 0;
            _player = new Player(playerX, playerY, _mapSize);
            List<int> shipX = new List<int>();
            for(int i = 0; i < shipNumber; i++)
            {
                int xPos;
                do
                {
                    xPos = _rand.Next(0, _mapSize);
                } while (shipX.Contains(xPos));
                shipX.Add(xPos);
                Direction direct = Direction.Right;
                if (_rand.Next(2) == 0)
                    direct = Direction.Left;
                Ship nShip = new Ship(xPos, 0, direct, _mapSize, i);
                _ships.Add(nShip);
                OnShipCreateEvent(nShip);
            }

            StartGame();
        }

        public void SaveGame()
        {

        }

        public void LoadGame()
        {

        }
    }
}
