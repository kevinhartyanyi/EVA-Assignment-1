using System;
using System.Collections.Generic;
using System.IO;
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
        private int _bombIDCount;
        private List<Bomb> _bombs;
        private Timer _difficultyTimer;

        public int gameTime { get {return _gameTime; } }
        public bool isPlaying { get { return _gameTimer.Enabled; } }

        public int mapSize { get { return _mapSize; } }

        public int playerX {  get { return _player.Position._x; } }
        public int playerY { get { return _player.Position._y; } }


        #region Events
        public EventHandler<ShipMoveEvent> shipMove;
        public EventHandler<BombMoveEvent> bombMove;
        public EventHandler<GameOverEvent> gameOver;
        public EventHandler<LoadGameEvent> loadGame;
        public EventHandler<ShipCreateEvent> shipCreate;
        public EventHandler<BombCreateEvent> bombCreate;
        public EventHandler<BombRemoveEvent> bombRemove;
        public EventHandler<PlayerMoveEvent> playerMove;

        
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
                    _bombs.RemoveAt(ind);
                    OnBombRemoveEvent(bomb);
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
                StopGame();
                gameOver(this, new GameOverEvent(currTime));
            }
        }

        private void OnTimerElapsed(object sender, EventArgs e)
        {
            _difficultyTimer.Interval = _difficultyTimer.Interval - 5;
            Console.WriteLine("Difficulty: " + _difficultyTimer.Interval);
            _gameTime += 1;
            MoveShips();
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
            playerMove(this, new PlayerMoveEvent(new Position(_player.Position)));
        }

        private void CheckCollision()
        {
            foreach (var b in _bombs)
            {
                if (b.Pos._x == _player.Position._x && b.Pos._y == _player.Position._y)
                {
                    OnGameOverEvent(_gameTime);
                    return;
                }
            }
        }

        private void MoveShips()
        {
            foreach (var s in _ships)
            {
                int move = _rand.Next(0, 3);
                s.Move(move);
                OnShipMoveEvent(s);
            }
        }       

       public GameControlModel()
        {
            _gameTimer = new Timer(1000);
            _gameTimer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
            _ships = new List<Ship>();
            _rand = new Random();
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

        public void NewGame(int mapSize, int playerX, int playerY, int shipNumber)
        {
            _gameTime = 0;
            _gameTimer.Enabled = false;
            _difficultyTimer.Interval = 1000;
            _difficultyTimer.Enabled = false;
            _mapSize = mapSize;
            if (shipNumber >= mapSize) 
            {
                shipNumber = mapSize - 1;
                Console.WriteLine("Change shipNumber");
            }
            _ships.Clear();
            _bombs.Clear();

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

        public void SaveGame(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(_gameTime);
                writer.WriteLine(_difficultyTimer.Interval);
                writer.WriteLine(_mapSize);
                writer.WriteLine(_player.Position._x);
                writer.WriteLine(_player.Position._y);
                writer.WriteLine(_ships.Count);
                foreach (var s in _ships)
                {
                    writer.WriteLine(s.ID);
                    writer.WriteLine(s.Pos._x);
                    writer.WriteLine(s.Pos._y);
                    writer.WriteLine(s.Direction);
                }
                writer.WriteLine(_bombIDCount);
                writer.WriteLine(_bombs.Count);
                foreach (var b in _bombs)
                {
                    writer.WriteLine(b.ID);
                    writer.WriteLine(b.Pos._x);
                    writer.WriteLine(b.Pos._y);
                    writer.WriteLine(b.bombType);
                }

            }
        }

        public void LoadGame(string fileName)
        {
            _ships.Clear();
            _bombs.Clear();

            int playerX;
            int playerY;
            int shipNumber;
            int bombNumber;
            using (StreamReader reader = new StreamReader(fileName))
            {
                _gameTime = Int32.Parse(reader.ReadLine());
                _gameTimer.Enabled = false;
                _difficultyTimer.Interval = Int32.Parse(reader.ReadLine());
                _difficultyTimer.Enabled = false;
                _mapSize = Int32.Parse(reader.ReadLine());
                playerX = Int32.Parse(reader.ReadLine());
                playerY = Int32.Parse(reader.ReadLine());
                shipNumber = Int32.Parse(reader.ReadLine());
                Console.WriteLine(_gameTime);
                Console.WriteLine(_difficultyTimer.Interval);
                Console.WriteLine(_mapSize);
                if (loadGame != null)
                    loadGame(this, new LoadGameEvent(_mapSize, playerX, playerY));
                Console.WriteLine(playerX);
                Console.WriteLine(playerY);
                Console.WriteLine(shipNumber);

                for (int i = 0; i < shipNumber; i++)
                {
                    int id = Int32.Parse(reader.ReadLine());
                    int xPos = Int32.Parse(reader.ReadLine());
                    int yPos = Int32.Parse(reader.ReadLine());
                    Direction direct;
                    direct = (Direction)Enum.Parse(typeof(Direction), reader.ReadLine());
                    Console.WriteLine(id);
                    Console.WriteLine(xPos);
                    Console.WriteLine(yPos);
                    Console.WriteLine(direct);

                    Ship nShip = new Ship(xPos, yPos, direct, _mapSize, id);
                    _ships.Add(nShip);
                    OnShipCreateEvent(nShip);
                }
                _bombIDCount = Int32.Parse(reader.ReadLine());
                bombNumber = Int32.Parse(reader.ReadLine());
                Console.WriteLine(_bombIDCount);
                Console.WriteLine(bombNumber);


                for (int i = 0; i < bombNumber; i++)
                {
                    int id = Int32.Parse(reader.ReadLine());
                    int xPos = Int32.Parse(reader.ReadLine());
                    int yPos = Int32.Parse(reader.ReadLine());
                    Bomb_Type bombType;
                    bombType = (Bomb_Type)Enum.Parse(typeof(Bomb_Type), reader.ReadLine());
                    Console.WriteLine(id);
                    Console.WriteLine(xPos);
                    Console.WriteLine(yPos);
                    Console.WriteLine(bombType);

                    Bomb nBomb = new Bomb(new Position(xPos, yPos), bombType, id, _mapSize);
                    _bombs.Add(nBomb);
                    nBomb.bombStep += new EventHandler<BombStepEvent>(OnBombStepEvent);
                    OnBombCreateEvent(nBomb);
                }
            }            
            _player = new Player(playerX, playerY, _mapSize);
            
        }
    }
}
