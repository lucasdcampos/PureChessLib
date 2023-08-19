using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PureChessLib.src.Misc;

namespace PureChess
{
    public static class Game
    {
        public static Board board = new Board();
        public static Engine engine = new Engine();
        public static Settings settings = new Settings();


        public static int playerTurn;
        public static string currentPosition;
        public static string gamePGN;

        public static bool aiFight = true;

        public static List<Move> moves = new List<Move>();

        public static GameState state;
        public static void StartGame(string position)
        {
            board.GenerateBoard(position);
        }

        public static void StopGame()
        {
            board.squares.Clear();
            moves.Clear();
            currentPosition = string.Empty;
            gamePGN = string.Empty;
            playerTurn = 0;
            state = GameState.Waiting;
        }

        public static int ConvertToIndex(char col, char row)
        {
            int colIndex = col - 'a';
            int rowIndex = row - '1';
            return colIndex + rowIndex * 8;
        }

        public static string ConvertToCoordinate(int index)
        {
            int colIndex = index % 8;
            int rowIndex = index / 8;

            char col = (char)('a' + colIndex);
            char row = (char)('1' + rowIndex);

            return $"{col}{row}";
        }

        public static string ConvertFENToSNA(string fen)
        {
            string finalString = string.Empty;
            string withNumbers = string.Empty;
            char p = fen.EndsWith("b") ? '1' : '0';

            fen = fen.Replace(" ", "");
            fen = fen.Replace("w", "");

            fen = fen.Replace("8", "........");
            fen = fen.Replace("7", ".......");
            fen = fen.Replace("6", "......");
            fen = fen.Replace("5", ".....");
            fen = fen.Replace("4", "....");
            fen = fen.Replace("3", "...");
            fen = fen.Replace("2", "..");
            fen = fen.Replace("1", ".");

            foreach (char c in withNumbers)
            {
                char newC = ' ';
                if (char.IsUpper(c)) { newC = char.ToLower(c); }
                if (char.IsLower(c)) { newC = char.ToUpper(c); }

                finalString += newC;
            }

            finalString += $" - {p}";

            return finalString;
        }


    }

    public enum GameState
    {
        Waiting, Playing, GameOver, Initializing
    }
}
