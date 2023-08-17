using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureChess
{
    internal class Piece
    {
        public string type;
        public int color;

        public string GetPieceName()
        {
            string colorString = color == 0 ? "White" : "Black";
            return colorString + " " + type;
        }

        public bool GetPieceType(string inputType)
        {
            return inputType.ToLower() == type.ToLower();
        }

        public char GetPieceSymbol()
        {
            char pieceChar;

            switch (type)
            {
                case "Pawn":
                    pieceChar = 'P';
                    break;
                case "Knight":
                    pieceChar = 'N';
                    break;
                case "Bishop":
                    pieceChar = 'B';
                    break;
                case "Queen":
                    pieceChar = 'Q';
                    break;
                case "King":
                    pieceChar = 'K';
                    break;
                case "Rook":
                    pieceChar = 'R';
                    break;
                default:
                    pieceChar = '.';
                    break;
            }

            if (color == 0) { pieceChar = char.ToLower(pieceChar); } else { pieceChar = char.ToUpper(pieceChar); }

            return pieceChar;

        }
    }
}