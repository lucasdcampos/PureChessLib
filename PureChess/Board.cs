using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PureChess
{
    internal class Board
    {
        public Game game;
        public string defaultPosition = "RNBQKBNR/PPPPPPPP/......../......../......../......../pppppppp/rnbqkbnr"; // Better than FEN? Nah..
        public int columns = 8;
        public int rows = 8;

        public List<Square> squares = new List<Square>();
        public void GenerateBoard()
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Square square = new Square();
                    square.x = x;
                    square.y = y;

                    square.index = ((columns + rows) / 2) * x + y;

                    squares.Add(square);

                    game.settings.DebugMessage($"Generating Square {square.index}");
                }
            }

            game.settings.DebugMessage("§1Board sucessfuly generated");

            LoadPosition(defaultPosition);

        }

        public void LoadPosition(string positionToLoad)
        {
            string legacyPosition = positionToLoad;

            game.settings.DebugMessage($"Loading Position '{positionToLoad}'");
            positionToLoad = positionToLoad.Replace("/", "");

            int i = 0;
            foreach (char c in positionToLoad)
            {
                switch (c)
                {
                    case 'p' or 'P':
                        squares[i].piece.type = "Pawn";
                        break;
                    case 'r' or 'R':
                        squares[i].piece.type = "Rook";
                        break;
                    case 'n' or 'N':
                        squares[i].piece.type = "Knight";
                        break;
                    case 'b' or 'B':
                        squares[i].piece.type = "Bishop";
                        break;
                    case 'q' or 'Q':
                        squares[i].piece.type = "Queen";
                        break;
                    case 'k' or 'K':
                        squares[i].piece.type = "King";
                        break;
                    case '.':
                        squares[i].piece.type = "None";
                        break;
                    default:
                        break;
                }

                if(char.IsUpper(c)) { squares[i].piece.color = 0; }
                else if(char.IsLower(c)) { squares[i].piece.color = 1; }
                else { squares[i].piece.color = 0; }; // No piece at that square (Set to White (0) by default)

                
                if (squares[i].piece.type != "None")
                {
                    Console.WriteLine($"Generating {squares[i].piece.GetPieceName()} at Square {squares[i].index}");
                }

                i++;
            }

            game.settings.DebugMessage("§1Pieces sucessfuly generated!");
            game.settings.DebugMessage("§aThe game is ready!");
            DrawCurrentPosition();
            
            game.state = GameState.Playing;
        }

        public string GetUpdatedPosition()
        {
            string rawPosition = string.Empty;

            foreach (Square square in squares)
            {
                Piece currentPiece = square.piece;
                
                rawPosition = rawPosition + currentPiece.GetPieceSymbol();

            }

            StringBuilder result = new StringBuilder();
            int i = 0;

            foreach (char c in rawPosition)
            {
                if (i == 8)
                {
                    result.Append("/");
                    i = 0;
                }

                result.Append(c);
                i++;
            }

            string reversedCase = new string(result.ToString().Select(c => char.IsLetter(c) ? (char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c)) : c).ToArray());

            game.currentPosition = reversedCase;
            return game.currentPosition;

        }

        public void DrawCurrentPosition()
        {
            string[] lines = GetUpdatedPosition().Split('/');

            Console.WriteLine("===============");
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                string line = lines[i];

                for (int j = 0; j < line.Length; j++)
                {
                    char c = line[j];

                    if (c == '.')
                    {
                        Console.Write("+ ");
                    }
                    else if (c == ' ')
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.Write($"{c} ");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("===============");

            Console.WriteLine((game.playerTurn == 0 ? "White's" : "Black's") + " turn");
        }

    }
}