using System.Collections.Generic;

namespace PureChess
{
    public class Engine
    {
        public Evaluation evaluation = new Evaluation();

        public bool[] uciValidations = {false,false,false, false};

        public bool ValidateMove(Move move)
        {
            Square initialSquare = move.initialSquare;
            Square targetSquare = move.targetSquare;
            Piece piece = move.initialSquare.piece;

            int difference = targetSquare.index - initialSquare.index;


            switch (piece.type)
            {
                case "None":
                    return false;
                case "Pawn":
                    return Pawn(move);
                case "King":
                    return King(move); ;
                default:
                    return true;
            }
        }

        private bool Pawn(Move move)
        {
            Square initial = move.initialSquare;
            Square final = move.targetSquare;
            Piece piece = move.initialSquare.piece;

            int d = final.index - initial.index;

            // Pawn moving forward
            if (d == 8 && piece.color == 0 || d == -8 && piece.color == 1)
            {
                return final.piece.GetPieceType("None");
            }

            // Pawn moving forward 2x
            if (d == 16 && piece.color == 0 && initial.x == 1 || d == -16 && piece.color == 1 && initial.x == 6)
            {
                return final.piece.GetPieceType("None");
            }

            if ((d == 9 || d == 8) && piece.color == 0 || (d == -8 || d == -9) && piece.color == 1)
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

        public bool MakeMove(Move move)
        {
            Square initial = move.initialSquare;
            Square final = move.targetSquare;
            Piece piece = move.initialSquare.piece;

            bool isValid = ValidateMove(move);

            if (isValid)
            {
                Piece defaultPiece = initial.piece;

                initial.piece = final.piece;

                final.piece = defaultPiece;
                Game.playerTurn = Game.playerTurn == 0 ? 1 : 0;

                move.AddToMoveList();
                Game.settings.DebugMessage($"§aMove {Game.ConvertToCoordinate(initial.index)}{Game.ConvertToCoordinate(final.index)} is valid!");

                Game.board.DrawCurrentPosition();


                return true;
            }

            Game.settings.DebugMessage($"§cMove {Game.ConvertToCoordinate(initial.index)}{Game.ConvertToCoordinate(final.index)} is not valid!");
            return false;

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
