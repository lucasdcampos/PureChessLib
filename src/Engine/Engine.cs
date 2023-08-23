using System;
using System.Collections.Generic;

namespace PureChess
{
    public class Engine
    {
        public Evaluation evaluation = new Evaluation();

        public bool[] uciValidations = { false, false, false, false };

        public bool MakeMove(Move move)
        {
            bool isValid = ValidateMove(move);

            if (isValid)
            {
                if (isInCheck())
                {
                    Game.StopGame();
                    Console.WriteLine("Something went wrong. King captured (?). Game aborted.");
                    return false;
                }

                Piece defaultTargetPiece = move.targetSquare.piece.Clone();
                Piece defaultInitialPiece = move.initialSquare.piece.Clone();

                move.targetSquare.piece.UpdatePiece(move.initialSquare.piece.type, move.initialSquare.piece.color, move.initialSquare.piece.value);
                move.initialSquare.piece.ResetPiece();


                Game.playerTurn = Game.playerTurn == 0 ? 1 : 0;

                if (isInCheck())
                {
                    // Undoing last move
                    move.initialSquare.piece = defaultInitialPiece.Clone();
                    move.targetSquare.piece = defaultTargetPiece.Clone();

                    Game.playerTurn = Game.playerTurn == 0 ? 1 : 0;

                    return false;
                }

                move.AddToMoveList();

                Game.board.DrawCurrentPosition();


                return true;
            }
            return false;

        }

        public bool ValidateMove(Move move)
        {
            Square initialSquare = move.initialSquare;
            Square targetSquare = move.targetSquare;
            Piece piece = move.initialSquare.piece;

            int difference = targetSquare.index - initialSquare.index;

            if (initialSquare.piece.color != Game.playerTurn) { return false; }
            if (difference == 0) { return false; }
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
                    return Rook(move);
                case "Knight":
                    return Knight(move);
                case "Queen":
                    return Queen(move);
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
            int dX = final.x - initial.x;
            int dY = final.y - initial.y;

            int i = piece.color == 0 ? 1 : -1;

            // Pawn moving forward
            if (d == 8 * i)
            {
                if(final.x == 0 || final.x == 7) { return false; }
                return final.piece.GetPieceType("None");
            }

            // Pawn moving forward 2x
            if ((dX == (2 * i) && dY == 0) && (initial.x == 1 || initial.x == 6))
            {
                if (final.x == 0 || final.x == 7) { return false; }
                return final.piece.GetPieceType("None") && Game.board.squares[final.index - (8 * i)].piece.GetPieceType("None");
            }

            // Pawn capturing
            if (dX == i)
            {
                if (!final.piece.GetPieceType("None"))
                {
                    if (final.x == 0 || final.x == 7) { return false; }
                    return dY == 1 || dY == -1;
                }

            }

            return false;
        }

        private bool King(Move move)
        {
            Square initial = move.initialSquare;
            Square final = move.targetSquare;

            if (initial.x == final.x)
            {
                return final.y - initial.y == 1 || final.y - initial.y == -1;
            }

            if (initial.y == final.y)
            {
                return final.x - initial.x == 1 || final.x - initial.x == -1;
            }

            if (initial.y == final.y + 1 || initial.y == final.y - 1)
            {
                return final.x - initial.x == 1 || final.x - initial.x == -1;
            }

            if (initial.x == final.x + 1 || initial.x == final.x - 1)
            {
                return final.y - initial.y == 1 || final.y - initial.y == -1;
            }

            return false;
        }

        bool Queen(Move move)
        {
            bool valid = SlidingCross(move) == true || SlidingDiagonal(move) == true;

            return valid;
        }

        private bool Knight(Move move)

        {
            int difference = move.targetSquare.index - move.initialSquare.index;

            if (move.targetSquare.x == move.initialSquare.x + 1 || move.targetSquare.x == move.initialSquare.x - 1)
            {
                return move.targetSquare.y == move.initialSquare.y + 2 || move.targetSquare.y == move.initialSquare.y - 2;
            }

            if (move.targetSquare.y == move.initialSquare.y + 1 || move.targetSquare.y == move.initialSquare.y - 1)
            {
                return move.targetSquare.x == move.initialSquare.x + 2 || move.targetSquare.x == move.initialSquare.x - 2;
            }

            return false;

        }

        private bool Rook(Move move)
        {
            return SlidingCross(move);

        }

        bool SlidingCross(Move move)
        {
            int a = move.targetSquare.index;
            int b = move.initialSquare.index;
            int d = a - b;

            if (move.initialSquare.x == move.targetSquare.x || move.initialSquare.y == move.targetSquare.y)
            {

                foreach (Square square in Game.board.squares)
                {
                    if (d > 0)
                    {
                        if (square.index > move.initialSquare.index && square.index < move.targetSquare.index)
                        {
                            if (!square.piece.GetPieceType("None")) { return false; }
                        }
                    }
                    else if (d < 0)
                    {
                        if (square.index > move.targetSquare.index && square.index < move.initialSquare.index)
                        {
                            if (!square.piece.GetPieceType("None")) { return false; }
                        }
                    }
                }

                return true;
            }
            return false;
        }

        // Terrible code, sorry (PLS FIX!!!!!!!!!)
        bool SlidingDiagonal(Move move)
        {
            Console.WriteLine("a");
            return false;
            // Geting the X and Y pos of the initial and target square
            int x1 = move.initialSquare.x;
            int x2 = move.targetSquare.x;

            int y1 = move.initialSquare.y;
            int y2 = move.targetSquare.y;

            // Checking which direction we are going based in the X and Y
            bool movingUpRight = ((x2 > x1) && (y2 > y1));
            bool movingDownRight = ((x2 > x1) && (y2 < y1));

            bool movingDownLeft = ((x2 < x1) && (y2 < y1));
            bool movingUpLeft = ((x2 < x1) && (y2 > y1));

            foreach (Square square in Game.board.squares)
            {

                // Checking if we are in a valid diagonal
                bool mainDiagonal = (move.targetSquare.index - square.index) % 9 == 0;
                bool secDiagonal = (move.targetSquare.index - square.index) % 7 == 0;

                if(square.index != move.initialSquare.index)
                {
                    if (movingUpRight && mainDiagonal)
                    {
                        Console.WriteLine(square.index);
                        if (!square.piece.GetPieceType("None")) { Console.WriteLine($"Peça bloqueando em {square.GetCoord()}");  return false; }
                    }

                }
                
            }

            return true;
        }

        bool isInCheck()
        {
            Game.board.LookForKings();
            Move move = new Move();

            move.targetSquare = Game.board.kingPos[Game.playerTurn == 0 ? 1 : 0];

            foreach (Square square in Game.board.squares)
            {
                move.initialSquare = square;
                Console.WriteLine($"Validating move: {move.initialSquare.GetCoord()} to {move.targetSquare.GetCoord()}. Status: {ValidateMove(move)}");
                if (ValidateMove(move)) { Console.WriteLine($"Check! {move.initialSquare.GetCoord()} - {move.targetSquare.GetCoord()}"); return true; }
            }

            return false;
        }

        void UndoLastMove()
        {
            Move lastMove = Game.moves[Game.moves.Count - 1];

            Game.moves.Remove(lastMove);
        }


        public int depth = 50;
        public int depthCalc = 0;
        Random r1 = new Random();
        Random r2 = new Random();
        public void MakeRandomMoves()
        {
            if (depthCalc >= depth)
                return;

            Move move = new Move();

            do
            {
                move.initialSquare = Game.board.squares[r1.Next(Game.board.squares.Count)];
                move.targetSquare = Game.board.squares[r2.Next(Game.board.squares.Count)];
            } while (!MakeMove(move));

            depthCalc++;

            if (depthCalc < depth)
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
