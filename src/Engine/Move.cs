using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PureChess
{
    public struct Move
    {
        public Square initialSquare;
        public Square targetSquare;

        public void AddToMoveList()
        {
            Game.moves.Add(this);

            string currentMoveString = initialSquare.piece.GetPieceSymbol().ToString();

            currentMoveString += initialSquare.GetCoord();
            currentMoveString += targetSquare.GetCoord();

            // Verify if is a move made by White
            if(initialSquare.piece.color == 0)
            {
                Game.gamePGN += $"{Game.moves.Count}. ";
            }

            Game.gamePGN += currentMoveString + " ";


        }

    }

}
