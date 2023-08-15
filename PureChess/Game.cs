using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureChess
{
    internal class Game
    {
        public Board board;
        public Engine engine = new Engine();
        public Settings settings = new Settings();

        public int playerTurn;
        public string currentPosition;

        public void StartGame()
        {
            board.GenerateBoard();
        }
    }
}
