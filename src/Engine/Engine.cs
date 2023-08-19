using System;
using System.Collections.Generic;

namespace PureChess
{
    public class Engine
    {
        public Evaluation evaluation = new Evaluation();

        public bool[] uciValidations = {false,false,false, false};
        public bool MakeMove(Move move)
        {
            bool isValid = ValidateMove(move);

            if (isValid)
            {
                move.targetSquare.piece.UpdatePiece(move.initialSquare.piece.type, move.initialSquare.piece.color);
                
                move.initialSquare.piece.ResetPiece();

                Game.playerTurn = Game.playerTurn == 0 ? 1 : 0;
                move.AddToMoveList();

                Game.settings.DebugMessage($"§aMove {Game.ConvertToCoordinate(move.initialSquare.index)}{Game.ConvertToCoordinate(move.targetSquare.index)} is valid!");

                Game.board.DrawCurrentPosition();

                return true;
            }

            Game.settings.DebugMessage($"§cMove {Game.ConvertToCoordinate(move.initialSquare.index)}{Game.ConvertToCoordinate(move.targetSquare.index)} is not valid!");
            return false;

        }

        public bool ValidateMove(Move move)
        {
            Square initialSquare = move.initialSquare;
            Square targetSquare = move.targetSquare;
            Piece piece = move.initialSquare.piece;

            int difference = targetSquare.index - initialSquare.index;

            if(initialSquare.piece.color != Game.playerTurn) { return false; }
            if(difference == 0) { return false; }
            if (!targetSquare.piece.GetPieceType("None") && targetSquare.piece.color == Game.playerTurn) { return false; }

            switch (piece.type)
            {
                case "None":
                    return false;
                case "Pawn":
                    return Pawn(move);
                case "King":
                    return King(move);
                case "Rook":
                    return false; 
                case "Knight":
                    return Knight(move); 
                default:
                    return false;
            }
            
        }

        private bool Pawn(Move move)
        {
            Square initial = move.initialSquare;
            Square final = move.targetSquare;
            Piece piece = move.initialSquare.piece;

            int d = final.index - initial.index;
            int i = piece.color == 0 ? 1 : -1;

            // Pawn moving forward
            if (d == 8 * i)
            {
                return final.piece.GetPieceType("None");
            }

            // Pawn moving forward 2x
            if (d == (16 * i) && initial.x == 1 || initial.x == 7)
            {
                return final.piece.GetPieceType("None") && Game.board.squares[final.index - (8*i)].piece.GetPieceType("None");
            }

            if (d == 9 * i || d == 7 * i)
            {
                return !final.piece.GetPieceType("None");
            }

            return false;
        }

        private bool King(Move move)
        {
            Square initial = move.initialSquare;
            Square final = move.targetSquare;
            Piece piece = move.initialSquare.piece;

            int d = final.index - initial.index;

            // Pawn moving forward
            if (d == 8 || d == -8 || d == 1 || d == -1 || d == 7 || d == -7 || d == 9 || d == -9)
            {
                return final.piece.GetPieceType("None") || (!final.piece.GetPieceType("None") && final.piece.color != piece.color);
            }

            return false;
        }

        private bool Knight(Move move)

        {
            int difference = move.targetSquare.index - move.initialSquare.index;

            if(move.targetSquare.x == 7) { return difference == 6 || difference == -6 || difference == -15|| difference == 15; }
            if (move.targetSquare.x == 1) { return difference == 10 || difference == -10 || difference == -17 || difference == 17; }

            return difference == 10 || difference == -10 || difference == 15 || difference == -15 || difference == 17 || difference == -17 || difference == 6 || difference == -6;
        }

        private bool Rook(Move move)
        {
            return SlidingPiece(move);

        }

        private bool SlidingPiece(Move move)
        {
            int difference = move.targetSquare.index - move.initialSquare.index;
            int xDifference = (int)move.targetSquare.x - (int)move.initialSquare.x;
            int yDifference = (int)move.targetSquare.y - (int)move.targetSquare.y;

            foreach (Square currentSquare in Game.board.squares)
            {

                bool betweenY = yDifference > 0 ? currentSquare.y > move.initialSquare.y && currentSquare.y < move.targetSquare.y : currentSquare.y < move.initialSquare.y && currentSquare.y > move.targetSquare.y;
                bool betweenX = xDifference > 0 ? currentSquare.x > move.initialSquare.x && currentSquare.x < move.targetSquare.x : currentSquare.x < move.initialSquare.x && currentSquare.x > move.targetSquare.x;

                bool vertical = move.initialSquare.x == move.targetSquare.x;
                bool horizontal = move.initialSquare.y == move.targetSquare.y;

                bool diagonal = move.initialSquare.x != move.targetSquare.x && move.initialSquare.y != move.targetSquare.y;

                // Moving the Piece vertically
                if (vertical && betweenY)
                {
                    if (!currentSquare.piece.GetPieceType("None") && currentSquare.x == move.initialSquare.x)
                    {
                        Console.WriteLine($"Piece blocking at square {currentSquare.GetCoord()}");
                        return false;
                    }
                }

                //Moving the Piece horizontally
                if (horizontal && betweenX && currentSquare.y == move.initialSquare.y)
                {
                    if (!currentSquare.piece.GetPieceType("None"))
                    {
                        Console.WriteLine($"Piece blocking at square {currentSquare.GetCoord()}");
                        return false;
                    }
                }

                if ((diagonal && betweenX && betweenY))
                {
                    // Moving UP RIGHT
                    if (xDifference > 0 && yDifference > 0 && difference % 9 == 0)
                    {
                        if (!currentSquare.piece.GetPieceType("None"))
                        {
                            if (move.targetSquare.index - currentSquare.index % 9 == 0)
                            {
                                return false;
                            }

                        }
                    }

                    // Moving DOWN RIGHT
                    if (xDifference > 0 && yDifference < 0 && difference % 7 == 0)
                    {
                        if (!currentSquare.piece.GetPieceType("None"))
                        {
                            if (move.targetSquare.index - currentSquare.index % 7 == 0)
                            {
                                return false;
                            }
                        }
                    }

                    // Moving DOWN LEFT
                    if (xDifference < 0 && yDifference < 0 && difference % 9 == 0)
                    {
                        if (!currentSquare.piece.GetPieceType("None"))
                        {
                            if (move.targetSquare.index - currentSquare.index % 9 == 0)
                            {
                                return false;
                            }
                        }
                    }

                    // Moving UP LEFT
                    if (xDifference < 0 && yDifference > 0 && difference % 7 == 0)
                    {
                        if (!currentSquare.piece.GetPieceType("None"))
                        {
                            if (move.targetSquare.index - currentSquare.index % 7 == 0)
                            {
                                return false;
                            }
                        }
                    }
                }

            }

            return true;
        }

        public int depth = 50;
        public int depthCalc = 0;
        public void MakeRandomMoves()
        {
            if (depthCalc >= depth)
                return;

            Move move = new Move();
            Random r1 = new Random();
            Random r2 = new Random();

            move.initialSquare = Game.board.squares[r1.Next(Game.board.squares.Count)];
            move.targetSquare = Game.board.squares[r2.Next(Game.board.squares.Count)];

            if (MakeMove(move)) { depthCalc++; Console.WriteLine(Game.gamePGN); }

            while (!MakeMove(move) && depthCalc < depth)
            {
                MakeRandomMoves();
            }

        }
        

        public void ForceMove(Square initialSquare, Square targetSquare)
        {
            Piece defaultPiece = initialSquare.piece;

            initialSquare.piece = targetSquare.piece;

            targetSquare.piece = defaultPiece;

            Game.settings.DebugMessage($"Making move {defaultPiece.GetPieceSymbol()}{initialSquare.index} - {targetSquare.index}");

            Game.board.DrawCurrentPosition();
        }

        
    }
}
