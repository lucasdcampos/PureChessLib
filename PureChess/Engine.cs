using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureChess
{
    internal class Engine
    {
        public Game game;
        public bool ValidateMove(Square initialSquare, Square targetSquare)
        {
            int difference = targetSquare.index - initialSquare.index;

            Piece currentPiece = initialSquare.piece;

            switch (initialSquare.piece.type)
            {
                case "None":
                    return true;
                case "Pawn":
                    return Pawn(initialSquare, targetSquare, currentPiece);
                default:
                    return true;
            }
        }
        
        private bool Pawn(Square initial, Square final, Piece piece)
        {
            int d = final.index - initial.index;

            // Pawn moving forward
            if((d == 8 && piece.color == 0) || (d == -8 && piece.color == 1))
            {
                return final.piece.GetPieceType("None");
            }

            // Pawn moving forward 2x
            if (((d == 16 && piece.color == 0) && initial.x == 1) || (d == -16 && piece.color == 1) && initial.x == 6)
            {
                return final.piece.GetPieceType("None");
            }

            if (((d == 9 || d == 8) && piece.color == 0) || (d == - 8 || d == -9) && piece.color == 1)
            {
                return !final.piece.GetPieceType("None");
            }

            return false;
        }

        public bool MakeMove(Square initial, Square final)
        {
            bool isValid = ValidateMove(initial, final);

            if(isValid)
            {
                Piece defaultPiece = initial.piece;

                initial.piece = final.piece;

                final.piece = defaultPiece;

                game.board.DrawCurrentPosition();

                game.settings.DebugMessage($"§aMove {initial.index}-{final.index} is valid!");

                return true;
            }

            game.settings.DebugMessage($"§cMove {initial.index}-{final.index} is not valid!");
            return false;

        }

        public void ForceMove(Square initialSquare, Square targetSquare)
        {
            Piece defaultPiece = initialSquare.piece;

            initialSquare.piece = targetSquare.piece;

            targetSquare.piece = defaultPiece;

            game.settings.DebugMessage($"Making move {defaultPiece.GetPieceSymbol()}{initialSquare.index} - {targetSquare.index}");

            game.board.DrawCurrentPosition();
        }
    }
}
