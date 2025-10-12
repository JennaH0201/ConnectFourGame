using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            // Find the nearest same-player ordinary disc below
            int foundRow = -1;


            for (int r = row + 1; r < grid.GetLength(0); r++)
            {
                Disc target = grid[r, col];
                if (target != null && target.Symbol == ordinarySymbol)
                {
                    foundRow = r;
                    break;
                }
            }


            // If found and not immediately below, swap it up by one position
            if (foundRow != -1 && foundRow > row)
            {
                int liftTo = foundRow - 1;

                // Only lift if the space above is empty
                if (grid[liftTo, col] == null)
                {
                    grid[liftTo, col] = grid[foundRow, col];
                    grid[foundRow, col] = null;
                }
            }
            // If found and immediately below, do nothing

            // Convert the magnetic disc to an ordinary disc at its original position
            grid[row, col] = new DiscOrdinary(ordinarySymbol);

        }

    }
}