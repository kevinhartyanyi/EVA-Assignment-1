using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Assignment.Model;

namespace Assignment.Data
{
    public class Data : IData
    {
        private ModelValues _values;

        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public ModelValues Load(String path)
        {
            _values = new ModelValues();

            int shipNumber;
            int bombNumber;
            using (StreamReader reader = new StreamReader(path))
            {
                _values.gameTime = Int32.Parse(reader.ReadLine());
                _values.gameTimer.Enabled = false;
                _values.difficultyTimer.Interval = Int32.Parse(reader.ReadLine());
                _values.difficultyTimer.Enabled = false;
                _values.mapSize = Int32.Parse(reader.ReadLine());
                _values.playerX = Int32.Parse(reader.ReadLine());
                _values.playerY = Int32.Parse(reader.ReadLine());
                shipNumber = Int32.Parse(reader.ReadLine());
                Console.WriteLine(shipNumber);

                List<Ship> ships = new List<Ship>();

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

                    Ship nShip = new Ship(xPos, yPos, direct, _values.mapSize, id);
                    ships.Add(nShip);
                }
                _values.ships = ships;
                _values.bombID = Int32.Parse(reader.ReadLine());
                bombNumber = Int32.Parse(reader.ReadLine());
                Console.WriteLine(bombNumber);

                List<Bomb> bombs = new List<Bomb>();

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

                    Bomb nBomb = new Bomb(new Position(xPos, yPos), bombType, id, _values.mapSize);
                    bombs.Add(nBomb);
                }

                _values.bombs = bombs;

                return _values;
            }
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public void Save(String path, GameControlModel model)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(model.gameTime);
                writer.WriteLine(model.difficultyTime);
                writer.WriteLine(model.mapSize);
                writer.WriteLine(model.playerX);
                writer.WriteLine(model.playerY);
                writer.WriteLine(model.shipCount);
                
                foreach (var s in model.ships)
                {
                    writer.WriteLine(s.ID);
                    writer.WriteLine(s.Pos._x);
                    writer.WriteLine(s.Pos._y);
                    writer.WriteLine(s.Direction);
                }
                writer.WriteLine(model.bombID);
                writer.WriteLine(model.bombs.Count);
                foreach (var b in model.bombs)
                {
                    writer.WriteLine(b.ID);
                    writer.WriteLine(b.Pos._x);
                    writer.WriteLine(b.Pos._y);
                    writer.WriteLine(b.bombType);
                }

            }
        }
    }
}
