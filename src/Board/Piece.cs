namespace PureChess
{
   public class Piece
    {
        public string type;
        public int color;
        public int value;

        char pieceChar;
        public string GetPieceName()
        {
            string colorString = color == 0 ? "White" : "Black";
            return colorString + " " + type;
        }

        public bool GetPieceType(string inputType)
        {
            return inputType.ToLower() == type.ToLower();
        }

        public void UpdatePieceInfo()
        {
            switch (type)
            {
                case "Pawn":
                    pieceChar = 'P';
                    value = 1;
                    break;
                case "Knight":
                    pieceChar = 'N';
                    value = 3;
                    break;
                case "Bishop":
                    pieceChar = 'B';
                    value = 3;
                    break;
                case "Queen":
                    pieceChar = 'Q';
                    value = 9;
                    break;
                case "King":
                    pieceChar = 'K';
                    value = 0;
                    break;
                case "Rook":
                    pieceChar = 'R';
                    value = 5;
                    break;
                default:
                    pieceChar = '.';
                    value = 0;
                    break;
            }

            if (color == 0) { pieceChar = char.ToLower(pieceChar); } else { pieceChar = char.ToUpper(pieceChar); }
        }

        public char GetPieceSymbol()
        {
            UpdatePieceInfo();
            return pieceChar;

        }
    }
}