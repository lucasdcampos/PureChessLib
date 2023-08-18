using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureChess
{
    internal class Game
    {
        public static Game Instance;

        public Board board = new Board();
        public Engine engine = new Engine();
        public Settings settings = new Settings();


        public int playerTurn;
        public string currentPosition;

        public GameState state;
        public void StartGame(string position)
        {
            board.GenerateBoard(position);
        }

        public void StopGame()
        {
            board.squares.Clear();
            currentPosition = string.Empty;
            playerTurn = 0;
            state = GameState.Waiting;
        }

        public int ConvertToIndex(char col, char row)
        {
            int colIndex = col - 'a';
            int rowIndex = row - '1';
            return colIndex + rowIndex * 8;
        }

        public string ConvertToCoordinate(int index)
        {
            int colIndex = index % 8;
            int rowIndex = index / 8;

            char col = (char)('a' + colIndex);
            char row = (char)('1' + rowIndex);

            return $"{col}{row}";
        }

        public string ConvertFENToSNA(string fen)
        {
            string[] fenParts = fen.Split('/');

            StringBuilder result = new StringBuilder();

            foreach (string fenPart in fenParts)
            {
                if (fenPart.Length == 1 && char.IsDigit(fenPart[0]))
                {
                    int emptyCount = int.Parse(fenPart);
                    result.Append('.', emptyCount);
                }
                else
                {
                    foreach (char c in fenPart)
                    {
                        if (char.IsUpper(c))
                        {
                            result.Append(char.ToLower(c));
                        }
                        else if (char.IsLower(c))
                        {
                            result.Append(char.ToUpper(c));
                        }
                        else
                        {
                            result.Append(c);
                        }
                    }
                }

                result.Append('/');
            }

            result.Remove(result.Length - 1, 1); // Remove the trailing '/'
            return result.ToString();
        }


    }

    public enum GameState
    {
        Waiting, Playing, GameOver, Initializing
    }
}
