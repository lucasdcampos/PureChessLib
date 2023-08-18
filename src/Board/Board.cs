using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PureChess
{
    public class Board
    {
        public string defaultPosition = "RNBQKBNR/PPPPPPPP/......../......../......../......../pppppppp/rnbqkbnr - 0"; // Better than FEN? Nah..
        public int columns = 8;
        public int rows = 8;

        public List<Square> squares = new List<Square>();
        public void GenerateBoard(string position)
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Square square = new Square();
                    square.x = x;
                    square.y = y;

                    square.index = (columns + rows) / 2 * x + y;

                    squares.Add(square);

                    Game.settings.DebugMessage($"Generating Square {square.index}");
                }
            }

            Game.settings.DebugMessage("Board sucessfuly generated");
            if (position == null || position == "" || position == " " || position == "default" || position == "startpos") { position = defaultPosition; }
            LoadPosition(position);


        }

        public void LoadPosition(string positionToLoad)
        {
            string legacyPosition = positionToLoad;

            Game.settings.DebugMessage($"Loading Position '{positionToLoad}'");

            if (positionToLoad.EndsWith("1")) { Game.playerTurn = 1; } else { Game.playerTurn= 0; }
            
            positionToLoad = positionToLoad.Replace("/", "");
            positionToLoad = positionToLoad.Replace("-", "");
            positionToLoad = positionToLoad.Replace(" ", "");
            positionToLoad = positionToLoad.Replace("0", "");
            positionToLoad = positionToLoad.Replace("1", "");

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

                if (char.IsUpper(c)) { squares[i].piece.color = 0; }
                else if (char.IsLower(c)) { squares[i].piece.color = 1; }
                else { squares[i].piece.color = 0; }; // No piece at that square (Set to White (0) by default)


                if (squares[i].piece.type != "None")
                {
                    Game.settings.DebugMessage($"Generating {squares[i].piece.GetPieceName()} at Square {squares[i].index}");
                }

                i++;
            }

            Game.settings.DebugMessage("Pieces sucessfuly generated!");
            Game.settings.DebugMessage("§aThe game is ready!");
            DrawCurrentPosition();

            Game.state = GameState.Playing;
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

            string reversedCase = new string(result.ToString().Select(c => char.IsLetter(c) ? char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c) : c).ToArray());

            Game.currentPosition = reversedCase + " - " + Game.playerTurn;
            return Game.currentPosition;

        }

        

        public void DrawCurrentPosition()
        {
            string pos = GetUpdatedPosition();

            pos = pos.Replace("0", "");
            pos = pos.Replace("1", "");
            pos = pos.Replace("-", "");
            pos = pos.Replace(" ", "");

            string[] lines = pos.Split('/');

            //Console.WriteLine($"{Game.currentPosition}\n");

            if (!Game.settings.graphicalBoard) { return; } // Used to display or not the ASCII Board

            Console.WriteLine("  +-----------------+");
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                string line = lines[i];

                for (int j = 0; j < line.Length; j++)
                {
                    char c = TurnCharToSprite(line[j]);

                    if(j == 0) { Console.Write($"{i+1} | "); }
                    
                    if (c == '.')
                    {
                        Console.Write(". ");
                    }
                    else
                    {
                        Console.Write($"{c} ");
                        Console.ResetColor();
                    }

                    if (j == 7) { Console.Write("|"); }
                }

                Console.WriteLine();
            }

            Console.WriteLine("  +-----------------+");
            Console.WriteLine("    a b c d e f g h");

            Console.WriteLine((Game.playerTurn == 0 ? "\nWhite's" : "\nBlack's") + " turn");

        }

        char TurnCharToSprite(char c)
        {
            if (Game.settings.charMode == true) { return c; }

            switch (c)
            {
                case 'p':
                    c = '♙';
                    break;
                case 'P':
                    c = '♟';
                    break;
                case 'n':
                    c = '♘';
                    break;
                case 'N':
                    c = '♞';
                    break;
                case 'b':
                    c = '♗';
                    break;
                case 'B':
                    c = '♝';
                    break;
                case 'q':
                    c = '♕';
                    break;
                case 'Q':
                    c = '♛';
                    break;
                case 'k':
                    c = '♔';
                    break;
                case 'K':
                    c = '♚';
                    break;
                case 'r':
                    c = '♖';
                    break;
                case 'R':
                    c = '♜';
                    break;
            }

            return c;
        }

    }
}