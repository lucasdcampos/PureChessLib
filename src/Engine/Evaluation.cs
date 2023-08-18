using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureChess
{
    public class Evaluation
    {
        public float eval;

        public float GetEval()
        {
            eval = 0f;

            foreach (Square square in Game.board.squares)
            {
                eval += square.piece.color == 0 ? square.piece.value : -square.piece.value;
            }

            return eval;
        }
    }
}
