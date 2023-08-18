namespace PureChess
{
    public class Square
    {
        public int index;
        public int x;
        public int y;

        public Piece piece = new Piece();

        public string GetCoord()
        {
            string possibleX = "abcdefgh";

            char coordX = possibleX[y];

            return coordX + (x+1).ToString();
        }

    }
}
