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
            // Bore through: Remove all existing discs from the column (above and below)
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                if (r == row) continue; // Skip the boring disc itself

                Disc disc = grid[r, col];
                if (disc != null)
                {
                    string type = Disc.GetDiscTypeFromSymbol(disc.Symbol);
                    int player = disc.Symbol switch
                    {
                        '@' or 'b' or 'M' or 'E' => 1,
                        '#' or 'B' or 'm' or 'e' => 2,
                        _ => 0
                    };

                    inventory.RestoreDisc(player, type);
                    grid[r, col] = null;
                }
            }

            // Remove the boring disc from its current position
            grid[row, col] = null;

            // Place the boring disc at the bottom (row 0), keeping its symbol
            grid[0, col] = this;
        }





    }
}