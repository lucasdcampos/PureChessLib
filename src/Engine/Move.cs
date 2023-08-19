using System;
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

            string currentMoveString = targetSquare.piece.GetPieceSymbol().ToString().ToUpper();

            currentMoveString += initialSquare.GetCoord();
            currentMoveString += targetSquare.GetCoord();

            // Verify if is a move made by White
            if(Game.playerTurn == 1)
            {
                Game.gamePGN += $"{Math.Ceiling((Decimal)Game.moves.Count/2)}. ";
                
            }

            Game.gamePGN += currentMoveString + " ";

        }


    }

}
