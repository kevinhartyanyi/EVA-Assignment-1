using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public class GameOverEvent : EventArgs
    {
        public int gameTime;

        public GameOverEvent(int goTime)
        {
            gameTime = goTime;
        }
    }
}
