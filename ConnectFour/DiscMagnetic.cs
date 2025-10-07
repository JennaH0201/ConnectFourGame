using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    public class DiscMagnetic : Disc
    {
        public DiscMagnetic(char symbol) : base(symbol) { }

        public override void ApplyEffect(Disc[,] grid, int row, int col, GameInventory inventory)
        {
            int player = Symbol == 'M' ? 1 : 2;
            char ordinarySymbol = player == 1 ? '@' : '#';

            // Lift the nearest same-player ordinary disc below
            for (int r = row + 1; r < grid.GetLength(0); r++)
            {
                //error here
                Disc target = grid[r, col];
                if (target != null && target.Symbol == ordinarySymbol)
                {
                    if (r == row + 1) break; // Directly below — no lift

                    grid[r - 1, col] = target; // Lift up
                    grid[r, col] = null;
                    break;
                }
            }

            //Remove magnetic disc from original position
            grid[row, col] = null;

            //Drop transformed disc to bottom of column
            int bottomRow = grid.GetLength(0) - 1;
            while (bottomRow >= 0 && grid[bottomRow, col] != null)
                bottomRow--;

            if (bottomRow >= 0)
                grid[bottomRow, col] = new DiscOrdinary(ordinarySymbol);
        }


    }
}