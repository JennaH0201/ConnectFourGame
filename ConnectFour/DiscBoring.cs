using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    public class DiscBoring : Disc
    {
        public DiscBoring(char symbol) : base(symbol) { }

        public override void ApplyEffect(Disc[,] grid, int row, int col, GameInventory inventory)
        {
            // Remove all discs below
            for (int r = row + 1; r < grid.GetLength(0); r++)
            {
                Disc discBelow = grid[r, col];
                if (discBelow != null)
                {
                    string type = Disc.GetDiscTypeFromSymbol(discBelow.Symbol);
                    int player = discBelow.Symbol switch
                    {
                        '@' or 'B' or 'M' => 1,
                        '#' or 'b' or 'm' => 2,
                        _ => 0
                    };

                    inventory.RestoreDisc(player, type);
                    grid[r, col] = null;
                }
            }

            //Remove the boring disc from its original position
            grid[row, col] = null;

            //Drop the boring disc to the bottom of the column
            int bottomRow = grid.GetLength(0) - 1;
            while (bottomRow >= 0 && grid[bottomRow, col] != null)
                bottomRow--;

            if (bottomRow >= 0)
                grid[bottomRow, col] = new DiscOrdinary(Symbol == 'b' ? '#' : '@');
        }





    }
}