using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public class LoadGameEvent : EventArgs
    {
        public int MapSize { get; private set; }
        public int PlayerX { get; private set; }
        public int PlayerY { get; private set; }
        
        public LoadGameEvent(int mapSize, int playerX, int playerY)
        {
            MapSize = mapSize;
            PlayerX = playerX;
            PlayerY = playerY;
        }
    }
}
