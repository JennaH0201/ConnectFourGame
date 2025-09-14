using System;

namespace ConnectFour
{
    public abstract class Disc
    {
        public string Symbol { get; }

        public Disc(string symbol)
        {
            Symbol = symbol;
        }

        //Return the column where the disc will drop.    
        public abstract int NextDropRow(Board board, int col);

        public bool DropDisc(Board board, int col)
        {
            int row = NextDropRow(board, col);
            if (row >= 0)
            {
                board.PlaceDisc(row, col, Symbol[0]);
                return true;
            }
            return false;
        }
    }

    //Oridnary Disc drops to the lowest column.
    public class OrdinaryDisc : Disc
    {
        public OrdinaryDisc(string symbol) : base(symbol) {}
        public override int NextDropRow(Board board, int col)
        {
            for (int r = 0; r < board.Rows; r++)
            {
                if (board.IsCellEmpty(r, col))
                {
                    return r;
                }
            }
            return -1;
        }
    }

}