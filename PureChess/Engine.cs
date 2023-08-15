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
        public void ValidateMove(Move move)
        {
            Piece currentPiece = move.initialSquare.piece;

            if (currentPiece.GetPieceType("Pawn"))
            {

            }
        }

        public void ForceMove(Square initialSquare, Square targetSquare)
        {
            Piece defaultPiece = initialSquare.piece;

            initialSquare.piece = targetSquare.piece;

            targetSquare.piece = defaultPiece;

            game.settings.DebugMessage($"Making move {defaultPiece.GetPieceName()} {initialSquare.index} - {targetSquare.index}");

            game.board.DrawCurrentPosition(game.board.UpdatePosition());
        }
    }
}
